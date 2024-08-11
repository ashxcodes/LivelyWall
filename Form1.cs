using System.Drawing;
using System.Windows.Forms;
using System;

namespace LivelyWall
{
    public partial class Form1 : Form
    {
        private readonly ScreenDetails screenDetails;
        private System.Windows.Forms.Timer timer;
        private WallpaperDetails Details;

        public Form1(WallpaperDetails _Details)
        {
            Details = _Details;
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
            vlcControl1.Parent = this;
            vlcControl1.Size = this.Size;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (vlcControl1.IsPlaying && vlcControl1.Length > 0)
            {
                long currentTime = vlcControl1.Time; // in milliseconds
                TimeSpan duration = vlcControl1.GetCurrentMedia().Duration;

                // Convert duration to milliseconds for comparison
                double totalDurationInMilliseconds = duration.TotalMilliseconds;

                // If we're close to the end of the video, restart it
                if (currentTime >= totalDurationInMilliseconds - 1000) // 1 second before end
                {
                    StartVideo();
                }
            }
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Start();
            timer.Tick += Timer1_Tick;
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
            Details.FilePath = filepath;
            Details.PlaybackSpeed = playback;
        }

        public void PauseVideo()
        {
            timer.Stop();
            vlcControl1.Pause();
        }

        public void StartVideo()
        {
            timer.Start(); 
            vlcControl1.Play(new Uri(Details.FilePath));
            vlcControl1.Rate = (float)Details.PlaybackSpeed;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
