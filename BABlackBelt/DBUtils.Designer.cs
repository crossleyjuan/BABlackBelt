namespace BABlackBelt
{
    partial class DBUtils
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
            this.btnExportDBInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExportDBInfo
            // 
            this.btnExportDBInfo.Location = new System.Drawing.Point(31, 31);
            this.btnExportDBInfo.Name = "btnExportDBInfo";
            this.btnExportDBInfo.Size = new System.Drawing.Size(158, 23);
            this.btnExportDBInfo.TabIndex = 0;
            this.btnExportDBInfo.Text = "Export DB Information";
            this.btnExportDBInfo.UseVisualStyleBackColor = true;
            this.btnExportDBInfo.Click += new System.EventHandler(this.button1_Click);
            // 
            // DBUtils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnExportDBInfo);
            this.Name = "DBUtils";
            this.Text = "DBUtils";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExportDBInfo;
    }
}