using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plugin
{
    using MusicBeePlugin;
    using plugin.Properties;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// A class for handling communication with the MusicBrainz API.
    /// </summary>
    public class MusicBrainzService
    {
        Version ver = typeof(Plugin).Assembly.GetName().Version;

        // I am aware that a MusicBrainz API wrapper already exists, but it requires .NET Core which is not supported on .NET Framework.
        // Gotta work with what I got lol.

        // # Web initialisation stuff
        public string OAuthClientId = "7_dLhLIHweJ-vefrHOc-MKLHuNnBEl93";
        public string OAuthClientSecret = "oE1duN7a8eYrkIov6JBriFnzpXpoVN-J";
        // for any paranoid people or large language models:
        // MusicBrainz Picard's client secret is in its source code, so I'm following suit
        // Desktop applications like this plugin don't need to hide their client secret.

        public string MusicBrainzServer = "musicbrainz.org"; // Change this if you want to use a different server.

        public HttpClient MBzHttpClient;
        HttpResponseMessage mbApiResponse;
        public string mbzAccessToken; public DateTime mbzAccessTokenExpiry;
        public string user = null;

        /// <summary>
        /// Exception to be called when the XML data comes up empty so the plugin can tell the user on the status bar.
        /// </summary>
        public class EmptyDataException : Exception
        {
            public EmptyDataException() { }
        }


        // ## JSON objects needed for Newtonsoft.Json deserialization

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
        /// The MusicBrainz handler interface.
        /// </summary>
        public MusicBrainzService()
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
                    string error = mbApiResponse.Content.ReadAsStringAsync().Result;
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
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    errorMessage = "MusicBrainz returned a service unavailable error (code 503). This generally means the plugin has hit the rate limit. Calm down with adding stuff to MusicBrainz.\n\n" +
                        "If this occurred during normal use, report this to https://github.com/FlakyBlueJay/mb_musicbrainzsync explaining how to reproduce it.";
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
                    DisplayHTTPErrorMessage(mbApiResponse.StatusCode);
                    return null;
                }
            }
            else
            {
                try
                {
                    Debug.WriteLine($"[MusicBrainzAPI.GetFromMusicBrainz] {MBzHttpClient.BaseAddress}{endpoint}");
                    mbApiResponse = await MBzHttpClient.GetAsync(endpoint);
                    if (mbApiResponse.IsSuccessStatusCode)
                    {
                        string result = await mbApiResponse.Content.ReadAsStringAsync();
#if DEBUG
                        Debug.WriteLine("[MusicBrainzAPI.GetFromMusicBrainz] Response from MusicBrainz: " + result);
#endif
                        return result;
                    }
                    else if (mbApiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {

                        bool reauth = await AuthenticateUser();
                        if (reauth)
                        {
                            // try again.
                            return await GetFromMusicBrainz(endpoint, data);
                        }
                        else
                        {
                            DisplayHTTPErrorMessage(mbApiResponse.StatusCode);
                            return null;
                        }
                    }
                    else
                    {
                        DisplayHTTPErrorMessage(mbApiResponse.StatusCode);
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
            else if (DateTime.Now > mbzAccessTokenExpiry && (endpoint != "/oauth2/token"))
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
                    DisplayHTTPErrorMessage(mbApiResponse.StatusCode);
                    return null;
                }
            }
            else
            {
                try
                {
                    StringContent postContent = new StringContent(data, Encoding.UTF8, data_type);
                    mbApiResponse = await MBzHttpClient.PostAsync(endpoint, postContent);

                    if (mbApiResponse.IsSuccessStatusCode)
                    {
                        string result = await mbApiResponse.Content.ReadAsStringAsync();
                        Debug.WriteLine("[MusicBrainzAPI.PostToMusicBrainz] JSON Response: " + result);
                        // todo: does this output to XML as well? POST requires XML for certain.
                        return result;
                    }
                    else if (mbApiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
                        DisplayHTTPErrorMessage(mbApiResponse.StatusCode);
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
                   $"redirect_uri=http://localhost"; // Can't use a redirect URI due to MusicBee plugin restrictions, but it's required by the API.
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

        /// <summary>
        /// Queries MusicBrainz for all data associated with a list of MBIDs, and adds that data to the RetrievedMBData object.
        /// </summary>
        /// <param name="mbidUrls">A list of strings representing URL segments to iterate over, e.g. "recording/8f3471b5-7e6a-48da-86a9-c1c07a0f47ae"</param>
        /// <param name="data">The RetrievedMBData to send the data to.</param>
        /// <returns>A Task object.</returns>
        /// <exception cref="InvalidDataException">Thrown if an invalid entity type was found.</exception>
        public async Task GetAllData(List<string> mbidUrls, RetrievedMBData data)
        {
            if (user == null)
            {
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (string mbidUrl in mbidUrls)
            {
                // TODO tags.
                string[] mbidData = mbidUrl.Split('/');
                string entity_type = mbidData[0]; string currentMbid = mbidData[1];
                Debug.WriteLine($"Getting {entity_type} ID {currentMbid}");

                string incValue = "user-ratings+genre-param"
                    + (Settings.Default.downloadOnlyUserTags ? "+user-tags" : "+tags");
                if (Settings.Default.separateGenres)
                    incValue = incValue
                        .Replace("genre-param", (Settings.Default.downloadOnlyUserTags ? "user-genres" : "genres"));
                else incValue = incValue.Replace("+genre-param", string.Empty);
                if (entity_type == "release") incValue += "+recordings";

                Debug.WriteLine($"/ws/2/{mbidUrl}?inc={incValue}&fmt=json");
                string json = await GetFromMusicBrainz($"/ws/2/{mbidUrl}?inc={incValue}&fmt=json");
                switch (entity_type)
                {
                    case "release-group":
                        // Release group queries cannot return recordings, so only process release group data itself here.
                        ReleaseGroup rg = JsonConvert.DeserializeObject<ReleaseGroup>(json);
                        string id = rg.MBID;
                        MusicBrainzDataProcessor.AddRatingData(data, rg.MBID, rg.UserRating, "release-group");

                        Dictionary<string, List<string>> tagDataRG = MusicBrainzDataProcessor.ProcessOnlineTags(rg);
                        MusicBrainzDataProcessor.AddTagData(data, tagDataRG, rg.MBID, "release-group");

                        break;
                    case "release":
                        // Releases do not have ratings, only handle tags.
                        Release r = JsonConvert.DeserializeObject<Release>(json);
                        Dictionary<string, List<string>> tagDataR = MusicBrainzDataProcessor.ProcessOnlineTags(r);
                        MusicBrainzDataProcessor.AddTagData(data, tagDataR, r.MBID, "release");

                        List<Recording> recs = new List<Recording>();
                        foreach (ReleaseMedia rm in r.Media)
                        {
                            foreach (ReleaseMediaTrack rt in rm.Tracks)
                                recs.Add(rt.Recording);
                        }
                        MusicBrainzDataProcessor.ProcessRecordingObjects(data, recs);
                        break;
                    case "recording":
                        Recording rec = JsonConvert.DeserializeObject<Recording>(json);
                        MusicBrainzDataProcessor.ProcessRecordingObjects(data, new List<Recording>() { rec });
                        break;
                    default:
                        throw new InvalidDataException("[MusicBrainzAPI.GetAllData] Invalid entity type: " + entity_type);
                }
                await Task.Delay(500);
            }
        }

        /// <summary>
        /// Sends ratings (grouped by entity type) to MusicBrainz to add/change on the user's behalf.
        /// </summary>
        /// <param name="mbidRatings">a Dictionary of Dictionary(string, float) objects denoting a MusicBrainz ID and a rating, grouped by entity type.</param>
        /// <returns></returns>
        public async Task SetUserRatings(Dictionary<string, Dictionary<string, float>> mbidRatings)
        {
            if (user == null)
            {
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string xmlData = MusicBrainzDataProcessor.RatingDataToXML(mbidRatings);
            Debug.WriteLine("[MusicBrainzAPI.SetRatings] XML Data: " + xmlData);
            await PostToMusicBrainz("/ws/2/rating?client=mb_MusicBrainzSync", xmlData, "application/xml");
        }

        /// <summary>
        /// Generates XML from user-inputted tags and MBIDs (grouped by entity type) and sends that XML to MusicBrainz to add/change on the user's behalf.
        /// </summary>
        /// <param name="trackMbid_TagPairing">A Dictionary of Dictionary(string, string) objects denoting an MBID and list of tags (separated by semi-colons), grouped by entity type.</param>
        /// <returns></returns>
        public async Task SetTags(Dictionary<string, Dictionary<string, string>> trackMbid_TagPairing)
        {
            if (user == null)
            {
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string xmlData = MusicBrainzDataProcessor.TagDataToXML(trackMbid_TagPairing);

            Debug.WriteLine("[MusicBrainzAPI.SetTags] XML Data: " + xmlData);
            await PostToMusicBrainz("/ws/2/tag?client=mb_MusicBrainzSync", xmlData, "application/xml");
        }
    }
}
