using Newtonsoft.Json;
using PortableUserVoice.Auth;
using PortableUserVoice.Data;
using PortableUserVoice.Clients;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Clients
{
    public class UserClient
    {

        /// <summary>
        /// request to get the information about the user
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your conusmer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <returns>the response to the request to get the information about the user</returns>
        private async Task<IRestResponse> GetUserResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            RestClient client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/users/", subdomain));
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            RestRequest request = new RestRequest("current.json", HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


            return await client.Execute(request);
        }

        /// <summary>
        /// request to get the information about the user
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your conusmer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <returns>the UserResult including its properties</returns>
        public async Task<UserResult> GetUser(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            UserResult user = new UserResult();

            if (AuthService.IsUserAuthenticated())
            {
                IRestResponse response = await GetUserResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret);

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
                    string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
                    
                    user = JsonConvert.DeserializeObject<UserResult>(jsonContent);
                }
            }
            else
            {
                throw new UnauthorizedAccessException("user needs to be authenticated first.");
            }
            return user;
        }




    }
}
