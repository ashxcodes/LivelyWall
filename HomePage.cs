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
        private bool dataSent;
        public HomePage()
        {
            InitializeComponent();
            InitWebView();
            InitializeComponentFormProperties();
        }
        private async void InitWebView()
        {
            await webView1.EnsureCoreWebView2Async();
            webView1.CoreWebView2.Navigate("C:\\Users\\Abdullah\\source\\repos\\LivelyWall\\FrontEnd\\index.html");
            webView1.WebMessageReceived += WebView_ScriptNotify;
        }
        private void InitializeComponentFormProperties()
        {
            this.ShowInTaskbar = false;
            this.MaximizeBox = false; 
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
                        SendFileNameToWebView(openFileDialog.FileName);
                        break;

                    case (int)Messages.SetBtnClick:
                        Form1 form = new Form1(openFileDialog.FileName);
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
    }
    enum Messages
    {
        SelectBtnClick = 1,
        SetBtnClick= 2
    }
}
