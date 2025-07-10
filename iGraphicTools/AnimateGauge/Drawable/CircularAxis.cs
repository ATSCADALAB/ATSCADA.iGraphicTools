using ATSCADA.iGraphicTools.AnimateGauge.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ATSCADA.iGraphicTools.AnimateGauge.Drawable
{
    [Description("The axis associated with this control")]
    [Category("Appearance")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CircularAxis : IDisposable, IDrawable
    {
        private double m_maxVal = 100;
        private float m_radPercent = 0.85f;
        private Color m_axisColor = Color.White;
        private float m_axisWidth = 2f;
        private int m_axisStartDegrees = 245;
        private int m_axisLengthDegrees = 310;
        private float m_majorTickLength = 10f;
        private int m_majorTickDivision = 10;
        private float m_majorTickWidth = 3f;
        private float m_minorTickLength = 10f;
        private int m_minorTickDivision = 10;
        private float m_minorTickWidth = 3f;
        private float m_labelOffset = 25f;
        private Color m_labelColor = Color.White;
        private string m_labelFontFamily = "Arial";
        private float m_labelFontSize = 15f;
        private double m_minVal = 0;
        private TickPosition m_majorTickPosition;
        private TickPosition m_minorTickPosition;
        private LabelPosition m_labelPosition;
        private FontStyle m_labelFontStyle;
        private GraphicsPath m_arcPath;
        private GraphicsPath m_majorTicksPath;
        private GraphicsPath m_minorTicksPath;
        private GraphicsPath m_labelPath;
        private Region m_redrawRegion;

        [Description("Raised when the appearance (color, etc) of the axis has changed")]
        public event EventHandler AppearanceChanged;

        [Description("Raised when the underlying graphics paths need to be recalculated")]
        public event EventHandler LayoutChanged;

        [Description("The minimum value for the axis")]      
        public double MinValue
        {
            get
            {
                return this.m_minVal;
            }
            set
            {
                this.m_minVal = value;
                this.OnLayoutChanged();
            }
        }

       
        [Description("The maximum value for the axis")]
        public double MaxValue
        {
            get
            {
                return this.m_maxVal;
            }
            set
            {
                this.m_maxVal = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The percent of the whole control to use for the axis radius")]
      
        public float RadiusPercent
        {
            get
            {
                return this.m_radPercent;
            }
            set
            {
                if ((double)value < 0.0 || (double)value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.m_radPercent = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The color of the axis")]
        [DefaultValue(typeof(Color), "Black")]
        public Color AxisColor
        {
            get
            {
                return this.m_axisColor;
            }
            set
            {
                this.m_axisColor = value;
                this.OnAppearanceChanged();
            }
        }

        [Description("The width (weight) of the axis")]       
        public float AxisWidth
        {
            get
            {
                return this.m_axisWidth;
            }
            set
            {
                this.m_axisWidth = value;
                this.OnLayoutChanged();
            }
        }

      
        [Description("Where the axis starts in relation to the center")]
        public int AxisStartDegrees
        {
            get
            {
                return this.m_axisStartDegrees;
            }
            set
            {
                if (Math.Abs(value) > 360)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.m_axisStartDegrees = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The length of the axis in degrees")]      
        public int AxisLengthDegrees
        {
            get
            {
                return this.m_axisLengthDegrees;
            }
            set
            {
                if (Math.Abs(value) > 360)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.m_axisLengthDegrees = value;
                this.OnLayoutChanged();
            }
        }

       
        [Description("The length of major ticks")]
        public float MajorTickLength
        {
            get
            {
                return this.m_majorTickLength;
            }
            set
            {
                this.m_majorTickLength = value;
                this.OnLayoutChanged();
            }
        }

     
        [Description("The spacing between major ticks (major tick interval)")]
        public int MajorTickDivision
        {
            get
            {
                return this.m_majorTickDivision;
            }
            set
            {
                this.m_majorTickDivision = value;
                this.OnLayoutChanged();
            }
        }

       
        [Description("The width (weight) of major ticks")]
        public float MajorTickWidth
        {
            get
            {
                return this.m_majorTickWidth;
            }
            set
            {
                this.m_majorTickWidth = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The position of major ticks")]
        [DefaultValue(typeof(TickPosition), "Inside")]
        public TickPosition MajorTickPosition
        {
            get
            {
                return this.m_majorTickPosition;
            }
            set
            {
                this.m_majorTickPosition = value;
                this.OnLayoutChanged();
            }
        }

      
        [Description("The length of minor ticks")]
        public float MinorTickLength
        {
            get
            {
                return this.m_minorTickLength;
            }
            set
            {
                this.m_minorTickLength = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The spacing between minor ticks (minor tick interval)")]
       
        public int MinorTickDivision
        {
            get
            {
                return this.m_minorTickDivision;
            }
            set
            {
                this.m_minorTickDivision = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The width (weight) of minor ticks")]
        
        public float MinorTickWidth
        {
            get
            {
                return this.m_minorTickWidth;
            }
            set
            {
                this.m_minorTickWidth = value;
                this.OnLayoutChanged();
            }
        }

        [DefaultValue(typeof(TickPosition), "Inside")]
        [Description("The position of minor ticks")]
        public TickPosition MinorTickPosition
        {
            get
            {
                return this.m_minorTickPosition;
            }
            set
            {
                this.m_minorTickPosition = value;
                this.OnLayoutChanged();
            }
        }

      
        [Description("The distance from the axis to the center of each label")]
        public float LabelOffset
        {
            get
            {
                return this.m_labelOffset;
            }
            set
            {
                this.m_labelOffset = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The position of the labels")]
        [DefaultValue(typeof(LabelPosition), "Inside")]
        public LabelPosition LabelPosition
        {
            get
            {
                return this.m_labelPosition;
            }
            set
            {
                this.m_labelPosition = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The color of the labels")]
        
        public Color LabelColor
        {
            get
            {
                return this.m_labelColor;
            }
            set
            {
                this.m_labelColor = value;
                this.OnAppearanceChanged();
            }
        }

        [Description("The font family for the labels")]       
        public string LabelFontFamily
        {
            get
            {
                return this.m_labelFontFamily;
            }
            set
            {
                this.m_labelFontFamily = value;
                this.OnLayoutChanged();
            }
        }

        
        [Description("The font size for the labels")]
        public float LabelFontSize
        {
            get
            {
                return this.m_labelFontSize;
            }
            set
            {
                this.m_labelFontSize = value;
                this.OnLayoutChanged();
            }
        }

        [Description("The font style for the labels")]
        [DefaultValue(typeof(FontStyle), "Regular")]
        public FontStyle LabelFontStyle
        {
            get
            {
                return this.m_labelFontStyle;
            }
            set
            {
                this.m_labelFontStyle = value;
                this.OnLayoutChanged();
            }
        }

        public CircularAxis()
        {
           
            this.MinValue = 0;
            this.MaxValue = 100;
            this.RadiusPercent = 0.85f;
            this.AxisWidth = 2f;
            this.AxisColor = Color.White;
            this.AxisStartDegrees = 245;
            this.AxisLengthDegrees = 310;
            this.MajorTickLength = 10f;
            this.MajorTickDivision = 10;
            this.MajorTickWidth = 3f;
            this.MajorTickPosition = TickPosition.Inside;
            this.MinorTickLength = 6f;
            this.MinorTickDivision = 2;
            this.MinorTickWidth = this.AxisWidth * 0.5f;
            this.MinorTickPosition = TickPosition.Inside;
            this.LabelOffset = 25f;
            this.LabelPosition = LabelPosition.Inside;
            this.LabelColor = Color.White;
            this.LabelFontFamily = "Arial";
            this.LabelFontSize = 15f;
            this.LabelFontStyle = FontStyle.Regular;
            this.m_redrawRegion = new Region();
            this.CalculatePaths(new RectangleF());
        }

        public Region GetRedrawRegion()
        {
            return this.m_redrawRegion;
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

        public void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen1 = new Pen(this.AxisColor, this.AxisWidth))
            {
                using (Pen pen2 = new Pen(this.AxisColor, this.MajorTickWidth))
                {
                    using (Pen pen3 = new Pen(this.AxisColor, this.MinorTickWidth))
                    {
                        using (Brush brush = (Brush)new SolidBrush(this.LabelColor))
                        {
                            pen1.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
                            pen2.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
                            pen3.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
                            try
                            {
                                g.DrawPath(pen1, this.m_arcPath);
                                g.DrawPath(pen2, this.m_majorTicksPath);
                                g.DrawPath(pen3, this.m_minorTicksPath);
                                g.FillPath(brush, this.m_labelPath);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            this.m_redrawRegion.Dispose();
            this.m_redrawRegion = new Region();
            this.m_redrawRegion.Union(this.m_arcPath);
            this.m_redrawRegion.Union(this.m_labelPath);
            this.m_redrawRegion.Union(this.m_majorTicksPath);
            this.m_redrawRegion.Union(this.m_minorTicksPath);
        }

        public void CalculatePaths(RectangleF container)
        {
            this.DisposePaths();
            float num1 = (float)((double)container.Width * (1.0 - (double)this.RadiusPercent) / 2.0);
            float num2 = (float)((double)container.Height * (1.0 - (double)this.RadiusPercent) / 2.0);
            RectangleF rectangleF = container;
            rectangleF.Inflate(-num1, -num2);
            this.m_arcPath = GraphicsHelper.GetArcPath(rectangleF, (double)this.AxisStartDegrees, (double)this.AxisLengthDegrees);
            this.m_majorTicksPath = new GraphicsPath();
            this.m_minorTicksPath = new GraphicsPath();
            this.m_labelPath = new GraphicsPath();
            this.CalculateTicks(this.m_majorTicksPath, this.m_minorTicksPath, this.m_labelPath, rectangleF);
            this.m_redrawRegion.Union(this.m_arcPath);
            this.m_redrawRegion.Union(this.m_labelPath);
            this.m_redrawRegion.Union(this.m_majorTicksPath);
            this.m_redrawRegion.Union(this.m_minorTicksPath);
        }

        private void CalculateTicks(
          GraphicsPath majorPath,
          GraphicsPath minorPath,
          GraphicsPath labelPath,
          RectangleF rect)
        {
            double num = (double)this.AxisLengthDegrees / (double)(this.MaxValue - this.MinValue);
            double offset1 = 0.0;
            double offset2 = 0.0;
            double offset3 = 0.0;
            double offset4 = 0.0;
            switch (this.MajorTickPosition)
            {
                case TickPosition.Inside:
                    offset2 = -(double)this.MajorTickLength;
                    break;
                case TickPosition.Middle:
                    offset1 = (double)this.MajorTickLength / 2.0;
                    offset2 = -offset1;
                    break;
                case TickPosition.Outside:
                    offset1 = (double)this.MajorTickLength;
                    break;
            }
            switch (this.MinorTickPosition)
            {
                case TickPosition.Inside:
                    offset4 = -(double)this.MinorTickLength;
                    break;
                case TickPosition.Middle:
                    offset3 = (double)this.MinorTickLength / 2.0;
                    offset4 = -offset3;
                    break;
                case TickPosition.Outside:
                    offset3 = (double)this.MinorTickLength;
                    break;
            }
            for (double minValue = this.MinValue; minValue <= this.MaxValue; ++minValue)
            {
                double degrees = (double)this.AxisStartDegrees - (double)(minValue - this.MinValue) * num;
                if (minValue % this.MajorTickDivision == 0)
                {
                    if (Math.Abs(this.AxisLengthDegrees) < 360 || minValue < this.MaxValue || this.MinValue % this.MajorTickDivision != 0)
                        this.AddLabel(labelPath, rect, degrees, minValue);
                    PointF pointInArc1 = GraphicsHelper.GetPointInArc(rect, degrees, offset1);
                    PointF pointInArc2 = GraphicsHelper.GetPointInArc(rect, degrees, offset2);
                    majorPath.StartFigure();
                    majorPath.AddLine(pointInArc1, pointInArc2);
                    majorPath.CloseFigure();
                }
                else if (minValue % this.MinorTickDivision == 0)
                {
                    PointF pointInArc1 = GraphicsHelper.GetPointInArc(rect, degrees, offset3);
                    PointF pointInArc2 = GraphicsHelper.GetPointInArc(rect, degrees, offset4);
                    minorPath.StartFigure();
                    minorPath.AddLine(pointInArc1, pointInArc2);
                    minorPath.CloseFigure();
                }
            }
        }

        private void AddLabel(GraphicsPath labelPath, RectangleF rect, double degrees, double value)
        {
            double offset = 0.0;
            switch (this.LabelPosition)
            {
                case LabelPosition.Inside:
                    offset = -(double)this.LabelOffset;
                    break;
                case LabelPosition.Outside:
                    offset = (double)this.LabelOffset;
                    break;
            }
            PointF pointInArc = GraphicsHelper.GetPointInArc(rect, degrees, offset);
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap))
                {
                    using (Font font = new Font(this.LabelFontFamily, this.LabelFontSize, this.LabelFontStyle))
                    {
                        SizeF sizeF = SizeF.Empty;
                        try
                        {
                            sizeF = graphics.MeasureDisplayStringSize(value.ToString(), font);
                        }
                        catch
                        {
                        }
                        PointF origin = new PointF(pointInArc.X - sizeF.Width / 2f, pointInArc.Y - sizeF.Height / 2f);
                        labelPath.AddString(value.ToString(), font.FontFamily, (int)font.Style, font.Size, origin, StringFormat.GenericDefault);
                    }
                }
            }
        }

        private void DisposePaths()
        {
            if (this.m_arcPath != null)
            {
                this.m_arcPath.Dispose();
                this.m_arcPath = (GraphicsPath)null;
            }
            if (this.m_majorTicksPath != null)
            {
                this.m_majorTicksPath.Dispose();
                this.m_majorTicksPath = (GraphicsPath)null;
            }
            if (this.m_minorTicksPath != null)
            {
                this.m_minorTicksPath.Dispose();
                this.m_minorTicksPath = (GraphicsPath)null;
            }
            if (this.m_labelPath == null)
                return;
            this.m_labelPath.Dispose();
            this.m_labelPath = (GraphicsPath)null;
        }

        public void Dispose()
        {
            this.DisposePaths();
        }

        public override string ToString()
        {
            return "Axis Settings";
        }
    }
}
