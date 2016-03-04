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
        private readonly PictureBox _lyrics = new PictureBox {Dock = DockStyle.Fill};
        private CdgFile _cdgFile;
        private Image _lyricImage;
        private OverlayForm _overlayForm;
        private DateTime _startTime;
        private readonly System.Timers.Timer _lyricTimer = new System.Timers.Timer();
        private Stopwatch _stopwatch = new Stopwatch();

        public KaraokeVideoPlayer()
        {
            InitializeComponent();
            _lyricTimer.Interval = 500;
            _lyricTimer.Elapsed += LyricTimerOnElapsed;
        }

        private async void LyricTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            

        }

        public void Play(Uri file)
        {
            vlcPlayer.SetMedia(file);
            _cdgFile = new CdgFile(Path.ChangeExtension(file.LocalPath, "cdg"), FileMode.Open, FileAccess.Read);
            vlcPlayer.Play();
        }

        private async void vlcPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            _startTime = DateTime.Now;
            _lyricTimer.Start();

            while (vlcPlayer.IsPlaying)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var picture = await _cdgFile.Render((long) (DateTime.Now - _startTime).TotalMilliseconds);
                stopwatch.Reset();
                Debug.Print(stopwatch.ElapsedMilliseconds.ToString());


                    _lyrics.Image = picture;

          
            }
        }

        private void vlcPlayer_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            _startTime = DateTime.Now.AddMilliseconds(-e.NewTime);
        }

        private void vlcPlayer_Stopped(object sender, VlcMediaPlayerStoppedEventArgs e)
        {
            _overlayForm.Hide();
        }

        private void KaraokeVideoPlayer_ParentChanged(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                _overlayForm = new OverlayForm(this);
                _overlayForm.Controls.Add(_lyrics);
            }
        }

        private void vlcPlayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(@"lib\vlc\");
        }
    }
}