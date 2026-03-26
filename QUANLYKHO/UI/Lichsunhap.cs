using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;

namespace QUANLYKHO.UI
{
    public partial class Lichsunhap : Form
    {
        private readonly int? _productId;

        public Lichsunhap(int? productId = null)
        {
            InitializeComponent();
            _productId = productId;

            date_start.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            date_end.Value = DateTime.Today;

            LoadLichSuNhap();
        }

        private void LoadLichSuNhap(DateTime? startDate = null, DateTime? endDate = null)
        {
            using var context = new QLKhoDbContext();

            var query = context.Imports
                .Include(i => i.CreatedBy)
                .Include(i => i.Details)
                    .ThenInclude(d => d.Product)
                .AsQueryable();

            if (_productId.HasValue)
            {
                var product = context.Products.Find(_productId.Value);
                string productName = product?.Name ?? $"ID {_productId}";
                Text = $"Lịch sử nhập kho - {productName}";
                query = query.Where(i => i.Details.Any(d => d.ProductId == _productId.Value));
            }
            else
            {
                Text = "Lịch sử nhập kho - Tất cả phiếu nhập";
            }

            if (startDate.HasValue)
            {
                query = query.Where(i => i.ImportDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(i => i.ImportDate <= endDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            var data = query
                .Select(i => new
                {
                    MaPhieu = i.ImportCode,
                    NgayNhap = i.ImportDate,
                    NguoiNhap = i.CreatedBy.FullName ?? "N/A",
                    NhaCungCap = i.SupplierName ?? "",
                    TongSoLuong = i.Details.Sum(d => d.Quantity),
                    GhiChu = i.Note ?? ""
                })
                .OrderByDescending(x => x.NgayNhap)
                .ToList();

            gv_lichsu.DataSource = data;

            var view = gv_lichsu.MainView as GridView;
            if (view == null) return;

            view.ApplyHeader();

            ConfigureColumn(view, "MaPhieu", "Mã phiếu", 140, HorzAlignment.Center);
            ConfigureColumn(view, "NgayNhap", "Ngày nhập", 160, HorzAlignment.Center);
            ConfigureColumn(view, "NguoiNhap", "Người nhập", 180);
            ConfigureColumn(view, "NhaCungCap", "Nhà cung cấp", 220);
            ConfigureColumn(view, "TongSoLuong", "Số Lượng Nhập", 120, HorzAlignment.Center);
            ConfigureColumn(view, "GhiChu", "Ghi chú", 250);

            view.BestFitColumns();
        }

        private static void ConfigureColumn(GridView view, string fieldName, string caption, int width = 0,
            HorzAlignment alignment = HorzAlignment.Default, bool visible = true)
        {
            var col = view.Columns[fieldName];
            if (col == null) return;

            col.Caption = caption;
            if (width > 0) col.Width = width;
            col.Visible = visible;

            if (alignment != HorzAlignment.Default)
                col.AppearanceCell.TextOptions.HAlignment = alignment;

            if (fieldName == "TongSoLuong")
            {
                col.DisplayFormat.FormatType = FormatType.Numeric;
                col.DisplayFormat.FormatString = "n0";
            }
        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            DateTime? startDate = date_start.Value;
            DateTime? endDate = date_end.Value;

            if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadLichSuNhap(startDate, endDate);
        }
    }
}