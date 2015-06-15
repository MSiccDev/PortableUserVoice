using Newtonsoft.Json;
using PortableUserVoice.Data;
using PortableUserVoice.Clients;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortableUserVoice.Tokens;

namespace PortableUserVoice.Auth
{
    public class AuthService
    {

        #region user
        /// <summary>
        /// requests a fresh RequestToken from UserVoice
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="callBackUrl">your callback url</param>
        /// <returns>the response for the RequestToken request</returns>
        private static async Task<IRestResponse> RequestTokenResponse(string subdomain, string consumerKey, string consumerSecret, string callBackUrl)
        {
            RestClient client = new RestClient(string.Format("https://{0}.uservoice.com/oauth/", subdomain));
            client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret, callBackUrl);

            RestRequest request = new RestRequest("request_token", HttpMethod.Get);

            return await client.Execute(request);            
        }

        /// <summary>
        ///  request a fresh request token
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="callBackUrl">your callback url</param>
        /// <returns>the request token as string</returns>
        private static async Task<string> GetRequestToken(string subdomain, string consumerKey, string consumerSecret, string callBackUrl)
        {
            IRestResponse response = await RequestTokenResponse(subdomain, consumerKey, consumerSecret, callBackUrl);

            string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

            return responseContent;
        }



        /// <summary>
        /// gets the oAuth authentication url to be used in the login request (which has to be performed in your platform project)
        /// </summary>
        /// <param name="subdomain"></param>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="callBackUrl"></param>
        /// <returns>the oAuth authentication url to be used in the login request (which has to be performed in your platform project)</returns>
        public static async Task<string> GetAuthenticationUrl(string subdomain, string consumerKey, string consumerSecret, string callBackUrl)
        {
            string requestTokenString = await GetRequestToken(subdomain, consumerKey, consumerSecret, callBackUrl);

            string authUrl = string.Format("https://{0}.uservoice.com/oauth/authorize?{1}", subdomain, requestTokenString);

            return authUrl;
        }


        /// <summary>
        /// splits the RequestToken string for later user in access token request
        /// </summary>
        /// <param name="requestTokenString"></param>
        /// <returns>array of tokens</returns>
        private static string[] GetSplittedRequestTokens(string authUrl)
        {
            string request_token = null;
            string oauth_token_secret = null;
            string tokenPart = authUrl.Substring(authUrl.IndexOf('?'));
            string[] keyValPairs = tokenPart.Split('&');

            for (int i = 0; i < keyValPairs.Length; i++)
            {
                string[] splits = keyValPairs[i].Split('=');
                switch (splits[0])
                {
                    case "?oauth_token":
                        request_token = keyValPairs[i].Substring("?oauth_token=".Length);
                        break;

                    case "oauth_token_secret":
                        oauth_token_secret = keyValPairs[i].Substring("oauth_token_secret=".Length);
                        break;

                }
            }

            string[] retVals = new string[2];
            retVals[0] = request_token;
            retVals[1] = oauth_token_secret;
            return retVals;
        }


        /// <summary>
        /// splits the returned oAuthResponse into tokens
        /// </summary>
        /// <param name="oAuthResponse"></param>
        /// <returns>array with all token values</returns>
        private static string GetoAuthVerifier(string oAuthResponse)
        {
            string oauth_verifier = null;
            string[] keyValPairs = oAuthResponse.Split('&');

            for (int i = 0; i < keyValPairs.Length; i++)
            {
                string[] splits = keyValPairs[i].Split('=');
                switch (splits[0])
                {
                    case "oauth_verifier":
                        oauth_verifier = keyValPairs[i].Substring("oauth_verifier=".Length);
                        break;
                }
            }

            return oauth_verifier;
        }


        /// <summary>
        /// request AccessToken and AccessTokenSecret from UserVoice
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="authUrl">the result of the GetAuthenticationUrl() request</param>
        /// <param name="oAuthResponse">the returned oAuthResponse (after login)</param>
        /// <param name="callBackUrl">your callback url</param>
        /// <returns>the response for the AccessToken and AccessTokenSecret request</returns>
        private static async Task<IRestResponse> AccessTokenResponse(string subdomain, string consumerKey, string consumerSecret, string authUrl, string oAuthResponse)
        {
            var splittedTokens = GetSplittedRequestTokens(authUrl);
            var verifier = GetoAuthVerifier(oAuthResponse);
            

            RestClient client = new RestClient(string.Format("https://{0}.uservoice.com/oauth/", subdomain));
            client.Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey, consumerSecret, splittedTokens[0], splittedTokens[1], verifier);

