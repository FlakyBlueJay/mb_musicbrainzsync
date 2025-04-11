using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YourNamespace
{

    public sealed class LibraryEntryPoint
    {
        private static string DLLDirectory = "";
        private static List<string> DLLDirectoryExpected = new List<string>();
        private static bool isInitialized = false;
        private static readonly object initializationLock = new object();

        public static string libraryDir { get; private set; } = "";

        // This static constructor will be called  when the DLL is loaded
        static LibraryEntryPoint()
        {
            // Use a lock to ensure thread safety
            lock (initializationLock)
            {
                if (!isInitialized)
                {

#if DEBUG
                    // Force exceptions to be in English
                    System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
#endif


                    Assembly thisAssem = typeof(LibraryEntryPoint).Assembly;
#if DEBUG

                    Console.WriteLine($"Loaded {thisAssem.GetName().Name}");
#endif

                    // Setup DLL dependencies
                    libraryDir = Path.GetDirectoryName(thisAssem.Location);

                    string libDepFolder = Path.Combine(libraryDir, thisAssem.GetCustomAttribute<AssemblyTitleAttribute>().Title);

                    SetupDllDependencies(libDepFolder);

                }
            }

            isInitialized = true;
        }

        static public void SetupDllDependencies(string dependencyDirPath)
        {
            DLLDirectory = dependencyDirPath;

            if (Directory.Exists(DLLDirectory))
            {
                DLLDirectoryExpected = Directory.GetFiles(DLLDirectory, "*.dll").Select(f => Path.GetFileName(f)).ToList();
            }

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        static private Assembly ResolveAssembly(Object sender, ResolveEventArgs e)
        {
            Assembly res = null;
            string dllName = $"{e.Name.Split(',')[0]}.dll";
            if (DLLDirectoryExpected.Count > 0 && !DLLDirectoryExpected.Contains(dllName))
            {
                return res;
            }

            string path = Path.Combine(DLLDirectory, dllName);
            try
            {
                res = System.Reflection.Assembly.LoadFile(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load {path}");
                Console.WriteLine(ex.ToString());
            }
            return res;
        }
    }

}

namespace MusicBeePlugin
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using plugin;
    using plugin.Properties;
    using YourNamespace;
    using static plugin.MusicBeeTrack;

    public partial class Plugin
    {
        // Required so entrypoint can be called
        static private LibraryEntryPoint entryPoint = new LibraryEntryPoint();

        // MusicBee API initialisation
        public static MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        // new MusicBrainz API instance
        public MusicBrainzAPI mbz = new MusicBrainzAPI();

        // expose shared controls as public variables, so it can be easily accessed by other functions.
        public TextBox mbzUserInputBox;
        public Label userAuthenticatedLabel;
        public TableLayoutPanel authConfigPanel;
        public TableLayoutPanel postAuthConfigPanel;

        // This is the list of tag bindings that are used in the plugin.

        public static Dictionary<string, MetaDataType> listTagBindings = new Dictionary<string, MetaDataType>
        {
            {"genres",   MetaDataType.Genre },
            {"mood",     MetaDataType.Mood },
            {"occasion", MetaDataType.Occasion },
            {"keywords", MetaDataType.Keywords },
            {"custom1",  MetaDataType.Custom1 },
            {"custom2",  MetaDataType.Custom2 },
            {"custom3",  MetaDataType.Custom3},
            {"custom4",  MetaDataType.Custom4},
            {"custom5",  MetaDataType.Custom5},
            {"custom6",  MetaDataType.Custom6},
            {"custom7",  MetaDataType.Custom7},
            {"custom8",  MetaDataType.Custom8},
            {"custom9",  MetaDataType.Custom9},
            {"custom10", MetaDataType.Custom10},
            {"custom11", MetaDataType.Custom11},
            {"custom12", MetaDataType.Custom12},
            {"custom13", MetaDataType.Custom13},
            {"custom14", MetaDataType.Custom14},
            {"custom15", MetaDataType.Custom15},
            {"custom16", MetaDataType.Custom16}
        };

        // # MusicBee plugin initialisation functions
        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            Assembly thisAssem = typeof(Plugin).Assembly;

            // Change these attributes in the .csproj
            string name = thisAssem.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            Version ver = thisAssem.GetName().Version;
            string author = thisAssem.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            string description = thisAssem.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = name;
            about.Description = description;
            about.Author = author;
            about.TargetApplication = "";   //  the name of a Plugin Storage device or panel header for a dockable panel
            about.Type = PluginType.General;
            about.VersionMajor = (short)ver.Major;  // your plugin version
            about.VersionMinor = (short)ver.Minor;
            about.Revision = (short)ver.Revision;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 145;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            // string dataPath = mbApiInterface.Setting_GetPersistentStoragePath() + "\\mb_MusicBrainzSync";
            // saving in case there are plans to store settings outside of Properties.Settings in the future.

            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {

                Panel mainPanel = (Panel)Panel.FromHandle(panelHandle);
                mainPanel.AutoSize = true;

                // ## panel for authentication - table layouts are used to fix issues with positioning and hiDPI
                authConfigPanel = GenerateTableLayoutPanel(1, 2);

                TableLayoutPanel userInputPanel = GenerateTableLayoutPanel(2, 1);
                TableLayoutPanel linkPanel = GenerateTableLayoutPanel(1, 2);

                // ### sub-panel for user input
                Label mbzUserInputLabel = new Label();
                mbzUserInputLabel.AutoSize = true;
                mbzUserInputLabel.Text = "Access token from MusicBrainz:";
                mbzUserInputLabel.TextAlign = ContentAlignment.MiddleLeft;
                mbzUserInputLabel.Anchor = AnchorStyles.Left; // align the label so it doesn't look misplaced

                mbzUserInputBox = new TextBox();
                mbzUserInputBox.Dock = DockStyle.Fill;
                mbzUserInputBox.ForeColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                   SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                mbzUserInputBox.BackColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                   SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                mbzUserInputBox.BorderStyle = BorderStyle.FixedSingle;
                mbzUserInputBox.Padding = new Padding(0, 0, 0, 5);

                userInputPanel.Controls.Add(mbzUserInputLabel, 0, 0);
                userInputPanel.Controls.Add(mbzUserInputBox, 1, 0);

                // ### sub-panel for links
                LinkLabel mbzVerifyLabel = new LinkLabel();
                mbzVerifyLabel.AutoSize = true;
                mbzVerifyLabel.Text = "Log in to MusicBrainz";
                mbzVerifyLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                mbzVerifyLabel.Padding = new Padding(0, 0, 0, 5);

                LinkLabel mbzAuthLabel = new LinkLabel();
                mbzAuthLabel.AutoSize = true;
                mbzAuthLabel.Text = "Get an access token from MusicBrainz";
                mbzAuthLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                linkPanel.Controls.Add(mbzVerifyLabel, 0, 0);
                linkPanel.Controls.Add(mbzAuthLabel, 0, 1);

                authConfigPanel.Controls.Add(userInputPanel, 0, 0);
                authConfigPanel.Controls.Add(linkPanel, 0, 1);
                mainPanel.Controls.Add(authConfigPanel);

                // ### authentication panel events
                mbzVerifyLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(mbzVerifyLabel_LinkClicked);
                mbzAuthLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(mbzAuthLabel_LinkClicked);

                // ## post-auth / logged in panel
                postAuthConfigPanel = GenerateTableLayoutPanel(1, 3); postAuthConfigPanel.Hide();

                userAuthenticatedLabel = new Label();
                userAuthenticatedLabel.AutoSize = true;
                userAuthenticatedLabel.Text = "Logged in as %USERNAME%";
                userAuthenticatedLabel.Padding = new Padding(0, 0, 0, 5);

                LinkLabel configLinkLabel = new LinkLabel();
                configLinkLabel.AutoSize = true;
                configLinkLabel.Text = "Configure tag bindings";
                configLinkLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                configLinkLabel.Padding = new Padding(0, 0, 0, 5);

                LinkLabel revokeAccessLabel = new LinkLabel();
                revokeAccessLabel.AutoSize = true;
                revokeAccessLabel.Text = "Log out from MusicBrainz";
                revokeAccessLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                postAuthConfigPanel.Controls.Add(userAuthenticatedLabel, 0, 0);
                postAuthConfigPanel.Controls.Add(configLinkLabel, 0, 1);
                postAuthConfigPanel.Controls.Add(revokeAccessLabel, 0, 2);
                mainPanel.Controls.Add(postAuthConfigPanel);

                // ## post-auth panel events
                configLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(tagBindingConfigLabel_LinkClicked);
                revokeAccessLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(revokeAccessLabel_Clicked);

                // Hide the authentication panel if the user is already authenticated to MusicBrainz, or vice versa if not logged in.
                ToggleAuthPanelsVisibility();

            }

            return false;

        }

        // # configure panel UI functions
        private TableLayoutPanel GenerateTableLayoutPanel(int columns, int rows)
        {
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = columns;
            tableLayoutPanel.RowCount = rows;
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;

            for (int i = 0; i < columns; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }

            for (int i = 0; i < rows; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            return tableLayoutPanel;
        }

        private void ToggleAuthPanelsVisibility()
        {
            if (string.IsNullOrEmpty(Settings.Default.refreshToken))
            {
                authConfigPanel.Show(); postAuthConfigPanel.Hide();
            }
            else
            {
                userAuthenticatedLabel.Text = $"Logged in as {mbz.user}";
                authConfigPanel.Hide(); postAuthConfigPanel.Show();
            }
        }

        // # Plugin config panel events
        private void mbzAuthLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // Test this functionality in WINE, too. This should just load the default web browser but, idk.

            string mbzAuthUrl = mbz.GetAuthenticationURL();

            try
            {
                Process.Start(mbzAuthUrl);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // if there is no default web browser or the URL is invalid.
                Clipboard.SetText(mbzAuthUrl);
                MessageBox.Show("Could not open the default web browser. The authentication URL has been copied to your clipboard.\n\n" +
                    "Please copy and paste the URL into your browser.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                // any other errors we haven't caught.
                Clipboard.SetText(mbzAuthUrl);
                MessageBox.Show("An unexpected error occurred. The authentication URL has been copied to your clipboard.\n\n" +
                    "Please copy and paste the URL into your browser.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void mbzVerifyLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mbzUserInputBox.Text))
            {
                bool userAuthenticated = mbz.AuthenticateUser(mbzUserInputBox.Text).Result;
                if (userAuthenticated)
                {
                    userAuthenticatedLabel.Text = $"Logged in as {mbz.user}";
                    ToggleAuthPanelsVisibility();
                }
            }
        }

        private void tagBindingConfigLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // open the tag binding config window
            TagBindingConfig tagBindingConfig = new TagBindingConfig();
            tagBindingConfig.ShowDialog();
        }
        private async void revokeAccessLabel_Clicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            await mbz.RevokeAccess();
            mbzUserInputBox.Clear();
            userAuthenticatedLabel.Text = "Logged in as %USERNAME%";
            ToggleAuthPanelsVisibility();
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    // perform startup initialisation

                    // add context menu items
                    // context.Main is the right-click menu for tracks and albums
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings", "", null);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags", "", null);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Sync Track Ratings", "", SendTrackRatings);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Sync Album Ratings to Release Group", "", SendAlbumGroupRatings);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Recordings", "", SendTrackTags);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Release", "", SendReleaseTags);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Release Group", "", SendReleaseGroupTags);

                    // add hotkey entries
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Track Ratings", SendTrackRatings);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Album Ratings to Release Group", SendAlbumGroupRatings);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Recording", SendTrackTags);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Release", SendReleaseTags);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Release Group", SendReleaseGroupTags);

                    // output username to status bar if logged in
                    if (!string.IsNullOrEmpty(plugin.Properties.Settings.Default.refreshToken))
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"mb_MusicBrainzSync: Logged in as {mbz.user}");
                    }
                    break;
            }
        }

        // # Data submission functions
        public async void SendRatingData(string entity_type)
        {
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                mbApiInterface.MB_SetBackgroundTaskMessage("Submitting ratings to MusicBrainz...");
                try
                {
                    Dictionary<string, float> tracksAndRatings = new Dictionary<string, float>();
                    List<MusicBeeTrack> tracks = files.Select(file => new MusicBeeTrack(file)).ToList();
                    foreach (MusicBeeTrack track in tracks)
                    {
                        string currentMbid = "";
                        switch (entity_type)
                        {
                            // no need to handle releases, MusicBrainz doesn't allow rating of releases at the moment.
                            case "release-group":
                                currentMbid = track.MusicBrainzReleaseGroupId;
                                break;
                            default:
                                currentMbid = track.MusicBrainzTrackId;
                                break;
                        }
                        if (!string.IsNullOrEmpty(currentMbid))
                        {
                            if (entity_type == "recording")
                            {
                                // Track ratings are out of 5, so we need to multiply by 20 to get a percentage that MusicBrainz is happy to use.
                                float onlineRating = float.Parse(track.Rating) * 20;
                                tracksAndRatings.Add(currentMbid, onlineRating);
                            }
                            else
                            {
                                if (tracksAndRatings.ContainsKey(currentMbid))
                                {
                                    if (tracksAndRatings[currentMbid] != float.Parse(track.AlbumRating))
                                    {
                                        MessageBox.Show($"Error: {track.Album} has inconsistent album ratings.\n\nGive every track on that album the exact same album rating and try to submit again.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else
                                {
                                    tracksAndRatings.Add(currentMbid, float.Parse(track.AlbumRating));
                                }
                            }

                        }
                    }
                    if (tracksAndRatings.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("Ratings not submitted due to empty data.");
                    }
                    else
                    {
                        await mbz.SetRatings(tracksAndRatings, entity_type);
                        mbApiInterface.MB_SetBackgroundTaskMessage("Successfully submitted ratings to MusicBrainz.");
                    }
                }
                catch (UnsupportedFormatException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating submission failed due to unsupported format.");
                }
            }
        }

        public async void SendTagData(string entity_type)
        {
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                mbApiInterface.MB_SetBackgroundTaskMessage("Submitting tags to MusicBrainz...");
                try
                {
                    Dictionary<string, string> tracksAndTags = new Dictionary<string, string>();
                    List<MusicBeeTrack> tracks = files.Select(file => new MusicBeeTrack(file)).ToList();
                    foreach (MusicBeeTrack track in tracks)
                    {
                        string currentMbid = "";
                        switch (entity_type)
                        {
                            case "release":
                                currentMbid = track.MusicBrainzReleaseId;
                                break;
                            case "release-group":
                                currentMbid = track.MusicBrainzReleaseGroupId;
                                break;
                            default:
                                currentMbid = track.MusicBrainzTrackId;
                                break;
                        }
                        if (!string.IsNullOrEmpty(currentMbid))
                        {
                            List<string> tags = track.GetAllTagsFromFile(entity_type);
                            if (entity_type == "recording")
                            {
                                if (tags != null)
                                {
                                    tracksAndTags.Add(currentMbid, String.Join(";", tags));
                                }
                                    
                            }
                            else
                            {
                                if (tracksAndTags.ContainsKey(currentMbid))
                                {
                                    if (tracksAndTags[currentMbid] != String.Join(";", tags))
                                    {
                                        MessageBox.Show($"Error: {track.Album} has inconsistent tags.\n\nGive every track on that album the exact same tags and try to submit again.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (tags != null)
                                    {
                                        // if it's empty, we don't want to add it to the dictionary.
                                        tracksAndTags.Add(currentMbid, String.Join(";", tags));
                                    }
                                }
                            }

                        }
                    }
                    if (tracksAndTags.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("Tags not submitted due to empty data.");
                    }
                    else
                    {
                        await mbz.SetTags(tracksAndTags, entity_type);
                        mbApiInterface.MB_SetBackgroundTaskMessage("Successfully submitted tags to MusicBrainz.");
                    }
                }
                catch (UnsupportedFormatException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag submission failed due to unsupported format.");
                }
            }
        }

        public void SendTrackRatings(object sender, EventArgs args)
        {
            SendRatingData("recording");
        }

        public void SendAlbumGroupRatings(object sender, EventArgs args)
        {
           SendRatingData("release-group");
        }

        public void SendTrackTags(object sender, EventArgs args)
        {
            SendTagData("recording");
        }
        
        public void SendReleaseTags(object sender, EventArgs args)
        {
            SendTagData("release");
        }

        public void SendReleaseGroupTags(object sender, EventArgs args)
        {
            SendTagData("release-group");
        }

    }
}

