using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CdgLib;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace KaraokePlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region "Private Declarations"

        private CdgFile mCDGFile;
        private CdgFileIoStream mCDGStream;
        private int mSemitones = 0;
        private bool mPaused;
        private long mFrameCount;
        private bool mStop;
        private string mCDGFileName;
        private string mMP3FileName;
        private string mTempDir;
        private int mMP3Stream;
        private CDGWindow withEventsField_mCDGWindow = new CDGWindow();

        private CDGWindow mCDGWindow
        {
            get { return withEventsField_mCDGWindow; }
            set
            {
                if (withEventsField_mCDGWindow != null)
                {
                    withEventsField_mCDGWindow.FormClosing -= mCDGWindow_FormClosing;
                }
                withEventsField_mCDGWindow = value;
                if (withEventsField_mCDGWindow != null)
                {
                    withEventsField_mCDGWindow.FormClosing += mCDGWindow_FormClosing;
                }
            }
        }

        private bool mBassInitalized;

        #endregion

        #region "Control Events"

        private void Form1_Load(object sender, EventArgs e)
        {
            //Add registration key here if you have a license
            //BassNet.Registration("email@domain.com", "0000000000000000")
            try
            {
                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, Handle);
                mBassInitalized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to initialize the audio playback system.");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BrowseCDGZip();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopPlayback();
        }

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            try
            {
                StopPlayback();
            }
            catch (Exception ex)
            {
                //Do nothing for now
            }
        }

        private void tsbPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            AdjustVolume();
        }

        private void nudKey_ValueChanged(object sender, EventArgs e)
        {
            AdjustPitch();
        }

        private void mCDGWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopPlayback();
            mCDGWindow.Hide();
            e.Cancel = true;
        }

        #endregion

        #region "CDG + MP3 Playback Operations"

        private void Pause()
        {
            mPaused = !mPaused;
            if (mMP3Stream != 0)
            {
                if (Bass.BASS_ChannelIsActive(mMP3Stream) != BASSActive.BASS_ACTIVE_PLAYING)
                {
                    Bass.BASS_ChannelPlay(mMP3Stream, false);
                    tsbPause.Text = "Pause";
                }
                else
                {
                    Bass.BASS_ChannelPause(mMP3Stream);
                    tsbPause.Text = "Resume";
                }
            }
        }

        private void PlayMP3Bass(string mp3FileName)
        {
            if (mBassInitalized || Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, Handle))
            {
                mMP3Stream = 0;
                mMP3Stream = Bass.BASS_StreamCreateFile(mp3FileName, 0, 0,
                    BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                mMP3Stream = BassFx.BASS_FX_TempoCreate(mMP3Stream,
                    BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_SAMPLE_LOOP);
                if (mMP3Stream != 0)
                {
                    AdjustPitch();
                    AdjustVolume();
                    ShowCDGWindow();
                    Bass.BASS_ChannelPlay(mMP3Stream, false);
                }
                else
                {
                    throw new Exception(string.Format("Stream error: {0}", Bass.BASS_ErrorGetCode()));
                }
            }
        }

        private void StopPlaybackBass()
        {
            Bass.BASS_Stop();
            Bass.BASS_StreamFree(mMP3Stream);
            Bass.BASS_Free();
            mMP3Stream = 0;
            mBassInitalized = false;
        }

        private void StopPlayback()
        {
            mStop = true;
            HideCDGWindow();
            StopPlaybackBass();
            mCDGFile.Dispose();
            CleanUp();
        }

        private void PausePlayback()
        {
            Bass.BASS_Pause();
        }

        private void ResumePlayback()
        {
            Bass.BASS_Pause();
        }

        private void Play()
        {
            try
            {
                if (mMP3Stream != 0 && Bass.BASS_ChannelIsActive(mMP3Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    StopPlayback();
                }
                PreProcessFiles();
                if (string.IsNullOrEmpty(mCDGFileName) | string.IsNullOrEmpty(mMP3FileName))
                {
                    MessageBox.Show("Cannot find a CDG and MP3 file to play together.");
                    StopPlayback();
                    return;
                }
                mPaused = false;
                mStop = false;
                mFrameCount = 0;
                mCDGFile = new CdgFile(mCDGFileName);
                var cdgLength = mCDGFile.GetTotalDuration();
                PlayMP3Bass(mMP3FileName);
                var startTime = DateTime.Now;
                var endTime = startTime.AddMilliseconds(mCDGFile.GetTotalDuration());
                var millisecondsRemaining = cdgLength;
                while (millisecondsRemaining > 0)
                {
                    if (mStop)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    millisecondsRemaining = (long) endTime.Subtract(DateTime.Now).TotalMilliseconds;
                    var pos = cdgLength - millisecondsRemaining;
                    while (mPaused)
                    {
                        endTime = DateTime.Now.AddMilliseconds(millisecondsRemaining);
                        Application.DoEvents();
                    }
                    mCDGFile.RenderAtPosition(pos);
                    mFrameCount += 1;
                    mCDGWindow.pbLyrics.Image = mCDGFile.RgbImage;
                    mCDGWindow.pbLyrics.BackColor = ((Bitmap) mCDGFile.RgbImage).GetPixel(1, 1);
                    mCDGWindow.pbLyrics.Refresh();

                    Application.DoEvents();
                }
                StopPlayback();
            }
            catch (Exception ex)
            {
            }
        }

        private void AdjustPitch()
        {
            if (mMP3Stream != 0)
            {
                Bass.BASS_ChannelSetAttribute(mMP3Stream, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, (float) nudKey.Value);
            }
        }

        private void AdjustVolume()
        {
            if (mMP3Stream != 0)
            {
                Bass.BASS_ChannelSetAttribute(mMP3Stream, BASSAttribute.BASS_ATTRIB_VOL,
                    trbVolume.Value == 0 ? 0 : trbVolume.Value/100);
            }
        }

        #endregion

        #region "File Access"

        private void BrowseCDGZip()
        {
            OpenFileDialog1.Filter = "CDG or Zip Files (*.zip, *.cdg)|*.zip;*.cdg";
            OpenFileDialog1.ShowDialog();
            tbFileName.Text = OpenFileDialog1.FileName;
        }

        private void PreProcessFiles()
        {
            var myCDGFileName = "";
            if (Regex.IsMatch(tbFileName.Text, "\\.zip$"))
            {
                var myTempDir = Path.GetTempPath() + Path.GetRandomFileName();
                Directory.CreateDirectory(myTempDir);
                mTempDir = myTempDir;
                myCDGFileName = Unzip.UnzipMP3GFiles(tbFileName.Text, myTempDir);
            }
            else if (Regex.IsMatch(tbFileName.Text, "\\.cdg$"))
            {
                myCDGFileName = tbFileName.Text;
                var myMP3FileName = Regex.Replace(myCDGFileName, "\\.cdg$", ".mp3");
                if (File.Exists(myMP3FileName))
                {
                    mMP3FileName = myMP3FileName;
                    mCDGFileName = myCDGFileName;
                    mTempDir = "";
                }
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

        #endregion

        #region "CDG Graphics Window"

        private void ShowCDGWindow()
        {
            mCDGWindow.Show();
        }

        private void HideCDGWindow()
        {
            mCDGWindow.pbLyrics.Image = null;
            mCDGWindow.Hide();
        }

        #endregion
    }
}