namespace QUANLYKHO.UI
{
    partial class uc_trangthai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_trangthai));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            txt_search = new System.Windows.Forms.ToolStripTextBox();
            btn_timkiem = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btn_execl = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            gv_trangthai = new DevExpress.XtraGrid.GridControl();
            grid_trangthai = new DevExpress.XtraGrid.Views.Grid.GridView();
            panel2 = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            btn_reset = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gv_trangthai).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_trangthai).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { txt_search, btn_timkiem, toolStripSeparator3, btn_execl, toolStripSeparator1, btn_reset, toolStripSeparator2 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1722, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // txt_search
            // 
            txt_search.Name = "txt_search";
            txt_search.Size = new System.Drawing.Size(250, 27);
            // 
            // btn_timkiem
            // 
            btn_timkiem.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_timkiem.ForeColor = System.Drawing.SystemColors.Control;
            btn_timkiem.Image = (System.Drawing.Image)resources.GetObject("btn_timkiem.Image");
            btn_timkiem.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_timkiem.Name = "btn_timkiem";
            btn_timkiem.Size = new System.Drawing.Size(107, 24);
            btn_timkiem.Text = "Tìm Kiếm";
            btn_timkiem.Click += btn_timkiem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btn_execl
            // 
            btn_execl.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_execl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_execl.Image = (System.Drawing.Image)resources.GetObject("btn_execl.Image");
            btn_execl.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_execl.Name = "btn_execl";
            btn_execl.Size = new System.Drawing.Size(74, 24);
            btn_execl.Text = "Execl";
            btn_execl.Click += btn_execl_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // gv_trangthai
            // 
            gv_trangthai.Dock = System.Windows.Forms.DockStyle.Fill;
            gv_trangthai.Location = new System.Drawing.Point(0, 0);
            gv_trangthai.MainView = grid_trangthai;
            gv_trangthai.Name = "gv_trangthai";
            gv_trangthai.Size = new System.Drawing.Size(1722, 880);
            gv_trangthai.TabIndex = 0;
            gv_trangthai.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grid_trangthai });
            // 
            // grid_trangthai
            // 
            grid_trangthai.GridControl = gv_trangthai;
            grid_trangthai.Name = "grid_trangthai";
            grid_trangthai.OptionsView.ShowGroupPanel = false;
            // 
            // panel2
            // 
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 27);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(1722, 34);
            panel2.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(gv_trangthai);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 61);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1722, 880);
            panel1.TabIndex = 3;
            // 
            // btn_reset
            // 
            btn_reset.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_reset.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_reset.Image = (System.Drawing.Image)resources.GetObject("btn_reset.Image");
            btn_reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_reset.Name = "btn_reset";
            btn_reset.Size = new System.Drawing.Size(74, 24);
            btn_reset.Text = "Reset";
            btn_reset.Click += btn_reset_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // uc_trangthai
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(toolStrip1);
            Name = "uc_trangthai";
            Size = new System.Drawing.Size(1722, 941);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gv_trangthai).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_trangthai).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox txt_search;
        private System.Windows.Forms.ToolStripButton btn_timkiem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_execl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private DevExpress.XtraGrid.GridControl gv_trangthai;
        private DevExpress.XtraGrid.Views.Grid.GridView grid_trangthai;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btn_reset;
    }
}
