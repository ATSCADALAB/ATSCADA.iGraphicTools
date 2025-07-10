using ATSCADA.iGraphicTools.AnimateGauge.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ATSCADA.iGraphicTools.AnimateGauge.Drawable
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Description("The needle associated with this control")]
    [Category("Appearance")]
    public class Needle : IDisposable, IDrawable
    {
        private bool m_bDrawShadows = true;
        private float m_radiusPercent = 0.8f;
        private Color m_needleColor = Color.Yellow;
        private float m_baseSizePercent = 0.02f;
        private float m_tipWidthDegrees = 1f;
        private float m_tipExtensionPercent = 0.01f;
        private Color m_hubColor = Color.Yellow;
        private float m_hubSizePercent = 0.15f;
        private Color m_hubShadeColor = Color.DarkGreen;
        private const float DROP_SHADOW_X = 0.01f;
        private const float DROP_SHADOW_Y = 0.01f;
        private const float HUB_BEVEL_PERCENT = 0.8f;
        private double m_orientation;
        private bool m_bIsNeedleAboveHub;
        private GraphicsPath m_needlePath;
        private GraphicsPath m_hubPath;
        private GraphicsPath m_shadowPath;
        private Region m_redrawRegion;

        [Description("Raised when the appearance (color, etc) of the axis has changed")]
        public event EventHandler AppearanceChanged;

        [Description("Raised when the underlying graphics paths need to be recalculated")]
        public event EventHandler LayoutChanged;

        [Description("The orientation of the needle")]
        public double Orientation
        {
            get
            {
                return this.m_orientation;
            }
            set
            {
                this.m_orientation = value;
                this.OnLayoutChanged();
            }
        }

        [Description("Whether or not shadows are drawn for the needle and hub")]
        [DefaultValue(true)]
        public bool ShadowsVisible
        {
            get
            {
                return this.m_bDrawShadows;
            }
            set
            {
                this.m_bDrawShadows = value;
                this.OnAppearanceChanged();
            }
        }

        [DefaultValue(0.8f)]
        [Description("The percent of the whole control to make the needle length")]
        public float RadiusPercent
        {
            get
            {
                return this.m_radiusPercent;
            }
            set
            {
                if ((double)value < 0.0 || (double)value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.m_radiusPercent = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The color of the main needle")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color NeedleColor
        {
            get
            {
                return this.m_needleColor;
            }
            set
            {
                this.m_needleColor = value;
                this.OnAppearanceChanged();
            }
        }

        [Description("The percent of the whole control to use as the base width for the needle")]
        [DefaultValue(0.02f)]
        public float BaseSizePercent
        {
            get
            {
                return this.m_baseSizePercent;
            }
            set
            {
                if ((double)value < 0.0 || (double)value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.m_baseSizePercent = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The width (in degrees) of the tip of the needle")]
        [DefaultValue(1f)]
        public float TipWidthDegrees
        {
            get
            {
                return this.m_tipWidthDegrees;
            }
            set
            {
                this.m_tipWidthDegrees = value;
                this.OnLayoutChanged();
            }
        }

        [Description("0 makes tip more square, larger values make it more pointed")]
        [DefaultValue(0.01f)]
        public float TipExtensionPercent
        {
            get
            {
                return this.m_tipExtensionPercent;
            }
            set
            {
                if ((double)value < 0.0 || (double)value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.m_tipExtensionPercent = value;
                this.OnLayoutChanged();
            }
        }

        [DefaultValue(false)]
        [Description("Whether or not the needle is drawn on top of the hub")]
        public bool IsNeedleAboveHub
        {
            get
            {
                return this.m_bIsNeedleAboveHub;
            }
            set
            {
                this.m_bIsNeedleAboveHub = value;
                this.OnAppearanceChanged();
            }
        }

        [Description("The color of the center circle on which the needle rotates")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color HubColor
        {
            get
            {
                return this.m_hubColor;
            }
            set
            {
                this.m_hubColor = value;
                this.OnAppearanceChanged();
            }
        }

        [Description("The percent of the whole control to use as the hub size")]
        [DefaultValue(0.15f)]
        public float HubSizePercent
        {
            get
            {
                return this.m_hubSizePercent;
            }
            set
            {
                this.m_hubSizePercent = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The shade color around the hub of the needle")]
        [DefaultValue(typeof(Color), "DarkGreen")]
        public Color HubShadeColor
        {
            get
            {
                return this.m_hubShadeColor;
            }
            set
            {
                this.m_hubShadeColor = value;
                this.OnAppearanceChanged();
            }
        }

        public Needle()
        {
            this.NeedleColor = Color.Yellow;
            this.HubColor = Color.Yellow;
            this.HubShadeColor = Color.DarkGreen;
            this.HubSizePercent = 0.15f;
            this.Orientation = 90.0;
            this.RadiusPercent = 0.8f;
            this.BaseSizePercent = 0.02f;
            this.TipWidthDegrees = 1f;
            this.TipExtensionPercent = 0.01f;
            this.IsNeedleAboveHub = false;
            this.ShadowsVisible = true;
            this.m_redrawRegion = new Region();
            this.CalculatePaths(new RectangleF());
        }

        public Region GetRedrawRegion()
        {
            return this.m_redrawRegion;
        }

        public void Draw(Graphics g)
        {
            if (this.m_hubPath == null || this.m_needlePath == null || this.m_shadowPath == null)
                return;
            using (Brush brush1 = (Brush)new SolidBrush(Color.FromArgb(100, Color.Black)))
            {
                using (Brush brush2 = (Brush)new SolidBrush(this.NeedleColor))
                {
                    using (PathGradientBrush pathGradientBrush = new PathGradientBrush(this.m_hubPath))
                    {
                        using (Pen pen1 = new Pen(this.HubShadeColor, 1f))
                        {
                            using (Pen pen2 = new Pen(GraphicsHelper.GetMixedColor(this.NeedleColor, Color.Black, 0.75)))
                            {
                                pathGradientBrush.SurroundColors = new Color[1]
                                {
                  this.HubShadeColor
                                };
                                pathGradientBrush.CenterColor = this.HubColor;
                                pathGradientBrush.FocusScales = new PointF(0.8f, 0.8f);
                                if (this.m_bDrawShadows)
                                    g.FillPath(brush1, this.m_shadowPath);
                                if (this.IsNeedleAboveHub)
                                {
                                    g.FillPath((Brush)pathGradientBrush, this.m_hubPath);
                                    g.DrawPath(pen1, this.m_hubPath);
                                    g.FillPath(brush2, this.m_needlePath);
                                    g.DrawPath(pen2, this.m_needlePath);
                                }
                                else
                                {
                                    g.FillPath(brush2, this.m_needlePath);
                                    g.DrawPath(pen2, this.m_needlePath);
                                    g.FillPath((Brush)pathGradientBrush, this.m_hubPath);
                                    g.DrawPath(pen1, this.m_hubPath);
                                }
                            }
                        }
                    }
                }
            }
            this.m_redrawRegion.Dispose();
            this.m_redrawRegion = new Region();
            this.m_redrawRegion.Union(this.m_shadowPath);
            this.m_redrawRegion.Union(this.m_needlePath);
            this.m_redrawRegion.Union(this.m_hubPath);
        }

        public void CalculatePaths(RectangleF container)
        {
            this.DisposePaths();
            this.m_needlePath = new GraphicsPath();
            this.m_hubPath = new GraphicsPath();
            this.m_shadowPath = new GraphicsPath(FillMode.Winding);
            float num1 = (float)((double)container.Width * (1.0 - (double)this.HubSizePercent) / 2.0);
            float num2 = (float)((double)container.Height * (1.0 - (double)this.HubSizePercent) / 2.0);
            RectangleF rect1 = container;
            rect1.Inflate(-num1, -num2);
            float num3 = (float)((double)container.Width * (1.0 - (double)this.RadiusPercent) / 2.0);
            float num4 = (float)((double)container.Height * (1.0 - (double)this.RadiusPercent) / 2.0);
            RectangleF rect2 = container;
            rect2.Inflate(-num3, -num4);
            float num5 = (float)((double)container.Width * (1.0 - (double)this.BaseSizePercent) / 2.0);
            float num6 = (float)((double)container.Height * (1.0 - (double)this.BaseSizePercent) / 2.0);
            RectangleF rect3 = container;
            rect3.Inflate(-num5, -num6);
            float x = (float)((double)container.Width * (double)this.TipExtensionPercent / 2.0);
            float y = (float)((double)container.Height * (double)this.TipExtensionPercent / 2.0);
            RectangleF rect4 = rect2;
            rect4.Inflate(x, y);
            PointF pointInArc1 = GraphicsHelper.GetPointInArc(rect4, this.Orientation, 0.0);
            PointF pointInArc2 = GraphicsHelper.GetPointInArc(rect2, this.Orientation + (double)this.TipWidthDegrees / 2.0, 0.0);
            PointF pointInArc3 = GraphicsHelper.GetPointInArc(rect2, this.Orientation - (double)this.TipWidthDegrees / 2.0, 0.0);
            PointF pointInArc4 = GraphicsHelper.GetPointInArc(rect3, this.Orientation + 90.0, 0.0);
            this.m_needlePath.AddCurve(new PointF[3]
            {
        GraphicsHelper.GetPointInArc(rect3, this.Orientation - 90.0, 0.0),
        GraphicsHelper.GetPointInArc(rect3, this.Orientation + 180.0, 0.0),
        pointInArc4
            });
            this.m_needlePath.AddLine(pointInArc4, pointInArc2);
            this.m_needlePath.AddCurve(new PointF[3]
            {
        pointInArc2,
        pointInArc1,
        pointInArc3
            });
            this.m_needlePath.CloseFigure();
            this.m_hubPath.AddEllipse(rect1);
            rect1.Width += container.Width * 0.01f;
            rect1.Height += container.Height * 0.01f;
            this.m_shadowPath.AddEllipse(rect1);
            using (GraphicsPath addingPath = this.m_needlePath.Clone() as GraphicsPath)
            {
                using (Matrix matrix = new Matrix())
                {
                    matrix.Translate(container.Width * 0.01f, container.Height * 0.01f);
                    addingPath.Transform(matrix);
                    this.m_shadowPath.AddPath(addingPath, true);
                }
            }
            this.m_redrawRegion.Union(this.m_shadowPath);
            this.m_redrawRegion.Union(this.m_needlePath);
            this.m_redrawRegion.Union(this.m_hubPath);
        }

        protected virtual void OnLayoutChanged()
        {
            if (this.LayoutChanged == null)
                return;
            this.LayoutChanged((object)this, EventArgs.Empty);
        }

        protected virtual void OnAppearanceChanged()
        {
            if (this.AppearanceChanged == null)
                return;
            this.AppearanceChanged((object)this, EventArgs.Empty);
        }

        private void DisposePaths()
        {
            if (this.m_needlePath != null)
            {
                this.m_needlePath.Dispose();
                this.m_needlePath = (GraphicsPath)null;
            }
            if (this.m_hubPath != null)
            {
                this.m_hubPath.Dispose();
                this.m_hubPath = (GraphicsPath)null;
            }
            if (this.m_shadowPath == null)
                return;
            this.m_shadowPath.Dispose();
            this.m_shadowPath = (GraphicsPath)null;
        }

        public void Dispose()
        {
            this.DisposePaths();
            if (this.m_redrawRegion == null)
                return;
            this.m_redrawRegion.Dispose();
            this.m_redrawRegion = (Region)null;
        }

        public override string ToString()
        {
            return "Needle Settings";
        }
    }
}
