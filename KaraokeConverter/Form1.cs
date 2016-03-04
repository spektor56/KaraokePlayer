using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CdgLib;

namespace KaraokeConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region "Events"

        private void mExportAVI_Status(string message)
        {
            pbAVI.Value = Convert.ToInt32(message);
        }

        #endregion

        #region "Private Declarations"

        private GraphicsFile mCDGFile;
        private CdgFileIoStream mCDGStream;
        private string mCDGFileName;
        private string mMP3FileName;
        private string mTempDir;
        private ExportAVI withEventsField_mExportAVI;

        private ExportAVI mExportAVI
        {
            get { return withEventsField_mExportAVI; }
            set
            {
                if (withEventsField_mExportAVI != null)
                {
                    withEventsField_mExportAVI.Status -= mExportAVI_Status;
                }
                withEventsField_mExportAVI = value;
                if (withEventsField_mExportAVI != null)
                {
                    withEventsField_mExportAVI.Status += mExportAVI_Status;
                }
            }
        }

        #endregion

        #region "Control Events"

        private void btOutputAVI_Click_1(object sender, EventArgs e)
        {
            SelectOutputAVI();
        }

        private void btBackGroundBrowse_Click(object sender, EventArgs e)
        {
            SelectBackGroundAVI();
        }

        private void btConvert_Click(object sender, EventArgs e)
        {
            ConvertAVI();
        }

        private void tbFPS_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
			if ((Strings.Asc(e.KeyChar) >= Keys.D0 & Strings.Asc(e.KeyChar) <= Keys.D9) | Strings.Asc(e.KeyChar) == Keys.Back | e.KeyChar == ".") {
				e.Handled = false;
			} else {
				e.Handled = true;
			}
            */
        }

        private void btBrowseCDG_Click(object sender, EventArgs e)
        {
            OpenFileDialog1.Filter = "CDG or Zip Files (*.zip, *.cdg)|*.zip;*.cdg";
            OpenFileDialog1.ShowDialog();
            tbFileName.Text = OpenFileDialog1.FileName;
        }

        private void chkBackGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBackGround.Checked && chkBackGraph.Checked)
            {
                chkBackGround.Checked = false;
            }
            ToggleCheckBox();
        }

        private void chkBackGround_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBackGraph.Checked && chkBackGround.Checked)
            {
                chkBackGraph.Checked = false;
            }
            ToggleCheckBox();
        }

        private void btBrowseImg_Click(object sender, EventArgs e)
        {
            SelectBackGroundGraphic();
        }

        #endregion

        #region "Private Methods"

        private void SelectOutputAVI()
        {
            SaveFileDialog1.Filter = "AVI Files (*.avi)|*.avi";
            SaveFileDialog1.ShowDialog();
            tbAVIFile.Text = SaveFileDialog1.FileName;
        }

        private void SelectBackGroundAVI()
        {
            OpenFileDialog1.Filter = "Movie Files (*.avi, *.mpg, *.wmv)|*.avi;*.mpg;*.wmv";
            OpenFileDialog1.ShowDialog();
            tbBackGroundAVI.Text = OpenFileDialog1.FileName;
        }

        private void SelectBackGroundGraphic()
        {
            OpenFileDialog1.Filter = "Graphic Files|*.jpg;*.bmp;*.png;*.tif;*.tiff;*.gif;*.wmf";
            OpenFileDialog1.ShowDialog();
            tbBackGroundImg.Text = OpenFileDialog1.FileName;
        }

        private void ConvertAVI()
        {
            try
            {
                PreProcessFiles();
                if (string.IsNullOrEmpty(mCDGFileName) | string.IsNullOrEmpty(mMP3FileName))
                {
                    MessageBox.Show("Cannot find a CDG and MP3 file to convert together.");
                    return;
                }
            }
            catch (Exception ex)
            {
                //Do nothing for now
            }
            mExportAVI = new ExportAVI();
            pbAVI.Value = 0;
            var backGroundFilename = "";
            if (chkBackGraph.Checked)
                backGroundFilename = tbBackGroundImg.Text;
            if (chkBackGround.Checked)
                backGroundFilename = tbBackGroundAVI.Text;
            mExportAVI.CDGtoAVI(tbAVIFile.Text, mCDGFileName, mMP3FileName, Convert.ToDouble(tbFPS.Text),
                backGroundFilename);
            pbAVI.Value = 0;
            try
            {
                CleanUp();
            }
            catch (Exception ex)
            {
                //Do nothing for now
            }
        }

        private void CleanUp()
        {
            if (!string.IsNullOrEmpty(mTempDir))
            {
                try
                {
                    Directory.Delete(mTempDir, true);
                }
                catch (Exception ex)
                {
                }
            }
            mTempDir = "";
        }

        private void PreProcessFiles()
        {
            
			string myCDGFileName = "";
			if (Regex.IsMatch(tbFileName.Text, "\\.zip$")) {
				string myTempDir = Path.GetTempPath() + Path.GetRandomFileName();
				Directory.CreateDirectory(myTempDir);
				mTempDir = myTempDir;
				myCDGFileName = Unzip.UnzipMP3GFiles(tbFileName.Text, myTempDir);
			} else if (Regex.IsMatch(tbFileName.Text, "\\.cdg$")) {
				myCDGFileName = tbFileName.Text;
				PairUpFiles:
				string myMP3FileName = System.Text.RegularExpressions.Regex.Replace(myCDGFileName, "\\.cdg$", ".mp3");
				if (File.Exists(myMP3FileName)) {
					mMP3FileName = myMP3FileName;
					mCDGFileName = myCDGFileName;
					mTempDir = "";
				}
			}
            
        }


        private void ToggleCheckBox()
        {
            tbBackGroundAVI.Enabled = chkBackGround.Checked;
            btBackGroundBrowse.Enabled = chkBackGround.Checked;
            tbBackGroundImg.Enabled = chkBackGraph.Checked;
            btBrowseImg.Enabled = chkBackGraph.Checked;
        }

        #endregion

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         


        }
    }
}