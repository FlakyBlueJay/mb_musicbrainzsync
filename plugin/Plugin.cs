using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
    using plugin;
    using plugin.Properties;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using YourNamespace;
    using static plugin.MusicBeeTrack;

    public partial class Plugin
    {
        // Required so entrypoint can be called
        static private LibraryEntryPoint entryPoint = new LibraryEntryPoint();

        // MusicBee API initialisation
        public static MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();
        public string stringVer = "";

        // new MusicBrainz API instance
        public MusicBrainzAPI mbz = new MusicBrainzAPI();

        // expose shared controls as public variables, so it can be easily accessed by other functions.
        public TextBox mbzUserInputBox;
        public Label userAuthenticatedLabel;
        public TableLayoutPanel authConfigPanel;
        public TableLayoutPanel postAuthConfigPanel;

        // This is the list of tag bindings that are used in the plugin.

        public static Dictionary<string, MetaDataType> tagBindings = new Dictionary<string, MetaDataType>
        {
            {"genre",    MetaDataType.Genre },
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
            stringVer = $"{ver.Major}.{ver.Minor}.{ver.Revision}";
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
            about.ReceiveNotifications = (ReceiveNotificationFlags.StartupOnly);
            about.ConfigurationPanelHeight = 90;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            // DO NOT decrease this height without testing on 100% and 200% DPI scaling!
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

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    // perform startup initialisation
                    Debug.WriteLine($"[mbz_MusicBrainzSync] Plugin initialised - version: {stringVer} ");
                    if (!string.IsNullOrEmpty(plugin.Properties.Settings.Default.refreshToken))
                    {
                        AddMenuItems();
                        // output username to status bar
                        mbApiInterface.MB_SetBackgroundTaskMessage($"mb_MusicBrainzSync: Logged in as {mbz.user}");
                    }

                    break;
            }
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
            catch (Exception ex)
            {
                // any other errors we haven't caught.
                Debug.WriteLine($"[Plugin.mbzAuthLabel_LinkClicked] Failed to open URL due to exception: {ex.Message}");
                Clipboard.SetText(mbzAuthUrl);
                MessageBox.Show("An unexpected error occurred. The authentication URL has been copied to your clipboard.\n\n" +
                    "Please copy and paste the URL into your browser.\n\n" +
                    $"Exception message: {ex.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async void mbzVerifyLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mbzUserInputBox.Text))
            {
                bool userAuthenticated = await mbz.AuthenticateUser(mbzUserInputBox.Text);
                if (userAuthenticated)
                {
                    userAuthenticatedLabel.Text = $"Logged in as {mbz.user}";
                    ToggleAuthPanelsVisibility(); AddMenuItems();
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

        /// <summary>
        /// Adds the menu items and hotkeys to the currently running MusicBee interface.
        /// </summary>
        private void AddMenuItems()
        {

            // # context menu items
            // context.Main is the right-click menu for tracks and albums

            // ## main menu items
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings", "", null);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags", "", null);

            // ### rating sub-menu items
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Sync Track Ratings to Recordings", "", SendTrackRatings);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Sync Album Ratings to Release Group", "", SendReleaseGroupRatings);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Retrieve Album Ratings from Release Group", "", GetAlbumRatings);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Retrieve Track Ratings from Recordings", "", GetTrackRatings);

            // ### tag sub-menu items
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Recordings", "", SendTrackTags);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Release", "", SendReleaseTags);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Release Group", "", SendReleaseGroupTags);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Get Tags from Release Group", "", GetReleaseGroupTags);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Get Tags from Release", "", GetReleaseTags);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Get Tags from Recordings", "", GetTrackTags);

            // ### open sub-menu items TODO

            // ## debug sub-menu items
#if DEBUG
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Debug", "", null);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Debug/Reset Online Track Ratings", "", ResetTrackRatings);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Debug/Reset Online Album Ratings", "", ResetReleaseGroupRatings);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Debug/Open JSON in Browser: Recording", "", OpenJSONRecording);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Debug/Open JSON in Browser: Release", "", OpenJSONRelease);
            mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Debug/Open JSON in Browser: Release Group", "", OpenJSONReleaseGroup);
