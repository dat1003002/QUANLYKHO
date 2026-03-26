using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.model;
using QUANLYKHO.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;

namespace QUANLYKHO.UI
{
    public partial class Chitietkiemkho : Form
    {
        private readonly int _inventoryCheckId;
        private readonly int _currentUserId;
        private readonly HashSet<int> _dirtyRows = new HashSet<int>();
        private bool _hasUnsavedChanges = false;
        private Series _seriesVariance;

        public Chitietkiemkho(int inventoryCheckId, int currentUserId)
        {
            InitializeComponent();
            InitializeChart();
            _inventoryCheckId = inventoryCheckId;
            _currentUserId = currentUserId;
            this.Text = $"Chi tiết phiếu kiểm kê #{_inventoryCheckId}";
            LoadCategoriesIntoComboBox();
            if (_inventoryCheckId > 0)
            {
                LoadDetailData();
            }
            else
            {
                XtraMessageBox.Show("Mã phiếu kiểm kê không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void LoadCategoriesIntoComboBox()
        {
            try
            {
                using var db = new QLKhoDbContext();
                var categories = db.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .Select(c => new { c.Id, c.Name })
                    .ToList();

                var displayList = new List<object>
                {
                    new { Id = 0, Name = "-- Tất cả danh mục --" }
                };
                displayList.AddRange(categories);

                cb_category.DataSource = null;
                cb_category.DataSource = displayList;
                cb_category.DisplayMember = "Name";
                cb_category.ValueMember = "Id";
                cb_category.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Không tải được danh sách danh mục.\n{ex.Message}",
                    "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cb_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_category.SelectedValue == null || gridViewInventoryCheckDetail == null)
                return;

            if (!int.TryParse(cb_category.SelectedValue.ToString(), out int selectedCategoryId))
                return;

            if (selectedCategoryId == 0)
            {
                string current = gridViewInventoryCheckDetail.ActiveFilterString ?? "";
                gridViewInventoryCheckDetail.ActiveFilterString = RemoveCategoryFilterPart(current);
            }
            else
            {
                ApplyCategoryFilter(selectedCategoryId);
            }
        }

        private void ApplyCategoryFilter(int categoryId)
        {
            string currentFilter = gridViewInventoryCheckDetail.ActiveFilterString ?? "";
            currentFilter = RemoveCategoryFilterPart(currentFilter);
            string newFilter = currentFilter;

            if (categoryId != 0)
            {
                string catFilter = $"[CategoryId] = {categoryId}";
                if (!string.IsNullOrWhiteSpace(currentFilter))
                {
                    newFilter = $"({currentFilter}) AND ({catFilter})";
                }
                else
                {
                    newFilter = catFilter;
                }
            }
            gridViewInventoryCheckDetail.ActiveFilterString = newFilter;

            if (gridViewInventoryCheckDetail.DataRowCount > 0)
            {
                gridViewInventoryCheckDetail.FocusedRowHandle = 0;
                gridViewInventoryCheckDetail.MakeRowVisible(0);
            }
        }

        private string RemoveCategoryFilterPart(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return "";
            var parts = filter.Split(new[] { " AND ", " OR " }, StringSplitOptions.RemoveEmptyEntries);
            var cleanedParts = parts
                .Where(p => !p.TrimStart().StartsWith("[CategoryId] =", StringComparison.OrdinalIgnoreCase))
                .ToList();
            return string.Join(" AND ", cleanedParts);
        }
        private void LoadDetailData()
        {
            try
            {
                using var db = new QLKhoDbContext();
                var check = db.InventoryChecks
                    .AsNoTracking()
                    .Include(ic => ic.CreatedBy)
                    .Include(ic => ic.ApprovedBy)
                    .FirstOrDefault(ic => ic.Id == _inventoryCheckId);

                if (check == null)
                {
                    XtraMessageBox.Show($"Không tìm thấy phiếu kiểm kê với ID = {_inventoryCheckId}",
                        "Không tìm thấy dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                bool isApproved = check.IsApproved;
                if (isApproved)
                {
                    btn_appover.Enabled = false;
                    btn_appover.Text = "ĐÃ DUYỆT";
                }
                else
                {
                    btn_appover.Enabled = (check.ApprovedById == _currentUserId);
                }

                var query = db.InventoryCheckDetails
                    .AsNoTracking()
                    .Where(d => d.InventoryCheckId == _inventoryCheckId)
                    .Include(d => d.Product)
                    .ThenInclude(p => p.Unit)
                    .Select(d => new
                    {
                        DetailId = d.Id,
                        ProductId = d.ProductId,
                        MaterialCode = d.Product == null ? "N/A" : (d.Product.MaterialCode ?? "N/A"),
                        ProductName = d.Product == null ? "N/A" : (d.Product.Name ?? "N/A"),
                        Unit = d.Product == null || d.Product.Unit == null ? "" : (d.Product.Unit.Name ?? ""),
                        SystemQuantity = d.SystemQuantity,
                        ActualQuantity = d.ActualQuantity,
                        Variance = d.ActualQuantity - d.SystemQuantity,
                        DetailNote = d.Note ?? "",
                        CategoryId = d.Product != null ? d.Product.CategoryId : 0
                    })
                    .OrderBy(x => x.ProductId)        // Sửa ở đây: theo ProductId từ thấp lên cao
                    .ToList();

                var dt = new DataTable();
                dt.Columns.Add("DetailId", typeof(int));
                dt.Columns.Add("ProductId", typeof(int));
                dt.Columns.Add("MaterialCode", typeof(string));
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("Unit", typeof(string));
                dt.Columns.Add("SystemQuantity", typeof(decimal));
                dt.Columns.Add("ActualQuantity", typeof(decimal));
                dt.Columns.Add("Variance", typeof(decimal));
                dt.Columns.Add("DetailNote", typeof(string));
                dt.Columns.Add("CategoryId", typeof(int));

                foreach (var row in query)
                {
                    dt.Rows.Add(
                        row.DetailId,
                        row.ProductId,
                        row.MaterialCode,
                        row.ProductName,
                        row.Unit,
                        row.SystemQuantity,
                        row.ActualQuantity,
                        row.Variance,
                        row.DetailNote,
                        row.CategoryId
                    );
                }

                gridControlInventoryCheckDetail.DataSource = dt;
                gridControlInventoryCheckDetail.ForceInitialize();
                gridControlInventoryCheckDetail.RefreshDataSource();

                if (gridViewInventoryCheckDetail != null && dt.Rows.Count > 0)
                {
                    gridViewInventoryCheckDetail.PopulateColumns();
                    ConfigureDetailGridColumns();

                    if (isApproved)
                    {
                        gridViewInventoryCheckDetail.OptionsBehavior.Editable = false;
                        gridViewInventoryCheckDetail.Columns["ActualQuantity"].OptionsColumn.AllowEdit = false;
                        gridViewInventoryCheckDetail.Columns["DetailNote"].OptionsColumn.AllowEdit = false;
                    }
                    else
                    {
                        gridViewInventoryCheckDetail.CellValueChanging += GridView_CellValueChanging;
                        gridViewInventoryCheckDetail.CellValueChanged += GridView_CellValueChanged;
                        gridViewInventoryCheckDetail.BeforeLeaveRow += GridView_BeforeLeaveRow;
                        gridViewInventoryCheckDetail.RowCellStyle += GridView_RowCellStyle;
                        gridViewInventoryCheckDetail.ValidatingEditor += GridView_ValidatingEditor;
                        gridViewInventoryCheckDetail.CustomColumnDisplayText += GridView_CustomColumnDisplayText;
                        gridViewInventoryCheckDetail.ShownEditor += GridView_ShownEditor;
                        gridViewInventoryCheckDetail.CustomDrawCell += GridViewDetail_CustomDrawCell;
                    }

                    gridViewInventoryCheckDetail.BestFitColumns();
                    gridViewInventoryCheckDetail.FocusedRowHandle = 0;
                    gridViewInventoryCheckDetail.FocusedColumn = gridViewInventoryCheckDetail.Columns["ActualQuantity"];
                }

                BindVarianceChart();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi tải chi tiết:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeChart()
        {
            if (chartControl_chenhlech == null) return;
            chartControl_chenhlech.Series.Clear();
            _seriesVariance = new Series("Chênh lệch", ViewType.Bar)
            {
                ArgumentDataMember = "ProductName",
                LabelsVisibility = DefaultBoolean.True,
            };
            _seriesVariance.ValueDataMembers.AddRange(new string[] { "Variance" });

            var barLabel = _seriesVariance.Label as BarSeriesLabel;
            if (barLabel != null)
            {
                barLabel.Position = BarSeriesLabelPosition.Top;
                barLabel.TextPattern = "{V:n0}";
            }
            else
            {
                _seriesVariance.Label.TextPattern = "{V:n0}";
            }

            var barView = (SideBySideBarSeriesView)_seriesVariance.View;
            barView.BarWidth = 0.7;

            chartControl_chenhlech.Series.Add(_seriesVariance);
            chartControl_chenhlech.Legend.Visibility = DefaultBoolean.False;
            chartControl_chenhlech.BorderOptions.Visibility = DefaultBoolean.False;

            var diagram = (XYDiagram)chartControl_chenhlech.Diagram;
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = true;
            diagram.AxisY.Title.Text = "Chênh lệch ";
            diagram.AxisY.Title.Visibility = DefaultBoolean.True;
            diagram.AxisX.Label.Angle = -45;
            diagram.AxisX.Label.TextPattern = "{A}";

            chartControl_chenhlech.CustomDrawSeriesPoint += ChartControl_CustomDrawSeriesPoint;
        }

        private void ChartControl_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            if (e.SeriesPoint == null) return;
            var value = e.SeriesPoint.Values[0];
            if (value > 0)
                e.SeriesDrawOptions.Color = Color.ForestGreen;
            else if (value < 0)
                e.SeriesDrawOptions.Color = Color.IndianRed;
            else
                e.SeriesDrawOptions.Color = Color.Gray;
        }

        private void BindVarianceChart()
        {
            if (chartControl_chenhlech == null || gridControlInventoryCheckDetail.DataSource == null) return;

            var dt = gridControlInventoryCheckDetail.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                chartControl_chenhlech.DataSource = null;
                return;
            }

            var data = dt.AsEnumerable()
                .Select(row => new
                {
                    ProductName = row.Field<string>("ProductName") ?? "N/A",
                    Variance = row.Field<decimal>("Variance")
                })
                .OrderByDescending(x => Math.Abs(x.Variance))
                .ThenBy(x => x.ProductName)
                .Take(20)
                .ToList();

            chartControl_chenhlech.DataSource = data;
            chartControl_chenhlech.RefreshData();
        }

        private void ConfigureDetailGridColumns()
        {
            var gv = gridViewInventoryCheckDetail;
            if (gv == null || gv.Columns.Count == 0) return;

            gv.OptionsView.ShowIndicator = false;
            gv.OptionsBehavior.Editable = true;
            gv.OptionsView.ColumnAutoWidth = true;
            gv.OptionsView.ShowGroupPanel = false;

            gv.Columns["DetailId"].Visible = false;
            gv.Columns["DetailId"].OptionsColumn.ShowInCustomizationForm = false;

            gv.Columns["ProductId"].Visible = false;
            gv.Columns["ProductId"].OptionsColumn.ShowInCustomizationForm = false;

            gv.Columns["CategoryId"].Visible = false;
            gv.Columns["CategoryId"].OptionsColumn.ShowInCustomizationForm = false;

            gv.Columns["DetailNote"].Visible = false;
            gv.Columns["DetailNote"].OptionsColumn.ShowInCustomizationForm = false;

            gv.Columns["MaterialCode"].Caption = "Mã NVL";
            gv.Columns["MaterialCode"].Width = 120;
            gv.Columns["MaterialCode"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;

            gv.Columns["ProductName"].Caption = "Tên sản phẩm";
            gv.Columns["ProductName"].Width = 300;

            gv.Columns["Unit"].Caption = "Đơn vị";
            gv.Columns["Unit"].Width = 90;
            gv.Columns["Unit"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;

            var systemCol = gv.Columns["SystemQuantity"];
            if (systemCol != null)
            {
                systemCol.Caption = "SL hệ thống";
                systemCol.Width = 130;
                systemCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                systemCol.DisplayFormat.FormatType = FormatType.Numeric;
                systemCol.DisplayFormat.FormatString = "n0";

                var textEdit = new RepositoryItemTextEdit
                {
                    Mask = { MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx, EditMask = @"[0-9]*" },
                    DisplayFormat = { FormatType = FormatType.Numeric, FormatString = "n0" },
                    EditFormat = { FormatType = FormatType.Numeric, FormatString = "n0" },
                    NullText = ""
                };
                systemCol.ColumnEdit = textEdit;
                systemCol.OptionsColumn.AllowEdit = true;
            }

            var actualCol = gv.Columns["ActualQuantity"];
            if (actualCol != null)
            {
                actualCol.Caption = "SL thực tế";
                actualCol.Width = 130;
                actualCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;

                var textEdit = new RepositoryItemTextEdit
                {
                    Mask = { MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx, EditMask = @"[0-9]*" },
                    AllowNullInput = DefaultBoolean.True,
                    NullText = "",
                    DisplayFormat = { FormatType = FormatType.Numeric, FormatString = "n0" },
                    EditFormat = { FormatType = FormatType.Numeric, FormatString = "n0" }
                };
                actualCol.ColumnEdit = textEdit;
                actualCol.OptionsColumn.AllowEdit = true;
            }

            var varianceCol = gv.Columns["Variance"];
            if (varianceCol != null)
            {
                varianceCol.Caption = "Chênh lệch";
                varianceCol.Width = 160;
                varianceCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                varianceCol.DisplayFormat.FormatType = FormatType.Numeric;
                varianceCol.DisplayFormat.FormatString = "n0";
                varianceCol.OptionsColumn.ReadOnly = true;
                varianceCol.OptionsColumn.AllowEdit = false;
            }

            gv.ApplyHeader();
            gv.BestFitColumns();
        }

        private void GridView_ShownEditor(object sender, EventArgs e)
        {
            var view = sender as GridView;
            if (view?.FocusedColumn?.FieldName != "ActualQuantity") return;
            var editor = view.ActiveEditor as TextEdit;
            if (editor == null) return;
            var val = view.GetFocusedValue();
            bool isZeroOrNull = val == null || (val is decimal d && d == 0) || (decimal.TryParse(val?.ToString(), out decimal dd) && dd == 0);
            if (isZeroOrNull)
            {
                editor.EditValue = null;
                editor.Text = "";
            }
        }

        private void GridView_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            var view = sender as GridView;
            if (view == null) return;
            if (e.Column.FieldName != "ActualQuantity" && e.Column.FieldName != "SystemQuantity") return;

            decimal actual = 0;
            decimal system = 0;

            var actualObj = view.GetRowCellValue(e.RowHandle, "ActualQuantity");
            if (actualObj != null && actualObj != DBNull.Value && decimal.TryParse(actualObj.ToString(), out decimal a))
                actual = a;

            var systemObj = view.GetRowCellValue(e.RowHandle, "SystemQuantity");
            if (systemObj != null && systemObj != DBNull.Value && decimal.TryParse(systemObj.ToString(), out decimal s))
                system = s;

            if (e.Column.FieldName == "ActualQuantity")
                actual = (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())) ? 0 : Convert.ToDecimal(e.Value);

            if (e.Column.FieldName == "SystemQuantity")
                system = (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())) ? 0 : Convert.ToDecimal(e.Value);

            view.SetRowCellValue(e.RowHandle, "Variance", actual - system);
        }

        private void GridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column?.FieldName != "ActualQuantity" &&
                e.Column?.FieldName != "DetailNote" &&
                e.Column?.FieldName != "SystemQuantity") return;

            MarkRowAsDirty(e.RowHandle);
            SaveSingleRow(e.RowHandle);

            var view = sender as GridView;
            if (view == null) return;

            decimal actual = 0;
            decimal system = 0;

            var actualObj = view.GetRowCellValue(e.RowHandle, "ActualQuantity");
            if (actualObj != null && actualObj != DBNull.Value)
                actual = Convert.ToDecimal(actualObj);

            var systemObj = view.GetRowCellValue(e.RowHandle, "SystemQuantity");
            if (systemObj != null && systemObj != DBNull.Value)
                system = Convert.ToDecimal(systemObj);

            view.SetRowCellValue(e.RowHandle, "Variance", actual - system);
            BindVarianceChart();
        }

        private void GridView_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (e.RowHandle < 0 || !_dirtyRows.Contains(e.RowHandle)) return;
            try
            {
                SaveSingleRow(e.RowHandle);
                _dirtyRows.Remove(e.RowHandle);
                if (_dirtyRows.Count == 0) _hasUnsavedChanges = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dòng này:\n{ex.Message}\nBạn có thể sửa lại hoặc thoát mà không lưu.",
                    "Lỗi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Allow = false;
            }
        }

        private void GridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (_dirtyRows.Contains(e.RowHandle))
            {
                e.Appearance.BackColor = Color.LightGoldenrodYellow;
            }
        }

        private void GridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (gridViewInventoryCheckDetail.FocusedColumn?.FieldName != "ActualQuantity") return;
            if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                e.Valid = true;
                return;
            }
            if (!decimal.TryParse(e.Value.ToString(), out decimal val))
            {
                e.Valid = false;
                e.ErrorText = "Vui lòng nhập số nguyên hợp lệ";
                return;
            }
            if (val < 0)
            {
                e.Valid = false;
                e.ErrorText = "Số lượng thực tế không được âm";
            }
        }

