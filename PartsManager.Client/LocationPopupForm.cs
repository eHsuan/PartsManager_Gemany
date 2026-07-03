using System;
using System.Drawing;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public class LocationPopupForm : Form
    {
        public LocationPopupForm(string location)
        {
            this.Text = "提示 (Notice)";
            this.Size = new Size(500, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30); // match dark theme typical in this app

            var lblMessage = new Label();
            lblMessage.AutoSize = false;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblMessage.Font = new Font("Microsoft JhengHei", 18F, FontStyle.Bold);
            lblMessage.ForeColor = Color.White;
            
            string template = LocalizationService.GetString("Msg_GoToLocation") ?? "請至儲位 {0} 領取";
            string locStr = string.IsNullOrEmpty(location) ? "--" : location;
            lblMessage.Text = string.Format(template, locStr);

            var btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Font = new Font("Microsoft JhengHei", 12F);
            btnOk.Size = new Size(120, 40);
            btnOk.BackColor = Color.FromArgb(0, 122, 204);
            btnOk.ForeColor = Color.White;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Cursor = Cursors.Hand;
            btnOk.Click += (s, e) => this.Close();

            var pnlBottom = new Panel();
            pnlBottom.Height = 70;
            pnlBottom.Dock = DockStyle.Bottom;
            
            btnOk.Location = new Point((this.ClientSize.Width - btnOk.Width) / 2, 10);
            pnlBottom.Resize += (s, e) => {
                btnOk.Location = new Point((pnlBottom.Width - btnOk.Width) / 2, 10);
            };

            pnlBottom.Controls.Add(btnOk);

            this.Controls.Add(lblMessage);
            this.Controls.Add(pnlBottom);
            
            this.AcceptButton = btnOk;
        }
    }
}
