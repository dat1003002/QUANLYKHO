using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using QUANLYKHO.model;

namespace QUANLYKHO.UI
{
    public partial class uc_nhapkho : UserControl
    {
        private int lastFocusedRow = -1;

        public int CurrentUserId { get; set; } = 1;
        public int CurrentFactoryId { get; set; } = 0;

        public uc_nhapkho()
        {
            InitializeComponent();

            LoadCategoryCombo();
            LoadUnitCombo();
            LoadLocationCombo();
            LoadProducts();
            LoadCategoryGrid();

            if (gv_product.MainView is GridView view)
            {
                view.FocusedRowChanged += GridView_FocusedRowChanged;
                view.DoubleClick += GridView_DoubleClick;
            }
            gridViewCategory.FocusedRowChanged += gridViewCategory_FocusedRowChanged;

            txt_soluong.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    e.Handled = true;
            };
        }
        public void RefreshData()
        {
            LoadLocationCombo();
            LoadProducts();
        }
        private void LoadCategoryCombo()
        {
            using var db = new QLKhoDbContext();
            cb_category.DataSource = db.Categories.Select(c => new { c.Id, c.Name }).ToList();
            cb_category.DisplayMember = "Name";
            cb_category.ValueMember = "Id";
            cb_category.SelectedIndex = -1;
        }

