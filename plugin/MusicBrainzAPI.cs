using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace plugin
{
    using System.Diagnostics;
    using plugin.Properties;

    public class MusicBrainzAPI
    {
        // I am aware that a MusicBrainz API wrapper already exists, but it requires .NET Core which is not supported for MusicBee plugins AFAIK.

        /*
         * TODO:
         * Handle access tokens ephemerally.
         * See if Get keeps to JSON or not.
         * **/

        // # Initialisation stuff
        public string OAuthClientId = "7_dLhLIHweJ-vefrHOc-MKLHuNnBEl93";
        public string OAuthClientSecret = "oE1duN7a8eYrkIov6JBriFnzpXpoVN-J";
        // Desktop applications like this plugin don't need to hide their client secret.
        // MusicBrainz Picard's client secret is in its source code, so I'm following suit.
        // Complain to MusicBrainz about this, not me.
        public string MusicBrainzServer = "musicbrainz.org"; // Change this if you want to use a different server.
        public HttpClient MBzHttpClient;
        System.Threading.Tasks.Task<HttpResponseMessage> mbApiResponse;
        public string mbzAccessToken;

        internal class MusicBrainzOAuthData
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }

        public class MusicBrainzUser
        {
            // MusicBrainz reports the user name in the "sub" field, but we want to represent it as "username".
            [JsonProperty("sub")]
            public string username { get; set; }
        }

        public class MusicBrainzAPIError
        {
            // Class for handling errors from MusicBrainz that use JSON.
            public string error { get; set; }
            public string error_description { get; set; }
        }

        // # Initialisation
        public MusicBrainzAPI()
        {
            // initialise the HTTP client.
            MBzHttpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://{MusicBrainzServer}/oauth2/")
            };

            // check if the user is logged in or not.
            if (string.IsNullOrEmpty(Settings.Default.refreshToken))
            {
                Debug.WriteLine("No refresh token found, assuming user is logged out.");
            }
            else
            {
                bool userAuthenticated = AuthenticateUser().Result;
            }
        }

        // # HTTP connectivity functions
        internal void DisplayHTTPErrorMessage(System.Net.HttpStatusCode statusCode)
        {
            string errorMessage = "";
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    errorMessage = "MusicBrainz returned a 401 error. This means you haven't logged in properly.";
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    errorMessage = "MusicBrainz returned a 500 error. This is likely a server-side issue. Try again at a later time.";
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    string error = mbApiResponse.Result.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine("Fail Response: " + error);
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
                            errorMessage = $"MusicBrainz returned the following error that hasn't been properly caught yet: {mbErrorData.error}\n\n" +
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
                $"Error: {errorMessage}", "",
                MessageBoxButtons.OK, MessageBoxIcon.Error
                );
        }

        private async Task<string> GetFromMusicBrainz(string endpoint, string data = null, bool silent = false)
        {
            string refreshToken = Settings.Default.refreshToken;
            if (string.IsNullOrEmpty(refreshToken))
            {
                if (!silent) 
                { 
                MessageBox.Show(
                    "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }
            else
            {
                try
                {
                    mbApiResponse = MBzHttpClient.GetAsync(endpoint);
                    if (mbApiResponse.Result.IsSuccessStatusCode)
                    {
                        string result = await mbApiResponse.Result.Content.ReadAsStringAsync();
                        Debug.WriteLine("JSON Response: " + result);
                        // todo: does this output to XML as well? POST requires XML for certain.
                        return result;
                    }
                    else
                    {
                        DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                        return null;
                    }
                }
                catch (HttpRequestException e) // catch all other HTTP exceptions here.
                {
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
        }

        private async Task<string> PostToMusicBrainz(string endpoint, string data, string data_type = "application/json", bool silent = false)
        {
            string refreshToken = Settings.Default.refreshToken;
            if (string.IsNullOrEmpty(refreshToken) && (endpoint != "token")) // do not trigger on authentication requests
            {
                if (!silent)
                {
                    MessageBox.Show(
                        "You need to authenticate with MusicBrainz first. Please do so in the plugin settings.",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
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
                        Debug.WriteLine("JSON Response: " + result);
                        // todo: does this output to XML as well? POST requires XML for certain.
                        return result;
                    }
                    else
                    {
                        DisplayHTTPErrorMessage(mbApiResponse.Result.StatusCode);
                        return null;
                    }
                }
                catch (HttpRequestException e) // catch all other HTTP exceptions here.
                {
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public string GetAuthenticationURL()
        {
            // since MusicBrainzServer is declared here and not in Plugin.cs, best to have a function to generate the URL here.
            string authURL = $"{MBzHttpClient.BaseAddress}authorize?response_type=code&client_id={OAuthClientId}&scope=tag%20rating%20profile&redirect_uri=urn:ietf:wg:oauth:2.0:oob";
            return authURL;
        }

        // # User authentication functions
        public async Task<bool> AuthenticateUser(string userApiKey = null)
        {
            Debug.WriteLine($"AuthenticateUser {userApiKey}");
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

            string userauth = await PostToMusicBrainz("token", parameters, "application/x-www-form-urlencoded");
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
                Settings.Default.Save();
                MBzHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mbzAccessToken);
                return true;
            }

        }

        public async Task RevokeAccess()
        {
            string parameters = $"token={Settings.Default.refreshToken}&" +
                $"client_id={OAuthClientId}&" +
                $"client_secret={OAuthClientSecret}&";
            string revokeRequest = await PostToMusicBrainz("revoke", parameters, "application/x-www-form-urlencoded");
            Settings.Default.refreshToken = null;
            Settings.Default.Save();
        }

        // # User functions
        // Generally should only have GetUserName to display it in the plugin settings.
        public async Task<string> GetUserName()
        {
            string userinfo = await GetFromMusicBrainz("userinfo");
            if (!string.IsNullOrEmpty(userinfo))
            {
                MusicBrainzUser user = JsonConvert.DeserializeObject<MusicBrainzUser>(userinfo);
                return user.username;
            }
            else return null;
        }

    }
}
