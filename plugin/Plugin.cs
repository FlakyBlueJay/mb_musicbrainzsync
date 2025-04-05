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

    public partial class Plugin
    {
        // Required so entrypoint can be called
        static private LibraryEntryPoint entryPoint = new LibraryEntryPoint();

        // MusicBee API initialisation
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        // new MusicBrainz API instance
        public MusicBrainzAPI mbz = new MusicBrainzAPI();

        // expose shared controls as public variables, so it can be easily accessed by other functions.
        public TextBox mbzUserInputBox;
        public Label userAuthenticatedLabel;
        public Panel authConfigPanel;
        public TableLayoutPanel postAuthConfigPanel;

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

                // panel for authentication
                authConfigPanel = new Panel();
                authConfigPanel.AutoSize = true; authConfigPanel.Hide();
                TableLayoutPanel authConfigPanel_new = new TableLayoutPanel();
                authConfigPanel_new.Dock = DockStyle.Fill;
                authConfigPanel_new.AutoSize = true;

                Label mbzUserInputLabel = new Label();
                mbzUserInputLabel.AutoSize = true;
                mbzUserInputLabel.Location = new Point(0, 20);
                mbzUserInputLabel.Text = "Access token from MusicBrainz:";

                mbzUserInputBox = new TextBox();
                mbzUserInputBox.Bounds = new Rectangle(355, 20, 500, mbzUserInputBox.Height);
                mbzUserInputBox.ForeColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                   SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                mbzUserInputBox.BackColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                   SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                mbzUserInputBox.BorderStyle = BorderStyle.FixedSingle;

                LinkLabel mbzVerifyLabel = new LinkLabel();
                mbzVerifyLabel.AutoSize = true;
                mbzVerifyLabel.Location = new Point(0, 70);
                mbzVerifyLabel.Text = "Log in to MusicBrainz";
                mbzVerifyLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                LinkLabel mbzAuthLabel = new LinkLabel();
                mbzAuthLabel.AutoSize = true;
                mbzAuthLabel.Location = new Point(0, 115);
                mbzAuthLabel.Text = "Get an access token from MusicBrainz";
                mbzAuthLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                authConfigPanel.Controls.AddRange(new Control[] { mbzUserInputLabel, mbzUserInputBox, mbzVerifyLabel, mbzAuthLabel });
                mainPanel.Controls.Add(authConfigPanel);

                // authentication panel events
                mbzVerifyLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(mbzVerifyLabel_LinkClicked);
                mbzAuthLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(mbzAuthLabel_LinkClicked);

                // post-auth / logged in panel
                postAuthConfigPanel = new TableLayoutPanel(); postAuthConfigPanel.AutoSize = true; postAuthConfigPanel.Hide();
                postAuthConfigPanel.Dock = DockStyle.Fill;
                postAuthConfigPanel.AutoSize = true;
                postAuthConfigPanel.ColumnCount = 1;
                postAuthConfigPanel.RowCount = 3;
                postAuthConfigPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                postAuthConfigPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // username label
                postAuthConfigPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // config link
                postAuthConfigPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // revoke access/logout link

                userAuthenticatedLabel = new Label();
                userAuthenticatedLabel.AutoSize = true;
                userAuthenticatedLabel.Text = "Logged in as %USERNAME%";
                LinkLabel configLinkLabel = new LinkLabel();
                configLinkLabel.AutoSize = true;
                configLinkLabel.Text = "Configure tag bindings";
                configLinkLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                LinkLabel revokeAccessLabel = new LinkLabel();
                revokeAccessLabel.AutoSize = true;
                revokeAccessLabel.Text = "Log out from MusicBrainz";
                revokeAccessLabel.LinkColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                    SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                postAuthConfigPanel.Controls.Add(userAuthenticatedLabel, 0, 0);
                postAuthConfigPanel.Controls.Add(configLinkLabel, 0, 1);
                postAuthConfigPanel.Controls.Add(revokeAccessLabel, 0, 2);
                mainPanel.Controls.Add(postAuthConfigPanel);

                // post-auth panel events
                revokeAccessLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(revokeAccessLabel_Clicked);

                // Hide the authentication panel if the user is already authenticated to MusicBrainz, or vice versa if not logged in.
                if (string.IsNullOrEmpty(Settings.Default.refreshToken))
                {
                    authConfigPanel.Show(); postAuthConfigPanel.Hide();
                }
                else
                {
                    string username = mbz.GetUserName().Result;
                    System.Diagnostics.Debug.WriteLine(username);
                    userAuthenticatedLabel.Text = $"Logged in as {username}";
                    authConfigPanel.Hide(); postAuthConfigPanel.Show();
                }

            }

            return false;

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
                    "Please copy and paste the URL into your browser.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                // any other errors we haven't caught.
                Clipboard.SetText(mbzAuthUrl);
                MessageBox.Show("An unexpected error occurred. The authentication URL has been copied to your clipboard.\n\n" +
                    "Please copy and paste the URL into your browser.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void mbzVerifyLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("verifyWithMusicBrainz called");
            if (!string.IsNullOrEmpty(mbzUserInputBox.Text))
            {
                bool userAuthenticated = mbz.AuthenticateUser(mbzUserInputBox.Text).Result;
                if (userAuthenticated)
                {
                    string username = mbz.GetUserName().Result;
                    userAuthenticatedLabel.Text = $"Logged in as {username}";
                    authConfigPanel.Hide(); postAuthConfigPanel.Show();
                }
            }
        }

        private void revokeAccessLabel_Clicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            mbz.RevokeAccess();
            mbzUserInputBox.Clear();
            userAuthenticatedLabel.Text = "Logged in as %USERNAME%";
            postAuthConfigPanel.Hide(); authConfigPanel.Show();
        }


        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // Auth process automatically saves the access and refresh tokens, and tag bindings will be handled via another GUI so no need for this at the moment.
        public void SaveSettings()
        {

        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            // todo: ask to delete settings
            Settings.Default.Reset();
            // todo: ask to delete tag binding data
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
                    System.Diagnostics.Debug.WriteLine("mb_musicbrainzsync has initialised and debug worfks!!!");

                    // add context menu items
                    // context.Main is the right-click menu for tracks and albums
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Sync Track Ratings", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Ratings/Sync Album Ratings", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Recording", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Release", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Release Group", "", DoNothing);
                    mbApiInterface.MB_AddMenuItem($"context.Main/MusicBrainz Sync: Tags/Sync Tags to Artist", "", DoNothing);

                    // add hotkey entries
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Track Ratings", DoNothing);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Album Ratings", DoNothing);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Recording", DoNothing);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Release", DoNothing);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Release Group", DoNothing);
                    mbApiInterface.MB_RegisterCommand("MusicBrainz Sync: Sync Tags to Artist", DoNothing);

                    // output username to status bar if logged in
                    if (!string.IsNullOrEmpty(plugin.Properties.Settings.Default.refreshToken))
                    {
                        string username = mbz.GetUserName().Result;
                        mbApiInterface.MB_SetBackgroundTaskMessage($"mb_musicbrainzsync: Logged in as {username}");
                    }
                    break;
            }
        }

        public void DoNothing(object sender, EventArgs args)
        {
            // just a filler function that does literally nothing, for anything that hasn't been implemented yet.
        }

    }
}