namespace QUANLYKHO
{
    partial class nhapkho
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(nhapkho));
            panel1 = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            txt_soluong = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txt_mota = new System.Windows.Forms.TextBox();
            btn_luu = new System.Windows.Forms.Button();
            btn_close = new System.Windows.Forms.Button();
            lb_name = new System.Windows.Forms.Label();
            lb_quycach = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(584, 64);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(12, 183);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 19);
            label2.TabIndex = 1;
            label2.Text = "Số Lượng :";
            // 
            // txt_soluong
            // 
            txt_soluong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_soluong.Location = new System.Drawing.Point(130, 175);
            txt_soluong.Name = "txt_soluong";
            txt_soluong.Size = new System.Drawing.Size(382, 27);
            txt_soluong.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(35, 248);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(66, 19);
            label3.TabIndex = 3;
            label3.Text = "Mô Tả :";
            // 
            // txt_mota
            // 
            txt_mota.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_mota.Location = new System.Drawing.Point(130, 240);
            txt_mota.Name = "txt_mota";
            txt_mota.Size = new System.Drawing.Size(382, 27);
            txt_mota.TabIndex = 4;
            // 
            // btn_luu
            // 
            btn_luu.BackColor = System.Drawing.Color.FromArgb(72, 143, 233);
            btn_luu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_luu.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_luu.Location = new System.Drawing.Point(130, 300);
            btn_luu.Name = "btn_luu";
            btn_luu.Size = new System.Drawing.Size(103, 45);
            btn_luu.TabIndex = 5;
            btn_luu.Text = "Lưu";
            btn_luu.UseVisualStyleBackColor = false;
            btn_luu.Click += btn_luu_Click;
            // 
            // btn_close
            // 
            btn_close.BackColor = System.Drawing.Color.Crimson;
            btn_close.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_close.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_close.Location = new System.Drawing.Point(320, 300);
            btn_close.Name = "btn_close";
            btn_close.Size = new System.Drawing.Size(103, 45);
            btn_close.TabIndex = 6;
            btn_close.Text = "Hủy";
            btn_close.UseVisualStyleBackColor = false;
            btn_close.Click += btn_close_Click;
            // 
            // lb_name
            // 
            lb_name.AutoSize = true;
            lb_name.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_name.Location = new System.Drawing.Point(12, 88);
            lb_name.Name = "lb_name";
            lb_name.Size = new System.Drawing.Size(118, 20);
            lb_name.TabIndex = 7;
            lb_name.Text = "Nguyên Liệu :";
            // 
            // lb_quycach
            // 
            lb_quycach.AutoSize = true;
            lb_quycach.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_quycach.Location = new System.Drawing.Point(40, 127);
            lb_quycach.Name = "lb_quycach";
            lb_quycach.Size = new System.Drawing.Size(90, 19);
            lb_quycach.TabIndex = 8;
            lb_quycach.Text = "Quy Cách :";
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(584, 63);
            label1.TabIndex = 0;
            label1.Text = "Tạo Phiếu Nhập Kho";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nhapkho
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 376);
            Controls.Add(lb_quycach);
            Controls.Add(lb_name);
            Controls.Add(btn_close);
            Controls.Add(btn_luu);
            Controls.Add(txt_mota);
            Controls.Add(label3);
            Controls.Add(txt_soluong);
            Controls.Add(label2);
            Controls.Add(panel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "nhapkho";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Phiếu Nhập Kho";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_soluong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_mota;
        private System.Windows.Forms.Button btn_luu;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label lb_name;
        private System.Windows.Forms.Label lb_quycach;
        private System.Windows.Forms.Label label1;
    }
}