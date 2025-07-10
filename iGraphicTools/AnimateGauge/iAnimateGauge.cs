using ATSCADA.iGraphicTools.AnimateGauge.Drawable;
using ATSCADA.iGraphicTools.AnimateGauge.Utils;
using ATSCADA.ToolExtensions.ExtensionMethods;
using ATSCADA.ToolExtensions.TagCollection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.AnimateGauge
{
    public class iAnimateGauge : Control
    {
        private ITag tagControl;

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

        //-----------------------
        private Color m_bgColor = Color.DarkGreen;
        private Color m_glossColor = Color.White;
        private Color m_gradColor = Color.White;
        private bool m_bShowGloss = true;
        private bool m_bShowGrad = true;
        private GraphicsPath m_bgPath;
        private GraphicsPath m_glossPath;
        private bool m_bHandleGloss;

        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return Color.Transparent;
            }
            set
            {
            }
        }

        private Color GradientColorTrans
        {
            get
            {
                return Color.FromArgb(204, this.GradientColor);
            }
        }

        private Color TopHalfColor
        {
            get
            {
                return Color.FromArgb(16, this.GlossColor);
            }
        }

        [Description("The background color of the control")]
        [DefaultValue(typeof(Color), "DarkGreen")]
        [Category("ATSCADA Settings")]
        public virtual Color BackgroundColor
        {
            get
            {
                return this.m_bgColor;
            }
            set
            {
                this.m_bgColor = value;
                this.Invalidate(true);
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The color of the control's gloss effect")]
        [DefaultValue(typeof(Color), "White")]
        public virtual Color GlossColor
        {
            get
            {
                return this.m_glossColor;
            }
            set
            {
                this.m_glossColor = value;
                this.Invalidate(true);
            }
        }

        [Category("ATSCADA Settings")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The color of the control's gradient effect")]
        public virtual Color GradientColor
        {
            get
            {
                return this.m_gradColor;
            }
            set
            {
                this.m_gradColor = value;
                this.Invalidate(true);
            }
        }

        [Description("Whether or not to show the control's gloss effect")]
        [DefaultValue(true)]
        [Category("ATSCADA Settings")]
        public virtual bool ShowGloss
        {
            get
            {
                return this.m_bShowGloss;
            }
            set
            {
                this.m_bShowGloss = value;
                this.Invalidate(true);
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Whether or not to show the control's gradient effect")]
        [DefaultValue(true)]
        public virtual bool ShowGradient
        {
            get
            {
                return this.m_bShowGrad;
            }
            set
            {
                this.m_bShowGrad = value;
                this.Invalidate(true);
            }
        }

        protected void Invalidate(IDrawable control)
        {
            this.Invalidate(control.GetRedrawRegion());
        }

        private void RecalculatePaths()
        {
            this.DisposePaths();
            this.m_bgPath = GraphicsHelper.GetGraphicsPath(this.ClientRectangle);
            this.m_glossPath = GraphicsHelper.Get3DShinePath(this.ClientRectangle);
        }

        protected virtual void OnPaintGloss(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (!this.ShowGloss)
                return;
            using (Brush brush = (Brush)new SolidBrush(this.TopHalfColor))
                g.FillPath(brush, this.m_glossPath);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            GraphicsState gstate = e.Graphics.Save();
            if (this.m_bgPath != null)
                e.Graphics.Clip.Exclude(this.m_bgPath);
            base.OnPaintBackground(e);
            e.Graphics.Restore(gstate);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Brush brush = (Brush)new SolidBrush(this.BackgroundColor))
            {
                using (Brush gradBrush = GraphicsHelper.GetGradBrush(this.ClientRectangle, this.GradientColorTrans))
                {
                    if (this.m_bgPath == null)
                        return;
                    e.Graphics.FillPath(brush, this.m_bgPath);
                    if (!this.ShowGradient)
                        return;
                    e.Graphics.FillPath(gradBrush, this.m_bgPath);
                }
            }
        }

        private void DisposePaths()
        {
            if (this.m_bgPath != null)
            {
                this.m_bgPath.Dispose();
                this.m_bgPath = (GraphicsPath)null;
            }
            if (this.m_glossPath == null)
                return;
            this.m_glossPath.Dispose();
            this.m_glossPath = (GraphicsPath)null;
        }

        //------------------------------------------


        private double m_value = 0f;
        private EaseFunction m_easeFunction;
        private DateTime m_animateStart;
        private readonly IContainer components = null;
        private System.Timers.Timer m_animationTimer;


        [Category("ATSCADA Settings")]
        [Description("The value of the gauge")]
        public double Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                var min = Axis.MinValue;
                var max = Axis.MaxValue;

                if (value < min)
                    this.m_value = min;
                else if (value > max)
                    this.m_value = max;
                else
                    this.m_value = value;

                this.HandleValueChanged();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The maximum value for the axis.")]
        public double MaxValue
        {
            get => Axis.MaxValue;
            set
            {
                if (Axis.MaxValue == value) return;
                if (Axis.MinValue >= value) return;

                Value = Axis.MinValue;
                Axis.MaxValue = value;
            }
        }

        [Category("ATSCADA Settings")]
        [Description("The minimum value for the axis")]
        public double MinValue
        {
            get => Axis.MinValue;
            set
            {
                if (Axis.MinValue == value) return;
                if (Axis.MaxValue <= value) return;

                Value = Axis.MinValue;
                Axis.MinValue = value;
            }
        }

        [DefaultValue(true)]
        [Description("Whether or not to animate value changes")]
        [Category("ATSCADA Settings")]
        public bool Animate { get; set; }

        [Category("ATSCADA Settings")]
        [DefaultValue(1000)]
        [Description("The total amount of time (ms) to get between two different values")]
        public int AnimationLength { get; set; }

        [Category("ATSCADA Settings")]
        [DefaultValue(typeof(EaseFunctionType), "Linear")]
        [Description("What type of interpolation to do when animating the needle")]
        public EaseFunctionType EaseFunction { get; set; }

        [DefaultValue(typeof(EaseMode), "InOut")]
        [Description("What mode of easing to use when animating the needle")]
        [Category("ATSCADA Settings")]
        public EaseMode EaseMode { get; set; }

        [Description("The amount of time (ms) between animation frames")]
        [DefaultValue(100)]
        [Category("ATSCADA Settings")]
        public double AnimationInterval
        {
            get
            {
                return this.m_animationTimer.Interval;
            }
            set
            {
                this.m_animationTimer.Interval = value;
            }
        }

        [Category("ATSCADA Settings")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Needle Needle { get; set; }

        [Category("ATSCADA Settings")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CircularAxis Axis { get; }


        public iAnimateGauge()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);


            this.ShowGloss = true;
            this.ShowGradient = true;
            this.m_bHandleGloss = true;
            this.RecalculatePaths();

            
            Axis = new CircularAxis();
            Needle = new Needle();
            Axis.AppearanceChanged += new EventHandler(this.OnItemAppearanceChanged);
            Axis.LayoutChanged += new EventHandler(this.OnItemLayoutChanged);
            Needle.AppearanceChanged += new EventHandler(this.OnItemAppearanceChanged);
            Needle.LayoutChanged += new EventHandler(this.OnItemLayoutChanged);
            Needle.CalculatePaths((RectangleF)this.ClientRectangle);
            Axis.CalculatePaths((RectangleF)this.ClientRectangle);

          
            this.Animate = true;
            this.EaseFunction = EaseFunctionType.Linear;
            this.AnimationLength = 1000;            
            this.EaseMode = EaseMode.InOut;

            this.m_animationTimer = new System.Timers.Timer();
            this.m_animationTimer.Elapsed += m_animationTimer_Tick;

            this.Size = new Size(200, 200);
            this.m_value = 50;
        }

        protected void HandleValueChanged()
        {
            this.StopAnimation();
            if (Axis == null || Needle == null)
                return;
            double dTo = (double)this.Axis.AxisStartDegrees - (this.m_value - (double)this.Axis.MinValue) / (double)(this.Axis.MaxValue - this.Axis.MinValue) * (double)this.Axis.AxisLengthDegrees;
            if (this.Animate && !this.DesignMode)
                this.StartAnimation(Needle.Orientation, dTo);
            else
                Needle.Orientation = dTo;
        }

        private void StartAnimation(double dFrom, double dTo)
        {
            this.m_easeFunction = new EaseFunction(this.EaseFunction, this.EaseMode, (double)this.AnimationLength, dFrom, dTo);
            this.m_animateStart = DateTime.Now;
            this.m_animationTimer.Start();
        }

        private void StopAnimation()
        {
            this.m_animationTimer.Stop();
        }

        private void m_animationTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now - this.m_animateStart;
            if (timeSpan.TotalMilliseconds >= (double)this.AnimationLength)
            {
                Needle.Orientation = this.m_easeFunction.ToValue;
                this.StopAnimation();
            }
            else
                Needle.Orientation = this.m_easeFunction.GetValue(timeSpan.TotalMilliseconds);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Axis != null)
                Axis.Draw(e.Graphics);
            if (Needle != null)
                Needle.Draw(e.Graphics);
            this.OnPaintGloss(e.Graphics);

            if (this.m_bHandleGloss)
                this.OnPaintGloss(e.Graphics);
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Axis != null)
                Axis.CalculatePaths((RectangleF)this.ClientRectangle);
            if (Needle != null)
                Needle.CalculatePaths((RectangleF)this.ClientRectangle);

            this.RecalculatePaths();
            base.OnResize(e);
        }

        private void OnItemAppearanceChanged(object sender, EventArgs e)
        {
            if (!(sender is IDrawable control))
                return;
            this.SynchronizedInvokeAction(() => this.Invalidate(control));

        }

        private void OnItemLayoutChanged(object sender, EventArgs e)
        {
            if (!(sender is IDrawable control))
                return;
            control.CalculatePaths((RectangleF)this.ClientRectangle);
            this.SynchronizedInvokeAction(() => this.Invalidate(control));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                    this.components.Dispose();
                if (Axis != null)
                    Axis.Dispose();
                if (Needle != null)
                    Needle.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