#endif

            // # hotkey entries
            // ## rating hotkeys
            mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Track Ratings", SendTrackRatings);
            mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Album Ratings to Release Group", SendReleaseGroupRatings);
            mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Retrieve Track Ratings", null);

            // ## tag hotkeys
            mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Recording", SendTrackTags);
            mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Release", SendReleaseTags);
            mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Release Group", SendReleaseGroupTags);

            Debug.WriteLine("[Plugin.AddMenuItems] Done");
        }

        // # Data submission functions

        /// <summary>
        /// Processes the track or album's rating data to be sent to MusicBrainz.
        /// </summary>
        private async Task SendRatingData(string entity_type, bool reset = false)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Submitting {entity_type.Replace('-', ' ')} ratings to MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    Dictionary<string, float> tracksAndRatings = new Dictionary<string, float>();
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());
                    // required to check album ratings for consistency.
                    await Task.Run(() => TrackProcessor.ProcessForRatingUpload(tracks, entity_type, tracksAndRatings, reset));
                    if (tracksAndRatings.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("Ratings not submitted due to no retrievable MusicBrainz IDs.");
                    }
                    else
                    {
                        await mbz.SetUserRatings(tracksAndRatings, entity_type);
                        mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully submitted {entity_type.Replace('-', ' ')} ratings to MusicBrainz.");
                    }
                }
                catch (UnsupportedFormatException e)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating submission failed due to unsupported format.");
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating submission failed due to an error during processing.");
                }
            }
        }

        /// <summary>
        /// Processes the track or album's tag data to be sent to MusicBrainz.
        /// </summary>
        private async Task SendTagData(string entity_type)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Sending {entity_type.Replace('-', ' ')} tags to MusicBrainz.");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    Dictionary<string, string> tracksAndTags = new Dictionary<string, string>();
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());
                    await Task.Run(() => TrackProcessor.ProcessForTagUpload(tracks, entity_type, tracksAndTags));
                    if (tracksAndTags.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("Tags not submitted due to empty data.");
                    }
                    else
                    {
                        await mbz.SetTags(tracksAndTags, entity_type);
                        mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully submitted {entity_type.Replace('-', ' ')} tags to MusicBrainz.");
                    }
                }
                catch (UnsupportedFormatException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag submission failed due to unsupported format.");
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag submission failed due to an error during processing.");
                }
            }
        }

        /// <summary>
        /// Processes track and/or album IDs to retrieve the currently logged in user's ratings.
        /// </summary>
        private async Task GetRatingData(string entity_type)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Requesting {entity_type.Replace('-', ' ')} ratings from MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());

                    // Dictionary of MBIDs and MusicBee tracks, so the ratings can be added to the correct tracks after retrieval.
                    Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs = new Dictionary<string, List<MusicBeeTrack>>();
                    // list of MBIDs to query MusicBrainz with - prevents duplicate queries and any potential rate limiting/DDoS.
                    List<string> onlineMbids = new List<string>();

                    await Task.Run(() =>
                    {
                        TrackProcessor.ProcessForRatingDataRetrieval(tracks, entity_type, onlineMbids, mbidTrackPairs);
                    });

                    if (mbidTrackPairs.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entity_type.Replace('-', ' ')}s.");
                        MessageBox.Show("No ratings have been received since none of the files being processed have any MusicBrainz IDs to use. You need to match them to entries on MusicBrainz using Picard.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (onlineMbids.Count > 50)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entity_type.Replace('-', ' ')}s.");
                        MessageBox.Show("You are attempting to download data for too many entries, which will result in you hitting MusicBrainz's rate limits. Cut down on the amount of things you're requesting MusicBrainz for.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        Dictionary<string, int?> albumsAndRatings = await mbz.GetUserRatings(onlineMbids);

#if DEBUG
                        foreach (var pair in albumsAndRatings)
                        {
                            Debug.WriteLine($"[Plugin.GetRatingData] MBID: {pair.Key}, Rating: {pair.Value}");
                        }
#endif
                        int editedRatingCounter = 0;
                        await Task.Run(() =>
                        {
                            foreach (var pair in albumsAndRatings)
                            {
                                // don't want to do anything if the user hasn't rated it, so check if the key exists in mbidTrackPairs.
                                if (mbidTrackPairs.ContainsKey(pair.Key))
                                {
                                    foreach (MusicBeeTrack track in mbidTrackPairs[pair.Key])
                                    {
                                        MetaDataType chosenMetadata = (entity_type == "release-group") ? MetaDataType.RatingAlbum : MetaDataType.Rating;
                                        mbApiInterface.Library_SetFileTag(track.FilePath, chosenMetadata, pair.Value.ToString());
                                        mbApiInterface.Library_CommitTagsToFile(track.FilePath);
                                        editedRatingCounter++;
                                    }
                                }

                            }
                        });
                            
                        if (editedRatingCounter != 0)
                        {
                            mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully retrieved {entity_type.Replace('-', ' ')} ratings from MusicBrainz.");
                        }
                        else
                        {
                            mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entity_type.Replace('-', ' ')}s.");
                        }

                    }

                }
                catch (UnsupportedFormatException e)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating retrieval failed due to unsupported format.");
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private async Task GetTagData(string entityType)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Requesting {entityType.Replace('-', ' ')} tags from MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());

                    // Dictionary of MBIDs and MusicBee tracks, so the ratings can be added to the correct tracks after retrieval.
                    // We may be duplicating these functions between GetTagData and GetRatingData, so it might be worth refactoring later.
                    Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs = new Dictionary<string, List<MusicBeeTrack>>();
                    // list of MBIDs to query MusicBrainz with - prevents duplicate queries and any potential rate limiting/DDoS.
                    List<string> onlineMbids = new List<string>();

                    await Task.Run(() =>
                    {
                        TrackProcessor.ProcessForTagDataRetrieval(tracks, entityType, onlineMbids, mbidTrackPairs);
                    });

                    if (mbidTrackPairs.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entityType.Replace('-', ' ')}s.");
                        MessageBox.Show("No ratings have been received since none of the files being processed have any MusicBrainz IDs to use. You need to match them to entries on MusicBrainz using Picard.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (onlineMbids.Count > 50)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entityType.Replace('-', ' ')}s.");
                        MessageBox.Show("You are attempting to download data for too many entries, which will result in you hitting MusicBrainz's rate limits. Cut down on the amount of things you're requesting MusicBrainz for.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        Dictionary<string, Dictionary<string, List<string>>> mbidsAndTags = await mbz.GetTags(onlineMbids);
                        await Task.Run(() =>
                        {
                            foreach (var pair in mbidsAndTags)
                            {
                                if (mbidTrackPairs.ContainsKey(pair.Key))
                                {
                                    foreach (MusicBeeTrack track in mbidTrackPairs[pair.Key])
                                    {
                                        foreach (var tagAndValues in pair.Value) // gonna need to find a new name for this variable
                                        {
                                            MetaDataType currentTag = tagBindings[tagAndValues.Key];
                                            mbApiInterface.Library_SetFileTag(track.FilePath, currentTag, string.Join("; ", tagAndValues.Value));
                                        }
                                        mbApiInterface.Library_CommitTagsToFile(track.FilePath);
                                        // Debug.WriteLine($"[Plugin.GetTagData] Successfully committed edited tags to {track.Artist} - {track.Title}");
                                        
                                    }
                                }
                            }
                            
                        });
                        mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully retrieved {entityType.Replace('-', ' ')} tags from MusicBrainz.");
                    }
                
                }
                catch (UnsupportedFormatException e)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag retrieval failed due to unsupported format.");
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        async private void SendTrackRatings(object sender, EventArgs args)
        {
            if (CheckWarning("set"))
                await SendRatingData("recording");
        }

        async private void SendReleaseGroupRatings(object sender, EventArgs args)
        {
            if (CheckWarning("set"))
                await SendRatingData("release-group");
        }

        async private void SendTrackTags(object sender, EventArgs args)
        {
            if (CheckWarning("set"))
                await SendTagData("recording");
        }

        async private void SendReleaseTags(object sender, EventArgs args)
        {
            if (CheckWarning("set"))
                await SendTagData("release");
        }

        async private void SendReleaseGroupTags(object sender, EventArgs args)
        {
            if (CheckWarning("set"))
                await SendTagData("release-group");
        }

        // # Data retrieval functions

        async private void GetAlbumRatings(object sender, EventArgs args)
        {
            if (CheckWarning("get"))
                await GetRatingData("release-group");
        }

        async private void GetTrackRatings(object sender, EventArgs args)
        {
            // this will handle both release-associated tracks and standalone recordings.
            if (CheckWarning("get"))
                await GetRatingData("track");
        }

        async private void GetReleaseGroupTags(object sender, EventArgs args)
        {
            if (CheckWarning("get"))
                await GetTagData("release-group");
        }

        async private void GetReleaseTags(object sender, EventArgs args)
        {
            if (CheckWarning("get"))
                await GetTagData("release");
        }

        async private void GetTrackTags(object sender, EventArgs args)
        {
            if (CheckWarning("get"))
                await GetTagData("recording");
        }

        private bool CheckWarning(string actionType)
        {

            SettingsProperty currentSetting = (actionType == "set") ? Settings.Default.Properties["setWarningAcknowledged"] : Settings.Default.Properties["getWarningAcknowledged"];
            bool currentSettingValue = (bool)Settings.Default.PropertyValues[currentSetting.Name].PropertyValue;
            if (currentSettingValue == true)
            {
                return true;
            }
            else
            {
                string warningText = (actionType == "set") ?
                    "Warning: You are about to send data to MusicBrainz. This action could result in data on MusicBrainz being overwritten." : 
                    "Warning: You are about to overwrite data on your files with data from MusicBrainz.";
                DialogResult warning = MessageBox.Show(warningText + "\n\n" +
                    "Unless you have backups, this action is irreversible. Do you wish to proceed? This dialog will only appear once.", "MusicBrainz Sync", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (warning == DialogResult.Yes)
                {
                    Settings.Default.PropertyValues[currentSetting.Name].PropertyValue = true;
                    Settings.Default.Save();
                    return true;
                }
                else
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Task aborted due to user.");
                    return false;
                }
            }
        }


