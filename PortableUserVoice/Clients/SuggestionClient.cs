using Newtonsoft.Json;
using PortableUserVoice.Auth;
using PortableUserVoice.Data;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Clients
{
     public class SuggestionClient
     {
         private static RestClient _client;

         public SuggestionClient()
         {
             
         }

         /// <summary>
         /// Gets a list of all forums for a UserVoice account
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="consumerKey">your consumer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <returns>IRestResponse for all forums of a Uservoice account</returns>
         private async Task<IRestResponse> GetAllForumsResponse(string subdomain, string consumerKey, string consumerSecret)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

             RestRequest request = new RestRequest("forums.json", HttpMethod.Get);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));

             return await _client.Execute(request);
         }


         /// <summary>
         /// Gets a list of all forums for a UserVoice account
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="consumerKey">your consumer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <returns>all forums of a Uservoice account</returns>
         public async Task<ForumsResult> GetAllForums(string subdomain, string consumerKey, string consumerSecret)
         {
             ForumsResult result = new ForumsResult();

             IRestResponse response = await GetAllForumsResponse(subdomain, consumerKey, consumerSecret);

             if (response.StatusCode == HttpStatusCode.OK)
             {
                 string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                 result = JsonConvert.DeserializeObject<ForumsResult>(jsonContent);
             }
             return result;
         }


         /// <summary>
         /// Gets all suggestions statuses
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="consumerKey">your consumer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <returns>IRestResponse for all suggestions statuses of a Uservoice account</returns>
         private async Task<IRestResponse> GetSuggestionStatusesResponse(string subdomain, string consumerKey, string consumerSecret)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

             RestRequest request = new RestRequest("statuses.json", HttpMethod.Get);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));

             return await _client.Execute(request);
         }

         /// <summary>
         /// Gets all suggestions statuses
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="consumerKey">your consumer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <returns> all suggestions statuses of a Uservoice account</returns>
         public async Task<SuggestionStatusesResult> GetSuggestionsStatuses(string subdomain, string consumerKey, string consumerSecret)
         {
             SuggestionStatusesResult result = new SuggestionStatusesResult();

             IRestResponse response = await GetSuggestionStatusesResponse(subdomain, consumerKey, consumerSecret);

             if (response.StatusCode == HttpStatusCode.OK)
             {
                 string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                 result = JsonConvert.DeserializeObject<SuggestionStatusesResult>(jsonContent);
             }
             return result;
         }





         #region list suggestions
         /// <summary>
         /// request all suggestions in a forum
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="page">the result's page</param>
         /// <param name="perPage">number of entries per page</param>
         /// <param name="sort">the status used to order results</param>
         /// <returns>the response for the request of all suggestions in a forum</returns>
         private async Task<IRestResponse> GetAllSuggestionsResponse(string subdomain, string consumerKey, string consumerSecret, int forumId, int page = 1, int perPage = 10, SuggestionSort sort = SuggestionSort.newest, SuggestionFilter filter = SuggestionFilter.all, SuggestionStatusesResult.Status status = null)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/forums/{1}/", subdomain, forumId));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

             RestRequest request = new RestRequest("suggestions.json", HttpMethod.Get);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


             request.AddParameter("sort", sort);
             request.AddParameter("filter", filter);
             if (status != null && status.id != 0)
             {
                 request.AddParameter("status_id", status.id);
             }
             request.AddParameter("page", page);
             request.AddParameter("per_page", perPage);
             

             return await _client.Execute(request);
         }

         /// <summary>
         /// requests all suggestions in a forum
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="page">the result's page</param>
         /// <param name="perPage">number of entries per page</param>
         /// <param name="sort">the status used to order results</param>
         /// <returns>all suggestions in a forum</returns>
         public async Task<SuggestionsResult> GetAllSuggesstions(string subdomain, string consumerKey, string consumerSecret, int forumId, int page = 1, int perPage = 10, SuggestionSort sort = SuggestionSort.newest, SuggestionFilter filter = SuggestionFilter.all, SuggestionStatusesResult.Status status = null)
         {
             SuggestionsResult result = new SuggestionsResult();

             IRestResponse response = await GetAllSuggestionsResponse(subdomain, consumerKey, consumerSecret, forumId, page, perPage, sort, filter, status);

             if (response.StatusCode == HttpStatusCode.OK)
             {
                 string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                 result = JsonConvert.DeserializeObject<SuggestionsResult>(jsonContent);
             }

             return result;
         }



         /// <summary>
         /// request to search all suggestions in a forum
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="query">the search query</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="page">the result's page</param>
         /// <param name="perPage">number of entries per page</param>
         /// <param name="sort">the status used to order results</param>
         /// <returns>the response for the search request of all suggestions in a forum</returns>
         private async Task<IRestResponse> GetSearchSuggestionsResponse(string subdomain, string query, string consumerKey, string consumerSecret, int forumId, int page = 1, int perPage = 10, SuggestionSort sort = SuggestionSort.newest, SuggestionStatusesResult.Status status = null)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/forums/{1}/suggestions/", subdomain, forumId));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

             RestRequest request = new RestRequest("search.json", HttpMethod.Get);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


             request.AddParameter("query", query);
             request.AddParameter("sort", sort);
             if (status != null && status.id != 0)
             {
                 request.AddParameter("status_id", status.id);
             }
             request.AddParameter("page", page);
             request.AddParameter("per_page", perPage);

             return await _client.Execute(request);
         }

         /// <summary>
         /// searches all suggestions in a forum
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="query">the search query</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="page">the result's page</param>
         /// <param name="perPage">number of entries per page</param>
         /// <param name="sort">the status used to order results</param>         
         /// <returns>all suggestions that match the search query in a forum</returns>
         public async Task<SuggestionsResult> SearchSuggesstions(string subdomain, string query, string consumerKey, string consumerSecret, int forumId, int page = 1, int perPage = 10, SuggestionSort sort = SuggestionSort.newest, SuggestionStatusesResult.Status status = null)
         {
             SuggestionsResult result = new SuggestionsResult();

             IRestResponse response = await GetSearchSuggestionsResponse(subdomain, query, consumerKey, consumerSecret, forumId, page, perPage, sort, status);

             if (response.StatusCode == HttpStatusCode.OK)
             {
                 string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                 result = JsonConvert.DeserializeObject<SuggestionsResult>(jsonContent);
             }

             return result;
         }
