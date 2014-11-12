namespace BABlackBelt
{
    partial class PrettyfyRule
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createStatementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIf = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExpression = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 76);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(637, 52);
            this.panel2.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(378, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save Rule";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(297, 17);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load Rule";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rule";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(92, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(198, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(637, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 76);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(637, 476);
            this.scintilla1.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintilla1.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintilla1.TabIndex = 3;
            this.scintilla1.TextChanged += new System.EventHandler(this.scintilla1_TextChanged);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStatementToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // createStatementToolStripMenuItem
            // 
            this.createStatementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExpression,
            this.mnuIf,
            this.mnuFor});
            this.createStatementToolStripMenuItem.Name = "createStatementToolStripMenuItem";
            this.createStatementToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.createStatementToolStripMenuItem.Text = "Create Statement";
            // 
            // mnuIf
            // 
            this.mnuIf.Name = "mnuIf";
            this.mnuIf.Size = new System.Drawing.Size(152, 22);
            this.mnuIf.Text = "If...";
            this.mnuIf.Click += new System.EventHandler(this.ifToolStripMenuItem_Click);
            // 
            // mnuFor
            // 
            this.mnuFor.Name = "mnuFor";
            this.mnuFor.Size = new System.Drawing.Size(152, 22);
            this.mnuFor.Text = "For...";
            // 
            // mnuExpression
            // 
            this.mnuExpression.Name = "mnuExpression";
            this.mnuExpression.Size = new System.Drawing.Size(152, 22);
            this.mnuExpression.Text = "Expression...";
            this.mnuExpression.Click += new System.EventHandler(this.mnuExpression_Click);
            // 
            // PrettyfyRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 552);
            this.Controls.Add(this.scintilla1);
            this.Controls.Add(this.panel1);
            this.Name = "PrettyfyRule";
            this.Text = "PrettyfyRule";
            this.Load += new System.EventHandler(this.PrettyfyRule_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createStatementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuIf;
        private System.Windows.Forms.ToolStripMenuItem mnuFor;
        private System.Windows.Forms.ToolStripMenuItem mnuExpression;
    }
}