using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class MaterialCreationForm : Form
    {
        private readonly ApiClient _apiClient;
        private int? _materialId = null;
        private List<string> _pendingFiles = new List<string>(); // 待上傳的本地路徑
        private List<AttachmentDto> _existingAttachments = new List<AttachmentDto>(); // 已存在於伺服器的附件
        private List<string> _filesToDelete = new List<string>(); // 待從伺服器刪除的檔名
        private Image _pdfIcon;

        public MaterialCreationForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);

            // 從資源檔載入嵌入的 PDF 圖示
            _pdfIcon = ClientResources.PdfIcon;

            this.Load += MaterialCreationForm_Load;
        }

        private int? _targetWarehouseId;
        private string _targetStorageLocation;
        private decimal? _targetQuantity;

        public MaterialCreationForm(int materialId, int? targetWarehouseId = null, string targetStorageLocation = null, decimal? targetQuantity = null) : this()
        {
            _materialId = materialId;
            _targetWarehouseId = targetWarehouseId;
            _targetStorageLocation = targetStorageLocation;
            _targetQuantity = targetQuantity;
            this.Tag = "MaterialEditForm"; // 修改 Tag 以觸發編輯模式標題翻譯
            I18nHelper.Apply(this); // 再次套用，切換標題
        }

        private async void MaterialCreationForm_Load(object sender, EventArgs e)
        {
            await LoadWarehousesAsync();

            if (_materialId.HasValue)
            {
                // 編輯模式：允許校正庫存
                numInitialStock.Enabled = true;
                cmbInitialWarehouse.Enabled = true;

                try
                {
                    var material = await _apiClient.GetMaterialAsync(_materialId.Value);
                    if (material != null)
                    {
                        txtPartNo.Text = material.PartNo;
                        txtName.Text = material.Name;
                        txtSpec.Text = material.Specification;
                        
                        // 優先顯示目標儲位與庫存
                        if (_targetStorageLocation != null)
                        {
                            txtStorageLocation.Text = _targetStorageLocation;
                            // 這裡由於 get material 尚未返回針對特定儲位的數量，我們依賴傳入的儲位，
                            // 其實我們可能還需要 quantity，但為了避免 API 更改，我們可以在 QueryForm 把點選的 quantity 先帶過來，
                            // 但既然 MaterialDto 沒有，我們可以選擇不改 quantity，或要求 api。
                            // 但我們沒有傳入 Quantity，我們至少能把 StorageLocation 顯示正確。
                        }
                        else
                        {
                            txtStorageLocation.Text = material.StorageLocation;
                        }

                        numSafeStock.Value = material.SafeStockQty;
                        numLeadTime.Value = material.LeadTimeDays;
                        numPrice.Value = material.Price;
                        txtPartNo.Enabled = false;
                        btnGenTempPartNo.Enabled = false;

                        // 顯示庫存與倉庫
                        if (_targetQuantity.HasValue)
                        {
                            numInitialStock.Value = _targetQuantity.Value;
                        }
                        else
                        {
                            numInitialStock.Value = material.CurrentStock;
                        }

                        if (_targetWarehouseId.HasValue)
                        {
                            cmbInitialWarehouse.SelectedValue = _targetWarehouseId.Value;
                        }
                        else if (material.WarehouseId.HasValue)
                        {
                            cmbInitialWarehouse.SelectedValue = material.WarehouseId.Value;
                        }

                        // 回顯製造商與號碼
                        txtManufacturer.Text = material.Manufacturer;
                        txtManufacturerNo.Text = material.ManufacturerNo;

                        // 載入附件清單
                        _existingAttachments = await _apiClient.GetAttachmentsAsync(_materialId.Value);
                        RefreshAttachmentPanel();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_LoadMaterialError") + ex.Message,
                        LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private async void btnGenTempPartNo_Click(object sender, EventArgs e)
        {
            try
            {
                btnGenTempPartNo.Enabled = false;
                string newPartNo = await _apiClient.GeneratePartNoAsync();
                txtPartNo.Text = newPartNo;
                
                MessageBox.Show(LocalizationService.GetString("Msg_TempPartNoGenerated"), 
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_GeneratePartNoFailed") + ex.Message, 
                    LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenTempPartNo.Enabled = true;
            }
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                cmbInitialWarehouse.DisplayMember = "WarehouseName";
                cmbInitialWarehouse.ValueMember = "WarehouseID";
                cmbInitialWarehouse.DataSource = warehouses;

                int defaultWhId = GlobalSettings.DefaultWarehouseId;
                cmbInitialWarehouse.SelectedValue = defaultWhId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoadWarehouseError") + ex.Message);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            int currentTotal = _existingAttachments.Count + _pendingFiles.Count;
            if (currentTotal >= 2)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_MaxAttachments"), 
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files (*.pdf)|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Length > 2 * 1024 * 1024)
                    {
                        MessageBox.Show(LocalizationService.GetString("Msg_FileSizeExceeded"),
                            LocalizationService.GetString("Common_Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _pendingFiles.Add(ofd.FileName);
                    RefreshAttachmentPanel();
                }
            }
        }

        private void RefreshAttachmentPanel()
        {
            pnlAttachments.Controls.Clear();
            foreach (var att in _existingAttachments) DisplayAttachment(att.FileName, true);
            foreach (var path in _pendingFiles) DisplayAttachment(Path.GetFileName(path), false, path);
            btnUpload.Enabled = (_existingAttachments.Count + _pendingFiles.Count) < 2;
        }

        private void DisplayAttachment(string fileName, bool isExisting, string localPath = null)
        {
            Panel itemPnl = new Panel { Size = new Size(60, 60), Padding = new Padding(2) };
            PictureBox pb = new PictureBox
            {
                Image = _pdfIcon,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(50, 50),
                Location = new Point(5, 5),
                Cursor = Cursors.Hand
            };
            toolTipAttachment.SetToolTip(pb, fileName);

            pb.Click += async (s, e) => {
                if (isExisting) await OpenRemoteFile(_materialId.Value, fileName);
                else {
                    try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(localPath) { UseShellExecute = true }); }
                    catch (Exception ex) { MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message); }
                }
            };

            Button btnDel = new Button
            {
                Text = "×",
                Size = new Size(18, 18),
                Location = new Point(40, 2),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Click += (s, e) => {
                if (isExisting) {
                    _existingAttachments.RemoveAll(a => a.FileName == fileName);
                    _filesToDelete.Add(fileName);
                } else _pendingFiles.Remove(localPath);
                RefreshAttachmentPanel();
            };

            itemPnl.Controls.Add(btnDel);
            itemPnl.Controls.Add(pb);
            btnDel.BringToFront();
            pnlAttachments.Controls.Add(itemPnl);
        }

        private async System.Threading.Tasks.Task OpenRemoteFile(int materialId, string fileName)
        {
            try
            {
                byte[] data = await _apiClient.DownloadAttachmentAsync(materialId, fileName);
                if (data != null)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), fileName);
                    File.WriteAllBytes(tempPath, data);
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex) { MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message); }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_NameRequired"));
                return;
            }

            if (_materialId.HasValue && string.IsNullOrWhiteSpace(txtPartNo.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PartNoRequired"));
                return;
            }

            btnSave.Enabled = false;

            try
            {
                int finalMaterialId;
                if (_materialId.HasValue)
                {
                    finalMaterialId = _materialId.Value;
                    var dto = new UpdateMaterialDto
                    {
                        PartNo = txtPartNo.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specification = txtSpec.Text.Trim(),
                        StorageLocation = txtStorageLocation.Text.Trim(),
                        SafeStockQty = (int)numSafeStock.Value,
                        LeadTimeDays = (int)numLeadTime.Value,
                        Price = numPrice.Value,
                        Manufacturer = txtManufacturer.Text.Trim(),
                        ManufacturerNo = txtManufacturerNo.Text.Trim(),
                        CurrentStock = numInitialStock.Value,
                        WarehouseId = (int?)cmbInitialWarehouse.SelectedValue,
                        OperatorID = UserSession.Username ?? "SYSTEM",
                        OldStorageLocation = _targetStorageLocation ?? string.Empty
                    };
                    await _apiClient.UpdateMaterialAsync(finalMaterialId, dto);
                }
                else
                {
                    var dto = new CreateMaterialDto
                    {
                        PartNo = txtPartNo.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specification = txtSpec.Text.Trim(),
                        StorageLocation = txtStorageLocation.Text.Trim(),
                        SafeStockQty = (int)numSafeStock.Value,
                        LeadTimeDays = (int)numLeadTime.Value,
                        Price = numPrice.Value,
                        InitialStock = numInitialStock.Value,
                        WarehouseId = (int?)cmbInitialWarehouse.SelectedValue,
                        SourceType = 1,
                        Manufacturer = txtManufacturer.Text.Trim(),
                        ManufacturerNo = txtManufacturerNo.Text.Trim(),
                        OperatorID = UserSession.Username ?? "SYSTEM"
                    };
                    var result = await _apiClient.CreateMaterialAsync(dto);
                    finalMaterialId = result.MaterialID;
                }

                foreach (var fileName in _filesToDelete) await _apiClient.DeleteAttachmentAsync(finalMaterialId, fileName);
                if (_pendingFiles.Count > 0) await _apiClient.UploadAttachmentsAsync(finalMaterialId, _pendingFiles);

                MessageBox.Show(LocalizationService.GetString("Msg_SaveSuccess"));

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SaveError") + ex.Message);
            }
            finally { btnSave.Enabled = true; }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
