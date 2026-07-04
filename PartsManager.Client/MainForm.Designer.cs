namespace PartsManager.Client
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.cmbStorageLocation = new System.Windows.Forms.ComboBox();
            this.labelStorageLocationTitle = new System.Windows.Forms.Label();
            this.lblSpecification = new System.Windows.Forms.Label();
            this.lblCurrentStock = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWarehouseLabel = new System.Windows.Forms.Label();
            this.cmbWarehouse = new System.Windows.Forms.ComboBox();
            this.pnlServerStatus = new System.Windows.Forms.Panel();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcode.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.ForeColor = System.Drawing.Color.White;
            this.txtBarcode.Location = new System.Drawing.Point(45, 154);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(629, 55);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // txtQty
            // 
            this.txtQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQty.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty.ForeColor = System.Drawing.Color.White;
            this.txtQty.Location = new System.Drawing.Point(45, 694);
            this.txtQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(179, 50);
            this.txtQty.TabIndex = 2;
            this.txtQty.Text = "1";
            this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(255, 687);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(420, 68);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Tag = "Btn_Confirm";
            this.btnConfirm.Text = "確認領料 (OUTBOUND)";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblStatus.ForeColor = System.Drawing.Color.Cyan;
            this.lblStatus.Location = new System.Drawing.Point(45, 784);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(223, 30);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Tag = "Status_Ready";
            this.lblStatus.Text = "系統就緒，請掃描...";
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblMaterialName.ForeColor = System.Drawing.Color.White;
            this.lblMaterialName.Location = new System.Drawing.Point(165, 30);
            this.lblMaterialName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(43, 37);
            this.lblMaterialName.TabIndex = 0;
            this.lblMaterialName.Text = "--";
            // 
            // cmbStorageLocation
            // 
            this.cmbStorageLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.cmbStorageLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorageLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbStorageLocation.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbStorageLocation.ForeColor = System.Drawing.Color.White;
            this.cmbStorageLocation.FormattingEnabled = true;
            this.cmbStorageLocation.Location = new System.Drawing.Point(175, 222);
            this.cmbStorageLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbStorageLocation.Name = "cmbStorageLocation";
            this.cmbStorageLocation.Size = new System.Drawing.Size(373, 38);
            this.cmbStorageLocation.TabIndex = 8;
            this.cmbStorageLocation.SelectedIndexChanged += new System.EventHandler(this.cmbStorageLocation_SelectedIndexChanged);
            // 
            // labelStorageLocationTitle
            // 
            this.labelStorageLocationTitle.AutoSize = true;
            this.labelStorageLocationTitle.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelStorageLocationTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelStorageLocationTitle.Location = new System.Drawing.Point(22, 225);
            this.labelStorageLocationTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStorageLocationTitle.Name = "labelStorageLocationTitle";
            this.labelStorageLocationTitle.Size = new System.Drawing.Size(85, 30);
            this.labelStorageLocationTitle.TabIndex = 7;
            this.labelStorageLocationTitle.Tag = "Label_StorageLocation";
            this.labelStorageLocationTitle.Text = "儲位：";
            // 
            // lblSpecification
            // 
            this.lblSpecification.AutoSize = true;
            this.lblSpecification.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSpecification.ForeColor = System.Drawing.Color.LightGray;
            this.lblSpecification.Location = new System.Drawing.Point(165, 90);
            this.lblSpecification.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpecification.Name = "lblSpecification";
            this.lblSpecification.Size = new System.Drawing.Size(33, 30);
            this.lblSpecification.TabIndex = 1;
            this.lblSpecification.Text = "--";
            // 
            // lblCurrentStock
            // 
            this.lblCurrentStock.AutoSize = true;
            this.lblCurrentStock.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentStock.ForeColor = System.Drawing.Color.White;
            this.lblCurrentStock.Location = new System.Drawing.Point(165, 150);
            this.lblCurrentStock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentStock.Name = "lblCurrentStock";
            this.lblCurrentStock.Size = new System.Drawing.Size(50, 56);
            this.lblCurrentStock.TabIndex = 2;
            this.lblCurrentStock.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(45, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 31);
            this.label1.TabIndex = 5;
            this.label1.Tag = "Label_ScanBarcode";
            this.label1.Text = "請掃描物料條碼：";
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Controls.Add(this.lblCurrentStock);
            this.pnlInfo.Controls.Add(this.lblSpecification);
            this.pnlInfo.Controls.Add(this.lblMaterialName);
            this.pnlInfo.Controls.Add(this.cmbStorageLocation);
            this.pnlInfo.Controls.Add(this.labelStorageLocationTitle);
            this.pnlInfo.Controls.Add(this.label2);
            this.pnlInfo.Controls.Add(this.label3);
            this.pnlInfo.Controls.Add(this.label4);
            this.pnlInfo.Location = new System.Drawing.Point(45, 230);
            this.pnlInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(629, 299);
            this.pnlInfo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(22, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 30);
            this.label2.TabIndex = 4;
            this.label2.Tag = "Label_MaterialName";
            this.label2.Text = "品名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(22, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 30);
            this.label3.TabIndex = 5;
            this.label3.Tag = "Label_Spec";
            this.label3.Text = "規格：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(22, 165);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 30);
            this.label4.TabIndex = 6;
            this.label4.Tag = "Label_CurrentStock";
            this.label4.Text = "當前庫存：";
            // 
            // lblWarehouseLabel
            // 
            this.lblWarehouseLabel.AutoSize = true;
            this.lblWarehouseLabel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblWarehouseLabel.ForeColor = System.Drawing.Color.White;
            this.lblWarehouseLabel.Location = new System.Drawing.Point(39, 540);
            this.lblWarehouseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWarehouseLabel.Name = "lblWarehouseLabel";
            this.lblWarehouseLabel.Size = new System.Drawing.Size(133, 30);
            this.lblWarehouseLabel.TabIndex = 8;
            this.lblWarehouseLabel.Tag = "Label_OpWarehouse";
            this.lblWarehouseLabel.Text = "作業倉庫：";
            // 
            // cmbWarehouse
            // 
            this.cmbWarehouse.BackColor = System.Drawing.Color.White;
            this.cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouse.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbWarehouse.ForeColor = System.Drawing.Color.Black;
            this.cmbWarehouse.FormattingEnabled = true;
            this.cmbWarehouse.Location = new System.Drawing.Point(45, 578);
            this.cmbWarehouse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(298, 38);
            this.cmbWarehouse.TabIndex = 0;
            // 
            // pnlServerStatus
            // 
            this.pnlServerStatus.BackColor = System.Drawing.Color.Gray;
            this.pnlServerStatus.Location = new System.Drawing.Point(45, 832);
            this.pnlServerStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlServerStatus.Name = "pnlServerStatus";
            this.pnlServerStatus.Size = new System.Drawing.Size(15, 15);
            this.pnlServerStatus.TabIndex = 9;
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lblServerStatus.ForeColor = System.Drawing.Color.White;
            this.lblServerStatus.Location = new System.Drawing.Point(68, 828);
            this.lblServerStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(166, 23);
            this.lblServerStatus.TabIndex = 10;
            this.lblServerStatus.Text = "Server Connecting";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1360, 583);
            this.Controls.Add(this.lblServerStatus);
            this.Controls.Add(this.pnlServerStatus);
            this.Controls.Add(this.cmbWarehouse);
            this.Controls.Add(this.lblWarehouseLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.txtBarcode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "零件管理系統 (PartsManager)";
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.ComboBox cmbStorageLocation;
        private System.Windows.Forms.Label labelStorageLocationTitle;
        private System.Windows.Forms.Label lblSpecification;
        private System.Windows.Forms.Label lblCurrentStock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblWarehouseLabel;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Panel pnlServerStatus;
        private System.Windows.Forms.Label lblServerStatus;
    }
}
