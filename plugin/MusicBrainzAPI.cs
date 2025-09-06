using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace plugin
{
    using MusicBeePlugin;
    using plugin.Properties;
    using System.Collections.Generic;
    using System.Diagnostics;
    using static MusicBeePlugin.Plugin;

    public class MusicBrainzAPI
    {
        Version ver = typeof(Plugin).Assembly.GetName().Version;

        // I am aware that a MusicBrainz API wrapper already exists, but it requires .NET Core which is not supported for MusicBee plugins AFAIK.

        // # Web initialisation stuff
        public string OAuthClientId = "7_dLhLIHweJ-vefrHOc-MKLHuNnBEl93";
        public string OAuthClientSecret = "oE1duN7a8eYrkIov6JBriFnzpXpoVN-J";
        // Desktop applications like this plugin don't need to hide their client secret.
        // MusicBrainz Picard's client secret is in its source code, so I'm following suit.
        // Complain to MusicBrainz about this, not me.
        public string MusicBrainzServer = "musicbrainz.org"; // Change this if you want to use a different server.
        public HttpClient MBzHttpClient;
        System.Threading.Tasks.Task<HttpResponseMessage> mbApiResponse;
        public string mbzAccessToken; public DateTime mbzAccessTokenExpiry;
        public string user = null;

        /// <summary>
        /// Exception to be called when the XML data comes up empty so the plugin can tell the user on the status bar.
        /// </summary>
        public class EmptyDataException : Exception {
            public EmptyDataException() { }
        }


        // # JSON objects needed for Newtonsoft.Json deserialization
        /// <summary>
        /// Object representation of the authentication JSON data received from MusicBrainz.
        /// </summary>
        internal class MusicBrainzOAuthData
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }

        /// <summary>
        /// Object representation of the user JSON data received from MusicBrainz.
        /// Partially implemented for now.
        /// </summary>
        public class MusicBrainzUser
        {
            // MusicBrainz reports the user name in the "sub" field, but we want to represent it as "username".
            [JsonProperty("sub")]
            public string username { get; set; }
        }

        // object version of the MusicBRainz error JSON data.
        /// <summary>
        /// Object representation of the error JSON data received from MusicBrainz.
        /// </summary>
        public class MusicBrainzAPIError
        {
            public string error { get; set; }
            public string error_description { get; set; }
        }

        // # Initialisation
        /// <summary>
        /// The MusicBrainz API interface.
        /// </summary>
        public MusicBrainzAPI()
        {
            // initialise the HTTP client.
            MBzHttpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://{MusicBrainzServer}")
                
            };

            // MusicBrainz will reject requests that don't come from valid user agents.
            string stringVer = $"{ver.Major}.{ver.Minor}.{ver.Revision}";
            MBzHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"mb_MusicBrainzSync/{stringVer} (https://github.com/FlakyBlueJay/mb_musicbrainzsync)");

            // check if the user is logged in or not.
            if (string.IsNullOrEmpty(Settings.Default.refreshToken))
            {
                Debug.WriteLine("[MusicBrainzAPI()] No refresh token found, assuming user is logged out.");
            }
            else
            {
                user = Settings.Default.cachedUsername;
            }
        }

        // # HTTP connectivity functions
        /// <summary>
        /// Displays an error MessageBox depending on the HttpStatusCode, or the HttpStatusCode.BadRequest type retrieved from MusicBrainz.
        /// </summary>
        internal void DisplayHTTPErrorMessage(System.Net.HttpStatusCode statusCode)
        {
            string errorMessage = "";
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    errorMessage = "MusicBrainz returned an unauthorised error (code 401). This means you haven't logged in properly. Log in again, or restart the plugin.";
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    errorMessage = "MusicBrainz returned an internal server error (code 500). This is likely a server-side issue. Try again at a later time.";
                    break;
                case System.Net.HttpStatusCode.BadGateway:
                    errorMessage = "MusicBrainz returned a bad gateway error (code 502). This is likely a server-side issue. Try again at a later time.";
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    string error = mbApiResponse.Result.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine("[MusicBrainzAPI] Fail Response: " + error);
                    MusicBrainzAPIError mbErrorData = JsonConvert.DeserializeObject<MusicBrainzAPIError>(error);
                    switch (mbErrorData.error)
                    {
                        case "invalid_grant":
                            errorMessage = "MusicBrainz returned an invalid grant error. This means the access token you provided is invalid. Make sure you copied the access token from MusicBrainz fully.\n\n" +
                                "If that does not work, authenticate yourself again to get a new access token.";
                            break;
                        case "invalid_request":
                            errorMessage = "MusicBrainz returned an invalid request error. This means the request was malformed. This is likely a bug, report it to https://github.com/FlakyBlueJay/mb_musicbrainzsync";
                            break;
                        default:
                            // any that I haven't caught yet.
                            errorMessage = $"MusicBrainz returned the following bad request (code 400) error that hasn't been properly caught yet: {mbErrorData.error}\n\n" +
                                $"Report this error to https://github.com/FlakyBlueJay/mb_musicbrainzsync";
                            break;
                    }
                    break;
                default:
                    errorMessage = $"MusicBrainz returned a {statusCode} error. This hasn't been properly caught by the plugin.\n\n" +
                        $"Report this error to https://github.com/FlakyBlueJay/mb_musicbrainzsync";
                    break;
            }

            MessageBox.Show(
                $"Error: {errorMessage}", "MusicBrainz Sync",
                MessageBoxButtons.OK, MessageBoxIcon.Error
                );
        }

        /// <summary>
        /// Generic function for handling GET functions to the MusicBrainz server.
        /// </summary>
        private async Task<string> GetFromMusicBrainz(string endpoint, string data = null, bool silent = false)
        {
            string refreshToken = Settings.Default.refreshToken;
            if (string.IsNullOrEmpty(refreshToken))
            {
                if (!silent) 
                { 
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }
            else if (DateTime.Now >= mbzAccessTokenExpiry && !string.IsNullOrEmpty(refreshToken))
            {
                // if the access token is expired, try to refresh it.
                bool reauth = await AuthenticateUser();
                if (reauth)
                {
                    // try again.
                    return await GetFromMusicBrainz(endpoint, data);
                }
                else
                {
                    DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                    return null;
                }
            }
            else
            {
                try
                {
                    Debug.WriteLine($"[MusicBrainzAPI.GetFromMusicBrainz] {MBzHttpClient.BaseAddress}{endpoint}");
                    mbApiResponse = MBzHttpClient.GetAsync(endpoint);
                    if (mbApiResponse.Result.IsSuccessStatusCode)
                    {
                        string result = await mbApiResponse.Result.Content.ReadAsStringAsync();
#if DEBUG
                        Debug.WriteLine("[MusicBrainzAPI.GetFromMusicBrainz] Response from MusicBrainz: " + result);
#endif
                        return result;
                    }
                    else if (mbApiResponse.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {

                        bool reauth = await AuthenticateUser();
                        if (reauth)
                        {
                            // try again.
                            return await GetFromMusicBrainz(endpoint, data);
                        }
                        else
                        {
                            DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                            return null;
                        }
                    }
                    else
                    {
                        DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                        return null;
                    }
                }
                catch (HttpRequestException e) // catch all other HTTP exceptions here.
                {
                    MessageBox.Show(e.ToString(), "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
        }

        /// <summary>
        /// Generic function for handling POST functions to the MusicBrainz server.
        /// </summary>
        private async Task<string> PostToMusicBrainz(string endpoint, string data, string data_type = "application/json", bool silent = false)
        {
            string refreshToken = Settings.Default.refreshToken;
            if (string.IsNullOrEmpty(refreshToken) && (endpoint != "/oauth2/token")) // do not trigger on authentication requests
            {
                if (!silent)
                {
                    MessageBox.Show(
                        "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                        "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }
            else if (DateTime.Now >= mbzAccessTokenExpiry && (endpoint != "/oauth2/token"))
            {
                // if the access token is expired, try to refresh it.
                bool reauth = await AuthenticateUser();
                if (reauth)
                {
                    // try again.
                    return await PostToMusicBrainz(endpoint, data, data_type);
                }
                else
                {
                    DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                    return null;
                }
            }
            else
            {
                try
                {
                    StringContent postContent = new StringContent(data, Encoding.UTF8, data_type);
                    mbApiResponse = MBzHttpClient.PostAsync(endpoint, postContent);
                    if (mbApiResponse.Result.IsSuccessStatusCode)
                    {
                        string result = await mbApiResponse.Result.Content.ReadAsStringAsync();
                        Debug.WriteLine("[MusicBrainzAPI.PostToMusicBrainz] JSON Response: " + result);
                        // todo: does this output to XML as well? POST requires XML for certain.
                        return result;
                    }
                    else if (mbApiResponse.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // if we get a 401 error, try to reauthenticate.
                        bool userAuthenticated = await AuthenticateUser();
                        if (userAuthenticated)
                        {
                            // try again.
                            return await PostToMusicBrainz(endpoint, data, data_type);
                        }
                        else
                        {
                            MessageBox.Show(
                                "An error has occurred attempting to authenticate with MusicBrainz.",
                                "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }
                    else
                    {
                        DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                        return null;
                    }
                }
                catch (HttpRequestException e) // catch all other HTTP exceptions here.
                {
                    MessageBox.Show(e.ToString(), "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        /// <summary>
        /// Function to return an authentication URL.
        /// </summary>
        public string GetAuthenticationURL()
        {
            // since MusicBrainzServer is declared here and not in Plugin.cs, best to have a function to generate the URL here.
            string authURL = $"{MBzHttpClient.BaseAddress}/oauth2/authorize?response_type=code&client_id={OAuthClientId}&scope=tag%20rating%20profile&redirect_uri=urn:ietf:wg:oauth:2.0:oob";
            return authURL;
        }

        // # User authentication functions
        /// <summary>
        /// Authenticates the user, only refreshing the refresh token if the user has authenticated before.
        /// </summary>
        public async Task<bool> AuthenticateUser(string userApiKey = null)
        {
            string parameters;

            // if user is sending an access token, assume they haven't logged in yet, otherwise assume refresh token.
            if (string.IsNullOrEmpty(userApiKey))
            {
                parameters = "grant_type=refresh_token&" +
                $"refresh_token={Settings.Default.refreshToken}&" +
                $"client_id={OAuthClientId}&" +
                $"client_secret={OAuthClientSecret}";
            }
            else
            {
                parameters = "grant_type=authorization_code&" +
                   $"code={userApiKey}&" +
                   $"client_id={OAuthClientId}&" +
                   $"client_secret={OAuthClientSecret}&" +
                   $"redirect_uri=http://localhost"; // We don't need a redirect URI for this plugin, but it's required by the API.
            }

            string userauth = await PostToMusicBrainz("/oauth2/token", parameters, "application/x-www-form-urlencoded");
            if (string.IsNullOrEmpty(userauth))
            {
                return false;
            }
            else
            {
                MusicBrainzOAuthData mbOAuthData = JsonConvert.DeserializeObject<MusicBrainzOAuthData>(userauth);
                // add refresh token only if refresh. (i.e. new login)
                if (!string.IsNullOrEmpty(userApiKey))
                {
                    Settings.Default.refreshToken = mbOAuthData.refresh_token;
                }
                // access token should be changed regardless.
                mbzAccessToken = mbOAuthData.access_token;
                mbzAccessTokenExpiry = DateTime.Now.AddSeconds(mbOAuthData.expires_in);
                Settings.Default.Save();
                MBzHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mbzAccessToken);
                user = await GetUserName(); Settings.Default.cachedUsername = user;
                Settings.Default.Save();
                return true;
            }

        }

        /// <summary>
        /// Revokes access to MusicBrainz and clears any associated data. (i.e. "log out")
        /// </summary>
        public async Task RevokeAccess()
        {
            string parameters = $"token={Settings.Default.refreshToken}&" +
                $"client_id={OAuthClientId}&" +
                $"client_secret={OAuthClientSecret}&";
            string revokeRequest = await PostToMusicBrainz("/oauth2/revoke", parameters, "application/x-www-form-urlencoded");
            Settings.Default.refreshToken = null; Settings.Default.cachedUsername = null;
            Settings.Default.Save();
        }

        // # User functions
        // Generally should only have GetUserName to display it in the plugin settings.
        /// <summary>
        /// Grabs the username of the currently authenticated user.
        /// </summary>
        public async Task<string> GetUserName()
        {
            string userinfo = await GetFromMusicBrainz("/oauth2/userinfo");
            if (!string.IsNullOrEmpty(userinfo))
            {
                MusicBrainzUser user = JsonConvert.DeserializeObject<MusicBrainzUser>(userinfo);
                return user.username;
            }
            else return null;
        }

        // # Rating functions
        /// <summary>
        /// Generates XML from user-inputted ratings and MBIDs and sends that XML to MusicBrainz to add/change on the user's behalf.
        /// </summary>
        public async Task SetUserRatings(Dictionary<string, float> mbidRatings, string entity_type)
        {
            if (user != null)
            {
                StringWriter stringWriter = new StringWriter();
                XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
                xmlWriter.WriteStartElement("metadata"); xmlWriter.WriteAttributeString("xmlns", "http://musicbrainz.org/ns/mmd-2.0#");
                xmlWriter.WriteStartElement($"{entity_type}-list");

                foreach (KeyValuePair<string, float> trackTagPair in mbidRatings)
                {
                    string mbid = trackTagPair.Key;
                    string rating = trackTagPair.Value.ToString();
                    xmlWriter.WriteStartElement(entity_type); xmlWriter.WriteAttributeString("id", mbid);
                    xmlWriter.WriteElementString("user-rating", rating);
                    xmlWriter.WriteEndElement(); // ends individual recording XML
                }
                xmlWriter.WriteEndElement(); // ends recording-list XML
                xmlWriter.WriteEndElement(); // ends metadata XML
                xmlWriter.Flush();
                string xmlData = stringWriter.ToString();
                Debug.WriteLine("[MusicBrainzAPI.SetRatings] XML Data: " + xmlData);
                await PostToMusicBrainz("/ws/2/rating?client=mb_MusicBrainzSync", xmlData, "application/xml");
            }
            else
            {
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Retrieves the rating data from the currently authenticated user.
        /// </summary>
        public async Task<Dictionary<string, int?>> GetUserRatings(List<string> mbids)
        {
            string entity_type = "";
            Dictionary<string, int?> mbidRatings = new Dictionary<string, int?>();
            foreach (string mbidUrl in mbids) {
                string[] mbidData = mbidUrl.Split('/');
                entity_type = mbidData[0]; string currentMbid = mbidData[1];
                double? rating = null;

                Debug.WriteLine($"[MusicBrainzAPI.GetUserRatings] {MBzHttpClient.BaseAddress}/ws/2/{mbidUrl}?inc=user-ratings&fmt=json");
                string json = "";
                
                switch (entity_type)
                {
                    case "release-group":
                        json = await GetFromMusicBrainz($"/ws/2/{mbidUrl}?inc=user-ratings&fmt=json");
                        if (!string.IsNullOrEmpty(json))
                        {
                            ReleaseGroup rg = JsonConvert.DeserializeObject<ReleaseGroup>(json);
                            rating = rg.UserRating; string id = rg.MBID;
                            Debug.WriteLine($"[MusicBrainzAPI.GetUserRatings] Release Group Title: {rg.Title}, Rating: {rating}");
                            if (rating.HasValue)
                            {
                                rating = (rating * 2) * 10; // convert to 0-100 scale
                                mbidRatings.Add(currentMbid, (int)Math.Round(rating.Value));
                            }
                        }
                        break;
                    case "release": // recordings in a release - TODO use "recording-release" and refactor!
                        json = await GetFromMusicBrainz($"/ws/2/{mbidUrl}?inc=user-ratings%2Brecordings&fmt=json");
                        Release r = JsonConvert.DeserializeObject<Release>(json);
                        Debug.WriteLine($"[MusicBrainzAPI.GetUserRatings] {r.Media}");
                        foreach (ReleaseMedia rm in r.Media)
                        {
                            foreach (ReleaseMediaTrack rt in rm.Tracks)
                            {
                                rating = rt.Recording.CurrentUserRating;
                                string recordingID = rt.Recording.MBID;
                                if (rating.HasValue)
                                {
                                    rating = (rating * 2) * 10; // convert to 0-100 scale
                                    mbidRatings.Add(recordingID, (int)Math.Round(rating.Value));
                                }
                                Debug.WriteLine($"[MusicBrainzAPI.GetUserRatings] Release Title: {r.Title}, Track Title: {rt.Title}, Rating: {rating}");
                            }
                        }
                        break;
                    case "recording":
                        json = await GetFromMusicBrainz($"/ws/2/{mbidUrl}?inc=user-ratings&fmt=json");
                        Recording rec = JsonConvert.DeserializeObject<Recording>(json);
                        rating = rec.CurrentUserRating;
                        Debug.WriteLine($"[MusicBrainzAPI.GetUserRatings] Recording Title: {rec.Title}, Rating: {rating}");
                        if (rating.HasValue)
                        {
                            rating = (rating * 2) * 10; // convert to 0-100 scale
                            mbidRatings.Add(currentMbid, (int)Math.Round(rating.Value));
                        }
                        break;
                }

                
                if (mbids.Count > 1)
                {
                    Debug.WriteLine($"[MusicBrainzAPI.GetUserRatings] Waiting one second...");
                    await Task.Delay(1000); // to avoid rate limiting  
                }
                  
            }
            return mbidRatings;
            
        }

        // # Tag functions

        // function to find and replace tags based on user specified settings.
        /// <summary>
        /// Function to find and replace tags based on user-specified settings.<br/><br/>
        /// This depends on how the user wants to handle tag submission<br/>(e.g. a user's genre tags may not match the tag associated with a genre on MusicBrainz and the user wishes to map the tag to the MusicBrainz version.).<br/>
        /// A reverse function is also available to convert MusicBrainz tags to user tags when retrieving tags.
        /// </summary>
        /// <param name="tag">The tag to find and replace.</param>
        /// <param name="reverse">If true, reverses the pairing (typically when retrieving the tags from MusicBrainz.)</param>
        private string FindReplaceTag(string tag, bool reverse = false)
        {
            tag = tag.ToLower();
            if (string.IsNullOrEmpty(Settings.Default.findReplace))
            {
                return tag;
            }
            else
            {
                Dictionary<string, string> findReplace = new Dictionary<string, string>();
                // convert the findReplace setting into a dictionary for easier.
                foreach (string line in Settings.Default.findReplace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    // split by the first semicolon to get the find and replace values
                    string[] row = line.Split(new[] { ';' }, 2);
                    if (row.Length == 2)
                    {
                        string findTag = (!reverse) ? row[0] : row[1];
                        string replaceTag = (!reverse) ? row[1] : row[0];
                        findReplace.Add(findTag, replaceTag);
                    }
                }
                if (findReplace.ContainsKey(tag))
                {
                    return findReplace[tag];
                }
                else
                {
                    return tag;
                }
            }
                     
        }

        /// <summary>
        /// Generates XML from user-inputted tags and MBIDs and sends that XML to MusicBrainz to add/change on the user's behalf.<br/><br/>
        /// </summary>
        /// <param name="trackMbid_TagPairing">A Dictionary with the key being a MusicBrainz ID and the value being the tag(s).</param>
        /// <param name="entityType">The type of MusicBrainz entity being dealt with.</param>
        public async Task SetTags(Dictionary<string, string> trackMbid_TagPairing, string entityType = "recording")
        {
            if (user != null) { // TODO: This check should not be here IMO, move it to main plugin functions instead.
                StringWriter stringWriter = new StringWriter();
                XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
                xmlWriter.WriteStartElement("metadata"); xmlWriter.WriteAttributeString("xmlns", "http://musicbrainz.org/ns/mmd-2.0#");
                xmlWriter.WriteStartElement($"{entityType}-list");
                foreach (KeyValuePair<string, string> trackTagPair in trackMbid_TagPairing)
                {
                    string recordingMbid = trackTagPair.Key;
                    string tag = trackTagPair.Value;
                    xmlWriter.WriteStartElement(entityType); xmlWriter.WriteAttributeString("id", recordingMbid);
                    xmlWriter.WriteStartElement("user-tag-list");

                    foreach (string tagString in tag.Split(';'))
                    {
                        xmlWriter.WriteStartElement("user-tag");
                        if (!Settings.Default.tagSubmitIsDestructive)
                        {
                            xmlWriter.WriteAttributeString("vote", "upvote");
                        }
                        xmlWriter.WriteElementString("name", FindReplaceTag(tagString));
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement(); // ends user-tag-list XML
                    xmlWriter.WriteEndElement(); // ends individual recording XML
                }
                xmlWriter.WriteEndElement(); // ends recording-list XML
                xmlWriter.WriteEndElement(); // ends metadata XML
                xmlWriter.Flush();
                string xmlData = stringWriter.ToString();
                Debug.WriteLine("[MusicBrainzAPI.SetTags] XML Data: " + xmlData);
                await PostToMusicBrainz("/ws/2/tag?client=mb_MusicBrainzSync", xmlData, "application/xml");
            }
            else
            {
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public async Task<Dictionary<string, Dictionary<string, List<string>>>> GetTags(List<string> mbids, bool releaseRecordings = false)
        {
            Dictionary<string, Dictionary<string, List<string>>> mbidTags = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (string mbidUrl in mbids)
            {
                string[] mbidData = mbidUrl.Split('/');
                string entityType = mbidData[0]; string currentMbid = mbidData[1];

                // TODO respect user-tag setting
                string retrievedGenreType = "genres";
                string retrievedTagType = "tags";
                string json = "";
                List<MusicBrainzTag> onlineGenreData = new List<MusicBrainzTag>();
                List<MusicBrainzTag> onlineTagData = new List<MusicBrainzTag>();

                if (entityType == "recording-release")
                {
                    // This is a fake entity tag to handle recordings associated with a release
                    json = await GetFromMusicBrainz($"/ws/2/{mbidUrl.Replace("recording-", "")}?inc={retrievedTagType}+{retrievedGenreType}+recordings&fmt=json");
                    if (!string.IsNullOrEmpty(json))
                    {
                        Release r = JsonConvert.DeserializeObject<Release>(json);
                        foreach (ReleaseMedia rm in r.Media)
                        {
                            foreach (ReleaseMediaTrack rt in rm.Tracks)
                            {
                                Recording rc = rt.Recording;
                                string recordingID = rc.MBID;
                                Debug.WriteLine($"[MusicBrainzAPI.GetTags] Processing tags for recording: {rc.Title}");
                                onlineGenreData = (retrievedGenreType == "genres") ? rc.Genres : rc.UserGenres;
                                onlineTagData = (retrievedTagType == "tags") ? rc.Tags : rc.UserTags;
                                Dictionary<string, List<string>> onlineRecordingTags = await ProcessOnlineTags(onlineTagData, onlineGenreData);
                                mbidTags.Add(recordingID, onlineRecordingTags);
                            }
                        }
                    }
                }
                else
                {
                    json = await GetFromMusicBrainz($"/ws/2/{mbidUrl}?inc={retrievedTagType}+{retrievedGenreType}&fmt=json");
                    if (!string.IsNullOrEmpty(json))
                    {
                        Debug.WriteLine($"[MusicBrainzAPI.GetTags] {entityType} JSON: {json}");
                        switch (entityType)
                        {
                            case "release-group":
                                ReleaseGroup rg = JsonConvert.DeserializeObject<ReleaseGroup>(json);
                                onlineGenreData = (retrievedGenreType == "genres") ? rg.Genres : rg.UserGenres;
                                onlineTagData = (retrievedTagType == "tags") ? rg.Tags : rg.UserTags;
                                break;
                            case "release":
                                Release rl = JsonConvert.DeserializeObject<Release>(json);
                                onlineGenreData = (retrievedGenreType == "genres") ? rl.Genres : rl.UserGenres;
                                onlineTagData = (retrievedTagType == "tags") ? rl.Tags : rl.UserTags;
                                break;
                            case "recording":
                                Recording rc = JsonConvert.DeserializeObject<Recording>(json);
                                onlineGenreData = (retrievedGenreType == "genres") ? rc.Genres : rc.UserGenres;
                                onlineTagData = (retrievedTagType == "tags") ? rc.Tags : rc.UserTags;
                                break;
                        }
                    }
                    Dictionary<string, List<string>> onlineTags = await ProcessOnlineTags(onlineTagData, onlineGenreData, entityType);
                    mbidTags.Add(currentMbid, onlineTags);
                }
            }
            return mbidTags;
        }

        private async Task<Dictionary<string, List<string>>> ProcessOnlineTags(List<MusicBrainzTag> onlineTagData, List<MusicBrainzTag> onlineGenreData = null, string entityType = "recording")
        {
            Dictionary<string, List<string>> combinedTagData = new Dictionary<string, List<string>>();

            // recording is the "default" for when separation by entity type is disabled...
            string genreField = Settings.Default.recordingGenreField;
            string tagField = Settings.Default.recordingTagField;

            if (Settings.Default.separateFieldsByEntityType)
            {
                if (entityType == "release-group")
                {
                    genreField = Settings.Default.releaseGroupGenreField;
                    tagField = Settings.Default.releaseGroupTagField;
                }
                else if (entityType == "release")
                {
                    genreField = Settings.Default.releaseGenreField;
                    tagField = Settings.Default.releaseTagField;
                }
                // ...no need to check recordings as a result.
            }

            List<string> tags = new List<string>();
            List<string> genres = new List<string>();

            if (Settings.Default.separateGenres)
            {
                if (onlineGenreData.Count > 0 || onlineGenreData != null)
                {
                    foreach (MusicBrainzTag mbzGenre in onlineGenreData)
                    {
                        Debug.WriteLine($"[MusicBrainzAPI.ProcessOnlineTags] Genre: {mbzGenre.Name}, FindReplaced: {FindReplaceTag(mbzGenre.Name, true)}");
                        genres.Add(FindReplaceTag(mbzGenre.Name, true));
                    }
                }
                combinedTagData.Add(genreField, genres);
            }

            if (onlineTagData.Count > 0)
            {
                foreach (MusicBrainzTag mbzTag in onlineTagData)
                {
                    string editedTag = FindReplaceTag(mbzTag.Name, true);
                    // if tag.Name in genres && genre grabbing enabled: break
                    if (genres.Contains(editedTag))
                    {
                        break;
                    }
                    else
                    {
                        Debug.WriteLine($"[MusicBrainzAPI.ProcessOnlineTags] Tag: {mbzTag.Name}, FindReplaced: {editedTag}");
                        tags.Add(editedTag);
                    }
                }
                combinedTagData.Add(tagField, tags);
            }

            // TODO check against other entity types and "advanced" config
            return combinedTagData;
        }

    }
}