#if DEBUG
        async private void ResetTrackRatings(object sender, EventArgs args)
        {
            await SendRatingData("recording", true);
        }

        async private void ResetReleaseGroupRatings(object sender, EventArgs args)
        {
            await SendRatingData("release-group", true);
        }


        private void OpenJSONDataInBrowser(MusicBeeTrack track, string entityType)
        {
            string MBID = string.Empty;
            string MusicBrainzServer = mbz.MusicBrainzServer;
            switch (entityType)
            {
                case "release-group":
                    MBID = track.MusicBrainzReleaseGroupId;
                    break;
                case "release":
                    MBID = track.MusicBrainzReleaseId;
                    break;
                case "recording":
                    MBID = track.MusicBrainzRecordingId;
                    break;
            }
            string URL = MusicBrainzServer + "/ws/2/" + entityType + "/" + MBID + "?fmt=json&inc=tags+genres+user-tags+user-genres";
            if (entityType == "release") URL += "+recordings"; 
            Process.Start("https://" + URL);
        }

        // I'm not going to spam-open windows... :|
        private MusicBeeTrack GetFirstTrackInSelection()
        {
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            List<MusicBeeTrack> tracks = files.Select(file => new MusicBeeTrack(file)).ToList();
            Debug.WriteLine(tracks.Count);
            MusicBeeTrack track = tracks[0];
            return track;
        }

        private void OpenJSONRecording(object sender, EventArgs args)
        {
            
            MusicBeeTrack track = GetFirstTrackInSelection();
            OpenJSONDataInBrowser(track, "recording");
        }

        private void OpenJSONRelease(object sender, EventArgs args)
        {

            MusicBeeTrack track = GetFirstTrackInSelection();
            OpenJSONDataInBrowser(track, "release");
        }

        private void OpenJSONReleaseGroup(object sender, EventArgs args)
        {

            MusicBeeTrack track = GetFirstTrackInSelection();
            OpenJSONDataInBrowser(track, "release-group");
        }
#endif
    }
}

