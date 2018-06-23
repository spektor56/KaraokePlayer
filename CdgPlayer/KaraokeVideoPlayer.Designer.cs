namespace CdgPlayer
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
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcPlayer
            // 
            this.vlcPlayer.BackColor = System.Drawing.Color.Black;
            this.vlcPlayer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
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
        "--projectm-width=896",
        "--projectm-height=504",
        "--audio-visual=projectm",
        "--effect-list=scope",
        "--no-video",
        "--verbose=2"};
            this.vlcPlayer.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcPlayer_VlcLibDirectoryNeeded);
            this.vlcPlayer.EndReached += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs>(this.vlcPlayer_EndReached);
            this.vlcPlayer.MediaChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs>(this.vlcPlayer_MediaChanged);
            this.vlcPlayer.Paused += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs>(this.vlcPlayer_Paused);
            this.vlcPlayer.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.vlcPlayer_Playing);
            this.vlcPlayer.TimeChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs>(this.vlcPlayer_TimeChanged);
            // 
            // KaraokeVideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.vlcPlayer);
            this.Name = "KaraokeVideoPlayer";
            this.Size = new System.Drawing.Size(428, 330);
            this.Load += new System.EventHandler(this.KaraokeVideoPlayer_Load);
            this.ParentChanged += new System.EventHandler(this.KaraokeVideoPlayer_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcPlayer;
    }
}
