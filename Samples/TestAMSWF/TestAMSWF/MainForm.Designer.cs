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
            this.listAssets = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listFiles = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAddAsset = new System.Windows.Forms.Button();
            this.buttonRemoveAsset = new System.Windows.Forms.Button();
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
            this.pictureBoxJob.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxJob.Image")));
            this.pictureBoxJob.Location = new System.Drawing.Point(657, 5);
            this.pictureBoxJob.Name = "pictureBoxJob";
            this.pictureBoxJob.Size = new System.Drawing.Size(96, 96);
            this.pictureBoxJob.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxJob.TabIndex = 43;
            this.pictureBoxJob.TabStop = false;
            // 
            // listAssets
            // 
            this.listAssets.FormattingEnabled = true;
            this.listAssets.Location = new System.Drawing.Point(15, 161);
            this.listAssets.Name = "listAssets";
            this.listAssets.Size = new System.Drawing.Size(352, 212);
            this.listAssets.TabIndex = 44;
            this.listAssets.SelectedIndexChanged += new System.EventHandler(this.listAssets_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "List of Assets";
            // 
            // listFiles
            // 
            this.listFiles.FormattingEnabled = true;
            this.listFiles.Location = new System.Drawing.Point(373, 161);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(380, 212);
            this.listFiles.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "List of Files";
            // 
            // buttonAddAsset
            // 
            this.buttonAddAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonAddAsset.Location = new System.Drawing.Point(15, 104);
            this.buttonAddAsset.Name = "buttonAddAsset";
            this.buttonAddAsset.Size = new System.Drawing.Size(105, 27);
            this.buttonAddAsset.TabIndex = 48;
            this.buttonAddAsset.Text = "Add Asset";
            this.buttonAddAsset.UseVisualStyleBackColor = true;
            this.buttonAddAsset.Click += new System.EventHandler(this.buttonAddAsset_Click);
            // 
            // buttonRemoveAsset
            // 
            this.buttonRemoveAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonRemoveAsset.Location = new System.Drawing.Point(136, 104);
            this.buttonRemoveAsset.Name = "buttonRemoveAsset";
            this.buttonRemoveAsset.Size = new System.Drawing.Size(105, 27);
            this.buttonRemoveAsset.TabIndex = 49;
            this.buttonRemoveAsset.Text = "Remove Asset";
            this.buttonRemoveAsset.UseVisualStyleBackColor = true;
            this.buttonRemoveAsset.Click += new System.EventHandler(this.buttonRemoveAsset_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 491);
            this.Controls.Add(this.buttonRemoveAsset);
            this.Controls.Add(this.buttonAddAsset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listAssets);
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
        private System.Windows.Forms.ListBox listAssets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAddAsset;
        private System.Windows.Forms.Button buttonRemoveAsset;
    }
}

