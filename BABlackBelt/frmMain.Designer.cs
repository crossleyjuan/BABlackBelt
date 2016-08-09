namespace BABlackBelt
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testUDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commitRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prettifyMyRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testEmailServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateBizagiLogoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xLSTLabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGitFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionToolStripMenuItem,
            this.commandsToolStripMenuItem,
            this.gitToolStripMenuItem,
            this.codeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(687, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.testUDPToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.connectionToolStripMenuItem.Text = "Connection";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Visible = false;
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Visible = false;
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // testUDPToolStripMenuItem
            // 
            this.testUDPToolStripMenuItem.Name = "testUDPToolStripMenuItem";
            this.testUDPToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.testUDPToolStripMenuItem.Text = "Chat Window";
            this.testUDPToolStripMenuItem.Click += new System.EventHandler(this.testUDPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // commandsToolStripMenuItem
            // 
            this.commandsToolStripMenuItem.Name = "commandsToolStripMenuItem";
            this.commandsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.commandsToolStripMenuItem.Text = "&Commands";
            // 
            // gitToolStripMenuItem
            // 
            this.gitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commitRulesToolStripMenuItem});
            this.gitToolStripMenuItem.Name = "gitToolStripMenuItem";
            this.gitToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.gitToolStripMenuItem.Text = "Git";
            // 
            // commitRulesToolStripMenuItem
            // 
            this.commitRulesToolStripMenuItem.Name = "commitRulesToolStripMenuItem";
            this.commitRulesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.commitRulesToolStripMenuItem.Text = "Commit Elements";
            this.commitRulesToolStripMenuItem.Click += new System.EventHandler(this.commitRulesToolStripMenuItem_Click);
            // 
            // codeToolStripMenuItem
            // 
            this.codeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prettifyMyRuleToolStripMenuItem,
            this.dBInfoToolStripMenuItem,
            this.testEmailServerToolStripMenuItem,
            this.advancedCompareToolStripMenuItem,
            this.updateBizagiLogoToolStripMenuItem,
            this.xLSTLabToolStripMenuItem});
            this.codeToolStripMenuItem.Name = "codeToolStripMenuItem";
            this.codeToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.codeToolStripMenuItem.Text = "Code";
            // 
            // prettifyMyRuleToolStripMenuItem
            // 
            this.prettifyMyRuleToolStripMenuItem.Name = "prettifyMyRuleToolStripMenuItem";
            this.prettifyMyRuleToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.prettifyMyRuleToolStripMenuItem.Text = "Prettify my rule";
            this.prettifyMyRuleToolStripMenuItem.Click += new System.EventHandler(this.prettifyMyRuleToolStripMenuItem_Click);
            // 
            // dBInfoToolStripMenuItem
            // 
            this.dBInfoToolStripMenuItem.Name = "dBInfoToolStripMenuItem";
            this.dBInfoToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.dBInfoToolStripMenuItem.Text = "DB Info";
            this.dBInfoToolStripMenuItem.Visible = false;
            this.dBInfoToolStripMenuItem.Click += new System.EventHandler(this.dBInfoToolStripMenuItem_Click);
            // 
            // testEmailServerToolStripMenuItem
            // 
            this.testEmailServerToolStripMenuItem.Name = "testEmailServerToolStripMenuItem";
            this.testEmailServerToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.testEmailServerToolStripMenuItem.Text = "Test Email Server";
            this.testEmailServerToolStripMenuItem.Visible = false;
            this.testEmailServerToolStripMenuItem.Click += new System.EventHandler(this.testEmailServerToolStripMenuItem_Click);
            // 
            // advancedCompareToolStripMenuItem
            // 
            this.advancedCompareToolStripMenuItem.Name = "advancedCompareToolStripMenuItem";
            this.advancedCompareToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.advancedCompareToolStripMenuItem.Text = "Advanced Compare";
            this.advancedCompareToolStripMenuItem.Visible = false;
            this.advancedCompareToolStripMenuItem.Click += new System.EventHandler(this.advancedCompareToolStripMenuItem_Click);
            // 
            // updateBizagiLogoToolStripMenuItem
            // 
            this.updateBizagiLogoToolStripMenuItem.Name = "updateBizagiLogoToolStripMenuItem";
            this.updateBizagiLogoToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.updateBizagiLogoToolStripMenuItem.Text = "Update Bizagi Logo";
            this.updateBizagiLogoToolStripMenuItem.Visible = false;
            this.updateBizagiLogoToolStripMenuItem.Click += new System.EventHandler(this.updateBizagiLogoToolStripMenuItem_Click);
            // 
            // xLSTLabToolStripMenuItem
            // 
            this.xLSTLabToolStripMenuItem.Name = "xLSTLabToolStripMenuItem";
            this.xLSTLabToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.xLSTLabToolStripMenuItem.Text = "XLST Lab";
            this.xLSTLabToolStripMenuItem.Click += new System.EventHandler(this.xLSTLabToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Git Repository";
            // 
            // txtGitFolder
            // 
            this.txtGitFolder.Location = new System.Drawing.Point(168, 94);
            this.txtGitFolder.Name = "txtGitFolder";
            this.txtGitFolder.ReadOnly = true;
            this.txtGitFolder.Size = new System.Drawing.Size(461, 20);
            this.txtGitFolder.TabIndex = 2;
            this.txtGitFolder.TextChanged += new System.EventHandler(this.txtGitFolder_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(267, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Load Project";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(47, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Clone Project";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(155, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(106, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Create Project";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 177);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtGitFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bizagi BlackBelt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prettifyMyRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dBInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commitRulesToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGitFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testEmailServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedCompareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateBizagiLogoToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem xLSTLabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testUDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandsToolStripMenuItem;
    }
}

