namespace OApis
{
    partial class AuthorizationDialog
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
            this.authWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // authWebBrowser
            // 
            this.authWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.authWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.authWebBrowser.Name = "authWebBrowser";
            this.authWebBrowser.Size = new System.Drawing.Size(548, 515);
            this.authWebBrowser.TabIndex = 0;
            // 
            // AuthorizationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 515);
            this.Controls.Add(this.authWebBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AuthorizationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AuthorizationDialog🔰";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser authWebBrowser;
    }
}