        private void LoadUnitCombo()
        {
            using var db = new QLKhoDbContext();
            cb_Unit.DataSource = db.Units.Select(u => new { u.Id, u.Name }).ToList();
            cb_Unit.DisplayMember = "Name";
            cb_Unit.ValueMember = "Id";
            cb_Unit.SelectedIndex = -1;
        }
        private void LoadLocationCombo()
        {
            if (CurrentFactoryId == 0)
            {
                cb_Location.DataSource = null;
                return;
            }

            using var db = new QLKhoDbContext();
            var locations = db.Locations
                .Include(l => l.Shelf).ThenInclude(s => s.Warehouse)
                .Where(l => l.Shelf.Warehouse.FactoryId == CurrentFactoryId)
                .Select(l => new
                {
                    l.Id,
                    DisplayName = l.Name
                })
                .OrderBy(x => x.DisplayName)
                .ToList();

            cb_Location.DataSource = locations;
            cb_Location.DisplayMember = "DisplayName";
            cb_Location.ValueMember = "Id";
            cb_Location.SelectedIndex = -1;
        }
        private void LoadCategoryGrid()
        {
            try
            {
                using var db = new QLKhoDbContext();
                var categories = db.Categories
                    .Select(c => new { c.Id, c.code, c.Name, c.Description })
                    .ToList();

                gridControlCategory.DataSource = categories;
                gridViewCategory.ApplyHeader();
                gridViewCategory.OptionsView.ShowIndicator = false;

                if (gridViewCategory.Columns["STT"] == null)
                {
                    var colSTT = gridViewCategory.Columns.AddVisible("STT");
                    colSTT.Caption = "STT";
                    colSTT.Width = 50;
                    colSTT.VisibleIndex = 0;
                    colSTT.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    colSTT.UnboundType = DevExpress.Data.UnboundColumnType.Integer;

                    gridViewCategory.CustomUnboundColumnData += (s, e) =>
                    {
                        if (e.Column.FieldName == "STT" && e.IsGetData)
                            e.Value = e.ListSourceRowIndex + 1;
                    };
                }

                gridViewCategory.Columns["Id"].Visible = false;
                gridViewCategory.Columns["code"].Caption = "Mã danh mục"; gridViewCategory.Columns["code"].Width = 120;
                gridViewCategory.Columns["Name"].Caption = "Tên danh mục"; gridViewCategory.Columns["Name"].Width = 250;
                gridViewCategory.Columns["Description"].Caption = "Mô tả"; gridViewCategory.Columns["Description"].Width = 250;
                gridViewCategory.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadProducts()
        {
            try
            {
                using var db = new QLKhoDbContext();

                var products = db.Products
                    .AsNoTracking()
                    .Include(p => p.Unit)
                    .Include(p => p.Location)
                    .Include(p => p.Category)
                    .Where(p => p.FactoryId == CurrentFactoryId)
                    .Select(p => new
                    {
                        p.Id,
                        p.MaterialCode,
                        p.Barcode,
                        p.Name,
                        p.Description,
                        p.Specification,
                        Unit = p.Unit.Name,
                        LocationId = p.LocationId,
                        LocationName = p.Location != null ? p.Location.Name : "Chưa phân vị trí",
                        Category = p.Category.code,
                        p.CurrentQuantity,
                        p.SafetyStock
                    })
                    .ToList();

                if (gv_product.MainView is not GridView view) return;

                gv_product.DataSource = products;

                view.OptionsView.ShowColumnHeaders = true;
                view.OptionsView.ShowIndicator = false;
                view.Columns.Clear();
                view.ApplyHeader();

                var colSTT = view.Columns.AddVisible("STT");
                colSTT.Caption = "STT";
                colSTT.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                colSTT.OptionsColumn.AllowEdit = false;
                colSTT.Width = 50;
                colSTT.VisibleIndex = 0;
                colSTT.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;

                view.Columns.AddVisible("MaterialCode").Caption = "Mã vật tư";
                view.Columns.AddVisible("Name").Caption = "Tên vật tư";
                view.Columns.AddVisible("Description").Caption = "Mô tả";
                view.Columns.AddVisible("Specification").Caption = "Quy cách";
                view.Columns.AddVisible("Unit").Caption = "Đơn vị";
                view.Columns.AddVisible("LocationName").Caption = "Vị trí";
                view.Columns.AddVisible("Category").Caption = "Phân Loại";

                var colQty = view.Columns.AddVisible("CurrentQuantity");
                colQty.Caption = "Tồn hiện tại";
                colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colQty.DisplayFormat.FormatString = "n0";

                var colSafety = view.Columns.AddVisible("SafetyStock");
                colSafety.Caption = "Tồn an toàn";
                colSafety.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colSafety.DisplayFormat.FormatString = "n0";

                ConfigureColumn(view, "Id", visible: false);
                ConfigureColumn(view, "LocationId", visible: false);
                ConfigureColumn(view, "Barcode", visible: false);

                view.CustomUnboundColumnData -= gv_product_CustomUnboundColumnData;
                view.CustomUnboundColumnData += gv_product_CustomUnboundColumnData;

                view.BestFitColumns();
                view.ClearSelection();
                view.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu sản phẩm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void gv_product_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "STT" && e.IsGetData)
            {
                e.Value = e.ListSourceRowIndex + 1;
            }
        }

        private static void ConfigureColumn(GridView view, string fieldName, string caption = null, int width = 0,
            HorzAlignment alignment = HorzAlignment.Default, bool visible = true)
        {
            var col = view.Columns[fieldName];
            if (col == null) return;
            if (caption != null) col.Caption = caption;
            if (width > 0) col.Width = width;
            col.Visible = visible;
            if (alignment != HorzAlignment.Default)
                col.AppearanceCell.TextOptions.HAlignment = alignment;
        }

        private void GridView_DoubleClick(object sender, EventArgs e) => ClearForm();
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0) return;

            if (sender is not GridView view) return;

            var row = view.GetRow(e.FocusedRowHandle) as dynamic;
            if (row == null) return;

            txt_MaterialCode.Text = row.MaterialCode ?? "";
            txt_Name.Text = row.Name ?? "";
            txt_Description.Text = row.Description ?? "";
            txt_Specification.Text = row.Specification ?? "";
            txt_SafetyStock.Text = Convert.ToDecimal(row.SafetyStock).ToString("0");
            cb_Unit.Text = row.Unit ?? "";
            cb_category.Text = row.Category ?? "";

            cb_Location.SelectedValue = row.LocationId ?? 0;
        }

