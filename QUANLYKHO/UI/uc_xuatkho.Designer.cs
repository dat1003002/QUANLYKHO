namespace QUANLYKHO.UI
{
    partial class uc_xuatkho
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_xuatkho));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btn_bracode = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            btn_xuatkho = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btn_lichsu = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            btn_reset = new System.Windows.Forms.ToolStripButton();
            btn_barcode = new System.Windows.Forms.ToolStripSeparator();
            txt_search = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            btn_search = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            btn_execl = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            panel2 = new System.Windows.Forms.Panel();
            gv_xuatkho = new DevExpress.XtraGrid.GridControl();
            grid_xuatkho = new DevExpress.XtraGrid.Views.Grid.GridView();
            toolStrip1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gv_xuatkho).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_xuatkho).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_bracode, toolStripSeparator8, btn_xuatkho, toolStripSeparator1, btn_lichsu, toolStripSeparator2, btn_execl, btn_barcode, txt_search, toolStripSeparator9, btn_search, toolStripSeparator10, btn_reset, toolStripSeparator3 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1430, 46);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btn_bracode
            // 
            btn_bracode.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_bracode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_bracode.Image = (System.Drawing.Image)resources.GetObject("btn_bracode.Image");
            btn_bracode.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_bracode.Name = "btn_bracode";
            btn_bracode.Size = new System.Drawing.Size(75, 43);
            btn_bracode.Text = "Bracode";
            btn_bracode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_bracode.Click += btn_bracode_Click_1;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_xuatkho
            // 
            btn_xuatkho.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_xuatkho.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_xuatkho.Image = (System.Drawing.Image)resources.GetObject("btn_xuatkho.Image");
            btn_xuatkho.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_xuatkho.Name = "btn_xuatkho";
            btn_xuatkho.Size = new System.Drawing.Size(82, 43);
            btn_xuatkho.Text = "Xuất Kho";
            btn_xuatkho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_xuatkho.Click += btn_xuatkho_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_lichsu
            // 
            btn_lichsu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_lichsu.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_lichsu.Image = (System.Drawing.Image)resources.GetObject("btn_lichsu.Image");
            btn_lichsu.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_lichsu.Name = "btn_lichsu";
            btn_lichsu.Size = new System.Drawing.Size(74, 43);
            btn_lichsu.Text = "Lịch Sử ";
            btn_lichsu.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_lichsu.Click += btn_lichsu_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_reset
            // 
            btn_reset.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_reset.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_reset.Image = (System.Drawing.Image)resources.GetObject("btn_reset.Image");
            btn_reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_reset.Name = "btn_reset";
            btn_reset.Size = new System.Drawing.Size(54, 43);
            btn_reset.Text = "Reset";
            btn_reset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_reset.Click += btn_reset_Click;
            // 
            // btn_barcode
            // 
            btn_barcode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_barcode.Name = "btn_barcode";
            btn_barcode.Size = new System.Drawing.Size(6, 46);
            // 
            // txt_search
            // 
            txt_search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_search.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txt_search.Name = "txt_search";
            txt_search.Size = new System.Drawing.Size(250, 46);
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_search
            // 
            btn_search.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_search.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_search.Image = (System.Drawing.Image)resources.GetObject("btn_search.Image");
            btn_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_search.Name = "btn_search";
            btn_search.Size = new System.Drawing.Size(87, 43);
            btn_search.Text = "Tìm Kiếm";
            btn_search.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_search.Click += btn_search_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new System.Drawing.Size(6, 46);
            // 
            // btn_execl
            // 
            btn_execl.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_execl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_execl.Image = (System.Drawing.Image)resources.GetObject("btn_execl.Image");
            btn_execl.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_execl.Name = "btn_execl";
            btn_execl.Size = new System.Drawing.Size(54, 43);
            btn_execl.Text = "Execl";
            btn_execl.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btn_execl.Click += btn_execl_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 46);
            // 
            // panel2
            // 
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(gv_xuatkho);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 46);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(1430, 829);
            panel2.TabIndex = 2;
            // 
            // gv_xuatkho
            // 
            gv_xuatkho.Dock = System.Windows.Forms.DockStyle.Fill;
            gv_xuatkho.Location = new System.Drawing.Point(0, 0);
            gv_xuatkho.MainView = grid_xuatkho;
            gv_xuatkho.Name = "gv_xuatkho";
            gv_xuatkho.Size = new System.Drawing.Size(1428, 827);
            gv_xuatkho.TabIndex = 0;
            gv_xuatkho.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grid_xuatkho });
            // 
            // grid_xuatkho
            // 
            grid_xuatkho.GridControl = gv_xuatkho;
            grid_xuatkho.Name = "grid_xuatkho";
            grid_xuatkho.OptionsView.ShowGroupPanel = false;
            grid_xuatkho.OptionsView.ShowViewCaption = true;
            grid_xuatkho.ViewCaption = "Danh Sách Sản Phẩm";
            // 
            // uc_xuatkho
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel2);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(2);
            Name = "uc_xuatkho";
            Size = new System.Drawing.Size(1430, 875);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gv_xuatkho).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_xuatkho).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gv_xuatkho;
        private DevExpress.XtraGrid.Views.Grid.GridView grid_xuatkho;
        private System.Windows.Forms.ToolStripButton btn_lichsu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btn_xuatkho;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripSeparator btn_barcode;
        private System.Windows.Forms.ToolStripTextBox txt_search;
        private System.Windows.Forms.ToolStripButton btn_reset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btn_bracode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton btn_execl;
    }
}