        private void GridView_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column?.FieldName == "ActualQuantity")
            {
                if (e.Value == null || e.Value == DBNull.Value || (e.Value is decimal v && v == 0))
                {
                    e.DisplayText = "";
                }
                else if (e.Value is decimal val)
                {
                    e.DisplayText = val.ToString("N0");
                }
            }
        }

        private void GridViewDetail_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column?.FieldName != "Variance" || e.CellValue is not decimal variance) return;
            if (variance > 0)
            {
                e.Appearance.ForeColor = Color.DarkGreen;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            else if (variance < 0)
            {
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        private void SaveSingleRow(int rowHandle)
        {
            int detailId = Convert.ToInt32(gridViewInventoryCheckDetail.GetRowCellValue(rowHandle, "DetailId"));
            using var db = new QLKhoDbContext();
            var detail = db.InventoryCheckDetails.Find(detailId);
            if (detail == null) return;

            var actualObj = gridViewInventoryCheckDetail.GetRowCellValue(rowHandle, "ActualQuantity");
            detail.ActualQuantity = (actualObj == null || actualObj == DBNull.Value) ? 0m : Convert.ToDecimal(actualObj);

            var systemObj = gridViewInventoryCheckDetail.GetRowCellValue(rowHandle, "SystemQuantity");
            detail.SystemQuantity = (systemObj == null || systemObj == DBNull.Value) ? 0m : Convert.ToDecimal(systemObj);

            detail.Note = gridViewInventoryCheckDetail.GetRowCellValue(rowHandle, "DetailNote")?.ToString() ?? "";

            db.SaveChanges();

            var newVariance = detail.ActualQuantity - detail.SystemQuantity;
            gridViewInventoryCheckDetail.SetRowCellValue(rowHandle, "Variance", newVariance);
            gridViewInventoryCheckDetail.RefreshRow(rowHandle);
            BindVarianceChart();
        }

        private void SaveAllChanges()
        {
            if (_dirtyRows.Count == 0)
            {
                MessageBox.Show("Không có thay đổi nào cần lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var db = new QLKhoDbContext();
                foreach (var rh in _dirtyRows.ToList())
                {
                    if (rh < 0) continue;
                    int detailId = Convert.ToInt32(gridViewInventoryCheckDetail.GetRowCellValue(rh, "DetailId"));
                    var detail = db.InventoryCheckDetails.Find(detailId);
                    if (detail == null) continue;

                    var actualObj = gridViewInventoryCheckDetail.GetRowCellValue(rh, "ActualQuantity");
                    detail.ActualQuantity = (actualObj == null || actualObj == DBNull.Value) ? 0m : Convert.ToDecimal(actualObj);
                    detail.Note = gridViewInventoryCheckDetail.GetRowCellValue(rh, "DetailNote")?.ToString() ?? "";
                }
                db.SaveChanges();

                foreach (var rh in _dirtyRows.ToList())
                {
                    var system = Convert.ToDecimal(gridViewInventoryCheckDetail.GetRowCellValue(rh, "SystemQuantity"));
                    var actualObj = gridViewInventoryCheckDetail.GetRowCellValue(rh, "ActualQuantity");
                    var actual = (actualObj == null || actualObj == DBNull.Value) ? 0m : Convert.ToDecimal(actualObj);
                    gridViewInventoryCheckDetail.SetRowCellValue(rh, "Variance", actual - system);
                    gridViewInventoryCheckDetail.RefreshRow(rh);
                }

                _dirtyRows.Clear();
                _hasUnsavedChanges = false;
                MessageBox.Show("Đã lưu tất cả thay đổi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindVarianceChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu tất cả:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MarkRowAsDirty(int rowHandle)
        {
            if (rowHandle < 0) return;
            _dirtyRows.Add(rowHandle);
            _hasUnsavedChanges = true;
            gridViewInventoryCheckDetail.RefreshRow(rowHandle);
        }

        private void btn_appover_Click(object sender, EventArgs e)
        {
            if (_inventoryCheckId <= 0)
            {
                MessageBox.Show("Không xác định được phiếu kiểm kê.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using var db = new QLKhoDbContext();
                var check = db.InventoryChecks
                    .Include(ic => ic.Details)
                    .FirstOrDefault(ic => ic.Id == _inventoryCheckId);

                if (check == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu kiểm kê.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (check.IsApproved)
                {
                    MessageBox.Show("Phiếu kiểm kê này đã được duyệt trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (check.ApprovedById == null || check.ApprovedById != _currentUserId)
                {
                    MessageBox.Show("Bạn không có quyền duyệt phiếu kiểm kê này.\nChỉ người được chỉ định mới có thể thực hiện.",
                        "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                check.IsApproved = true;
                check.ApprovedDate = DateTime.Now;
                db.SaveChanges();

                MessageBox.Show("Phiếu kiểm kê đã được duyệt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_appover.Enabled = false;
                btn_appover.Text = "ĐÃ DUYỆT";
                LoadDetailData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi duyệt phiếu:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hasUnsavedChanges && _dirtyRows.Count > 0)
            {
                var dr = MessageBox.Show("Có thay đổi chưa lưu. Bạn muốn lưu trước khi thoát không?",
                    "Xác nhận", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SaveAllChanges();
                }
                else if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            base.OnFormClosing(e);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
            this.Dispose();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (gridViewInventoryCheckDetail == null)
            {
                XtraMessageBox.Show("Grid chi tiết chưa được khởi tạo.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string keyword = (txt_search?.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                gridViewInventoryCheckDetail.ActiveFilterString = null;
                gridViewInventoryCheckDetail.ApplyFindFilter(null);
                txt_search?.Focus();
                txt_search?.SelectAll();
                return;
            }

            string safeKeyword = keyword.Replace("'", "''");
            string filter = $"[ProductName] LIKE '%{safeKeyword}%'";

            try
            {
                string current = gridViewInventoryCheckDetail.ActiveFilterString;
                if (!string.IsNullOrWhiteSpace(current))
                {
                    gridViewInventoryCheckDetail.ActiveFilterString = $"({current}) AND ({filter})";
                }
                else
                {
                    gridViewInventoryCheckDetail.ActiveFilterString = filter;
                }

                int visibleRows = gridViewInventoryCheckDetail.DataRowCount;
                if (visibleRows == 0)
                {
                    XtraMessageBox.Show($"Không tìm thấy sản phẩm nào chứa \"{keyword}\".",
                        "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    gridViewInventoryCheckDetail.FocusedRowHandle = 0;
                    gridViewInventoryCheckDetail.MakeRowVisible(0, false);
                    gridViewInventoryCheckDetail.FocusedColumn = gridViewInventoryCheckDetail.Columns["ProductName"];
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi áp dụng bộ lọc tìm kiếm:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_history_Click(object sender, EventArgs e)
        {
            int rowHandle = gridViewInventoryCheckDetail.FocusedRowHandle;
            if (rowHandle < 0)
            {
                XtraMessageBox.Show("Vui lòng chọn một sản phẩm để xem lịch sử xuất kho.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int productId = Convert.ToInt32(gridViewInventoryCheckDetail.GetRowCellValue(rowHandle, "ProductId"));

            if (productId <= 0)
            {
                XtraMessageBox.Show("Không tìm thấy mã sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using var frm = new Lichsuxuat(productId);
            frm.ShowDialog();
        }
    }
}