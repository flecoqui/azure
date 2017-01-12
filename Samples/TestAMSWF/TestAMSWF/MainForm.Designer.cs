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
            this.textBoxSearchAccountName = new System.Windows.Forms.TextBox();
            this.textBoxSearchAccountKey = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonCreateIndex = new System.Windows.Forms.Button();
            this.buttonDeleteIndex = new System.Windows.Forms.Button();
            this.buttonPopulateIndex = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonPlaySubtitle = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxStorageAccountKey = new System.Windows.Forms.TextBox();
            this.textBoxStorageAccountName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxStorageContainerName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJob)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxAccountKey
            // 
            this.textBoxAccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAccountKey.Location = new System.Drawing.Point(15, 74);
            this.textBoxAccountKey.Name = "textBoxAccountKey";
            this.textBoxAccountKey.Size = new System.Drawing.Size(154, 20);
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
            this.textBoxAccountName.Size = new System.Drawing.Size(154, 20);
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
            this.buttonLogin.Location = new System.Drawing.Point(531, 25);
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
            this.buttonDonwloadSubtitle.Location = new System.Drawing.Point(504, 287);
            this.buttonDonwloadSubtitle.Name = "buttonDonwloadSubtitle";
            this.buttonDonwloadSubtitle.Size = new System.Drawing.Size(116, 27);
            this.buttonDonwloadSubtitle.TabIndex = 55;
            this.buttonDonwloadSubtitle.Text = "Download Subtitle";
            this.buttonDonwloadSubtitle.UseVisualStyleBackColor = true;
            this.buttonDonwloadSubtitle.Click += new System.EventHandler(this.buttonPlaySubtitle_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(15, 518);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(727, 184);
            this.richTextBoxLog.TabIndex = 56;
            this.richTextBoxLog.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 502);
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
            this.buttonOpenSubtitle.Size = new System.Drawing.Size(102, 27);
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
            // textBoxSearchAccountName
            // 
            this.textBoxSearchAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchAccountName.Location = new System.Drawing.Point(175, 35);
            this.textBoxSearchAccountName.Name = "textBoxSearchAccountName";
            this.textBoxSearchAccountName.Size = new System.Drawing.Size(148, 20);
            this.textBoxSearchAccountName.TabIndex = 63;
            // 
            // textBoxSearchAccountKey
            // 
            this.textBoxSearchAccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchAccountKey.Location = new System.Drawing.Point(175, 74);
            this.textBoxSearchAccountKey.Name = "textBoxSearchAccountKey";
            this.textBoxSearchAccountKey.Size = new System.Drawing.Size(148, 20);
            this.textBoxSearchAccountKey.TabIndex = 64;
            this.textBoxSearchAccountKey.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(172, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 65;
            this.label9.Text = "Search Account Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(172, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 66;
            this.label10.Text = "Search Account Key";
            // 
            // buttonCreateIndex
            // 
            this.buttonCreateIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateIndex.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonCreateIndex.Location = new System.Drawing.Point(15, 466);
            this.buttonCreateIndex.Name = "buttonCreateIndex";
            this.buttonCreateIndex.Size = new System.Drawing.Size(86, 27);
            this.buttonCreateIndex.TabIndex = 67;
            this.buttonCreateIndex.Text = "Create Index";
            this.buttonCreateIndex.UseVisualStyleBackColor = true;
            this.buttonCreateIndex.Click += new System.EventHandler(this.buttonCreateIndex_Click);
            // 
            // buttonDeleteIndex
            // 
            this.buttonDeleteIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteIndex.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonDeleteIndex.Location = new System.Drawing.Point(107, 466);
            this.buttonDeleteIndex.Name = "buttonDeleteIndex";
            this.buttonDeleteIndex.Size = new System.Drawing.Size(86, 27);
            this.buttonDeleteIndex.TabIndex = 68;
            this.buttonDeleteIndex.Text = "Delete Index";
            this.buttonDeleteIndex.UseVisualStyleBackColor = true;
            this.buttonDeleteIndex.Click += new System.EventHandler(this.buttonDeleteIndex_Click);
            // 
            // buttonPopulateIndex
            // 
            this.buttonPopulateIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPopulateIndex.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonPopulateIndex.Location = new System.Drawing.Point(199, 466);
            this.buttonPopulateIndex.Name = "buttonPopulateIndex";
            this.buttonPopulateIndex.Size = new System.Drawing.Size(96, 27);
            this.buttonPopulateIndex.TabIndex = 69;
            this.buttonPopulateIndex.Text = "Populate Index";
            this.buttonPopulateIndex.UseVisualStyleBackColor = true;
            this.buttonPopulateIndex.Click += new System.EventHandler(this.buttonPopulateIndex_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonSearch.Location = new System.Drawing.Point(394, 466);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(63, 27);
            this.buttonSearch.TabIndex = 70;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch.Location = new System.Drawing.Point(463, 473);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(283, 20);
            this.textBoxSearch.TabIndex = 71;
            // 
            // buttonPlaySubtitle
            // 
            this.buttonPlaySubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlaySubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonPlaySubtitle.Location = new System.Drawing.Point(626, 287);
            this.buttonPlaySubtitle.Name = "buttonPlaySubtitle";
            this.buttonPlaySubtitle.Size = new System.Drawing.Size(116, 27);
            this.buttonPlaySubtitle.TabIndex = 72;
            this.buttonPlaySubtitle.Text = "Play Subtitle";
            this.buttonPlaySubtitle.UseVisualStyleBackColor = true;
            this.buttonPlaySubtitle.Click += new System.EventHandler(this.buttonPlaySubtitle_Click_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(329, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 13);
            this.label11.TabIndex = 76;
            this.label11.Text = "Storage Account Key";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(329, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(118, 13);
            this.label12.TabIndex = 75;
            this.label12.Text = "Storage Account Name";
            // 
            // textBoxStorageAccountKey
            // 
            this.textBoxStorageAccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStorageAccountKey.Location = new System.Drawing.Point(332, 74);
            this.textBoxStorageAccountKey.Name = "textBoxStorageAccountKey";
            this.textBoxStorageAccountKey.Size = new System.Drawing.Size(148, 20);
            this.textBoxStorageAccountKey.TabIndex = 74;
            this.textBoxStorageAccountKey.UseSystemPasswordChar = true;
            // 
            // textBoxStorageAccountName
            // 
            this.textBoxStorageAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStorageAccountName.Location = new System.Drawing.Point(332, 35);
            this.textBoxStorageAccountName.Name = "textBoxStorageAccountName";
            this.textBoxStorageAccountName.Size = new System.Drawing.Size(148, 20);
            this.textBoxStorageAccountName.TabIndex = 73;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(486, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 13);
            this.label13.TabIndex = 78;
            this.label13.Text = "Storage Container Name";
            // 
            // textBoxStorageContainerName
            // 
            this.textBoxStorageContainerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStorageContainerName.Location = new System.Drawing.Point(489, 74);
            this.textBoxStorageContainerName.Name = "textBoxStorageContainerName";
            this.textBoxStorageContainerName.Size = new System.Drawing.Size(148, 20);
            this.textBoxStorageContainerName.TabIndex = 77;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 714);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxStorageContainerName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxStorageAccountKey);
            this.Controls.Add(this.textBoxStorageAccountName);
            this.Controls.Add(this.buttonPlaySubtitle);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonPopulateIndex);
            this.Controls.Add(this.buttonDeleteIndex);
            this.Controls.Add(this.buttonCreateIndex);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxSearchAccountKey);
            this.Controls.Add(this.textBoxSearchAccountName);
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
        private System.Windows.Forms.TextBox textBoxSearchAccountName;
        private System.Windows.Forms.TextBox textBoxSearchAccountKey;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonCreateIndex;
        private System.Windows.Forms.Button buttonDeleteIndex;
        private System.Windows.Forms.Button buttonPopulateIndex;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonPlaySubtitle;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxStorageAccountKey;
        private System.Windows.Forms.TextBox textBoxStorageAccountName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxStorageContainerName;
    }
}

