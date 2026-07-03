using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class UserManagementForm : Form
    {
        private readonly ApiClient _apiClient;

        public UserManagementForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private async void UserManagementForm_Load(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async System.Threading.Tasks.Task LoadUsersAsync()
        {
            try
            {
                var users = await _apiClient.GetUsersAsync();
                dgvUsers.DataSource = users.Select(u => new 
                {
                    u.UserID,
                    u.Username,
                    u.UserLevel,
                    u.IsActive
                }).ToList();

                // 欄位在地化
                if (dgvUsers.Columns["UserID"] != null) dgvUsers.Columns["UserID"].HeaderText = "ID";
                if (dgvUsers.Columns["Username"] != null) dgvUsers.Columns["Username"].HeaderText = LocalizationService.GetString("Col_UserName");
                if (dgvUsers.Columns["UserLevel"] != null) dgvUsers.Columns["UserLevel"].HeaderText = LocalizationService.GetString("Col_UserLevel");
                if (dgvUsers.Columns["IsActive"] != null) dgvUsers.Columns["IsActive"].HeaderText = LocalizationService.GetString("Col_IsActive");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Users Error: " + ex.Message);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewUsername.Text) || string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputRequired"));
                return;
            }

            if (!int.TryParse(txtNewLevel.Text, out int level)) level = 4;

            try
            {
                var dto = new CreateUserDto
                {
                    Username = txtNewUsername.Text.Trim(),
                    Password = txtNewPassword.Text,
                    UserLevel = level
                };

                await _apiClient.CreateUserAsync(dto, UserSession.UserLevel);
                txtNewUsername.Clear();
                txtNewPassword.Clear();
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Create User Error: " + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            
            dynamic user = dgvUsers.CurrentRow.DataBoundItem;
            string username = user.Username;
            int userId = user.UserID;

            if (MessageBox.Show(string.Format(LocalizationService.GetString("Msg_DeleteConfirm"), username), 
                LocalizationService.GetString("Common_Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteUserAsync(userId, UserSession.UserLevel);
                    await LoadUsersAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_DeleteError") + ex.Message);
                }
            }
        }
    }
}