#endregion


         #region create and vote on suggestions
         /// <summary>
         /// request to post a new suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="title">the suggestion title</param>
         /// <param name="votes">the number of votes that should be applied to the new suggestion</param>
         /// <param name="text">additional comments on the suggestion</param>
         /// <param name="referrer">the referrer, e. g. your app's name</param>
         /// <returns>response for the request to post a new suggestion</returns>
         private async Task<IRestResponse> PostCreateSuggestionResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, string title, int votes, string text = null, string referrer = null)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/forums/{1}/", subdomain, forumId));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

             RestRequest request = new RestRequest("suggestions.json", HttpMethod.Post);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


             request.AddParameter("suggestion[title]", title);
             request.AddParameter("suggestion[votes]", votes);
             if (!String.IsNullOrEmpty(text))
             {
                 request.AddParameter("suggestion[text]", text);
             }             
             if (!String.IsNullOrEmpty(referrer))
             {
                 request.AddParameter("suggestion[referrer]", referrer);
             }

             return await _client.Execute(request);
         }

         /// <summary>
         /// posts a new suggestion
         /// </summary>
         /// <param name="subdomain"></param>
         /// <param name="forumId"></param>
         /// <param name="consumerKey"></param>
         /// <param name="consumerSecret"></param>
         /// <param name="accessToken"></param>
         /// <param name="accessTokenSecret"></param>
         /// <param name="title"></param>
         /// <param name="votes"></param>
         /// <param name="text"></param>
         /// <param name="referrer"></param>
         /// <returns>the posted suggestion including its properties</returns>
         public async Task<SingleSuggestionResult> CreateSuggestion(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, string title, int votes, string text = null, string referrer = null)
         {
             SingleSuggestionResult result = new SingleSuggestionResult();


             if (AuthService.IsUserAuthenticated())
             {
                 IRestResponse response = await PostCreateSuggestionResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret, forumId, title, votes, text, referrer);

                 string jsonContent = string.Empty;


                 if (response.StatusCode == HttpStatusCode.OK)
                 {
                     jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                     result = JsonConvert.DeserializeObject<SingleSuggestionResult>(jsonContent);
                 }
             }
             else
             {
                 throw new UnauthorizedAccessException("user needs to be authenticated first.");
             }


             return result;
         }



         /// <summary>
         /// request to post votes on a suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="suggestionId">the suggestion's id</param>
         /// <param name="votes">the number of votes that should be applied to the suggestion</param>
         /// <returns>response for the request to post votes on a suggestion</returns>
         private async Task<IRestResponse> PostVoteOnSuggestionResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, int suggestionId, int votes)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{2}/", subdomain, forumId, suggestionId));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

             RestRequest request = new RestRequest("votes.json", HttpMethod.Post);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


             request.AddParameter("to", votes);

             return await _client.Execute(request);
         }

         /// <summary>
         /// request to post votes on a suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="suggestionId">the suggestion's id</param>
         /// <param name="votes">the number of votes that should be applied to the suggestion</param>
         /// <returns>updated suggestion with new count of votes</returns>
         public async Task<SingleSuggestionResult> VoteOnSuggestion(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, int suggestionId, int votes)
         {
             SingleSuggestionResult result = new SingleSuggestionResult();

             if (AuthService.IsUserAuthenticated())
             {
                 IRestResponse response = await PostVoteOnSuggestionResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret, forumId, suggestionId, votes);

                 string jsonContent = string.Empty;

                 if (response.StatusCode == HttpStatusCode.OK)
                 {
                     jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                     result = JsonConvert.DeserializeObject<SingleSuggestionResult>(jsonContent);
                 }
             }
             else
             {
                 throw new UnauthorizedAccessException("user needs to be authenticated first.");
             }

             return result;
         }
