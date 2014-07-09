namespace BABlackBelt
{
    partial class SelectObjects
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
            this.lstObjects = new System.Windows.Forms.CheckedListBox();
            this.btnAddElements = new System.Windows.Forms.Button();
            this.chkDependant = new System.Windows.Forms.CheckBox();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstObjects
            // 
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.Location = new System.Drawing.Point(23, 13);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(466, 154);
            this.lstObjects.TabIndex = 0;
            // 
            // btnAddElements
            // 
            this.btnAddElements.Location = new System.Drawing.Point(23, 173);
            this.btnAddElements.Name = "btnAddElements";
            this.btnAddElements.Size = new System.Drawing.Size(123, 23);
            this.btnAddElements.TabIndex = 1;
            this.btnAddElements.Text = "Add Elements";
            this.btnAddElements.UseVisualStyleBackColor = true;
            this.btnAddElements.Click += new System.EventHandler(this.btnAddElements_Click);
            // 
            // chkDependant
            // 
            this.chkDependant.AutoSize = true;
            this.chkDependant.Location = new System.Drawing.Point(153, 173);
            this.chkDependant.Name = "chkDependant";
            this.chkDependant.Size = new System.Drawing.Size(151, 17);
            this.chkDependant.TabIndex = 2;
            this.chkDependant.Text = "Select Dependant Objects";
            this.chkDependant.UseVisualStyleBackColor = true;
            this.chkDependant.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lstSelected
            // 
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.Location = new System.Drawing.Point(23, 203);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(466, 147);
            this.lstSelected.TabIndex = 3;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(383, 362);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(106, 23);
            this.btnDone.TabIndex = 4;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // SelectObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 397);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.chkDependant);
            this.Controls.Add(this.btnAddElements);
            this.Controls.Add(this.lstObjects);
            this.Name = "SelectObjects";
            this.Text = "SelectObjects";
            this.Load += new System.EventHandler(this.SelectObjects_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lstObjects;
        private System.Windows.Forms.Button btnAddElements;
        private System.Windows.Forms.CheckBox chkDependant;
        private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.Button btnDone;
    }
}