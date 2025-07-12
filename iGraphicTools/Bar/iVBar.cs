using ATSCADA.ToolExtensions.ExtensionMethods;
using ATSCADA.ToolExtensions.TagCollection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Bar
{
    // Custom Panel class for drawing threshold lines
    public class ThresholdPanel : Panel
    {
        public double LowThreshold { get; set; } = -100000;
        public double HighThreshold { get; set; } = -100000;
        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 100;
        public Color LowThresholdColor { get; set; } = Color.LightBlue;
        public Color HighThresholdColor { get; set; } = Color.Red;
        public bool ShowThresholdLines { get; set; } = true;

        // Custom background panel to draw on top
        public class ThresholdBackgroundPanel : Panel
        {
            public ThresholdPanel ParentThresholdPanel { get; set; }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                // Draw threshold lines on top of background
                if (ParentThresholdPanel != null && ParentThresholdPanel.ShowThresholdLines)
                {
                    ParentThresholdPanel.DrawThresholdLinesOnBackground(e.Graphics, this);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw threshold lines if no background panel covers them
            if (ShowThresholdLines)
            {
                DrawThresholdLines(e.Graphics);
            }
        }

        public void DrawThresholdLinesOnBackground(Graphics g, Control backgroundPanel)
        {
            // Convert coordinates relative to background panel
            var parentBounds = this.Bounds;
            var backgroundBounds = backgroundPanel.Bounds;

            // Draw Low threshold line
            if (LowThreshold > -100000 && LowThreshold >= MinValue && LowThreshold <= MaxValue)
            {
                using (Pen lowPen = new Pen(LowThresholdColor, 1))
                {
                    double lowPercent = (LowThreshold - MinValue) / (MaxValue - MinValue);
                    int lowY = parentBounds.Height - (int)(lowPercent * parentBounds.Height);

                    // Only draw if line intersects with background panel
                    if (lowY >= backgroundBounds.Top && lowY <= backgroundBounds.Bottom)
                    {
                        // Adjust Y coordinate relative to background panel
                        int relativeY = lowY - backgroundBounds.Top;

                        // Draw horizontal line across the background panel
                        g.DrawLine(lowPen, 0, relativeY, backgroundPanel.Width, relativeY);

                        // Extend beyond panel edges
                        g.DrawLine(lowPen, -5, relativeY, 0, relativeY);
                        g.DrawLine(lowPen, backgroundPanel.Width, relativeY, backgroundPanel.Width + 5, relativeY);

                        // Draw small vertical marks on sides
                        g.DrawLine(lowPen, -5, relativeY - 2, -5, relativeY + 2);
                        g.DrawLine(lowPen, backgroundPanel.Width + 5, relativeY - 2, backgroundPanel.Width + 5, relativeY + 2);
                    }
                }
            }

            // Draw High threshold line
            if (HighThreshold > -100000 && HighThreshold >= MinValue && HighThreshold <= MaxValue)
            {
                using (Pen highPen = new Pen(HighThresholdColor, 1))
                {
                    double highPercent = (HighThreshold - MinValue) / (MaxValue - MinValue);
                    int highY = parentBounds.Height - (int)(highPercent * parentBounds.Height);

                    // Only draw if line intersects with background panel
                    if (highY >= backgroundBounds.Top && highY <= backgroundBounds.Bottom)
                    {
                        // Adjust Y coordinate relative to background panel
                        int relativeY = highY - backgroundBounds.Top;

                        // Draw horizontal line across the background panel
                        g.DrawLine(highPen, 0, relativeY, backgroundPanel.Width, relativeY);

                        // Extend beyond panel edges
                        g.DrawLine(highPen, -5, relativeY, 0, relativeY);
                        g.DrawLine(highPen, backgroundPanel.Width, relativeY, backgroundPanel.Width + 5, relativeY);

                        // Draw small vertical marks on sides
                        g.DrawLine(highPen, -5, relativeY - 2, -5, relativeY + 2);
                        g.DrawLine(highPen, backgroundPanel.Width + 5, relativeY - 2, backgroundPanel.Width + 5, relativeY + 2);
                    }
                }
            }
        }

        private void DrawThresholdLines(Graphics g)
        {
            // Draw Low threshold line on main panel (visible part)
            if (LowThreshold > -100000 && LowThreshold >= MinValue && LowThreshold <= MaxValue)
            {
                using (Pen lowPen = new Pen(LowThresholdColor, 1))
                {
                    double lowPercent = (LowThreshold - MinValue) / (MaxValue - MinValue);
                    int lowY = this.Height - (int)(lowPercent * this.Height);

                    // Draw horizontal line across the panel
                    g.DrawLine(lowPen, 0, lowY, this.Width, lowY);

                    // Extend beyond panel edges
                    g.DrawLine(lowPen, -5, lowY, 0, lowY);
                    g.DrawLine(lowPen, this.Width, lowY, this.Width + 5, lowY);

                    // Draw small vertical marks on sides
                    g.DrawLine(lowPen, -5, lowY - 2, -5, lowY + 2);
                    g.DrawLine(lowPen, this.Width + 5, lowY - 2, this.Width + 5, lowY + 2);
                }
            }

            // Draw High threshold line on main panel (visible part)
            if (HighThreshold > -100000 && HighThreshold >= MinValue && HighThreshold <= MaxValue)
            {
                using (Pen highPen = new Pen(HighThresholdColor, 1))
                {
                    double highPercent = (HighThreshold - MinValue) / (MaxValue - MinValue);
                    int highY = this.Height - (int)(highPercent * this.Height);

                    // Draw horizontal line across the panel
                    g.DrawLine(highPen, 0, highY, this.Width, highY);

                    // Extend beyond panel edges
                    g.DrawLine(highPen, -5, highY, 0, highY);
                    g.DrawLine(highPen, this.Width, highY, this.Width + 5, highY);

                    // Draw small vertical marks on sides
                    g.DrawLine(highPen, -5, highY - 2, -5, highY + 2);
                    g.DrawLine(highPen, this.Width + 5, highY - 2, this.Width + 5, highY + 2);
                }
            }
        }

        public void UpdateThresholds()
        {
            this.Invalidate();
            // Also invalidate child controls
            foreach (Control child in this.Controls)
            {
                child.Invalidate();
            }
        }
    }

    [ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar))]
    public partial class iVBar : UserControl
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
            string message = $"iVBar License Expired!\n\n" +
                           $"License expired on: {LicenseExpiry.ToString("dd/MM/yyyy")}\n" +
                           $"Current date: {DateTime.Now.ToString("dd/MM/yyyy")}\n\n" +
                           $"Please contact your software vendor to renew the license.";

            MessageBox.Show(message, "License Expired - iVBar",
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
        private double maxValue = 100;
        private double currentValue = 0;
        private double lowThreshold = -100000;
        private double highThreshold = -100000;
        private double gain = 1;

        private Color normalColor = Color.Green;
        private Color lowColor = Color.LightBlue;
        private Color highColor = Color.Red;
        private Color lowThresholdLineColor = Color.Blue;
        private Color highThresholdLineColor = Color.Red;

        // Custom threshold panel to replace pnlFill
        private ThresholdPanel thresholdPanel;

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
        [Description("Fill color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color FillColor
        {
            get => thresholdPanel?.BackColor ?? pnlFill?.BackColor ?? Color.Green;
            set
            {
                if (thresholdPanel != null)
                    thresholdPanel.BackColor = value;
                if (pnlFill != null)
                    pnlFill.BackColor = value;
                Invalidate();
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Background Color.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color BackgroundColor
        {
            get => pnlBackground?.BackColor ?? Color.Gainsboro;
            set
            {
                if (pnlBackground != null)
                {
                    pnlBackground.BackColor = value;
                    Invalidate();
                }
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Inverse direction (fill from top).")]
        public bool InverseDirect { get; set; } = false;

        [Category("ATSCADA Settings")]
        [Description("Show threshold lines on bar.")]
        public bool ShowThresholdLines
        {
            get => thresholdPanel?.ShowThresholdLines ?? true;
            set
            {
                if (thresholdPanel != null)
                {
                    thresholdPanel.ShowThresholdLines = value;
                    thresholdPanel.Invalidate();
                }
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Color of low threshold line.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color LowThresholdLineColor
        {
            get => lowThresholdLineColor;
            set
            {
                lowThresholdLineColor = value;
                if (thresholdPanel != null)
                {
                    thresholdPanel.LowThresholdColor = value;
                    thresholdPanel.Invalidate();
                }
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Color of high threshold line.")]
        [TypeConverter(typeof(ColorConverter))]
        public Color HighThresholdLineColor
        {
            get => highThresholdLineColor;
            set
            {
                highThresholdLineColor = value;
                if (thresholdPanel != null)
                {
                    thresholdPanel.HighThresholdColor = value;
                    thresholdPanel.Invalidate();
                }
            }
        }

        public iVBar()
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

            // Replace pnlFill with custom ThresholdPanel
            InitializeThresholdPanel();

            this.SizeChanged += IVBar_SizeChanged;
            UpdateGain();
        }

        private void InitializeThresholdPanel()
        {
            if (pnlFill != null)
            {
                // Create new threshold panel with same properties as pnlFill
                thresholdPanel = new ThresholdPanel()
                {
                    BackColor = pnlFill.BackColor,
                    Dock = DockStyle.Fill,
                    MinValue = this.minValue,
                    MaxValue = this.maxValue,
                    LowThreshold = this.lowThreshold,
                    HighThreshold = this.highThreshold,
                    LowThresholdColor = this.lowThresholdLineColor,
                    HighThresholdColor = this.highThresholdLineColor,
                    ShowThresholdLines = true
                };

                // Replace background panel with custom one
                if (pnlBackground != null)
                {
                    var bgBounds = pnlBackground.Bounds;
                    var bgColor = pnlBackground.BackColor;
                    var bgDock = pnlBackground.Dock;

                    // Remove old background
                    pnlBackground.Parent.Controls.Remove(pnlBackground);

                    // Create new custom background panel
                    var customBackground = new ThresholdPanel.ThresholdBackgroundPanel()
                    {
                        BackColor = bgColor,
                        Dock = bgDock,
                        Bounds = bgBounds,
                        ParentThresholdPanel = thresholdPanel
                    };

                    // Add to threshold panel
                    thresholdPanel.Controls.Add(customBackground);

                    // Update reference
                    pnlBackground = customBackground;
                }

                // Replace pnlFill with thresholdPanel
                var parent = pnlFill.Parent;
                parent.Controls.Remove(pnlFill);
                parent.Controls.Add(thresholdPanel);

                // Update reference
                pnlFill = thresholdPanel;
            }
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

        private void IVBar_SizeChanged(object sender, EventArgs e)
        {
            var heightOfBackground = (thresholdPanel?.Height ?? pnlFill?.Height ?? 0) / 2;
            this.SynchronizedInvokeAction(() => {
                if (pnlBackground != null)
                    pnlBackground.Height = heightOfBackground;
            });
            UpdateGain();
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
                            UpdateThresholdPanelValues();
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
                            UpdateThresholdPanelValues();
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
                            UpdateThresholdPanelValues();
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
                            UpdateThresholdPanelValues();
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

        private void UpdateThresholdPanelValues()
        {
            if (thresholdPanel != null)
            {
                thresholdPanel.MinValue = this.minValue;
                thresholdPanel.MaxValue = this.maxValue;
                thresholdPanel.LowThreshold = this.lowThreshold;
                thresholdPanel.HighThreshold = this.highThreshold;
                thresholdPanel.LowThresholdColor = this.lowThresholdLineColor;
                thresholdPanel.HighThresholdColor = this.highThresholdLineColor;
                thresholdPanel.UpdateThresholds();
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
                    UpdateThresholdPanelValues();
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
                    UpdateThresholdPanelValues();
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
                    UpdateThresholdPanelValues();
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
                    UpdateThresholdPanelValues();
                    UpdateGain();
                    UpdateGraphic();
                });
            }
        }

        private void UpdateGraphic()
        {
            if (!GetValueBar(this.currentValue.ToString(), out double valueBar)) return;
            var heightOfBar = Convert.ToInt32(this.gain * (valueBar - this.minValue));

            this.SynchronizedInvokeAction(() =>
            {
                if (InverseDirect)
                    this.pnlBackground.Height = heightOfBar;
                else
                    this.pnlBackground.Height = (thresholdPanel?.Height ?? pnlFill?.Height ?? 0) - heightOfBar > 0 ?
                        (thresholdPanel?.Height ?? pnlFill?.Height ?? 0) - heightOfBar : 0;

                // Determine color based on thresholds
                Color fillColor;
                if (this.currentValue <= this.lowThreshold && this.lowThreshold > -100000)
                    fillColor = LowColor;
                else if (this.currentValue >= this.highThreshold && this.highThreshold > -100000)
                    fillColor = HighColor;
                else
                    fillColor = NormalColor;

                if (thresholdPanel != null)
                    thresholdPanel.BackColor = fillColor;
                else if (pnlFill != null)
                    pnlFill.BackColor = fillColor;
            });
        }

        private void UpdateGain()
        {
            var panelHeight = thresholdPanel?.Height ?? pnlFill?.Height ?? 0;
            if (panelHeight > 0)
            {
                this.gain = panelHeight / (this.maxValue - this.minValue);
            }
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