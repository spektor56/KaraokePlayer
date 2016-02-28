namespace KaraokeConverter
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.btBrowseCDG = new System.Windows.Forms.Button();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.chkBackGraph = new System.Windows.Forms.CheckBox();
            this.tbBackGroundImg = new System.Windows.Forms.TextBox();
            this.btBrowseImg = new System.Windows.Forms.Button();
            this.chkBackGround = new System.Windows.Forms.CheckBox();
            this.tbBackGroundAVI = new System.Windows.Forms.TextBox();
            this.btBackGroundBrowse = new System.Windows.Forms.Button();
            this.lbSaveAs = new System.Windows.Forms.Label();
            this.tbAVIFile = new System.Windows.Forms.TextBox();
            this.btOutputAVI = new System.Windows.Forms.Button();
            this.tbFPS = new System.Windows.Forms.TextBox();
            this.lbFPS = new System.Windows.Forms.Label();
            this.btConvert = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.pbAVI = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.vlcVideo = new AxAXVLC.AxVLCPlugin2();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Panel1.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(9, 13);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.ReadOnly = true;
            this.tbFileName.Size = new System.Drawing.Size(475, 20);
            this.tbFileName.TabIndex = 0;
            this.tbFileName.Text = "D:\\Karaoke\\SF001 - SF339 Sunfly Hits Karaoke Complete\\SF339\\SF339-01 - Kiesza - H" +
    "ideaway.cdg";
            // 
            // btBrowseCDG
            // 
            this.btBrowseCDG.Location = new System.Drawing.Point(490, 11);
            this.btBrowseCDG.Name = "btBrowseCDG";
            this.btBrowseCDG.Size = new System.Drawing.Size(68, 23);
            this.btBrowseCDG.TabIndex = 1;
            this.btBrowseCDG.Text = "Browse...";
            this.btBrowseCDG.UseVisualStyleBackColor = true;
            this.btBrowseCDG.Click += new System.EventHandler(this.btBrowseCDG_Click);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.GroupBox3);
            this.Panel1.Controls.Add(this.GroupBox2);
            this.Panel1.Controls.Add(this.GroupBox1);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(649, 255);
            this.Panel1.TabIndex = 3;
            this.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.chkBackGraph);
            this.GroupBox3.Controls.Add(this.tbBackGroundImg);
            this.GroupBox3.Controls.Add(this.btBrowseImg);
            this.GroupBox3.Controls.Add(this.chkBackGround);
            this.GroupBox3.Controls.Add(this.tbBackGroundAVI);
            this.GroupBox3.Controls.Add(this.btBackGroundBrowse);
            this.GroupBox3.Controls.Add(this.lbSaveAs);
            this.GroupBox3.Controls.Add(this.tbAVIFile);
            this.GroupBox3.Controls.Add(this.btOutputAVI);
            this.GroupBox3.Controls.Add(this.tbFPS);
            this.GroupBox3.Controls.Add(this.lbFPS);
            this.GroupBox3.Controls.Add(this.btConvert);
            this.GroupBox3.Location = new System.Drawing.Point(3, 53);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(571, 145);
            this.GroupBox3.TabIndex = 18;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "AVI Settings";
            // 
            // chkBackGraph
            // 
            this.chkBackGraph.AutoSize = true;
            this.chkBackGraph.Location = new System.Drawing.Point(7, 79);
            this.chkBackGraph.Name = "chkBackGraph";
            this.chkBackGraph.Size = new System.Drawing.Size(122, 17);
            this.chkBackGraph.TabIndex = 23;
            this.chkBackGraph.Text = "Background graphic";
            this.chkBackGraph.UseVisualStyleBackColor = true;
            this.chkBackGraph.CheckedChanged += new System.EventHandler(this.chkBackGraph_CheckedChanged);
            // 
            // tbBackGroundImg
            // 
            this.tbBackGroundImg.Enabled = false;
            this.tbBackGroundImg.Location = new System.Drawing.Point(128, 77);
            this.tbBackGroundImg.Name = "tbBackGroundImg";
            this.tbBackGroundImg.Size = new System.Drawing.Size(356, 20);
            this.tbBackGroundImg.TabIndex = 21;
            // 
            // btBrowseImg
            // 
            this.btBrowseImg.Enabled = false;
            this.btBrowseImg.Location = new System.Drawing.Point(490, 75);
            this.btBrowseImg.Name = "btBrowseImg";
            this.btBrowseImg.Size = new System.Drawing.Size(75, 23);
            this.btBrowseImg.TabIndex = 22;
            this.btBrowseImg.Text = "Browse...";
            this.btBrowseImg.UseVisualStyleBackColor = true;
            this.btBrowseImg.Click += new System.EventHandler(this.btBrowseImg_Click);
            // 
            // chkBackGround
            // 
            this.chkBackGround.AutoSize = true;
            this.chkBackGround.Location = new System.Drawing.Point(7, 51);
            this.chkBackGround.Name = "chkBackGround";
            this.chkBackGround.Size = new System.Drawing.Size(115, 17);
            this.chkBackGround.TabIndex = 20;
            this.chkBackGround.Text = "Background movie";
            this.chkBackGround.UseVisualStyleBackColor = true;
            this.chkBackGround.CheckedChanged += new System.EventHandler(this.chkBackGround_CheckedChanged);
            // 
            // tbBackGroundAVI
            // 
            this.tbBackGroundAVI.Enabled = false;
            this.tbBackGroundAVI.Location = new System.Drawing.Point(128, 49);
            this.tbBackGroundAVI.Name = "tbBackGroundAVI";
            this.tbBackGroundAVI.Size = new System.Drawing.Size(356, 20);
            this.tbBackGroundAVI.TabIndex = 17;
            this.tbBackGroundAVI.Text = "C:\\Users\\l-bre\\Downloads\\Kristel\'s Jams\\drop.avi";
            // 
            // btBackGroundBrowse
            // 
            this.btBackGroundBrowse.Enabled = false;
            this.btBackGroundBrowse.Location = new System.Drawing.Point(490, 47);
            this.btBackGroundBrowse.Name = "btBackGroundBrowse";
            this.btBackGroundBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBackGroundBrowse.TabIndex = 18;
            this.btBackGroundBrowse.Text = "Browse...";
            this.btBackGroundBrowse.UseVisualStyleBackColor = true;
            this.btBackGroundBrowse.Click += new System.EventHandler(this.btBackGroundBrowse_Click);
            // 
            // lbSaveAs
            // 
            this.lbSaveAs.AutoSize = true;
            this.lbSaveAs.Location = new System.Drawing.Point(76, 22);
            this.lbSaveAs.Name = "lbSaveAs";
            this.lbSaveAs.Size = new System.Drawing.Size(46, 13);
            this.lbSaveAs.TabIndex = 16;
            this.lbSaveAs.Text = "Save as";
            // 
            // tbAVIFile
            // 
            this.tbAVIFile.Location = new System.Drawing.Point(128, 19);
            this.tbAVIFile.Name = "tbAVIFile";
            this.tbAVIFile.Size = new System.Drawing.Size(356, 20);
            this.tbAVIFile.TabIndex = 9;
            this.tbAVIFile.Text = "C:\\Users\\l-bre\\Desktop\\tester.avi";
            // 
            // btOutputAVI
            // 
            this.btOutputAVI.Location = new System.Drawing.Point(490, 17);
            this.btOutputAVI.Name = "btOutputAVI";
            this.btOutputAVI.Size = new System.Drawing.Size(75, 23);
            this.btOutputAVI.TabIndex = 10;
            this.btOutputAVI.Text = "Browse...";
            this.btOutputAVI.UseVisualStyleBackColor = true;
            this.btOutputAVI.Click += new System.EventHandler(this.btOutputAVI_Click_1);
            // 
            // tbFPS
            // 
            this.tbFPS.Location = new System.Drawing.Point(128, 108);
            this.tbFPS.Name = "tbFPS";
            this.tbFPS.Size = new System.Drawing.Size(67, 20);
            this.tbFPS.TabIndex = 15;
            this.tbFPS.Text = "15";
            // 
            // lbFPS
            // 
            this.lbFPS.AutoSize = true;
            this.lbFPS.Location = new System.Drawing.Point(201, 111);
            this.lbFPS.Name = "lbFPS";
            this.lbFPS.Size = new System.Drawing.Size(94, 13);
            this.lbFPS.TabIndex = 12;
            this.lbFPS.Text = "frames per second";
            // 
            // btConvert
            // 
            this.btConvert.Location = new System.Drawing.Point(490, 106);
            this.btConvert.Name = "btConvert";
            this.btConvert.Size = new System.Drawing.Size(75, 23);
            this.btConvert.TabIndex = 13;
            this.btConvert.Text = "Create AVI";
            this.btConvert.UseVisualStyleBackColor = true;
            this.btConvert.Click += new System.EventHandler(this.btConvert_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.tbFileName);
            this.GroupBox2.Controls.Add(this.btBrowseCDG);
            this.GroupBox2.Location = new System.Drawing.Point(3, 3);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(571, 40);
            this.GroupBox2.TabIndex = 17;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "MP3 + CDG File";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.pbAVI);
            this.GroupBox1.Location = new System.Drawing.Point(3, 204);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(571, 48);
            this.GroupBox1.TabIndex = 16;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Progress";
            // 
            // pbAVI
            // 
            this.pbAVI.Location = new System.Drawing.Point(7, 19);
            this.pbAVI.Name = "pbAVI";
            this.pbAVI.Size = new System.Drawing.Size(555, 23);
            this.pbAVI.TabIndex = 14;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(207, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(392, 151);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.pictureBox1);
            this.Panel2.Controls.Add(this.vlcVideo);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2.Location = new System.Drawing.Point(0, 255);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(649, 225);
            this.Panel2.TabIndex = 4;
            this.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel2_Paint);
            // 
            // vlcVideo
            // 
            this.vlcVideo.Enabled = true;
            this.vlcVideo.Location = new System.Drawing.Point(317, 47);
            this.vlcVideo.Name = "vlcVideo";
            this.vlcVideo.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("vlcVideo.OcxState")));
            this.vlcVideo.Size = new System.Drawing.Size(320, 175);
            this.vlcVideo.TabIndex = 25;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 480);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.Name = "Form1";
            this.Text = "MP3+CDG To Video Converter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Panel1.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcVideo)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Button btBrowseCDG;

        private System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.Panel Panel2;
        private System.Windows.Forms.GroupBox GroupBox2;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.ProgressBar pbAVI;
        private System.Windows.Forms.TextBox tbFPS;

        private System.Windows.Forms.Button btConvert;

        private System.Windows.Forms.Label lbFPS;
        private System.Windows.Forms.Button btOutputAVI;

        private System.Windows.Forms.TextBox tbAVIFile;
        private System.Windows.Forms.GroupBox GroupBox3;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        private System.Windows.Forms.TextBox tbBackGroundAVI;
        private System.Windows.Forms.Button btBackGroundBrowse;

        private System.Windows.Forms.Label lbSaveAs;
        private System.Windows.Forms.CheckBox chkBackGround;

        private System.Windows.Forms.CheckBox chkBackGraph;

        private System.Windows.Forms.TextBox tbBackGroundImg;
        private System.Windows.Forms.Button btBrowseImg;


        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private AxAXVLC.AxVLCPlugin2 vlcVideo;
    }
}

