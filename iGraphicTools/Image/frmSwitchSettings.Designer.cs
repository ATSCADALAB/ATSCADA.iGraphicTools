namespace ATSCADA.iGraphicTools.Image
{
    partial class frmSwitchSettings
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
            this.grbPositionImage = new System.Windows.Forms.GroupBox();
            this.btnPosition2 = new System.Windows.Forms.Button();
            this.txtPosition2 = new System.Windows.Forms.TextBox();
            this.lblPosition2 = new System.Windows.Forms.Label();
            this.btnPosition1 = new System.Windows.Forms.Button();
            this.txtPosition1 = new System.Windows.Forms.TextBox();
            this.lblPositon1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grbPositionImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbPositionImage
            // 
            this.grbPositionImage.Controls.Add(this.btnPosition2);
            this.grbPositionImage.Controls.Add(this.txtPosition2);
            this.grbPositionImage.Controls.Add(this.lblPosition2);
            this.grbPositionImage.Controls.Add(this.btnPosition1);
            this.grbPositionImage.Controls.Add(this.txtPosition1);
            this.grbPositionImage.Controls.Add(this.lblPositon1);
            this.grbPositionImage.Location = new System.Drawing.Point(15, 9);
            this.grbPositionImage.Name = "grbPositionImage";
            this.grbPositionImage.Size = new System.Drawing.Size(352, 95);
            this.grbPositionImage.TabIndex = 8;
            this.grbPositionImage.TabStop = false;
            this.grbPositionImage.Text = "Position Image";
            // 
            // btnPosition2
            // 
            this.btnPosition2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPosition2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPosition2.Location = new System.Drawing.Point(304, 58);
            this.btnPosition2.Name = "btnPosition2";
            this.btnPosition2.Size = new System.Drawing.Size(35, 23);
            this.btnPosition2.TabIndex = 3;
            this.btnPosition2.Text = "...";
            this.btnPosition2.UseVisualStyleBackColor = true;
            // 
            // txtPosition2
            // 
            this.txtPosition2.Enabled = false;
            this.txtPosition2.Location = new System.Drawing.Point(76, 58);
            this.txtPosition2.Multiline = true;
            this.txtPosition2.Name = "txtPosition2";
            this.txtPosition2.Size = new System.Drawing.Size(222, 23);
            this.txtPosition2.TabIndex = 2;
            this.txtPosition2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPosition2
            // 
            this.lblPosition2.AutoSize = true;
            this.lblPosition2.Location = new System.Drawing.Point(7, 62);
            this.lblPosition2.Name = "lblPosition2";
            this.lblPosition2.Size = new System.Drawing.Size(61, 15);
            this.lblPosition2.TabIndex = 4;
            this.lblPosition2.Text = "Position 2";
            // 
            // btnPosition1
            // 
            this.btnPosition1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPosition1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPosition1.Location = new System.Drawing.Point(304, 27);
            this.btnPosition1.Name = "btnPosition1";
            this.btnPosition1.Size = new System.Drawing.Size(35, 23);
            this.btnPosition1.TabIndex = 1;
            this.btnPosition1.Text = "...";
            this.btnPosition1.UseVisualStyleBackColor = true;
            // 
            // txtPosition1
            // 
            this.txtPosition1.Enabled = false;
            this.txtPosition1.Location = new System.Drawing.Point(76, 27);
            this.txtPosition1.Multiline = true;
            this.txtPosition1.Name = "txtPosition1";
            this.txtPosition1.Size = new System.Drawing.Size(222, 23);
            this.txtPosition1.TabIndex = 0;
            this.txtPosition1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPositon1
            // 
            this.lblPositon1.AutoSize = true;
            this.lblPositon1.Location = new System.Drawing.Point(7, 31);
            this.lblPositon1.Name = "lblPositon1";
            this.lblPositon1.Size = new System.Drawing.Size(61, 15);
            this.lblPositon1.TabIndex = 1;
            this.lblPositon1.Text = "Position 1";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(278, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(183, 110);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 28);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // frmSwitchSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 152);
            this.Controls.Add(this.grbPositionImage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSwitchSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Switch Settings";
            this.TopMost = true;
            this.grbPositionImage.ResumeLayout(false);
            this.grbPositionImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbPositionImage;
        private System.Windows.Forms.Button btnPosition2;
        private System.Windows.Forms.TextBox txtPosition2;
        private System.Windows.Forms.Label lblPosition2;
        private System.Windows.Forms.Button btnPosition1;
        private System.Windows.Forms.TextBox txtPosition1;
        private System.Windows.Forms.Label lblPositon1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}