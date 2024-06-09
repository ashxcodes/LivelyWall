using System.IO;
using System;
using System.Windows.Forms;

namespace LivelyWall
{
    public partial class HomePage : Form
    {
        private Form1 Form1 { get; set; }
        private readonly OpenFileDialog openFileDialog = new OpenFileDialog();
        private string filepath;
        private double playback = 1;
        private readonly ConfigManager configManager = new ConfigManager();

        public HomePage()
        {
            InitializeComponent();
            InitializeWebView();
            InitializeFormProperties();
            LoadUserConfig();
        }

        private async void InitializeWebView()
        {
            await webView1.EnsureCoreWebView2Async();
            string htmlPath = Path.Combine(Application.StartupPath, "FrontEnd", "index.html");
            webView1.Source = new Uri(htmlPath);
            webView1.WebMessageReceived += ReceiveMessageFromWebView;
        }

        private void InitializeFormProperties()
        {

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Location = new System.Drawing.Point(0, 0);
            this.ShowInTaskbar = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.AllowDrop = true;
            this.notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            this.DragOver += DragArea_DragOver;
            this.FormClosing += new FormClosingEventHandler(this.Prevent_FormClosing);

        }

        private void ReceiveMessageFromWebView(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (int.TryParse(e.WebMessageAsJson, out int message))
            {
                if (e.Source != webView1.CoreWebView2.Source)
                {
                    return;
                }

                if (message >= 0.25 && message <= 2)
                {
                    this.playback = message;
                    return;
                }

                switch (message)
                {
                    case (int)Messages.SelectBtnClick:
                        openFileDialog.Filter = "Video files|*.mp4;*.avi;*.mkv;*.mov|All files (*.*)|*.*";
                        openFileDialog.CheckFileExists = true;
                        openFileDialog.ShowDialog();
                        this.filepath = openFileDialog.FileName;
                        SendDataToWebView(this.filepath);
                    break;

                    case (int)Messages.SetBtnClick:
                        if (filepath == null || filepath == "")
                        {
                            MessageBox.Show("File Not found","Error");
                            return;
                        }
                        Form1 = new Form1(filepath, playback);
                        Form1.Show();
                        SendEventToWebView("SetButton", "Success");
                        configManager.SaveConfig(this.filepath);
                        break;

                    case (int)Messages.StopBtnClick:
                        Form1?.Close();
                        this.filepath = null;
                        SendEventToWebView("StopButton", "Success");
                    break;
                        
                    default:
                    return;
                }
            }
        }

        private async void SendDataToWebView(string data)
        {
            if (webView1 != null && webView1.CoreWebView2 != null)
            {
                string encodedString = Encoder.Encoder.EncodeTo64(data);
                string script = $"recieveDataFromForm('{encodedString}');";
                await webView1.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        private async void SendEventToWebView(string data, string type)
        {
            if (webView1 != null && webView1.CoreWebView2 != null)
            {
                string script = $"successEvent('{data}','{type}');";
                await webView1.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        private void LoadUserConfig()
        {
            UserConfig config = configManager.LoadConfig();
            if (config.Paths.Count != 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, config.Paths.Count);
                filepath = config.Paths[index];
                Form1 = new Form1(filepath, playback);
                Form1.Show();
                SendEventToWebView("SetButton", "Success");
            }

        }

        private void DragArea_DragOver(object sender, DragEventArgs e)
        {
            // Check if the data format is FileDrop
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Data.GetDataPresent(DataFormats.FileDrop).GetType();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.filepath = files[0];
                return;
            }
        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                // Toggle the visibility of the form
                if (WindowState == FormWindowState.Minimized)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    WindowState = FormWindowState.Minimized;
                    Hide();
                }
            }
        }

        private void Prevent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                Hide();
            }
        }

        enum Messages
        {
            SelectBtnClick = 111,
            SetBtnClick = 222,
            StopBtnClick= 333
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeCleanUp();
            Application.Exit();
        }

        private void CodeCleanUp()
        {
            Form1?.Close();
        }

    }
}
