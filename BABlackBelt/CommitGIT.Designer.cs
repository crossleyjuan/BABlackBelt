namespace BABlackBelt
{
    partial class CommitGIT
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtCommit = new System.Windows.Forms.TextBox();
            this.lstChanges = new System.Windows.Forms.CheckedListBox();
            this.mnuFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDiffTool = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.mnuFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(643, 34);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(556, 469);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Commit";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCommit
            // 
            this.txtCommit.AcceptsReturn = true;
            this.txtCommit.AcceptsTab = true;
            this.txtCommit.Location = new System.Drawing.Point(16, 238);
            this.txtCommit.Multiline = true;
            this.txtCommit.Name = "txtCommit";
            this.txtCommit.Size = new System.Drawing.Size(615, 225);
            this.txtCommit.TabIndex = 4;
            this.txtCommit.Text = "# Commit Text";
            // 
            // lstChanges
            // 
            this.lstChanges.ContextMenuStrip = this.mnuFile;
            this.lstChanges.FormattingEnabled = true;
            this.lstChanges.Location = new System.Drawing.Point(16, 40);
            this.lstChanges.Name = "lstChanges";
            this.lstChanges.Size = new System.Drawing.Size(615, 184);
            this.lstChanges.TabIndex = 5;
            this.lstChanges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstChanges_KeyDown);
            // 
            // mnuFile
            // 
            this.mnuFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDiffTool});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(156, 26);
            // 
            // mnuDiffTool
            // 
            this.mnuDiffTool.Name = "mnuDiffTool";
            this.mnuDiffTool.Size = new System.Drawing.Size(155, 22);
            this.mnuDiffTool.Text = " Show Diff Tool";
            this.mnuDiffTool.Click += new System.EventHandler(this.mnuDiffTool_Click);
            // 
            // CommitGIT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 504);
            this.Controls.Add(this.lstChanges);
            this.Controls.Add(this.txtCommit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "CommitGIT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CommitGIT";
            this.Load += new System.EventHandler(this.CommitGIT_Load);
            this.panel1.ResumeLayout(false);
            this.mnuFile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCommit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox lstChanges;
        private System.Windows.Forms.ContextMenuStrip mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuDiffTool;
    }
}