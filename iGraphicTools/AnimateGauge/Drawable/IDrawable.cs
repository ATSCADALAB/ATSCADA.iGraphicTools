using System.Drawing;

namespace ATSCADA.iGraphicTools.AnimateGauge.Drawable
{
    public interface IDrawable
    {
        void Draw(Graphics g);

        void CalculatePaths(RectangleF container);

        Region GetRedrawRegion();
    }
}
