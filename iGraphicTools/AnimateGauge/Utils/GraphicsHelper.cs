using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ATSCADA.iGraphicTools.AnimateGauge.Utils
{
    internal static class GraphicsHelper
    {
        private const double ROUNDED_RECT_RAD_PERCENT = 0.05;

        public static GraphicsPath GetGraphicsPath(Rectangle container)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = container;
            --rect.Width;
            --rect.Height;
            path.AddEllipse(rect);
            return path;
        }

        public static GraphicsPath Get3DShinePath(Rectangle container)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect1 = container;
            --rect1.Width;
            --rect1.Height;
            RectangleF rect2 = new RectangleF((float)rect1.X, (float)rect1.Y, (float)rect1.Width, (float)rect1.Height / 2f);
            if (rect1.Height > 0 && rect1.Width > 0)
            {
                path.AddArc(rect1, 180f, 142f);
                PointF[] points = new PointF[4]
                {
              path.GetLastPoint(),
              new PointF((float) container.Width * 0.7f, (float) container.Height * 0.33f),
              new PointF((float) container.Width * 0.25f, (float) container.Height * 0.5f),
              path.PathPoints[0]
                };
                path.AddCurve(points);
                path.CloseFigure();               
            }
            return path;
        }

        public static Brush GetGradBrush(Rectangle container, Color color)
        {
            Brush brush = (Brush)null;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(container);
                brush = (Brush)new PathGradientBrush(path)
                {
                    CenterColor = color,
                    SurroundColors = new Color[1]
                  {
                Color.Transparent
                  },
                    CenterPoint = new PointF((float)container.Left + (float)container.Width * 0.5f, (float)(container.Bottom + container.Height))
                };               
            }
            return brush;
        }

        public static GraphicsPath GetArcPath(
          RectangleF container,
          double dStartDegrees,
          double dArcLengthDegrees)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            int num1 = 100;
            double num2 = dArcLengthDegrees / (double)num1;
            List<PointF> pointFList = new List<PointF>();
            for (int index = 0; index <= num1; ++index)
            {
                double degrees = dStartDegrees - (double)index * num2;
                pointFList.Add(GraphicsHelper.GetPointInArc(container, degrees, 0.0));
            }
            graphicsPath.AddCurve(pointFList.ToArray());
            return graphicsPath;
        }

        public static PointF GetPointInArc(RectangleF rect, double degrees, double offset)
        {
            PointF pointF = new PointF(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
            double num = Math.PI / 180.0 * (degrees + 90.0);
            return new PointF(pointF.X + (float)((offset + (double)rect.Width / 2.0) * Math.Sin(num)), pointF.Y + (float)((offset + (double)rect.Height / 2.0) * Math.Cos(num)));
        }

        public static Color GetMixedColor(Color start, Color end, double percentage)
        {
            double num1 = (double)((int)end.A - (int)start.A);
            double num2 = (double)((int)end.R - (int)start.R);
            double num3 = (double)((int)end.G - (int)start.G);
            double num4 = (double)((int)end.B - (int)start.B);
            double num5 = num1 * percentage;
            double num6 = num2 * percentage;
            double num7 = num3 * percentage;
            double num8 = num4 * percentage;
            double num9 = (double)Math.Min(start.A, end.A);
            double num10 = (double)Math.Min(start.R, end.R);
            double num11 = (double)Math.Min(start.G, end.G);
            double num12 = (double)Math.Min(start.B, end.B);
            double num13 = (double)Math.Max(start.A, end.A);
            double num14 = (double)Math.Max(start.R, end.R);
            double num15 = (double)Math.Max(start.G, end.G);
            double num16 = (double)Math.Max(start.B, end.B);
            return Color.FromArgb((int)(num5 + (num5 > 0.0 ? num9 : num13)), (int)(num6 + (num6 > 0.0 ? num10 : num14)), (int)(num7 + (num7 > 0.0 ? num11 : num15)), (int)(num8 + (num8 > 0.0 ? num12 : num16)));
        }
    }
}
