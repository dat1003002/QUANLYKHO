namespace QUANLYKHO.UI
{
    partial class Lichsuxuat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lichsuxuat));
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            gv_lichsu = new DevExpress.XtraGrid.GridControl();
            grid_lichsu = new DevExpress.XtraGrid.Views.Grid.GridView();
            btn_timkiem = new System.Windows.Forms.Button();
            date_end = new System.Windows.Forms.DateTimePicker();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            date_start = new System.Windows.Forms.DateTimePicker();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gv_lichsu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_lichsu).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1687, 85);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(1687, 84);
            label1.TabIndex = 2;
            label1.Text = "Lịch Sử Xuất Kho";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(btn_timkiem);
            panel2.Controls.Add(date_end);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(date_start);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 85);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(1687, 116);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(gv_lichsu);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(0, 201);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(1687, 752);
            panel3.TabIndex = 2;
            // 
            // gv_lichsu
            // 
            gv_lichsu.Dock = System.Windows.Forms.DockStyle.Fill;
            gv_lichsu.Location = new System.Drawing.Point(0, 0);
            gv_lichsu.MainView = grid_lichsu;
            gv_lichsu.Name = "gv_lichsu";
            gv_lichsu.Size = new System.Drawing.Size(1687, 752);
            gv_lichsu.TabIndex = 0;
            gv_lichsu.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grid_lichsu });
            // 
            // grid_lichsu
            // 
            grid_lichsu.GridControl = gv_lichsu;
            grid_lichsu.Name = "grid_lichsu";
            grid_lichsu.OptionsView.ShowGroupPanel = false;
            // 
            // btn_timkiem
            // 
            btn_timkiem.BackColor = System.Drawing.SystemColors.MenuHighlight;
            btn_timkiem.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_timkiem.ForeColor = System.Drawing.SystemColors.Control;
            btn_timkiem.Location = new System.Drawing.Point(931, 47);
            btn_timkiem.Name = "btn_timkiem";
            btn_timkiem.Size = new System.Drawing.Size(97, 32);
            btn_timkiem.TabIndex = 9;
            btn_timkiem.Text = "Tìm Kiếm";
            btn_timkiem.UseVisualStyleBackColor = false;
            btn_timkiem.Click += btn_timkiem_Click;
            // 
            // date_end
            // 
            date_end.CalendarFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            date_end.CustomFormat = "dd/MM/yyyy";
            date_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            date_end.Location = new System.Drawing.Point(636, 48);
            date_end.Name = "date_end";
            date_end.Size = new System.Drawing.Size(251, 27);
            date_end.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(480, 51);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(150, 23);
            label3.TabIndex = 7;
            label3.Text = "Ngày Kết Thúc :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(40, 51);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(135, 23);
            label2.TabIndex = 6;
            label2.Text = "Ngày Bắt Đầu:";
            // 
            // date_start
            // 
            date_start.CalendarFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            date_start.CustomFormat = "dd/MM/yyyy";
            date_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            date_start.Location = new System.Drawing.Point(181, 48);
            date_start.Name = "date_start";
            date_start.Size = new System.Drawing.Size(251, 27);
            date_start.TabIndex = 5;
            // 
            // Lichsuxuat
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1687, 953);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Lichsuxuat";
            Text = "Lịch Sử Xuất Kho";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gv_lichsu).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_lichsu).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gv_lichsu;
        private DevExpress.XtraGrid.Views.Grid.GridView grid_lichsu;
        private System.Windows.Forms.Button btn_timkiem;
        private System.Windows.Forms.DateTimePicker date_end;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker date_start;
    }
}