using System;
using System.Drawing;
using System.IO;
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

        public KaraokeVideoPlayer()
        {
            InitializeComponent();
            _lyricTimer.Interval = 30;
            _lyricTimer.Elapsed += LyricTimerOnElapsed;
        }

        private async void LyricTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            System.Diagnostics.Debug.Print(DateTime.Now.Millisecond.ToString());
            if (vlcPlayer.IsPlaying)
            {
                await Task.Run(() =>
                {
                    _cdgFile.RenderAtPosition(
                        (long)(DateTime.Now - _startTime).TotalMilliseconds);
                });


                Invoke((MethodInvoker)(() =>
                {
                    _lyrics.Image = _cdgFile.RgbImage;
                    _lyrics.BackColor = ((Bitmap)_cdgFile.RgbImage).GetPixel(1, 1);
                }));
            }
            else
            {
                this.Enabled = false;
            }
        }

        public void Play(Uri file)
        {
            vlcPlayer.SetMedia(file);
            _cdgFile = new CdgFile(Path.ChangeExtension(file.LocalPath, "cdg"));
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
    }
}