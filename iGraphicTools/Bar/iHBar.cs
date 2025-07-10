using ATSCADA.ToolExtensions.ExtensionMethods;
using ATSCADA.ToolExtensions.TagCollection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Bar
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar))]
    public partial class iHBar : UserControl
    {
        private ITag tagControl;

        private double minValue = 0;

        private double maxValue = 100;

        private double gain = 1;

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
        [Description("Fill color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color FillColor
        {
            get => this.pnlFill.BackColor;
            set
            {
                this.pnlFill.BackColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Background Color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color BackgroundColor
        {
            get => this.pnlBackground.BackColor;
            set
            {
                this.pnlBackground.BackColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Min value.")]
        public double MinValue
        {
            get => this.minValue;
            set
            {
                if (this.maxValue <= value) return;
                this.minValue = value;
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Max value.")]
        public double MaxValue
        {
            get => this.maxValue;
            set
            {
                if (this.minValue >= value) return;
                this.maxValue = value;
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Max value.")]
        public bool InverseDirect { get; set; } = false;

        public iHBar()
        {
            InitializeComponent();

            this.SizeChanged += IVBar_SizeChanged;
        }

        private void IVBar_SizeChanged(object sender, EventArgs e)
        {
            var widthOfBackground = this.pnlFill.Width / 2;
            this.SynchronizedInvokeAction(() => this.pnlBackground.Width = widthOfBackground);
        }

        private void Driver_ConstructionCompleted()
        {
            this.tagControl = DriverExtensionMethod.GetTagByName(this.driver, TagName);
            if (this.tagControl == null) return;
            this.gain = (this.pnlFill.Width) / (this.maxValue - this.minValue);

            this.tagControl.TagValueChanged += (sender, e) => UpdateGraphic(this.tagControl.Value);
            this.tagControl.TagStatusChanged += (sender, e) => UpdateGraphic(this.tagControl.Value);
            UpdateGraphic(this.tagControl.Value);
        }

        private void UpdateGraphic(string value)
        {
            if (!GetValueBar(value, out double valueBar)) return;
            var widthOfBar = Convert.ToInt32(this.gain * (valueBar - this.minValue));

            this.SynchronizedInvokeAction(() =>
            {
                if (InverseDirect) this.pnlBackground.Width = widthOfBar;
                else
                    this.pnlBackground.Width = this.pnlFill.Width - widthOfBar > 0 ?
                    this.pnlFill.Width - widthOfBar :
                    0;
            });
        }


        private bool GetValueBar(string value, out double valueBar)
        {
            if (double.TryParse(value, out valueBar))
            {
                if (valueBar > this.maxValue) valueBar = this.maxValue;
                else if (valueBar < this.minValue) valueBar = this.minValue;

                return true;
            }

            return false;
        }      
    }
}
