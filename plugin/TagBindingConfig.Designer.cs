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
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.findColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.replaceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // separateCheckBox
            // 
            this.separateCheckBox.AutoSize = true;
            this.separateCheckBox.Location = new System.Drawing.Point(3, 3);
            this.separateCheckBox.Name = "separateCheckBox";
            this.separateCheckBox.Size = new System.Drawing.Size(279, 17);
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
            this.separateHelperLabel.Size = new System.Drawing.Size(267, 32);
            this.separateHelperLabel.TabIndex = 1;
            this.separateHelperLabel.Text = "Useful if you use separate tags for whole releases (e.g. album genres)";
            // 
            // tagGroupBox
            // 
            this.tagGroupBox.Controls.Add(this.tagTabControl);
            this.tagGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagGroupBox.Location = new System.Drawing.Point(2, 57);
            this.tagGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.tagGroupBox.Name = "tagGroupBox";
            this.tagGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.tagGroupBox.Size = new System.Drawing.Size(281, 319);
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
            this.tagTabControl.Size = new System.Drawing.Size(277, 302);
            this.tagTabControl.TabIndex = 0;
            // 
            // trackTab
            // 
            this.trackTab.Controls.Add(this.trackListBox);
            this.trackTab.Location = new System.Drawing.Point(4, 22);
            this.trackTab.Name = "trackTab";
            this.trackTab.Padding = new System.Windows.Forms.Padding(3);
            this.trackTab.Size = new System.Drawing.Size(269, 276);
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
            this.trackListBox.Size = new System.Drawing.Size(263, 270);
            this.trackListBox.TabIndex = 0;
            // 
            // releaseTab
            // 
            this.releaseTab.Controls.Add(this.releaseListBox);
            this.releaseTab.Location = new System.Drawing.Point(4, 22);
            this.releaseTab.Name = "releaseTab";
            this.releaseTab.Padding = new System.Windows.Forms.Padding(3);
            this.releaseTab.Size = new System.Drawing.Size(269, 276);
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
            this.releaseListBox.Size = new System.Drawing.Size(263, 270);
            this.releaseListBox.TabIndex = 0;
            // 
            // releaseGroupTab
            // 
            this.releaseGroupTab.Controls.Add(this.releaseGroupListBox);
            this.releaseGroupTab.Location = new System.Drawing.Point(4, 22);
            this.releaseGroupTab.Name = "releaseGroupTab";
            this.releaseGroupTab.Size = new System.Drawing.Size(269, 276);
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
            this.releaseGroupListBox.Size = new System.Drawing.Size(269, 276);
            this.releaseGroupListBox.TabIndex = 0;
            // 
            // saveNote
            // 
            this.saveNote.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.saveNote.AutoSize = true;
            this.saveNote.Location = new System.Drawing.Point(50, 29);
            this.saveNote.Name = "saveNote";
            this.saveNote.Size = new System.Drawing.Size(246, 26);
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
            this.buttonPanel.Location = new System.Drawing.Point(3, 419);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(299, 55);
            this.buttonPanel.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(221, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(140, 3);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(59, 3);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 2;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tagBindingPage);
            this.mainTabControl.Controls.Add(this.findReplacePage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(3, 3);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(299, 410);
            this.mainTabControl.TabIndex = 1;
            // 
            // tagBindingPage
            // 
            this.tagBindingPage.Controls.Add(this.tagBindingLayoutPanel);
            this.tagBindingPage.Location = new System.Drawing.Point(4, 22);
            this.tagBindingPage.Name = "tagBindingPage";
            this.tagBindingPage.Padding = new System.Windows.Forms.Padding(3);
            this.tagBindingPage.Size = new System.Drawing.Size(291, 384);
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
            this.tagBindingLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tagBindingLayoutPanel.Name = "tagBindingLayoutPanel";
            this.tagBindingLayoutPanel.RowCount = 5;
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tagBindingLayoutPanel.Size = new System.Drawing.Size(285, 378);
            this.tagBindingLayoutPanel.TabIndex = 1;
            // 
            // findReplacePage
            // 
            this.findReplacePage.Controls.Add(this.findReplaceTable);
            this.findReplacePage.Location = new System.Drawing.Point(4, 22);
            this.findReplacePage.Name = "findReplacePage";
            this.findReplacePage.Padding = new System.Windows.Forms.Padding(3);
            this.findReplacePage.Size = new System.Drawing.Size(291, 384);
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
            this.findReplaceTable.Size = new System.Drawing.Size(285, 378);
            this.findReplaceTable.TabIndex = 0;
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
            this.mainLayoutPanel.Size = new System.Drawing.Size(305, 477);
            this.mainLayoutPanel.TabIndex = 6;
            // 
            // findColumn
            // 
            this.findColumn.HeaderText = "Find (in tags)";
            this.findColumn.Name = "findColumn";
            // 
            // replaceColumn
            // 
            this.replaceColumn.HeaderText = "Replace (for MusicBrainz)";
            this.replaceColumn.Name = "replaceColumn";
            // 
            // TagBindingConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(305, 477);
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
            this.findReplacePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.findReplaceTable)).EndInit();
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
    }
}