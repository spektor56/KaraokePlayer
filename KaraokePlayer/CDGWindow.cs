using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace KaraokePlayer
{
    public partial class CDGWindow : Form
    {
        public CDGWindow()
        {
            InitializeComponent();

        }


        private void CDGWindow_Load(object sender, EventArgs e)
        {
            

            var plexiGlass =new Plexiglass(this);
            plexiGlass.Controls.Add(pbLyrics);
            var file = new Uri(@"D:\Karaoke\SF001 - SF339 Sunfly Hits Karaoke Complete\SF339\SF339-01 - Kiesza - Hideaway.mp3");

            vlcPlayer.SetMedia(file);
            vlcPlayer.Play();

            
            
        }

        private void CDGWindow_DoubleClick_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            
        }
    }
}