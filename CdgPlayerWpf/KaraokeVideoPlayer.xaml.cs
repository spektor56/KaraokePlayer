using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CdgLib;
using xBRZNet;
using Path = System.IO.Path;
using Timer = System.Timers.Timer;

namespace CdgPlayerWpf
{
    /// <summary>
    /// Interaction logic for KaraokeVideoPlayer.xaml
    /// </summary>
    public partial class KaraokeVideoPlayer : UserControl
    {
        private bool processing = false;
        private readonly Timer _lyricTimer = new Timer();
        private GraphicsFile _cdgFile;
        private bool _fullscreen;
        private DateTime _startTime;
        private int iteration = 0;

        public event EventHandler SongFinished;
        public void OnSongFinished()
        {
            EventHandler handler = SongFinished;
            if (null != handler) handler(this, EventArgs.Empty);
        }


        public KaraokeVideoPlayer()
        {
            InitializeComponent();

            _lyricTimer.Interval = 33;
            _lyricTimer.Elapsed += LyricTimerOnElapsed;
        }

        private void LyricTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!processing)
            {
                processing = true;
                try
                {
                    var picture = _cdgFile.RenderAtTime((long)(DateTime
                                                                   .Now - _startTime).TotalMilliseconds);

                    const int scaleSize = 4;
                    var scaledImage = new xBRZScaler().ScaleImage(picture, scaleSize);
                    scaledImage.MakeTransparent(scaledImage.GetPixel(1, 1));
                    /*
                    if (iteration++ % 100 == 0)
                    {
                        scaledImage.Save(@"C:\Test\" + scaleSize + Guid.NewGuid() + ".bmp", ImageFormat.Bmp);
                    }
                    */
                    this.Dispatcher.Invoke(() =>
                    {
                        var access = lyrics.Dispatcher.CheckAccess();
                        BitmapImage image = new BitmapImage();
                        var ms = new MemoryStream();

                        scaledImage.Save(ms, ImageFormat.Bmp);

                        image.BeginInit();
                        ms.Seek(0, SeekOrigin.Begin);
                        image.StreamSource = ms;
                        image.EndInit();

                        lyrics.Source = image;

                    });

                }
                finally
                {
                    processing = false;
                }
            }
            
        }

        public async void Play(Uri file)
        {
            vlcPlayer.LoadMedia(file);
            _cdgFile = await GraphicsFile.LoadAsync(Path.ChangeExtension(file.LocalPath, "cdg"));
            vlcPlayer.Play();
        }
        /*
        private void vlcPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            _startTime = DateTime.Now;
            _lyricTimer.Start();
        }
        */
        /*
        private void vlcPlayer_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            _startTime = DateTime.Now.AddMilliseconds(-e.NewTime);
        }
        */
        /*
        private void KaraokeVideoPlayer_ParentChanged(object sender, EventArgs e)
        {
            if (FindForm() != null)
            {
                _overlayForm = new KaraokeVideoOverlay(this);
            }
        }
        */
        /*
        private void vlcPlayer_VlcLibDirectoryNeeded(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(@"lib\vlc\");
        }
        */
        /*
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
        */
        /*
        private void panelDoubleClick_MouseClick(object sender, MouseEventArgs e)
        {
            ToggleFullScreen();
        }
        */
        /*
        private void KaraokeVideoPlayer_Load(object sender, EventArgs e)
        {
            if (FindForm() != null)
            {
                _overlayForm = new KaraokeVideoOverlay(this);
            }
        }
        */
        /*
        private void vlcPlayer_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            OnSongFinished();
        }
        */
        private void vlcPlayer_VideoSourceChanged(object sender, Meta.Vlc.Wpf.VideoSourceChangedEventArgs e)
        {
            _startTime = DateTime.Now;
            _lyricTimer.Start();
        }

        private void vlcPlayer_TimeChanged_1(object sender, EventArgs e)
        {
            _startTime = DateTime.Now.AddMilliseconds(-vlcPlayer.Time.Milliseconds);
        }
    }
}
