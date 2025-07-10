namespace ATSCADA.iGraphicTools.Image
{
    partial class frmPushSettings
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
            this.btnPress = new System.Windows.Forms.Button();
            this.txtPress = new System.Windows.Forms.TextBox();
            this.lblPress = new System.Windows.Forms.Label();
            this.btnNormal = new System.Windows.Forms.Button();
            this.txtNormal = new System.Windows.Forms.TextBox();
            this.lblNormal = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grbStatusImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbStatusImage
            // 
            this.grbStatusImage.Controls.Add(this.btnPress);
            this.grbStatusImage.Controls.Add(this.txtPress);
            this.grbStatusImage.Controls.Add(this.lblPress);
            this.grbStatusImage.Controls.Add(this.btnNormal);
            this.grbStatusImage.Controls.Add(this.txtNormal);
            this.grbStatusImage.Controls.Add(this.lblNormal);
            this.grbStatusImage.Location = new System.Drawing.Point(15, 9);
            this.grbStatusImage.Name = "grbStatusImage";
            this.grbStatusImage.Size = new System.Drawing.Size(342, 93);
            this.grbStatusImage.TabIndex = 6;
            this.grbStatusImage.TabStop = false;
            this.grbStatusImage.Text = "Status Image";
            // 
            // btnPress
            // 
            this.btnPress.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPress.Location = new System.Drawing.Point(297, 55);
            this.btnPress.Name = "btnPress";
            this.btnPress.Size = new System.Drawing.Size(35, 23);
            this.btnPress.TabIndex = 3;
            this.btnPress.Text = "...";
            this.btnPress.UseVisualStyleBackColor = true;
            // 
            // txtPress
            // 
            this.txtPress.Enabled = false;
            this.txtPress.Location = new System.Drawing.Point(69, 55);
            this.txtPress.Multiline = true;
            this.txtPress.Name = "txtPress";
            this.txtPress.Size = new System.Drawing.Size(222, 23);
            this.txtPress.TabIndex = 2;
            this.txtPress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPress
            // 
            this.lblPress.AutoSize = true;
            this.lblPress.Location = new System.Drawing.Point(7, 59);
            this.lblPress.Name = "lblPress";
            this.lblPress.Size = new System.Drawing.Size(38, 15);
            this.lblPress.TabIndex = 4;
            this.lblPress.Text = "Press";
            // 
            // btnNormal
            // 
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNormal.Location = new System.Drawing.Point(297, 24);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(35, 23);
            this.btnNormal.TabIndex = 1;
            this.btnNormal.Text = "...";
            this.btnNormal.UseVisualStyleBackColor = true;
            // 
            // txtNormal
            // 
            this.txtNormal.Enabled = false;
            this.txtNormal.Location = new System.Drawing.Point(69, 24);
            this.txtNormal.Multiline = true;
            this.txtNormal.Name = "txtNormal";
            this.txtNormal.Size = new System.Drawing.Size(222, 23);
            this.txtNormal.TabIndex = 0;
            this.txtNormal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNormal
            // 
            this.lblNormal.AutoSize = true;
            this.lblNormal.Location = new System.Drawing.Point(7, 28);
            this.lblNormal.Name = "lblNormal";
            this.lblNormal.Size = new System.Drawing.Size(48, 15);
            this.lblNormal.TabIndex = 1;
            this.lblNormal.Text = "Normal";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(268, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(173, 108);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 28);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // frmPushSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 151);
            this.Controls.Add(this.grbStatusImage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPushSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Push Settings";
            this.TopMost = true;
            this.grbStatusImage.ResumeLayout(false);
            this.grbStatusImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbStatusImage;
        private System.Windows.Forms.Button btnPress;
        private System.Windows.Forms.TextBox txtPress;
        private System.Windows.Forms.Label lblPress;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.TextBox txtNormal;
        private System.Windows.Forms.Label lblNormal;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}