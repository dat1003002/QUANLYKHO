using DevExpress.Utils.Animation;
using DevExpress.XtraWaitForm;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QUANLYKHO
{
    public partial class fLoading : WaitForm
    {
        public fLoading()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;

            // Ẩn caption
            this.progressPanel1.Caption = "";
            this.progressPanel1.Description = "Vui lòng chờ...";

            // Nền tối đẹp (gradient nhẹ)
            this.progressPanel1.Appearance.BackColor = Color.White;
            this.progressPanel1.Appearance.BackColor2 = Color.White;
            this.progressPanel1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.progressPanel1.Appearance.Options.UseBackColor = true;

            this.progressPanel1.Appearance.ForeColor = Color.Black;
            this.progressPanel1.Appearance.Options.UseForeColor = true;

            // Font chữ đẹp
            this.progressPanel1.AppearanceDescription.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.progressPanel1.AppearanceDescription.ForeColor = Color.Black;
            this.progressPanel1.AppearanceDescription.Options.UseForeColor = true;

            this.progressPanel1.WaitAnimationType = WaitingAnimatorType.Ring;
        }

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }

        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }
    }
}