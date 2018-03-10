using System;
using System.Collections;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;


namespace KaraokePlayer
{
    public partial class MainForm : MaterialForm
    {
        private readonly MaterialSkinManager _materialSkinManager;
        private List<FileInfo> _fileList;
        private DateTime _lastKey = DateTime.Now;
        private BindingList<FileInfo> _queue = new BindingList<FileInfo>();

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
            if (_queue.Count > 0)
            {
                var file = _queue[0];
                _queue.Remove(file);

                mediaPlayer.Play(new Uri(Path.ChangeExtension(file.FullName, ".mp3")));
                mediaPlayer.ToggleFullScreen();
                return;
            }

            int index = this.mlbSongList.SelectedIndex;
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                mediaPlayer.Play(new Uri(Path.ChangeExtension(((FileInfo)mlbSongList.Items[index]).FullName, ".mp3")));
                mediaPlayer.ToggleFullScreen();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (browseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                var files = System.IO.Directory.GetFiles(browseDialog.SelectedPath, "*.cdg", searchOption: System.IO.SearchOption.AllDirectories);
                _fileList = files.Select(file => new FileInfo(file)).ToList();
                mlbSongList.DataSource = _fileList;
                mlbSongList.DisplayMember = "Name";

                lblSongs.Text = string.Format("({0:N0} / {1:N0}) Songs Displayed",
                    ((List<FileInfo>)mlbSongList.DataSource).Count, _fileList.Count);

            }
        }

        private void materialListBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int index = (Int16)mlbSongList.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                _queue.Add((FileInfo)mlbSongList.Items[index]);
                //karaokeVideoPlayer1.Play(new Uri(Path.ChangeExtension(((FileInfo)materialListBox1.Items[index]).FullName,".mp3")));
                //karaokeVideoPlayer1.ToggleFullScreen();
            }
        }

        private async void materialSingleLineTextField1_TextChanged(object sender, EventArgs e)
        {
            var text = materialSingleLineTextField1.Text;
            await Task.Delay(1000);
            
            if (text == materialSingleLineTextField1.Text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    mlbSongList.DataSource = _fileList.ToList();

                    lblSongs.Text = string.Format("({0:N0} / {1:N0}) Songs Displayed",
                        ((List<FileInfo>)mlbSongList.DataSource).Count, _fileList.Count);
                }
                else
                {
                    mlbSongList.DataSource = _fileList.Where(
                            file => Regex.IsMatch(file.Name, text,
                                RegexOptions.IgnoreCase))
                        .ToList();

                    lblSongs.Text = string.Format("({0:N0} / {1:N0}) Songs Displayed",
                        ((List<FileInfo>) mlbSongList.DataSource).Count, _fileList.Count);
                }
            }
            
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            playToolStripMenuItem.MouseDown += (o, args) => {

                

                int index = mlbSongList.SelectedIndex;
                if (index != ListBox.NoMatches)
                {
                    var file = (FileInfo) mlbSongList.Items[index];
                    mediaPlayer.Play(new Uri(Path.ChangeExtension(file.FullName, ".mp3")));
                    mediaPlayer.ToggleFullScreen();
                }
            };
            addToQueueToolStripMenuItem.MouseDown += (o, args) =>
            {
                int index = mlbSongList.SelectedIndex;
                if (index != ListBox.NoMatches)
                {
                    _queue.Add((FileInfo)mlbSongList.Items[index]);
                    //karaokeVideoPlayer1.Play(new Uri(Path.ChangeExtension(((FileInfo)materialListBox1.Items[index]).FullName,".mp3")));
                    //karaokeVideoPlayer1.ToggleFullScreen();
                }
            };

            var files = System.IO.Directory.GetFiles(browseDialog.SelectedPath, "*.cdg", searchOption: System.IO.SearchOption.AllDirectories);
            _fileList = files.Select(file => new FileInfo(file)).OrderBy(file => file.Name).ToList();
            mlbSongList.DataSource = _fileList;
            mlbSongList.DisplayMember = "Name";
            mlbQueue.DataSource = _queue;
            mlbQueue.DisplayMember = "Name";

            lblSongs.Text = string.Format("({0:N0} / {1:N0}) Songs Displayed",
                ((List<FileInfo>)mlbSongList.DataSource).Count, _fileList.Count);

            string apiKey = System.Configuration.ConfigurationManager.AppSettings["ApiKey"];

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = "Karaoke"; 
            searchListRequest.MaxResults = 50;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }
        }

        private void mlbQueue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int index = mlbQueue.SelectedIndex;
                if (index != ListBox.NoMatches)
                {
                    _queue.RemoveAt(index);
                }
            }
        }


        private void mlbQueue_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            int index = (Int16)mlbQueue.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var file = _queue[index];
                _queue.Remove(file);

                mediaPlayer.Play(new Uri(Path.ChangeExtension(file.FullName, ".mp3")));
                mediaPlayer.ToggleFullScreen();
            }
        }

        private void mlbQueue_Move(object sender, EventArgs e)
        {

        }

        private void mlbQueue_Resize(object sender, EventArgs e)
        {

        }

        private void mlbQueue_SizeChanged(object sender, EventArgs e)
        {
            lblQueue.Location = new Point(tableLayoutPanel1.Location.X + splitContainer2.Panel2.Location.X + splitContainer1.Panel2.Location.X + splitContainer1.Location.X, lblQueue.Location.Y); 
        }

        private void mlbQueue_LocationChanged(object sender, EventArgs e)
        {

        }

        private void PlayNextSong()
        {
            if (_queue.Count > 0)
            {
                BeginInvoke(new MethodInvoker(() => {
                    var file = _queue[0];
                    _queue.Remove(file);
                    mediaPlayer.Play(new Uri(Path.ChangeExtension(file.FullName, ".mp3")));
                }));
                return;
            }
            Random rnd = new Random();
            int index = rnd.Next(0, mlbSongList.Items.Count - 1);
            BeginInvoke(new MethodInvoker(() => {
                mediaPlayer.Play(new Uri(Path.ChangeExtension(((FileInfo)mlbSongList.Items[index]).FullName, ".mp3")));
            }));
        }

        private void mediaPlayer_SongFinished(object sender, EventArgs e)
        {
            PlayNextSong();
        }

        private void mlbSongList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var index = (Int16)mlbSongList.IndexFromPoint(e.X, e.Y);
                if (index != ListBox.NoMatches)
                {
                    mlbSongList.SelectedIndex = index;
                }
            }
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            mlbSongList.BeginUpdate();
            mlbQueue.BeginUpdate();

        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            mlbSongList.EndUpdate();
            mlbQueue.EndUpdate();
        }

        private void mlbQueue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var index = (Int16)mlbQueue.IndexFromPoint(e.X, e.Y);
                if (index != ListBox.NoMatches)
                {
                    mlbQueue.SelectedIndex = index;
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int index = mlbQueue.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                var file = _queue[index];
                _queue.Remove(file);

                mediaPlayer.Play(new Uri(Path.ChangeExtension(file.FullName, ".mp3")));
                mediaPlayer.ToggleFullScreen();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int index = mlbQueue.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                _queue.RemoveAt(index);
            }
        }
    }
}
