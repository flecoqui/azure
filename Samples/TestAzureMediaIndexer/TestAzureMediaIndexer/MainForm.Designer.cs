namespace TestAzureMediaIndexer
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
            this.textBoxMediaAccountKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMediaAccountName = new System.Windows.Forms.TextBox();
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
            this.buttonPlayAudioSubtitle = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxAssetPrefix = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxTranslatorAPIKey = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxPlayerUri = new System.Windows.Forms.TextBox();
            this.buttonPlayVideoSubtitle = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBoxTranslateLanguages = new System.Windows.Forms.ComboBox();
            this.buttonTranslateSubtitile = new System.Windows.Forms.Button();
            this.buttonUpdateSubtitle = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJob)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxMediaAccountKey
            // 
            this.textBoxMediaAccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMediaAccountKey.Location = new System.Drawing.Point(15, 79);
            this.textBoxMediaAccountKey.Name = "textBoxMediaAccountKey";
            this.textBoxMediaAccountKey.Size = new System.Drawing.Size(150, 20);
            this.textBoxMediaAccountKey.TabIndex = 34;
            this.textBoxMediaAccountKey.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Media Service Account Key";
            // 
            // textBoxMediaAccountName
            // 
            this.textBoxMediaAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMediaAccountName.Location = new System.Drawing.Point(15, 40);
            this.textBoxMediaAccountName.Name = "textBoxMediaAccountName";
            this.textBoxMediaAccountName.Size = new System.Drawing.Size(150, 20);
            this.textBoxMediaAccountName.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Media Service Account Name";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonLogin.Location = new System.Drawing.Point(469, 24);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(96, 27);
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
            this.pictureBoxJob.Location = new System.Drawing.Point(670, 3);
            this.pictureBoxJob.Name = "pictureBoxJob";
            this.pictureBoxJob.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxJob.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxJob.TabIndex = 43;
            this.pictureBoxJob.TabStop = false;
            // 
            // listInputAssets
            // 
            this.listInputAssets.FormattingEnabled = true;
            this.listInputAssets.HorizontalScrollbar = true;
            this.listInputAssets.Location = new System.Drawing.Point(15, 248);
            this.listInputAssets.Name = "listInputAssets";
            this.listInputAssets.Size = new System.Drawing.Size(352, 121);
            this.listInputAssets.TabIndex = 44;
            this.listInputAssets.SelectedIndexChanged += new System.EventHandler(this.listAssets_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 229);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "List of Video/Audio Assets:";
            // 
            // listInputFiles
            // 
            this.listInputFiles.FormattingEnabled = true;
            this.listInputFiles.HorizontalScrollbar = true;
            this.listInputFiles.Location = new System.Drawing.Point(15, 391);
            this.listInputFiles.Name = "listInputFiles";
            this.listInputFiles.Size = new System.Drawing.Size(352, 121);
            this.listInputFiles.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(392, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "List of Subtitle Assets:";
            // 
            // buttonAddAsset
            // 
            this.buttonAddAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonAddAsset.Location = new System.Drawing.Point(29, 17);
            this.buttonAddAsset.Name = "buttonAddAsset";
            this.buttonAddAsset.Size = new System.Drawing.Size(94, 27);
            this.buttonAddAsset.TabIndex = 48;
            this.buttonAddAsset.Text = "Add Asset";
            this.buttonAddAsset.UseVisualStyleBackColor = true;
            this.buttonAddAsset.Click += new System.EventHandler(this.buttonAddAsset_Click);
            // 
            // buttonRemoveAsset
            // 
            this.buttonRemoveAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonRemoveAsset.Location = new System.Drawing.Point(29, 49);
            this.buttonRemoveAsset.Name = "buttonRemoveAsset";
            this.buttonRemoveAsset.Size = new System.Drawing.Size(93, 27);
            this.buttonRemoveAsset.TabIndex = 49;
            this.buttonRemoveAsset.Text = "Remove Asset";
            this.buttonRemoveAsset.UseVisualStyleBackColor = true;
            this.buttonRemoveAsset.Click += new System.EventHandler(this.buttonRemoveAsset_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 375);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "List of Input Files:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(387, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "List of Output Files:";
            // 
            // listOutputFiles
            // 
            this.listOutputFiles.FormattingEnabled = true;
            this.listOutputFiles.HorizontalScrollbar = true;
            this.listOutputFiles.Location = new System.Drawing.Point(390, 391);
            this.listOutputFiles.Name = "listOutputFiles";
            this.listOutputFiles.Size = new System.Drawing.Size(352, 121);
            this.listOutputFiles.TabIndex = 52;
            // 
            // listOutputAssets
            // 
            this.listOutputAssets.FormattingEnabled = true;
            this.listOutputAssets.HorizontalScrollbar = true;
            this.listOutputAssets.Location = new System.Drawing.Point(388, 251);
            this.listOutputAssets.Name = "listOutputAssets";
            this.listOutputAssets.Size = new System.Drawing.Size(352, 121);
            this.listOutputAssets.TabIndex = 51;
            this.listOutputAssets.SelectedIndexChanged += new System.EventHandler(this.listOutputAssets_SelectedIndexChanged);
            // 
            // buttonGenerateSubtitle
            // 
            this.buttonGenerateSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerateSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonGenerateSubtitle.Location = new System.Drawing.Point(10, 60);
            this.buttonGenerateSubtitle.Name = "buttonGenerateSubtitle";
            this.buttonGenerateSubtitle.Size = new System.Drawing.Size(121, 27);
            this.buttonGenerateSubtitle.TabIndex = 54;
            this.buttonGenerateSubtitle.Text = "Generate Subtitle";
            this.buttonGenerateSubtitle.UseVisualStyleBackColor = true;
            this.buttonGenerateSubtitle.Click += new System.EventHandler(this.buttonGenerateSubtitle_Click);
            // 
            // buttonDonwloadSubtitle
            // 
            this.buttonDonwloadSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDonwloadSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonDonwloadSubtitle.Location = new System.Drawing.Point(132, 48);
            this.buttonDonwloadSubtitle.Name = "buttonDonwloadSubtitle";
            this.buttonDonwloadSubtitle.Size = new System.Drawing.Size(118, 27);
            this.buttonDonwloadSubtitle.TabIndex = 55;
            this.buttonDonwloadSubtitle.Text = "Download Subtitle";
            this.buttonDonwloadSubtitle.UseVisualStyleBackColor = true;
            this.buttonDonwloadSubtitle.Click += new System.EventHandler(this.buttonDownloadSubtitle_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(15, 605);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(727, 184);
            this.richTextBoxLog.TabIndex = 56;
            this.richTextBoxLog.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 589);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 57;
            this.label7.Text = "Logs:";
            // 
            // comboBoxLanguages
            // 
            this.comboBoxLanguages.FormattingEnabled = true;
            this.comboBoxLanguages.Location = new System.Drawing.Point(10, 34);
            this.comboBoxLanguages.Name = "comboBoxLanguages";
            this.comboBoxLanguages.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLanguages.TabIndex = 58;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 59;
            this.label8.Text = "Language:";
            // 
            // buttonOpenSubtitle
            // 
            this.buttonOpenSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonOpenSubtitle.Location = new System.Drawing.Point(132, 16);
            this.buttonOpenSubtitle.Name = "buttonOpenSubtitle";
            this.buttonOpenSubtitle.Size = new System.Drawing.Size(118, 27);
            this.buttonOpenSubtitle.TabIndex = 61;
            this.buttonOpenSubtitle.Text = "Open Subtitle";
            this.buttonOpenSubtitle.UseVisualStyleBackColor = true;
            this.buttonOpenSubtitle.Click += new System.EventHandler(this.buttonOpenSubtitle_Click);
            // 
            // buttonDisplayJobs
            // 
            this.buttonDisplayJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDisplayJobs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonDisplayJobs.Location = new System.Drawing.Point(11, 93);
            this.buttonDisplayJobs.Name = "buttonDisplayJobs";
            this.buttonDisplayJobs.Size = new System.Drawing.Size(120, 27);
            this.buttonDisplayJobs.TabIndex = 62;
            this.buttonDisplayJobs.Text = "Display Jobs";
            this.buttonDisplayJobs.UseVisualStyleBackColor = true;
            this.buttonDisplayJobs.Click += new System.EventHandler(this.buttonDisplayJobs_Click);
            // 
            // textBoxSearchAccountName
            // 
            this.textBoxSearchAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchAccountName.Location = new System.Drawing.Point(324, 40);
            this.textBoxSearchAccountName.Name = "textBoxSearchAccountName";
            this.textBoxSearchAccountName.Size = new System.Drawing.Size(150, 20);
            this.textBoxSearchAccountName.TabIndex = 63;
            // 
            // textBoxSearchAccountKey
            // 
            this.textBoxSearchAccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchAccountKey.Location = new System.Drawing.Point(324, 79);
            this.textBoxSearchAccountKey.Name = "textBoxSearchAccountKey";
            this.textBoxSearchAccountKey.Size = new System.Drawing.Size(150, 20);
            this.textBoxSearchAccountKey.TabIndex = 64;
            this.textBoxSearchAccountKey.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(321, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 65;
            this.label9.Text = "Search Account Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(321, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 66;
            this.label10.Text = "Search Account Key";
            // 
            // buttonCreateIndex
            // 
            this.buttonCreateIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateIndex.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonCreateIndex.Location = new System.Drawing.Point(8, 19);
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
            this.buttonDeleteIndex.Location = new System.Drawing.Point(100, 19);
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
            this.buttonPopulateIndex.Location = new System.Drawing.Point(192, 19);
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
            this.buttonSearch.Location = new System.Drawing.Point(633, 19);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(86, 27);
            this.buttonSearch.TabIndex = 70;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch.Location = new System.Drawing.Point(309, 19);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(320, 20);
            this.textBoxSearch.TabIndex = 71;
            // 
            // buttonPlayAudioSubtitle
            // 
            this.buttonPlayAudioSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlayAudioSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonPlayAudioSubtitle.Location = new System.Drawing.Point(8, 48);
            this.buttonPlayAudioSubtitle.Name = "buttonPlayAudioSubtitle";
            this.buttonPlayAudioSubtitle.Size = new System.Drawing.Size(118, 27);
            this.buttonPlayAudioSubtitle.TabIndex = 72;
            this.buttonPlayAudioSubtitle.Text = "Play Audio/Subtitle";
            this.buttonPlayAudioSubtitle.UseVisualStyleBackColor = true;
            this.buttonPlayAudioSubtitle.Click += new System.EventHandler(this.buttonPlayAudioSubtitle_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(158, 53);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 80;
            this.label14.Text = "Asset Prefix";
            // 
            // textBoxAssetPrefix
            // 
            this.textBoxAssetPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAssetPrefix.Location = new System.Drawing.Point(159, 69);
            this.textBoxAssetPrefix.Name = "textBoxAssetPrefix";
            this.textBoxAssetPrefix.Size = new System.Drawing.Size(150, 20);
            this.textBoxAssetPrefix.TabIndex = 79;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(166, 24);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(95, 13);
            this.label15.TabIndex = 82;
            this.label15.Text = "Translator API Key";
            // 
            // textBoxTranslatorAPIKey
            // 
            this.textBoxTranslatorAPIKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTranslatorAPIKey.Location = new System.Drawing.Point(169, 40);
            this.textBoxTranslatorAPIKey.Name = "textBoxTranslatorAPIKey";
            this.textBoxTranslatorAPIKey.Size = new System.Drawing.Size(150, 20);
            this.textBoxTranslatorAPIKey.TabIndex = 81;
            this.textBoxTranslatorAPIKey.UseSystemPasswordChar = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(469, 53);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(52, 13);
            this.label16.TabIndex = 84;
            this.label16.Text = "Player Uri";
            // 
            // textBoxPlayerUri
            // 
            this.textBoxPlayerUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPlayerUri.Location = new System.Drawing.Point(469, 70);
            this.textBoxPlayerUri.Name = "textBoxPlayerUri";
            this.textBoxPlayerUri.Size = new System.Drawing.Size(261, 20);
            this.textBoxPlayerUri.TabIndex = 83;
            // 
            // buttonPlayVideoSubtitle
            // 
            this.buttonPlayVideoSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlayVideoSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonPlayVideoSubtitle.Location = new System.Drawing.Point(6, 17);
            this.buttonPlayVideoSubtitle.Name = "buttonPlayVideoSubtitle";
            this.buttonPlayVideoSubtitle.Size = new System.Drawing.Size(120, 27);
            this.buttonPlayVideoSubtitle.TabIndex = 85;
            this.buttonPlayVideoSubtitle.Text = "Play Video/Subtitle";
            this.buttonPlayVideoSubtitle.UseVisualStyleBackColor = true;
            this.buttonPlayVideoSubtitle.Click += new System.EventHandler(this.buttonPlayVideoSubtitle_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 20);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 88;
            this.label17.Text = "Language:";
            // 
            // comboBoxTranslateLanguages
            // 
            this.comboBoxTranslateLanguages.FormattingEnabled = true;
            this.comboBoxTranslateLanguages.Location = new System.Drawing.Point(9, 45);
            this.comboBoxTranslateLanguages.Name = "comboBoxTranslateLanguages";
            this.comboBoxTranslateLanguages.Size = new System.Drawing.Size(148, 21);
            this.comboBoxTranslateLanguages.TabIndex = 87;
            // 
            // buttonTranslateSubtitile
            // 
            this.buttonTranslateSubtitile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTranslateSubtitile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonTranslateSubtitile.Location = new System.Drawing.Point(9, 76);
            this.buttonTranslateSubtitile.Name = "buttonTranslateSubtitile";
            this.buttonTranslateSubtitile.Size = new System.Drawing.Size(148, 27);
            this.buttonTranslateSubtitile.TabIndex = 86;
            this.buttonTranslateSubtitile.Text = "Translate Subtitle";
            this.buttonTranslateSubtitile.UseVisualStyleBackColor = true;
            this.buttonTranslateSubtitile.Click += new System.EventHandler(this.buttonTranslateSubtitile_Click);
            // 
            // buttonUpdateSubtitle
            // 
            this.buttonUpdateSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonUpdateSubtitle.Location = new System.Drawing.Point(132, 80);
            this.buttonUpdateSubtitle.Name = "buttonUpdateSubtitle";
            this.buttonUpdateSubtitle.Size = new System.Drawing.Size(118, 27);
            this.buttonUpdateSubtitle.TabIndex = 89;
            this.buttonUpdateSubtitle.Text = "Update Subtitle";
            this.buttonUpdateSubtitle.UseVisualStyleBackColor = true;
            this.buttonUpdateSubtitle.Click += new System.EventHandler(this.buttonUpdateSubtitle_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxJob);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.buttonLogin);
            this.groupBox1.Controls.Add(this.textBoxPlayerUri);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.textBoxAssetPrefix);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(734, 102);
            this.groupBox1.TabIndex = 90;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Connection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRemoveAsset);
            this.groupBox2.Controls.Add(this.buttonAddAsset);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 85);
            this.groupBox2.TabIndex = 91;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Upload Video/Audio file";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonGenerateSubtitle);
            this.groupBox3.Controls.Add(this.comboBoxLanguages);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.buttonDisplayJobs);
            this.groupBox3.Location = new System.Drawing.Point(167, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(148, 124);
            this.groupBox3.TabIndex = 92;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. Generate Subtitles ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.buttonTranslateSubtitile);
            this.groupBox4.Controls.Add(this.comboBoxTranslateLanguages);
            this.groupBox4.Location = new System.Drawing.Point(583, 118);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(163, 109);
            this.groupBox4.TabIndex = 93;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "5. Translate Subtitles ";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttonUpdateSubtitle);
            this.groupBox5.Controls.Add(this.buttonPlayVideoSubtitle);
            this.groupBox5.Controls.Add(this.buttonPlayAudioSubtitle);
            this.groupBox5.Controls.Add(this.buttonOpenSubtitle);
            this.groupBox5.Controls.Add(this.buttonDonwloadSubtitle);
            this.groupBox5.Location = new System.Drawing.Point(321, 115);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(256, 112);
            this.groupBox5.TabIndex = 94;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "4. Check and update subtitles";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buttonSearch);
            this.groupBox6.Controls.Add(this.buttonCreateIndex);
            this.groupBox6.Controls.Add(this.buttonDeleteIndex);
            this.groupBox6.Controls.Add(this.textBoxSearch);
            this.groupBox6.Controls.Add(this.buttonPopulateIndex);
            this.groupBox6.Location = new System.Drawing.Point(12, 518);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(734, 68);
            this.groupBox6.TabIndex = 95;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "6. Search";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 803);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxTranslatorAPIKey);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxSearchAccountKey);
            this.Controls.Add(this.textBoxSearchAccountName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listOutputFiles);
            this.Controls.Add(this.listOutputAssets);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listInputFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listInputAssets);
            this.Controls.Add(this.textBoxMediaAccountKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxMediaAccountName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox6);
            this.Name = "MainForm";
            this.Text = "Test Azure Media Services indexer and Azure Search";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJob)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMediaAccountKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMediaAccountName;
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
        private System.Windows.Forms.Button buttonPlayAudioSubtitle;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxAssetPrefix;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxTranslatorAPIKey;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxPlayerUri;
        private System.Windows.Forms.Button buttonPlayVideoSubtitle;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboBoxTranslateLanguages;
        private System.Windows.Forms.Button buttonTranslateSubtitile;
        private System.Windows.Forms.Button buttonUpdateSubtitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}

