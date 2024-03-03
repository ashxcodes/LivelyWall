using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;


namespace LivelyWall
{
    public partial class HomePage : Form
    {
        private readonly OpenFileDialog openFileDialog = new OpenFileDialog();
        private string filepath;

        public HomePage()
        {
            InitializeComponent();
            InitializeWebView();
            InitializeComponentFormProperties();
            InitializeDragArea();
        }
        
        private async void InitializeWebView()
        {
            await webView1.EnsureCoreWebView2Async();
            webView1.CoreWebView2.Navigate("C:\\Users\\Abdullah\\source\\repos\\LivelyWall\\FrontEnd\\index.html");
            webView1.WebMessageReceived += WebView_ScriptNotify;
        }
        
        private void InitializeComponentFormProperties()
        {
            this.ShowInTaskbar = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }
        
        private void InitializeDragArea()
        {
            this.AllowDrop = true;
            this.DragEnter += DragArea_DragEnter;
        }
        
        private void WebView_ScriptNotify(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (int.TryParse(e.WebMessageAsJson, out int message))
            {
                // Check for the message sent from JavaScript
                if (e.Source != webView1.CoreWebView2.Source)
                {
                    return;
                }

                switch (message)
                {
                    case (int)Messages.SelectBtnClick:
                        openFileDialog.Filter = "Video files|*.mp4;*.avi;*.mkv;*.mov|All files (*.*)|*.*";
                        openFileDialog.CheckFileExists = true;
                        openFileDialog.ShowDialog();
                        this.filepath = openFileDialog.FileName;
                        SendFileNameToWebView(this.filepath);
                        break;

                    case (int)Messages.SetBtnClick:
                        if (filepath == null || filepath == "")
                        {
                            MessageBox.Show("File Not found");
                            return;
                        }
                        Form1 form = new Form1(filepath);
                        form.Show();
                        break;

                    default:
                        return;
                }
            }
        }

        private async void SendFileNameToWebView(string data)
        {
            if (webView1 != null && webView1.CoreWebView2 != null)
            {
                string script = $"recieveFileNameFromForm('{data}');";
                await webView1.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        private void DragArea_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the data format is FileDrop
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.filepath = files[0];
                return;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
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

    }
    enum Messages
    {
        SelectBtnClick = 1,
        SetBtnClick= 2
    }
}
