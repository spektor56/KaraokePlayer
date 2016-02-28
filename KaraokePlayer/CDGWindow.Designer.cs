namespace KaraokePlayer
{
    partial class CDGWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDGWindow));
            this.pbLyrics = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vlcPlayer = new Vlc.DotNet.Forms.VlcControl();
            ((System.ComponentModel.ISupportInitialize)(this.pbLyrics)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLyrics
            // 
            this.pbLyrics.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbLyrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLyrics.Image = global::KaraokePlayer.Properties.Resources.Google;
            this.pbLyrics.Location = new System.Drawing.Point(0, 0);
            this.pbLyrics.Name = "pbLyrics";
            this.pbLyrics.Size = new System.Drawing.Size(553, 414);
            this.pbLyrics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLyrics.TabIndex = 0;
            this.pbLyrics.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.vlcPlayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 414);
            this.panel1.TabIndex = 2;
            // 
            // vlcPlayer
            // 
            this.vlcPlayer.BackColor = System.Drawing.Color.Black;
            this.vlcPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcPlayer.Location = new System.Drawing.Point(0, 0);
            this.vlcPlayer.Name = "vlcPlayer";
            this.vlcPlayer.Size = new System.Drawing.Size(553, 414);
            this.vlcPlayer.Spu = -1;
            this.vlcPlayer.TabIndex = 2;
            this.vlcPlayer.Text = "vlcControl1";
            this.vlcPlayer.VlcLibDirectory = ((System.IO.DirectoryInfo)(resources.GetObject("vlcPlayer.VlcLibDirectory")));
            this.vlcPlayer.VlcMediaplayerOptions = new string[] {
        "--audio-visual=visual",
        "--effect-list=scope",
        "--no-video"};
            // 
            // CDGWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(553, 414);
            this.Controls.Add(this.pbLyrics);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "CDGWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Karaoke";
            this.Load += new System.EventHandler(this.CDGWindow_Load);
            this.DoubleClick += new System.EventHandler(this.CDGWindow_DoubleClick_1);
            ((System.ComponentModel.ISupportInitialize)(this.pbLyrics)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).EndInit();
            this.ResumeLayout(false);

        }
        public System.Windows.Forms.PictureBox pbLyrics;


        #endregion
        private System.Windows.Forms.Panel panel1;
        private Vlc.DotNet.Forms.VlcControl vlcPlayer;
    }
}