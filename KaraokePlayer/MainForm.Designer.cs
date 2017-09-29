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
            this.btnPlay = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnBrowse = new MaterialSkin.Controls.MaterialRaisedButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.materialListBox1 = new MaterialSkin.Controls.MaterialListBox();
            this.materialSingleLineTextField1 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.karaokeVideoPlayer1 = new CdgPlayer.KaraokeVideoPlayer();
            this.browseDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Depth = 0;
            this.btnPlay.Location = new System.Drawing.Point(629, 3);
            this.btnPlay.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(81, 37);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Depth = 0;
            this.btnBrowse.Location = new System.Drawing.Point(542, 3);
            this.btnBrowse.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(81, 37);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 71);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(719, 513);
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 467);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(713, 43);
            this.flowLayoutPanel1.TabIndex = 6;
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.karaokeVideoPlayer1);
            this.splitContainer1.Size = new System.Drawing.Size(713, 458);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.materialListBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.materialSingleLineTextField1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(236, 458);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // materialListBox1
            // 
            this.materialListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialListBox1.Depth = 0;
            this.materialListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.materialListBox1.Font = new System.Drawing.Font("Roboto", 24F);
            this.materialListBox1.FormattingEnabled = true;
            this.materialListBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.materialListBox1.Location = new System.Drawing.Point(3, 32);
            this.materialListBox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialListBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialListBox1.Name = "materialListBox1";
            this.materialListBox1.Size = new System.Drawing.Size(231, 428);
            this.materialListBox1.TabIndex = 5;
            this.materialListBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.materialListBox1_MouseDoubleClick);
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
            this.materialSingleLineTextField1.Size = new System.Drawing.Size(231, 23);
            this.materialSingleLineTextField1.TabIndex = 6;
            this.materialSingleLineTextField1.TabStop = false;
            this.materialSingleLineTextField1.UseSystemPasswordChar = false;
            this.materialSingleLineTextField1.TextChanged += new System.EventHandler(this.materialSingleLineTextField1_TextChanged);
            // 
            // karaokeVideoPlayer1
            // 
            this.karaokeVideoPlayer1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.karaokeVideoPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.karaokeVideoPlayer1.Location = new System.Drawing.Point(0, 0);
            this.karaokeVideoPlayer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.karaokeVideoPlayer1.Name = "karaokeVideoPlayer1";
            this.karaokeVideoPlayer1.Size = new System.Drawing.Size(473, 458);
            this.karaokeVideoPlayer1.TabIndex = 0;
            this.karaokeVideoPlayer1.SongFinished += new System.EventHandler(this.karaokeVideoPlayer1_SongFinished);
            this.karaokeVideoPlayer1.Load += new System.EventHandler(this.karaokeVideoPlayer1_Load);
            // 
            // browseDialog
            // 
            this.browseDialog.SelectedPath = "E:\\Karaoke\\Sunfly";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 596);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Karaoke Player";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private MaterialSkin.Controls.MaterialRaisedButton btnPlay;
        private MaterialSkin.Controls.MaterialRaisedButton btnBrowse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FolderBrowserDialog browseDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private KaraokeVideoPlayer karaokeVideoPlayer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialListBox materialListBox1;
        private MaterialSkin.Controls.MaterialSingleLineTextField materialSingleLineTextField1;
    }
}