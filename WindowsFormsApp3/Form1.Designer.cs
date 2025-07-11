
namespace WindowsFormsApp3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.iDriver1 = new ATSCADA.iDriver();
            this.iVBarEnhanced1 = new ATSCADA.iGraphicTools.Bar.iVBarEnhanced();
            this.SuspendLayout();
            // 
            // iDriver1
            // 
            this.iDriver1.Designmode = false;
            this.iDriver1.GetTaskTimeOut = ((ulong)(5000ul));
            this.iDriver1.MaxTagWriteTimes = 10;
            this.iDriver1.ProjectFile = null;
            this.iDriver1.WaitingTime = 3000;
            // 
            // iVBarEnhanced1
            // 
            this.iVBarEnhanced1.BackColor = System.Drawing.Color.Transparent;
            this.iVBarEnhanced1.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.iVBarEnhanced1.BarWidth = 0;
            this.iVBarEnhanced1.Driver = this.iDriver1;
            this.iVBarEnhanced1.HighColor = System.Drawing.Color.Red;
            this.iVBarEnhanced1.InverseDirect = false;
            this.iVBarEnhanced1.LeftMargin = 0;
            this.iVBarEnhanced1.Location = new System.Drawing.Point(201, 162);
            this.iVBarEnhanced1.LowColor = System.Drawing.Color.LightBlue;
            this.iVBarEnhanced1.Name = "iVBarEnhanced1";
            this.iVBarEnhanced1.NormalColor = System.Drawing.Color.Blue;
            this.iVBarEnhanced1.ScaleFontSize = 8F;
            this.iVBarEnhanced1.ScaleSteps = 8;
            this.iVBarEnhanced1.ScaleTextColor = System.Drawing.Color.LightGray;
            this.iVBarEnhanced1.ShowScale = true;
            this.iVBarEnhanced1.Size = new System.Drawing.Size(243, 200);
            this.iVBarEnhanced1.TabIndex = 0;
            this.iVBarEnhanced1.TagHigh = "Location1.NhietCao";
            this.iVBarEnhanced1.TagLow = "Location1.NhietThap";
            this.iVBarEnhanced1.TagMax = "\"150\"";
            this.iVBarEnhanced1.TagMin = "\"30\"";
            this.iVBarEnhanced1.TagName = "Location1.NhietDo";
            this.iVBarEnhanced1.ThresholdLineColor = System.Drawing.Color.Orange;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.iVBarEnhanced1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private ATSCADA.iDriver iDriver1;
        private ATSCADA.iGraphicTools.Bar.iVBarEnhanced iVBarEnhanced1;
    }
}

