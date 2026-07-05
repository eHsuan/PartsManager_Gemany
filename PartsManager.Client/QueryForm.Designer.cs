namespace PartsManager.Client
{
    partial class QueryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchKeyword = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.Col_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_PartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_StorageLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Manufacturer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ManufacturerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_SafeStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_LeadTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_TotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Att1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Col_Att2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOutbound = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnExport);
            this.panelTop.Controls.Add(this.btnShowAll);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.txtSearchKeyword);
            this.panelTop.Controls.Add(this.lblKeyword);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1800, 105);
            this.panelTop.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btnExport.Location = new System.Drawing.Point(1016, 29);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(180, 48);
            this.btnExport.TabIndex = 4;
            this.btnExport.Tag = "Btn_ExportExcel";
            this.btnExport.Text = "匯出 Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnShowAll
            // 
            this.btnShowAll.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btnShowAll.Location = new System.Drawing.Point(806, 29);
            this.btnShowAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(180, 48);
            this.btnShowAll.TabIndex = 3;
            this.btnShowAll.Tag = "Btn_ShowAll";
            this.btnShowAll.Text = "顯示全部";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(626, 29);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(150, 48);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Tag = "Btn_Search";
            this.btnSearch.Text = "🔍 查詢";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.txtSearchKeyword.Location = new System.Drawing.Point(318, 33);
            this.txtSearchKeyword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(276, 39);
            this.txtSearchKeyword.TabIndex = 1;
            this.txtSearchKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyDown);
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblKeyword.Location = new System.Drawing.Point(18, 38);
            this.lblKeyword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(144, 28);
            this.lblKeyword.TabIndex = 0;
            this.lblKeyword.Tag = "Label_Keyword";
            this.lblKeyword.Text = "關鍵字搜尋：";
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_Name,
            this.Col_Spec,
            this.Col_PartNo,
            this.Col_StorageLocation,
            this.Col_Warehouse,
            this.Col_Manufacturer,
            this.Col_ManufacturerNo,
            this.Col_Qty,
            this.Col_SafeStock,
            this.Col_LeadTime,
            this.Col_Price,
            this.Col_TotalAmount,
            this.Col_Att1,
            this.Col_Att2});
            this.dgvResults.ContextMenuStrip = this.ctxMenu;
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(0, 105);
            this.dgvResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersWidth = 62;
            this.dgvResults.RowTemplate.Height = 28;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Size = new System.Drawing.Size(1800, 795);
            this.dgvResults.TabIndex = 1;
            this.dgvResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResults_CellMouseDown);
            // 
            // Col_Name
            // 
            this.Col_Name.DataPropertyName = "Name";
            this.Col_Name.HeaderText = "物料名稱";
            this.Col_Name.MinimumWidth = 8;
            this.Col_Name.Name = "Col_Name";
            this.Col_Name.ReadOnly = true;
            this.Col_Name.Width = 180;
            // 
            // Col_Spec
            // 
            this.Col_Spec.DataPropertyName = "Specification";
            this.Col_Spec.HeaderText = "規格/型號";
            this.Col_Spec.MinimumWidth = 8;
            this.Col_Spec.Name = "Col_Spec";
            this.Col_Spec.ReadOnly = true;
            this.Col_Spec.Width = 180;
            // 
            // Col_PartNo
            // 
            this.Col_PartNo.DataPropertyName = "PartNo";
            this.Col_PartNo.HeaderText = "料號";
            this.Col_PartNo.MinimumWidth = 8;
            this.Col_PartNo.Name = "Col_PartNo";
            this.Col_PartNo.ReadOnly = true;
            this.Col_PartNo.Width = 120;
            // 
            // Col_StorageLocation
            // 
            this.Col_StorageLocation.DataPropertyName = "StorageLocation";
            this.Col_StorageLocation.HeaderText = "儲位";
            this.Col_StorageLocation.MinimumWidth = 8;
            this.Col_StorageLocation.Name = "Col_StorageLocation";
            this.Col_StorageLocation.ReadOnly = true;
            // 
            // Col_Warehouse
            // 
            this.Col_Warehouse.DataPropertyName = "WarehouseName";
            this.Col_Warehouse.HeaderText = "倉庫";
            this.Col_Warehouse.MinimumWidth = 8;
            this.Col_Warehouse.Name = "Col_Warehouse";
            this.Col_Warehouse.ReadOnly = true;
            this.Col_Warehouse.Width = 130;
            // 
            // Col_Manufacturer
            // 
            this.Col_Manufacturer.DataPropertyName = "Manufacturer";
            this.Col_Manufacturer.HeaderText = "製造商";
            this.Col_Manufacturer.MinimumWidth = 8;
            this.Col_Manufacturer.Name = "Col_Manufacturer";
            this.Col_Manufacturer.ReadOnly = true;
            this.Col_Manufacturer.Width = 120;
            // 
            // Col_ManufacturerNo
            // 
            this.Col_ManufacturerNo.DataPropertyName = "ManufacturerNo";
            this.Col_ManufacturerNo.HeaderText = "製造商號碼";
            this.Col_ManufacturerNo.MinimumWidth = 8;
            this.Col_ManufacturerNo.Name = "Col_ManufacturerNo";
            this.Col_ManufacturerNo.ReadOnly = true;
            // 
            // Col_Qty
            // 
            this.Col_Qty.DataPropertyName = "Quantity";
            this.Col_Qty.HeaderText = "在庫庫存";
            this.Col_Qty.MinimumWidth = 8;
            this.Col_Qty.Name = "Col_Qty";
            this.Col_Qty.ReadOnly = true;
            this.Col_Qty.Width = 80;
            // 
            // Col_SafeStock
            // 
            this.Col_SafeStock.DataPropertyName = "SafeStockQty";
            this.Col_SafeStock.HeaderText = "安全庫存";
            this.Col_SafeStock.MinimumWidth = 8;
            this.Col_SafeStock.Name = "Col_SafeStock";
            this.Col_SafeStock.ReadOnly = true;
            this.Col_SafeStock.Width = 80;
            // 
            // Col_LeadTime
            // 
            this.Col_LeadTime.DataPropertyName = "LeadTimeDays";
            this.Col_LeadTime.HeaderText = "交期(天)";
            this.Col_LeadTime.MinimumWidth = 8;
            this.Col_LeadTime.Name = "Col_LeadTime";
            this.Col_LeadTime.ReadOnly = true;
            this.Col_LeadTime.Width = 80;
            // 
            // Col_Price
            // 
            this.Col_Price.DataPropertyName = "Price";
            this.Col_Price.HeaderText = "金額";
            this.Col_Price.MinimumWidth = 8;
            this.Col_Price.Name = "Col_Price";
            this.Col_Price.ReadOnly = true;
            this.Col_Price.Width = 80;
            // 
            // Col_TotalAmount
            // 
            this.Col_TotalAmount.DataPropertyName = "TotalAmount";
            this.Col_TotalAmount.HeaderText = "總金額";
            this.Col_TotalAmount.MinimumWidth = 8;
            this.Col_TotalAmount.Name = "Col_TotalAmount";
            this.Col_TotalAmount.ReadOnly = true;
            // 
            // Col_Att1
            // 
            this.Col_Att1.HeaderText = "附1";
            this.Col_Att1.MinimumWidth = 8;
            this.Col_Att1.Name = "Col_Att1";
            this.Col_Att1.ReadOnly = true;
            this.Col_Att1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Col_Att1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Col_Att1.Width = 40;
            // 
            // Col_Att2
            // 
            this.Col_Att2.HeaderText = "附2";
            this.Col_Att2.MinimumWidth = 8;
            this.Col_Att2.Name = "Col_Att2";
            this.Col_Att2.ReadOnly = true;
            this.Col_Att2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Col_Att2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Col_Att2.Width = 40;
            // 
            // ctxMenu
            // 
            this.ctxMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete,
            this.menuOutbound});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(117, 94);
            this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(116, 30);
            this.menuEdit.Tag = "Menu_Edit";
            this.menuEdit.Text = "編輯";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(116, 30);
            this.menuDelete.Tag = "Menu_Delete";
            this.menuDelete.Text = "刪除";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // menuOutbound
            // 
            this.menuOutbound.Name = "menuOutbound";
            this.menuOutbound.Size = new System.Drawing.Size(116, 30);
            this.menuOutbound.Tag = "Menu_Outbound";
            this.menuOutbound.Text = "領料";
            this.menuOutbound.Click += new System.EventHandler(this.menuOutbound_Click);
            // 
            // QueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 900);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.panelTop);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "QueryForm";
            this.Tag = "QueryForm";
            this.Text = "物料庫存查詢";
            this.Load += new System.EventHandler(this.QueryForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ctxMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox txtSearchKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripMenuItem menuOutbound;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_StorageLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Manufacturer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ManufacturerNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_SafeStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_LeadTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_TotalAmount;
        private System.Windows.Forms.DataGridViewImageColumn Col_Att1;
        private System.Windows.Forms.DataGridViewImageColumn Col_Att2;
        private System.Windows.Forms.Button btnExport;
    }
}
