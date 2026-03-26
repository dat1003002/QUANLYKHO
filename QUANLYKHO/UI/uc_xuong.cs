using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using QUANLYKHO.Data;
using QUANLYKHO.model;
using DevExpress.XtraGrid.Columns;

namespace QUANLYKHO.UI
{
    public partial class uc_xuong : UserControl
    {
        private Dictionary<int, HashSet<int>> originalUserFactories = new Dictionary<int, HashSet<int>>();

        public uc_xuong()
        {
            InitializeComponent();
            LoadFactoryData();
            LoadUserWithFactoryCheckbox();
            this.VisibleChanged += uc_xuong_VisibleChanged;
        }

        private void uc_xuong_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                LoadUserWithFactoryCheckbox();
        }

        private void LoadFactoryData()
        {
            using (var db = new QLKhoDbContext())
            {
                var factories = db.Factories
                    .Select(f => new
                    {
                        f.Id,
                        f.Code,
                        f.Name,
                        f.Description,
                        Trạng_thái = f.IsActive ? "Hoạt động" : "Ngừng hoạt động",
                        f.CreatedAt
                    })
                    .ToList();

                gridControlfactory.DataSource = factories;
                gridViewfactory.Columns["Id"].Visible = false;
                gridViewfactory.Columns["Code"].Caption = "Mã xưởng";
                gridViewfactory.Columns["Name"].Caption = "Tên xưởng";
                gridViewfactory.Columns["Description"].Caption = "Mô tả";
                gridViewfactory.Columns["Trạng_thái"].Caption = "Trạng thái";
                gridViewfactory.Columns["CreatedAt"].Caption = "Ngày tạo";
                gridViewfactory.OptionsView.ShowIndicator = false;
                gridViewfactory.ApplyHeader();
                gridViewfactory.BestFitColumns();
            }
        }

