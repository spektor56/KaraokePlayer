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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            karaokeVideoPlayer1.Play(new Uri(@"D:\Karaoke\SF360 February 2016\SF360-01 - Charlie Puth - One Call Away\SF360-01 - Charlie Puth - One Call Away.mp3"));
        }
    }
}
