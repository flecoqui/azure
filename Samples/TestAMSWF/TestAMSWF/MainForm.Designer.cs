namespace TestAMSWF
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxAccountKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAccountName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.pictureBoxJob = new System.Windows.Forms.PictureBox();
            this.listInputAssets = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listInputFiles = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAddAsset = new System.Windows.Forms.Button();
            this.buttonRemoveAsset = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listOutputFiles = new System.Windows.Forms.ListBox();
            this.listOutputAssets = new System.Windows.Forms.ListBox();
            this.buttonGenerateSubtitle = new System.Windows.Forms.Button();
            this.buttonDonwloadSubtitle = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxLanguages = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonOpenSubtitle = new System.Windows.Forms.Button();
            this.buttonDisplayJobs = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJob)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxAccountKey
            // 
            this.textBoxAccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAccountKey.Location = new System.Drawing.Point(15, 74);
            this.textBoxAccountKey.Name = "textBoxAccountKey";
            this.textBoxAccountKey.Size = new System.Drawing.Size(352, 20);
            this.textBoxAccountKey.TabIndex = 34;
            this.textBoxAccountKey.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Media Service Account Key";
            // 
            // textBoxAccountName
            // 
            this.textBoxAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAccountName.Location = new System.Drawing.Point(15, 35);
            this.textBoxAccountName.Name = "textBoxAccountName";
            this.textBoxAccountName.Size = new System.Drawing.Size(352, 20);
            this.textBoxAccountName.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Media Service Account Name";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonLogin.Location = new System.Drawing.Point(373, 31);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(105, 27);
            this.buttonLogin.TabIndex = 37;
            this.buttonLogin.Text = "Connect";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // pictureBoxJob
            // 
            this.pictureBoxJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxJob.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxJob.ErrorImage")));
            this.pictureBoxJob.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxJob.Image")));
            this.pictureBoxJob.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxJob.InitialImage")));
            this.pictureBoxJob.Location = new System.Drawing.Point(647, 5);
            this.pictureBoxJob.Name = "pictureBoxJob";
            this.pictureBoxJob.Size = new System.Drawing.Size(96, 96);
            this.pictureBoxJob.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxJob.TabIndex = 43;
            this.pictureBoxJob.TabStop = false;
            // 
            // listInputAssets
            // 
            this.listInputAssets.FormattingEnabled = true;
            this.listInputAssets.HorizontalScrollbar = true;
            this.listInputAssets.Location = new System.Drawing.Point(15, 188);
            this.listInputAssets.Name = "listInputAssets";
            this.listInputAssets.Size = new System.Drawing.Size(352, 121);
            this.listInputAssets.TabIndex = 44;
            this.listInputAssets.SelectedIndexChanged += new System.EventHandler(this.listAssets_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "List of Input Assets:";
            // 
            // listInputFiles
            // 
            this.listInputFiles.FormattingEnabled = true;
            this.listInputFiles.HorizontalScrollbar = true;
            this.listInputFiles.Location = new System.Drawing.Point(15, 330);
            this.listInputFiles.Name = "listInputFiles";
            this.listInputFiles.Size = new System.Drawing.Size(352, 121);
            this.listInputFiles.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(392, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "List of Ouput Assets:";
            // 
            // buttonAddAsset
            // 
            this.buttonAddAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonAddAsset.Location = new System.Drawing.Point(15, 104);
            this.buttonAddAsset.Name = "buttonAddAsset";
            this.buttonAddAsset.Size = new System.Drawing.Size(76, 27);
            this.buttonAddAsset.TabIndex = 48;
            this.buttonAddAsset.Text = "Add Asset";
            this.buttonAddAsset.UseVisualStyleBackColor = true;
            this.buttonAddAsset.Click += new System.EventHandler(this.buttonAddAsset_Click);
            // 
            // buttonRemoveAsset
            // 
            this.buttonRemoveAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonRemoveAsset.Location = new System.Drawing.Point(97, 104);
            this.buttonRemoveAsset.Name = "buttonRemoveAsset";
            this.buttonRemoveAsset.Size = new System.Drawing.Size(89, 27);
            this.buttonRemoveAsset.TabIndex = 49;
            this.buttonRemoveAsset.Text = "Remove Asset";
            this.buttonRemoveAsset.UseVisualStyleBackColor = true;
            this.buttonRemoveAsset.Click += new System.EventHandler(this.buttonRemoveAsset_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 314);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "List of Input Files:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(391, 314);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "List of Output Files:";
            // 
            // listOutputFiles
            // 
            this.listOutputFiles.FormattingEnabled = true;
            this.listOutputFiles.HorizontalScrollbar = true;
            this.listOutputFiles.Location = new System.Drawing.Point(394, 330);
            this.listOutputFiles.Name = "listOutputFiles";
            this.listOutputFiles.Size = new System.Drawing.Size(352, 121);
            this.listOutputFiles.TabIndex = 52;
            // 
            // listOutputAssets
            // 
            this.listOutputAssets.FormattingEnabled = true;
            this.listOutputAssets.HorizontalScrollbar = true;
            this.listOutputAssets.Location = new System.Drawing.Point(394, 188);
            this.listOutputAssets.Name = "listOutputAssets";
            this.listOutputAssets.Size = new System.Drawing.Size(352, 82);
            this.listOutputAssets.TabIndex = 51;
            this.listOutputAssets.SelectedIndexChanged += new System.EventHandler(this.listOutputAssets_SelectedIndexChanged);
            // 
            // buttonGenerateSubtitle
            // 
            this.buttonGenerateSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerateSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonGenerateSubtitle.Location = new System.Drawing.Point(15, 137);
            this.buttonGenerateSubtitle.Name = "buttonGenerateSubtitle";
            this.buttonGenerateSubtitle.Size = new System.Drawing.Size(105, 27);
            this.buttonGenerateSubtitle.TabIndex = 54;
            this.buttonGenerateSubtitle.Text = "Generate Subtitle";
            this.buttonGenerateSubtitle.UseVisualStyleBackColor = true;
            this.buttonGenerateSubtitle.Click += new System.EventHandler(this.buttonGenerateSubtitle_Click);
            // 
            // buttonDonwloadSubtitle
            // 
            this.buttonDonwloadSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDonwloadSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonDonwloadSubtitle.Location = new System.Drawing.Point(518, 287);
            this.buttonDonwloadSubtitle.Name = "buttonDonwloadSubtitle";
            this.buttonDonwloadSubtitle.Size = new System.Drawing.Size(116, 27);
            this.buttonDonwloadSubtitle.TabIndex = 55;
            this.buttonDonwloadSubtitle.Text = "Download Subtitle";
            this.buttonDonwloadSubtitle.UseVisualStyleBackColor = true;
            this.buttonDonwloadSubtitle.Click += new System.EventHandler(this.buttonPlaySubtitle_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(15, 469);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(727, 119);
            this.richTextBoxLog.TabIndex = 56;
            this.richTextBoxLog.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 453);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 57;
            this.label7.Text = "Logs:";
            // 
            // comboBoxLanguages
            // 
            this.comboBoxLanguages.FormattingEnabled = true;
            this.comboBoxLanguages.Location = new System.Drawing.Point(189, 141);
            this.comboBoxLanguages.Name = "comboBoxLanguages";
            this.comboBoxLanguages.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLanguages.TabIndex = 58;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(127, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 59;
            this.label8.Text = "Language:";
            // 
            // buttonOpenSubtitle
            // 
            this.buttonOpenSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonOpenSubtitle.Location = new System.Drawing.Point(394, 287);
            this.buttonOpenSubtitle.Name = "buttonOpenSubtitle";
            this.buttonOpenSubtitle.Size = new System.Drawing.Size(116, 27);
            this.buttonOpenSubtitle.TabIndex = 61;
            this.buttonOpenSubtitle.Text = "Open Subtitle";
            this.buttonOpenSubtitle.UseVisualStyleBackColor = true;
            this.buttonOpenSubtitle.Click += new System.EventHandler(this.buttonOpenSubtitle_Click);
            // 
            // buttonDisplayJobs
            // 
            this.buttonDisplayJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDisplayJobs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonDisplayJobs.Location = new System.Drawing.Point(192, 104);
            this.buttonDisplayJobs.Name = "buttonDisplayJobs";
            this.buttonDisplayJobs.Size = new System.Drawing.Size(89, 27);
            this.buttonDisplayJobs.TabIndex = 62;
            this.buttonDisplayJobs.Text = "Display Jobs";
            this.buttonDisplayJobs.UseVisualStyleBackColor = true;
            this.buttonDisplayJobs.Click += new System.EventHandler(this.buttonDisplayJobs_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 600);
            this.Controls.Add(this.buttonDisplayJobs);
            this.Controls.Add(this.buttonOpenSubtitle);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBoxLanguages);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonDonwloadSubtitle);
            this.Controls.Add(this.buttonGenerateSubtitle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listOutputFiles);
            this.Controls.Add(this.listOutputAssets);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonRemoveAsset);
            this.Controls.Add(this.buttonAddAsset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listInputFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listInputAssets);
            this.Controls.Add(this.pictureBoxJob);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.textBoxAccountKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxAccountName);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Test Azure Media Services indexer and Azure Search";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJob)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAccountKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAccountName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.PictureBox pictureBoxJob;
        private System.Windows.Forms.ListBox listInputAssets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listInputFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAddAsset;
        private System.Windows.Forms.Button buttonRemoveAsset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listOutputFiles;
        private System.Windows.Forms.ListBox listOutputAssets;
        private System.Windows.Forms.Button buttonGenerateSubtitle;
        private System.Windows.Forms.Button buttonDonwloadSubtitle;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxLanguages;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonOpenSubtitle;
        private System.Windows.Forms.Button buttonDisplayJobs;
    }
}

