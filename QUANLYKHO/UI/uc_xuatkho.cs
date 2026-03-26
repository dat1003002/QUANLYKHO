using DevExpress.XtraGrid.Views.Grid;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QUANLYKHO.UI
{
    public partial class uc_xuatkho : UserControl
    {
        public int CurrentUserId { get; set; } = 0;
        public int CurrentFactoryId { get; set; } = 0;

        private object[] allProducts;

        public uc_xuatkho()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void SetupGrid()
        {
            grid_xuatkho.OptionsView.ShowGroupPanel = false;
            grid_xuatkho.OptionsView.ShowIndicator = false; // ✅ Ẩn cột ngoài cùng bên trái
            grid_xuatkho.OptionsBehavior.Editable = false;
            grid_xuatkho.OptionsSelection.EnableAppearanceFocusedCell = false;
            grid_xuatkho.Columns.Clear();

            // ✅ Cột STT
            var colSTT = grid_xuatkho.Columns.AddVisible("STT");
            colSTT.Caption = "STT";
            colSTT.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            colSTT.OptionsColumn.AllowEdit = false;
            colSTT.Width = 50;
            colSTT.VisibleIndex = 0;

            // Các cột còn lại
            grid_xuatkho.Columns.AddVisible("MaterialCode").Caption = "Mã vật tư";
            grid_xuatkho.Columns.AddVisible("Name").Caption = "Tên vật tư";
            grid_xuatkho.Columns.AddVisible("Description").Caption = "Mô tả";
            grid_xuatkho.Columns.AddVisible("Specification").Caption = "Quy Cách";
            grid_xuatkho.Columns.AddVisible("Unit").Caption = "Đơn vị";
            grid_xuatkho.Columns.AddVisible("FullLocation").Caption = "Vị trí";
            grid_xuatkho.Columns.AddVisible("Category").Caption = "Phân Loại";

            var colQty = grid_xuatkho.Columns.AddVisible("CurrentQuantity");
            colQty.Caption = "Tồn hiện tại";
            colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colQty.DisplayFormat.FormatString = "n0";

            var colSafety = grid_xuatkho.Columns.AddVisible("SafetyStock");
            colSafety.Caption = "Tồn an toàn";
            colSafety.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colSafety.DisplayFormat.FormatString = "n0";

            grid_xuatkho.Columns["MaterialCode"].Width = 100;
            grid_xuatkho.Columns["Name"].Width = 180;
            grid_xuatkho.Columns["Description"].Width = 160;
            grid_xuatkho.Columns["FullLocation"].Width = 220;
            grid_xuatkho.Columns["CurrentQuantity"].Width = 90;
            grid_xuatkho.Columns["SafetyStock"].Width = 90;

            // ✅ Gắn event STT
            grid_xuatkho.CustomUnboundColumnData += grid_xuatkho_CustomUnboundColumnData;

            grid_xuatkho.ApplyHeader();
        }
        private void grid_xuatkho_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "STT" && e.IsGetData)
            {
                e.Value = e.ListSourceRowIndex + 1;
            }
        }
        private void LoadData()
        {
            if (CurrentFactoryId <= 0)
            {
                gv_xuatkho.DataSource = null;
                allProducts = null;
                return;
            }

            try
            {
                using var db = new QLKhoDbContext();

                var products = db.Products
                    .Where(p => p.FactoryId == CurrentFactoryId)
                    .Where(p => db.ExportDetails.Any(ed => ed.ProductId == p.Id))   // Chỉ lấy sản phẩm đã từng xuất kho
                    .Include(p => p.Unit)
                    .Include(p => p.Location).ThenInclude(l => l.Shelf).ThenInclude(s => s.Warehouse)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .Select(p => new
                    {
                        p.Id,
                        p.MaterialCode,
                        p.Name,
                        p.Description,
                        p.Specification,
                        Unit = p.Unit != null ? p.Unit.Name : "",
                        FullLocation = p.Location != null && p.Location.Shelf != null && p.Location.Shelf.Warehouse != null
                            ? $"{p.Location.Shelf.Warehouse.Name} → {p.Location.Shelf.Name} → {p.Location.Code}"
                            : "Chưa phân vị trí",
                        Category = p.Category != null ? p.Category.Name : "",
                        p.CurrentQuantity,
                        p.SafetyStock
                    })
                    .ToArray();

                allProducts = products;
                gv_xuatkho.DataSource = allProducts;
                grid_xuatkho.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu xuất kho:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }
        public void RefreshData()
        {
            LoadData();
        }
        private bool CanExport()
        {
            using var db = new QLKhoDbContext();
            var user = db.Users.Find(CurrentUserId);
            return user != null && (user.CanExport || user.IsSuperAdmin);
        }
        private void btn_xuatkho_Click(object sender, EventArgs e)
        {
            if (!CanExport())
            {
                MessageBox.Show("Bạn không có quyền xuất kho!", "Phân quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var view = grid_xuatkho;
            if (view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xuất kho.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int productId = Convert.ToInt32(view.GetRowCellValue(view.FocusedRowHandle, "Id"));
            var homeForm = Application.OpenForms["Home"] as Home;
            int userId = homeForm?.CurrentUserId ?? CurrentUserId;
            using var form = new xuatkho(productId, userId);
            form.ShowDialog();
            RefreshData();
        }
        private void btn_lichsu_Click(object sender, EventArgs e)
        {
            var view = grid_xuatkho;
            if (view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xem lịch sử xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(view.GetRowCellValue(view.FocusedRowHandle, "Id"));
            var form = new Lichsuxuat();
            form.LoadLichSuXuat(productId);
            form.ShowDialog();
        }

        private void btn_bracode_Click_1(object sender, EventArgs e)
        {
            if (!CanExport())
            {
                MessageBox.Show("Bạn không có quyền xuất kho!", "Phân quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var scanForm = new BarcodeScanForm { Mode = "ONLY_XUAT" };
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
                MessageBox.Show("Không tìm thấy sản phẩm với barcode này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (product.FactoryId != CurrentFactoryId && CurrentFactoryId != 0)
            {
                MessageBox.Show("Bạn không có quyền xuất kho!", "Phân quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var homeForm = Application.OpenForms["Home"] as Home;
            int userId = homeForm?.CurrentUserId ?? CurrentUserId;
            using var form = new xuatkho(product.Id, userId);
            form.ShowDialog();
            RefreshData();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_search.Clear();
            if (allProducts != null && allProducts.Length > 0)
            {
                gv_xuatkho.DataSource = allProducts;
                grid_xuatkho.BestFitColumns();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (allProducts == null || allProducts.Length == 0)
            {
                return;
            }

            string keyword = txt_search.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                gv_xuatkho.DataSource = allProducts;
            }
            else
            {
                var filtered = allProducts
                    .Where(p =>
                    {
                        var item = (dynamic)p;
                        return
                            (item.MaterialCode?.ToString().ToLower().Contains(keyword) ?? false) ||
                            (item.Name?.ToString().ToLower().Contains(keyword) ?? false);
                    })
                    .ToArray();

                gv_xuatkho.DataSource = filtered;
            }

            grid_xuatkho.BestFitColumns();
        }

        private void btn_execl_Click(object sender, EventArgs e)
        {
            try
            {
                if (gv_xuatkho.DataSource == null)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Xuất file Excel";
                saveFileDialog.FileName = "DanhSachXuatKho.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    grid_xuatkho.ExportToXlsx(saveFileDialog.FileName);

                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}