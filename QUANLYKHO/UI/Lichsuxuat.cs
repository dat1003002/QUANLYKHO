using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.model;
using QUANLYKHO.Data;

namespace QUANLYKHO.UI
{
    public partial class Lichsuxuat : Form
    {
        private int _productId;

        public Lichsuxuat()
        {
            InitializeComponent();
            SetupGrid();

            date_start.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            date_end.Value = DateTime.Today;
        }

        public Lichsuxuat(int productId) : this()
        {
            _productId = productId;
            LoadLichSuXuat();
        }

        private void SetupGrid()
        {
            grid_lichsu.OptionsView.ShowGroupPanel = false;
            grid_lichsu.OptionsBehavior.Editable = false;
            grid_lichsu.OptionsSelection.EnableAppearanceFocusedCell = false;
            grid_lichsu.Columns.Clear();

            grid_lichsu.Columns.AddVisible("ExportCode").Caption = "Mã phiếu xuất";
            grid_lichsu.Columns["ExportCode"].Width = 120;

            var colDate = grid_lichsu.Columns.AddVisible("ExportDate");
            colDate.Caption = "Ngày xuất";
            colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            colDate.Width = 140;

            grid_lichsu.Columns.AddVisible("CreatedBy").Caption = "Người xuất";
            grid_lichsu.Columns["CreatedBy"].Width = 140;

            var colQty = grid_lichsu.Columns.AddVisible("Quantity");
            colQty.Caption = "Số lượng xuất";
            colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colQty.DisplayFormat.FormatString = "n0";
            colQty.Width = 100;

            var colStock = grid_lichsu.Columns.AddVisible("CurrentQuantity");
            colStock.Caption = "Tồn sau xuất";
            colStock.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colStock.DisplayFormat.FormatString = "n0";
            colStock.Width = 100;

            grid_lichsu.Columns.AddVisible("Specification").Caption = "Quy cách";
            grid_lichsu.Columns["Specification"].Width = 160;

            var colReason = grid_lichsu.Columns.AddVisible("Reason");
            colReason.Caption = "Lý do xuất";
            colReason.Width = 180;

            grid_lichsu.ApplyHeader();
        }

        public void LoadLichSuXuat(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (_productId <= 0) return;

            try
            {
                using var db = new QLKhoDbContext();

                var query = db.ExportDetails
                    .Where(d => d.ProductId == _productId)
                    .Include(d => d.Export)
                        .ThenInclude(e => e.CreatedBy)
                    .Include(d => d.Product)
                    .AsQueryable();

                if (startDate.HasValue)
                {
                    query = query.Where(d => d.Export.ExportDate >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(d => d.Export.ExportDate <= endDate.Value.Date.AddDays(1).AddTicks(-1));
                }

                var data = query
                    .Select(d => new
                    {
                        d.Export.ExportCode,
                        d.Export.ExportDate,
                        CreatedBy = d.Export.CreatedBy != null
                            ? (d.Export.CreatedBy.FullName ?? d.Export.CreatedBy.Username ?? "N/A")
                            : "N/A",
                        d.Quantity,
                        CurrentQuantity = d.Product.CurrentQuantity + d.Quantity,
                        d.Product.Specification,
                        d.Export.Reason
                    })
                    .OrderByDescending(x => x.ExportDate)
                    .ToList();

                gv_lichsu.DataSource = data;
                grid_lichsu.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử: " + ex.Message);
            }
        }

        public void LoadLichSuXuat(int productId)
        {
            _productId = productId;
            LoadLichSuXuat();
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

            LoadLichSuXuat(startDate, endDate);
        }
    }
}