using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System;

namespace LivelyWall
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private string filePath;
        private IntPtr originalParent;

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

        public Form1(string filePath)
        {
            this.filePath = filePath;
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

        public void SetDefaultWallpaper()
        {
            timer.Stop(); 
            axWindowsMediaPlayer1.close();
            SetWallpaper("C:/Windows/Web/Wallpaper/Windows/img0.jpg");
        }

        private void InitializeTransparentFormProperties()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 1;

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            int widthWithoutTaskbar = screen.Width;
            int heightWithoutTaskbar = screen.Height;

            this.Size = new Size(widthWithoutTaskbar, heightWithoutTaskbar);
            // Set the location of the form to the top-left corner of the working area
            this.Location = new Point(screen.Left, screen.Top);
        }

        private void InitializeMediaPlayer()
        {
            axWindowsMediaPlayer1.Size = this.Size;
            axWindowsMediaPlayer1.uiMode = "none"; // Hide the player controls
            axWindowsMediaPlayer1.URL = filePath; // Path to your video file
            axWindowsMediaPlayer1.settings.autoStart = true; // Start playing automatically
            axWindowsMediaPlayer1.stretchToFit = true;
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
            IntPtr progman = FindWindow("Progman", null);
            IntPtr result = IntPtr.Zero;

            // Send 0x052C to Progman to recreate the WorkerW windows
            SendMessage(progman, 0x052C, new IntPtr(0), new IntPtr(0));

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                IntPtr shelldll_defview = FindWindowEx(wnd, IntPtr.Zero, "SHELLDLL_DefView", null);
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
                // If you're in the Form's Load event, you can close the form. This will end the application if it's the main form.
                this.Close();
            }
        }

        private void InitializeTimer()
        {
            timer = new Timer();
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
