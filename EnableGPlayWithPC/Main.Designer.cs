namespace EnableGPlayWithPC
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Label_Vending = new System.Windows.Forms.Label();
            this.Label_GMS = new System.Windows.Forms.Label();
            this.Label_GSF = new System.Windows.Forms.Label();
            this.Label_GSFLogin = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_Process = new System.Windows.Forms.Button();
            this.LinkLabel_Repo = new System.Windows.Forms.LinkLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.InfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.FileSelector_GSFLogin = new EnableGPlayWithPC.FileSelector();
            this.FileSelector_GSF = new EnableGPlayWithPC.FileSelector();
            this.FileSelector_GMS = new EnableGPlayWithPC.FileSelector();
            this.FileSelector_Vending = new EnableGPlayWithPC.FileSelector();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label_Vending
            // 
            resources.ApplyResources(this.Label_Vending, "Label_Vending");
            this.Label_Vending.Name = "Label_Vending";
            // 
            // Label_GMS
            // 
            resources.ApplyResources(this.Label_GMS, "Label_GMS");
            this.Label_GMS.Name = "Label_GMS";
            // 
            // Label_GSF
            // 
            resources.ApplyResources(this.Label_GSF, "Label_GSF");
            this.Label_GSF.Name = "Label_GSF";
            // 
            // Label_GSFLogin
            // 
            resources.ApplyResources(this.Label_GSFLogin, "Label_GSFLogin");
            this.Label_GSFLogin.Name = "Label_GSFLogin";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Button_Process
            // 
            this.Button_Process.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Button_Process.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            resources.ApplyResources(this.Button_Process, "Button_Process");
            this.Button_Process.Name = "Button_Process";
            this.Button_Process.UseVisualStyleBackColor = false;
            this.Button_Process.Click += new System.EventHandler(this.Button_Process_Click);
            // 
            // LinkLabel_Repo
            // 
            resources.ApplyResources(this.LinkLabel_Repo, "LinkLabel_Repo");
            this.LinkLabel_Repo.Name = "LinkLabel_Repo";
            this.LinkLabel_Repo.TabStop = true;
            this.LinkLabel_Repo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_Repo_LinkClicked);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InfoToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // 情報XToolStripMenuItem
            // 
            this.InfoToolStripMenuItem.Name = "情報XToolStripMenuItem";
            resources.ApplyResources(this.InfoToolStripMenuItem, "情報XToolStripMenuItem");
            this.InfoToolStripMenuItem.Click += new System.EventHandler(this.InfoToolStripMenuItem_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // FileSelector_GSFLogin
            // 
            this.FileSelector_GSFLogin.AllowDrop = true;
            resources.ApplyResources(this.FileSelector_GSFLogin, "FileSelector_GSFLogin");
            this.FileSelector_GSFLogin.Name = "FileSelector_GSFLogin";
            // 
            // FileSelector_GSF
            // 
            this.FileSelector_GSF.AllowDrop = true;
            resources.ApplyResources(this.FileSelector_GSF, "FileSelector_GSF");
            this.FileSelector_GSF.Name = "FileSelector_GSF";
            // 
            // FileSelector_GMS
            // 
            this.FileSelector_GMS.AllowDrop = true;
            resources.ApplyResources(this.FileSelector_GMS, "FileSelector_GMS");
            this.FileSelector_GMS.Name = "FileSelector_GMS";
            // 
            // FileSelector_Vending
            // 
            this.FileSelector_Vending.AllowDrop = true;
            this.FileSelector_Vending.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.FileSelector_Vending, "FileSelector_Vending");
            this.FileSelector_Vending.Name = "FileSelector_Vending";
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LinkLabel_Repo);
            this.Controls.Add(this.Button_Process);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileSelector_GSFLogin);
            this.Controls.Add(this.Label_GSFLogin);
            this.Controls.Add(this.Label_GSF);
            this.Controls.Add(this.FileSelector_GSF);
            this.Controls.Add(this.FileSelector_GMS);
            this.Controls.Add(this.FileSelector_Vending);
            this.Controls.Add(this.Label_GMS);
            this.Controls.Add(this.Label_Vending);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Label_Vending;
        private System.Windows.Forms.Label Label_GMS;
        private FileSelector FileSelector_Vending;
        private FileSelector FileSelector_GMS;
        private FileSelector FileSelector_GSF;
        private System.Windows.Forms.Label Label_GSF;
        private System.Windows.Forms.Label Label_GSFLogin;
        private FileSelector FileSelector_GSFLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_Process;
        private System.Windows.Forms.LinkLabel LinkLabel_Repo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem InfoToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}

