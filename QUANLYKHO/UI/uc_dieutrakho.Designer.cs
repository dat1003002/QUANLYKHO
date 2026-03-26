namespace QUANLYKHO.UI
{
    partial class uc_dieutrakho
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_dieutrakho));
            xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            cb_category = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            cb_user = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            date_CutOffDate = new System.Windows.Forms.DateTimePicker();
            label2 = new System.Windows.Forms.Label();
            txt_CheckCode = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btn_addkiemkho = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btn_exportexecl = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            btn_Detail = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            panel2 = new System.Windows.Forms.Panel();
            gridViewInventory = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridControlInventory = new DevExpress.XtraGrid.GridControl();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).BeginInit();
            xtraTabControl1.SuspendLayout();
            xtraTabPage1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cb_category.Properties).BeginInit();
            toolStrip1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridViewInventory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControlInventory).BeginInit();
            SuspendLayout();
            // 
            // xtraTabControl1
            // 
            xtraTabControl1.Appearance.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            xtraTabControl1.Appearance.Options.UseFont = true;
            xtraTabControl1.AppearancePage.Header.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            xtraTabControl1.AppearancePage.Header.Options.UseFont = true;
            xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            xtraTabControl1.Name = "xtraTabControl1";
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            xtraTabControl1.Size = new System.Drawing.Size(1790, 960);
            xtraTabControl1.TabIndex = 0;
            xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { xtraTabPage1 });
            // 
            // xtraTabPage1
            // 
            xtraTabPage1.Controls.Add(tableLayoutPanel1);
            xtraTabPage1.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage1.ImageOptions.Image");
            xtraTabPage1.Name = "xtraTabPage1";
            xtraTabPage1.Size = new System.Drawing.Size(1788, 927);
            xtraTabPage1.Text = "Danh Sách";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.25998F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.74002F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1788, 927);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(cb_category);
            panel1.Controls.Add(cb_user);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(date_CutOffDate);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txt_CheckCode);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(toolStrip1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1782, 154);
            panel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(39, 70);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(94, 19);
            label4.TabIndex = 8;
            label4.Text = "Danh Mục :";
            // 
            // cb_category
            // 
            cb_category.Location = new System.Drawing.Point(151, 65);
            cb_category.Name = "cb_category";
            cb_category.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cb_category.Properties.Appearance.Options.UseFont = true;
            cb_category.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cb_category.Size = new System.Drawing.Size(273, 28);
            cb_category.TabIndex = 7;
            cb_category.EditValueChanged += cb_category_EditValueChanged;
            // 
            // cb_user
            // 
            cb_user.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cb_user.FormattingEnabled = true;
            cb_user.Location = new System.Drawing.Point(850, 42);
            cb_user.Name = "cb_user";
            cb_user.Size = new System.Drawing.Size(264, 27);
            cb_user.TabIndex = 6;
            cb_user.SelectedIndexChanged += cb_user_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(750, 52);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(94, 19);
            label3.TabIndex = 5;
            label3.Text = "Phê Duyệt :";
            // 
            // date_CutOffDate
            // 
            date_CutOffDate.CustomFormat = "dd/MM/yyyy";
            date_CutOffDate.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            date_CutOffDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            date_CutOffDate.Location = new System.Drawing.Point(557, 44);
            date_CutOffDate.Name = "date_CutOffDate";
            date_CutOffDate.Size = new System.Drawing.Size(164, 27);
            date_CutOffDate.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(451, 50);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(100, 19);
            label2.TabIndex = 3;
            label2.Text = "Ngày Kiểm :";
            // 
            // txt_CheckCode
            // 
            txt_CheckCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_CheckCode.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txt_CheckCode.Location = new System.Drawing.Point(151, 15);
            txt_CheckCode.Name = "txt_CheckCode";
            txt_CheckCode.Size = new System.Drawing.Size(273, 27);
            txt_CheckCode.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(39, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(106, 19);
            label1.TabIndex = 1;
            label1.Text = "Phiếu Kiểm :";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_addkiemkho, toolStripSeparator1, btn_exportexecl, toolStripSeparator2, btn_Detail, toolStripSeparator3 });
            toolStrip1.Location = new System.Drawing.Point(0, 106);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1780, 46);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btn_addkiemkho
            // 
            btn_addkiemkho.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_addkiemkho.ForeColor = System.Drawing.Color.White;
            btn_addkiemkho.Image = (System.Drawing.Image)resources.GetObject("btn_addkiemkho.Image");
            btn_addkiemkho.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_addkiemkho.Name = "btn_addkiemkho";
            btn_addkiemkho.Size = new System.Drawing.Size(87, 43);
            btn_addkiemkho.Text = "Kiểm Kho";
            btn_addkiemkho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_addkiemkho.Click += btn_addkiemkho_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_exportexecl
            // 
            btn_exportexecl.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_exportexecl.ForeColor = System.Drawing.Color.White;
            btn_exportexecl.Image = (System.Drawing.Image)resources.GetObject("btn_exportexecl.Image");
            btn_exportexecl.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_exportexecl.Name = "btn_exportexecl";
            btn_exportexecl.Size = new System.Drawing.Size(93, 43);
            btn_exportexecl.Text = "Xuất Execl";
            btn_exportexecl.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_exportexecl.Click += btn_exportexecl_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_Detail
            // 
            btn_Detail.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_Detail.ForeColor = System.Drawing.Color.White;
            btn_Detail.Image = (System.Drawing.Image)resources.GetObject("btn_Detail.Image");
            btn_Detail.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Detail.Name = "btn_Detail";
            btn_Detail.Size = new System.Drawing.Size(74, 43);
            btn_Detail.Text = "Chi Tiểt";
            btn_Detail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_Detail.Click += btn_Detail_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 46);
            // 
            // panel2
            // 
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(gridControlInventory);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(3, 163);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(1782, 761);
            panel2.TabIndex = 1;
            // 
            // gridViewInventory
            // 
            gridViewInventory.GridControl = gridControlInventory;
            gridViewInventory.Name = "gridViewInventory";
            gridViewInventory.OptionsView.ShowGroupPanel = false;
            // 
            // gridControlInventory
            // 
            gridControlInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControlInventory.Location = new System.Drawing.Point(0, 0);
            gridControlInventory.MainView = gridViewInventory;
            gridControlInventory.Name = "gridControlInventory";
            gridControlInventory.Size = new System.Drawing.Size(1780, 759);
            gridControlInventory.TabIndex = 0;
            gridControlInventory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewInventory });
            // 
            // uc_dieutrakho
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(xtraTabControl1);
            Name = "uc_dieutrakho";
            Size = new System.Drawing.Size(1790, 960);
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).EndInit();
            xtraTabControl1.ResumeLayout(false);
            xtraTabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cb_category.Properties).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridViewInventory).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControlInventory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_addkiemkho;
        private System.Windows.Forms.ToolStripButton btn_Detail;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CheckCode;
        private System.Windows.Forms.DateTimePicker date_CutOffDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_user;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripButton btn_exportexecl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cb_category;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gridControlInventory;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewInventory;
    }
}
