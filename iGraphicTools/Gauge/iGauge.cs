using ATSCADA.ToolExtensions.ExtensionMethods;
using ATSCADA.ToolExtensions.TagCollection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Gauge
{
    public partial class iGauge : Control
    {
        private ITag tagControl;

        private bool drag = false;        
        private int opacity = 100;     

        private Color bodyColor;
        private Color needleColor;
        private Color scaleColor;
        private bool viewGlass;
        private double currValue;
        private double minValue;
        private double maxValue;
        private int scaleDivisions;
        private int scaleSubDivisions;
        private GaugeRenderer renderer;
        private GaugeThresholdCollection listThreshold;
       
        protected PointF needleCenter;
        protected RectangleF drawRect;
        protected RectangleF glossyRect;
        protected RectangleF needleCoverRect;
        protected float startAngle;
        protected float endAngle;
        protected float drawRatio;
        protected GaugeRenderer defaultRenderer;

        private iDriver driver;

        [Category("ATSCADA Settings")]
        [Description("Select driver object.")]
        public iDriver Driver
        {
            get => driver;
            set
            {                
                if (driver != null) driver.ConstructionCompleted -= Driver_ConstructionCompleted;
                driver = value;
                if (driver != null) driver.ConstructionCompleted += Driver_ConstructionCompleted;
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Select tag for ATSCADA control.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagName { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Color of the background.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color BodyColor
        {
            get { return bodyColor; }
            set
            {
                bodyColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Color of needle.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color NeedleColor
        {
            get { return needleColor; }
            set
            {
                needleColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Allow a detailed view through the glass.")]       
        public bool ViewGlass
        {
            get { return viewGlass; }
            set
            {
                viewGlass = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The color displayed on the gauge.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color ScaleColor
        {
            get { return scaleColor; }
            set
            {
                scaleColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The value to which the pointer will point.")]        
        public double Value
        {
            get { return currValue; }
            set
            {
                double val = value;
                if (val > maxValue)
                    val = maxValue;

                if (val < minValue)
                    val = minValue;

                currValue = val;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The minimum value shown on the gauge.")]
        public double MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The maximum value shown on the gauge.")]
        public double MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The number of divisions on the gauge.")]
        public int ScaleDivisions
        {
            get { return scaleDivisions; }
            set
            {
                scaleDivisions = value;
                CalculateDimensions();
                Invalidate();

            }
        }

        [Category("ATSCADA Settings")]
        [Description("The number of subdivisions displayed on the scale for each division.")]
        public int ScaleSubDivisions
        {
            get { return scaleSubDivisions; }
            set
            {
                scaleSubDivisions = value;
                CalculateDimensions();
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Browsable(false)]
        public GaugeThresholdCollection Thresholds
        {
            get { return this.listThreshold; }
            set
            {
                this.listThreshold = value;
                Invalidate();
            }
        }
       
        [Browsable(false)]
        public GaugeRenderer Renderer
        {
            get { return this.renderer; }
            set
            {
                this.renderer = value;
                if (this.renderer != null)
                    renderer.AnalogMeter = this;
                Invalidate();
            }
        }

        public iGauge()
        {

            this.Size = new Size(200, 200);
            this.bodyColor = Color.LimeGreen;
            this.needleColor = Color.Yellow;
            this.scaleColor = Color.White;           
            this.viewGlass = false;
            this.startAngle = 135;
            this.endAngle = 405;
            this.minValue = 0;
            this.maxValue = 100;
            this.currValue = 50;
            this.scaleDivisions = 11;
            this.scaleSubDivisions = 10;
            this.renderer = null;

           
            this.listThreshold = new GaugeThresholdCollection();         

         
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            
            this.BackColor = Color.Transparent;

           
            this.defaultRenderer = new GaugeRenderer();
            this.defaultRenderer.AnalogMeter = this;

            this.CalculateDimensions();
        }

        private void Driver_ConstructionCompleted()
        {
            this.tagControl = DriverExtensionMethod.GetTagByName(this.driver, TagName);
            if (this.tagControl == null) return;

            this.tagControl.TagValueChanged += (sender, e) => Update(this.tagControl.Value);
            this.tagControl.TagStatusChanged += (sender, e) => Update(this.tagControl.Value);
            Update(this.tagControl.Value);
        }

        private void Update(string value)
        {
            if (double.TryParse(value, out double valueParse))
                this.SynchronizedInvokeAction(() => Value = valueParse);
        }

        public float GetDrawRatio()
        {
            return this.drawRatio;
        }

        public float GetStartAngle()
        {
            return this.startAngle;
        }

        public float GetEndAngle()
        {
            return this.endAngle;
        }

        public PointF GetNeedleCenter()
        {
            return this.needleCenter;
        }
        
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);            
            CalculateDimensions();

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF _rc = new RectangleF(0, 0, this.Width, this.Height);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (this.renderer == null)
            {
                this.defaultRenderer.DrawBackground(e.Graphics, _rc);
                this.defaultRenderer.DrawBody(e.Graphics, drawRect);
                this.defaultRenderer.DrawThresholds(e.Graphics, drawRect);
                this.defaultRenderer.DrawDivisions(e.Graphics, drawRect);
                this.defaultRenderer.DrawUM(e.Graphics, drawRect);
                this.defaultRenderer.DrawValue(e.Graphics, drawRect);
                this.defaultRenderer.DrawNeedle(e.Graphics, drawRect);
                this.defaultRenderer.DrawNeedleCover(e.Graphics, this.needleCoverRect);
                this.defaultRenderer.DrawGlass(e.Graphics, this.glossyRect);
                return;
            }

            this.renderer.DrawBackground(e.Graphics, _rc);
            this.renderer.DrawBody(e.Graphics, drawRect);
            this.renderer.DrawThresholds(e.Graphics, drawRect);
            this.renderer.DrawDivisions(e.Graphics, drawRect);
            this.renderer.DrawUM(e.Graphics, drawRect);
            this.renderer.DrawValue(e.Graphics, drawRect);
            this.renderer.DrawNeedle(e.Graphics, drawRect);
            this.renderer.DrawNeedleCover(e.Graphics, this.needleCoverRect);
            this.renderer.DrawGlass(e.Graphics, this.glossyRect);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                base.OnPaintBackground(e);
                Graphics g = e.Graphics;
                if (Parent != null && !drag)
                {
                    BackColor = Color.Transparent;
                    int index = Parent.Controls.GetChildIndex(this);

                    for (int i = Parent.Controls.Count - 1; i > index; i--)
                    {
                        System.Windows.Forms.Control c = Parent.Controls[i];
                        if (c.Bounds.IntersectsWith(Bounds) && c.Visible)
                        {
                            Bitmap bmp = new Bitmap(c.Width, c.Height, g);
                            c.DrawToBitmap(bmp, c.ClientRectangle);

                            g.TranslateTransform(c.Left - Left, c.Top - Top);
                            g.DrawImageUnscaled(bmp, Point.Empty);
                            g.TranslateTransform(Left - c.Left, Top - c.Top);
                            bmp.Dispose();
                        }
                    }
                }
                else
                {
                    g.Clear(Parent.BackColor);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(opacity * 255 / 100, Color.Transparent)),
                                                   this.ClientRectangle);
                }
            }
            catch { }
        }      

        protected virtual void CalculateDimensions()
        {
            // Rectangle
            float x, y, w, h;
            x = 0;
            y = 0;
            w = this.Size.Width;
            h = this.Size.Height;

            // Calculate ratio
            drawRatio = (Math.Min(w, h)) / 200;
            if (drawRatio == 0.0)
                drawRatio = 1;

            // Draw rectangle
            drawRect.X = x;
            drawRect.Y = y;
            drawRect.Width = w - 2;
            drawRect.Height = h - 2;

            if (w < h)
                drawRect.Height = w;
            else if (w > h)
                drawRect.Width = h;

            if (drawRect.Width < 10)
                drawRect.Width = 10;
            if (drawRect.Height < 10)
                drawRect.Height = 10;

            // Calculate needle center
            needleCenter.X = drawRect.X + (drawRect.Width / 2);
            needleCenter.Y = drawRect.Y + (drawRect.Height / 2);

            // Needle cover rect
            needleCoverRect.X = needleCenter.X - (20 * drawRatio);
            needleCoverRect.Y = needleCenter.Y - (20 * drawRatio);
            needleCoverRect.Width = 40 * drawRatio;
            needleCoverRect.Height = 40 * drawRatio;

            // Glass effect rect
            glossyRect.X = drawRect.X + (20 * drawRatio);
            glossyRect.Y = drawRect.Y + (10 * drawRatio);
            glossyRect.Width = drawRect.Width - (40 * drawRatio);
            glossyRect.Height = needleCenter.Y + (30 * drawRatio);
        }      

    }  
    
}