#endregion


         #region comments
         /// <summary>
         /// request to get all comments of a suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="suggestionId">the suggestion's id</param>
         /// <param name="page">the result's page</param>
         /// <param name="perPage">number of entries per page</param>
         /// <returns>the response for the request to get all comments of a suggestion</returns>
         private async Task<IRestResponse> GetCommentsForSuggestionResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, int suggestionId, int page = 1, int perPage = 10)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{2}/", subdomain, forumId, suggestionId));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

             RestRequest request = new RestRequest("comments.json", HttpMethod.Get);
             request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


             request.AddParameter("page", page);
             request.AddParameter("per_page", perPage);

             return await _client.Execute(request);
         }

         /// <summary>
         /// request to get all all comments of a suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="suggestionId">the suggestion's id</param>
         /// <param name="page">the result's page</param>
         /// <param name="perPage">number of entries per page</param>
         /// <returns>List with all comments of a suggestion </returns>
         public async Task<SuggestionCommentsResult> GetCommentsForSuggestion(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, int suggestionId, int page = 1, int perPage = 10)
         {
             SuggestionCommentsResult result = new SuggestionCommentsResult();

             if (AuthService.IsUserAuthenticated() || AuthService.IsOwnerAuthenticated())
             {
                 IRestResponse response = await GetCommentsForSuggestionResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret, forumId, suggestionId, page, perPage);

                 if (response.StatusCode == HttpStatusCode.OK)
                 {
                     string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                     result = JsonConvert.DeserializeObject<SuggestionCommentsResult>(jsonContent);
                 }
             }
             else
             {
                 throw new UnauthorizedAccessException("user needs to be authenticated first.");
             }
             return result;
         }
     


         /// <summary>
         /// request to post a comment on a suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="suggestionId">the suggestion's id</param>
         /// <param name="comment">the comment text</param>
         /// <returns>the response for the request to post a comment on a suggestion</returns>
         private async Task<IRestResponse> PostCommentOnSuggestionResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, int suggestionId, string comment)
         {
             _client = new RestClient(string.Format("https://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{2}/", subdomain, forumId, suggestionId));
             _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

             RestRequest request = new RestRequest("comments.json", HttpMethod.Post);

             request.AddParameter("comment[text]", comment);

             return await _client.Execute(request);
         }

         /// <summary>
         /// posts a comment on the specified suggestion
         /// </summary>
         /// <param name="subdomain">the site's subdomain</param>
         /// <param name="forumId">the forumId</param>
         /// <param name="consumerKey">your conusmer key</param>
         /// <param name="consumerSecret">your consumer secret</param>
         /// <param name="accessToken">the user's access token</param>
         /// <param name="accessTokenSecret">the user's access token secret</param>
         /// <param name="suggestionId">the suggestion's id</param>
         /// <param name="comment">the comment to post</param>
         /// <returns>the posted comment including its properties</returns>
         public async Task<SingleCommentResult> PostCommentOnSuggestion(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int forumId, int suggestionId, string comment)
         {
             SingleCommentResult result = new SingleCommentResult();

             if (AuthService.IsUserAuthenticated())
             {
                 IRestResponse response = await PostCommentOnSuggestionResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret, forumId, suggestionId, comment);

                 if (response.StatusCode == HttpStatusCode.OK)
                 {
                     string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                     result = JsonConvert.DeserializeObject<SingleCommentResult>(jsonContent);
                 }

                 return result;
             }
             else
             {
                 throw new UnauthorizedAccessException("user needs to be authenticated first.");
             }

         }
#endregion



         /// <summary>
         /// all possible suggestion sorting options
         /// </summary>
         public enum SuggestionSort
         {
             votes =1,
             hot = 2,
             oldest =3,
             newest = 4,
             supporters = 5
         }

         /// <summary>
         /// all possible suggestion filters
         /// </summary>
         public enum SuggestionFilter
         {
             all = 1,
             published = 2, 
             active = 3, 
             closed = 4, 
             deleted = 5, 
             inbox = 6, 
             spam = 7, 
             merged = 8,
         }
     }
}