        private void gridViewCategory_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {
                txt_code.Clear();
                txt_namecategory.Clear();
                return;
            }
            var row = gridViewCategory.GetRow(e.FocusedRowHandle) as dynamic;
            if (row == null) return;
            txt_code.Text = row.code ?? "";
            txt_namecategory.Text = row.Name ?? "";
        }
        private void ClearForm()
        {
            txt_MaterialCode.Clear();
            txt_Name.Clear();
            txt_Description.Clear();
            txt_Specification?.Clear();
            txt_SafetyStock.Clear();
            txt_soluong.Clear();
            cb_Unit.SelectedIndex = -1;
            cb_Location.SelectedIndex = -1;
            cb_category.SelectedIndex = -1;
        }
        private void btn_add_Click_1(object sender, EventArgs e)
        {
            if (CurrentFactoryId == 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà máy từ cbfactory trước khi thêm sản phẩm!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_MaterialCode.Text) ||
                string.IsNullOrWhiteSpace(txt_Name.Text) ||
                cb_Unit.SelectedValue == null ||
                cb_Location.SelectedValue == null ||
                cb_category.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc (Mã vật tư, Tên, Đơn vị, Vị trí, Danh mục)!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txt_soluong.Text.Trim(), out decimal soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập lần đầu phải là số lớn hơn 0!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_soluong.Focus();
                return;
            }

            string materialCode = txt_MaterialCode.Text.Trim();
            string specification = txt_Specification?.Text?.Trim();

            try
            {
                using var db = new QLKhoDbContext();

                bool existsInCurrentFactory = db.Products
                    .Any(p => p.MaterialCode == materialCode && p.FactoryId == CurrentFactoryId);

                if (existsInCurrentFactory)
                {
                    MessageBox.Show(
                        $"Mã vật tư '{materialCode}' đã tồn tại trong nhà máy hiện tại!\n" +
                        "Vui lòng sử dụng mã khác hoặc cập nhật sản phẩm cũ.",
                        "Trùng mã trong nhà máy",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_MaterialCode.Focus();
                    return;
                }

                var product = new Product
                {
                    MaterialCode = materialCode,
                    Barcode = "BC" + DateTime.Now.ToString("yyMMddHHmmssfff"),
                    Name = txt_Name.Text.Trim(),
                    Description = txt_Description.Text?.Trim() ?? "",
                    Specification = specification,
                    UnitId = Convert.ToInt32(cb_Unit.SelectedValue),
                    LocationId = Convert.ToInt32(cb_Location.SelectedValue),
                    FactoryId = CurrentFactoryId,
                    CategoryId = Convert.ToInt32(cb_category.SelectedValue),
                    SafetyStock = decimal.TryParse(txt_SafetyStock.Text?.Trim(), out var ss) ? ss : 0m,
                    CurrentQuantity = soLuongNhap,
                    TotalImported = soLuongNhap,
                    TotalExported = 0,
                    LastImportDate = DateTime.Now,
                    LastImportById = CurrentUserId
                };

                db.Products.Add(product);
                db.SaveChanges();

                MessageBox.Show("Thêm sản phẩm và nhập kho lần đầu thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                LoadProducts();
                txt_MaterialCode.Focus();
            }
            catch (DbUpdateException dbEx)
            {
                string errorMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                MessageBox.Show($"Lỗi cơ sở dữ liệu khi thêm sản phẩm:\n{errorMsg}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (gv_product.MainView is not GridView view || view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_MaterialCode.Text) ||
                string.IsNullOrWhiteSpace(txt_Name.Text) ||
                cb_Unit.SelectedValue == null ||
                cb_Location.SelectedValue == null ||
                cb_category.SelectedValue == null)
            {
                MessageBox.Show("Điền đầy đủ thông tin bắt buộc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string materialCode = txt_MaterialCode.Text.Trim();
            int currentId = Convert.ToInt32(view.GetFocusedRowCellValue("Id"));

            try
            {
                using var db = new QLKhoDbContext();
                if (db.Products.Any(p => p.MaterialCode == materialCode && p.Id != currentId))
                {
                    MessageBox.Show("Mã vật tư đã tồn tại!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var product = db.Products.Find(currentId);
                if (product == null) return;

                product.MaterialCode = materialCode;
                product.Name = txt_Name.Text.Trim();
                product.Description = txt_Description.Text?.Trim();
                product.Specification = txt_Specification?.Text?.Trim();
                product.SafetyStock = decimal.TryParse(txt_SafetyStock.Text?.Trim(), out var ss) ? ss : 0m;
                product.UnitId = Convert.ToInt32(cb_Unit.SelectedValue);
                product.LocationId = Convert.ToInt32(cb_Location.SelectedValue);
                product.CategoryId = Convert.ToInt32(cb_category.SelectedValue);

                db.SaveChanges();
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (gv_product.MainView is not GridView view || view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(view.GetFocusedRowCellValue("Id"));

            if (MessageBox.Show("Bạn có chắc muốn xóa hoàn toàn sản phẩm này?\n\n" +
                                "Hành động này sẽ xóa cả lịch sử nhập/xuất kho liên quan và không thể khôi phục.",
                    "Xác nhận xóa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using var db = new QLKhoDbContext();

                var user = db.Users.Find(CurrentUserId);
                if (user == null || !user.CanDeleteProduct)
                {
                    MessageBox.Show("Bạn không có quyền xóa sản phẩm!\n\nVui lòng liên hệ quản trị viên để được cấp quyền.",
                        "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var product = db.Products
                    .Include(p => p.ImportDetails)
                    .Include(p => p.ExportDetails)
                    .FirstOrDefault(p => p.Id == productId);

                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.Products.Remove(product);
                db.SaveChanges();

                MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadProducts();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_chuyen_bracode_Click(object sender, EventArgs e)
        {
            if (gv_product.MainView is not GridView view || view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Chọn sản phẩm trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var row = view.GetFocusedRow() as dynamic;
            if (row == null || string.IsNullOrWhiteSpace(row.Barcode))
            {
                MessageBox.Show("Không có mã barcode!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string barcodeValue = row.Barcode.ToString().Trim();
            string productName = row.Name ?? "Sản phẩm";

            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    Width = pn_bracode.Width - 20,
                    Height = pn_bracode.Height - 80,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            var pixelData = writer.Write(barcodeValue);
            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            pn_bracode.Controls.Clear();
            var container = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var picBox = new PictureBox
            {
                Image = (Image)bitmap.Clone(),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = bitmap.Width,
                Height = bitmap.Height,
                Location = new Point((pn_bracode.Width - bitmap.Width) / 2, 10)
            };

            var lblName = new Label
            {
                Text = productName,
                AutoSize = false,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, picBox.Bottom + 10),
                Width = pn_bracode.Width,
                Height = 40
            };

            container.Controls.Add(picBox);
            container.Controls.Add(lblName);
            pn_bracode.Controls.Add(container);
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            if (pn_bracode.Controls.Count == 0 || pn_bracode.Controls[0] is not Panel)
            {
                MessageBox.Show("Chưa có mã QR!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Lưu mã QR",
                FileName = $"QR_{txt_MaterialCode.Text.Trim()}_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                DefaultExt = "png"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                using var bmp = new Bitmap(pn_bracode.Width, pn_bracode.Height);
                pn_bracode.DrawToBitmap(bmp, new Rectangle(0, 0, pn_bracode.Width, pn_bracode.Height));
                bmp.Save(sfd.FileName, ImageFormat.Png);
                MessageBox.Show("Đã lưu!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_print_Click(object sender, EventArgs e)
        {
            if (pn_bracode.Controls.Count == 0)
            {
                MessageBox.Show("Chưa có mã QR để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var pd = new PrintDocument();
            pd.PrintPage += (_, ev) =>
            {
                using var bmp = new Bitmap(pn_bracode.Width, pn_bracode.Height);
                pn_bracode.DrawToBitmap(bmp, new Rectangle(0, 0, pn_bracode.Width, pn_bracode.Height));
                ev.Graphics.DrawImage(bmp, new Point(50, 50));
            };

            try
            {
                pd.Print();
                MessageBox.Show("Đã in thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi in: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool CanImport()
        {
            using var db = new QLKhoDbContext();
            var user = db.Users.Find(CurrentUserId);
            return user != null && user.CanImport;
        }

        private void btn_nhapkho_Click(object sender, EventArgs e)
        {
            if (gv_product.MainView is not GridView view || view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Chọn sản phẩm cần nhập kho!", "Cảnh báo");
                return;
            }
            if (!CanImport())
            {
                MessageBox.Show("Bạn không có quyền nhập kho!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(view.GetFocusedRowCellValue("Id"));
            var form = new nhapkho { CurrentUserId = CurrentUserId };
            form.LoadProduct(productId);
            form.FormClosed += (s, args) => LoadProducts();
            form.ShowDialog();
        }

        private void btn_barcode_Click(object sender, EventArgs e)
        {
            if (!CanImport())
            {
                MessageBox.Show("Bạn không có quyền nhập kho!", "Từ chối",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var scanForm = new BarcodeScanForm { Mode = "ONLY_NHAP" };

            if (scanForm.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(scanForm.ScannedBarcode))
                return;

            string barcode = scanForm.ScannedBarcode.Trim();

            using var db = new QLKhoDbContext();

            var product = db.Products
                .Where(p => p.Barcode == barcode)
                .Select(p => new { p.Id, p.FactoryId })
                .FirstOrDefault();

            if (product == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm với mã vạch này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (product.FactoryId != CurrentFactoryId && CurrentFactoryId != 0)
            {
                MessageBox.Show("Bạn không có quyền nhập kho!", "Từ chối",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nhapForm = new nhapkho
            {
                CurrentUserId = CurrentUserId,
                CurrentFactoryId = CurrentFactoryId
            };

            nhapForm.LoadProduct(product.Id);
            nhapForm.FormClosed += (s, args) => LoadProducts();
            nhapForm.ShowDialog();
        }

        private void btn_lichsu_Click(object sender, EventArgs e)
        {
            if (gv_product.MainView is not GridView view || view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Chọn sản phẩm để xem lịch sử nhập kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(view.GetFocusedRowCellValue("Id"));
            var form = new Lichsunhap(productId);
            form.ShowDialog();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string keyword = txt_search.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadProducts();
                return;
            }

            if (gv_product.MainView is not GridView view) return;

            using var db = new QLKhoDbContext();
            var filtered = db.Products
                .Include(p => p.Unit)
                .Include(p => p.Location).ThenInclude(l => l.Shelf).ThenInclude(s => s.Warehouse)
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(keyword) ||
                            (p.Specification != null && p.Specification.ToLower().Contains(keyword)))
                .Select(p => new
                {
                    p.Id,
                    p.MaterialCode,
                    p.Barcode,
                    p.Name,
                    p.Description,
                    p.Specification,
                    Unit = p.Unit.Name,
                    FullLocation = p.Location == null || p.Location.Shelf == null || p.Location.Shelf.Warehouse == null
                        ? "Chưa có vị trí"
                        : p.Location.Shelf.Warehouse.Name + " → " + p.Location.Shelf.Name + " → " + p.Location.Code,
                    Category = p.Category.Name,
                    p.CurrentQuantity,
                    p.SafetyStock
                })
                .ToList();

            gv_product.DataSource = filtered;
            view.RefreshData();
        }

        private void btn_addcategory_Click(object sender, EventArgs e)
        {
            string code = txt_code.Text.Trim();
            string name = txt_namecategory.Text.Trim();
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập đủ Mã danh mục và Tên danh mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new QLKhoDbContext();
                if (db.Categories.Any(c => c.code == code))
                {
                    MessageBox.Show("Mã danh mục này đã tồn tại!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (db.Categories.Any(c => c.Name.ToLower() == name.ToLower()))
                {
                    MessageBox.Show("Tên danh mục này đã tồn tại!", "Trùng tên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var category = new Category
                {
                    code = code,
                    Name = name,
                    Description = null
                };

                db.Categories.Add(category);
                db.SaveChanges();

                MessageBox.Show("Thêm danh mục thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategoryCombo();
                LoadCategoryGrid();
                LoadProducts();
                txt_code.Clear();
                txt_namecategory.Clear();
                txt_code.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm danh mục:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_editcategory_Click(object sender, EventArgs e)
        {
            if (gridViewCategory.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần sửa trong bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newCode = txt_code.Text.Trim();
            string newName = txt_namecategory.Text.Trim();
            if (string.IsNullOrWhiteSpace(newCode) || string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Mã danh mục và Tên danh mục không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int categoryId = Convert.ToInt32(gridViewCategory.GetFocusedRowCellValue("Id"));

            try
            {
                using var db = new QLKhoDbContext();
                var category = db.Categories.Find(categoryId);
                if (category == null)
                {
                    MessageBox.Show("Không tìm thấy danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (db.Categories.Any(c => c.code == newCode && c.Id != categoryId))
                {
                    MessageBox.Show("Mã danh mục này đã tồn tại!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (db.Categories.Any(c => c.Name.ToLower() == newName.ToLower() && c.Id != categoryId))
                {
                    MessageBox.Show("Tên danh mục này đã tồn tại!", "Trùng tên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                category.code = newCode;
                category.Name = newName;
                db.SaveChanges();

                MessageBox.Show("Cập nhật danh mục thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategoryGrid();
                LoadCategoryCombo();
                LoadProducts();
                gridViewCategory.FocusedRowHandle = gridViewCategory.LocateByValue("Id", categoryId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật danh mục:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_deletecategory_Click(object sender, EventArgs e)
        {
            if (gridViewCategory.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa trong bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int categoryId = Convert.ToInt32(gridViewCategory.GetFocusedRowCellValue("Id"));
            string categoryName = gridViewCategory.GetFocusedRowCellValue("Name").ToString();

            if (MessageBox.Show($"Bạn có chắc muốn xóa danh mục:\n\n{categoryName} ?\n\nHành động này không thể hoàn tác!",
                                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using var db = new QLKhoDbContext();
                var category = db.Categories.Find(categoryId);
                if (category == null)
                {
                    MessageBox.Show("Không tìm thấy danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (db.Products.Any(p => p.CategoryId == categoryId))
                {
                    MessageBox.Show("Không thể xóa danh mục này!\n\nDanh mục đang được sử dụng bởi một hoặc nhiều sản phẩm.",
                                    "Không cho phép xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.Categories.Remove(category);
                db.SaveChanges();

                MessageBox.Show("Xóa danh mục thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategoryGrid();
                LoadCategoryCombo();
                LoadProducts();
                txt_code.Clear();
                txt_namecategory.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa danh mục:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_addexecl_Click(object sender, EventArgs e)
        {
            if (CurrentFactoryId == 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà máy trước khi import Excel!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using var ofd = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                Title = "Chọn file Excel để import sản phẩm",
                Multiselect = false
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var filePath = ofd.FileName;
            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (IOException)
            {
                MessageBox.Show("Không thể đọc file vì nó đang được mở bởi Excel.\nVui lòng đóng file Excel hoàn toàn rồi thử lại!",
                    "File đang bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var errorList = new List<string>();
            var skippedCodes = new List<string>();
            int successCount = 0;
            try
            {
                using var workbook = new ClosedXML.Excel.XLWorkbook(filePath);
                var worksheet = workbook.Worksheet(1);
                using var db = new QLKhoDbContext();
                var allProducts = db.Products
                    .AsNoTracking()
                    .Select(p => new { p.MaterialCode, p.FactoryId })
                    .ToList();
                var currentFactoryCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var p in allProducts)
                {
                    currentFactoryCodes.Add($"{p.MaterialCode}|{p.FactoryId}");
                }
                foreach (var row in worksheet.Rows().Skip(1))
                {
                    try
                    {
                        string materialCode = row.Cell(1).GetString().Trim();
                        if (string.IsNullOrWhiteSpace(materialCode)) continue;
                        string name = row.Cell(2).GetString().Trim();
                        string specification = row.Cell(3).GetString().Trim();
                        string description = row.Cell(4).GetString().Trim();
                        string unitName = row.Cell(5).GetString().Trim();
                        decimal currentQty = row.Cell(6).GetValue<decimal>();
                        decimal safetyStock = row.Cell(7).GetValue<decimal>();
                        string locationName = row.Cell(8).GetString().Trim();
                        string categoryName = row.Cell(9).GetString().Trim();
                        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(unitName) ||
                            string.IsNullOrWhiteSpace(locationName) || string.IsNullOrWhiteSpace(categoryName))
                            throw new Exception("Thiếu thông tin bắt buộc");
                        if (currentQty <= 0)
                            throw new Exception("Số lượng tồn đầu phải > 0");
                        string key = $"{materialCode}|{CurrentFactoryId}";
                        if (currentFactoryCodes.Contains(key))
                        {
                            skippedCodes.Add(materialCode);
                            continue;
                        }
                        var unit = db.Units.FirstOrDefault(u => u.Name.Trim().ToLower() == unitName.Trim().ToLower());
                        if (unit == null)
                        {
                            unit = new Unit { Name = unitName.Trim(), Symbol = unitName.Trim() };
                            db.Units.Add(unit);
                            db.SaveChanges();
                        }
                        var category = db.Categories.FirstOrDefault(c => c.Name.Trim().ToLower() == categoryName.Trim().ToLower());
                        if (category == null)
                        {
                            category = new Category
                            {
                                code = "CAT" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                                Name = categoryName.Trim()
                            };
                            db.Categories.Add(category);
                            db.SaveChanges();
                        }
                        var location = db.Locations.FirstOrDefault(l => l.Name.Trim() == locationName.Trim());
                        if (location == null)
                            throw new Exception($"Không tìm thấy vị trí '{locationName.Trim()}'");
                        var product = new Product
                        {
                            MaterialCode = materialCode,
                            Barcode = "BC" + DateTime.Now.ToString("yyMMddHHmmssfff") + successCount,
                            Name = name,
                            Specification = specification,
                            Description = description,
                            UnitId = unit.Id,
                            LocationId = location.Id,
                            FactoryId = CurrentFactoryId,
                            CategoryId = category.Id,
                            CurrentQuantity = currentQty,
                            TotalImported = currentQty,
                            TotalExported = 0,
                            SafetyStock = safetyStock,
                            LastImportDate = DateTime.Now,
                            LastImportById = CurrentUserId
                        };
                        db.Products.Add(product);
                        db.SaveChanges();
                        currentFactoryCodes.Add(key);
                        successCount++;
                    }
                    catch (DbUpdateException dbEx)
                    {
                        string msg = dbEx.InnerException?.Message ?? dbEx.Message;
                        errorList.Add($"Dòng {row.RowNumber()}: Lỗi lưu dữ liệu - {msg}");
                    }
                    catch (Exception ex)
                    {
                        errorList.Add($"Dòng {row.RowNumber()}: {ex.Message}");
                    }
                }
                string message = $"✅ Import hoàn tất!\n\nĐã thêm {successCount} sản phẩm vào kho.";
                if (skippedCodes.Count > 0)
                    message += $"\n\n⚠️ Các mã bị bỏ qua (trùng trong nhà máy hiện tại):\n{string.Join(", ", skippedCodes)}";
                if (errorList.Count > 0)
                    message += $"\n\n❌ Các lỗi theo dòng:\n{string.Join("\n", errorList)}";
                MessageBoxIcon icon = (successCount > 0 && errorList.Count == 0) ? MessageBoxIcon.Information :
                                      (successCount > 0) ? MessageBoxIcon.Warning : MessageBoxIcon.Error;
                MessageBox.Show(message, "Kết quả import", MessageBoxButtons.OK, icon);
                LoadProducts();
                LoadCategoryCombo();
                LoadUnitCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi import tổng thể:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
