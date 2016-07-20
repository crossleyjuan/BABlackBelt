namespace BABlackBelt
{
    partial class ProjectConfiguration
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtGitFolder = new System.Windows.Forms.TextBox();
            this.txtProjectFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseGit = new System.Windows.Forms.Button();
            this.btnBrowseProject = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Git Folder";
            // 
            // txtGitFolder
            // 
            this.txtGitFolder.Location = new System.Drawing.Point(161, 42);
            this.txtGitFolder.Name = "txtGitFolder";
            this.txtGitFolder.Size = new System.Drawing.Size(343, 20);
            this.txtGitFolder.TabIndex = 1;
            this.txtGitFolder.TextChanged += new System.EventHandler(this.txtGitFolder_TextChanged);
            // 
            // txtProjectFolder
            // 
            this.txtProjectFolder.Location = new System.Drawing.Point(161, 77);
            this.txtProjectFolder.Name = "txtProjectFolder";
            this.txtProjectFolder.Size = new System.Drawing.Size(343, 20);
            this.txtProjectFolder.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project Folder";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(161, 112);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(343, 20);
            this.txtConnectionString.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Connection String";
            // 
            // btnBrowseGit
            // 
            this.btnBrowseGit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBrowseGit.Location = new System.Drawing.Point(510, 42);
            this.btnBrowseGit.Name = "btnBrowseGit";
            this.btnBrowseGit.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseGit.TabIndex = 6;
            this.btnBrowseGit.Text = "...";
            this.btnBrowseGit.UseVisualStyleBackColor = true;
            this.btnBrowseGit.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnBrowseProject
            // 
            this.btnBrowseProject.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBrowseProject.Location = new System.Drawing.Point(510, 77);
            this.btnBrowseProject.Name = "btnBrowseProject";
            this.btnBrowseProject.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseProject.TabIndex = 7;
            this.btnBrowseProject.Text = "...";
            this.btnBrowseProject.UseVisualStyleBackColor = true;
            this.btnBrowseProject.Click += new System.EventHandler(this.btnBrowseProject_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(419, 165);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(97, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(522, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(97, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ProjectConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 200);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnBrowseProject);
            this.Controls.Add(this.btnBrowseGit);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtProjectFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGitFolder);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProjectConfiguration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGitFolder;
        private System.Windows.Forms.TextBox txtProjectFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseGit;
        private System.Windows.Forms.Button btnBrowseProject;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}