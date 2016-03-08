using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using CdgLib;
using Vlc.DotNet.Core;

namespace KaraokePlayer
{
    public partial class KaraokeVideoPlayer : UserControl
    {
        private GraphicsFile _cdgFile;
        private OverlayForm _overlayForm;
        private DateTime _startTime;
        private readonly System.Timers.Timer _lyricTimer = new System.Timers.Timer();

        public KaraokeVideoPlayer()
        {
            InitializeComponent();
            _lyricTimer.Interval = 50;
            _lyricTimer.Elapsed += LyricTimerOnElapsed;
        }

        private void LyricTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var picture = _cdgFile.RenderAtTime((long)(DateTime.Now - _startTime).TotalMilliseconds);
            _overlayForm.Graphic.Image = picture;
        }

        public async void Play(Uri file)
        {
            vlcPlayer.SetMedia(file);
           _cdgFile = await  GraphicsFile.LoadAsync(Path.ChangeExtension(file.LocalPath, "cdg"));
            vlcPlayer.Play();
        }

        private void vlcPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            _startTime = DateTime.Now;
            _lyricTimer.Start();
        }

        private void vlcPlayer_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            _startTime = DateTime.Now.AddMilliseconds(-e.NewTime);
        }

        private void KaraokeVideoPlayer_ParentChanged(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                _overlayForm = new OverlayForm(this);
            }
        }

        private void vlcPlayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(@"lib\vlc\");
        }
    }
}