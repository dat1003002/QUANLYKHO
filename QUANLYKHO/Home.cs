    using DevExpress.XtraBars;
    using DevExpress.XtraBars.FluentDesignSystem;
    using DevExpress.XtraBars.Navigation;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraSplashScreen;
    using Microsoft.EntityFrameworkCore;
    using QUANLYKHO.Data;
    using QUANLYKHO.UI;
    using System;
    using System.Linq;
    using System.Windows.Forms;

    namespace QUANLYKHO
    {
        public partial class Home : FluentDesignForm
        {
            private uc_khuvuc uc_Khuvuc;
            private uc_nhapkho ucnhapkho;
            private uc_xuatkho ucxuatkho;
            private uc_user ucUser;
            private uc_xuong ucXuong;
            private uc_trangthai ucTrangthai;
            private uc_dieutrakho ucDieutrakho;
            private SplashScreenManager splashManager;

            public string FullName { get; set; }
            public int CurrentUserId { get; set; } = 0;
            public string CurrentUserRole { get; set; }
            public bool IsSuperAdmin { get; set; } = false;
            public bool CanImport { get; set; } = false;
            public bool CanExport { get; set; } = false;
            public bool CanCreateInventoryCheck { get; set; } = false;
            public bool CanApproveInventoryCheck { get; set; } = false;
            public int CurrentFactoryId { get; set; } = 0;

            public Home()
            {
                InitializeComponent();
                splashManager = new SplashScreenManager(this, typeof(fLoading), true, true);
                Shown += Home_Shown;
            }

        private void Home_Shown(object sender, EventArgs e)
        {
            name_user.Caption = $"{FullName}";

            if (CurrentUserRole != "Admin")
            {
                btn_system.Visible = false;
                foreach (AccordionControlElement child in btn_system.Elements)
                    child.Visible = false;
            }

            ApplyPermissions();
            LoadFactory();
            LoadChartNhapXuat();
            LoadPieChart();
            LoadPieChartCategory();
        }


        private void LoadFactory()
            {
                using var db = new QLKhoDbContext();
                var factories = db.UserFactories
                    .Where(uf => uf.UserId == CurrentUserId)
                    .Select(uf => new { uf.Factory.Id, uf.Factory.Name })
                    .ToList();

                var repo = cbfactory.Edit as RepositoryItemComboBox;
                repo.Items.Clear();
                repo.Items.AddRange(factories.Select(f => f.Name).ToArray());

                if (factories.Any())
                {
                    cbfactory.EditValue = factories.First().Name;
                    CurrentFactoryId = factories.First().Id;
                }
                else
                {
                    cbfactory.EditValue = "Chưa có nhà máy";
                    CurrentFactoryId = 0;
                }

                cbfactory.EditValueChanged -= Cbfactory_EditValueChanged;
                cbfactory.EditValueChanged += Cbfactory_EditValueChanged;
            }

            private void Cbfactory_EditValueChanged(object sender, EventArgs e)
            {
                var selectedName = cbfactory.EditValue?.ToString();

                using var db = new QLKhoDbContext();
                CurrentFactoryId = db.UserFactories
                    .Where(uf => uf.UserId == CurrentUserId && uf.Factory.Name == selectedName)
                    .Select(uf => uf.Factory.Id)
                    .FirstOrDefault();

                ChangeFactory();
                LoadChartNhapXuat();
                LoadPieChart();
                LoadPieChartCategory();

        }

        private void ChangeFactory()
            {
                if (uc_Khuvuc != null && !uc_Khuvuc.IsDisposed)
                {
                    uc_Khuvuc.CurrentFactoryId = CurrentFactoryId;
                    uc_Khuvuc.RefreshData();
                }

                if (ucnhapkho != null && !ucnhapkho.IsDisposed)
                {
                    ucnhapkho.CurrentFactoryId = CurrentFactoryId;
                    ucnhapkho.RefreshData();
                }

                if (ucxuatkho != null && !ucxuatkho.IsDisposed)
                {
                    ucxuatkho.CurrentFactoryId = CurrentFactoryId;
                    ucxuatkho.RefreshData();
                }

                if (ucTrangthai != null && !ucTrangthai.IsDisposed)
                {
                    ucTrangthai.CurrentFactoryId = CurrentFactoryId;
                    ucTrangthai.RefreshData();
                }

                if (ucDieutrakho != null && !ucDieutrakho.IsDisposed)
                {
                    ucDieutrakho.CurrentFactoryId = CurrentFactoryId;
                    ucDieutrakho.RefreshData();
                }
            }

            private void ApplyPermissions()
            {
                if (IsSuperAdmin) return;

                if (!CanImport) btnnhapkho.Visible = false;
                if (!CanExport) btnxuatkho.Visible = false;
            }

            private void btnnhapkho_Click(object sender, EventArgs e)
            {
                if (!CanImport && !IsSuperAdmin)
                {
                    XtraMessageBox.Show("Bạn không có quyền nhập kho.", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ucnhapkho == null || ucnhapkho.IsDisposed)
                {
                    ucnhapkho = new uc_nhapkho
                    {
                        Dock = DockStyle.Fill,
                        CurrentUserId = CurrentUserId,
                        CurrentFactoryId = CurrentFactoryId
                    };
                    ContainerMain.Controls.Add(ucnhapkho);
                }
                else
                {
                    ucnhapkho.CurrentFactoryId = CurrentFactoryId;
                }

                ucnhapkho.BringToFront();
                ucnhapkho.RefreshData();
                lbltieude.Caption = "Quản lý Nhập Kho";
            }

            private void btnxuatkho_Click(object sender, EventArgs e)
            {
                if (!CanExport && !IsSuperAdmin)
                {
                    XtraMessageBox.Show("Bạn không có quyền xuất kho.", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ucxuatkho == null || ucxuatkho.IsDisposed)
                {
                    ucxuatkho = new uc_xuatkho
                    {
                        Dock = DockStyle.Fill,
                        CurrentUserId = CurrentUserId,
                        CurrentFactoryId = CurrentFactoryId
                    };
                    ContainerMain.Controls.Add(ucxuatkho);
                }
                else
                {
                    ucxuatkho.CurrentFactoryId = CurrentFactoryId;
                }

                ucxuatkho.BringToFront();
                ucxuatkho.RefreshData();
                lbltieude.Caption = "Quản lý Xuất Kho";
            }

            private void btnkhuvuc_Click(object sender, EventArgs e)
            {
                if (uc_Khuvuc == null || uc_Khuvuc.IsDisposed)
                {
                    uc_Khuvuc = new uc_khuvuc
                    {
                        Dock = DockStyle.Fill,
                        CurrentFactoryId = CurrentFactoryId
                    };
                    ContainerMain.Controls.Add(uc_Khuvuc);
                }
                else
                {
                    uc_Khuvuc.CurrentFactoryId = CurrentFactoryId;
                }

                uc_Khuvuc.BringToFront();
                uc_Khuvuc.RefreshData();
                lbltieude.Caption = "Quản lý Khu Vực";
            }

            private void btnnguoidung_Click(object sender, EventArgs e)
            {
                if (!IsSuperAdmin && CurrentUserRole != "Admin")
                {
                    XtraMessageBox.Show("Bạn không có quyền quản lý người dùng.", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ucUser == null || ucUser.IsDisposed)
                {
                    ucUser = new uc_user();
                    ucUser.Dock = DockStyle.Fill;
                    ContainerMain.Controls.Add(ucUser);
                }

                ucUser.BringToFront();
                lbltieude.Caption = "Quản lý Người Dùng";
            }

            private void btn_xuong_Click(object sender, EventArgs e)
            {
                if (!IsSuperAdmin && CurrentUserRole != "Admin")
                {
                    XtraMessageBox.Show("Bạn không có quyền quản lý phân xưởng.", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ucXuong == null || ucXuong.IsDisposed)
                {
                    ucXuong = new uc_xuong();
                    ucXuong.Dock = DockStyle.Fill;
                    ContainerMain.Controls.Add(ucXuong);
                }

                ucXuong.BringToFront();
                lbltieude.Caption = "Danh Mục Phân Xưởng";
            }

            private void btntinhtrang_Click(object sender, EventArgs e)
            {
                if (ucTrangthai == null || ucTrangthai.IsDisposed)
                {
                    ucTrangthai = new uc_trangthai
                    {
                        Dock = DockStyle.Fill,
                        CurrentFactoryId = CurrentFactoryId
                    };
                    ContainerMain.Controls.Add(ucTrangthai);
                }
                else
                {
                    ucTrangthai.CurrentFactoryId = CurrentFactoryId;
                }

                ucTrangthai.BringToFront();
                ucTrangthai.RefreshData();
                lbltieude.Caption = "Trạng Thái Thiết Bị";
            }

            private void btn_dieutrakho_Click(object sender, EventArgs e)
            {
                if (ucDieutrakho == null || ucDieutrakho.IsDisposed)
                {
                    ucDieutrakho = new uc_dieutrakho
                    {
                        Dock = DockStyle.Fill,
                        CurrentFactoryId = CurrentFactoryId,
                        CurrentUserId = CurrentUserId
                    };
                    ContainerMain.Controls.Add(ucDieutrakho);
                }
                else
                {
                    ucDieutrakho.CurrentFactoryId = CurrentFactoryId;
                }

                ucDieutrakho.BringToFront();
                ucDieutrakho.RefreshData();
                lbltieude.Caption = "Điều Tra Kho";
            }

            private void btn_home_Click(object sender, EventArgs e)
            {
                foreach (var ctrl in ContainerMain.Controls.OfType<UserControl>().ToList())
                {
                    ContainerMain.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }

                ucnhapkho = null;
                ucxuatkho = null;
                uc_Khuvuc = null;
                ucUser = null;
                ucXuong = null;
                ucTrangthai = null;
                ucDieutrakho = null;

                lbltieude.Caption = "Trang Chủ";
            }
            private void LoadChartNhapXuat()
            {
                using var db = new QLKhoDbContext();

                var monthsNhap = db.ImportDetails
                    .Where(d => d.Import.FactoryId == CurrentFactoryId)
                    .Select(d => d.Import.ImportDate.Month);

                var monthsXuat = db.ExportDetails
                    .Where(d => d.Export.FactoryId == CurrentFactoryId)
                    .Select(d => d.Export.ExportDate.Month);

                var months = monthsNhap
                    .Union(monthsXuat)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                if (!months.Any())
                {
                    months = Enumerable.Range(1, 12).ToList();
                }

                var data = months.Select(month => new
                {
                    Thang = "Tháng " + month,

                    Nhap = db.ImportDetails
                        .Where(d => d.Import.ImportDate.Month == month
                                 && d.Import.FactoryId == CurrentFactoryId)
                        .Sum(d => (decimal?)d.Quantity) ?? 0,

                    Xuat = db.ExportDetails
                        .Where(d => d.Export.ExportDate.Month == month
                                 && d.Export.FactoryId == CurrentFactoryId)
                        .Sum(d => (decimal?)d.Quantity) ?? 0,

                    Ton = db.Products
                        .Where(p => p.FactoryId == CurrentFactoryId)
                        .Sum(p => (decimal?)p.CurrentQuantity) ?? 0
                }).ToList();

                chartControl_nhapxuat.Series.Clear();

                var sNhap = new DevExpress.XtraCharts.Series("Nhập kho", DevExpress.XtraCharts.ViewType.Bar);
                sNhap.DataSource = data;
                sNhap.ArgumentDataMember = "Thang";
                sNhap.ValueDataMembers.AddRange("Nhap");

                var sXuat = new DevExpress.XtraCharts.Series("Xuất kho", DevExpress.XtraCharts.ViewType.Bar);
                sXuat.DataSource = data;
                sXuat.ArgumentDataMember = "Thang";
                sXuat.ValueDataMembers.AddRange("Xuat");

                var sTon = new DevExpress.XtraCharts.Series("Tồn kho", DevExpress.XtraCharts.ViewType.Bar);
                sTon.DataSource = data;
                sTon.ArgumentDataMember = "Thang";
                sTon.ValueDataMembers.AddRange("Ton");

                chartControl_nhapxuat.Series.AddRange(new[] { sNhap, sXuat, sTon });

                foreach (DevExpress.XtraCharts.Series s in chartControl_nhapxuat.Series)
                {
                    s.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                }

                var diagram = chartControl_nhapxuat.Diagram as DevExpress.XtraCharts.XYDiagram;
                if (diagram != null)
                {
                    diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.False;

                    diagram.AxisY.Title.Text = "Số lượng";
                    diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;

                    diagram.AxisX.Label.TextPattern = "{A}";
                    diagram.AxisX.Label.Angle = 0;
                    diagram.AxisX.Label.Staggered = false;
                }
            }

            private void btn_out_ItemClick(object sender, ItemClickEventArgs e)
            {
                this.Close();
            }

            private void btn_nhapxuat_Click(object sender, EventArgs e)
            {
                if (!CanImport && !CanExport && !IsSuperAdmin)
                {
                    XtraMessageBox.Show("Bạn không có quyền nhập kho hoặc xuất kho.",
                                        "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (CurrentFactoryId == 0 && !IsSuperAdmin)
                {
                    XtraMessageBox.Show("Vui lòng chọn nhà máy trước khi quét barcode.",
                                        "Chưa chọn nhà máy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var scanForm = new QUANLYKHO.UI.BarcodeScanForm
                {
                    Mode = "BOTH",
                    StartPosition = FormStartPosition.CenterParent,
                    Owner = this
                };

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
                    XtraMessageBox.Show("Không tìm thấy sản phẩm với barcode này!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra nhà máy giống hệt cách trong btnbracode_Click
                if (product.FactoryId != CurrentFactoryId && CurrentFactoryId != 0)
                {
                    XtraMessageBox.Show("Bạn không có quyền xuất nhập kho sản phẩm này!",
                                        "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string action = scanForm.ActionChosen;

                if (action == "NHAP")
                {
                    if (!CanImport && !IsSuperAdmin)
                    {
                        XtraMessageBox.Show("Bạn không có quyền nhập kho.", "Phân quyền",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var frmNhap = new nhapkho
                    {
                        CurrentUserId = CurrentUserId,
                        CurrentFactoryId = CurrentFactoryId
                    };
                    frmNhap.SetBarcode(barcode);
                    frmNhap.ShowDialog();
                }
                else if (action == "XUAT")
                {
                    if (!CanExport && !IsSuperAdmin)
                    {
                        XtraMessageBox.Show("Bạn không có quyền xuất kho.", "Phân quyền",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var frmXuat = new xuatkho(product.Id, CurrentUserId);
                    frmXuat.ShowDialog();
                }
                else
                {
                    XtraMessageBox.Show("Hành động không hợp lệ.", "Lỗi",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        private void LoadPieChart()
        {
            using var db = new QLKhoDbContext();

            decimal nhap = db.ImportDetails
                .Where(d => d.Import.FactoryId == CurrentFactoryId)
                .Sum(d => (decimal?)d.Quantity) ?? 0;

            decimal xuat = db.ExportDetails
                .Where(d => d.Export.FactoryId == CurrentFactoryId)
                .Sum(d => (decimal?)d.Quantity) ?? 0;

            decimal ton = db.Products
                .Where(p => p.FactoryId == CurrentFactoryId)
                .Sum(p => (decimal?)p.CurrentQuantity) ?? 0;

            chart_data.Series.Clear();

            var series = new DevExpress.XtraCharts.Series("Thống kê", DevExpress.XtraCharts.ViewType.Pie);

            series.Points.Add(new DevExpress.XtraCharts.SeriesPoint("Nhập", nhap));
            series.Points.Add(new DevExpress.XtraCharts.SeriesPoint("Xuất", xuat));
            series.Points.Add(new DevExpress.XtraCharts.SeriesPoint("Tồn", ton));

            series.Label.TextPattern = "{A}: {V:#,##0} ({VP:p0})";
            series.ToolTipPointPattern = "{A}: {V:#,##0} ({VP:p0})";

            // Hiệu ứng pie
            var pieView = series.View as DevExpress.XtraCharts.PieSeriesView;
            if (pieView != null)
            {
                pieView.ExplodeMode = DevExpress.XtraCharts.PieExplodeMode.UsePoints;
                pieView.ExplodedDistancePercentage = 10;
            }

            // 👉 Bật tooltip
            chart_data.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;

            // 👉 FIX: dùng ToolTipController để bỏ delay
            var toolTipController = new DevExpress.Utils.ToolTipController();
            toolTipController.InitialDelay = 0; // hiện ngay
            toolTipController.ReshowDelay = 0;  // không delay khi rê
            toolTipController.AutoPopDelay = 5000;

            chart_data.ToolTipController = toolTipController;

            chart_data.Series.Add(series);
        }
        private void LoadPieChartCategory()
        {
            using var db = new QLKhoDbContext();

            var data = db.Products
                .Where(p => p.FactoryId == CurrentFactoryId)
                .Include(p => p.Category)
                .GroupBy(p => p.Category.code)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(x => (decimal?)x.CurrentQuantity) ?? 0
                })
                .Where(x => x.Total > 0)
                .OrderByDescending(x => x.Total)
                .ToList();

            chart_datacategory.Series.Clear();

            // Không có dữ liệu thì thoát
            if (!data.Any())
                return;

            var series = new DevExpress.XtraCharts.Series(
                "Tồn kho theo loại",
                DevExpress.XtraCharts.ViewType.Pie
            );

            // Add dữ liệu
            foreach (var item in data)
            {
                series.Points.Add(
                    new DevExpress.XtraCharts.SeriesPoint(item.Category, item.Total)
                );
            }
            series.Label.TextPattern = "{A}: {VP:p0}";

            var label = series.Label as DevExpress.XtraCharts.PieSeriesLabel;
            if (label != null)
            {
                label.Position = DevExpress.XtraCharts.PieSeriesLabelPosition.Outside;
                label.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.Default;
            }

            var pieView = series.View as DevExpress.XtraCharts.PieSeriesView;
            if (pieView != null)
            {
                pieView.MinAllowedSizePercentage = 80; // 🔥 phóng to pie
                pieView.ExplodeMode = DevExpress.XtraCharts.PieExplodeMode.UsePoints;
                pieView.ExplodedDistancePercentage = 5;
            }

            chart_datacategory.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chart_datacategory.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Right;

            chart_datacategory.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;

            var toolTipController = new DevExpress.Utils.ToolTipController
            {
                InitialDelay = 0,
                ReshowDelay = 0,
                AutoPopDelay = 5000
            };

            chart_datacategory.ToolTipController = toolTipController;

            chart_datacategory.Series.Add(series);
        }

    }
}