            RestRequest request = new RestRequest("access_token", HttpMethod.Post);

            return await client.Execute(request);
        }

        /// <summary>
        /// request for getting the user's access token and access token secret
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="authUrl">the result of the GetAuthenticationUrl() request</param>
        /// <param name="oAuthResponse">the returned oAuthResponse (after login)</param>
        /// <param name="callBackUrl">your callback url</param>
        /// <returns>the UserTokens object containing both access token and access token secret</returns>
        public static async Task<UserTokens> GetAccessToken(string subdomain, string consumerKey, string consumerSecret, string authUrl, string oAuthResponse)
        {
            UserTokens tokens = new UserTokens();

            IRestResponse response = await AccessTokenResponse(subdomain, consumerKey, consumerSecret, authUrl, oAuthResponse);

            string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

            string oauth_token = null;
            string oauth_token_secret = null;
            string[] keyValPairs = responseContent.Split('&');

            for (int i = 0; i < keyValPairs.Length; i++)
            {
                string[] splits = keyValPairs[i].Split('=');
                switch (splits[0])
                {
                    case "oauth_token":
                        oauth_token = keyValPairs[i].Substring("oauth_token=".Length);
                        break;

                    case "oauth_token_secret":
                        oauth_token_secret = keyValPairs[i].Substring("oauth_token_secret=".Length);
                        break;

                }
            }

            UserTokens.AccessToken = oauth_token;
            UserTokens.AccessTokenSecret = oauth_token_secret;

            try
            {
                //Windows Platforms
                Debug.WriteLine("AccessToken and AccessTokenSecret received. Please make sure you are saving them savely in your app for further use.");
            }
            catch
            {
                //Xamarin Platforms
#if __MOBILE__
                Console.WriteLine("AccessToken and AccessTokenSecret received. Please make sure you are saving them savely in your app for further use.");
#endif
            }

            return tokens;
        }


        /// <summary>
        /// checks if there is already valid AccessToken and AccessTokenSecret
        /// </summary>
        /// <returns>bool that determins wheter the user is already authenticated</returns>
        public static bool IsUserAuthenticated()
        {
            bool value = false;

            if (!string.IsNullOrEmpty(UserTokens.AccessToken) && !string.IsNullOrEmpty(UserTokens.AccessTokenSecret))
            {
                value = true;
            }
            else
            {
                value = false;
            }

            return value;
        }
        #endregion



        #region owner

        /// <summary>
        /// request to log in as owner of the uservoice site
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="callBackUrl">your callback url</param>
        /// <returns>response for the request to log in as owner of the uservoice site</returns>
        private static async Task<IRestResponse> LoginAsOwnerResponse(string subdomain, string consumerKey, string consumerSecret, string callbackUrl)
        {
            RestClient client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/users/", subdomain));
            client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret, callbackUrl);

            RestRequest request = new RestRequest("login_as_owner", HttpMethod.Post);

            return await client.Execute(request);
        }


        /// <summary>
        /// request to log in as owner of the uservoice site
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="callBackUrl">your callback url</param>
        /// <returns>the owner result. owner tokens will be written to the OwnerTokens class</returns>
        public static async Task<OwnerResult> LoginAsOwner(string subdomain, string consumerKey, string consumerSecret, string callbackUrl)
        {
            OwnerResult ownerData = new OwnerResult();

            IRestResponse response = await LoginAsOwnerResponse(subdomain, consumerKey, consumerSecret, callbackUrl);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized:
                        
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        break;
                    default:

                        break;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
                ownerData = JsonConvert.DeserializeObject<OwnerResult>(responseContent);

                OwnerTokens.AccessToken = ownerData.token.oauth_token;
                OwnerTokens.AccessTokenSecret = ownerData.token.oauth_token_secret;  
            }

            return ownerData;
        }


        /// <summary>
        /// checks if there is already valid owner AccessToken and AccessTokenSecret
        /// </summary>
        /// <returns>bool that determins wheter the owner is already authenticated</returns>
        public static bool IsOwnerAuthenticated()
        {
            bool value = false;

            if (!String.IsNullOrEmpty(OwnerTokens.AccessToken) && !String.IsNullOrEmpty(OwnerTokens.AccessTokenSecret))
            {
                value = true;
            }
            else
            {
                value = false;
            }

            return value;
        }

        #endregion

    }
}
