using PartsManager.Api.Data;
using PartsManager.Api.Entities;
using PartsManager.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartsManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MasterDataController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly string _storagePath;

    public MasterDataController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _storagePath = configuration["System:AttachmentPath"] ?? "Attachments";
        if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
    }

    [HttpPost("materials/{id}/attachments")]
    public async Task<IActionResult> UploadAttachments(int id, IFormFileCollection files)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound("Material not found");

        var currentCount = await _context.Mdm_MaterialAttachments.CountAsync(a => a.MaterialID == id);
        if (currentCount + files.Count > 2)
        {
            return BadRequest("Maximum 2 attachments allowed per material.");
        }

        string materialFolder = Path.Combine(_storagePath, id.ToString());
        if (!Directory.Exists(materialFolder)) Directory.CreateDirectory(materialFolder);

        foreach (var file in files)
        {
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf") continue;

            string physicalPath = Path.Combine(materialFolder, file.FileName);
            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string relativePath = Path.Combine(id.ToString(), file.FileName);

            var attachment = new Mdm_MaterialAttachments
            {
                MaterialID = id,
                FileName = file.FileName,
                FilePath = relativePath, 
                UploadTime = DateTime.Now
            };
            _context.Mdm_MaterialAttachments.Add(attachment);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("materials/{id}/attachments")]
    public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAttachments(int id)
    {
        return await _context.Mdm_MaterialAttachments
            .Where(a => a.MaterialID == id)
            .Select(a => new AttachmentDto
            {
                ID = a.ID,
                MaterialID = a.MaterialID,
                FileName = a.FileName,
                UploadTime = a.UploadTime
            }).ToListAsync();
    }

    [HttpGet("materials/{id}/attachments/{fileName}/download")]
    public async Task<IActionResult> DownloadAttachment(int id, string fileName)
    {
        var attachment = await _context.Mdm_MaterialAttachments
            .FirstOrDefaultAsync(a => a.MaterialID == id && a.FileName == fileName);
        
        if (attachment == null) return NotFound();

        string fullPath = Path.Combine(_storagePath, attachment.FilePath);
        
        if (!System.IO.File.Exists(fullPath))
            return NotFound();

        var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
        return File(bytes, "application/pdf", attachment.FileName);
    }

    [HttpDelete("materials/{id}/attachments/{fileName}")]
    public async Task<IActionResult> DeleteAttachment(int id, string fileName)
    {
        var attachment = await _context.Mdm_MaterialAttachments
            .FirstOrDefaultAsync(a => a.MaterialID == id && a.FileName == fileName);

        if (attachment != null)
        {
            string fullPath = Path.Combine(_storagePath, attachment.FilePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _context.Mdm_MaterialAttachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }
        return NoContent();
    }

    [HttpPost("materials")]
    public async Task<ActionResult<MaterialDto>> CreateMaterial(CreateMaterialDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PartNo))
        {
            dto.PartNo = await GenerateUniquePartNoAsync();
        }

        if (await _context.Mdm_Materials.AnyAsync(m => m.PartNo == dto.PartNo))
        {
            return Conflict(new { message = $"料號 {dto.PartNo} 已存在" });
        }

        var material = new Mdm_Materials
        {
            PartNo = dto.PartNo,
            Name = dto.Name,
            Specification = dto.Specification,
            StorageLocation = dto.StorageLocation,
            SafeStockQty = dto.SafeStockQty,
            LeadTimeDays = dto.LeadTimeDays,
            Price = dto.Price,
            SourceType = dto.SourceType,
            BarCode = dto.PartNo.ToLower(),
            NeedsPrintLabel = true,
            Manufacturer = dto.Manufacturer ?? string.Empty,
            ManufacturerNo = dto.ManufacturerNo ?? string.Empty
        };

        _context.Mdm_Materials.Add(material);
        await _context.SaveChangesAsync();

        if (dto.InitialStock > 0)
        {
            int targetWhId = dto.WarehouseId ?? 1;
            
            var stock = new Inv_CurrentStock
            {
                MaterialID = material.MaterialID,
                WarehouseID = targetWhId,
                Quantity = dto.InitialStock,
                LastUpdated = DateTime.Now
            };
            _context.Inv_CurrentStock.Add(stock);

            var transaction = new Inv_Transactions
            {
                MaterialID = material.MaterialID,
                WarehouseID = targetWhId,
                TransType = "IN",
                ChangeQty = dto.InitialStock,
                AfterQty = dto.InitialStock,
                TransTime = DateTime.Now,
                OperatorID = "SYSTEM_INIT",
                ReasonCode = "Initial Import"
            };
            _context.Inv_Transactions.Add(transaction);

            await _context.SaveChangesAsync();
        }

        var result = new MaterialDto
        {
            MaterialID = material.MaterialID,
            BarCode = material.BarCode,
            PartNo = material.PartNo,
            Name = material.Name,
            Specification = material.Specification,
            StorageLocation = material.StorageLocation,
            SafeStockQty = material.SafeStockQty,
            Manufacturer = material.Manufacturer,
            ManufacturerNo = material.ManufacturerNo
        };

        return StatusCode(201, result);
    }

    [HttpPut("materials/{id}")]
    public async Task<IActionResult> UpdateMaterial(int id, UpdateMaterialDto dto)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound();

        if (material.PartNo != dto.PartNo)
        {
            if (await _context.Mdm_Materials.AnyAsync(m => m.PartNo == dto.PartNo))
            {
                return Conflict(new { message = $"料號 {dto.PartNo} 已存在" });
            }
        }

        material.PartNo = dto.PartNo;
        material.Name = dto.Name;
        material.Specification = dto.Specification;
        material.StorageLocation = dto.StorageLocation;
        material.SafeStockQty = dto.SafeStockQty;
        material.LeadTimeDays = dto.LeadTimeDays;
        material.Price = dto.Price;
        material.BarCode = dto.PartNo.ToLower();
        material.Manufacturer = dto.Manufacturer ?? string.Empty;
        material.ManufacturerNo = dto.ManufacturerNo ?? string.Empty;

        int targetWhId = dto.WarehouseId ?? 1;
        var stock = await _context.Inv_CurrentStock.FirstOrDefaultAsync(s => s.MaterialID == id && s.WarehouseID == targetWhId);
        decimal oldQty = stock?.Quantity ?? 0;

        if (dto.CurrentStock != oldQty)
        {
            if (stock == null)
            {
                stock = new Inv_CurrentStock
                {
                    MaterialID = id,
                    WarehouseID = targetWhId,
                    Quantity = dto.CurrentStock,
                    LastUpdated = DateTime.Now
                };
                _context.Inv_CurrentStock.Add(stock);
            }
            else
            {
                stock.Quantity = dto.CurrentStock;
                stock.LastUpdated = DateTime.Now;
            }

            _context.Inv_Transactions.Add(new Inv_Transactions
            {
                MaterialID = id,
                WarehouseID = targetWhId,
                TransType = "ADJ",
                ChangeQty = dto.CurrentStock - oldQty,
                AfterQty = dto.CurrentStock,
                TransTime = DateTime.Now,
                OperatorID = dto.OperatorID ?? "SYSTEM_ADJ",
                ReasonCode = "Manual Adjustment"
            });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("materials/{id}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound();

        _context.Mdm_Materials.Remove(material);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("materials/{id}")]
    public async Task<ActionResult<MaterialDto>> GetMaterial(int id)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound();

        var stocks = await _context.Inv_CurrentStock
            .Where(s => s.MaterialID == id)
            .ToListAsync();
        
        decimal totalStock = stocks.Sum(s => s.Quantity);
        int? whId = stocks.FirstOrDefault()?.WarehouseID ?? 1;

        return new MaterialDto
        {
            MaterialID = material.MaterialID,
            BarCode = material.BarCode ?? "",
            PartNo = material.PartNo,
            Name = material.Name,
            Specification = material.Specification ?? "",
            StorageLocation = material.StorageLocation ?? "",
            SafeStockQty = material.SafeStockQty,
            LeadTimeDays = material.LeadTimeDays,
            Price = material.Price,
            Manufacturer = material.Manufacturer,
            ManufacturerNo = material.ManufacturerNo,
            CurrentStock = totalStock,
            WarehouseId = whId
        };
    }

    [HttpGet("machines")]
    public async Task<ActionResult<IEnumerable<Mdm_Machines>>> GetMachines()
    {
        return await _context.Mdm_Machines.ToListAsync();
    }

    [HttpGet("warehouses")]
    public async Task<ActionResult<IEnumerable<Mdm_Warehouses>>> GetWarehouses()
    {
        return await _context.Mdm_Warehouses.ToListAsync();
    }

    [HttpGet("materials/generate-partno")]
    public async Task<ActionResult<object>> GeneratePartNo()
    {
        string partNo = await GenerateUniquePartNoAsync();
        return Ok(new { PartNo = partNo });
    }

    private async Task<string> GenerateUniquePartNoAsync()
    {
        string partNo;
        var random = new Random();
        const string chars = "0123456789";
        do
        {
            partNo = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
        } while (await _context.Mdm_Materials.AnyAsync(m => m.PartNo == partNo));
        return partNo;
    }
}
