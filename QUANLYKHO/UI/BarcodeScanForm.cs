using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Windows.Compatibility;
using System.Threading.Tasks;
using QUANLYKHO.Data;
using QUANLYKHO.model;
using DevExpress.XtraEditors;

namespace QUANLYKHO.UI
{
    public partial class BarcodeScanForm : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private BarcodeReader barcodeReader;
        private PictureBox picBox;
        private Label lblStatus;
        private Button btnNhapKho;
        private Button btnXuatKho;
        private Button btnCancel;

        public string ScannedBarcode { get; private set; } = null;
        public string ActionChosen { get; private set; } = null;
        public string Mode { get; set; } = "BOTH";

        public BarcodeScanForm()
        {
            InitializeComponent();

            this.Text = "Quét Barcode";
            this.Size = new Size(640, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += BarcodeScanForm_FormClosing;

            picBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Black
            };
            this.Controls.Add(picBox);

            lblStatus = new Label
            {
                Text = "Đang quét...",
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(240, 240, 240),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(lblStatus);

            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240)
            };
            this.Controls.Add(panelButtons);

            btnNhapKho = new Button
            {
                Text = "Nhập kho",
                Width = 120,
                Height = 36,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                Visible = false
            };
            btnNhapKho.Click += BtnNhapKho_Click;
            panelButtons.Controls.Add(btnNhapKho);

            btnXuatKho = new Button
            {
                Text = "Xuất kho",
                Width = 120,
                Height = 36,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Visible = false,
                Margin = new Padding(10, 0, 0, 0)
            };
            btnXuatKho.Click += BtnXuatKho_Click;
            panelButtons.Controls.Add(btnXuatKho);

            btnCancel = new Button
            {
                Text = "Hủy",
                Width = 120,
                Height = 36,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Margin = new Padding(10, 0, 0, 0)
            };
            btnCancel.Click += (s, e) => this.Close();
            panelButtons.Controls.Add(btnCancel);

            barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new[] { BarcodeFormat.CODE_128, BarcodeFormat.QR_CODE, BarcodeFormat.EAN_13 }
                }
            };

            this.Load += (s, e) => StartScanning();
        }

        private void StartScanning()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                MessageBox.Show("Không tìm thấy camera nào!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            FilterInfo rearCamera = videoDevices
                .Cast<FilterInfo>()
                .FirstOrDefault(d => d.Name.ToLower().Contains("rear") || d.Name.ToLower().Contains("back"));

            FilterInfo selectedCamera = rearCamera ?? videoDevices[0];

            if (rearCamera == null)
            {
                var result = MessageBox.Show(
                    "Không tìm thấy camera sau!\nBạn có muốn dùng camera trước không?",
                    "Thông báo",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information
                );

                if (result != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }

            videoSource = new VideoCaptureDevice(selectedCamera.MonikerString);
            videoSource.NewFrame += video_NewFrame;
            videoSource.Start();
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (videoSource == null || !videoSource.IsRunning) return;

            try
            {
                using var bitmap = (Bitmap)eventArgs.Frame.Clone();

                var result = barcodeReader.Decode(bitmap);

                if (result != null)
                {
                    ScannedBarcode = result.Text?.Trim();

                    videoSource.SignalToStop();

                    this.Invoke(new Action(() =>
                    {
                        using var db = new QLKhoDbContext();
                        var product = db.Products.FirstOrDefault(p => p.Barcode == ScannedBarcode);

                        string display = product != null
                            ? $"Đã quét: {product.Name}"
                            : $"Đã quét: {ScannedBarcode} (không tìm thấy)";

                        lblStatus.Text = display;

                        if (Mode == "ONLY_XUAT")
                        {
                            ActionChosen = "XUAT";
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }

                        if (Mode == "ONLY_NHAP")
                        {
                            ActionChosen = "NHAP";
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }

                        btnNhapKho.Visible = true;
                        btnXuatKho.Visible = true;
                        btnCancel.Text = "Đóng";
                    }));

                    return;
                }

                picBox?.Invoke(new Action(() =>
                {
                    var old = picBox.Image;
                    picBox.Image = (Bitmap)bitmap.Clone();
                    old?.Dispose();
                }));
            }
            catch { }
        }

        private void BtnNhapKho_Click(object sender, EventArgs e)
        {
            ActionChosen = "NHAP";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnXuatKho_Click(object sender, EventArgs e)
        {
            ActionChosen = "XUAT";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BarcodeScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var source = videoSource;
            if (source != null && source.IsRunning)
            {
                source.SignalToStop();
                Task.Run(() => { try { source.WaitForStop(); } catch { } });
            }

            videoSource = null;

            if (picBox?.Image != null)
            {
                picBox.Image.Dispose();
                picBox.Image = null;
            }
        }
    }
}