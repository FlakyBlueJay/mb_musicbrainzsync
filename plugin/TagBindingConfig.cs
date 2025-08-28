using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using plugin.Properties;
using static MusicBeePlugin.Plugin;

namespace plugin
{
    public partial class TagBindingConfig : Form
    {
        public TagBindingConfig()
        {
            InitializeComponent();
        }


        public class TagListItem
        {
            public string tagDisplayName { get; set; }
            public string tagKey { get; set; }
            public override string ToString()
            {
                return tagDisplayName;
            }
        }

        private void UpdateTagBindingList(CheckedListBox listBox, string entity_type = null)
        {
            listBox.Items.Clear();
            HashSet<string> TagBindingsSetting;

            switch (entity_type)
            {
                case "release":
                    TagBindingsSetting = Settings.Default.releaseTagBindings.Split(';').ToHashSet();
                    break;
                case "release-group":
                    TagBindingsSetting = Settings.Default.releaseGroupTagBindings.Split(';').ToHashSet();
                    break;
                default:
                    TagBindingsSetting = Settings.Default.recordingTagBindings.Split(';').ToHashSet();
                    break;
            }
            foreach (KeyValuePair<string, MetaDataType> tagBindingPair in MusicBeePlugin.Plugin.listTagBindings)
            {
                string tagName = mbApiInterface.Setting_GetFieldName(tagBindingPair.Value);
                var tagKey = tagBindingPair.Key;
                bool itemChecked = TagBindingsSetting.Contains(tagKey);
                listBox.Items.Add(new TagListItem { tagDisplayName = tagName, tagKey = tagKey }, itemChecked);

            }
        }

        private void TagBindingConfig_Load(object sender, EventArgs e)
        {
            this.separateCheckBox.Checked = Settings.Default.separateTagBindings;
            ToggleSeparateBindUI(sender, e, Settings.Default.separateTagBindings);
            UpdateFindReplaceTable(); UpdateTagModeRadioButtons();
        }

        // UI functions
        private void ToggleSeparateBindUI(object sender, EventArgs e, bool separate = false)
        {
            UpdateTagBindingList(trackListBox);
            // god I hate WinForms.
            if (separate)
            {
                trackTab.Text = "Tracks/Recordings";
                if (!tagTabControl.TabPages.Contains(releaseTab))
                {
                    tagTabControl.TabPages.Insert(1, releaseTab);
                }
                if (!tagTabControl.TabPages.Contains(releaseGroupTab))
                {
                    tagTabControl.TabPages.Insert(2, releaseGroupTab);
                }
                UpdateTagBindingList(releaseListBox, "release");
                UpdateTagBindingList(releaseGroupListBox, "release-group");

            }
            else
            {
                trackTab.Text = "Global tag bindings";
                tagTabControl.SelectTab(trackTab);
                tagTabControl.TabPages.Remove(tagTabControl.TabPages["releaseGroupTab"]);
                tagTabControl.TabPages.Remove(tagTabControl.TabPages["releaseTab"]);
            }
        }

        private void UpdateFindReplaceTable()
        {
            findReplaceTable.Rows.Clear();
            // split by new-line to get the individual pairings
            foreach (string line in Settings.Default.findReplace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                // split by the first semicolon to get the find and replace values
                string[] row = line.Split(new[] { ';' }, 2);
                if (row.Length == 2)
                {
                    findReplaceTable.Rows.Add(row[0], row[1]);
                }
            }
        }

        private void UpdateTagModeRadioButtons()
        {
            if (Settings.Default.tagSubmitIsDestructive)
            {
                replaceRadioButton.Checked = true;
            }
            else
            {
                appendRadioButton.Checked = true;
            }
        }

        private void separateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToggleSeparateBindUI(sender, e, separateCheckBox.Checked);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HandleListBoxCheck(object sender, ItemCheckEventArgs listBoxItem)
        {
            CheckedListBox listBox = (CheckedListBox)sender;

            int checkedItemCount = listBox.CheckedItems.Count;
            if (listBoxItem.NewValue == CheckState.Checked)
            {
                checkedItemCount++;
            }
            else
            {
                checkedItemCount--;
            }

            if (checkedItemCount == 0)
            {
                listBoxItem.NewValue = CheckState.Checked;
            }
        }

        private void trackListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            HandleListBoxCheck(sender, e);
        }

