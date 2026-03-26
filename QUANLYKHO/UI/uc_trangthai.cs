using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QUANLYKHO.UI
{
    public partial class uc_trangthai : UserControl
    {
        public int CurrentFactoryId { get; set; } = 0;

        public uc_trangthai()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            if (CurrentFactoryId <= 0)
            {
                gv_trangthai.DataSource = null;
                return;
            }

            try
            {
                using var db = new QLKhoDbContext();

                var data = db.Products
                    .Where(p => p.FactoryId == CurrentFactoryId)
                    .Include(p => p.Unit)
                    .Include(p => p.Location)
                        .ThenInclude(l => l.Shelf)
                            .ThenInclude(s => s.Warehouse)
                    .AsNoTracking()
                    .Select(p => new
                    {
                        p.MaterialCode,
                        p.Name,
                        p.Specification,
                        p.Description,
                        Unit = p.Unit != null ? p.Unit.Name : "",
                        FullLocation = p.Location != null
                            && p.Location.Shelf != null
                            && p.Location.Shelf.Warehouse != null
                                ? $"{p.Location.Shelf.Warehouse.Name} → {p.Location.Shelf.Name} → {p.Location.Code}"
                                : "Chưa phân vị trí",
                        p.CurrentQuantity,
                        p.TotalImported,
                        p.TotalExported,
                        p.SafetyStock,
                        SortPriority = p.CurrentQuantity < p.SafetyStock ? 0 :
                                       p.CurrentQuantity == p.SafetyStock ? 1 : 2
                    })
                    .OrderBy(x => x.SortPriority)
                    .ThenBy(x => x.CurrentQuantity)
                    .ToList();

                gv_trangthai.DataSource = data;

                GridView view = grid_trangthai;
                if (view == null) return;

                view.ApplyHeader();
                view.OptionsView.ShowGroupPanel = false;
                view.OptionsBehavior.Editable = false;
                view.OptionsSelection.EnableAppearanceFocusedCell = false;
                view.OptionsSelection.EnableAppearanceFocusedRow = false;
                view.OptionsSelection.EnableAppearanceHideSelection = false;
                view.OptionsView.EnableAppearanceOddRow = false;
                view.OptionsView.EnableAppearanceEvenRow = false;

                view.Columns.Clear();

                view.Columns.AddVisible("MaterialCode").Caption = "Mã vật tư";
                view.Columns.AddVisible("Name").Caption = "Tên sản phẩm";
                view.Columns.AddVisible("Specification").Caption = "Quy cách";
                view.Columns.AddVisible("Description").Caption = "Mô tả";
                view.Columns.AddVisible("Unit").Caption = "Đơn vị";
                view.Columns.AddVisible("FullLocation").Caption = "Vị trí";

                var colNhap = view.Columns.AddVisible("TotalImported");
                colNhap.Caption = "Tổng nhập";
                colNhap.DisplayFormat.FormatType = FormatType.Numeric;
                colNhap.DisplayFormat.FormatString = "n0";

                var colXuat = view.Columns.AddVisible("TotalExported");
                colXuat.Caption = "Tổng xuất";
                colXuat.DisplayFormat.FormatType = FormatType.Numeric;
                colXuat.DisplayFormat.FormatString = "n0";

                var colTon = view.Columns.AddVisible("CurrentQuantity");
                colTon.Caption = "Tồn kho";
                colTon.DisplayFormat.FormatType = FormatType.Numeric;
                colTon.DisplayFormat.FormatString = "n0";

                var colSafety = view.Columns.AddVisible("SafetyStock");
                colSafety.Caption = "Tồn kho an toàn";
                colSafety.DisplayFormat.FormatType = FormatType.Numeric;
                colSafety.DisplayFormat.FormatString = "n0";

                if (view.Columns["SortPriority"] != null)
                    view.Columns["SortPriority"].Visible = false;

                // Tô màu hàng
                view.RowStyle += (s, e) =>
                {
                    if (e.RowHandle < 0) return;

                    var row = view.GetRow(e.RowHandle) as dynamic;
                    if (row == null) return;

                    decimal ton = row.CurrentQuantity;
                    decimal safety = row.SafetyStock;

                    if (ton < safety)
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                    }
                    else if (ton == safety)
                    {
                        e.Appearance.BackColor = Color.Blue;
                        e.Appearance.ForeColor = Color.White;
                    }
                };

                view.Columns["MaterialCode"].Width = 100;
                view.Columns["Name"].Width = 180;
                view.Columns["Specification"].Width = 120;
                view.Columns["Description"].Width = 200;
                view.Columns["Unit"].Width = 80;
                view.Columns["FullLocation"].Width = 220;
                view.Columns["CurrentQuantity"].Width = 90;
                view.Columns["TotalImported"].Width = 90;
                view.Columns["TotalExported"].Width = 90;
                view.Columns["SafetyStock"].Width = 110;

                view.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu trạng thái:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData()
        {
            LoadData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            if (CurrentFactoryId <= 0) return;

            string keyword = txt_search.Text.Trim().ToLower();

            try
            {
                using var db = new QLKhoDbContext();

                var query = db.Products
                    .Where(p => p.FactoryId == CurrentFactoryId)
                    .Include(p => p.Unit)
                    .Include(p => p.Location)
                        .ThenInclude(l => l.Shelf)
                            .ThenInclude(s => s.Warehouse)
                    .AsNoTracking();

                // Nếu có nhập tìm kiếm thì lọc
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p =>
                        p.MaterialCode.ToLower().Contains(keyword) ||
                        p.Name.ToLower().Contains(keyword) ||
                        p.Specification.ToLower().Contains(keyword)
                    );
                }

                var data = query
                    .Select(p => new
                    {
                        p.MaterialCode,
                        p.Name,
                        p.Specification,
                        p.Description,
                        Unit = p.Unit != null ? p.Unit.Name : "",
                        FullLocation = p.Location != null
                            && p.Location.Shelf != null
                            && p.Location.Shelf.Warehouse != null
                                ? $"{p.Location.Shelf.Warehouse.Name} → {p.Location.Shelf.Name} → {p.Location.Code}"
                                : "Chưa phân vị trí",
                        p.CurrentQuantity,
                        p.TotalImported,
                        p.TotalExported,
                        p.SafetyStock,
                        SortPriority = p.CurrentQuantity < p.SafetyStock ? 0 :
                                       p.CurrentQuantity == p.SafetyStock ? 1 : 2
                    })
                    .OrderBy(x => x.SortPriority)
                    .ThenBy(x => x.CurrentQuantity)
                    .ToList();

                gv_trangthai.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_execl_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = "TrangThaiKho.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    grid_trangthai.ExportToXlsx(sfd.FileName);

                    MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = sfd.FileName,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_search.Text = string.Empty;

            LoadData();
        }
    }
}