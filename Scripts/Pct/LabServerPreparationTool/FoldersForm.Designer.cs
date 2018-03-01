namespace LabServerPreparationTool
{
    partial class FoldersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FoldersForm));
            this.FoldersSaveButton = new System.Windows.Forms.Button();
            this.SoftwareDirTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DataDirTextbox = new System.Windows.Forms.TextBox();
            this.DatabaseFolderTextbox = new System.Windows.Forms.TextBox();
            this.TempBDFolderTextBox = new System.Windows.Forms.TextBox();
            this.TransactionLogFolderTextbox = new System.Windows.Forms.TextBox();
            this.OLTPFolderTextbox = new System.Windows.Forms.TextBox();
            this.SRFolderTextbox = new System.Windows.Forms.TextBox();
            this.WindowsTempFolderTextbox = new System.Windows.Forms.TextBox();
            this.MediaFolderTextbox = new System.Windows.Forms.TextBox();
            this.SQL2008FolderTextbox = new System.Windows.Forms.TextBox();
            this.guidelineLabel = new System.Windows.Forms.Label();
            this.DefaultButton = new System.Windows.Forms.Button();
            this.CancelFolderButton = new System.Windows.Forms.Button();
            this.SQL2012FolderTextbox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SQLRootFolderTextbox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.SpeechDatadirTextbox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.Media2FolderTextbox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SQL2014FolderTextbox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FoldersSaveButton
            // 
            this.FoldersSaveButton.Location = new System.Drawing.Point(139, 564);
            this.FoldersSaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.FoldersSaveButton.Name = "FoldersSaveButton";
            this.FoldersSaveButton.Size = new System.Drawing.Size(100, 36);
            this.FoldersSaveButton.TabIndex = 20;
            this.FoldersSaveButton.Text = "Save";
            this.FoldersSaveButton.UseVisualStyleBackColor = true;
            this.FoldersSaveButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // SoftwareDirTextbox
            // 
            this.SoftwareDirTextbox.Location = new System.Drawing.Point(201, 66);
            this.SoftwareDirTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SoftwareDirTextbox.Name = "SoftwareDirTextbox";
            this.SoftwareDirTextbox.Size = new System.Drawing.Size(373, 22);
            this.SoftwareDirTextbox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Software Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Logs and Data Directory";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 134);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Database Directory";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 230);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Temp DB Directory";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 166);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Transaction Log Directory";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 198);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Contact OLTP DB";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 295);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "SR Folder";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 327);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(148, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Windows Temp Folder";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 359);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(176, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "Import Manager Call Buffer";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 457);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "SQL 2008 Folder";
            // 
            // DataDirTextbox
            // 
            this.DataDirTextbox.Location = new System.Drawing.Point(201, 98);
            this.DataDirTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.DataDirTextbox.Name = "DataDirTextbox";
            this.DataDirTextbox.Size = new System.Drawing.Size(373, 22);
            this.DataDirTextbox.TabIndex = 2;
            // 
            // DatabaseFolderTextbox
            // 
            this.DatabaseFolderTextbox.Location = new System.Drawing.Point(201, 130);
            this.DatabaseFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.DatabaseFolderTextbox.Name = "DatabaseFolderTextbox";
            this.DatabaseFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.DatabaseFolderTextbox.TabIndex = 3;
            // 
            // TempBDFolderTextBox
            // 
            this.TempBDFolderTextBox.Location = new System.Drawing.Point(201, 226);
            this.TempBDFolderTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.TempBDFolderTextBox.Name = "TempBDFolderTextBox";
            this.TempBDFolderTextBox.Size = new System.Drawing.Size(373, 22);
            this.TempBDFolderTextBox.TabIndex = 6;
            // 
            // TransactionLogFolderTextbox
            // 
            this.TransactionLogFolderTextbox.Location = new System.Drawing.Point(201, 162);
            this.TransactionLogFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.TransactionLogFolderTextbox.Name = "TransactionLogFolderTextbox";
            this.TransactionLogFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.TransactionLogFolderTextbox.TabIndex = 4;
            // 
            // OLTPFolderTextbox
            // 
            this.OLTPFolderTextbox.Location = new System.Drawing.Point(201, 194);
            this.OLTPFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.OLTPFolderTextbox.Name = "OLTPFolderTextbox";
            this.OLTPFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.OLTPFolderTextbox.TabIndex = 5;
            // 
            // SRFolderTextbox
            // 
            this.SRFolderTextbox.Location = new System.Drawing.Point(201, 292);
            this.SRFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SRFolderTextbox.Name = "SRFolderTextbox";
            this.SRFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.SRFolderTextbox.TabIndex = 8;
            // 
            // WindowsTempFolderTextbox
            // 
            this.WindowsTempFolderTextbox.Location = new System.Drawing.Point(201, 324);
            this.WindowsTempFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.WindowsTempFolderTextbox.Name = "WindowsTempFolderTextbox";
            this.WindowsTempFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.WindowsTempFolderTextbox.TabIndex = 9;
            // 
            // MediaFolderTextbox
            // 
            this.MediaFolderTextbox.Location = new System.Drawing.Point(201, 356);
            this.MediaFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.MediaFolderTextbox.Name = "MediaFolderTextbox";
            this.MediaFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.MediaFolderTextbox.TabIndex = 10;
            // 
            // SQL2008FolderTextbox
            // 
            this.SQL2008FolderTextbox.Location = new System.Drawing.Point(201, 453);
            this.SQL2008FolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SQL2008FolderTextbox.Name = "SQL2008FolderTextbox";
            this.SQL2008FolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.SQL2008FolderTextbox.TabIndex = 13;
            // 
            // guidelineLabel
            // 
            this.guidelineLabel.AutoSize = true;
            this.guidelineLabel.ForeColor = System.Drawing.Color.Blue;
            this.guidelineLabel.Location = new System.Drawing.Point(20, 28);
            this.guidelineLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.guidelineLabel.Name = "guidelineLabel";
            this.guidelineLabel.Size = new System.Drawing.Size(345, 17);
            this.guidelineLabel.TabIndex = 0;
            this.guidelineLabel.Text = "If the folder is not relevant for the server, set it as NA.";
            // 
            // DefaultButton
            // 
            this.DefaultButton.Location = new System.Drawing.Point(359, 564);
            this.DefaultButton.Margin = new System.Windows.Forms.Padding(4);
            this.DefaultButton.Name = "DefaultButton";
            this.DefaultButton.Size = new System.Drawing.Size(100, 36);
            this.DefaultButton.TabIndex = 23;
            this.DefaultButton.Text = "Set Defaults";
            this.DefaultButton.UseVisualStyleBackColor = true;
            this.DefaultButton.Click += new System.EventHandler(this.DefaultButton_Click);
            // 
            // CancelFolderButton
            // 
            this.CancelFolderButton.Location = new System.Drawing.Point(249, 564);
            this.CancelFolderButton.Margin = new System.Windows.Forms.Padding(4);
            this.CancelFolderButton.Name = "CancelFolderButton";
            this.CancelFolderButton.Size = new System.Drawing.Size(100, 36);
            this.CancelFolderButton.TabIndex = 22;
            this.CancelFolderButton.Text = "Cancel";
            this.CancelFolderButton.UseVisualStyleBackColor = true;
            this.CancelFolderButton.Click += new System.EventHandler(this.CancelFolderButton_Click);
            // 
            // SQL2012FolderTextbox
            // 
            this.SQL2012FolderTextbox.Location = new System.Drawing.Point(201, 485);
            this.SQL2012FolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SQL2012FolderTextbox.Name = "SQL2012FolderTextbox";
            this.SQL2012FolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.SQL2012FolderTextbox.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 489);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "SQL 2012 Folder";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(20, 9);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(313, 17);
            this.label13.TabIndex = 29;
            this.label13.Text = "Provide the folder names that will be used in SR.";
            // 
            // SQLRootFolderTextbox
            // 
            this.SQLRootFolderTextbox.Location = new System.Drawing.Point(201, 421);
            this.SQLRootFolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SQLRootFolderTextbox.Name = "SQLRootFolderTextbox";
            this.SQLRootFolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.SQLRootFolderTextbox.TabIndex = 12;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 425);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 17);
            this.label14.TabIndex = 30;
            this.label14.Text = "SQL Root Folder";
            // 
            // SpeechDatadirTextbox
            // 
            this.SpeechDatadirTextbox.Location = new System.Drawing.Point(201, 258);
            this.SpeechDatadirTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SpeechDatadirTextbox.Name = "SpeechDatadirTextbox";
            this.SpeechDatadirTextbox.Size = new System.Drawing.Size(373, 22);
            this.SpeechDatadirTextbox.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(21, 262);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 17);
            this.label15.TabIndex = 31;
            this.label15.Text = "Speech Data";
            // 
            // Media2FolderTextbox
            // 
            this.Media2FolderTextbox.Location = new System.Drawing.Point(201, 389);
            this.Media2FolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.Media2FolderTextbox.Name = "Media2FolderTextbox";
            this.Media2FolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.Media2FolderTextbox.TabIndex = 11;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 393);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(176, 17);
            this.label16.TabIndex = 32;
            this.label16.Text = "CF Verbatim Folder (DWH)";
            // 
            // SQL2014FolderTextbox
            // 
            this.SQL2014FolderTextbox.Location = new System.Drawing.Point(201, 516);
            this.SQL2014FolderTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.SQL2014FolderTextbox.Name = "SQL2014FolderTextbox";
            this.SQL2014FolderTextbox.Size = new System.Drawing.Size(373, 22);
            this.SQL2014FolderTextbox.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 519);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 17);
            this.label12.TabIndex = 34;
            this.label12.Text = "SQL 2014 Folder";
            // 
            // FoldersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 650);
            this.ControlBox = false;
            this.Controls.Add(this.SQL2014FolderTextbox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Media2FolderTextbox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.SpeechDatadirTextbox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.SQLRootFolderTextbox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.SQL2012FolderTextbox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.CancelFolderButton);
            this.Controls.Add(this.DefaultButton);
            this.Controls.Add(this.guidelineLabel);
            this.Controls.Add(this.SQL2008FolderTextbox);
            this.Controls.Add(this.MediaFolderTextbox);
            this.Controls.Add(this.WindowsTempFolderTextbox);
            this.Controls.Add(this.SRFolderTextbox);
            this.Controls.Add(this.OLTPFolderTextbox);
            this.Controls.Add(this.TransactionLogFolderTextbox);
            this.Controls.Add(this.TempBDFolderTextBox);
            this.Controls.Add(this.DatabaseFolderTextbox);
            this.Controls.Add(this.DataDirTextbox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SoftwareDirTextbox);
            this.Controls.Add(this.FoldersSaveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FoldersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Folders";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FoldersSaveButton;
        private System.Windows.Forms.TextBox SoftwareDirTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox DataDirTextbox;
        private System.Windows.Forms.TextBox DatabaseFolderTextbox;
        private System.Windows.Forms.TextBox TempBDFolderTextBox;
        private System.Windows.Forms.TextBox TransactionLogFolderTextbox;
        private System.Windows.Forms.TextBox OLTPFolderTextbox;
        private System.Windows.Forms.TextBox SRFolderTextbox;
        private System.Windows.Forms.TextBox WindowsTempFolderTextbox;
        private System.Windows.Forms.TextBox MediaFolderTextbox;
        private System.Windows.Forms.TextBox SQL2008FolderTextbox;
        private System.Windows.Forms.Label guidelineLabel;
        private System.Windows.Forms.Button DefaultButton;
        private System.Windows.Forms.Button CancelFolderButton;
        private System.Windows.Forms.TextBox SQL2012FolderTextbox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox SQLRootFolderTextbox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox SpeechDatadirTextbox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox Media2FolderTextbox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox SQL2014FolderTextbox;
        private System.Windows.Forms.Label label12;
    }
}