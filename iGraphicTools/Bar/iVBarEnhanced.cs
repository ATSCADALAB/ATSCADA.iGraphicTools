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
    public partial class iVBarEnhanced : UserControl
    {
        #region License Management

        private static readonly DateTime LicenseExpiry = new DateTime(2025, 7, 15);
        private static bool licenseChecked = false;
        private static bool isLicenseValid = false;

        private bool CheckLicense()
        {
            if (!licenseChecked)
            {
                DateTime currentDate = DateTime.Now.Date;
                isLicenseValid = currentDate <= LicenseExpiry;

                if (!isLicenseValid)
                {
                    ShowLicenseExpiredMessage();
                }

                licenseChecked = true;
            }

            return isLicenseValid;
        }

        private void ShowLicenseExpiredMessage()
        {
            string message = $"iVBarEnhanced License Expired!\n\n" +
                           $"License expired on: {LicenseExpiry.ToString("dd/MM/yyyy")}\n" +
                           $"Current date: {DateTime.Now.ToString("dd/MM/yyyy")}\n\n" +
                           $"Please contact your software vendor to renew the license.";

            MessageBox.Show(message, "License Expired - iVBarEnhanced",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        #region Fields
        private ITag tagControl;
        private ITag tagLow;
        private ITag tagHigh;
        private ITag tagMin;
        private ITag tagMax;

        private double minValue = 0;
        private double maxValue = 160;
        private double currentValue = 0;
        private double lowThreshold = -100000;
        private double highThreshold = -100000;
        private double gain = 1;

        private Color normalColor = Color.Blue;
        private Color lowColor = Color.LightBlue;
        private Color highColor = Color.Red;
        private Color scaleTextColor = Color.LightGray;
        private Color thresholdLineColor = Color.Orange;
        private bool showScale = true;
        private float scaleFontSize = 8f;
        private int scaleSteps = 8;

        #endregion

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
        [Description("Select tag for main value.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagName { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Select tag for low threshold.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagLow { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Select tag for high threshold.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagHigh { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Select tag for min value or enter value in quotes like \"0\".")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagMin { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Select tag for max value or enter value in quotes like \"100\".")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagMax { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Normal state color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color NormalColor
        {
            get => normalColor;
            set
            {
                normalColor = value;
                UpdateGraphic();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Low state color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color LowColor
        {
            get => lowColor;
            set
            {
                lowColor = value;
                UpdateGraphic();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("High state color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color HighColor
        {
            get => highColor;
            set
            {
                highColor = value;
                UpdateGraphic();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Background Color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color BackgroundColor
        {
            get => this.pnlBackground?.BackColor ?? Color.Gainsboro;
            set
            {
                if (this.pnlBackground != null)
                {
                    this.pnlBackground.BackColor = value;
                    Invalidate();
                }
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Text color for scale marks.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color ScaleTextColor
        {
            get => scaleTextColor;
            set
            {
                scaleTextColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Show scale markings on the side.")]
        public bool ShowScale
        {
            get => showScale;
            set
            {
                showScale = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Font size for scale numbers.")]
        public float ScaleFontSize
        {
            get => scaleFontSize;
            set
            {
                scaleFontSize = Math.Max(6f, Math.Min(16f, value));
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Number of scale divisions.")]
        public int ScaleSteps
        {
            get => scaleSteps;
            set
            {
                scaleSteps = Math.Max(4, Math.Min(20, value));
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Bar width in pixels (0 = auto-responsive).")]
        public int BarWidth { get; set; } = 0;

        [Category("ATSCADA Settings")]
        [Description("Left margin for bar position (0 = auto-center).")]
        public int LeftMargin { get; set; } = 0;

        [Category("ATSCADA Settings")]
        [Description("Inverse direction (fill from top).")]
        public bool InverseDirect { get; set; } = false;

        public iVBarEnhanced()
        {
            // Check license first
            if (!CheckLicense())
            {
                // If license expired, initialize with limited functionality
                InitializeComponent();
                SetLicenseExpiredState();
                return;
            }

            InitializeComponent();
            this.SizeChanged += IVBarEnhanced_SizeChanged;
            UpdateGain();
            UpdateLabels();
        }

        private void SetLicenseExpiredState()
        {
            // Disable functionality and show expired state
            this.Enabled = false;
            this.BackColor = Color.DarkRed;

            // Add expired label
            Label expiredLabel = new Label();
            expiredLabel.Text = "LICENSE\nEXPIRED";
            expiredLabel.ForeColor = Color.White;
            expiredLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            expiredLabel.TextAlign = ContentAlignment.MiddleCenter;
            expiredLabel.Dock = DockStyle.Fill;
            expiredLabel.BackColor = Color.Transparent;

            this.Controls.Clear();
            this.Controls.Add(expiredLabel);
        }

        private void IVBarEnhanced_SizeChanged(object sender, EventArgs e)
        {
            if (pnlContainer != null)
            {
                // Calculate bar width and position with scale space
                int scaleSpace = ShowScale ? 50 : 20; // Space for scale on the right
                int barWidth, leftMargin;

                // Responsive bar width 
                if (BarWidth > 0)
                {
                    barWidth = Math.Max(BarWidth, (int)(BarWidth * (this.Width / 120.0)));
                }
                else
                {
                    // Auto width with scale consideration
                    int availableWidth = this.Width - scaleSpace;
                    barWidth = Math.Max(40, Math.Min(availableWidth / 2, availableWidth - 20));
                }

                // Center the bar in available space (excluding scale area)
                if (LeftMargin > 0)
                {
                    leftMargin = LeftMargin;
                }
                else
                {
                    int availableWidth = this.Width - scaleSpace;
                    leftMargin = (availableWidth - barWidth) / 2;
                }

                // Full height usage
                int barHeight = this.Height - 20;

                // Constraints
                if (barHeight < 50) barHeight = 50;
                if (barWidth < 40) barWidth = 40;
                if (leftMargin < 5) leftMargin = 5;

                // Ensure bar doesn't overlap with scale
                int maxBarRight = this.Width - scaleSpace;
                if (leftMargin + barWidth > maxBarRight)
                {
                    if (barWidth > maxBarRight - 10)
                        barWidth = maxBarRight - 10;
                    leftMargin = maxBarRight - barWidth;
                }

                // Update container
                pnlContainer.Size = new Size(barWidth, barHeight);
                pnlContainer.Location = new Point(leftMargin, 10);

                UpdateGain();
                UpdateGraphic();
                Invalidate();
            }
        }

        private void Driver_ConstructionCompleted()
        {
            // Check license before allowing functionality
            if (!CheckLicense())
            {
                return; // Block functionality if license expired
            }

            // Main value tag
            this.tagControl = DriverExtensionMethod.GetTagByName(this.driver, TagName);
            if (this.tagControl != null)
            {
                this.tagControl.TagValueChanged += (sender, e) => UpdateValue(this.tagControl.Value);
                this.tagControl.TagStatusChanged += (sender, e) => UpdateValue(this.tagControl.Value);
                UpdateValue(this.tagControl.Value);
            }

            // Min value tag or direct value
            if (!string.IsNullOrEmpty(TagMin))
            {
                // Check if it's a direct value (enclosed in quotes)
                if (TagMin.StartsWith("\"") && TagMin.EndsWith("\""))
                {
                    string directValue = TagMin.Substring(1, TagMin.Length - 2);
                    if (double.TryParse(directValue, out double minVal))
                    {
                        this.SynchronizedInvokeAction(() =>
                        {
                            this.minValue = minVal;
                            UpdateLabels();
                            UpdateGain();
                            UpdateGraphic();
                        });
                    }
                }
                else
                {
                    // It's a tag name
                    this.tagMin = DriverExtensionMethod.GetTagByName(this.driver, TagMin);
                    if (this.tagMin != null)
                    {
                        this.tagMin.TagValueChanged += (sender, e) => UpdateMinValue(this.tagMin.Value);
                        this.tagMin.TagStatusChanged += (sender, e) => UpdateMinValue(this.tagMin.Value);
                        UpdateMinValue(this.tagMin.Value);
                    }
                }
            }

            // Max value tag or direct value
            if (!string.IsNullOrEmpty(TagMax))
            {
                // Check if it's a direct value (enclosed in quotes)
                if (TagMax.StartsWith("\"") && TagMax.EndsWith("\""))
                {
                    string directValue = TagMax.Substring(1, TagMax.Length - 2);
                    if (double.TryParse(directValue, out double maxVal))
                    {
                        this.SynchronizedInvokeAction(() =>
                        {
                            this.maxValue = maxVal;
                            UpdateLabels();
                            UpdateGain();
                            UpdateGraphic();
                        });
                    }
                }
                else
                {
                    // It's a tag name
                    this.tagMax = DriverExtensionMethod.GetTagByName(this.driver, TagMax);
                    if (this.tagMax != null)
                    {
                        this.tagMax.TagValueChanged += (sender, e) => UpdateMaxValue(this.tagMax.Value);
                        this.tagMax.TagStatusChanged += (sender, e) => UpdateMaxValue(this.tagMax.Value);
                        UpdateMaxValue(this.tagMax.Value);
                    }
                }
            }

            // Low threshold tag or direct value
            if (!string.IsNullOrEmpty(TagLow))
            {
                // Check if it's a direct value (enclosed in quotes)
                if (TagLow.StartsWith("\"") && TagLow.EndsWith("\""))
                {
                    string directValue = TagLow.Substring(1, TagLow.Length - 2);
                    if (double.TryParse(directValue, out double lowVal))
                    {
                        this.SynchronizedInvokeAction(() =>
                        {
                            this.lowThreshold = lowVal;
                            UpdateGraphic();
                        });
                    }
                }
                else
                {
                    // It's a tag name
                    this.tagLow = DriverExtensionMethod.GetTagByName(this.driver, TagLow);
                    if (this.tagLow != null)
                    {
                        this.tagLow.TagValueChanged += (sender, e) => UpdateLowThreshold(this.tagLow.Value);
                        this.tagLow.TagStatusChanged += (sender, e) => UpdateLowThreshold(this.tagLow.Value);
                        UpdateLowThreshold(this.tagLow.Value);
                    }
                }
            }

            // High threshold tag or direct value
            if (!string.IsNullOrEmpty(TagHigh))
            {
                // Check if it's a direct value (enclosed in quotes)
                if (TagHigh.StartsWith("\"") && TagHigh.EndsWith("\""))
                {
                    string directValue = TagHigh.Substring(1, TagHigh.Length - 2);
                    if (double.TryParse(directValue, out double highVal))
                    {
                        this.SynchronizedInvokeAction(() =>
                        {
                            this.highThreshold = highVal;
                            UpdateGraphic();
                        });
                    }
                }
                else
                {
                    // It's a tag name
                    this.tagHigh = DriverExtensionMethod.GetTagByName(this.driver, TagHigh);
                    if (this.tagHigh != null)
                    {
                        this.tagHigh.TagValueChanged += (sender, e) => UpdateHighThreshold(this.tagHigh.Value);
                        this.tagHigh.TagStatusChanged += (sender, e) => UpdateHighThreshold(this.tagHigh.Value);
                        UpdateHighThreshold(this.tagHigh.Value);
                    }
                }
            }
        }

        private void UpdateValue(string value)
        {
            // Check license before updating
            if (!CheckLicense()) return;

            if (double.TryParse(value, out double valueParse))
            {
                this.SynchronizedInvokeAction(() =>
                {
                    this.currentValue = valueParse;
                    UpdateGraphic();
                });
            }
        }

        private void UpdateLowThreshold(string value)
        {
            // Check license before updating
            if (!CheckLicense()) return;

            if (double.TryParse(value, out double valueParse))
            {
                this.SynchronizedInvokeAction(() =>
                {
                    this.lowThreshold = valueParse;
                    UpdateGraphic();
                });
            }
        }

        private void UpdateHighThreshold(string value)
        {
            // Check license before updating
            if (!CheckLicense()) return;

            if (double.TryParse(value, out double valueParse))
            {
                this.SynchronizedInvokeAction(() =>
                {
                    this.highThreshold = valueParse;
                    UpdateGraphic();
                });
            }
        }

        private void UpdateMinValue(string value)
        {
            // Check license before updating
            if (!CheckLicense()) return;

            if (double.TryParse(value, out double valueParse))
            {
                this.SynchronizedInvokeAction(() =>
                {
                    this.minValue = valueParse;
                    UpdateLabels();
                    UpdateGain();
                    UpdateGraphic();
                });
            }
        }

        private void UpdateMaxValue(string value)
        {
            // Check license before updating
            if (!CheckLicense()) return;

            if (double.TryParse(value, out double valueParse))
            {
                this.SynchronizedInvokeAction(() =>
                {
                    this.maxValue = valueParse;
                    UpdateLabels();
                    UpdateGain();
                    UpdateGraphic();
                });
            }
        }

        private void UpdateGraphic()
        {
            if (pnlFill == null || pnlBackground == null) return;

            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateGraphicInternal()));
                }
                else
                {
                    UpdateGraphicInternal();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating graphic: {ex.Message}");
            }
        }

        private void UpdateGraphicInternal()
        {
            double valueBar = this.currentValue;
            if (valueBar > this.maxValue) valueBar = this.maxValue;
            else if (valueBar < this.minValue) valueBar = this.minValue;

            int heightOfBar = Convert.ToInt32(this.gain * (valueBar - this.minValue));

            if (InverseDirect)
            {
                this.pnlBackground.Height = heightOfBar;
            }
            else
            {
                int backgroundHeight = this.pnlContainer.Height - heightOfBar > 0 ?
                    this.pnlContainer.Height - heightOfBar : 0;
                this.pnlBackground.Height = backgroundHeight;
            }

            // Determine color based on thresholds
            Color fillColor;
            if (this.currentValue <= this.lowThreshold)
                fillColor = LowColor;
            else if (this.currentValue >= this.highThreshold)
                fillColor = HighColor;
            else
                fillColor = NormalColor;

            this.pnlFill.BackColor = fillColor;

            // Force redraw
            this.Invalidate();
        }

        private void UpdateGain()
        {
            if (pnlContainer != null && pnlContainer.Height > 0)
            {
                this.gain = pnlContainer.Height / (this.maxValue - this.minValue);
            }
        }

        private void UpdateLabels()
        {
            // Method kept for internal use but no longer updates visible labels
            // Min/Max values are still maintained internally for calculations
        }

        private void UpdateCurrentValueLabel()
        {
            // Method removed - no longer needed as current value labels are removed
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Check license before painting
            if (!CheckLicense())
            {
                base.OnPaint(e);
                return; // Skip custom painting if license expired
            }

            base.OnPaint(e);

            // Draw scale marks and threshold lines
            if (pnlContainer != null && ShowScale)
            {
                DrawScale(e.Graphics);
            }

            if (pnlContainer != null)
            {
                DrawThresholdLines(e.Graphics);
            }
        }

        private void DrawScale(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen scalePen = new Pen(ScaleTextColor, 1))
            using (Font scaleFont = new Font("Arial", ScaleFontSize, FontStyle.Regular))
            using (SolidBrush textBrush = new SolidBrush(ScaleTextColor))
            {
                double stepValue = (maxValue - minValue) / ScaleSteps;
                double stepHeight = pnlContainer.Height / (double)ScaleSteps;

                for (int i = 0; i <= ScaleSteps; i++)
                {
                    int y = pnlContainer.Top + pnlContainer.Height - (int)(i * stepHeight);

                    // Draw tick mark on the right side of bar
                    int tickStartX = pnlContainer.Right + 2;
                    int tickEndX = pnlContainer.Right + 8;
                    g.DrawLine(scalePen, tickStartX, y, tickEndX, y);

                    // Draw scale value
                    double scaleValue = minValue + (i * stepValue);
                    string scaleText = scaleValue.ToString("F0");
                    SizeF textSize = g.MeasureString(scaleText, scaleFont);

                    g.DrawString(scaleText, scaleFont, textBrush,
                        pnlContainer.Right + 12, y - textSize.Height / 2);
                }
            }
        }

        private void DrawThresholdLines(Graphics g)
        {
            if (lowThreshold <= minValue && highThreshold >= maxValue) return;

            using (Pen thresholdPen = new Pen(LowColor, 2))
            {
                // Draw Low threshold line
                if (lowThreshold > minValue && lowThreshold < maxValue)
                {
                    double lowPercent = (lowThreshold - minValue) / (maxValue - minValue);
                    int lowY = pnlContainer.Bottom - (int)(lowPercent * pnlContainer.Height);

                    // Horizontal line across the bar
                    g.DrawLine(thresholdPen,
                        pnlContainer.Left - 5, lowY,
                        pnlContainer.Right + 5, lowY);

                    // Optional: Add "LOW" text
                    using (Font labelFont = new Font("Arial", 7, FontStyle.Bold))
                    using (SolidBrush labelBrush = new SolidBrush(LowColor))
                    {
                        g.DrawString("", labelFont, labelBrush,
                            pnlContainer.Left - 25, lowY - 7);
                    }
                }
                
            }
            using (Pen thresholdPen = new Pen(HighColor, 2))
            {
                
                if (highThreshold > minValue && highThreshold < maxValue)
                {
                    double highPercent = (highThreshold - minValue) / (maxValue - minValue);
                    int highY = pnlContainer.Bottom - (int)(highPercent * pnlContainer.Height);

                    // Horizontal line across the bar
                    g.DrawLine(thresholdPen,
                        pnlContainer.Left - 5, highY,
                        pnlContainer.Right + 5, highY);

                    // Optional: Add "HIGH" text
                    using (Font labelFont = new Font("Arial", 7, FontStyle.Bold))
                    using (SolidBrush labelBrush = new SolidBrush(HighColor))
                    {
                        g.DrawString("", labelFont, labelBrush,
                            pnlContainer.Left - 30, highY - 7);
                    }
                }
            }
        }
    }
}