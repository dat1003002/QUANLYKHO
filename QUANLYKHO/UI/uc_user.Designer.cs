namespace QUANLYKHO.UI
{
    partial class uc_user
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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            panel1 = new System.Windows.Forms.Panel();
            gv_user = new DevExpress.XtraGrid.GridControl();
            grid_user = new DevExpress.XtraGrid.Views.Grid.GridView();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            cb_delete = new System.Windows.Forms.CheckBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            btn_delete = new System.Windows.Forms.Button();
            btn_edit = new System.Windows.Forms.Button();
            btn_add = new System.Windows.Forms.Button();
            cb_off = new System.Windows.Forms.CheckBox();
            cb_on = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            cb_Export = new System.Windows.Forms.CheckBox();
            cb_Import = new System.Windows.Forms.CheckBox();
            cb_role = new System.Windows.Forms.ComboBox();
            txt_password = new System.Windows.Forms.TextBox();
            txt_username = new System.Windows.Forms.TextBox();
            txt_fullname = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gv_user).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_user).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(62, 91, 135);
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1922, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 25);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Size = new System.Drawing.Size(1922, 996);
            splitContainer1.SplitterDistance = 1322;
            splitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Controls.Add(gv_user);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1322, 996);
            panel1.TabIndex = 0;
            // 
            // gv_user
            // 
            gv_user.Dock = System.Windows.Forms.DockStyle.Fill;
            gv_user.Location = new System.Drawing.Point(0, 0);
            gv_user.MainView = grid_user;
            gv_user.Name = "gv_user";
            gv_user.Size = new System.Drawing.Size(1322, 996);
            gv_user.TabIndex = 0;
            gv_user.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grid_user });
            // 
            // grid_user
            // 
            grid_user.Appearance.ViewCaption.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            grid_user.Appearance.ViewCaption.Options.UseFont = true;
            grid_user.GridControl = gv_user;
            grid_user.Name = "grid_user";
            grid_user.OptionsView.ShowGroupPanel = false;
            grid_user.OptionsView.ShowViewCaption = true;
            grid_user.ViewCaption = "Danh Sách Tài Khoản";
            grid_user.RowClick += grid_user_RowClick;
            // 
            // panel2
            // 
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(panel3);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(596, 996);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(cb_delete);
            panel3.Controls.Add(tableLayoutPanel2);
            panel3.Controls.Add(cb_off);
            panel3.Controls.Add(cb_on);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(cb_Export);
            panel3.Controls.Add(cb_Import);
            panel3.Controls.Add(cb_role);
            panel3.Controls.Add(txt_password);
            panel3.Controls.Add(txt_username);
            panel3.Controls.Add(txt_fullname);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label1);
            panel3.Dock = System.Windows.Forms.DockStyle.Top;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(594, 447);
            panel3.TabIndex = 0;
            // 
            // cb_delete
            // 
            cb_delete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cb_delete.AutoSize = true;
            cb_delete.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            cb_delete.ForeColor = System.Drawing.Color.Crimson;
            cb_delete.Location = new System.Drawing.Point(156, 313);
            cb_delete.Name = "cb_delete";
            cb_delete.Size = new System.Drawing.Size(98, 23);
            cb_delete.TabIndex = 15;
            cb_delete.Text = "Xóa NVL";
            cb_delete.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(btn_delete, 2, 0);
            tableLayoutPanel2.Controls.Add(btn_edit, 1, 0);
            tableLayoutPanel2.Controls.Add(btn_add, 0, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanel2.Location = new System.Drawing.Point(0, 399);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(594, 48);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_delete
            // 
            btn_delete.BackColor = System.Drawing.Color.Crimson;
            btn_delete.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_delete.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_delete.ForeColor = System.Drawing.SystemColors.ButtonFace;
            btn_delete.Location = new System.Drawing.Point(296, 0);
            btn_delete.Margin = new System.Windows.Forms.Padding(0);
            btn_delete.Name = "btn_delete";
            btn_delete.Size = new System.Drawing.Size(148, 48);
            btn_delete.TabIndex = 2;
            btn_delete.Text = "Xóa";
            btn_delete.UseVisualStyleBackColor = false;
            btn_delete.Click += btn_delete_Click;
            // 
            // btn_edit
            // 
            btn_edit.BackColor = System.Drawing.Color.FromArgb(37, 171, 206);
            btn_edit.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_edit.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_edit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            btn_edit.Location = new System.Drawing.Point(148, 0);
            btn_edit.Margin = new System.Windows.Forms.Padding(0);
            btn_edit.Name = "btn_edit";
            btn_edit.Size = new System.Drawing.Size(148, 48);
            btn_edit.TabIndex = 1;
            btn_edit.Text = "Chỉnh Sửa";
            btn_edit.UseVisualStyleBackColor = false;
            btn_edit.Click += btn_edit_Click;
            // 
            // btn_add
            // 
            btn_add.BackColor = System.Drawing.Color.FromArgb(72, 143, 233);
            btn_add.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_add.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_add.ForeColor = System.Drawing.SystemColors.ButtonFace;
            btn_add.Location = new System.Drawing.Point(0, 0);
            btn_add.Margin = new System.Windows.Forms.Padding(0);
            btn_add.Name = "btn_add";
            btn_add.Size = new System.Drawing.Size(148, 48);
            btn_add.TabIndex = 0;
            btn_add.Text = "Thêm Mới";
            btn_add.UseVisualStyleBackColor = false;
            btn_add.Click += btn_add_Click;
            // 
            // cb_off
            // 
            cb_off.AutoSize = true;
            cb_off.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            cb_off.ForeColor = System.Drawing.Color.Crimson;
            cb_off.Location = new System.Drawing.Point(296, 356);
            cb_off.Name = "cb_off";
            cb_off.Size = new System.Drawing.Size(54, 23);
            cb_off.TabIndex = 14;
            cb_off.Text = "Off";
            cb_off.UseVisualStyleBackColor = true;
            // 
            // cb_on
            // 
            cb_on.AutoSize = true;
            cb_on.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            cb_on.Location = new System.Drawing.Point(159, 356);
            cb_on.Name = "cb_on";
            cb_on.Size = new System.Drawing.Size(53, 23);
            cb_on.TabIndex = 13;
            cb_on.Text = "On";
            cb_on.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label6.Location = new System.Drawing.Point(34, 360);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(99, 19);
            label6.TabIndex = 12;
            label6.Text = "Trạng Thái :";
            // 
            // cb_Export
            // 
            cb_Export.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cb_Export.AutoSize = true;
            cb_Export.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            cb_Export.Location = new System.Drawing.Point(283, 269);
            cb_Export.Name = "cb_Export";
            cb_Export.Size = new System.Drawing.Size(100, 23);
            cb_Export.TabIndex = 10;
            cb_Export.Text = "Xuất Kho";
            cb_Export.UseVisualStyleBackColor = true;
            // 
            // cb_Import
            // 
            cb_Import.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cb_Import.AutoSize = true;
            cb_Import.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            cb_Import.Location = new System.Drawing.Point(154, 273);
            cb_Import.Name = "cb_Import";
            cb_Import.Size = new System.Drawing.Size(103, 23);
            cb_Import.TabIndex = 9;
            cb_Import.Text = "Nhập Kho";
            cb_Import.UseVisualStyleBackColor = true;
            // 
            // cb_role
            // 
            cb_role.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cb_role.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cb_role.FormattingEnabled = true;
            cb_role.Location = new System.Drawing.Point(156, 214);
            cb_role.Name = "cb_role";
            cb_role.Size = new System.Drawing.Size(418, 34);
            cb_role.TabIndex = 8;
            // 
            // txt_password
            // 
            txt_password.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txt_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_password.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txt_password.Location = new System.Drawing.Point(156, 154);
            txt_password.Name = "txt_password";
            txt_password.Size = new System.Drawing.Size(418, 34);
            txt_password.TabIndex = 7;
            // 
            // txt_username
            // 
            txt_username.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txt_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_username.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txt_username.Location = new System.Drawing.Point(159, 95);
            txt_username.Name = "txt_username";
            txt_username.Size = new System.Drawing.Size(415, 34);
            txt_username.TabIndex = 6;
            // 
            // txt_fullname
            // 
            txt_fullname.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txt_fullname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt_fullname.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txt_fullname.Location = new System.Drawing.Point(159, 30);
            txt_fullname.Name = "txt_fullname";
            txt_fullname.Size = new System.Drawing.Size(415, 34);
            txt_fullname.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(26, 273);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(107, 19);
            label5.TabIndex = 4;
            label5.Text = "Phân Quyền :";
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(79, 229);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(54, 19);
            label4.TabIndex = 3;
            label4.Text = "Role :";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(42, 169);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(91, 19);
            label3.TabIndex = 2;
            label3.Text = "Mật Khẩu :";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(3, 102);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(130, 19);
            label2.TabIndex = 1;
            label2.Text = "Tên Đăng Nhập :";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(61, 45);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 19);
            label1.TabIndex = 0;
            label1.Text = "Họ Tên :";
            // 
            // uc_user
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Name = "uc_user";
            Size = new System.Drawing.Size(1922, 1021);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gv_user).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_user).EndInit();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gv_user;
        private DevExpress.XtraGrid.Views.Grid.GridView grid_user;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cb_Export;
        private System.Windows.Forms.CheckBox cb_Import;
        private System.Windows.Forms.ComboBox cb_role;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_fullname;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.CheckBox cb_off;
        private System.Windows.Forms.CheckBox cb_on;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cb_delete;
    }
}
