namespace plugin
{
    partial class TagBindingConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagBindingConfig));
            this.separateCheckBox = new System.Windows.Forms.CheckBox();
            this.separateHelperLabel = new System.Windows.Forms.Label();
            this.tagGroupBox = new System.Windows.Forms.GroupBox();
            this.tagTabControl = new System.Windows.Forms.TabControl();
            this.trackTab = new System.Windows.Forms.TabPage();
            this.trackListBox = new System.Windows.Forms.CheckedListBox();
            this.releaseTab = new System.Windows.Forms.TabPage();
            this.releaseListBox = new System.Windows.Forms.CheckedListBox();
            this.releaseGroupTab = new System.Windows.Forms.TabPage();
            this.releaseGroupListBox = new System.Windows.Forms.CheckedListBox();
            this.saveNote = new System.Windows.Forms.Label();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tagBindingPage = new System.Windows.Forms.TabPage();
            this.tagBindingLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tagDownloadTab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.userTagConfigTitle = new System.Windows.Forms.Label();
            this.userTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.genreConfigTitle = new System.Windows.Forms.Label();
            this.genreDownloadCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldConfigTitle = new System.Windows.Forms.Label();
            this.sepByEntityCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.recordingGenreLabel = new System.Windows.Forms.Label();
            this.recordingGenreComboBox = new System.Windows.Forms.ComboBox();
            this.recordingTagLabel = new System.Windows.Forms.Label();
            this.recordingTagComboBox = new System.Windows.Forms.ComboBox();
            this.otherTypePanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.releaseGenreLabel = new System.Windows.Forms.Label();
            this.releaseGenreComboBox = new System.Windows.Forms.ComboBox();
            this.releaseTagLabel = new System.Windows.Forms.Label();
            this.releaseTagComboBox = new System.Windows.Forms.ComboBox();
            this.releaseGroupGenreLabel = new System.Windows.Forms.Label();
            this.releaseGroupGenreComboBox = new System.Windows.Forms.ComboBox();
            this.releaseGroupTagLabel = new System.Windows.Forms.Label();
            this.releaseGroupTagComboBox = new System.Windows.Forms.ComboBox();
            this.findReplacePage = new System.Windows.Forms.TabPage();
            this.findReplaceTable = new System.Windows.Forms.DataGridView();
            this.findColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.replaceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otherSettingsPage = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tagSubmitModeLabel = new System.Windows.Forms.Label();
            this.appendRadioButton = new System.Windows.Forms.RadioButton();
            this.replaceRadioButton = new System.Windows.Forms.RadioButton();
            this.tagSubmitNote = new System.Windows.Forms.Label();
            this.importExportLabel = new System.Windows.Forms.Label();
            this.exportLink = new System.Windows.Forms.LinkLabel();
            this.importLink = new System.Windows.Forms.LinkLabel();
            this.resetTitle = new System.Windows.Forms.Label();
            this.resetDescription = new System.Windows.Forms.Label();
            this.resetLink = new System.Windows.Forms.LinkLabel();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tagGroupBox.SuspendLayout();
            this.tagTabControl.SuspendLayout();
            this.trackTab.SuspendLayout();
            this.releaseTab.SuspendLayout();
            this.releaseGroupTab.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.tagBindingPage.SuspendLayout();
            this.tagBindingLayoutPanel.SuspendLayout();
            this.tagDownloadTab.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.otherTypePanel.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.findReplacePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.findReplaceTable)).BeginInit();
            this.otherSettingsPage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // separateCheckBox
            // 
            this.separateCheckBox.AutoSize = true;
            this.separateCheckBox.Location = new System.Drawing.Point(3, 3);
            this.separateCheckBox.Name = "separateCheckBox";
            this.separateCheckBox.Size = new System.Drawing.Size(314, 17);
            this.separateCheckBox.TabIndex = 0;
            this.separateCheckBox.Text = "Submit separate tags to releases and release groups.             ";
            this.separateCheckBox.UseVisualStyleBackColor = true;
            this.separateCheckBox.CheckedChanged += new System.EventHandler(this.separateCheckBox_CheckedChanged);
            // 
            // separateHelperLabel
            // 
            this.separateHelperLabel.AutoSize = true;
            this.separateHelperLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.separateHelperLabel.Location = new System.Drawing.Point(3, 23);
            this.separateHelperLabel.Name = "separateHelperLabel";
            this.separateHelperLabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.separateHelperLabel.Size = new System.Drawing.Size(298, 32);
            this.separateHelperLabel.TabIndex = 1;
            this.separateHelperLabel.Text = "Useful if you use separate tags for whole releases (e.g. album genres)";
            // 
            // tagGroupBox
            // 
            this.tagGroupBox.Controls.Add(this.tagTabControl);
            this.tagGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagGroupBox.Location = new System.Drawing.Point(2, 57);
            this.tagGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tagGroupBox.Name = "tagGroupBox";
            this.tagGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tagGroupBox.Size = new System.Drawing.Size(334, 459);
            this.tagGroupBox.TabIndex = 3;
            this.tagGroupBox.TabStop = false;
            this.tagGroupBox.Text = "Tags";
            // 
            // tagTabControl
            // 
            this.tagTabControl.Controls.Add(this.trackTab);
            this.tagTabControl.Controls.Add(this.releaseTab);
            this.tagTabControl.Controls.Add(this.releaseGroupTab);
            this.tagTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagTabControl.Location = new System.Drawing.Point(2, 15);
            this.tagTabControl.Name = "tagTabControl";
            this.tagTabControl.SelectedIndex = 0;
            this.tagTabControl.Size = new System.Drawing.Size(330, 442);
            this.tagTabControl.TabIndex = 0;
            // 
            // trackTab
            // 
            this.trackTab.Controls.Add(this.trackListBox);
            this.trackTab.Location = new System.Drawing.Point(4, 22);
            this.trackTab.Name = "trackTab";
            this.trackTab.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.trackTab.Size = new System.Drawing.Size(322, 416);
            this.trackTab.TabIndex = 0;
            this.trackTab.Text = "Tracks/Recordings";
            this.trackTab.UseVisualStyleBackColor = true;
            // 
            // trackListBox
            // 
            this.trackListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackListBox.FormattingEnabled = true;
            this.trackListBox.Location = new System.Drawing.Point(3, 3);
            this.trackListBox.Name = "trackListBox";
            this.trackListBox.Size = new System.Drawing.Size(316, 410);
            this.trackListBox.TabIndex = 0;
            this.trackListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.trackListBox_ItemCheck);
            // 
            // releaseTab
            // 
            this.releaseTab.Controls.Add(this.releaseListBox);
            this.releaseTab.Location = new System.Drawing.Point(4, 22);
            this.releaseTab.Name = "releaseTab";
            this.releaseTab.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.releaseTab.Size = new System.Drawing.Size(324, 450);
            this.releaseTab.TabIndex = 1;
            this.releaseTab.Text = "Releases";
            this.releaseTab.UseVisualStyleBackColor = true;
            // 
            // releaseListBox
            // 
            this.releaseListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.releaseListBox.FormattingEnabled = true;
            this.releaseListBox.Location = new System.Drawing.Point(3, 3);
            this.releaseListBox.Name = "releaseListBox";
            this.releaseListBox.Size = new System.Drawing.Size(318, 444);
            this.releaseListBox.TabIndex = 0;
            this.releaseListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.releaseListBox_ItemCheck);
            // 
            // releaseGroupTab
            // 
            this.releaseGroupTab.Controls.Add(this.releaseGroupListBox);
            this.releaseGroupTab.Location = new System.Drawing.Point(4, 22);
            this.releaseGroupTab.Name = "releaseGroupTab";
            this.releaseGroupTab.Size = new System.Drawing.Size(324, 450);
            this.releaseGroupTab.TabIndex = 2;
            this.releaseGroupTab.Text = "Release Groups";
            this.releaseGroupTab.UseVisualStyleBackColor = true;
            // 
            // releaseGroupListBox
            // 
            this.releaseGroupListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.releaseGroupListBox.FormattingEnabled = true;
            this.releaseGroupListBox.Location = new System.Drawing.Point(0, 0);
            this.releaseGroupListBox.Name = "releaseGroupListBox";
            this.releaseGroupListBox.Size = new System.Drawing.Size(324, 450);
            this.releaseGroupListBox.TabIndex = 0;
            this.releaseGroupListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.releaseGroupListBox_ItemCheck);
            // 
            // saveNote
            // 
            this.saveNote.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.saveNote.AutoSize = true;
            this.saveNote.Location = new System.Drawing.Point(52, 29);
            this.saveNote.Name = "saveNote";
            this.saveNote.Size = new System.Drawing.Size(297, 26);
            this.saveNote.TabIndex = 6;
            this.saveNote.Text = "Settings are automatically saved upon clicking Save or Apply, independent from Mu" +
    "sicBee.";
            this.saveNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonPanel
            // 
            this.buttonPanel.AutoSize = true;
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Controls.Add(this.OKButton);
            this.buttonPanel.Controls.Add(this.applyButton);
            this.buttonPanel.Controls.Add(this.saveNote);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(3, 559);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(352, 55);
            this.buttonPanel.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(274, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(193, 3);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "Save";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(112, 3);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 7;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tagBindingPage);
            this.mainTabControl.Controls.Add(this.tagDownloadTab);
            this.mainTabControl.Controls.Add(this.findReplacePage);
            this.mainTabControl.Controls.Add(this.otherSettingsPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(3, 3);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(352, 550);
            this.mainTabControl.TabIndex = 1;
            // 
            // tagBindingPage
            // 
            this.tagBindingPage.Controls.Add(this.tagBindingLayoutPanel);
            this.tagBindingPage.Location = new System.Drawing.Point(4, 22);
            this.tagBindingPage.Name = "tagBindingPage";
            this.tagBindingPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tagBindingPage.Size = new System.Drawing.Size(344, 524);
            this.tagBindingPage.TabIndex = 0;
            this.tagBindingPage.Text = "Tag Uploading";
            this.tagBindingPage.UseVisualStyleBackColor = true;
            // 
            // tagBindingLayoutPanel
            // 
            this.tagBindingLayoutPanel.AutoSize = true;
            this.tagBindingLayoutPanel.ColumnCount = 1;
            this.tagBindingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tagBindingLayoutPanel.Controls.Add(this.tagGroupBox, 0, 2);
            this.tagBindingLayoutPanel.Controls.Add(this.separateHelperLabel, 0, 1);
            this.tagBindingLayoutPanel.Controls.Add(this.separateCheckBox, 0, 0);
            this.tagBindingLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagBindingLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tagBindingLayoutPanel.Name = "tagBindingLayoutPanel";
            this.tagBindingLayoutPanel.RowCount = 5;
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.Size = new System.Drawing.Size(338, 518);
            this.tagBindingLayoutPanel.TabIndex = 1;
            // 
            // tagDownloadTab
            // 
            this.tagDownloadTab.Controls.Add(this.flowLayoutPanel2);
            this.tagDownloadTab.Location = new System.Drawing.Point(4, 22);
            this.tagDownloadTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tagDownloadTab.Name = "tagDownloadTab";
            this.tagDownloadTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tagDownloadTab.Size = new System.Drawing.Size(344, 524);
            this.tagDownloadTab.TabIndex = 3;
            this.tagDownloadTab.Text = "Tag Downloading";
            this.tagDownloadTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.userTagConfigTitle);
            this.flowLayoutPanel2.Controls.Add(this.userTagsCheckBox);
            this.flowLayoutPanel2.Controls.Add(this.genreConfigTitle);
            this.flowLayoutPanel2.Controls.Add(this.genreDownloadCheckBox);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.fieldConfigTitle);
            this.flowLayoutPanel2.Controls.Add(this.sepByEntityCheckBox);
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.recordingGenreLabel);
            this.flowLayoutPanel2.Controls.Add(this.recordingGenreComboBox);
            this.flowLayoutPanel2.Controls.Add(this.recordingTagLabel);
            this.flowLayoutPanel2.Controls.Add(this.recordingTagComboBox);
            this.flowLayoutPanel2.Controls.Add(this.otherTypePanel);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(340, 520);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // userTagConfigTitle
            // 
            this.userTagConfigTitle.AutoSize = true;
            this.userTagConfigTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userTagConfigTitle.Location = new System.Drawing.Point(2, 0);
            this.userTagConfigTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.userTagConfigTitle.Name = "userTagConfigTitle";
            this.userTagConfigTitle.Size = new System.Drawing.Size(61, 13);
            this.userTagConfigTitle.TabIndex = 23;
            this.userTagConfigTitle.Text = "User tags";
            // 
            // userTagsCheckBox
            // 
            this.userTagsCheckBox.AutoSize = true;
            this.userTagsCheckBox.Location = new System.Drawing.Point(2, 15);
            this.userTagsCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.userTagsCheckBox.Name = "userTagsCheckBox";
            this.userTagsCheckBox.Size = new System.Drawing.Size(158, 17);
            this.userTagsCheckBox.TabIndex = 24;
            this.userTagsCheckBox.Text = "Download only my own tags";
            this.userTagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // genreConfigTitle
            // 
            this.genreConfigTitle.AutoSize = true;
            this.genreConfigTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genreConfigTitle.Location = new System.Drawing.Point(2, 34);
            this.genreConfigTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.genreConfigTitle.Name = "genreConfigTitle";
            this.genreConfigTitle.Size = new System.Drawing.Size(47, 13);
            this.genreConfigTitle.TabIndex = 5;
            this.genreConfigTitle.Text = "Genres";
            // 
            // genreDownloadCheckBox
            // 
            this.genreDownloadCheckBox.AutoSize = true;
            this.genreDownloadCheckBox.Location = new System.Drawing.Point(2, 49);
            this.genreDownloadCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.genreDownloadCheckBox.Name = "genreDownloadCheckBox";
            this.genreDownloadCheckBox.Size = new System.Drawing.Size(183, 17);
            this.genreDownloadCheckBox.TabIndex = 6;
            this.genreDownloadCheckBox.Text = "Separate genres and regular tags";
            this.genreDownloadCheckBox.UseVisualStyleBackColor = true;
            this.genreDownloadCheckBox.CheckedChanged += new System.EventHandler(this.genreDownloadCheckBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label3.Location = new System.Drawing.Point(3, 68);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.label3.Size = new System.Drawing.Size(334, 32);
            this.label3.TabIndex = 7;
            this.label3.Text = "This will ask MusicBrainz for genres as well as tags, and will separate genres an" +
    "d regular tags upon retrieval.\r\n";
            // 
            // fieldConfigTitle
            // 
            this.fieldConfigTitle.AutoSize = true;
            this.fieldConfigTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fieldConfigTitle.Location = new System.Drawing.Point(2, 100);
            this.fieldConfigTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fieldConfigTitle.Name = "fieldConfigTitle";
            this.fieldConfigTitle.Size = new System.Drawing.Size(124, 13);
            this.fieldConfigTitle.TabIndex = 4;
            this.fieldConfigTitle.Text = "Tag field assignment";
            // 
            // sepByEntityCheckBox
            // 
            this.sepByEntityCheckBox.AutoSize = true;
            this.sepByEntityCheckBox.Location = new System.Drawing.Point(2, 115);
            this.sepByEntityCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sepByEntityCheckBox.Name = "sepByEntityCheckBox";
            this.sepByEntityCheckBox.Size = new System.Drawing.Size(276, 17);
            this.sepByEntityCheckBox.TabIndex = 0;
            this.sepByEntityCheckBox.Text = "Send tags to separate fields depending on entity type";
            this.sepByEntityCheckBox.UseVisualStyleBackColor = true;
            this.sepByEntityCheckBox.CheckedChanged += new System.EventHandler(this.sepByEntityCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(3, 134);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.label1.Size = new System.Drawing.Size(333, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Useful if you use separate tags for whole releases (e.g. album genres)";
            // 
            // recordingGenreLabel
            // 
            this.recordingGenreLabel.AutoSize = true;
            this.recordingGenreLabel.Location = new System.Drawing.Point(2, 153);
            this.recordingGenreLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.recordingGenreLabel.Name = "recordingGenreLabel";
            this.recordingGenreLabel.Size = new System.Drawing.Size(106, 13);
            this.recordingGenreLabel.TabIndex = 10;
            this.recordingGenreLabel.Text = "Send genre tags to...";
            // 
            // recordingGenreComboBox
            // 
            this.recordingGenreComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordingGenreComboBox.Location = new System.Drawing.Point(2, 168);
            this.recordingGenreComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.recordingGenreComboBox.Name = "recordingGenreComboBox";
            this.recordingGenreComboBox.Size = new System.Drawing.Size(155, 21);
            this.recordingGenreComboBox.TabIndex = 11;
            // 
            // recordingTagLabel
            // 
            this.recordingTagLabel.AutoSize = true;
            this.recordingTagLabel.Location = new System.Drawing.Point(2, 191);
            this.recordingTagLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.recordingTagLabel.Name = "recordingTagLabel";
            this.recordingTagLabel.Size = new System.Drawing.Size(122, 13);
            this.recordingTagLabel.TabIndex = 8;
            this.recordingTagLabel.Text = "Send all (other) tags to...";
            // 
            // recordingTagComboBox
            // 
            this.recordingTagComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordingTagComboBox.FormattingEnabled = true;
            this.recordingTagComboBox.Location = new System.Drawing.Point(2, 206);
            this.recordingTagComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.recordingTagComboBox.Name = "recordingTagComboBox";
            this.recordingTagComboBox.Size = new System.Drawing.Size(155, 21);
            this.recordingTagComboBox.TabIndex = 9;
            // 
            // otherTypePanel
            // 
            this.otherTypePanel.Controls.Add(this.flowLayoutPanel3);
            this.otherTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.otherTypePanel.Location = new System.Drawing.Point(2, 231);
            this.otherTypePanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otherTypePanel.Name = "otherTypePanel";
            this.otherTypePanel.Size = new System.Drawing.Size(336, 159);
            this.otherTypePanel.TabIndex = 22;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.releaseGenreLabel);
            this.flowLayoutPanel3.Controls.Add(this.releaseGenreComboBox);
            this.flowLayoutPanel3.Controls.Add(this.releaseTagLabel);
            this.flowLayoutPanel3.Controls.Add(this.releaseTagComboBox);
            this.flowLayoutPanel3.Controls.Add(this.releaseGroupGenreLabel);
            this.flowLayoutPanel3.Controls.Add(this.releaseGroupGenreComboBox);
            this.flowLayoutPanel3.Controls.Add(this.releaseGroupTagLabel);
            this.flowLayoutPanel3.Controls.Add(this.releaseGroupTagComboBox);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(336, 159);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // releaseGenreLabel
            // 
            this.releaseGenreLabel.AutoSize = true;
            this.releaseGenreLabel.Location = new System.Drawing.Point(2, 0);
            this.releaseGenreLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.releaseGenreLabel.Name = "releaseGenreLabel";
            this.releaseGenreLabel.Size = new System.Drawing.Size(143, 13);
            this.releaseGenreLabel.TabIndex = 16;
            this.releaseGenreLabel.Text = "Send release genre tags to...";
            // 
            // releaseGenreComboBox
            // 
            this.releaseGenreComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.releaseGenreComboBox.Location = new System.Drawing.Point(2, 15);
            this.releaseGenreComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.releaseGenreComboBox.Name = "releaseGenreComboBox";
            this.releaseGenreComboBox.Size = new System.Drawing.Size(155, 21);
            this.releaseGenreComboBox.TabIndex = 17;
            // 
            // releaseTagLabel
            // 
            this.releaseTagLabel.AutoSize = true;
            this.releaseTagLabel.Location = new System.Drawing.Point(2, 38);
            this.releaseTagLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.releaseTagLabel.Name = "releaseTagLabel";
            this.releaseTagLabel.Size = new System.Drawing.Size(159, 13);
            this.releaseTagLabel.TabIndex = 14;
            this.releaseTagLabel.Text = "Send all (other) release tags to...";
            // 
            // releaseTagComboBox
            // 
            this.releaseTagComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.releaseTagComboBox.FormattingEnabled = true;
            this.releaseTagComboBox.Location = new System.Drawing.Point(2, 53);
            this.releaseTagComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.releaseTagComboBox.Name = "releaseTagComboBox";
            this.releaseTagComboBox.Size = new System.Drawing.Size(155, 21);
            this.releaseTagComboBox.TabIndex = 15;
            // 
            // releaseGroupGenreLabel
            // 
            this.releaseGroupGenreLabel.AutoSize = true;
            this.releaseGroupGenreLabel.Location = new System.Drawing.Point(2, 76);
            this.releaseGroupGenreLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.releaseGroupGenreLabel.Name = "releaseGroupGenreLabel";
            this.releaseGroupGenreLabel.Size = new System.Drawing.Size(173, 13);
            this.releaseGroupGenreLabel.TabIndex = 20;
            this.releaseGroupGenreLabel.Text = "Send release group genre tags to...";
            // 
            // releaseGroupGenreComboBox
            // 
            this.releaseGroupGenreComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.releaseGroupGenreComboBox.Location = new System.Drawing.Point(2, 91);
            this.releaseGroupGenreComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.releaseGroupGenreComboBox.Name = "releaseGroupGenreComboBox";
            this.releaseGroupGenreComboBox.Size = new System.Drawing.Size(155, 21);
            this.releaseGroupGenreComboBox.TabIndex = 21;
            // 
            // releaseGroupTagLabel
            // 
            this.releaseGroupTagLabel.AutoSize = true;
            this.releaseGroupTagLabel.Location = new System.Drawing.Point(2, 114);
            this.releaseGroupTagLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.releaseGroupTagLabel.Name = "releaseGroupTagLabel";
            this.releaseGroupTagLabel.Size = new System.Drawing.Size(189, 13);
            this.releaseGroupTagLabel.TabIndex = 18;
            this.releaseGroupTagLabel.Text = "Send all (other) release group tags to...";
            // 
            // releaseGroupTagComboBox
            // 
            this.releaseGroupTagComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.releaseGroupTagComboBox.FormattingEnabled = true;
            this.releaseGroupTagComboBox.Location = new System.Drawing.Point(2, 129);
            this.releaseGroupTagComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.releaseGroupTagComboBox.Name = "releaseGroupTagComboBox";
            this.releaseGroupTagComboBox.Size = new System.Drawing.Size(155, 21);
            this.releaseGroupTagComboBox.TabIndex = 19;
            // 
            // findReplacePage
            // 
            this.findReplacePage.Controls.Add(this.findReplaceTable);
            this.findReplacePage.Location = new System.Drawing.Point(4, 22);
            this.findReplacePage.Name = "findReplacePage";
            this.findReplacePage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.findReplacePage.Size = new System.Drawing.Size(344, 542);
            this.findReplacePage.TabIndex = 1;
            this.findReplacePage.Text = "Find and Replace";
            this.findReplacePage.UseVisualStyleBackColor = true;
            // 
            // findReplaceTable
            // 
            this.findReplaceTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.findReplaceTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.findReplaceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.findColumn,
            this.replaceColumn});
            this.findReplaceTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findReplaceTable.Location = new System.Drawing.Point(3, 3);
            this.findReplaceTable.Name = "findReplaceTable";
            this.findReplaceTable.RowHeadersWidth = 82;
            this.findReplaceTable.Size = new System.Drawing.Size(338, 536);
            this.findReplaceTable.TabIndex = 0;
            // 
            // findColumn
            // 
            this.findColumn.HeaderText = "Find (in tags)";
            this.findColumn.MinimumWidth = 10;
            this.findColumn.Name = "findColumn";
            // 
            // replaceColumn
            // 
            this.replaceColumn.HeaderText = "Replace (for MusicBrainz)";
            this.replaceColumn.MinimumWidth = 10;
            this.replaceColumn.Name = "replaceColumn";
            // 
            // otherSettingsPage
            // 
            this.otherSettingsPage.Controls.Add(this.flowLayoutPanel1);
            this.otherSettingsPage.Location = new System.Drawing.Point(4, 22);
            this.otherSettingsPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otherSettingsPage.Name = "otherSettingsPage";
            this.otherSettingsPage.Size = new System.Drawing.Size(344, 542);
            this.otherSettingsPage.TabIndex = 2;
            this.otherSettingsPage.Text = "Other Settings";
            this.otherSettingsPage.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.tagSubmitModeLabel);
            this.flowLayoutPanel1.Controls.Add(this.appendRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.replaceRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.tagSubmitNote);
            this.flowLayoutPanel1.Controls.Add(this.importExportLabel);
            this.flowLayoutPanel1.Controls.Add(this.exportLink);
            this.flowLayoutPanel1.Controls.Add(this.importLink);
            this.flowLayoutPanel1.Controls.Add(this.resetTitle);
            this.flowLayoutPanel1.Controls.Add(this.resetDescription);
            this.flowLayoutPanel1.Controls.Add(this.resetLink);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(344, 542);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tagSubmitModeLabel
            // 
            this.tagSubmitModeLabel.AutoSize = true;
            this.tagSubmitModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSubmitModeLabel.Location = new System.Drawing.Point(8, 5);
            this.tagSubmitModeLabel.Name = "tagSubmitModeLabel";
            this.tagSubmitModeLabel.Size = new System.Drawing.Size(128, 13);
            this.tagSubmitModeLabel.TabIndex = 0;
            this.tagSubmitModeLabel.Text = "Tag submission mode";
            // 
            // appendRadioButton
            // 
            this.appendRadioButton.AutoSize = true;
            this.appendRadioButton.Location = new System.Drawing.Point(8, 21);
            this.appendRadioButton.Name = "appendRadioButton";
            this.appendRadioButton.Size = new System.Drawing.Size(233, 17);
            this.appendRadioButton.TabIndex = 1;
            this.appendRadioButton.TabStop = true;
            this.appendRadioButton.Text = "Append tags to existing tags on MusicBrainz";
            this.appendRadioButton.UseVisualStyleBackColor = true;
            // 
            // replaceRadioButton
            // 
            this.replaceRadioButton.AutoSize = true;
            this.replaceRadioButton.Location = new System.Drawing.Point(8, 44);
            this.replaceRadioButton.Name = "replaceRadioButton";
            this.replaceRadioButton.Size = new System.Drawing.Size(287, 17);
            this.replaceRadioButton.TabIndex = 2;
            this.replaceRadioButton.TabStop = true;
            this.replaceRadioButton.Text = "Clear existing tags when submitting tags to MusicBrainz.";
            this.replaceRadioButton.UseVisualStyleBackColor = true;
            // 
            // tagSubmitNote
            // 
            this.tagSubmitNote.AutoSize = true;
            this.tagSubmitNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSubmitNote.Location = new System.Drawing.Point(8, 64);
            this.tagSubmitNote.Name = "tagSubmitNote";
            this.tagSubmitNote.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.tagSubmitNote.Size = new System.Drawing.Size(322, 44);
            this.tagSubmitNote.TabIndex = 3;
            this.tagSubmitNote.Text = "Append is enabled by default to prevent the accidental wiping of tags you already" +
    " have saved to MusicBrainz. If you are happy with replacing already existing tag" +
    "s, choose the second option.";
            // 
            // importExportLabel
            // 
            this.importExportLabel.AutoSize = true;
            this.importExportLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importExportLabel.Location = new System.Drawing.Point(8, 108);
            this.importExportLabel.Name = "importExportLabel";
            this.importExportLabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.importExportLabel.Size = new System.Drawing.Size(120, 16);
            this.importExportLabel.TabIndex = 4;
            this.importExportLabel.Text = "Export / import data";
            // 
            // exportLink
            // 
            this.exportLink.AutoSize = true;
            this.exportLink.Location = new System.Drawing.Point(7, 124);
            this.exportLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.exportLink.Name = "exportLink";
            this.exportLink.Size = new System.Drawing.Size(61, 13);
            this.exportLink.TabIndex = 5;
            this.exportLink.TabStop = true;
            this.exportLink.Text = "Export data";
            this.exportLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.exportLink_LinkClicked);
            // 
            // importLink
            // 
            this.importLink.AutoSize = true;
            this.importLink.Location = new System.Drawing.Point(7, 137);
            this.importLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.importLink.Name = "importLink";
            this.importLink.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.importLink.Size = new System.Drawing.Size(60, 18);
            this.importLink.TabIndex = 6;
            this.importLink.TabStop = true;
            this.importLink.Text = "Import data";
            this.importLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.importLink_LinkClicked);
            // 
            // resetTitle
            // 
            this.resetTitle.AutoSize = true;
            this.resetTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetTitle.Location = new System.Drawing.Point(8, 155);
            this.resetTitle.Name = "resetTitle";
            this.resetTitle.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.resetTitle.Size = new System.Drawing.Size(98, 16);
            this.resetTitle.TabIndex = 7;
            this.resetTitle.Text = "Reset to default";
            // 
            // resetDescription
            // 
            this.resetDescription.AutoSize = true;
            this.resetDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetDescription.Location = new System.Drawing.Point(8, 171);
            this.resetDescription.Name = "resetDescription";
            this.resetDescription.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.resetDescription.Size = new System.Drawing.Size(326, 57);
            this.resetDescription.TabIndex = 8;
            this.resetDescription.Text = resources.GetString("resetDescription.Text");
            // 
            // resetLink
            // 
            this.resetLink.AutoSize = true;
            this.resetLink.Location = new System.Drawing.Point(7, 228);
            this.resetLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.resetLink.Name = "resetLink";
            this.resetLink.Size = new System.Drawing.Size(74, 13);
            this.resetLink.TabIndex = 9;
            this.resetLink.TabStop = true;
            this.resetLink.Text = "Reset settings";
            this.resetLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.resetLink_LinkClicked);
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.buttonPanel, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.mainTabControl, 0, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayoutPanel.Size = new System.Drawing.Size(358, 617);
            this.mainLayoutPanel.TabIndex = 6;
            // 
            // TagBindingConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(358, 617);
            this.Controls.Add(this.mainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TagBindingConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MusicBrainz Sync - Tag Binding Configuration";
            this.Load += new System.EventHandler(this.TagBindingConfig_Load);
            this.tagGroupBox.ResumeLayout(false);
            this.tagTabControl.ResumeLayout(false);
            this.trackTab.ResumeLayout(false);
            this.releaseTab.ResumeLayout(false);
            this.releaseGroupTab.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.tagBindingPage.ResumeLayout(false);
            this.tagBindingPage.PerformLayout();
            this.tagBindingLayoutPanel.ResumeLayout(false);
            this.tagBindingLayoutPanel.PerformLayout();
            this.tagDownloadTab.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.otherTypePanel.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.findReplacePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.findReplaceTable)).EndInit();
            this.otherSettingsPage.ResumeLayout(false);
            this.otherSettingsPage.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox separateCheckBox;
        private System.Windows.Forms.Label separateHelperLabel;
        private System.Windows.Forms.GroupBox tagGroupBox;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label saveNote;
        private System.Windows.Forms.TabControl tagTabControl;
        private System.Windows.Forms.TabPage trackTab;
        private System.Windows.Forms.TabPage releaseTab;
        private System.Windows.Forms.TabPage releaseGroupTab;
        private System.Windows.Forms.CheckedListBox trackListBox;
        private System.Windows.Forms.CheckedListBox releaseListBox;
        private System.Windows.Forms.CheckedListBox releaseGroupListBox;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tagBindingPage;
        private System.Windows.Forms.TabPage findReplacePage;
        private System.Windows.Forms.TableLayoutPanel tagBindingLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.DataGridView findReplaceTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn findColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn replaceColumn;
        private System.Windows.Forms.TabPage otherSettingsPage;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label tagSubmitModeLabel;
        private System.Windows.Forms.RadioButton appendRadioButton;
        private System.Windows.Forms.RadioButton replaceRadioButton;
        private System.Windows.Forms.Label tagSubmitNote;
        private System.Windows.Forms.Label importExportLabel;
        private System.Windows.Forms.LinkLabel exportLink;
        private System.Windows.Forms.LinkLabel importLink;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label resetTitle;
        private System.Windows.Forms.Label resetDescription;
        private System.Windows.Forms.LinkLabel resetLink;
        private System.Windows.Forms.TabPage tagDownloadTab;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox sepByEntityCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label genreConfigTitle;
        private System.Windows.Forms.CheckBox genreDownloadCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label fieldConfigTitle;
        private System.Windows.Forms.Label recordingTagLabel;
        private System.Windows.Forms.ComboBox recordingTagComboBox;
        private System.Windows.Forms.Label recordingGenreLabel;
        private System.Windows.Forms.ComboBox recordingGenreComboBox;
        private System.Windows.Forms.Label releaseGenreLabel;
        private System.Windows.Forms.ComboBox releaseGenreComboBox;
        private System.Windows.Forms.Label releaseTagLabel;
        private System.Windows.Forms.ComboBox releaseTagComboBox;
        private System.Windows.Forms.Label releaseGroupGenreLabel;
        private System.Windows.Forms.ComboBox releaseGroupGenreComboBox;
        private System.Windows.Forms.Label releaseGroupTagLabel;
        private System.Windows.Forms.ComboBox releaseGroupTagComboBox;
        private System.Windows.Forms.Panel otherTypePanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label userTagConfigTitle;
        private System.Windows.Forms.CheckBox userTagsCheckBox;
    }
}