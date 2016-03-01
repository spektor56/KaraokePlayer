namespace KaraokePlayer
{
    partial class MainForm
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
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.karaokeVideoPlayer1 = new KaraokePlayer.KaraokeVideoPlayer();
            this.SuspendLayout();
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.Location = new System.Drawing.Point(385, 358);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Size = new System.Drawing.Size(81, 37);
            this.materialRaisedButton1.TabIndex = 1;
            this.materialRaisedButton1.Text = "Play";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            this.materialRaisedButton1.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // karaokeVideoPlayer1
            // 
            this.karaokeVideoPlayer1.Location = new System.Drawing.Point(100, 140);
            this.karaokeVideoPlayer1.Name = "karaokeVideoPlayer1";
            this.karaokeVideoPlayer1.Size = new System.Drawing.Size(249, 146);
            this.karaokeVideoPlayer1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 407);
            this.Controls.Add(this.materialRaisedButton1);
            this.Controls.Add(this.karaokeVideoPlayer1);
            this.Name = "MainForm";
            this.Text = "Karaoke Player";
            this.ResumeLayout(false);

        }

        #endregion

        private KaraokeVideoPlayer karaokeVideoPlayer1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton1;
    }
}