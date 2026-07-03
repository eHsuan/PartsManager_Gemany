using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class CreateMaterialDto
    {
        public string PartNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Specification { get; set; } = "None";

        public string StorageLocation { get; set; } = string.Empty;

        public int SafeStockQty { get; set; }

        public int LeadTimeDays { get; set; }

        public decimal Price { get; set; }

        public decimal InitialStock { get; set; }
        
        public int? WarehouseId { get; set; }

        public string OperatorID { get; set; } = "SYSTEM";
        
        public string Manufacturer { get; set; }
        public string ManufacturerNo { get; set; }

        public byte SourceType { get; set; } = 1; // 預設 1: Line-Side Purchased
    }
}
