using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using CdgLib;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;
using Timer = System.Timers.Timer;

namespace CdgPlayer
{
    public partial class KaraokeVideoPlayer : UserControl
    {
        private readonly Timer _lyricTimer = new Timer();
        private GraphicsFile _cdgFile;
        private bool _fullscreen;
        private KaraokeVideoOverlay _overlayForm;
        private DateTime _startTime;

        public KaraokeVideoPlayer()
        {
            InitializeComponent();
            var panelDoubleClick = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            panelDoubleClick.MouseClick += panelDoubleClick_MouseClick;
            ;
            vlcPlayer.Controls.Add(panelDoubleClick);
            panelDoubleClick.BringToFront();

            _lyricTimer.Interval = 33;
            _lyricTimer.Elapsed += LyricTimerOnElapsed;
        }

        private Bitmap ResizeBitmap(Bitmap bitmap, int width, int height)
        {
            var aspectRatio = (double) bitmap.Width/bitmap.Height;
            var resizedBitmap = (int) (height*aspectRatio) <= width
                ? new Bitmap((int) (height*aspectRatio), height)
                : new Bitmap(width, (int) (width/aspectRatio));

            using (var graphic = Graphics.FromImage(resizedBitmap))
            {
                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                graphic.InterpolationMode = InterpolationMode.Low;
                graphic.DrawImage(bitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            }
            return resizedBitmap;
        }

        private void LyricTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Bitmap resizedImage;
            using (var picture = _cdgFile.RenderAtTime((long) (DateTime
                .Now - _startTime).TotalMilliseconds))
            {
                resizedImage = ResizeBitmap(picture, _overlayForm.Width, _overlayForm.Height);
                BeginInvoke(new MethodInvoker(() => { _overlayForm.BackgroundImage = resizedImage; }));
            }
        }

        public async void Play(Uri file)
        {
            vlcPlayer.SetMedia(file);
            _cdgFile = await GraphicsFile.LoadAsync(Path.ChangeExtension(file.LocalPath, "cdg"));
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
            if (FindForm() != null)
            {
                _overlayForm = new KaraokeVideoOverlay(this);
            }
        }

        private void vlcPlayer_VlcLibDirectoryNeeded(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(@"lib\vlc\");
        }


        public void ToggleFullScreen()
        {
            if (!_fullscreen)
            {
                var fullScreenForm = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    WindowState = FormWindowState.Maximized,
                    ShowInTaskbar = false
                };
                vlcPlayer.Dock = DockStyle.Fill;
                fullScreenForm.Controls.Add(vlcPlayer);

                fullScreenForm.Show(this);
                _overlayForm.WindowState = FormWindowState.Maximized;

                _fullscreen = true;
            }
            else
            {
                var parentForm = vlcPlayer.FindForm();
                if (parentForm != null)
                {
                    Controls.Add(vlcPlayer);
                    parentForm.Close();
                    _overlayForm.WindowState = FormWindowState.Normal;
                    _fullscreen = false;
                }
            }
        }

        private void panelDoubleClick_MouseClick(object sender, MouseEventArgs e)
        {
            ToggleFullScreen();
        }

        private void KaraokeVideoPlayer_Load(object sender, EventArgs e)
        {
            if (FindForm() != null)
            {
                _overlayForm = new KaraokeVideoOverlay(this);
            }
        }
    }
}