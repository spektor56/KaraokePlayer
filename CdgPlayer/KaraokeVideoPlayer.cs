﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
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
        private bool processing = false;
        private readonly Timer _lyricTimer = new Timer();
        private GraphicsFile _cdgFile;
        private bool _fullscreen;
        private KaraokeVideoOverlay _overlayForm;
        private Stopwatch _stopWatch = new Stopwatch();
        private long _currentTime = 0;
        private long _lastRenderTime = -1;
        //private int iteration = 0;
        
        public bool FullScreen => _fullscreen;

        public event EventHandler SongFinished;
        public void OnSongFinished()
        {
            EventHandler handler = SongFinished;
            if (null != handler) handler(this, EventArgs.Empty);
        }

        public KaraokeVideoPlayer()
        {
            InitializeComponent();
            
            var panelDoubleClick = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            panelDoubleClick.MouseClick += panelDoubleClick_MouseClick;

            //vlcPlayer.Video.AspectRatio = "16:9";
            vlcPlayer.Controls.Add(panelDoubleClick);
            panelDoubleClick.BringToFront();

            _lyricTimer.Interval = 8;
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
            if (!processing)
            {
                processing = true;
                try
                {
                    var renderTime = _stopWatch.ElapsedMilliseconds + _currentTime;
                    if(renderTime < _lastRenderTime)
                    {
                        return;
                    }
                    var picture = _cdgFile.RenderAtTime(renderTime);
                    _lastRenderTime = renderTime;

                    if (picture == null)
                    {
                        return;
                    }
                    
                    const int scaleSize = 4;
                    var scaledImage = ImageFilters.Xbrz.ScaleImage(picture, scaleSize);
                    scaledImage.MakeTransparent(scaledImage.GetPixel(1, 1));

                    /*
                    if (iteration++ % 100 == 0)
                    {
                        picture.Save(@"C:\Test\" + Guid.NewGuid() + ".bmp", ImageFormat.Bmp);
                        //scaledImage.Save(@"C:\Test\" + scaleSize + Guid.NewGuid() + ".bmp", ImageFormat.Bmp);
                    }
                    */

                    if (!Disposing && !IsDisposed)
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            if (_overlayForm != null && !_overlayForm.IsDisposed)
                                _overlayForm.BackgroundImage = scaledImage;
                        }));
                    }
                }
                finally
                {
                    processing = false;
                }
            }
        }

        public async void Play(Uri file)
        {
            vlcPlayer.SetMedia(file);
            _cdgFile = await GraphicsFile.LoadAsync(Path.ChangeExtension(file.LocalPath, "cdg"));
            vlcPlayer.Play();
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
                Cursor.Hide();
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
                    parentForm.Focus();
                    Cursor.Show();
                }
            }
        }

        public void TogglePause()
        {
            if (vlcPlayer.IsPlaying)
            {
                vlcPlayer.Pause();
            }
            else
            {
                vlcPlayer.Play();
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

        private void vlcPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            _stopWatch.Start();
            _lyricTimer.Start();
        }

        private void vlcPlayer_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            //vlcPlayer.Audio.Volume = 50;
            _stopWatch.Restart();
            _currentTime = e.NewTime;
        }

        private void vlcPlayer_Paused(object sender, VlcMediaPlayerPausedEventArgs e)
        {
            _lyricTimer.Stop();
            _stopWatch.Stop();
        }

        private void vlcPlayer_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            _lyricTimer.Stop();
            _lastRenderTime = -1;
            _currentTime = 0;
            _stopWatch.Reset();
            OnSongFinished();
        }

        private void vlcPlayer_MediaChanged(object sender, VlcMediaPlayerMediaChangedEventArgs e)
        {
            _lyricTimer.Stop();
            _lastRenderTime = -1;
            _currentTime = 0;
            _stopWatch.Reset();
        }
    }
}