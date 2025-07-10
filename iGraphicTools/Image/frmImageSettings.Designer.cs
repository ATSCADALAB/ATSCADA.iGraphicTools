namespace ATSCADA.iGraphicTools.Image
{
    partial class frmImageSettings
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
            this.grbStatusImage = new System.Windows.Forms.GroupBox();
            this.btnPassive = new System.Windows.Forms.Button();
            this.txtPassive = new System.Windows.Forms.TextBox();
            this.lblPassive = new System.Windows.Forms.Label();
            this.btnActive = new System.Windows.Forms.Button();
            this.txtActive = new System.Windows.Forms.TextBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grbStatusImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbStatusImage
            // 
            this.grbStatusImage.Controls.Add(this.btnPassive);
            this.grbStatusImage.Controls.Add(this.txtPassive);
            this.grbStatusImage.Controls.Add(this.lblPassive);
            this.grbStatusImage.Controls.Add(this.btnActive);
            this.grbStatusImage.Controls.Add(this.txtActive);
            this.grbStatusImage.Controls.Add(this.lblActive);
            this.grbStatusImage.Location = new System.Drawing.Point(15, 9);
            this.grbStatusImage.Name = "grbStatusImage";
            this.grbStatusImage.Size = new System.Drawing.Size(342, 93);
            this.grbStatusImage.TabIndex = 1;
            this.grbStatusImage.TabStop = false;
            this.grbStatusImage.Text = "Status Image";
            // 
            // btnPassive
            // 
            this.btnPassive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPassive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassive.Location = new System.Drawing.Point(297, 58);
            this.btnPassive.Name = "btnPassive";
            this.btnPassive.Size = new System.Drawing.Size(35, 23);
            this.btnPassive.TabIndex = 3;
            this.btnPassive.Text = "...";
            this.btnPassive.UseVisualStyleBackColor = true;
            // 
            // txtPassive
            // 
            this.txtPassive.Enabled = false;
            this.txtPassive.Location = new System.Drawing.Point(69, 58);
            this.txtPassive.Multiline = true;
            this.txtPassive.Name = "txtPassive";
            this.txtPassive.Size = new System.Drawing.Size(222, 23);
            this.txtPassive.TabIndex = 2;
            this.txtPassive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPassive
            // 
            this.lblPassive.AutoSize = true;
            this.lblPassive.Location = new System.Drawing.Point(7, 62);
            this.lblPassive.Name = "lblPassive";
            this.lblPassive.Size = new System.Drawing.Size(49, 15);
            this.lblPassive.TabIndex = 4;
            this.lblPassive.Text = "Passive";
            // 
            // btnActive
            // 
            this.btnActive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActive.Location = new System.Drawing.Point(297, 27);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(35, 23);
            this.btnActive.TabIndex = 1;
            this.btnActive.Text = "...";
            this.btnActive.UseVisualStyleBackColor = true;
            // 
            // txtActive
            // 
            this.txtActive.Enabled = false;
            this.txtActive.Location = new System.Drawing.Point(69, 27);
            this.txtActive.Multiline = true;
            this.txtActive.Name = "txtActive";
            this.txtActive.Size = new System.Drawing.Size(222, 23);
            this.txtActive.TabIndex = 0;
            this.txtActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Location = new System.Drawing.Point(7, 31);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(38, 15);
            this.lblActive.TabIndex = 1;
            this.lblActive.Text = "Active";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(268, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(173, 108);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 28);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // frmImageSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 151);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grbStatusImage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImageSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Settings";
            this.TopMost = true;
            this.grbStatusImage.ResumeLayout(false);
            this.grbStatusImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbStatusImage;
        private System.Windows.Forms.Button btnActive;
        private System.Windows.Forms.TextBox txtActive;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnPassive;
        private System.Windows.Forms.TextBox txtPassive;
        private System.Windows.Forms.Label lblPassive;
    }
}