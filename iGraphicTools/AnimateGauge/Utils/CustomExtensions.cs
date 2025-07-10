using System.Drawing;
using System.Drawing.Drawing2D;

namespace ATSCADA.iGraphicTools.AnimateGauge.Utils
{
    public static class CustomExtensions
    {
        public static SizeF MeasureDisplayStringSize(
          this Graphics graphics,
          string text,
          Font font)
        {
            StringFormat genericTypographic = StringFormat.GenericTypographic;
            RectangleF layoutRect = new RectangleF(0.0f, 0.0f, 1000f, 1000f);
            CharacterRange[] ranges = new CharacterRange[1]
            {
        new CharacterRange(0, text.Length)
            };
            Region[] regionArray1 = new Region[1];
            genericTypographic.SetMeasurableCharacterRanges(ranges);
            Region[] regionArray2 = graphics.MeasureCharacterRanges(text, font, layoutRect, genericTypographic);
            RectangleF bounds = regionArray2[0].GetBounds(graphics);
            genericTypographic.Dispose();
            foreach (Region region in regionArray2)
                region.Dispose();
            return new SizeF(bounds.Right, bounds.Bottom);
        }

        public static void AddRoundedRectangle(
          this GraphicsPath path,
          RectangleF rect,
          int cornerRadius)
        {
            if (cornerRadius > 0)
            {
                path.StartFigure();
                path.AddArc(rect.X, rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 180f, 90f);
                path.AddLine(rect.X + (float)cornerRadius, rect.Y, rect.Right - (float)(cornerRadius * 2), rect.Y);
                path.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 270f, 90f);
                path.AddLine(rect.Right, rect.Y + (float)(cornerRadius * 2), rect.Right, rect.Y + rect.Height - (float)(cornerRadius * 2));
                path.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y + rect.Height - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 0.0f, 90f);
                path.AddLine(rect.Right - (float)(cornerRadius * 2), rect.Bottom, rect.X + (float)(cornerRadius * 2), rect.Bottom);
                path.AddArc(rect.X, rect.Bottom - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 90f, 90f);
                path.AddLine(rect.X, rect.Bottom - (float)(cornerRadius * 2), rect.X, rect.Y + (float)(cornerRadius * 2));
                path.CloseFigure();
            }
            else
                path.AddRectangle(rect);
        }
    }
}
