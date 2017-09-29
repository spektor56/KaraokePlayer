using System;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace KaraokePlayer
{
    public partial class MainForm : MaterialForm
    {
        private readonly MaterialSkinManager _materialSkinManager;
        private List<FileInfo> _fileList;

        public MainForm()
        {
            InitializeComponent();
            // Initialize MaterialSkinManager
            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);
            //_materialSkinManager.Theme = new DarkTheme();
            _materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE);
        }

        private void materialRaisedButton1_Click(object sender, System.EventArgs e)
        {
            int index = this.materialListBox1.SelectedIndex;
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                karaokeVideoPlayer1.Play(new Uri(Path.ChangeExtension(((FileInfo)materialListBox1.Items[index]).FullName, ".mp3")));
                karaokeVideoPlayer1.ToggleFullScreen();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (browseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                var files = System.IO.Directory.GetFiles(browseDialog.SelectedPath, "*.cdg", searchOption: System.IO.SearchOption.AllDirectories);
                _fileList = files.Select(file => new FileInfo(file)).ToList();
                materialListBox1.DataSource = _fileList;
                materialListBox1.DisplayMember = "Name";

            }
        }

        private void materialListBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int index = this.materialListBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                karaokeVideoPlayer1.Play(new Uri(Path.ChangeExtension(((FileInfo)materialListBox1.Items[index]).FullName,".mp3")));
                karaokeVideoPlayer1.ToggleFullScreen();
            }
        }

        private void materialSingleLineTextField1_TextChanged(object sender, EventArgs e)
        {
            if (materialSingleLineTextField1.Text.Length >= 3)
            {
                materialListBox1.DataSource = _fileList.Where(
                    file => Regex.IsMatch(file.Name, materialSingleLineTextField1.Text, RegexOptions.IgnoreCase)).ToList();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var files = System.IO.Directory.GetFiles(browseDialog.SelectedPath, "*.cdg", searchOption: System.IO.SearchOption.AllDirectories);
            _fileList = files.Select(file => new FileInfo(file)).OrderBy(file => file.Name).ToList();
            materialListBox1.DataSource = _fileList;
            materialListBox1.DisplayMember = "Name";
        }

        private void karaokeVideoPlayer1_SongFinished(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, materialListBox1.Items.Count-1);
            BeginInvoke(new MethodInvoker(() => {
                karaokeVideoPlayer1.Play(new Uri(Path.ChangeExtension(((FileInfo)materialListBox1.Items[index]).FullName, ".mp3"))); }));
        }

        private void karaokeVideoPlayer1_Load(object sender, EventArgs e)
        {

        }
    }
}