        private void releaseListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            HandleListBoxCheck(sender, e);
        }

        private void releaseGroupListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            HandleListBoxCheck(sender, e);
        }

        // settings save functions
        private void SaveChanges()
        {
            List<string> trackSavedTags = new List<string>();
            foreach (object tag in trackListBox.CheckedItems)
            {
                TagListItem tagListItem = (TagListItem)tag;
                trackSavedTags.Add(tagListItem.tagKey);
            }
            Debug.WriteLine(string.Join(";", trackSavedTags));

            Properties.Settings.Default.recordingTagBindings = string.Join(";", trackSavedTags);
            Properties.Settings.Default.separateTagBindings = separateCheckBox.Checked;

            // this setting could have been changed so trust the check box over the setting. Either way we'll save the check box state afterwards.
            if (separateCheckBox.Checked)
            {
                List<string> releaseSavedTags = new List<string>();
                List<string> releaseGroupSavedTags = new List<string>();
                foreach (object tag in releaseListBox.CheckedItems)
                {
                    TagListItem tagListItem = (TagListItem)tag;
                    releaseSavedTags.Add(tagListItem.tagKey);
                }
                foreach (object tag in releaseGroupListBox.CheckedItems)
                {
                    TagListItem tagListItem = (TagListItem)tag;
                    releaseGroupSavedTags.Add(tagListItem.tagKey);
                }
                Properties.Settings.Default.releaseTagBindings = string.Join(";", releaseSavedTags);
                Properties.Settings.Default.releaseGroupTagBindings = string.Join(";", releaseGroupSavedTags);

            }

            string newFindReplaceString = "";
            foreach (DataGridViewRow row in findReplaceTable.Rows)
            {
                // get both tags in lowercase. makes it easier to compare, and MusicBrainz doesn't care either: tags are always processed lowercase.
                string findTag = row.Cells[0].Value?.ToString().ToLower();
                string replaceTag = row.Cells[1].Value?.ToString().ToLower();
                if (findTag != null && replaceTag != null)
                {
                    newFindReplaceString += $"{findTag};{replaceTag}{Environment.NewLine}";
                }

            }
            Settings.Default.findReplace = newFindReplaceString;

            if (appendRadioButton.Checked)
            {
                Settings.Default.tagSubmitIsDestructive = false;
            }
            else
            {
                Settings.Default.tagSubmitIsDestructive = true;
            }

            Settings.Default.Save();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
            this.Close();

        }

        private void applyButton_click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        // Export and import functions

        // These classes are used to structure the settings for export/import
        public class TagBindings
        {
            public string recording { get; set; }
            public string release { get; set; }
            public string release_group { get; set; }
        }

        public class PluginSettings
        {
            public bool separate_tag_bindings { get; set; }
            public bool tag_submit_destructive { get; set; }
            public string find_replace { get; set; }
            public TagBindings tag_bindings { get; set; }
        }

        private void exportLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JavaScript Object Notation file|*.json";
            saveDialog.Title = "Export MusicBrainz Sync plugin settings";
            saveDialog.FileName = "MusicBrainzSyncSettings.json";
            DialogResult result = saveDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    Debug.WriteLine("tagSubmitIsDestructive: " + Settings.Default.tagSubmitIsDestructive);
                    PluginSettings exportedSettings = new PluginSettings
                    {
                        separate_tag_bindings = Settings.Default.separateTagBindings,
                        tag_submit_destructive = Settings.Default.tagSubmitIsDestructive,
                        find_replace = Settings.Default.findReplace,
                        tag_bindings = new TagBindings
                        {
                            recording = Settings.Default.recordingTagBindings,
                            release = Settings.Default.releaseTagBindings,
                            release_group = Settings.Default.releaseGroupTagBindings
                        },
                    };
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(exportedSettings, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(saveDialog.FileName, json);
                    MessageBox.Show($"Settings have been successfully exported to {saveDialog.FileName}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while exporting the settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void importLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "JavaScript Object Notation file|*.json";
            openDialog.Title = "Import MusicBrainz Sync plugin settings";
            openDialog.FileName = "MusicBrainzSyncSettings.json";
            DialogResult result = openDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    // 1. get data from JSON import
                    if (new System.IO.FileInfo(openDialog.FileName).Extension != ".json")
                    {
                        throw new InvalidOperationException("The selected file does not appear to be a valid JSON file.");
                    }
                    else
                    {
                        string json = System.IO.File.ReadAllText(openDialog.FileName);
                        PluginSettings importedSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<PluginSettings>(json);
                        if (importedSettings == null || importedSettings.tag_bindings == null)
                        {
                            throw new InvalidOperationException("The selected file does not appear to be a valid MusicBrainz Sync settings file.");
                        }
                        else
                        {
                            Settings.Default.separateTagBindings = importedSettings.separate_tag_bindings;
                            Settings.Default.tagSubmitIsDestructive = importedSettings.tag_submit_destructive;
                            Settings.Default.findReplace = importedSettings.find_replace ?? "";
                            Settings.Default.recordingTagBindings = importedSettings.tag_bindings.recording ?? "";
                            Settings.Default.releaseTagBindings = importedSettings.tag_bindings.release ?? "";
                            Settings.Default.releaseGroupTagBindings = importedSettings.tag_bindings.release_group ?? "";
                            Settings.Default.Save();
                            separateCheckBox.Checked = Settings.Default.separateTagBindings;
                            UpdateFindReplaceTable();
                            UpdateTagModeRadioButtons();
                            MessageBox.Show("Settings have been successfully imported. Please review the settings and click OK to apply them.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // 2. process the data.
                        // 3. if all is well, save the data to settings and update the UI.
                        //MessageBox.Show("Importing settings is not yet implemented.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }  
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    MessageBox.Show("The selected file does not appear to be a valid JSON file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while importing the settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void resetLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult resetMessageResult = MessageBox.Show("Are you sure you want to reset the tag binding settings to default? This will remove all custom tag bindings, set them to the default values and default back to sharing the tag bindings across all entities.", null, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resetMessageResult == DialogResult.Yes)
            {
                //Settings.Default.separateTagBindings = Convert.ToBoolean(Settings.Default.Properties["separateTagBindings"].DefaultValue);
                Settings.Default.recordingTagBindings = (string)Settings.Default.Properties["recordingTagBindings"].DefaultValue;
                Settings.Default.releaseTagBindings = (string)Settings.Default.Properties["releaseTagBindings"].DefaultValue;
                Settings.Default.releaseGroupTagBindings = (string)Settings.Default.Properties["releaseTagBindings"].DefaultValue;
                Settings.Default.findReplace = (string)Settings.Default.Properties["findReplace"].DefaultValue;
                Settings.Default.tagSubmitIsDestructive = Convert.ToBoolean(Settings.Default.Properties["tagSubmitIsDestructive"].DefaultValue);
                Settings.Default.Save();
                separateCheckBox.Checked = Convert.ToBoolean(Settings.Default.Properties["separateTagBindings"].DefaultValue);
                UpdateFindReplaceTable(); UpdateTagModeRadioButtons();
                MessageBox.Show("Your MusicBrainz Sync plugin settings have been reset to default values.", null, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
