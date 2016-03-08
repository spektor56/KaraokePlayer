namespace KaraokePlayer
{
    partial class KaraokeVideoPlayer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vlcPlayer = new Vlc.DotNet.Forms.VlcControl();
            this.panelVideo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).BeginInit();
            this.panelVideo.SuspendLayout();
            this.SuspendLayout();
            // 
            // vlcPlayer
            // 
            this.vlcPlayer.BackColor = System.Drawing.Color.Black;
            this.vlcPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcPlayer.Location = new System.Drawing.Point(0, 0);
            this.vlcPlayer.Name = "vlcPlayer";
            this.vlcPlayer.Size = new System.Drawing.Size(428, 330);
            this.vlcPlayer.Spu = -1;
            this.vlcPlayer.TabIndex = 0;
            this.vlcPlayer.Text = "vlcControl1";
            this.vlcPlayer.VlcLibDirectory = null;
            this.vlcPlayer.VlcMediaplayerOptions = new string[] {
        "--projectm-preset-path=lib\\presets",
        "--audio-visual=projectm",
        "--effect-list=scope",
        "--no-video",
        "--verbose=2"};
            this.vlcPlayer.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcPlayer_VlcLibDirectoryNeeded);
            this.vlcPlayer.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.vlcPlayer_Playing);
            this.vlcPlayer.TimeChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs>(this.vlcPlayer_TimeChanged);
            this.vlcPlayer.DoubleClick += new System.EventHandler(this.vlcPlayer_DoubleClick);
            // 
            // panelVideo
            // 
            this.panelVideo.Controls.Add(this.vlcPlayer);
            this.panelVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVideo.Location = new System.Drawing.Point(0, 0);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(428, 330);
            this.panelVideo.TabIndex = 2;
            // 
            // KaraokeVideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelVideo);
            this.Name = "KaraokeVideoPlayer";
            this.Size = new System.Drawing.Size(428, 330);
            this.DoubleClick += new System.EventHandler(this.KaraokeVideoPlayer_DoubleClick);
            this.ParentChanged += new System.EventHandler(this.KaraokeVideoPlayer_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).EndInit();
            this.panelVideo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcPlayer;
        private System.Windows.Forms.Panel panelVideo;
    }
}
