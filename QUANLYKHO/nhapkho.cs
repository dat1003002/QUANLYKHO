using DevExpress.XtraEditors;
using Microsoft.EntityFrameworkCore;
using QUANLYKHO.Data;
using QUANLYKHO.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QUANLYKHO
{
    public partial class nhapkho : Form
    {
        public int ProductId { get; set; }
        public int CurrentUserId { get; set; } = 0;
        public int CurrentFactoryId { get; set; } = 0;        // ← ĐÃ THÊM DÒNG NÀY

        public nhapkho()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
        }

        public void LoadProduct(int productId)
        {
            ProductId = productId;
            using var db = new QLKhoDbContext();
            var product = db.Products
                .Include(p => p.Unit)
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                lb_name.Text = "Nguyên Liệu: " + product.Name;
                lb_quycach.Text = "Quy cách: " + (product.Specification ?? "Không có");
            }
            else
            {
                lb_name.Text = "Sản phẩm không tồn tại";
                lb_quycach.Text = "Quy cách: Không xác định";
            }
        }

        public void SetBarcode(string barcode)
        {
            using var db = new QLKhoDbContext();
            var product = db.Products.FirstOrDefault(p => p.Barcode == barcode);

            if (product != null)
            {
                LoadProduct(product.Id);
                txt_soluong.Focus();
                txt_soluong.SelectAll();
            }
            else
            {
                MessageBox.Show($"Không tìm thấy sản phẩm với barcode: {barcode}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void btn_Huy_Click(object sender, EventArgs e) => this.Close();

        private void btn_close_Click(object sender, EventArgs e) => this.Close();

        private void btn_luu_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txt_soluong.Text.Trim(), out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số dương lớn hơn 0!", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_soluong.Focus();
                txt_soluong.SelectAll();
                return;
            }

            string note = txt_mota.Text?.Trim() ?? "";

            try
            {
                using var db = new QLKhoDbContext();
                var product = db.Products
                    .Include(p => p.Unit)
                    .Include(p => p.Factory)
                    .Include(p => p.Category)
                    .FirstOrDefault(p => p.Id == ProductId);

                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra Factory (tăng tính an toàn)
                if (product.FactoryId != CurrentFactoryId && CurrentFactoryId != 0)
                {
                    MessageBox.Show("Sản phẩm không thuộc nhà máy hiện tại!\nKhông thể nhập kho.",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int factoryId = product.FactoryId;

                var importDetail = new ImportDetail
                {
                    ProductId = ProductId,
                    Quantity = quantity
                };

                var import = new Import
                {
                    ImportCode = "NK" + DateTime.Now.ToString("yyMMddHHmmssfff"),
                    ImportDate = DateTime.Now,
                    Note = note,
                    FactoryId = factoryId,
                    CreatedById = CurrentUserId,
                    Details = new List<ImportDetail> { importDetail }
                };

                db.Imports.Add(import);

                product.CurrentQuantity += quantity;
                product.TotalImported += quantity;
                product.LastImportDate = DateTime.Now;
                product.LastImportById = CurrentUserId;

                db.SaveChanges();

                MessageBox.Show("Nhập kho thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}