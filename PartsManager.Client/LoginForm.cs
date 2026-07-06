using System;
using System.Drawing;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class LoginForm : Form
    {
        private readonly ApiClient _apiClient;

        public LoginForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);

            string version = typeof(LoginForm).Assembly.GetName().Version?.ToString(3) ?? "1.3.0";
            this.Text = $"{LocalizationService.GetString("LoginForm")} v{version}";
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            txtServerIP.Text = GlobalSettings.ServerIP;
            txtServerPort.Text = GlobalSettings.ServerPort;

            lblStatus.Text = LocalizationService.GetString("Status_Connecting");
            
            try
            {
                await _apiClient.GetWarehousesAsync();
                pnlStatus.BackColor = Color.Lime;
                lblStatus.Text = LocalizationService.GetString("Msg_ServerOnline");
                lblStatus.ForeColor = Color.DarkGreen;
            }
            catch
            {
                pnlStatus.BackColor = Color.Red;
                lblStatus.Text = LocalizationService.GetString("Msg_ServerOffline");
                lblStatus.ForeColor = Color.DarkRed;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputRequired"));
                return;
            }

            btnLogin.Enabled = false;
            try
            {
                var user = await _apiClient.LoginAsync(username, password);
                if (user != null)
                {
                    UserSession.UserID = user.UserID;
                    UserSession.Username = user.Username;
                    UserSession.UserLevel = user.UserLevel;
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_LoginFailed"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoginError") + ex.Message);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Application.Exit();

        private void btnSettings_Click(object sender, EventArgs e)
        {
            pnlSettings.Visible = !pnlSettings.Visible;
            
            // 動態調整視窗高度，以顯示或隱藏設定面板
            if (pnlSettings.Visible)
            {
                this.Height = 520; // 展開後的高度 (包含標題列)
            }
            else
            {
                this.Height = 360; // 原始高度 (包含標題列)
            }
        }

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            GlobalSettings.ServerIP = txtServerIP.Text.Trim();
            GlobalSettings.ServerPort = txtServerPort.Text.Trim();
            
            // 重新初始化 ApiClient 並重試
            LoginForm_Load(null, null);
        }
    }
}
