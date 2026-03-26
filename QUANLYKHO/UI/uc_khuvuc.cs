using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using QUANLYKHO.Data;
using QUANLYKHO.model;

namespace QUANLYKHO.UI
{
    public partial class uc_khuvuc : UserControl
    {
        public int CurrentFactoryId { get; set; } = 0;

        private int _currentEditingShelfId = 0;
        private int _currentEditingLocationId = 0;

        public uc_khuvuc()
        {
            InitializeComponent();
            SetupGrids();
            SetupCombobox();
        }

        private void SetupGrids()
        {
            SetupWarehouseGrid();
            SetupShelfGrid();
            SetupLocationGrid();
        }

        private void SetupWarehouseGrid()
        {
            var gv = gridViewWarehouse;
            gv.OptionsView.ShowGroupPanel = false;
            gv.OptionsBehavior.Editable = false;
            gv.OptionsSelection.EnableAppearanceFocusedCell = false;
            gv.Columns.Clear();

            var colSTT = gv.Columns.AddVisible("STT");
            colSTT.Caption = "STT";
            colSTT.Width = colSTT.MinWidth = colSTT.MaxWidth = 45;
            colSTT.OptionsColumn.FixedWidth = true;
            colSTT.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            colSTT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            colSTT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            gv.Columns.AddVisible("MaKho").Caption = "Mã kho";
            gv.Columns.AddVisible("Name").Caption = "Tên kho";
            gv.Columns.AddVisible("Description").Caption = "Mô tả";

            gv.CustomColumnDisplayText += GridViewWarehouse_CustomColumnDisplayText;
            gv.FocusedRowChanged += GridViewWarehouse_FocusedRowChanged;
            gv.ApplyHeader();
        }

        private void SetupShelfGrid()
        {
            var gv = gridViewShelf;
            gv.OptionsView.ShowGroupPanel = false;
            gv.OptionsBehavior.Editable = false;
            gv.OptionsSelection.EnableAppearanceFocusedCell = false;
            gv.Columns.Clear();

            var colSTT = gv.Columns.AddVisible("STT");
            colSTT.Caption = "STT";
            colSTT.Width = colSTT.MinWidth = colSTT.MaxWidth = 45;
            colSTT.OptionsColumn.FixedWidth = true;
            colSTT.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            colSTT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            colSTT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            gv.Columns.AddVisible("Name").Caption = "Tên kệ";
            gv.Columns.AddVisible("WarehouseName").Caption = "Kho";

            gv.CustomColumnDisplayText += GridViewShelf_CustomColumnDisplayText;
            gv.RowClick += gridViewShelf_RowClick;
            gv.ApplyHeader();
        }

        private void SetupLocationGrid()
        {
            var gv = gridViewLocation;
            gv.OptionsView.ShowGroupPanel = false;
            gv.OptionsBehavior.Editable = false;
            gv.OptionsSelection.EnableAppearanceFocusedCell = false;
            gv.Columns.Clear();

            var colSTT = gv.Columns.AddVisible("STT");
            colSTT.Caption = "STT";
            colSTT.Width = colSTT.MinWidth = colSTT.MaxWidth = 45;
            colSTT.OptionsColumn.FixedWidth = true;
            colSTT.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            colSTT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            colSTT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            gv.Columns.AddVisible("Code").Caption = "Mã vị trí";
            gv.Columns.AddVisible("Name").Caption = "Tên vị trí";
            gv.Columns.AddVisible("ShelfName").Caption = "Tên kệ";

            gv.CustomColumnDisplayText += GridViewLocation_CustomColumnDisplayText;
            gv.RowClick += gridViewLocation_RowClick;
            gv.ApplyHeader();
        }

        private void SetupCombobox()
        {
            cb_Warehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_Warehouse.SelectedIndexChanged += cb_Warehouse_SelectedIndexChanged;

            cb_Shelf.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_Shelf.SelectedIndexChanged += cb_Shelf_SelectedIndexChanged;
        }

