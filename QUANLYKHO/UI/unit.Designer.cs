namespace QUANLYKHO.UI
{
    partial class unit
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(unit));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btn_add = new System.Windows.Forms.ToolStripButton();
            btn_edit = new System.Windows.Forms.ToolStripButton();
            btn_delete = new System.Windows.Forms.ToolStripButton();
            panel1 = new System.Windows.Forms.Panel();
            txt_symbol = new System.Windows.Forms.TextBox();
            txt_name = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            gv_unit = new DevExpress.XtraGrid.GridControl();
            grid_unit = new DevExpress.XtraGrid.Views.Grid.GridView();
            toolStrip1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gv_unit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_unit).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_add, btn_edit, btn_delete });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1103, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btn_add
            // 
            btn_add.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_add.ForeColor = System.Drawing.Color.White;
            btn_add.Image = (System.Drawing.Image)resources.GetObject("btn_add.Image");
            btn_add.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_add.Name = "btn_add";
            btn_add.Size = new System.Drawing.Size(78, 24);
            btn_add.Text = "Thêm ";
            btn_add.Click += btn_add_Click;
            // 
            // btn_edit
            // 
            btn_edit.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_edit.ForeColor = System.Drawing.Color.White;
            btn_edit.Image = (System.Drawing.Image)resources.GetObject("btn_edit.Image");
            btn_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_edit.Name = "btn_edit";
            btn_edit.Size = new System.Drawing.Size(60, 24);
            btn_edit.Text = "Sửa";
            btn_edit.Click += btn_edit_Click;
            // 
            // btn_delete
            // 
            btn_delete.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_delete.ForeColor = System.Drawing.Color.White;
            btn_delete.Image = (System.Drawing.Image)resources.GetObject("btn_delete.Image");
            btn_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_delete.Name = "btn_delete";
            btn_delete.Size = new System.Drawing.Size(62, 24);
            btn_delete.Text = "Xóa";
            btn_delete.Click += btn_delete_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(txt_symbol);
            panel1.Controls.Add(txt_name);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 27);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1103, 64);
            panel1.TabIndex = 1;
            // 
            // txt_symbol
            // 
            txt_symbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_symbol.Location = new System.Drawing.Point(445, 24);
            txt_symbol.Name = "txt_symbol";
            txt_symbol.Size = new System.Drawing.Size(222, 23);
            txt_symbol.TabIndex = 3;
            // 
            // txt_name
            // 
            txt_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_name.Location = new System.Drawing.Point(109, 24);
            txt_name.Name = "txt_name";
            txt_name.Size = new System.Drawing.Size(222, 23);
            txt_name.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(353, 24);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(76, 19);
            label2.TabIndex = 1;
            label2.Text = "ký Hiệu :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(33, 24);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 19);
            label1.TabIndex = 0;
            label1.Text = "Đơn Vị :";
            // 
            // gv_unit
            // 
            gv_unit.Dock = System.Windows.Forms.DockStyle.Fill;
            gv_unit.Location = new System.Drawing.Point(0, 91);
            gv_unit.MainView = grid_unit;
            gv_unit.Name = "gv_unit";
            gv_unit.Size = new System.Drawing.Size(1103, 596);
            gv_unit.TabIndex = 2;
            gv_unit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grid_unit });
            // 
            // grid_unit
            // 
            grid_unit.GridControl = gv_unit;
            grid_unit.Name = "grid_unit";
            grid_unit.OptionsView.ShowGroupPanel = false;
            grid_unit.RowClick += grid_unit_RowClick;
            // 
            // unit
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1103, 687);
            Controls.Add(gv_unit);
            Controls.Add(panel1);
            Controls.Add(toolStrip1);
            Name = "unit";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Đơn Vị Tính";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gv_unit).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_unit).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_add;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gv_unit;
        private DevExpress.XtraGrid.Views.Grid.GridView grid_unit;
        private System.Windows.Forms.TextBox txt_symbol;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
    }
}