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
            this.resetButton = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tagBindingPage = new System.Windows.Forms.TabPage();
            this.tagBindingLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
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
            this.separateCheckBox.Location = new System.Drawing.Point(6, 6);
            this.separateCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.separateCheckBox.Name = "separateCheckBox";
            this.separateCheckBox.Size = new System.Drawing.Size(558, 29);
            this.separateCheckBox.TabIndex = 0;
            this.separateCheckBox.Text = "Submit separate tags to releases and release groups.             ";
            this.separateCheckBox.UseVisualStyleBackColor = true;
            this.separateCheckBox.CheckedChanged += new System.EventHandler(this.separateCheckBox_CheckedChanged);
            // 
            // separateHelperLabel
            // 
            this.separateHelperLabel.AutoSize = true;
            this.separateHelperLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.separateHelperLabel.Location = new System.Drawing.Point(6, 41);
            this.separateHelperLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.separateHelperLabel.Name = "separateHelperLabel";
            this.separateHelperLabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.separateHelperLabel.Size = new System.Drawing.Size(545, 62);
            this.separateHelperLabel.TabIndex = 1;
            this.separateHelperLabel.Text = "Useful if you use separate tags for whole releases (e.g. album genres)";
            // 
            // tagGroupBox
            // 
            this.tagGroupBox.Controls.Add(this.tagTabControl);
            this.tagGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagGroupBox.Location = new System.Drawing.Point(4, 107);
            this.tagGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tagGroupBox.Name = "tagGroupBox";
            this.tagGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tagGroupBox.Size = new System.Drawing.Size(562, 617);
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
            this.tagTabControl.Location = new System.Drawing.Point(4, 28);
            this.tagTabControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tagTabControl.Name = "tagTabControl";
            this.tagTabControl.SelectedIndex = 0;
            this.tagTabControl.Size = new System.Drawing.Size(554, 585);
            this.tagTabControl.TabIndex = 0;
            // 
            // trackTab
            // 
            this.trackTab.Controls.Add(this.trackListBox);
            this.trackTab.Location = new System.Drawing.Point(8, 39);
            this.trackTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trackTab.Name = "trackTab";
            this.trackTab.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trackTab.Size = new System.Drawing.Size(538, 538);
            this.trackTab.TabIndex = 0;
            this.trackTab.Text = "Tracks/Recordings";
            this.trackTab.UseVisualStyleBackColor = true;
            // 
            // trackListBox
            // 
            this.trackListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackListBox.FormattingEnabled = true;
            this.trackListBox.Location = new System.Drawing.Point(6, 6);
            this.trackListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trackListBox.Name = "trackListBox";
            this.trackListBox.Size = new System.Drawing.Size(526, 526);
            this.trackListBox.TabIndex = 0;
            this.trackListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.trackListBox_ItemCheck);
            // 
            // releaseTab
            // 
            this.releaseTab.Controls.Add(this.releaseListBox);
            this.releaseTab.Location = new System.Drawing.Point(8, 39);
            this.releaseTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseTab.Name = "releaseTab";
            this.releaseTab.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseTab.Size = new System.Drawing.Size(538, 538);
            this.releaseTab.TabIndex = 1;
            this.releaseTab.Text = "Releases";
            this.releaseTab.UseVisualStyleBackColor = true;
            // 
            // releaseListBox
            // 
            this.releaseListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.releaseListBox.FormattingEnabled = true;
            this.releaseListBox.Location = new System.Drawing.Point(6, 6);
            this.releaseListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseListBox.Name = "releaseListBox";
            this.releaseListBox.Size = new System.Drawing.Size(526, 526);
            this.releaseListBox.TabIndex = 0;
            this.releaseListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.releaseListBox_ItemCheck);
            // 
            // releaseGroupTab
            // 
            this.releaseGroupTab.Controls.Add(this.releaseGroupListBox);
            this.releaseGroupTab.Location = new System.Drawing.Point(8, 39);
            this.releaseGroupTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseGroupTab.Name = "releaseGroupTab";
            this.releaseGroupTab.Size = new System.Drawing.Size(538, 538);
            this.releaseGroupTab.TabIndex = 2;
            this.releaseGroupTab.Text = "Release Groups";
            this.releaseGroupTab.UseVisualStyleBackColor = true;
            // 
            // releaseGroupListBox
            // 
            this.releaseGroupListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.releaseGroupListBox.FormattingEnabled = true;
            this.releaseGroupListBox.Location = new System.Drawing.Point(0, 0);
            this.releaseGroupListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseGroupListBox.Name = "releaseGroupListBox";
            this.releaseGroupListBox.Size = new System.Drawing.Size(538, 538);
            this.releaseGroupListBox.TabIndex = 0;
            this.releaseGroupListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.releaseGroupListBox_ItemCheck);
            // 
            // saveNote
            // 
            this.saveNote.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.saveNote.AutoSize = true;
            this.saveNote.Location = new System.Drawing.Point(95, 56);
            this.saveNote.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.saveNote.Name = "saveNote";
            this.saveNote.Size = new System.Drawing.Size(497, 50);
            this.saveNote.TabIndex = 6;
            this.saveNote.Text = "Settings are automatically saved upon clicking OK, independent from MusicBee.";
            this.saveNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonPanel
            // 
            this.buttonPanel.AutoSize = true;
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Controls.Add(this.OKButton);
            this.buttonPanel.Controls.Add(this.resetButton);
            this.buttonPanel.Controls.Add(this.saveNote);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(6, 805);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(598, 106);
            this.buttonPanel.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(442, 6);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 44);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(280, 6);
            this.OKButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(150, 44);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(118, 6);
            this.resetButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(150, 44);
            this.resetButton.TabIndex = 2;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tagBindingPage);
            this.mainTabControl.Controls.Add(this.findReplacePage);
            this.mainTabControl.Controls.Add(this.otherSettingsPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(6, 6);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(598, 787);
            this.mainTabControl.TabIndex = 1;
            // 
            // tagBindingPage
            // 
            this.tagBindingPage.Controls.Add(this.tagBindingLayoutPanel);
            this.tagBindingPage.Location = new System.Drawing.Point(8, 39);
            this.tagBindingPage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tagBindingPage.Name = "tagBindingPage";
            this.tagBindingPage.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tagBindingPage.Size = new System.Drawing.Size(582, 740);
            this.tagBindingPage.TabIndex = 0;
            this.tagBindingPage.Text = "Tag Binding";
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
            this.tagBindingLayoutPanel.Location = new System.Drawing.Point(6, 6);
            this.tagBindingLayoutPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tagBindingLayoutPanel.Name = "tagBindingLayoutPanel";
            this.tagBindingLayoutPanel.RowCount = 5;
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.Size = new System.Drawing.Size(570, 728);
            this.tagBindingLayoutPanel.TabIndex = 1;
            // 
            // findReplacePage
            // 
            this.findReplacePage.Controls.Add(this.findReplaceTable);
            this.findReplacePage.Location = new System.Drawing.Point(8, 39);
            this.findReplacePage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.findReplacePage.Name = "findReplacePage";
            this.findReplacePage.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.findReplacePage.Size = new System.Drawing.Size(582, 647);
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
            this.findReplaceTable.Location = new System.Drawing.Point(6, 6);
            this.findReplaceTable.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.findReplaceTable.Name = "findReplaceTable";
            this.findReplaceTable.RowHeadersWidth = 82;
            this.findReplaceTable.Size = new System.Drawing.Size(570, 635);
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
            this.otherSettingsPage.Location = new System.Drawing.Point(8, 39);
            this.otherSettingsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.otherSettingsPage.Name = "otherSettingsPage";
            this.otherSettingsPage.Size = new System.Drawing.Size(582, 740);
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
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(582, 740);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tagSubmitModeLabel
            // 
            this.tagSubmitModeLabel.AutoSize = true;
            this.tagSubmitModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSubmitModeLabel.Location = new System.Drawing.Point(16, 10);
            this.tagSubmitModeLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.tagSubmitModeLabel.Name = "tagSubmitModeLabel";
            this.tagSubmitModeLabel.Size = new System.Drawing.Size(244, 26);
            this.tagSubmitModeLabel.TabIndex = 0;
            this.tagSubmitModeLabel.Text = "Tag submission mode";
            // 
            // appendRadioButton
            // 
            this.appendRadioButton.AutoSize = true;
            this.appendRadioButton.Location = new System.Drawing.Point(16, 42);
            this.appendRadioButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.appendRadioButton.Name = "appendRadioButton";
            this.appendRadioButton.Size = new System.Drawing.Size(469, 29);
            this.appendRadioButton.TabIndex = 1;
            this.appendRadioButton.TabStop = true;
            this.appendRadioButton.Text = "Append tags to existing tags on MusicBrainz";
            this.appendRadioButton.UseVisualStyleBackColor = true;
            // 
            // replaceRadioButton
            // 
            this.replaceRadioButton.AutoSize = true;
            this.replaceRadioButton.Location = new System.Drawing.Point(16, 83);
            this.replaceRadioButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.replaceRadioButton.Name = "replaceRadioButton";
            this.replaceRadioButton.Size = new System.Drawing.Size(583, 29);
            this.replaceRadioButton.TabIndex = 2;
            this.replaceRadioButton.TabStop = true;
            this.replaceRadioButton.Text = "Clear existing tags when submitting tags to MusicBrainz.";
            this.replaceRadioButton.UseVisualStyleBackColor = true;
            // 
            // tagSubmitNote
            // 
            this.tagSubmitNote.AutoSize = true;
            this.tagSubmitNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSubmitNote.Location = new System.Drawing.Point(16, 118);
            this.tagSubmitNote.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.tagSubmitNote.Name = "tagSubmitNote";
            this.tagSubmitNote.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tagSubmitNote.Size = new System.Drawing.Size(580, 218);
            this.tagSubmitNote.TabIndex = 3;
            this.tagSubmitNote.Text = resources.GetString("tagSubmitNote.Text");
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.buttonPanel, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.mainTabControl, 0, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayoutPanel.Size = new System.Drawing.Size(610, 917);
            this.mainLayoutPanel.TabIndex = 6;
            // 
            // TagBindingConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(610, 917);
            this.Controls.Add(this.mainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
        private System.Windows.Forms.Button resetButton;
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
    }
}