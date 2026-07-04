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

        private Label lblCurrentStorageStock;

        public InboundForm()
        {
            InitializeComponent();
            
            // 動態新增庫存顯示標籤
            lblCurrentStorageStock = new Label
            {
                AutoSize = true,
                Font = new Font("Microsoft JhengHei", 12F, FontStyle.Bold),
                ForeColor = Color.Orange,
                Location = new Point(45, cmbStorageLocation.Top), // 原本 combobox 的位置
                Text = "目前庫存: 0"
            };
            pnlLeft.Controls.Add(lblCurrentStorageStock);

            // 將下拉選單與下方按鈕往下推
            cmbStorageLocation.Top += 35;
            btnInbound.Top += 35;

            // 註冊選單變更事件
            cmbStorageLocation.SelectedIndexChanged += CmbStorageLocation_SelectedIndexChanged;

            UIStyle.Apply(this);
            I18nHelper.Apply(this); 
            
            // 將按鈕文字對應到語系
            btnManualInput.Text = LocalizationService.GetString("Menu_Query");
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private void CmbStorageLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStorageLocation.SelectedItem != null)
            {
                dynamic selected = cmbStorageLocation.SelectedItem;
                try
                {
                    decimal qty = selected.Qty;
                    lblCurrentStorageStock.Text = $"目前庫存: {qty:N0}";
                }
                catch
                {
                    lblCurrentStorageStock.Text = "目前庫存: 0";
                }
            }
            else
            {
                lblCurrentStorageStock.Text = "目前庫存: 0";
            }
        }

        private async void InboundForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = LocalizationService.GetString("Status_Ready"); // 使用資源檔
            
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

                    cmbStorageLocation.DataSource = null;
                    cmbStorageLocation.Items.Clear();

                    var dataSource = new List<dynamic>();
                    // 加入現有庫存身上的其他儲位
                    if (info.Stocks != null && info.Stocks.Any())
                    {
                        foreach (var stock in info.Stocks.OrderByDescending(s => s.Quantity))
                        {
                            dataSource.Add(new { Display = string.IsNullOrEmpty(stock.StorageLocation) ? "(未設定儲位) - " + stock.Quantity.ToString("N0") : $"{stock.StorageLocation} - {stock.Quantity:N0}", Value = stock.StorageLocation ?? "", Qty = stock.Quantity });
                        }
                    }
                    if (!string.IsNullOrEmpty(info.StorageLocation) && !dataSource.Any(x => x.Value == info.StorageLocation))
                    {
                        dataSource.Insert(0, new { Display = info.StorageLocation, Value = info.StorageLocation, Qty = 0m });
                    }

                    if (dataSource.Any())
                    {
                        cmbStorageLocation.DataSource = dataSource;
                        cmbStorageLocation.DisplayMember = "Display";
                        cmbStorageLocation.ValueMember = "Value";
                        cmbStorageLocation.SelectedIndex = 0; // 預設為數量最大的 (因為有做 OrderByDescending) 或主檔預設儲位
                    }

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
                    StorageLocation = (cmbStorageLocation.SelectedValue?.ToString() ?? cmbStorageLocation.Text).Trim(),
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
                    cmbStorageLocation.DataSource = null;
                    cmbStorageLocation.Items.Clear();
                    cmbStorageLocation.Text = "";
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
