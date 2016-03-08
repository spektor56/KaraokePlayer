using System;
using MaterialSkin;
using MaterialSkin.Controls;

namespace KaraokePlayer
{
    public partial class MainForm : MaterialForm
    {
        private readonly MaterialSkinManager _materialSkinManager;
        public MainForm()
        {
            InitializeComponent();
            // Initialize MaterialSkinManager
            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);
            _materialSkinManager.Theme = new DarkTheme();
            _materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE);
        }

        private void materialRaisedButton1_Click(object sender, System.EventArgs e)
        {
            karaokeVideoPlayer1.Play(new Uri(@"D:\Karaoke\SF360 February 2016\SF360-01 - Charlie Puth - One Call Away\SF360-01 - Charlie Puth - One Call Away.mp3"));
        }


    }
}
