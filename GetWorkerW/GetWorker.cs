using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LivelyWall.GetWorkerW
{
    public class GetWorker
    {
        IntPtr result { get; set; }
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

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        public GetWorker()
        {
            FindTheWorkerW();
        }
        public IntPtr GetWorkerW()
        {
            return result;
        }
        public void SetVideo(IntPtr handle)
        {
            if (result != IntPtr.Zero && handle != IntPtr.Zero)
            {
                // Try to set the parent of the handle
                IntPtr oldParent = SetParent(handle, result);

                if (oldParent == IntPtr.Zero)
                {
                    // If SetParent fails, display the error
                    int errorCode = Marshal.GetLastWin32Error();
                    MessageBox.Show($"SetParent failed with error code: {errorCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Display an error message if the handles are not valid
                MessageBox.Show("Unable to find the desktop worker window. The application will now exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // Optionally, exit the application if handles are invalid
            }
        }
        private void FindTheWorkerW()
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
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        public void SetDefaultWallpaper(string imagePath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }
    }
}
