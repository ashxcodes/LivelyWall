namespace LivelyWall
{
    partial class HomePage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem openMenuItem1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePage));
            this.webView1 = new Microsoft.Web.WebView2.WinForms.WebView2();
            openMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.webView1)).BeginInit();
            this.SuspendLayout();
            // 
            // openMenuItem1
            // 
            openMenuItem1.Name = "openMenuItem1";
            openMenuItem1.ShowShortcutKeys = false;
            openMenuItem1.Size = new System.Drawing.Size(180, 22);
            openMenuItem1.Text = "Open";
            openMenuItem1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // webView1
            // 
            this.webView1.AllowExternalDrop = true;
            this.webView1.BackColor = System.Drawing.SystemColors.GrayText;
            this.webView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.webView1.CreationProperties = null;
            this.webView1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView1.Location = new System.Drawing.Point(47, 25);
            this.webView1.Name = "webView1";
            this.webView1.Size = new System.Drawing.Size(699, 392);
            this.webView1.TabIndex = 0;
            this.webView1.ZoomFactor = 1D;
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HomePage";
            this.ShowInTaskbar = false;
            this.Text = "LivelyWall";
            this.TransparencyKey = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.webView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView1;
    }
}