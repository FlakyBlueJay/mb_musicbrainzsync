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
            this.mainPanel = new System.Windows.Forms.FlowLayoutPanel();
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
            this.mainPanel.SuspendLayout();
            this.tagGroupBox.SuspendLayout();
            this.tagTabControl.SuspendLayout();
            this.trackTab.SuspendLayout();
            this.releaseTab.SuspendLayout();
            this.releaseGroupTab.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoSize = true;
            this.mainPanel.Controls.Add(this.separateCheckBox);
            this.mainPanel.Controls.Add(this.separateHelperLabel);
            this.mainPanel.Controls.Add(this.tagGroupBox);
            this.mainPanel.Controls.Add(this.saveNote);
            this.mainPanel.Controls.Add(this.buttonPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mainPanel.Size = new System.Drawing.Size(806, 619);
            this.mainPanel.TabIndex = 0;
            // 
            // separateCheckBox
            // 
            this.separateCheckBox.AutoSize = true;
            this.separateCheckBox.Location = new System.Drawing.Point(12, 12);
            this.separateCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.separateCheckBox.Name = "separateCheckBox";
            this.separateCheckBox.Size = new System.Drawing.Size(634, 29);
            this.separateCheckBox.TabIndex = 0;
            this.separateCheckBox.Text = "Submit separate tags to releases and release groups.             ";
            this.separateCheckBox.UseVisualStyleBackColor = true;
            this.separateCheckBox.CheckedChanged += new System.EventHandler(this.separateCheckBox_CheckedChanged);
            // 
            // separateHelperLabel
            // 
            this.separateHelperLabel.AutoSize = true;
            this.separateHelperLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.separateHelperLabel.Location = new System.Drawing.Point(12, 47);
            this.separateHelperLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.separateHelperLabel.Name = "separateHelperLabel";
            this.separateHelperLabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.separateHelperLabel.Size = new System.Drawing.Size(682, 37);
            this.separateHelperLabel.TabIndex = 1;
            this.separateHelperLabel.Text = "Useful if you use separate tags for whole releases (e.g. album genres)";
            // 
            // tagGroupBox
            // 
            this.tagGroupBox.Controls.Add(this.tagTabControl);
            this.tagGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagGroupBox.Location = new System.Drawing.Point(10, 88);
            this.tagGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tagGroupBox.Name = "tagGroupBox";
            this.tagGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tagGroupBox.Size = new System.Drawing.Size(781, 313);
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
            this.tagTabControl.Size = new System.Drawing.Size(773, 281);
            this.tagTabControl.TabIndex = 0;
            // 
            // trackTab
            // 
            this.trackTab.Controls.Add(this.trackListBox);
            this.trackTab.Location = new System.Drawing.Point(8, 39);
            this.trackTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trackTab.Name = "trackTab";
            this.trackTab.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trackTab.Size = new System.Drawing.Size(757, 234);
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
            this.trackListBox.Size = new System.Drawing.Size(745, 222);
            this.trackListBox.TabIndex = 0;
            // 
            // releaseTab
            // 
            this.releaseTab.Controls.Add(this.releaseListBox);
            this.releaseTab.Location = new System.Drawing.Point(8, 39);
            this.releaseTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseTab.Name = "releaseTab";
            this.releaseTab.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseTab.Size = new System.Drawing.Size(757, 234);
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
            this.releaseListBox.Size = new System.Drawing.Size(745, 222);
            this.releaseListBox.TabIndex = 0;
            // 
            // releaseGroupTab
            // 
            this.releaseGroupTab.Controls.Add(this.releaseGroupListBox);
            this.releaseGroupTab.Location = new System.Drawing.Point(8, 39);
            this.releaseGroupTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.releaseGroupTab.Name = "releaseGroupTab";
            this.releaseGroupTab.Size = new System.Drawing.Size(757, 234);
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
            this.releaseGroupListBox.Size = new System.Drawing.Size(757, 234);
            this.releaseGroupListBox.TabIndex = 0;
            // 
            // saveNote
            // 
            this.saveNote.AutoSize = true;
            this.saveNote.Location = new System.Drawing.Point(12, 405);
            this.saveNote.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.saveNote.Name = "saveNote";
            this.saveNote.Size = new System.Drawing.Size(777, 25);
            this.saveNote.TabIndex = 6;
            this.saveNote.Text = "Settings are automatically saved upon clicking OK, independent from MusicBee.";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Controls.Add(this.OKButton);
            this.buttonPanel.Controls.Add(this.resetButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(12, 436);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(777, 62);
            this.buttonPanel.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(621, 6);
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
            this.OKButton.Location = new System.Drawing.Point(459, 6);
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
            this.resetButton.Location = new System.Drawing.Point(297, 6);
            this.resetButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(150, 44);
            this.resetButton.TabIndex = 2;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // TagBindingConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(806, 619);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TagBindingConfig";
            this.Text = "MusicBrainz Sync - Tag Binding Configuration";
            this.Load += new System.EventHandler(this.TagBindingConfig_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.tagGroupBox.ResumeLayout(false);
            this.tagTabControl.ResumeLayout(false);
            this.trackTab.ResumeLayout(false);
            this.releaseTab.ResumeLayout(false);
            this.releaseGroupTab.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainPanel;
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
    }
}