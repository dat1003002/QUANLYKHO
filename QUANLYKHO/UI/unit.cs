using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using QUANLYKHO.model;

namespace QUANLYKHO.UI
{
    public partial class unit : DevExpress.XtraEditors.XtraForm
    {
        private bool _dataLoaded = false;

        public unit()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Quản lý Đơn vị tính";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            LoadUnitData();
        }

        private void unit_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !_dataLoaded && !DesignMode)
            {
                _dataLoaded = true;
                LoadUnitData();
            }
        }

        private void LoadUnitData()
        {
            try
            {
                using var context = new QLKhoDbContext();

                var units = context.Units
                    .AsNoTracking()
                    .Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u.Symbol
                    })
                    .OrderBy(u => u.Name)
                    .ToList();

                gv_unit.DataSource = units;

                if (gv_unit.MainView is GridView gridViewUnit)
                {
                    gridViewUnit.ApplyHeader();       

                    gridViewUnit.OptionsBehavior.Editable = false;
                    gridViewUnit.OptionsSelection.EnableAppearanceFocusedCell = false;
                    gridViewUnit.OptionsView.ShowGroupPanel = false;
                    gridViewUnit.OptionsView.ShowIndicator = false;
                    gridViewUnit.OptionsView.EnableAppearanceEvenRow = true;
                    gridViewUnit.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(245, 245, 250);

                    ConfigureColumn(gridViewUnit, "Id", visible: false);
                    ConfigureColumn(gridViewUnit, "Name", "Tên đơn vị", 250);
                    ConfigureColumn(gridViewUnit, "Symbol", "Ký hiệu", 120, HorzAlignment.Center);

                    gridViewUnit.BestFitColumns();
                }
                else
                {
                    MessageBox.Show("Không thể lấy GridView từ GridControl (gv_unit).\n" +
                        "Vui lòng kiểm tra tên GridView trong Designer phải là 'grid_unit'.",
                        "Lỗi cấu hình Grid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách đơn vị tính.\n\nChi tiết lỗi: " + ex.Message,
                    "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureColumn(GridView view, string fieldName, string caption = null,
            int width = 0, HorzAlignment? alignment = null, bool visible = true)
        {
            var column = view.Columns[fieldName];
            if (column == null) return;

            if (!string.IsNullOrEmpty(caption)) column.Caption = caption;
            if (width > 0) column.Width = width;
            if (alignment.HasValue) column.AppearanceCell.TextOptions.HAlignment = alignment.Value;
            column.Visible = visible;
        }

        private void grid_unit_RowClick(object sender, RowClickEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null || view.FocusedRowHandle < 0) return;

            txt_name.Text = view.GetFocusedRowCellValue("Name")?.ToString() ?? "";
            txt_symbol.Text = view.GetFocusedRowCellValue("Symbol")?.ToString() ?? "";
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string name = txt_name.Text.Trim();
            string symbol = txt_symbol.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập Tên đơn vị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_name.Focus(); txt_name.SelectAll();
                return;
            }
            if (name.Length > 100)
            {
                MessageBox.Show("Tên đơn vị không được vượt quá 100 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_name.Focus(); txt_name.SelectAll();
                return;
            }
            if (symbol.Length > 50)
            {
                MessageBox.Show("Ký hiệu không được vượt quá 50 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_symbol.Focus(); txt_symbol.SelectAll();
                return;
            }

            try
            {
                using var context = new QLKhoDbContext();

                if (context.Units.Any(u => u.Name == name))
                {
                    MessageBox.Show($"Tên đơn vị '{name}' đã tồn tại. Vui lòng nhập tên khác.",
                        "Trùng tên đơn vị", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_name.Focus(); txt_name.SelectAll();
                    return;
                }

                if (!string.IsNullOrEmpty(symbol) && context.Units.Any(u => u.Symbol == symbol))
                {
                    MessageBox.Show($"Ký hiệu '{symbol}' đã được sử dụng. Vui lòng nhập ký hiệu khác.",
                        "Trùng ký hiệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_symbol.Focus(); txt_symbol.SelectAll();
                    return;
                }

                var newUnit = new Unit { Name = name, Symbol = symbol };
                context.Units.Add(newUnit);
                context.SaveChanges();

                MessageBox.Show("Thêm đơn vị tính mới thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txt_name.Clear();
                txt_symbol.Clear();
                LoadUnitData();
                txt_name.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (grid_unit.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newName = txt_name.Text.Trim();
            string newSymbol = txt_symbol.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Tên đơn vị không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_name.Focus();
                return;
            }
            if (newName.Length > 100)
            {
                MessageBox.Show("Tên đơn vị tối đa 100 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_name.Focus();
                return;
            }
            if (newSymbol.Length > 50)
            {
                MessageBox.Show("Ký hiệu tối đa 50 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_symbol.Focus();
                return;
            }

            try
            {
                using var context = new QLKhoDbContext();
                int id = Convert.ToInt32(grid_unit.GetFocusedRowCellValue("Id"));

                var unitToUpdate = context.Units.Find(id);
                if (unitToUpdate == null) return;

                if (context.Units.Any(u => u.Name == newName && u.Id != id))
                {
                    MessageBox.Show("Tên đơn vị đã tồn tại!", "Trùng tên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(newSymbol) && context.Units.Any(u => u.Symbol == newSymbol && u.Id != id))
                {
                    MessageBox.Show("Ký hiệu đã tồn tại!", "Trùng ký hiệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                unitToUpdate.Name = newName;
                unitToUpdate.Symbol = newSymbol;
                context.SaveChanges();

                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUnitData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_unit.FocusedRowHandle < 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = grid_unit.GetFocusedRowCellValue("Name")?.ToString() ?? "";

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa đơn vị tính:\n\nTên: {name} ?\n\nHành động này không thể hoàn tác.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            try
            {
                using var context = new QLKhoDbContext();
                int id = Convert.ToInt32(grid_unit.GetFocusedRowCellValue("Id"));

                var unitToDelete = context.Units.Find(id);
                if (unitToDelete != null)
                {
                    context.Units.Remove(unitToDelete);
                    context.SaveChanges();

                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUnitData();
                    txt_name.Clear();
                    txt_symbol.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}