        #region Custom Column Display Text
        private void GridViewWarehouse_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "STT" && e.ListSourceRowIndex >= 0)
                e.DisplayText = (e.ListSourceRowIndex + 1).ToString();
        }

        private void GridViewShelf_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "STT" && e.ListSourceRowIndex >= 0)
                e.DisplayText = (e.ListSourceRowIndex + 1).ToString();
        }

        private void GridViewLocation_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "STT" && e.ListSourceRowIndex >= 0)
                e.DisplayText = (e.ListSourceRowIndex + 1).ToString();
        }
        #endregion

        #region Event Handlers
        private void GridViewWarehouse_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0) return;

            txt_code.Text = gridViewWarehouse.GetRowCellValue(e.FocusedRowHandle, "MaKho")?.ToString();
            txt_name.Text = gridViewWarehouse.GetRowCellValue(e.FocusedRowHandle, "Name")?.ToString();
            txt_mota.Text = gridViewWarehouse.GetRowCellValue(e.FocusedRowHandle, "Description")?.ToString();

            LoadShelfData();
        }

        private void gridViewShelf_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            _currentEditingShelfId = Convert.ToInt32(gridViewShelf.GetRowCellValue(e.RowHandle, "Id"));
            txt_NameShelf.Text = gridViewShelf.GetRowCellValue(e.RowHandle, "Name")?.ToString() ?? "";

            string warehouseName = gridViewShelf.GetRowCellValue(e.RowHandle, "WarehouseName")?.ToString() ?? "";
            foreach (var item in cb_Warehouse.Items)
            {
                if (((dynamic)item).Name == warehouseName)
                {
                    cb_Warehouse.SelectedItem = item;
                    break;
                }
            }
            LoadLocationByShelfId(_currentEditingShelfId);
        }

        private void gridViewLocation_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            _currentEditingLocationId = Convert.ToInt32(gridViewLocation.GetRowCellValue(e.RowHandle, "Id"));

            txt_maLocation.Text = gridViewLocation.GetRowCellValue(e.RowHandle, "Code")?.ToString() ?? "";
            txt_nameLocation.Text = gridViewLocation.GetRowCellValue(e.RowHandle, "Name")?.ToString() ?? "";

            string shelfName = gridViewLocation.GetRowCellValue(e.RowHandle, "ShelfName")?.ToString() ?? "";
            foreach (var item in cb_Shelf.Items)
            {
                if (((dynamic)item).Name == shelfName)
                {
                    cb_Shelf.SelectedItem = item;
                    break;
                }
            }
        }

        private void cb_Warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Warehouse.SelectedItem == null)
            {
                gridControlShelf.DataSource = null;
                cb_Shelf.DataSource = null;
                gridControlLocation.DataSource = null;
                return;
            }

            dynamic item = cb_Warehouse.SelectedItem;
            int warehouseId = item.Id;

            int rowHandle = gridViewWarehouse.LocateByValue("Id", warehouseId);
            if (rowHandle >= 0)
                gridViewWarehouse.FocusedRowHandle = rowHandle;

            LoadShelfByWarehouseId(warehouseId);
            LoadShelfToCombobox(warehouseId);
        }

        private void cb_Shelf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Shelf.SelectedItem == null)
            {
                gridControlLocation.DataSource = null;
                return;
            }

            dynamic selectedShelf = cb_Shelf.SelectedItem;
            int shelfId = selectedShelf.Id;

            int rowHandle = gridViewShelf.LocateByValue("Id", shelfId);
            if (rowHandle >= 0)
                gridViewShelf.FocusedRowHandle = rowHandle;

            LoadLocationByShelfId(shelfId);
        }
        #endregion

        #region Load Data
        public void LoadData()
        {
            if (CurrentFactoryId <= 0) return;

            using (var db = new QLKhoDbContext())
            {
                var warehouses = db.Warehouses
                    .Where(x => x.FactoryId == CurrentFactoryId)
                    .Select(x => new { x.Id, x.MaKho, x.Name, x.Description })
                    .ToList();

                gridControlWarehouse.DataSource = warehouses;
                HideIdColumn(gridViewWarehouse);
                gridViewWarehouse.BestFitColumns();
            }

            LoadWarehouseToCombobox();

            gridViewWarehouse.FocusedRowHandle = -1;
            ClearWarehouseInput();

            if (cb_Warehouse.Items.Count > 0)
                cb_Warehouse.SelectedIndex = 0;
        }

        private void LoadWarehouseToCombobox()
        {
            if (CurrentFactoryId <= 0) return;

            using (var db = new QLKhoDbContext())
            {
                var list = db.Warehouses
                    .Where(x => x.FactoryId == CurrentFactoryId)
                    .Select(x => new { x.Id, x.MaKho, x.Name })
                    .OrderBy(x => x.Name)
                    .ToList();

                cb_Warehouse.DataSource = list;
                cb_Warehouse.DisplayMember = "Name";
                cb_Warehouse.ValueMember = "Id";
            }
        }

        private void LoadShelfToCombobox(int warehouseId)
        {
            if (warehouseId <= 0)
            {
                cb_Shelf.DataSource = null;
                return;
            }

            using (var db = new QLKhoDbContext())
            {
                var shelves = db.Shelves
                    .Where(s => s.WarehouseId == warehouseId)
                    .Select(s => new { s.Id, s.Name })
                    .OrderBy(s => s.Name)
                    .ToList();

                cb_Shelf.DataSource = shelves;
                cb_Shelf.DisplayMember = "Name";
                cb_Shelf.ValueMember = "Id";

                if (shelves.Count > 0)
                    cb_Shelf.SelectedIndex = 0;
            }
        }

        public void LoadShelfData()
        {
            if (gridViewWarehouse.FocusedRowHandle < 0) return;

            var idObj = gridViewWarehouse.GetRowCellValue(gridViewWarehouse.FocusedRowHandle, "Id");
            if (idObj == null) return;

            LoadShelfByWarehouseId((int)idObj);
        }

        private void LoadShelfByWarehouseId(int warehouseId)
        {
            using (var db = new QLKhoDbContext())
            {
                var data = db.Shelves
                    .Where(x => x.WarehouseId == warehouseId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        WarehouseName = x.Warehouse.Name
                    })
                    .ToList();

                gridControlShelf.DataSource = data;
                HideIdColumn(gridViewShelf);
                gridViewShelf.BestFitColumns();
            }
        }

        private void LoadLocationByShelfId(int shelfId)
        {
            using (var db = new QLKhoDbContext())
            {
                var data = db.Locations
                    .Where(x => x.ShelfId == shelfId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                        ShelfName = x.Shelf.Name
                    })
                    .ToList();

                gridControlLocation.DataSource = data;
                HideIdColumn(gridViewLocation);
                gridViewLocation.BestFitColumns();
            }
        }

        private void HideIdColumn(GridView gv)
        {
            var col = gv.Columns["Id"];
            if (col != null)
            {
                col.Visible = false;
                col.OptionsColumn.ShowInCustomizationForm = false;
            }
        }

        public void RefreshData() => LoadData();
        #endregion

        #region CRUD Warehouse
        private void btn_addWarehouse_Click(object sender, EventArgs e)
        {
            if (CurrentFactoryId <= 0 || string.IsNullOrWhiteSpace(txt_code.Text) || string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà máy và nhập đầy đủ Mã kho, Tên kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new QLKhoDbContext();
            db.Warehouses.Add(new Warehouse
            {
                MaKho = txt_code.Text.Trim(),
                Name = txt_name.Text.Trim(),
                Description = txt_mota.Text?.Trim() ?? "",
                FactoryId = CurrentFactoryId
            });
            db.SaveChanges();

            MessageBox.Show("Thêm kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshData();
            ClearWarehouseInput();
        }

        private void btn_editWarehouse_Click(object sender, EventArgs e)
        {
            if (gridViewWarehouse.FocusedRowHandle < 0) return;

            var idObj = gridViewWarehouse.GetRowCellValue(gridViewWarehouse.FocusedRowHandle, "Id");
            if (idObj == null) return;

            using var db = new QLKhoDbContext();
            var warehouse = db.Warehouses.Find((int)idObj);
            if (warehouse == null) return;

            warehouse.MaKho = txt_code.Text.Trim();
            warehouse.Name = txt_name.Text.Trim();
            warehouse.Description = txt_mota.Text?.Trim() ?? "";
            db.SaveChanges();

            MessageBox.Show("Cập nhật kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshData();
            ClearWarehouseInput();
        }

        private void btn_deleteWarehouse_Click(object sender, EventArgs e)
        {
            if (gridViewWarehouse.FocusedRowHandle < 0) return;

            var idObj = gridViewWarehouse.GetRowCellValue(gridViewWarehouse.FocusedRowHandle, "Id");
            if (idObj == null) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa kho này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var db = new QLKhoDbContext();
            var warehouse = db.Warehouses.Find((int)idObj);
            if (warehouse == null) return;

            db.Warehouses.Remove(warehouse);
            db.SaveChanges();

            MessageBox.Show("Xóa kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshData();
            ClearWarehouseInput();
        }

        private void ClearWarehouseInput()
        {
            txt_code.Clear();
            txt_name.Clear();
            txt_mota.Clear();
        }
        #endregion

        #region CRUD Shelf
        private void btn_addShelf_Click(object sender, EventArgs e)
        {
            if (cb_Warehouse.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn kho trước khi thêm kệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_NameShelf.Text))
            {
                MessageBox.Show("Vui lòng nhập tên kệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_NameShelf.Focus();
                return;
            }

            dynamic selected = cb_Warehouse.SelectedItem;
            int warehouseId = selected.Id;

            using var db = new QLKhoDbContext();
            db.Shelves.Add(new Shelf { Name = txt_NameShelf.Text.Trim(), WarehouseId = warehouseId });
            db.SaveChanges();

            MessageBox.Show("Thêm kệ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txt_NameShelf.Clear();
            LoadShelfByWarehouseId(warehouseId);
            LoadShelfToCombobox(warehouseId);
        }

        private void btn_editShelf_Click(object sender, EventArgs e)
        {
            if (_currentEditingShelfId == 0)
            {
                MessageBox.Show("Vui lòng chọn kệ cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_NameShelf.Text) || cb_Warehouse.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập tên kệ và chọn kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic selected = cb_Warehouse.SelectedItem;
            int newWarehouseId = selected.Id;

            using var db = new QLKhoDbContext();
            var shelf = db.Shelves.Find(_currentEditingShelfId);
            if (shelf == null) return;

            shelf.Name = txt_NameShelf.Text.Trim();
            shelf.WarehouseId = newWarehouseId;
            db.SaveChanges();

            MessageBox.Show("Cập nhật kệ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _currentEditingShelfId = 0;
            txt_NameShelf.Clear();
            LoadShelfByWarehouseId(newWarehouseId);
            LoadShelfToCombobox(newWarehouseId);
        }

        private void btn_deleteShelf_Click(object sender, EventArgs e)
        {
            if (_currentEditingShelfId == 0)
            {
                MessageBox.Show("Vui lòng chọn kệ cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa kệ này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var db = new QLKhoDbContext();
            var shelf = db.Shelves.Find(_currentEditingShelfId);
            if (shelf == null)
            {
                _currentEditingShelfId = 0;
                return;
            }

            db.Shelves.Remove(shelf);
            db.SaveChanges();

            MessageBox.Show("Xóa kệ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _currentEditingShelfId = 0;
            txt_NameShelf.Clear();

            if (gridViewWarehouse.FocusedRowHandle >= 0)
            {
                int currentId = (int)gridViewWarehouse.GetRowCellValue(gridViewWarehouse.FocusedRowHandle, "Id");
                LoadShelfByWarehouseId(currentId);
                LoadShelfToCombobox(currentId);
            }
            else if (cb_Warehouse.SelectedItem != null)
            {
                dynamic item = cb_Warehouse.SelectedItem;
                LoadShelfByWarehouseId(item.Id);
                LoadShelfToCombobox(item.Id);
            }
        }
        #endregion

        #region CRUD Location
        private void btn_addLocation_Click(object sender, EventArgs e)
        {
            if (cb_Shelf.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một kệ trước khi thêm vị trí.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_Shelf.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_maLocation.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã vị trí.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_maLocation.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_nameLocation.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên vị trí.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_nameLocation.Focus();
                return;
            }

            dynamic selectedShelf = cb_Shelf.SelectedItem;
            int shelfId = selectedShelf.Id;

            using (var db = new QLKhoDbContext())
            {
                bool exists = db.Locations.Any(l => l.ShelfId == shelfId && l.Code == txt_maLocation.Text.Trim());
                if (exists)
                {
                    MessageBox.Show("Mã vị trí này đã tồn tại trong kệ hiện tại.\nVui lòng nhập mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_maLocation.Focus();
                    txt_maLocation.SelectAll();
                    return;
                }

                var newLocation = new Location
                {
                    Code = txt_maLocation.Text.Trim(),
                    Name = txt_nameLocation.Text.Trim(),
                    ShelfId = shelfId
                };
                db.Locations.Add(newLocation);
                db.SaveChanges();
            }

            MessageBox.Show("Thêm vị trí thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txt_maLocation.Clear();
            txt_nameLocation.Clear();
            LoadLocationByShelfId(shelfId);
            txt_maLocation.Focus();
        }

        private void btn_editLocation_Click(object sender, EventArgs e)
        {
            if (_currentEditingLocationId == 0)
            {
                MessageBox.Show("Vui lòng chọn vị trí cần sửa từ bảng bên dưới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cb_Shelf.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn kệ chứa vị trí.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_Shelf.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_maLocation.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã vị trí.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_maLocation.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_nameLocation.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên vị trí.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_nameLocation.Focus();
                return;
            }

            dynamic selectedShelf = cb_Shelf.SelectedItem;
            int newShelfId = selectedShelf.Id;

            using (var db = new QLKhoDbContext())
            {
                var location = db.Locations.Find(_currentEditingLocationId);
                if (location == null)
                {
                    MessageBox.Show("Không tìm thấy vị trí này trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _currentEditingLocationId = 0;
                    return;
                }

                bool exists = db.Locations.Any(l =>
                    l.ShelfId == newShelfId &&
                    l.Code == txt_maLocation.Text.Trim() &&
                    l.Id != _currentEditingLocationId);

                if (exists)
                {
                    MessageBox.Show("Mã vị trí này đã tồn tại trong kệ được chọn.\nVui lòng nhập mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_maLocation.Focus();
                    txt_maLocation.SelectAll();
                    return;
                }

                location.Code = txt_maLocation.Text.Trim();
                location.Name = txt_nameLocation.Text.Trim();
                location.ShelfId = newShelfId;
                db.SaveChanges();
            }

            MessageBox.Show("Cập nhật vị trí thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _currentEditingLocationId = 0;
            txt_maLocation.Clear();
            txt_nameLocation.Clear();

            if (cb_Shelf.SelectedItem != null)
            {
                dynamic shelf = cb_Shelf.SelectedItem;
                LoadLocationByShelfId(shelf.Id);
            }
        }

        private void btn_deleteLocation_Click(object sender, EventArgs e)
        {
            if (_currentEditingLocationId == 0)
            {
                MessageBox.Show("Vui lòng chọn vị trí cần xóa từ bảng bên dưới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa vị trí này?\n\nHành động này không thể hoàn tác!",
                                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using (var db = new QLKhoDbContext())
            {
                var location = db.Locations.Find(_currentEditingLocationId);
                if (location == null)
                {
                    MessageBox.Show("Không tìm thấy vị trí này trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _currentEditingLocationId = 0;
                    return;
                }

                db.Locations.Remove(location);
                db.SaveChanges();
            }

            MessageBox.Show("Xóa vị trí thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _currentEditingLocationId = 0;
            txt_maLocation.Clear();
            txt_nameLocation.Clear();

            if (cb_Shelf.SelectedItem != null)
            {
                dynamic shelf = cb_Shelf.SelectedItem;
                LoadLocationByShelfId(shelf.Id);
            }
            else if (gridViewShelf.FocusedRowHandle >= 0)
            {
                int shelfId = Convert.ToInt32(gridViewShelf.GetRowCellValue(gridViewShelf.FocusedRowHandle, "Id"));
                LoadLocationByShelfId(shelfId);
            }
        }
        #endregion

        private void btn_unit_Click(object sender, EventArgs e)
        {
            using (var form = new unit())
            {
                form.ShowDialog();
            }
        }
    }
}