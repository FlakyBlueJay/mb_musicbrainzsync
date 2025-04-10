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

        // This is the list of tag bindings that are used in the plugin.
        // The key is how the plugin will call it internally, the value is the name the user will see..
        // This is done this way so custom tags can be renamed if needed.
        // Right now I don't think that's exposed in the MusicBee API though...

        private Dictionary<string, string> tagBindings = new Dictionary<string, string>
        {
            {"genres", "Genre" },
            {"mood", "Mood" },
            {"occasion", "Occasion" },
            {"keywords", "Keywords" },
            {"custom1", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom1) },
            {"custom2", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom2) },
            {"custom3", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom3) },
            {"custom4", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom4) },
            {"custom5", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom5) },
            {"custom6", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom6) },
            {"custom7", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom7) },
            {"custom8", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom8) },
            {"custom9", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom9) },
            {"custom10", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom10) },
            {"custom11", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom11) },
            {"custom12", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom12) },
            {"custom13", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom13) },
            {"custom14", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom14) },
            {"custom15", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom15) },
            {"custom16", mbApiInterface.Setting_GetFieldName(MetaDataType.Custom16) }
        };

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
            foreach (KeyValuePair<string, string> tagBindingPair in tagBindings)
            {
                var tagName = tagBindingPair.Value;
                var tagKey = tagBindingPair.Key;
                bool itemChecked = TagBindingsSetting.Contains(tagKey);
                listBox.Items.Add(new TagListItem { tagDisplayName = tagName, tagKey = tagKey}, itemChecked);

            }
        }

        private void TagBindingConfig_Load(object sender, EventArgs e)
        {
            this.separateCheckBox.Checked = Settings.Default.separateTagBindings;
            ToggleSeparateBindUI(sender, e, Settings.Default.separateTagBindings);

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
                Settings.Default.recordingTagBindings = (string)Settings.Default.Properties["recordingTagBindings"].DefaultValue;
                Settings.Default.releaseTagBindings = (string)Settings.Default.Properties["releaseTagBindings"].DefaultValue;
                Settings.Default.releaseGroupTagBindings = (string)Settings.Default.Properties["releaseTagBindings"].DefaultValue;
                Settings.Default.Save();
                separateCheckBox.Checked = false;
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
            Properties.Settings.Default.Save();
            this.Close();

        }
    }
}
