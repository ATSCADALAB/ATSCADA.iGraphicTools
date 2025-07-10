using ATSCADA.ToolExtensions.ExtensionMethods;
using ATSCADA.ToolExtensions.TagCollection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Led
{
    public class iLed7Segment : Control
    {
        private Led7SegmentBase[] segments = null;

        private string value = "0";

        private int elementWidth = 10;

        private float italicFactor = -0.1F;

        private Color colorBackground = Color.Black;

        private Color colorDark = Color.DimGray;

        private Color colorLight = Color.Lime;

        private bool showDot = true;

        private Padding elementPadding;

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
        [Description("Background color of the LED array.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color ColorBackground { get { return colorBackground; } set { colorBackground = value; UpdateSegments(); } }


        [Category("ATSCADA Settings")]
        [Description("Color of inactive LED segments.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color ColorDark { get { return colorDark; } set { colorDark = value; UpdateSegments(); } }


        [Category("ATSCADA Settings")]
        [Description("Color of active LED segments.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color ColorLight { get { return colorLight; } set { colorLight = value; UpdateSegments(); } }


        [Category("ATSCADA Settings")]
        [Description("Width of LED segments.")]
        public int ElementWidth { get { return elementWidth; } set { elementWidth = value; UpdateSegments(); } }


        [Category("ATSCADA Settings")]
        [Description("Shear coefficient for italicizing the displays.")]
        public float ItalicFactor { get { return italicFactor; } set { italicFactor = value; UpdateSegments(); } }


        [Category("ATSCADA Settings")]
        [Description("Specifies if the decimal point LED is displayed.")]
        public bool DecimalShow { get { return showDot; } set { showDot = value; UpdateSegments(); } }


        [Category("ATSCADA Settings")]
        [Description("Number of seven-segment elements in this array.")]
        public int DigitNumber { get { return segments.Length; } set { if ((value > 0) && (value <= 100)) RecreateSegments(value); } }


        [Category("ATSCADA Settings")]
        [Description("Padding that applies to each seven-segment element.")]
        public Padding ElementPadding { get { return elementPadding; } set { elementPadding = value; UpdateSegments(); } }        

        [Category("ATSCADA Settings")]
        [Description("The value to be displayed on the LED array.\nThis can contain numbers, certain letters, and decimal points.")]
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                for (int i = 0; i < segments.Length; i++) { segments[i].CustomPattern = 0; segments[i].DecimalOn = false; }
                if (this.value != null)
                {
                    int segmentIndex = 0;
                    for (int i = this.value.Length - 1; i >= 0; i--)
                    {
                        if (segmentIndex >= segments.Length) break;
                        if (this.value[i] == '.') segments[segmentIndex].DecimalOn = true;
                        else segments[segmentIndex++].Value = this.value[i].ToString();
                    }
                }
            }
        }

        public iLed7Segment()
        {           
            this.SuspendLayout();           
            this.Size = new System.Drawing.Size(300, 100);
            this.Resize += new System.EventHandler(this.SevenSegmentArray_Resize);
            this.ResumeLayout(false);

            this.TabStop = false;
            elementPadding = new Padding(10, 10, 10, 10);
            RecreateSegments(4);            
        }    
        
        private void Driver_ConstructionCompleted()
        {
            var tagControl = DriverExtensionMethod.GetTagByName(this.driver, TagName);
            if (tagControl == null) return;

            tagControl.TagValueChanged += (sender, e) => this.SynchronizedInvokeAction(() => Value = tagControl.Value);
            tagControl.TagStatusChanged += (sender, e) => this.SynchronizedInvokeAction(() => Value = tagControl.Value);
            this.SynchronizedInvokeAction(() => Value = tagControl.Value);
        }

        private void RecreateSegments(int count)
        {
            if (segments != null)
                for (int i = 0; i < segments.Length; i++) { segments[i].Parent = null; segments[i].Dispose(); }

            if (count <= 0) return;
            segments = new Led7SegmentBase[count];

            for (int i = 0; i < count; i++)
            {
                segments[i] = new Led7SegmentBase();
                segments[i].Parent = this;
                segments[i].Top = 0;
                segments[i].Height = this.Height;
                segments[i].Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
                segments[i].Visible = true;
            }

            ResizeSegments();
            UpdateSegments();
            this.Value = value;
        }
        
        private void ResizeSegments()
        {
            int segWidth = this.Width / segments.Length;
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i].Left = this.Width * (segments.Length - 1 - i) / segments.Length;
                segments[i].Width = segWidth;
            }
        }
     
        private void UpdateSegments()
        {
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i].ColorBackground = colorBackground;
                segments[i].ColorDark = colorDark;
                segments[i].ColorLight = colorLight;
                segments[i].ElementWidth = elementWidth;
                segments[i].ItalicFactor = italicFactor;
                segments[i].DecimalShow = showDot;
                segments[i].Padding = elementPadding;
            }
        }

        private void SevenSegmentArray_Resize(object sender, EventArgs e) { ResizeSegments(); }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(colorBackground);
        }      
    }
}
