using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class MainForm : Form, IMessageFilter
    {
        private readonly ApiClient _apiClient;
        private MaterialStockInfoDto _currentMaterial;
        private Timer _idleTimer;
        private DateTime _lastActivity;
        private int _timeoutMinutes;
        private Panel _navPanel;
        private ComboBox cmbStorageLocation;

        public class WarehouseViewModel
        {
            public int Id { get; set; }
            public string Display { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
            
            // 初始化導航列容器
            _navPanel = new Panel();
            _navPanel.Dock = DockStyle.Top;
            _navPanel.Height = 45; // 稍微增加高度
            _navPanel.Padding = new Padding(5);
            _navPanel.BackColor = Color.FromArgb(45, 45, 48);
            this.Controls.Add(_navPanel);

            RefreshUI(); // 初始載入語系與選單
            
            this.Shown += MainForm_Shown;
            this.Load += MainForm_Load;
            
            cmbWarehouse.SelectedIndexChanged += (s, e) =>
            {
                 ResetInfo();
                 txtBarcode.Focus();
                 txtBarcode.SelectAll();
            };

            InitializeIdleTimer();
        }

        /// <summary>
        /// 重新整理整個 UI 的語系 (包含動態選單)
        /// </summary>
        public void RefreshUI()
        {
            I18nHelper.Apply(this);
            BuildNavigation();
            string version = typeof(MainForm).Assembly.GetName().Version?.ToString(3) ?? "1.1.0";
            this.Text = $"{LocalizationService.GetString("App_Title")} v{version} - {UserSession.Username}";
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await CheckServerStatusAsync();
            var timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += async (s, ev) => await CheckServerStatusAsync();
            timer.Start();

            // 動態加入 cmbStorageLocation 替換 lblStorageLocation
            cmbStorageLocation = new ComboBox
            {
                Location = lblStorageLocation.Location,
                Size = new Size(250, 30),
                Font = lblStorageLocation.Font,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmbStorageLocation.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbStorageLocation.SelectedItem != null)
                {
                    dynamic selected = cmbStorageLocation.SelectedItem;
                    lblCurrentStock.Text = selected.Qty.ToString("N0");
                }
            };
            lblStorageLocation.Parent.Controls.Add(cmbStorageLocation);
            lblStorageLocation.Visible = false;
        }

        private async System.Threading.Tasks.Task CheckServerStatusAsync()
        {
            try
            {
                await _apiClient.GetWarehousesAsync();
                pnlServerStatus.BackColor = UIStyle.StatusOkColor;
                lblServerStatus.Text = LocalizationService.GetString("Msg_ServerOnline");
                lblServerStatus.ForeColor = Color.Lime;
            }
            catch
            {
                pnlServerStatus.BackColor = UIStyle.StatusErrorColor;
                lblServerStatus.Text = LocalizationService.GetString("Msg_ServerOffline");
                lblServerStatus.ForeColor = UIStyle.StatusErrorColor;
            }
        }

        private void InitializeIdleTimer()
        {
            _lastActivity = DateTime.Now;
            _timeoutMinutes = GlobalSettings.AutoLogoutMinutes;

            _idleTimer = new Timer();
            _idleTimer.Interval = 5000;
            _idleTimer.Tick += (s, e) =>
            {
                if ((DateTime.Now - _lastActivity).TotalMinutes >= _timeoutMinutes)
                {
                    _idleTimer.Stop();
                    Application.RemoveMessageFilter(this);
                    MessageBox.Show(LocalizationService.GetString("Msg_AutoLogout") ?? "Idle timeout, logging out.", "Auto Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            };
            _idleTimer.Start();
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0200 || m.Msg == 0x0201 || m.Msg == 0x0202 || m.Msg == 0x0100 || m.Msg == 0x0101)
            {
                _lastActivity = DateTime.Now;
            }
            return false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _idleTimer?.Stop();
            Application.RemoveMessageFilter(this);
            base.OnFormClosing(e);
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            await LoadWarehousesAsync();
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                
                var displayList = warehouses.Select(w => new WarehouseViewModel
                { 
                    Id = w.WarehouseID, 
                    Display = $"{w.WarehouseCode} - {w.WarehouseName}" 
                }).ToList();

                cmbWarehouse.DisplayMember = "Display";
                cmbWarehouse.ValueMember = "Id";
                cmbWarehouse.DataSource = displayList;

                int defaultId = GlobalSettings.DefaultWarehouseId;
                cmbWarehouse.SelectedValue = defaultId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoadWarehouseError") + ex.Message);
            }
        }

        private void BuildNavigation()
        {
            _navPanel.Controls.Clear();

            // --- 建立功能按鈕 ---

            var btnInbound = CreateNavButton(LocalizationService.GetString("Menu_Inbound"), "Menu_Inbound");
            btnInbound.Visible = UserSession.UserLevel <= 3;
            btnInbound.Click += (s, e) => {
                var form = new InboundForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnQuery = CreateNavButton(LocalizationService.GetString("Menu_Query"), "Menu_Query");
            btnQuery.Visible = UserSession.UserLevel <= 4;
            btnQuery.Click += (s, e) => {
                var form = new QueryForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnTransHistory = CreateNavButton(LocalizationService.GetString("Menu_TransHistory"), "Menu_TransHistory");
            btnTransHistory.Visible = UserSession.UserLevel <= 3;
            btnTransHistory.Click += (s, e) => {
                var form = new TransactionHistoryForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnLowStock = CreateNavButton(LocalizationService.GetString("Menu_LowStock"), "Menu_LowStock");
            btnLowStock.Visible = UserSession.UserLevel <= 3;
            btnLowStock.Click += (s, e) => {
                var form = new LowStockAlertView(_apiClient);
                form.Show();
            };

            var btnInventory = CreateNavButton(LocalizationService.GetString("Menu_Inventory"), "Menu_Inventory");
            btnInventory.Visible = UserSession.UserLevel <= 3;
            btnInventory.Click += (s, e) => {
                var form = new InventoryForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnCreateMaterial = CreateNavButton(LocalizationService.GetString("Menu_CreateMaterial"), "Menu_CreateMaterial");
            btnCreateMaterial.Visible = UserSession.UserLevel <= 2;
            btnCreateMaterial.Click += (s, e) => {
                var form = new MaterialCreationForm();
                form.ShowDialog();
            };

            var btnBatchImport = CreateNavButton(LocalizationService.GetString("Menu_BatchImport"), "Menu_BatchImport");
            btnBatchImport.Visible = UserSession.UserLevel <= 2;
            btnBatchImport.Click += (s, e) => {
                var form = new BatchImportForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnUserMgmt = CreateNavButton(LocalizationService.GetString("Menu_UserMgmt"), "Menu_UserMgmt");
            btnUserMgmt.Visible = UserSession.UserLevel == 1;
            btnUserMgmt.Click += (s, e) => {
                var form = new UserManagementForm();
                form.ShowDialog();
            };

            // --- 系統設定按鈕 (點擊顯示 ContextMenu) ---
            var btnSettings = CreateNavButton(LocalizationService.GetString("Menu_Settings"), "Menu_Settings");
            btnSettings.Dock = DockStyle.Right; 
            btnSettings.Width = 160; // 德文較長
            
            var ctxSettings = new ContextMenuStrip();
            
            // 變更密碼選項
            var itemChangePassword = new ToolStripMenuItem(LocalizationService.GetString("Menu_ChangePassword"));
            itemChangePassword.Tag = "Menu_ChangePassword";
            itemChangePassword.Click += (s, e) => {
                new ChangePasswordForm().ShowDialog();
            };
            ctxSettings.Items.Add(itemChangePassword);

            // 機台管理選項 (僅管理員)
            if (UserSession.UserLevel == 1)
            {
                var itemMachineMgmt = new ToolStripMenuItem(LocalizationService.GetString("Menu_MachineMgmt"));
                itemMachineMgmt.Tag = "Menu_MachineMgmt";
                itemMachineMgmt.Click += (s, e) => {
                    new MachineManagementForm().ShowDialog();
                };
                ctxSettings.Items.Add(itemMachineMgmt);

                var itemBackupRestore = new ToolStripMenuItem(LocalizationService.GetString("Menu_BackupRestore"));
                itemBackupRestore.Tag = "Menu_BackupRestore";
                itemBackupRestore.Click += (s, e) => {
                    new BackupRestoreForm().ShowDialog();
                };
                ctxSettings.Items.Add(itemBackupRestore);
            }

            // 分隔線
            // ctxSettings.Items.Add(new ToolStripSeparator());

            // 語系切換範例：如果是從這裡切換，應呼叫 RefreshUI()
            // 假設您在某處有切換語系的邏輯，請確保它會呼叫 mainForm.RefreshUI()

            btnSettings.Click += (s, e) => {
                ctxSettings.Show(btnSettings, new Point(0, btnSettings.Height));
            };

            // --- 加入 Panel (順序：由右向左加入 DockLeft) ---
            // 注意：WinForms Dock.Left 加入順序會影響由左至右的排列

            _navPanel.Controls.Add(btnInbound);
            _navPanel.Controls.Add(btnQuery);
            _navPanel.Controls.Add(btnTransHistory);
            _navPanel.Controls.Add(btnLowStock);
            _navPanel.Controls.Add(btnInventory);
            _navPanel.Controls.Add(btnCreateMaterial);
            _navPanel.Controls.Add(btnBatchImport);
            _navPanel.Controls.Add(btnUserMgmt);
            
            _navPanel.Controls.Add(btnSettings);
        }

        private Button CreateNavButton(string text, string tag)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Tag = tag; // 賦予 Tag 以便之後可能的手動刷新
            btn.Dock = DockStyle.Left;
            btn.Width = 135; // 增加寬度以適應德文翻譯
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Microsoft JhengHei", 10F, FontStyle.Bold);
            btn.BackColor = Color.Transparent;
            
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(63, 63, 65);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.Transparent;

            return btn;
        }

        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SearchMaterialAsync();
            }
        }

        private async System.Threading.Tasks.Task SearchMaterialAsync()
        {
            string barcode = txtBarcode.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(barcode)) return;

            try
            {
                lblStatus.Text = LocalizationService.GetString("Status_Searching");
                _currentMaterial = await _apiClient.GetInventoryAsync(barcode);
                DisplayMaterial(_currentMaterial);
                lblStatus.Text = LocalizationService.GetString("Status_Ready");
                lblStatus.ForeColor = Color.Cyan;
                txtQty.Focus();
                txtQty.SelectAll();
            }
            catch (Exception)
            {
                ResetInfo();
                lblStatus.Text = LocalizationService.GetString("Status_NotFound");
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void DisplayMaterial(MaterialStockInfoDto material)
        {
            lblMaterialName.Text = material.Name;
            lblSpecification.Text = material.Specification ?? "--";
            
            int warehouseId = (int)(cmbWarehouse.SelectedValue ?? 0);
            var stocks = material.Stocks?.Where(s => s.WarehouseId == warehouseId).ToList() ?? new List<StockDetailDto>();
            
            var dataSource = stocks.Select(s => new {
                Display = string.IsNullOrEmpty(s.StorageLocation) ? "(未設定儲位) - " + s.Quantity.ToString("N0") : $"{s.StorageLocation} - {s.Quantity:N0}",
                Value = s.StorageLocation ?? "",
                Qty = s.Quantity
            }).ToList();

            cmbStorageLocation.DataSource = null;
            if (dataSource.Any())
            {
                cmbStorageLocation.DataSource = dataSource;
                cmbStorageLocation.DisplayMember = "Display";
                cmbStorageLocation.ValueMember = "Value";
                cmbStorageLocation.SelectedIndex = 0;
            }
            else
            {
                lblCurrentStock.Text = "0";
            }
        }

        private void ResetInfo()
        {
            _currentMaterial = null;
            lblMaterialName.Text = "--";
            lblSpecification.Text = "--";
            cmbStorageLocation.DataSource = null;
            lblCurrentStock.Text = "0";
        }

        public async void NavigateToOutboundWithBarcode(string barcode)
        {
            this.BringToFront();
            txtBarcode.Text = barcode;
            await SearchMaterialAsync();
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_currentMaterial == null) return;
            if (!decimal.TryParse(txtQty.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputCorrectQty"));
                return;
            }

            try
            {
                var dto = new OutboundDto
                {
                    Barcode = _currentMaterial.Barcode,
                    WarehouseId = (int)cmbWarehouse.SelectedValue,
                    StorageLocation = cmbStorageLocation.SelectedValue?.ToString() ?? "",
                    Quantity = qty,
                    OperatorId = UserSession.Username
                };

                var result = await _apiClient.PostOutboundAsync(dto);
                if (result.IsSuccess)
                {
                    using (var popup = new LocationPopupForm(cmbStorageLocation.SelectedValue?.ToString() ?? ""))
                    {
                        popup.ShowDialog(this);
                    }
                    
                    if (result.IsLowStock)
                    {
                        string template = LocalizationService.GetString("Msg_LowStockAlert");
                        string lowStockMsg = string.Format(template, result.TotalQuantity.ToString("N0"), result.SafeStockQty);
                        LowStockWarningForm.ShowAlert(lowStockMsg);
                    }

                    txtBarcode.Clear();
                    ResetInfo();
                    txtBarcode.Focus();
                }
                else
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_PostFailed") + result.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PostFailed") + ex.Message);
            }
        }
    }
}
