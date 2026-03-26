using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.model;
using QUANLYKHO.Data;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;

namespace QUANLYKHO.UI
{
    public partial class uc_dieutrakho : UserControl
    {
        private int _currentFactoryId = 0;

        public int CurrentFactoryId
        {
            get => _currentFactoryId;
            set
            {
                _currentFactoryId = value;
                LoadUsersToComboBox();
                LoadCategories();
                RefreshData();
            }
        }

        public int CurrentUserId { get; set; } = 0;

        public uc_dieutrakho()
        {
            InitializeComponent();
        }

        private void uc_dieutrakho_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                LoadUsersToComboBox();
                LoadCategories();
            }
        }

        private void LoadUsersToComboBox()
        {
            if (cb_user == null) return;

            try
            {
                using var db = new QLKhoDbContext();

                var users = (from u in db.Users.AsNoTracking()
                             join uf in db.UserFactories.AsNoTracking() on u.Id equals uf.UserId
                             where uf.FactoryId == CurrentFactoryId
                                   && u.IsActive
                                   && !u.FullName.Trim().ToUpper().Contains("ADMINISTRATOR")
                             orderby u.FullName
                             select new { u.Id, u.FullName })
                            .Distinct()
                            .ToList();

                users.Insert(0, new { Id = 0, FullName = "Người Phê Duyệt" });

                cb_user.DataSource = null;
                cb_user.DataSource = users;
                cb_user.DisplayMember = "FullName";
                cb_user.ValueMember = "Id";
                cb_user.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách người dùng:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            if (cb_category == null) return;
            try
            {
                using var db = new QLKhoDbContext();
                var categories = db.Categories
                    .AsNoTracking()
                    .Select(c => new { c.Id, c.Name })
                    .OrderBy(c => c.Name)
                    .ToList();

                cb_category.Properties.DataSource = categories;
                cb_category.Properties.DisplayMember = "Name";
                cb_category.Properties.ValueMember = "Id";
                cb_category.Properties.SeparatorChar = ';';
                cb_category.Properties.AllowMultiSelect = true;
                cb_category.Properties.NullText = "Chọn danh mục...";
                cb_category.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData(int createdById = 0)
        {
            if (CurrentFactoryId <= 0)
            {
                gridControlInventory.DataSource = null;
                return;
            }
            if (gridControlInventory == null || gridViewInventory == null)
            {
                MessageBox.Show("GridControl hoặc GridView chưa khởi tạo.", "Lỗi giao diện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using var db = new QLKhoDbContext();
                var query = db.InventoryChecks
                    .AsNoTracking()
                    .Where(ic => ic.FactoryId == CurrentFactoryId);
                var checks = query
                    .Include(ic => ic.CreatedBy)
                    .Include(ic => ic.ApprovedBy)
                    .OrderByDescending(ic => ic.CheckDate)
                    .Select(ic => new
                    {
                        ic.Id,
                        ic.CheckCode,
                        ic.CheckDate,
                        ic.CutOffDate,
                        CreatedBy = ic.CreatedBy != null ? ic.CreatedBy.FullName : "N/A",
                        ApprovedStatus = ic.IsApproved ? "Đã duyệt" : "Chưa duyệt",
                        ApprovedDate = ic.ApprovedDate,
                        ApprovedBy = ic.ApprovedBy != null ? ic.ApprovedBy.FullName : null,
                        DetailCount = ic.Details.Count
                    })
                    .ToList();
                gridControlInventory.DataSource = checks;
                if (gridViewInventory.Columns.Count > 0)
                {
                    ConfigureGridColumns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu kiểm kê:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGridColumns()
        {
            var gv = gridViewInventory;
            if (gv == null || gv.Columns.Count == 0) return;
            gv.ApplyHeader();
            gv.OptionsView.ShowIndicator = false;
            gv.OptionsBehavior.Editable = false;

            var idColumn = gv.Columns["Id"];
            if (idColumn != null)
            {
                idColumn.Visible = false;
            }

            gv.Columns["CheckCode"]?.SetColumnProperties("Mã phiếu", 150, HorzAlignment.Center);
            if (gv.Columns["CheckCode"] != null)
            {
                gv.Columns["CheckCode"].AppearanceCell.Font = new Font(gv.Appearance.Row.Font, FontStyle.Bold);
                gv.Columns["CheckCode"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }
            gv.Columns["CheckDate"]?.SetDateTimeColumn("Ngày kiểm kê", "dd/MM/yyyy HH:mm", 140);
            gv.Columns["CutOffDate"]?.SetDateTimeColumn("Ngày cắt số liệu", "dd/MM/yyyy", 110);
            gv.Columns["CreatedBy"]?.SetColumnProperties("Người tạo", 140);
            gv.Columns["ApprovedStatus"]?.SetColumnProperties("Trạng thái", 100, HorzAlignment.Center);
            gv.Columns["ApprovedDate"]?.SetDateTimeColumn("Ngày duyệt", "dd/MM/yyyy HH:mm", 140);
            gv.Columns["ApprovedBy"]?.SetColumnProperties("Người duyệt", 140);
            gv.Columns["DetailCount"]?.SetColumnProperties("SL mặt hàng", 90, HorzAlignment.Center);

            gv.Columns["CheckCode"].VisibleIndex = 0;

            gv.CustomDrawCell -= GridViewInventory_CustomDrawCell;
            gv.CustomDrawCell += GridViewInventory_CustomDrawCell;
        }

        private void GridViewInventory_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column?.FieldName == "ApprovedStatus")
            {
                var status = e.CellValue?.ToString() ?? "";
                if (status == "Đã duyệt")
                {
                    e.Appearance.ForeColor = Color.DarkGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                else if (status == "Chưa duyệt")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        private void cb_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_user.SelectedValue is int id)
            {
                Console.WriteLine($"Đã chọn người phê duyệt Id: {id}");
            }
        }

        private void btn_addkiemkho_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_CheckCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã phiếu kiểm kê.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_CheckCode.Focus();
                return;
            }

            if (date_CutOffDate.Value == DateTime.MinValue)
            {
                MessageBox.Show("Vui lòng chọn ngày cắt số liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                date_CutOffDate.Focus();
                return;
            }

            if (cb_user.SelectedValue is not int approvedById || approvedById <= 0)
            {
                MessageBox.Show("Vui lòng chọn người phê duyệt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_user.Focus();
                return;
            }

            if (CurrentFactoryId <= 0)
            {
                MessageBox.Show("Chưa chọn nhà máy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string checkCode = txt_CheckCode.Text.Trim();

            try
            {
                using var db = new QLKhoDbContext();

                if (db.InventoryChecks.Any(ic => ic.FactoryId == CurrentFactoryId && ic.CheckCode == checkCode))
                {
                    MessageBox.Show($"Mã phiếu '{checkCode}' đã tồn tại.", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_CheckCode.Focus();
                    txt_CheckCode.SelectAll();
                    return;
                }

                var checkedItems = cb_category.Properties.GetCheckedItems() as string;
                List<int> selectedCategoryIds = new List<int>();

                if (!string.IsNullOrEmpty(checkedItems))
                {
                    var ids = checkedItems.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var idStr in ids)
                    {
                        if (int.TryParse(idStr, out int id))
                            selectedCategoryIds.Add(id);
                    }
                }

                var productsQuery = db.Products.AsNoTracking()
                    .Where(p => p.FactoryId == CurrentFactoryId);

                if (selectedCategoryIds.Any())
                {
                    productsQuery = productsQuery.Where(p => selectedCategoryIds.Contains(p.CategoryId));
                }

                var products = productsQuery
                    .OrderBy(p => p.Id)
                    .Select(p => new { p.Id, p.CurrentQuantity })
                    .ToList();

                if (products.Count == 0)
                {
                    MessageBox.Show(
                        selectedCategoryIds.Any()
                            ? "Không có sản phẩm nào thuộc danh mục đã chọn."
                            : "Nhà máy chưa có sản phẩm nào.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var newCheck = new InventoryCheck
                {
                    CheckCode = checkCode,
                    CheckDate = DateTime.Now,
                    CutOffDate = date_CutOffDate.Value.Date,
                    CreatedById = CurrentUserId,
                    FactoryId = CurrentFactoryId,
                    Note = "",
                    IsApproved = false,
                    ApprovedById = approvedById,
                    ApprovedDate = null,
                    Details = new List<InventoryCheckDetail>()
                };

                foreach (var product in products)
                {
                    newCheck.Details.Add(new InventoryCheckDetail
                    {
                        ProductId = product.Id,
                        SystemQuantity = product.CurrentQuantity,
                        ActualQuantity = 0,
                        Note = null
                    });
                }

                db.InventoryChecks.Add(newCheck);
                db.SaveChanges();

                MessageBox.Show($"Tạo phiếu thành công!\n" +
                                $"Mã phiếu: {checkCode}\n" +
                                $"Số mặt hàng: {products.Count}" +
                                (selectedCategoryIds.Any() ? $"\nDanh mục đã chọn: {selectedCategoryIds.Count}" : "\nToàn bộ sản phẩm"),
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txt_CheckCode.Clear();
                date_CutOffDate.Value = DateTime.Today;
                cb_user.SelectedIndex = 0;

                for (int i = 0; i < cb_category.Properties.Items.Count; i++)
                {
                    cb_category.Properties.Items[i].CheckState = CheckState.Unchecked;
                }

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phiếu:\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Detail_Click(object sender, EventArgs e)
        {
            if (gridViewInventory == null || gridViewInventory.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu kiểm kê.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gridViewInventory.GetFocusedRowCellValue("Id") is int checkId && checkId > 0)
            {
                try
                {
                    var frm = new Chitietkiemkho(checkId, CurrentUserId);
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi mở chi tiết:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không thể xác định phiếu được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_exportexecl_Click(object sender, EventArgs e)
        {
            if (gridViewInventory == null || gridViewInventory.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu để xuất Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gridViewInventory.GetFocusedRowCellValue("CheckCode") is not string selectedCheckCode || string.IsNullOrEmpty(selectedCheckCode))
            {
                MessageBox.Show("Không thể xác định mã phiếu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using var db = new QLKhoDbContext();
                var check = db.InventoryChecks
                    .Include(ic => ic.CreatedBy)
                    .Include(ic => ic.ApprovedBy)
                    .Include(ic => ic.Details)
                        .ThenInclude(d => d.Product)
                            .ThenInclude(p => p.Unit)
                    .FirstOrDefault(ic => ic.FactoryId == CurrentFactoryId && ic.CheckCode == selectedCheckCode);
                if (check == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu kiểm kê.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Xuất phiếu kiểm kê ra Excel",
                    FileName = $"PhieuKiemKe_{check.CheckCode}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                };
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(new FileInfo(saveFileDialog.FileName));
                var worksheet = package.Workbook.Worksheets.Add("Chi tiết kiểm kê");
                worksheet.Cells[1, 1].Value = $"PHIẾU KIỂM KÊ KHO - {check.CheckCode}";
                worksheet.Cells[1, 1, 1, 7].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                int row = 3;
                worksheet.Cells[row, 1].Value = "Ngày kiểm kê:"; worksheet.Cells[row++, 2].Value = check.CheckDate.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[row, 1].Value = "Ngày cắt số liệu:"; worksheet.Cells[row++, 2].Value = check.CutOffDate.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 1].Value = "Người tạo:"; worksheet.Cells[row++, 2].Value = check.CreatedBy?.FullName ?? "N/A";
                worksheet.Cells[row, 1].Value = "Trạng thái:"; worksheet.Cells[row++, 2].Value = check.IsApproved ? "ĐÃ DUYỆT" : "CHƯA DUYỆT";
                worksheet.Cells[row, 1].Value = "Người duyệt:"; worksheet.Cells[row++, 2].Value = check.ApprovedBy?.FullName ?? "";
                worksheet.Cells[row, 1].Value = "Ngày duyệt:"; worksheet.Cells[row++, 2].Value = check.ApprovedDate?.ToString("dd/MM/yyyy HH:mm") ?? "";
                row += 2;
                string[] headers = { "STT", "Mã NVL", "Tên sản phẩm", "Đơn vị", "SL hệ thống", "SL thực tế", "Chênh lệch" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[row, i + 1].Value = headers[i];
                    worksheet.Cells[row, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                int stt = 1;
                foreach (var detail in check.Details.OrderBy(d => d.Product?.Name))
                {
                    row++;
                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 2].Value = detail.Product?.MaterialCode ?? "N/A";
                    worksheet.Cells[row, 3].Value = detail.Product?.Name ?? "N/A";
                    worksheet.Cells[row, 4].Value = detail.Product?.Unit?.Name ?? "";
                    worksheet.Cells[row, 5].Value = detail.SystemQuantity;
                    worksheet.Cells[row, 6].Value = detail.ActualQuantity;
                    worksheet.Cells[row, 7].Value = detail.Variance;
                    worksheet.Cells[row, 5, row, 7].Style.Numberformat.Format = "#,##0";
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.Save();
                MessageBox.Show($"Đã xuất file Excel thành công!\n{saveFileDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = saveFileDialog.FileName,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cb_category_EditValueChanged(object sender, EventArgs e)
        {
            var checkedItems = cb_category.Properties.GetCheckedItems() as string;
            List<int> selectedCategoryIds = new List<int>();

            if (!string.IsNullOrEmpty(checkedItems))
            {
                var ids = checkedItems.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var idStr in ids)
                {
                    if (int.TryParse(idStr, out int id))
                        selectedCategoryIds.Add(id);
                }
            }

            // Ví dụ: lưu tạm danh sách Id vào biến private, hoặc refresh grid sản phẩm ở đây
            // _selectedCategoryIds = selectedCategoryIds;
            // RefreshProductGrid(_selectedCategoryIds);

            // Hoặc chỉ cần debug xem danh mục đã chọn
            Console.WriteLine("Danh mục đã chọn: " + string.Join(", ", selectedCategoryIds));
        }

    }

    internal static class GridColumnExtensions
    {
        public static void SetColumnProperties(this DevExpress.XtraGrid.Columns.GridColumn col, string caption, int width, HorzAlignment alignment = HorzAlignment.Near)
        {
            if (col == null) return;
            col.Caption = caption;
            col.Width = width;
            col.AppearanceCell.TextOptions.HAlignment = alignment;
        }

        public static void SetDateTimeColumn(this DevExpress.XtraGrid.Columns.GridColumn col, string caption, string format, int width)
        {
            if (col == null) return;
            col.Caption = caption;
            col.Width = width;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            col.DisplayFormat.FormatString = format;
        }
    }
}