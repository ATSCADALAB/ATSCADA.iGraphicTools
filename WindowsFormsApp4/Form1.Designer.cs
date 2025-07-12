namespace WindowsFormsApp4
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
            this.iVBar1 = new ATSCADA.iGraphicTools.Bar.iVBar();
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
            // iVBar1
            // 
            this.iVBar1.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.iVBar1.Driver = this.iDriver1;
            this.iVBar1.FillColor = System.Drawing.Color.Green;
            this.iVBar1.HighColor = System.Drawing.Color.Red;
            this.iVBar1.HighThresholdLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.iVBar1.InverseDirect = false;
            this.iVBar1.Location = new System.Drawing.Point(122, 28);
            this.iVBar1.LowColor = System.Drawing.Color.Yellow;
            this.iVBar1.LowThresholdLineColor = System.Drawing.Color.Blue;
            this.iVBar1.Name = "iVBar1";
            this.iVBar1.NormalColor = System.Drawing.Color.Green;
            this.iVBar1.ShowThresholdLines = true;
            this.iVBar1.Size = new System.Drawing.Size(224, 368);
            this.iVBar1.TabIndex = 0;
            this.iVBar1.TagHigh = "Location1.NhietCao";
            this.iVBar1.TagLow = "Location1.NhietThap";
            this.iVBar1.TagMax = "\"1000\"";
            this.iVBar1.TagMin = "\"0\"";
            this.iVBar1.TagName = "Location1.NhietDo";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.iVBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ATSCADA.iDriver iDriver1;
        private ATSCADA.iGraphicTools.Bar.iVBar iVBar1;
    }
}

