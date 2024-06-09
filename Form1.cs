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
        private readonly string filePath;
        private readonly double playbackspeed;
        IntPtr result = IntPtr.Zero;
        IntPtr progman = IntPtr.Zero;
        IntPtr shelldll_defview = IntPtr.Zero;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public Form1(string filePath, double playbackspeed = 1.0)
        {
            this.filePath = filePath;
            this.playbackspeed = playbackspeed;
            this.screenDetails = new ScreenDetails();
            InitializeComponent();
            InitializeTransparentFormProperties();
        }
        protected override void OnLoad(EventArgs e)
        {
            if (filePath != null && filePath != "")
            {
                base.OnLoad(e);
                FindTheWindowAndReparent();
                InitializeMediaPlayer();
                InitializeTimer();
            }

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            timer.Stop();
            axWindowsMediaPlayer1.close();
            DetachAndRestore();
            SetDefaultWallpaper();
        }

        public void SetDefaultWallpaper()
        {
            SetWallpaper("C:/Windows/Web/Wallpaper/Windows/img0.jpg");
        }

        private void InitializeTransparentFormProperties()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 1;
            this.Size = this.screenDetails.Dimensions();
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
            if (axWindowsMediaPlayer1.Ctlcontrols.currentPosition > axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration - 0.01)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
            }
        }

        private void FindTheWindowAndReparent()
        {
            this.progman = FindWindow("Progman", null);

            // Send 0x052C to Progman to recreate the WorkerW windows
            SendMessage(progman, 0x052C, new IntPtr(0), new IntPtr(0));

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                shelldll_defview = FindWindowEx(wnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shelldll_defview != IntPtr.Zero)
                {
                    result = FindWindowEx(IntPtr.Zero, wnd, "WorkerW", null);
                }
                return true;
            }, IntPtr.Zero);

            if (result != IntPtr.Zero)
            {
                SetParent(this.Handle, result);
            }
            else
            {
                MessageBox.Show("Unable to find the desktop worker window. The application will now exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                this.Dispose();
            }

        }
        private void DetachAndRestore()
        {
            if (result != IntPtr.Zero)
            {
                SetParent(this.Handle, this.progman);
            }
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Start();
            timer.Tick += timer1_Tick;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        private void SetWallpaper(string imagePath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

    }
}
