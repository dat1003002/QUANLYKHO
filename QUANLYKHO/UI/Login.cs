using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using System.Threading.Tasks;
using System.Linq;
using QUANLYKHO.model;
using BCrypt.Net;

namespace QUANLYKHO.UI
{
    public partial class Login : Form
    {
        private Panel pnlLoading;
        private Label lblLoading;
        private ProgressBar pbLoading;
        private bool _loadingShown = false;

        public Login()
        {
            InitializeComponent();
            InitializeLoadingPanel();

            // Cấu hình form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập - Quản lý kho";

            // Quan trọng: Bấm Enter bất kỳ đâu trên form → kích hoạt nút Login
            this.AcceptButton = btn_login;

            // Focus vào username khi form load
            this.Load += (s, e) => txt_username?.Focus();

            // Xử lý Enter chi tiết trên các textbox (tăng trải nghiệm người dùng)
            txt_username.KeyDown += TxtUsername_KeyDown;
            txt_password.KeyDown += TxtPassword_KeyDown;
        }

        private void TxtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng bíp và hành vi mặc định
                txt_password?.Focus();     // Chuyển sang ô mật khẩu
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btn_login.PerformClick();   // Kích hoạt đăng nhập
            }
        }

        private void InitializeLoadingPanel()
        {
            pnlLoading = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(180, 240, 240, 240),
                Visible = false
            };

            lblLoading = new Label
            {
                Text = "Đang đăng nhập...",
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            pbLoading = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                Width = 300,
                Height = 25,
                MarqueeAnimationSpeed = 30
            };

            pnlLoading.Controls.Add(lblLoading);
            pnlLoading.Controls.Add(pbLoading);
            this.Controls.Add(pnlLoading);
            pnlLoading.BringToFront();

            this.SizeChanged += (s, e) => CenterLoading();
        }

        private void CenterLoading()
        {
            if (!_loadingShown) return;

            lblLoading.Location = new Point(
                (ClientSize.Width - lblLoading.Width) / 2,
                (ClientSize.Height - lblLoading.Height - pbLoading.Height - 30) / 2);

            pbLoading.Location = new Point(
                (ClientSize.Width - pbLoading.Width) / 2,
                lblLoading.Bottom + 20);
        }

        private void ShowLoading(bool show)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowLoading(show)));
                return;
            }

            _loadingShown = show;
            pnlLoading.Visible = show;

            if (show)
            {
                CenterLoading();
                pbLoading.MarqueeAnimationSpeed = 30;
            }
            else
            {
                pbLoading.MarqueeAnimationSpeed = 0;
            }
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            string username = txt_username?.Text?.Trim() ?? "";
            string password = txt_password?.Text ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShowLoading(true);
            User authenticatedUser = null;
            string errorMessage = null;

            try
            {
                await Task.Run(() =>
                {
                    using var db = new QLKhoDbContext();

                    var foundUser = db.Users
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Username == username);

                    if (foundUser == null &&
                        username.Equals("admin", StringComparison.OrdinalIgnoreCase) &&
                        password == "admin123")
                    {
                        var admin = new User
                        {
                            Username = "admin",
                            FullName = "Administrator",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                            Role = "Admin",
                            IsActive = true,
                            IsSuperAdmin = true,
                            CanImport = true,
                            CanExport = true,
                            CanCreateInventoryCheck = true,
                            CanApproveInventoryCheck = true
                        };
                        db.Users.Add(admin);
                        db.SaveChanges();
                        authenticatedUser = admin;
                    }
                    else
                    {
                        authenticatedUser = foundUser;
                    }
                });

                if (authenticatedUser == null || !authenticatedUser.IsActive)
                {
                    errorMessage = "Tài khoản không tồn tại hoặc đã bị khóa!";
                    return;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, authenticatedUser.PasswordHash))
                {
                    errorMessage = "Mật khẩu không đúng!";
                    return;
                }

                this.Hide();

                var home = new Home
                {
                    CurrentUserId = authenticatedUser.Id,
                    FullName = authenticatedUser.FullName ?? username,
                    CurrentUserRole = authenticatedUser.Role,
                    IsSuperAdmin = authenticatedUser.IsSuperAdmin,
                    CanImport = authenticatedUser.CanImport,
                    CanExport = authenticatedUser.CanExport,
                    CanCreateInventoryCheck = authenticatedUser.CanCreateInventoryCheck,
                    CanApproveInventoryCheck = authenticatedUser.CanApproveInventoryCheck
                };

                home.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                errorMessage = $"Có lỗi xảy ra khi đăng nhập:\n{ex.Message}";
            }
            finally
            {
                ShowLoading(false);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(errorMessage, "Đăng nhập thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}