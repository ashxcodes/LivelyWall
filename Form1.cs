using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System;

namespace LivelyWall
{
    public partial class Form1 : Form
    {
        private readonly ScreenDetails screenDetails;
        private System.Windows.Forms.Timer timer;
        private string filePath;
        private double playbackspeed;

        public Form1(string filePath, double playbackspeed = 1.0)
        {
            this.filePath = filePath;
            this.playbackspeed = playbackspeed;
            this.screenDetails = new ScreenDetails();
            InitializeComponent();
            InitializeTransparentFormProperties();
            InitializeMediaPlayer();
            InitializeTimer();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            timer.Stop();
            axWindowsMediaPlayer1.close();
        }

        private void InitializeTransparentFormProperties()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 1;
            Size scn = this.screenDetails.Dimensions();
            this.Size = new Size(scn.Width,scn.Height);
            this.Location = new Point(this.screenDetails.PrimaryScreen().Bounds.Left, this.screenDetails.PrimaryScreen().Bounds.Top);
        }

        private void InitializeMediaPlayer()
        {
            axWindowsMediaPlayer1.Size = this.Size;
            axWindowsMediaPlayer1.uiMode = "none"; // Hide the player controls
            axWindowsMediaPlayer1.URL = filePath; // Path to your video file
            axWindowsMediaPlayer1.settings.autoStart = true; // Start playing automatically
            axWindowsMediaPlayer1.stretchToFit = true;
            axWindowsMediaPlayer1.settings.rate = this.playbackspeed;
            axWindowsMediaPlayer1.settings.setMode("loop", true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.Ctlcontrols.currentItem != null && axWindowsMediaPlayer1.Ctlcontrols.currentPosition > axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration - 0.01)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
            }
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Start();
            timer.Tick += timer1_Tick;
        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                Controller.Controller.Instance.ToggleHomePage();
            }
        }

        public void UpdateValues(string filepath, double playback)
        {
            this.filePath = filepath;
            this.playbackspeed = playback;
        }

        public void StopVideo()
        {
            timer.Stop();
            axWindowsMediaPlayer1.close();
        }

        public void StartVideo()
        {
            timer.Start();
            axWindowsMediaPlayer1.URL = this.filePath;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
