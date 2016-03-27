using System;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using CdgLib;
using Vlc.DotNet.Core;

namespace CdgPlayer
{
    public partial class KaraokeVideoPlayer : UserControl
    {
        private GraphicsFile _cdgFile;
        private KaraokeVideoOverlay _overlayForm;
        private DateTime _startTime;
        private readonly System.Timers.Timer _lyricTimer = new System.Timers.Timer();
        private bool _fullscreen = false;

        public KaraokeVideoPlayer()
        {
            InitializeComponent();
            Panel panelDoubleClick = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            }; 
            panelDoubleClick.MouseClick += panelDoubleClick_MouseClick; ;
            vlcPlayer.Controls.Add(panelDoubleClick);
            panelDoubleClick.BringToFront();

            _lyricTimer.Interval = 33;
            _lyricTimer.Elapsed += LyricTimerOnElapsed;
        }

        private void LyricTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var picture = _cdgFile.RenderAtTime((long)(DateTime
                .Now - _startTime).TotalMilliseconds);
            BeginInvoke(new MethodInvoker(() => { _overlayForm.BackgroundImage = picture; }));
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
            if (FindForm() != null)
            {
                _overlayForm = new KaraokeVideoOverlay(this);
            }
        }

        private void vlcPlayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
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
            if (this.FindForm() != null)
            {
                _overlayForm = new KaraokeVideoOverlay(this);
            }
        }
    }
}