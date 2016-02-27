using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraokePlayer
{
    public partial class CDGWindow : Form
    {

        private void CDGWindow_DoubleClick(object sender, System.EventArgs e)
        {
            AutoSizeWindow();
        }

        private void PictureBox1_DoubleClick(object sender, System.EventArgs e)
        {
            AutoSizeWindow();
        }

        private void AutoSizeWindow()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.TopMost = true;
                this.Refresh();
            }
            else {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.TopMost = false;
                this.Refresh();
            }
        }

        private void CDGWindow_SizeChanged(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.TopMost = true;
            }
            else {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.TopMost = false;
            }
        }
        public CDGWindow()
        {
            SizeChanged += CDGWindow_SizeChanged;
            DoubleClick += CDGWindow_DoubleClick;
            InitializeComponent();
        }

    }
}
