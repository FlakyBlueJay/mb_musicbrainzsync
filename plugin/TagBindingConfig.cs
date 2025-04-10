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
                listBox.Items.Add(new TagListItem { tagDisplayName = tagName, tagKey = tagKey}, itemChecked);

            }
        }

        private void TagBindingConfig_Load(object sender, EventArgs e)
        {
            this.separateCheckBox.Checked = Settings.Default.separateTagBindings;
            ToggleSeparateBindUI(sender, e, Settings.Default.separateTagBindings);
            UpdateFindReplaceTable();
        }

        private void ToggleSeparateBindUI(object sender, EventArgs e, bool separate = false)
        {
            UpdateTagBindingList(trackListBox);
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

        private void separateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToggleSeparateBindUI(sender, e, separateCheckBox.Checked);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            DialogResult resetMessageResult = MessageBox.Show("Are you sure you want to reset the tag binding settings to default? This will remove all custom tag bindings, set them to the default values and default back to sharing the tag bindings across all entities.", null, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resetMessageResult == DialogResult.Yes)
            {
                Debug.WriteLine("hhhhh"+Convert.ToBoolean(Settings.Default.Properties["separateTagBindings"].DefaultValue));
                //Settings.Default.separateTagBindings = Convert.ToBoolean(Settings.Default.Properties["separateTagBindings"].DefaultValue);
                Settings.Default.recordingTagBindings = (string)Settings.Default.Properties["recordingTagBindings"].DefaultValue;
                Settings.Default.releaseTagBindings = (string)Settings.Default.Properties["releaseTagBindings"].DefaultValue;
                Settings.Default.releaseGroupTagBindings = (string)Settings.Default.Properties["releaseTagBindings"].DefaultValue;
                Settings.Default.findReplace = (string)Settings.Default.Properties["findReplace"].DefaultValue;
                Settings.Default.Save();
                separateCheckBox.Checked = Convert.ToBoolean(Settings.Default.Properties["separateTagBindings"].DefaultValue);
                UpdateFindReplaceTable();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            List<string> trackSavedTags = new List<string>();
            foreach (object tag in trackListBox.CheckedItems)
            {
                TagListItem tagListItem = (TagListItem)tag;
                trackSavedTags.Add(tagListItem.tagKey);
            }
            Debug.WriteLine(string.Join(";", trackSavedTags));

            Properties.Settings.Default.separateTagBindings = separateCheckBox.Checked;
            Properties.Settings.Default.recordingTagBindings = string.Join(";", trackSavedTags);

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
            
            Settings.Default.Save();
            this.Close();

        }
    }
}
