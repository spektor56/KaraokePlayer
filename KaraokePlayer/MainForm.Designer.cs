using CdgPlayer;

namespace KaraokePlayer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPlay = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnBrowse = new MaterialSkin.Controls.MaterialRaisedButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.mlbSongList = new MaterialSkin.Controls.MaterialListBox();
            this.materialSingleLineTextField1 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.mediaPlayer = new CdgPlayer.KaraokeVideoPlayer();
            this.mlbQueue = new MaterialSkin.Controls.MaterialListBox();
            this.browseDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lblSongs = new System.Windows.Forms.Label();
            this.lblQueue = new System.Windows.Forms.Label();
            this.cmsSongList = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToQueueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsQueue = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.cmsSongList.SuspendLayout();
            this.cmsQueue.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 95);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1253, 489);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.btnPlay);
            this.flowLayoutPanel1.Controls.Add(this.btnBrowse);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 444);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1247, 42);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.AutoSize = true;
            this.btnPlay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPlay.Depth = 0;
            this.btnPlay.Icon = null;
            this.btnPlay.Location = new System.Drawing.Point(1189, 3);
            this.btnPlay.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Primary = true;
            this.btnPlay.Size = new System.Drawing.Size(55, 36);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.AutoSize = true;
            this.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowse.Depth = 0;
            this.btnBrowse.Icon = null;
            this.btnBrowse.Location = new System.Drawing.Point(1107, 3);
            this.btnBrowse.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Primary = true;
            this.btnBrowse.Size = new System.Drawing.Size(76, 36);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1247, 435);
            this.splitContainer1.SplitterDistance = 290;
            this.splitContainer1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.mlbSongList, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.materialSingleLineTextField1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(290, 435);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // mlbSongList
            // 
            this.mlbSongList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mlbSongList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mlbSongList.ContextMenuStrip = this.cmsSongList;
            this.mlbSongList.Depth = 0;
            this.mlbSongList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mlbSongList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.mlbSongList.Font = new System.Drawing.Font("Roboto", 24F);
            this.mlbSongList.FormattingEnabled = true;
            this.mlbSongList.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.mlbSongList.Location = new System.Drawing.Point(3, 32);
            this.mlbSongList.MouseLocation = new System.Drawing.Point(-1, -1);
            this.mlbSongList.MouseState = MaterialSkin.MouseState.HOVER;
            this.mlbSongList.Name = "mlbSongList";
            this.mlbSongList.Size = new System.Drawing.Size(286, 400);
            this.mlbSongList.TabIndex = 5;
            this.mlbSongList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.materialListBox1_MouseDoubleClick);
            this.mlbSongList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mlbSongList_MouseDown);
            // 
            // materialSingleLineTextField1
            // 
            this.materialSingleLineTextField1.Depth = 0;
            this.materialSingleLineTextField1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialSingleLineTextField1.Hint = "";
            this.materialSingleLineTextField1.Location = new System.Drawing.Point(3, 3);
            this.materialSingleLineTextField1.MaxLength = 32767;
            this.materialSingleLineTextField1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialSingleLineTextField1.Name = "materialSingleLineTextField1";
            this.materialSingleLineTextField1.PasswordChar = '\0';
            this.materialSingleLineTextField1.SelectedText = "";
            this.materialSingleLineTextField1.SelectionLength = 0;
            this.materialSingleLineTextField1.SelectionStart = 0;
            this.materialSingleLineTextField1.Size = new System.Drawing.Size(286, 23);
            this.materialSingleLineTextField1.TabIndex = 6;
            this.materialSingleLineTextField1.TabStop = false;
            this.materialSingleLineTextField1.UseSystemPasswordChar = false;
            this.materialSingleLineTextField1.TextChanged += new System.EventHandler(this.materialSingleLineTextField1_TextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.mediaPlayer);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mlbQueue);
            this.splitContainer2.Panel2MinSize = 150;
            this.splitContainer2.Size = new System.Drawing.Size(953, 435);
            this.splitContainer2.SplitterDistance = 654;
            this.splitContainer2.TabIndex = 0;
            // 
            // mediaPlayer
            // 
            this.mediaPlayer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.mediaPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPlayer.Location = new System.Drawing.Point(0, 0);
            this.mediaPlayer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mediaPlayer.Name = "mediaPlayer";
            this.mediaPlayer.Size = new System.Drawing.Size(654, 435);
            this.mediaPlayer.TabIndex = 1;
            this.mediaPlayer.SongFinished += new System.EventHandler(this.mediaPlayer_SongFinished);
            // 
            // mlbQueue
            // 
            this.mlbQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mlbQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mlbQueue.ContextMenuStrip = this.cmsQueue;
            this.mlbQueue.Depth = 0;
            this.mlbQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mlbQueue.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.mlbQueue.Font = new System.Drawing.Font("Roboto", 24F);
            this.mlbQueue.FormattingEnabled = true;
            this.mlbQueue.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.mlbQueue.Location = new System.Drawing.Point(0, 0);
            this.mlbQueue.MouseLocation = new System.Drawing.Point(-1, -1);
            this.mlbQueue.MouseState = MaterialSkin.MouseState.HOVER;
            this.mlbQueue.Name = "mlbQueue";
            this.mlbQueue.Size = new System.Drawing.Size(295, 435);
            this.mlbQueue.TabIndex = 8;
            this.mlbQueue.LocationChanged += new System.EventHandler(this.mlbQueue_LocationChanged);
            this.mlbQueue.SizeChanged += new System.EventHandler(this.mlbQueue_SizeChanged);
            this.mlbQueue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mlbQueue_KeyDown);
            this.mlbQueue.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mlbQueue_MouseDoubleClick_1);
            this.mlbQueue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mlbQueue_MouseDown);
            this.mlbQueue.Move += new System.EventHandler(this.mlbQueue_Move);
            this.mlbQueue.Resize += new System.EventHandler(this.mlbQueue_Resize);
            // 
            // browseDialog
            // 
            this.browseDialog.SelectedPath = "E:\\Karaoke\\Sunfly\\Sunfly Hits\\SF383";
            // 
            // lblSongs
            // 
            this.lblSongs.AutoSize = true;
            this.lblSongs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSongs.Location = new System.Drawing.Point(14, 71);
            this.lblSongs.Name = "lblSongs";
            this.lblSongs.Size = new System.Drawing.Size(52, 21);
            this.lblSongs.TabIndex = 6;
            this.lblSongs.Text = "label1";
            // 
            // lblQueue
            // 
            this.lblQueue.AutoSize = true;
            this.lblQueue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueue.Location = new System.Drawing.Point(964, 71);
            this.lblQueue.Name = "lblQueue";
            this.lblQueue.Size = new System.Drawing.Size(96, 21);
            this.lblQueue.TabIndex = 7;
            this.lblQueue.Text = "Song Queue";
            // 
            // cmsSongList
            // 
            this.cmsSongList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmsSongList.Depth = 0;
            this.cmsSongList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.addToQueueToolStripMenuItem});
            this.cmsSongList.MouseState = MaterialSkin.MouseState.HOVER;
            this.cmsSongList.Name = "contextMenuStrip1";
            this.cmsSongList.Size = new System.Drawing.Size(147, 48);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.playToolStripMenuItem.Text = "Play";
            // 
            // addToQueueToolStripMenuItem
            // 
            this.addToQueueToolStripMenuItem.Name = "addToQueueToolStripMenuItem";
            this.addToQueueToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.addToQueueToolStripMenuItem.Text = "Add to queue";
            // 
            // cmsQueue
            // 
            this.cmsQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmsQueue.Depth = 0;
            this.cmsQueue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.cmsQueue.MouseState = MaterialSkin.MouseState.HOVER;
            this.cmsQueue.Name = "contextMenuStrip1";
            this.cmsQueue.Size = new System.Drawing.Size(153, 70);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Play";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Remove";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 596);
            this.Controls.Add(this.lblQueue);
            this.Controls.Add(this.lblSongs);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Karaoke Player";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.cmsSongList.ResumeLayout(false);
            this.cmsQueue.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FolderBrowserDialog browseDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialListBox mlbSongList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private MaterialSkin.Controls.MaterialRaisedButton btnPlay;
        private MaterialSkin.Controls.MaterialRaisedButton btnBrowse;
        private System.Windows.Forms.Label lblSongs;
        private System.Windows.Forms.SplitContainer splitContainer2;
        public KaraokeVideoPlayer mediaPlayer;
        private MaterialSkin.Controls.MaterialListBox mlbQueue;
        private System.Windows.Forms.Label lblQueue;
        private MaterialSkin.Controls.MaterialSingleLineTextField materialSingleLineTextField1;
        private MaterialSkin.Controls.MaterialContextMenuStrip cmsSongList;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToQueueToolStripMenuItem;
        private MaterialSkin.Controls.MaterialContextMenuStrip cmsQueue;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}