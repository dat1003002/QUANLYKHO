using DevExpress.XtraEditors;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using QUANLYKHO.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QUANLYKHO.UI
{
    public partial class xuatkho : Form
    {
        private readonly int _productId;
        private readonly int _userId;

        public xuatkho(int productId, int userId)
        {
            InitializeComponent();
            _productId = productId;
            _userId = userId;
            LoadProductInfo();
        }

        private void LoadProductInfo()
        {
            using var db = new QLKhoDbContext();
            var product = db.Products
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == _productId);

            if (product != null)
            {
                lb_name.Text = $"Nguyên liệu: {product.Name}";
                lb_quycach.Text = $"Quy cách: {product.Specification ?? "Không có"}";
                lb_CurrentQuantity.Text = $"Tồn hiện tại: {product.CurrentQuantity:n0}";
            }
            else
            {
                lb_name.Text = "Sản phẩm không tồn tại";
                lb_quycach.Text = "Quy cách: Không xác định";
                lb_CurrentQuantity.Text = "Tồn hiện tại: 0";
            }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txt_soluong.Text.Trim(), out decimal soLuongXuat) || soLuongXuat <= 0)
            {
                MessageBox.Show("Số lượng xuất phải là số dương lớn hơn 0!", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_soluong.Focus();
                txt_soluong.SelectAll();
                return;
            }

            string lyDo = txt_lydo.Text?.Trim();
            if (string.IsNullOrWhiteSpace(lyDo))
            {
                MessageBox.Show("Vui lòng nhập lý do xuất kho.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_lydo.Focus();
                return;
            }

            string recipient = txt_RecipientName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(recipient))
            {
                MessageBox.Show("Vui lòng nhập tên người nhận.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_RecipientName.Focus();
                return;
            }

            try
            {
                using var db = new QLKhoDbContext();

                var product = db.Products
                    .Include(p => p.Factory)
                    .FirstOrDefault(p => p.Id == _productId);

                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ==================== KIỂM TRA FACTORY ====================
                // Lấy CurrentFactoryId từ form Home (nếu mở từ Home)
                var homeForm = Application.OpenForms["Home"] as Home;
                int currentFactoryId = homeForm?.CurrentFactoryId ?? 0;

                if (currentFactoryId != 0 && product.FactoryId != currentFactoryId)
                {
                    MessageBox.Show($"Sản phẩm này thuộc nhà máy khác.\n\n" +
                                    $"Sản phẩm: {product.Name}\n" +
                                    $"Nhà máy hiện tại: {currentFactoryId}\n" +
                                    $"Nhà máy của sản phẩm: {product.FactoryId}\n\n" +
                                    "Bạn không được phép xuất kho sản phẩm này.",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra tồn kho
                if (soLuongXuat > product.CurrentQuantity)
                {
                    MessageBox.Show($"Không đủ tồn kho!\nTồn hiện tại: {product.CurrentQuantity:n0}",
                        "Không đủ hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ==================== THỰC HIỆN XUẤT KHO ====================
                int factoryId = product.FactoryId;

                var exportDetail = new ExportDetail
                {
                    ProductId = _productId,
                    Quantity = soLuongXuat
                };

                var export = new Export
                {
                    ExportCode = "XK" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ExportDate = DateTime.Now,
                    Reason = lyDo,
                    RecipientName = recipient,
                    FactoryId = factoryId,
                    CreatedById = _userId,
                    Details = new List<ExportDetail> { exportDetail }
                };

                db.Exports.Add(export);

                product.CurrentQuantity -= soLuongXuat;
                product.TotalExported += soLuongXuat;

                db.SaveChanges();

                MessageBox.Show("Xuất kho thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh uc_xuatkho nếu đang mở
                var uc = Application.OpenForms.OfType<Form>()
                    .SelectMany(f => f.Controls.OfType<uc_xuatkho>())
                    .FirstOrDefault();

                if (uc != null)
                    uc.RefreshData();

                this.Close();
            }
            catch (DbUpdateException dbEx)
            {
                string errorMsg = "Lỗi khi lưu dữ liệu:\n\n" +
                                (dbEx.InnerException?.Message ?? dbEx.Message);
                if (dbEx.InnerException?.InnerException != null)
                    errorMsg += "\n\nChi tiết: " + dbEx.InnerException.InnerException.Message;
                MessageBox.Show(errorMsg, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string fullError = $"Lỗi không xác định:\n{ex.Message}";
                if (ex.InnerException != null)
                    fullError += $"\n\nInner: {ex.InnerException.Message}";
                MessageBox.Show(fullError, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}