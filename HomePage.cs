using System.IO;
using System;
using System.Windows.Forms;

namespace LivelyWall
{
    public partial class HomePage : Form
    {
        private readonly OpenFileDialog openFileDialog = new OpenFileDialog();
        private readonly ConfigManager configManager = new ConfigManager();
        private Form1 Form1 { get; set; }
        private string filepath;
        private double playback = 1;

        public HomePage(Form1 Form1)
        {
            this.Form1 = Form1;
            InitializeComponent();
            InitializeWebView();
            InitializeFormProperties();
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
                        Form1.UpdateValues(filepath, playback);
                        Controller.Controller.Instance.SetVideo();
                        SendEventToWebView("SetButton", "Success");
                        configManager.SaveConfig(this.filepath);
                    break;

                    case (int)Messages.StopBtnClick:
                        Form1?.StopVideo();
                        Form1?.Hide();
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
                string script = $"handleEvent('{data}','{type}');";
                await webView1.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        private async void SendStateToWebView(string data)
        {
            if (webView1 != null && webView1.CoreWebView2 != null)
            {
                string encodedString = Encoder.Encoder.EncodeTo64(data);
                string script = $"recieveDataFromForm('{encodedString}');";
                await webView1.CoreWebView2.ExecuteScriptAsync(script);
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

        private enum Messages
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
            Form1.Dispose();
        }

    }
}
