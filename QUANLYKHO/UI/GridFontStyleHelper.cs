using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using System.Drawing;

namespace QUANLYKHO.UI
{
    public static class GridFontStyleHelper
    {
        public static void ApplyHeader(this GridView gv)
        {
            var h = gv.Appearance.HeaderPanel;
            h.Font = new Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            h.ForeColor = Color.Black;
            h.TextOptions.HAlignment = HorzAlignment.Center;
            h.Options.UseFont = true;
            h.Options.UseForeColor = true;
            h.Options.UseTextOptions = true;
        }
    }
}