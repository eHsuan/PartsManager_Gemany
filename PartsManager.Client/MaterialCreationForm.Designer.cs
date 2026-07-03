namespace PartsManager.Client
{
    partial class MaterialCreationForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPartNo = new System.Windows.Forms.TextBox();
            this.btnGenTempPartNo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numSafeStock = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numLeadTime = new System.Windows.Forms.NumericUpDown();
            this.labelStorageLocation = new System.Windows.Forms.Label();
            this.txtStorageLocation = new System.Windows.Forms.TextBox();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.txtManufacturer = new System.Windows.Forms.TextBox();
            this.labelManufacturerNo = new System.Windows.Forms.Label();
            this.txtManufacturerNo = new System.Windows.Forms.TextBox();
            this.labelPrice = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numInitialStock = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbInitialWarehouse = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpAttachments = new System.Windows.Forms.GroupBox();
            this.pnlAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.toolTipAttachment = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numSafeStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeadTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInitialStock)).BeginInit();
            this.grpAttachments.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 19);
            this.label1.TabIndex = 10;
            this.label1.Tag = "Label_PartNo";
            this.label1.Text = "料號 (Part No)";
            // 
            // txtPartNo
            // 
            this.txtPartNo.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtPartNo.Location = new System.Drawing.Point(30, 47);
            this.txtPartNo.Name = "txtPartNo";
            this.txtPartNo.Size = new System.Drawing.Size(300, 29);
            this.txtPartNo.TabIndex = 0;
            // 
            // btnGenTempPartNo
            // 
            this.btnGenTempPartNo.Font = new System.Drawing.Font("Microsoft JhengHei", 9F);
            this.btnGenTempPartNo.Location = new System.Drawing.Point(340, 46);
            this.btnGenTempPartNo.Name = "btnGenTempPartNo";
            this.btnGenTempPartNo.Size = new System.Drawing.Size(110, 31);
            this.btnGenTempPartNo.TabIndex = 11;
            this.btnGenTempPartNo.Tag = "Button_GenTempPartNo";
            this.btnGenTempPartNo.Text = "產生臨時料號";
            this.btnGenTempPartNo.UseVisualStyleBackColor = true;
            this.btnGenTempPartNo.Click += new System.EventHandler(this.btnGenTempPartNo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label2.Location = new System.Drawing.Point(30, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 19);
            this.label2.TabIndex = 12;
            this.label2.Tag = "Label_Name";
            this.label2.Text = "品名 (Name)";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtName.Location = new System.Drawing.Point(30, 107);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(420, 29);
            this.txtName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label3.Location = new System.Drawing.Point(30, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 19);
            this.label3.TabIndex = 13;
            this.label3.Tag = "Label_Specification";
            this.label3.Text = "規格 (Specification)";
            // 
            // txtSpec
            // 
            this.txtSpec.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtSpec.Location = new System.Drawing.Point(30, 167);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(420, 29);
            this.txtSpec.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label4.Location = new System.Drawing.Point(30, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 19);
            this.label4.TabIndex = 14;
            this.label4.Tag = "Label_SafeStock";
            this.label4.Text = "安全庫存 (Safe Stock)";
            // 
            // numSafeStock
            // 
            this.numSafeStock.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.numSafeStock.Location = new System.Drawing.Point(30, 227);
            this.numSafeStock.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numSafeStock.Name = "numSafeStock";
            this.numSafeStock.Size = new System.Drawing.Size(200, 29);
            this.numSafeStock.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label5.Location = new System.Drawing.Point(250, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 19);
            this.label5.TabIndex = 15;
            this.label5.Tag = "Label_LeadTime";
            this.label5.Text = "交期 (Lead Time)";
            // 
            // numLeadTime
            // 
            this.numLeadTime.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.numLeadTime.Location = new System.Drawing.Point(250, 227);
            this.numLeadTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numLeadTime.Name = "numLeadTime";
            this.numLeadTime.Size = new System.Drawing.Size(200, 29);
            this.numLeadTime.TabIndex = 4;
            // 
            // labelStorageLocation
            // 
            this.labelStorageLocation.AutoSize = true;
            this.labelStorageLocation.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.labelStorageLocation.Location = new System.Drawing.Point(30, 265);
            this.labelStorageLocation.Name = "labelStorageLocation";
            this.labelStorageLocation.Size = new System.Drawing.Size(164, 19);
            this.labelStorageLocation.TabIndex = 18;
            this.labelStorageLocation.Tag = "Label_StorageLocation";
            this.labelStorageLocation.Text = "儲位 (Storage Location)";
            // 
            // txtStorageLocation
            // 
            this.txtStorageLocation.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtStorageLocation.Location = new System.Drawing.Point(30, 287);
            this.txtStorageLocation.Name = "txtStorageLocation";
            this.txtStorageLocation.Size = new System.Drawing.Size(420, 29);
            this.txtStorageLocation.TabIndex = 5;
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.AutoSize = true;
            this.labelManufacturer.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.labelManufacturer.Location = new System.Drawing.Point(30, 325);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(117, 19);
            this.labelManufacturer.TabIndex = 23;
            this.labelManufacturer.Tag = "Label_Manufacturer";
            this.labelManufacturer.Text = "製造商 (Manufacturer)";
            // 
            // txtManufacturer
            // 
            this.txtManufacturer.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtManufacturer.Location = new System.Drawing.Point(30, 347);
            this.txtManufacturer.Name = "txtManufacturer";
            this.txtManufacturer.Size = new System.Drawing.Size(420, 29);
            this.txtManufacturer.TabIndex = 6;
            // 
            // labelManufacturerNo
            // 
            this.labelManufacturerNo.AutoSize = true;
            this.labelManufacturerNo.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.labelManufacturerNo.Location = new System.Drawing.Point(30, 385);
            this.labelManufacturerNo.Name = "labelManufacturerNo";
            this.labelManufacturerNo.Size = new System.Drawing.Size(164, 19);
            this.labelManufacturerNo.TabIndex = 19;
            this.labelManufacturerNo.Tag = "Label_ManufacturerNo";
            this.labelManufacturerNo.Text = "製造商號碼 (Manufacturer No.)";
            // 
            // txtManufacturerNo
            // 
            this.txtManufacturerNo.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtManufacturerNo.Location = new System.Drawing.Point(30, 407);
            this.txtManufacturerNo.Name = "txtManufacturerNo";
            this.txtManufacturerNo.Size = new System.Drawing.Size(420, 29);
            this.txtManufacturerNo.TabIndex = 7;
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.labelPrice.Location = new System.Drawing.Point(30, 445);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(84, 19);
            this.labelPrice.TabIndex = 22;
            this.labelPrice.Tag = "Label_Price";
            this.labelPrice.Text = "金額 (Price)";
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.numPrice.Location = new System.Drawing.Point(30, 467);
            this.numPrice.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(420, 29);
            this.numPrice.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label8.Location = new System.Drawing.Point(30, 505);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(176, 19);
            this.label8.TabIndex = 16;
            this.label8.Tag = "Label_InitialStock";
            this.label8.Text = "初期庫存 (Initial Stock)";
            // 
            // numInitialStock
            // 
            this.numInitialStock.Enabled = false;
            this.numInitialStock.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.numInitialStock.Location = new System.Drawing.Point(30, 527);
            this.numInitialStock.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numInitialStock.Name = "numInitialStock";
            this.numInitialStock.Size = new System.Drawing.Size(200, 29);
            this.numInitialStock.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label9.Location = new System.Drawing.Point(250, 505);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 19);
            this.label9.TabIndex = 17;
            this.label9.Tag = "Label_InitialWarehouse";
            this.label9.Text = "存放倉庫 (Warehouse)";
            // 
            // cmbInitialWarehouse
            // 
            this.cmbInitialWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInitialWarehouse.Enabled = false;
            this.cmbInitialWarehouse.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.cmbInitialWarehouse.FormattingEnabled = true;
            this.cmbInitialWarehouse.Location = new System.Drawing.Point(250, 528);
            this.cmbInitialWarehouse.Name = "cmbInitialWarehouse";
            this.cmbInitialWarehouse.Size = new System.Drawing.Size(200, 27);
            this.cmbInitialWarehouse.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(230, 690);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 45);
            this.btnSave.TabIndex = 13;
            this.btnSave.Tag = "Btn_Save";
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.btnCancel.Location = new System.Drawing.Point(350, 690);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 45);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Tag = "Btn_Cancel";
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpAttachments
            // 
            this.grpAttachments.Controls.Add(this.pnlAttachments);
            this.grpAttachments.Controls.Add(this.btnUpload);
            this.grpAttachments.Font = new System.Drawing.Font("Microsoft JhengHei", 10F);
            this.grpAttachments.Location = new System.Drawing.Point(30, 570);
            this.grpAttachments.Name = "grpAttachments";
            this.grpAttachments.Size = new System.Drawing.Size(420, 100);
            this.grpAttachments.TabIndex = 21;
            this.grpAttachments.TabStop = false;
            this.grpAttachments.Tag = "Label_Attachments";
            this.grpAttachments.Text = "附件 (PDF)";
            // 
            // pnlAttachments
            // 
            this.pnlAttachments.Location = new System.Drawing.Point(10, 25);
            this.pnlAttachments.Name = "pnlAttachments";
            this.pnlAttachments.Size = new System.Drawing.Size(280, 65);
            this.pnlAttachments.TabIndex = 1;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(300, 35);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 40);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Tag = "Btn_UploadPDF";
            this.btnUpload.Text = "上傳 PDF";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // MaterialCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 760);
            this.Controls.Add(this.grpAttachments);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbInitialWarehouse);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numInitialStock);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.txtManufacturer);
            this.Controls.Add(this.labelManufacturer);
            this.Controls.Add(this.txtManufacturerNo);
            this.Controls.Add(this.labelManufacturerNo);
            this.Controls.Add(this.txtStorageLocation);
            this.Controls.Add(this.labelStorageLocation);
            this.Controls.Add(this.numLeadTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numSafeStock);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSpec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGenTempPartNo);
            this.Controls.Add(this.txtPartNo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialCreationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "MaterialCreationForm";
            this.Text = "建立物料資訊 (New Material)";
            ((System.ComponentModel.ISupportInitialize)(this.numSafeStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeadTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInitialStock)).EndInit();
            this.grpAttachments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPartNo;
        private System.Windows.Forms.Button btnGenTempPartNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSafeStock;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numLeadTime;
        private System.Windows.Forms.Label labelStorageLocation;
        private System.Windows.Forms.TextBox txtStorageLocation;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.TextBox txtManufacturer;
        private System.Windows.Forms.Label labelManufacturerNo;
        private System.Windows.Forms.TextBox txtManufacturerNo;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numInitialStock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbInitialWarehouse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpAttachments;
        private System.Windows.Forms.FlowLayoutPanel pnlAttachments;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ToolTip toolTipAttachment;
    }
}