        public void LoadUserWithFactoryCheckbox()
        {
            using (var db = new QLKhoDbContext())
            {
                var factories = db.Factories
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.Code)
                    .ToList();

                var users = db.Users
                    .OrderBy(u => u.Username)
                    .ToList();

                originalUserFactories.Clear();
                var userFactoryDict = db.UserFactories
                    .GroupBy(uf => uf.UserId)
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            var set = g.Select(uf => uf.FactoryId).ToHashSet();
                            originalUserFactories[g.Key] = new HashSet<int>(set);
                            return set;
                        }
                    );

                var data = new List<ExpandoObject>();

                foreach (var user in users)
                {
                    dynamic row = new ExpandoObject();
                    var dict = (IDictionary<string, object?>)row;

                    dict["Id"] = user.Id;
                    dict["Tên_đăng_nhập"] = user.Username;
                    dict["Họ_và_tên"] = user.FullName ?? string.Empty;

                    var hasAccessSet = userFactoryDict.GetValueOrDefault(user.Id, new HashSet<int>());

                    foreach (var factory in factories)
                    {
                        string colKey = $"F_{factory.Id}";
                        dict[colKey] = hasAccessSet.Contains(factory.Id);
                    }

                    data.Add(row);
                }

                gridControlUserFactory.DataSource = data;
                gridControlUserFactory.RefreshDataSource();

                var view = gridViewUserFactory;
                view.Columns.Clear();

                var colId = view.Columns.AddVisible("Id");
                colId.Visible = false;
                colId.OptionsColumn.ReadOnly = true;

                var colUsername = view.Columns.AddVisible("Tên_đăng_nhập");
                colUsername.Caption = "TÊN ĐĂNG NHẬP";
                colUsername.Width = 150;

                var colFullName = view.Columns.AddVisible("Họ_và_tên");
                colFullName.Caption = "HỌ VÀ TÊN";
                colFullName.Width = 180;

                foreach (var factory in factories)
                {
                    var col = view.Columns.AddVisible($"F_{factory.Id}");
                    col.Caption = factory.Name;
                    col.Width = 100;
                    col.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    col.OptionsColumn.ReadOnly = false;

                    var repo = new RepositoryItemCheckEdit
                    {
                        ValueChecked = true,
                        ValueUnchecked = false,
                        AllowGrayed = false,
                        NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked,
                        CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Standard,
                        AutoWidth = true
                    };
                    col.ColumnEdit = repo;
                }

                view.RefreshData();
                gridControlUserFactory.Refresh();
                view.OptionsView.ShowIndicator = false;
                view.BestFitColumns();
                view.ApplyHeader();
            }
        }

        private void btn_phanquyen_Click(object sender, EventArgs e)
        {
            using (var db = new QLKhoDbContext())
            {
                bool hasChanges = false;

                for (int rowHandle = 0; rowHandle < gridViewUserFactory.RowCount; rowHandle++)
                {
                    int userId = Convert.ToInt32(gridViewUserFactory.GetRowCellValue(rowHandle, "Id"));

                    originalUserFactories.TryGetValue(userId, out var originalSet);
                    originalSet ??= new HashSet<int>();

                    foreach (GridColumn col in gridViewUserFactory.Columns)
                    {
                        if (!col.FieldName.StartsWith("F_")) continue;

                        string factoryStr = col.FieldName.Substring(2);
                        if (!int.TryParse(factoryStr, out int factoryId)) continue;

                        bool currentChecked = Convert.ToBoolean(gridViewUserFactory.GetRowCellValue(rowHandle, col.FieldName));
                        bool originallyChecked = originalSet.Contains(factoryId);

                        if (currentChecked && !originallyChecked)
                        {
                            if (!db.UserFactories.Any(uf => uf.UserId == userId && uf.FactoryId == factoryId))
                            {
                                var newUf = new UserFactory
                                {
                                    UserId = userId,
                                    FactoryId = factoryId,
                                    IsDefault = false,
                                    AssignedAt = DateTime.UtcNow,
                                    AssignedByUsername = "Admin",
                                    CanManageProducts = true,
                                    CanImport = true,
                                    CanExport = true,
                                    CanApproveInventoryCheck = true
                                };
                                db.UserFactories.Add(newUf);
                                hasChanges = true;
                            }
                        }
                        else if (!currentChecked && originallyChecked)
                        {
                            var ufToRemove = db.UserFactories
                                .FirstOrDefault(uf => uf.UserId == userId && uf.FactoryId == factoryId);
                            if (ufToRemove != null)
                            {
                                db.UserFactories.Remove(ufToRemove);
                                hasChanges = true;
                            }
                        }
                    }
                }

                if (hasChanges)
                {
                    db.SaveChanges();
                    MessageBox.Show("Đã lưu phân quyền thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserWithFactoryCheckbox();
                }
                else
                {
                    MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void gridViewfactory_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                txt_code.Text = gridViewfactory.GetFocusedRowCellValue("Code")?.ToString() ?? "";
                txt_name.Text = gridViewfactory.GetFocusedRowCellValue("Name")?.ToString() ?? "";
                txt_Description.Text = gridViewfactory.GetFocusedRowCellValue("Description")?.ToString() ?? "";
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_code.Text) || string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Mã xưởng và Tên xưởng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new QLKhoDbContext())
            {
                if (db.Factories.Any(f => f.Name.Trim().ToLower() == txt_name.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Tên xưởng đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (db.Factories.Any(f => f.Code.Trim().ToLower() == txt_code.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Mã xưởng đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var factory = new Factory
                {
                    Code = txt_code.Text.Trim(),
                    Name = txt_name.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(txt_Description.Text) ? null : txt_Description.Text.Trim(),
                    IsActive = true
                };

                db.Factories.Add(factory);
                db.SaveChanges();

                MessageBox.Show("Thêm xưởng mới thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txt_code.Clear();
                txt_name.Clear();
                txt_Description.Clear();

                LoadFactoryData();
                LoadUserWithFactoryCheckbox();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (gridViewfactory.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn xưởng cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(gridViewfactory.GetFocusedRowCellValue("Id"));

            if (string.IsNullOrWhiteSpace(txt_code.Text) || string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Mã xưởng và Tên xưởng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new QLKhoDbContext())
            {
                var factory = db.Factories.Find(id);
                if (factory == null) return;

                if (db.Factories.Any(f => f.Id != id && f.Name.Trim().ToLower() == txt_name.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Tên xưởng đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (db.Factories.Any(f => f.Id != id && f.Code.Trim().ToLower() == txt_code.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Mã xưởng đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                factory.Code = txt_code.Text.Trim();
                factory.Name = txt_name.Text.Trim();
                factory.Description = string.IsNullOrWhiteSpace(txt_Description.Text) ? null : txt_Description.Text.Trim();

                db.SaveChanges();

                MessageBox.Show("Cập nhật xưởng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txt_code.Clear();
                txt_name.Clear();
                txt_Description.Clear();

                LoadFactoryData();
                LoadUserWithFactoryCheckbox();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (gridViewfactory.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn xưởng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(gridViewfactory.GetFocusedRowCellValue("Id"));
            string tenXuong = gridViewfactory.GetFocusedRowCellValue("Name")?.ToString() ?? "";

            if (MessageBox.Show($"Bạn có chắc muốn xóa xưởng \"{tenXuong}\" không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (var db = new QLKhoDbContext())
            {
                bool coDuLieu = db.Warehouses.Any(w => w.FactoryId == id) ||
                                db.Products.Any(p => p.FactoryId == id) ||
                                db.Imports.Any(i => i.FactoryId == id) ||
                                db.Exports.Any(ex => ex.FactoryId == id) ||
                                db.InventoryChecks.Any(ic => ic.FactoryId == id);

                if (coDuLieu)
                {
                    MessageBox.Show("Không thể xóa!\n\nXưởng này đang có kho, sản phẩm, phiếu nhập/xuất hoặc kiểm kê.\n\nVui lòng xóa hết dữ liệu liên quan trước.",
                        "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var factory = db.Factories.Find(id);
                if (factory != null)
                {
                    db.Factories.Remove(factory);
                    db.SaveChanges();

                    MessageBox.Show("Xóa xưởng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadFactoryData();
                    LoadUserWithFactoryCheckbox();
                }
            }
        }
    }
}