using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class MachineManagementForm : Form
    {
        private readonly ApiClient _apiClient;
        private int? _selectedMachineId = null;

        public MachineManagementForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private async void MachineManagementForm_Load(object sender, EventArgs e)
        {
            await LoadMachinesAsync();
        }

        private async System.Threading.Tasks.Task LoadMachinesAsync()
        {
            try
            {
                var machines = await _apiClient.GetMachinesAsync();
                dgvMachines.DataSource = machines;

                // 欄位在地化
                if (dgvMachines.Columns["MachineID"] != null) dgvMachines.Columns["MachineID"].Visible = false;
                if (dgvMachines.Columns["MachineCode"] != null) dgvMachines.Columns["MachineCode"].HeaderText = LocalizationService.GetString("Col_MachineCode");
                if (dgvMachines.Columns["MachineName"] != null) dgvMachines.Columns["MachineName"].HeaderText = LocalizationService.GetString("Col_MachineName");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Machines Error: " + ex.Message);
            }
        }

        private void dgvMachines_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMachines.CurrentRow != null)
            {
                var machine = (MachineDto)dgvMachines.CurrentRow.DataBoundItem;
                txtMachineCode.Text = machine.MachineCode;
                txtMachineName.Text = machine.MachineName;
                _selectedMachineId = machine.MachineID;
                btnDelete.Enabled = true;
            }
            else
            {
                _selectedMachineId = null;
                btnDelete.Enabled = false;
            }
        }

        private void btnAddMachine_Click(object sender, EventArgs e)
        {
            _selectedMachineId = null;
            txtMachineCode.Clear();
            txtMachineName.Clear();
            txtMachineCode.Focus();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMachineCode.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_MachineCodeRequired"));
                return;
            }

            try
            {
                if (_selectedMachineId.HasValue)
                {
                    var dto = new UpdateMachineDto
                    {
                        MachineCode = txtMachineCode.Text.Trim(),
                        MachineName = txtMachineName.Text.Trim()
                    };
                    await _apiClient.UpdateMachineAsync(_selectedMachineId.Value, dto);
                }
                else
                {
                    var dto = new CreateMachineDto
                    {
                        MachineCode = txtMachineCode.Text.Trim(),
                        MachineName = txtMachineName.Text.Trim()
                    };
                    await _apiClient.CreateMachineAsync(dto);
                }

                MessageBox.Show(LocalizationService.GetString("Msg_SaveSuccess"));
                await LoadMachinesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SaveError") + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_selectedMachineId.HasValue) return;

            if (MessageBox.Show(LocalizationService.GetString("Msg_DeleteConfirm"), 
                LocalizationService.GetString("Common_Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteMachineAsync(_selectedMachineId.Value);
                    await LoadMachinesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete Error: " + ex.Message);
                }
            }
        }
    }
}
