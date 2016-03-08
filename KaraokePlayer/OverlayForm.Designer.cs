namespace KaraokePlayer
{
    partial class OverlayForm
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
            this.Graphic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Graphic)).BeginInit();
            this.SuspendLayout();
            // 
            // Graphic
            // 
            this.Graphic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Graphic.Location = new System.Drawing.Point(0, 0);
            this.Graphic.Name = "Graphic";
            this.Graphic.Size = new System.Drawing.Size(284, 261);
            this.Graphic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Graphic.TabIndex = 0;
            this.Graphic.TabStop = false;
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Graphic);
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            ((System.ComponentModel.ISupportInitialize)(this.Graphic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox Graphic;
    }
}