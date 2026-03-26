using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using QUANLYKHO.Data;
using QUANLYKHO.model;
using BCrypt.Net;

namespace QUANLYKHO.UI
{
    public partial class uc_user : UserControl
    {
        public uc_user()
        {
            InitializeComponent();
            InitComboBox();
            grid_user.ApplyHeader();
            LoadData();
        }

        private void InitComboBox()
        {
            cb_role.Items.Add("Admin");
            cb_role.Items.Add("User");
            cb_role.SelectedIndex = 0;
        }

        private void LoadData()
        {
            try
            {
                using (var db = new QLKhoDbContext())
                {
                    var users = db.Users
                        .Select(u => new
                        {
                            Mã = u.Id,
                            Tên_đăng_nhập = u.Username,
                            Họ_tên = u.FullName,
                            Vai_trò = u.Role,
                            Nhập_kho = u.CanImport,
                            Xuất_kho = u.CanExport,
                            Xóa_sản_phẩm = u.CanDeleteProduct,
                            Trạng_thái = u.IsActive ? "Đang sử dụng" : "Ngưng sử dụng"
                        })
                        .ToList();

                    gv_user.DataSource = users;

                    grid_user.Columns["Mã"].Visible = false;
                    grid_user.Columns["Tên_đăng_nhập"].Width = 120;
                    grid_user.Columns["Họ_tên"].Width = 180;
                    grid_user.Columns["Vai_trò"].Width = 100;
                    grid_user.Columns["Nhập_kho"].Width = 80;
                    grid_user.Columns["Xuất_kho"].Width = 80;
                    grid_user.Columns["Xóa_sản_phẩm"].Width = 100;
                    grid_user.Columns["Trạng_thái"].Width = 110;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cb_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();
            string fullname = txt_fullname.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nhập Username và Password", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải từ 6 ký tự trở lên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_password.Focus();
                return;
            }

            btn_add.Enabled = false;

            try
            {
                using (var db = new QLKhoDbContext())
                {
                    if (db.Users.Any(u => u.Username == username))
                    {
                        MessageBox.Show("Username đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var user = new User
                    {
                        Username = username,
                        FullName = fullname,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12),
                        Role = cb_role.SelectedItem?.ToString() ?? "User",
                        CanImport = cb_Import.Checked,
                        CanExport = cb_Export.Checked,
                        CanDeleteProduct = cb_delete.Checked,
                        IsActive = true
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();

                    txt_username.Clear();
                    txt_fullname.Clear();
                    txt_password.Clear();
                    cb_role.SelectedIndex = 0;
                    cb_Import.Checked = false;
                    cb_Export.Checked = false;
                    cb_delete.Checked = false;
                    txt_username.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_add.Enabled = true;
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_user.FocusedRowHandle < 0)
            {
                MessageBox.Show("Chọn người dùng cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = grid_user.GetFocusedRowCellValue("Mã") as int?;
            if (!selected.HasValue) return;

            int userId = selected.Value;

            if (MessageBox.Show("Xác nhận xóa người dùng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (var db = new QLKhoDbContext())
                {
                    var user = db.Users.Find(userId);
                    if (user == null)
                    {
                        MessageBox.Show("Không tìm thấy người dùng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();

                    MessageBox.Show("Xóa thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            GridView view = grid_user;
            if (view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Chọn người dùng cần sửa");
                return;
            }

            var idObj = view.GetRowCellValue(view.FocusedRowHandle, "Mã");
            if (idObj == null) return;

            int userId = Convert.ToInt32(idObj);

            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();
            string fullname = txt_fullname.Text.Trim();

            try
            {
                using (var db = new QLKhoDbContext())
                {
                    var user = db.Users.Find(userId);
                    if (user == null)
                    {
                        MessageBox.Show("Không tìm thấy user");
                        return;
                    }

                    user.Username = username;
                    user.FullName = fullname;
                    user.Role = cb_role.SelectedItem?.ToString() ?? "User";
                    user.CanImport = cb_Import.Checked;
                    user.CanExport = cb_Export.Checked;
                    user.CanDeleteProduct = cb_delete.Checked;
                    user.IsActive = cb_on.Checked;

                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);
                    }

                    db.SaveChanges();

                    MessageBox.Show("Cập nhật thành công");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void grid_user_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            GridView view = sender as GridView;

            txt_username.Text = view.GetRowCellValue(e.RowHandle, "Tên_đăng_nhập")?.ToString();
            txt_fullname.Text = view.GetRowCellValue(e.RowHandle, "Họ_tên")?.ToString();
            txt_password.Clear();

            cb_role.SelectedItem = view.GetRowCellValue(e.RowHandle, "Vai_trò")?.ToString();
            cb_Import.Checked = Convert.ToBoolean(view.GetRowCellValue(e.RowHandle, "Nhập_kho"));
            cb_Export.Checked = Convert.ToBoolean(view.GetRowCellValue(e.RowHandle, "Xuất_kho"));
            cb_delete.Checked = Convert.ToBoolean(view.GetRowCellValue(e.RowHandle, "Xóa_sản_phẩm"));

            string status = view.GetRowCellValue(e.RowHandle, "Trạng_thái")?.ToString();
            if (status == "Đang sử dụng")
            {
                cb_on.Checked = true;
                cb_off.Checked = false;
            }
            else
            {
                cb_on.Checked = false;
                cb_off.Checked = true;
            }
        }
    }
}