using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace KaraokePlayer
{
    public partial class QueueForm : MaterialForm
    {
        private readonly MaterialSkinManager _materialSkinManager;

        public BindingList<FileInfo> Queue { get; }

        public QueueForm()
        {
            InitializeComponent();
            Queue = new BindingList<FileInfo>();
            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);
            //_materialSkinManager.Theme = new DarkTheme();
            _materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE);
            materialListBox1.DataSource = Queue;
            materialListBox1.DisplayMember = "Name";
        }

        private void materialListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mediaPlayer = ((MainForm) this.Owner).mediaPlayer;

            int index = materialListBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var file = Queue[index];
                Queue.Remove(file);

                mediaPlayer.Play(new Uri(Path.ChangeExtension(file.FullName, ".mp3")));
                mediaPlayer.ToggleFullScreen();
            }
        }

        private void materialListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int index = this.materialListBox1.SelectedIndex;
                if (index != ListBox.NoMatches)
                {
                    Queue.RemoveAt(index);
                }
            }
        }
    }
}
