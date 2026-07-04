using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class InboundForm : Form
    {
        private readonly ApiClient _apiClient;

        public InboundForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); 
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private async void InboundForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = LocalizationService.GetString("Status_Ready"); // 使用資源檔
            btnManualInput.Text = LocalizationService.GetString("Menu_Search") ?? "查詢";
            
            await LoadWarehousesAsync();
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                
                var displayList = warehouses.Select(w => new 
                { 
                    w.WarehouseID, 
                    DisplayName = $"{w.WarehouseCode} - {w.WarehouseName}" 
                }).ToList();

                cmbWarehouse.DataSource = displayList;
                cmbWarehouse.DisplayMember = "DisplayName";
                cmbWarehouse.ValueMember = "WarehouseID";

                cmbWarehouse.SelectedValue = GlobalSettings.DefaultWarehouseId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoadWarehouseError") + ex.Message, 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string barcode = txtBarcode.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(barcode)) return;

                await PerformQuery(barcode);
            }
        }

        private async System.Threading.Tasks.Task PerformQuery(string barcode)
        {
            lblStatus.Text = LocalizationService.GetString("Status_InboundSearching");
            lblStatus.ForeColor = Color.Cyan;
            
            try
            {
                var info = await _apiClient.GetInventoryAsync(barcode);
                if (info != null)
                {
                    lblMaterialName.Text = info.Name;
                    lblSpecification.Text = LocalizationService.GetString("Label_PartNoPrefix") + info.PartNo;
                    
                    lblStatus.Text = LocalizationService.GetString("Status_IdentifySuccess");
                    lblStatus.ForeColor = Color.Lime;
                    
                    txtBarcode.Text = barcode;

                    // 載入該物料現有的所有儲位
                    cmbStorageLocation.Items.Clear();
                    var locations = new List<string>();
                    
                    // 加入主檔預設儲位
                    if (!string.IsNullOrEmpty(info.StorageLocation))
                        locations.Add(info.StorageLocation);
                        
                    // 加入現有庫存身上的其他儲位
                    if (info.Stocks != null)
                    {
                        locations.AddRange(info.Stocks.Select(s => s.StorageLocation).Where(l => !string.IsNullOrEmpty(l)));
                    }

                    // 去除重複並綁定
                    var distinctLocations = locations.Distinct().ToArray();
                    cmbStorageLocation.Items.AddRange(distinctLocations);

                    // 預設帶出該物料的主儲位或第一個儲位
                    if (!string.IsNullOrEmpty(info.StorageLocation))
                        cmbStorageLocation.Text = info.StorageLocation;
                    else if (cmbStorageLocation.Items.Count > 0)
                        cmbStorageLocation.SelectedIndex = 0;

                    txtQty.Focus();
                    txtQty.SelectAll();
                }
            }
            catch (Exception)
            {
                Console.Beep();
                lblStatus.Text = LocalizationService.GetString("Status_NotFound");
                lblStatus.ForeColor = Color.Red;
                lblMaterialName.Text = "--";
                lblSpecification.Text = "--";
            }
        }

        private async void btnManualInput_Click(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(barcode)) return;

            await PerformQuery(barcode);
        }

        private async void btnInbound_Click(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(barcode))
            {
                MessageBox.Show(LocalizationService.GetString("Label_ScanBarcode"));
                return;
            }

            if (!decimal.TryParse(txtQty.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputCorrectQty"));
                return;
            }

            if (cmbWarehouse.SelectedValue == null) return;
            
            int targetWarehouseId = (int)cmbWarehouse.SelectedValue;

            btnInbound.Enabled = false;
            lblStatus.Text = LocalizationService.GetString("Status_InboundProcessing") ?? "Processing...";

            try
            {
                var dto = new InboundDto
                {
                    WarehouseId = targetWarehouseId,
                    Barcode = barcode,
                    StorageLocation = cmbStorageLocation.Text.Trim(),
                    Quantity = qty,
                    OperatorId = UserSession.Username
                };

                bool success = await _apiClient.PostInboundAsync(dto);
                if (success)
                {
                    lblStatus.Text = string.Format(LocalizationService.GetString("Status_InboundSuccess"), barcode);
                    lblStatus.ForeColor = Color.Lime;
                    
                    txtBarcode.Clear();
                    txtQty.Text = "1";
                    cmbStorageLocation.Text = "";
                    cmbStorageLocation.Items.Clear();
                    lblMaterialName.Text = "--";
                    lblSpecification.Text = "--";
                    txtBarcode.Focus();
                }
            }
            catch (Exception ex)
            {
                string failedMsg = LocalizationService.GetString("Status_InboundFailed") ?? "Failed: {0}";
                lblStatus.Text = string.Format(failedMsg, ex.Message);
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnInbound.Enabled = true;
            }
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form() 
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label textLabel = new Label() { Left = 30, Top = 20, Text = text, AutoSize = true, Font = new Font("Microsoft JhengHei", 10) };
            TextBox textBox = new TextBox() { Left = 30, Top = 50, Width = 320, Font = new Font("Consolas", 12) };
            Button confirmation = new Button() { Text = LocalizationService.GetString("Btn_Confirm"), Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK, Font = new Font("Microsoft JhengHei", 10) };
            
            confirmation.Click += (sender, e) => { prompt.Close(); };
            
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
