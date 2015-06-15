using Newtonsoft.Json;
using PortableUserVoice.Data;
using PortableUserVoice.Clients;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;


namespace PortableUserVoice.Clients
{
    public class KnowledgeBaseClient
    {
        private static RestClient _client;

        public KnowledgeBaseClient()
        {
            _client = new RestClient();
        }

        #region allKB
        /// <summary>
        /// the response for the all KB request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>IRestResponse containing the requested data</returns>
        private async Task<IRestResponse> GetAllArticlesResponse(string subdomain, string consumerKey, string consumerSecret, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);
            
            RestRequest request = new RestRequest("articles.json", HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));

            request.AddParameter("page", page);
            request.AddParameter("per_page", per_page);
            request.AddParameter("filter", filter);
            request.AddParameter("sort", sort);

            return await _client.Execute(request);
        }

        /// <summary>
        /// async wrapper for the KnowledgeBase Model
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>data to be filled in the KnowledgeBase model</returns>
        public async Task<KnowledgeBaseResult> GetAllArticles(string subdomain, string consumerKey, string consumerSecret, int page= 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            KnowledgeBaseResult allKB = new KnowledgeBaseResult();

            IRestResponse response = await GetAllArticlesResponse(subdomain, consumerKey, consumerSecret, page, per_page, filter, sort);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                allKB = JsonConvert.DeserializeObject<KnowledgeBaseResult>(jsonContent);
            }

            return allKB;
        }


        /// <summary>
        /// the response for the search all KB request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>IRestResponse containing the requested data</returns>
        private async Task<IRestResponse> SearchAllArticlesResponse(string subdomain, string consumerKey, string consumerSecret, string query, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);
            
            RestRequest request = new RestRequest("search.json", HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


            request.AddParameter("page", page);
            request.AddParameter("query", query);
            request.AddParameter("per_page", per_page);
            request.AddParameter("filter", filter);
            request.AddParameter("sort", sort);

            return await _client.Execute(request);
        }

        /// <summary>
        /// async wrapper for the KnowledgeBase Model
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>data to be filled in the KnowledgeBase model when KnowledgeBase is searched</returns>
        public async Task<KnowledgeBaseResult> SearchAllArticles(string subdomain, string consumerKey, string consumerSecret, string query, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            KnowledgeBaseResult allKB = new KnowledgeBaseResult();

            IRestResponse response = await SearchAllArticlesResponse(subdomain, consumerKey, consumerSecret, query, page, per_page, filter, sort);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                allKB = JsonConvert.DeserializeObject<KnowledgeBaseResult>(jsonContent);
            }

            return allKB;
        }
        #endregion

        #region KB Topic
        /// <summary>
        /// the response for the KB topic request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="topicId">the requested topic's id</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>IRestResponse containing the requested data</returns>
        private async Task<IRestResponse> GetTopicArticlesResponse(string subdomain, string consumerKey, string consumerSecret, int topicId, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

            RestRequest request = new RestRequest(string.Format("topics/{0}/articles.json", topicId), HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


            request.AddParameter("page", page);
            request.AddParameter("per_page", per_page);
            request.AddParameter("filter", filter);
            request.AddParameter("sort", sort);

            return await _client.Execute(request);
        }

        /// <summary>
        /// async wrapper for the KnowledgeBase Model based on the specified topicId
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="topicId">the requested topic's id</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>data to be filled in the KnowledgeBase model based on the specified topicId</returns>
        public async Task<KnowledgeBaseResult> GetTopic(string subdomain, string consumerKey, string consumerSecret, int topicId, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            KnowledgeBaseResult allKB = new KnowledgeBaseResult();

            IRestResponse response = await GetTopicArticlesResponse(subdomain, consumerKey, consumerSecret, topicId, page, per_page, filter, sort);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                allKB = JsonConvert.DeserializeObject<KnowledgeBaseResult>(jsonContent);
            }

            return allKB;
        }

        /// <summary>
        /// the response for the KB topic search request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="topicId">the requested topic's id</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>IRestResponse containing the requested data</returns>
        private async Task<IRestResponse> SearchTopicArticlesResponse(string subdomain, string consumerKey, string consumerSecret, int topicId, string query, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

            RestRequest request = new RestRequest(string.Format("topics/{1}/articles/search.json", topicId) , HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));

            
            request.AddParameter("page", page);
            request.AddParameter("query", query);
            request.AddParameter("per_page", per_page);
            request.AddParameter("filter", filter);
            request.AddParameter("sort", sort);

            return await _client.Execute(request);
        }

        /// <summary>
        /// async wrapper for the KnowledgeBase Model based on the specified topicId
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="topicId">the requested topic's id</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <param name="filter">how the request response should be filtered</param>
        /// <param name="sort">the order in which the response should be sorted</param>
        /// <returns>data to be filled in the KnowledgeBase model based on the specified topicId and search query</returns>
        public async Task<KnowledgeBaseResult> SearchTopic(string subdomain, string consumerKey, string consumerSecret, int topicId, string query, int page = 1, int per_page = 10, ArticlesFilter filter = ArticlesFilter.all, ArticlesSort sort = ArticlesSort.newest)
        {
            KnowledgeBaseResult allKB = new KnowledgeBaseResult();

            IRestResponse response = await SearchTopicArticlesResponse(subdomain, consumerKey, consumerSecret, topicId, query, page, per_page, filter, sort);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                allKB = JsonConvert.DeserializeObject<KnowledgeBaseResult>(jsonContent);
            }

            return allKB;
        }
        #endregion

        #region article
        /// <summary>
        /// the response of the mark as helpful request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="articleId">the article's id</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <returns>IRestResponse containing the response for the requested data</returns>
        private async Task<IRestResponse> MarkArticleHelpfulResponse(string subdomain, int articleId, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            RestRequest request = new RestRequest("useful.json", HttpMethod.Post);

            return await _client.Execute(request);
        }

        /// <summary>
        /// marks the selected article as helpful
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="articleId">the article's id</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <returns>boolean that indicates if the article was marked as helpful</returns>
        public async Task<bool> MarkArticleHelpful(string subdomain, int articleId, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            bool wasSuccessful = false;

            IRestResponse response = await MarkArticleHelpfulResponse(subdomain, articleId, consumerKey, consumerSecret, accessToken, accessTokenSecret);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                wasSuccessful = true;
            }

            return wasSuccessful;
        }

        #endregion



        /// <summary>
        /// the response for the all KB topics request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <returns>IRestResponse containing the requested data</returns>
        private async Task<IRestResponse> GetAllTopicsResponse(string subdomain, string consumerKey, string consumerSecret, int page = 1, int per_page = 10)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, null, null);

            RestRequest request = new RestRequest("topics.json", HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));
            

            request.AddParameter("page", page);
            request.AddParameter("per_page", per_page);

            return await _client.Execute(request);
        }

        /// <summary>
        /// async wrapper for the KnowledgeBase topics model
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumer key</param>
        /// <param name="consumerSecret">your consumer secret</param>
        /// <param name="page">the page number</param>
        /// <param name="per_page">the number of articles per page</param>
        /// <returns>data to be filled in the KnowledgeBase topics model</returns>
        public async Task<KnowledgeBaseTopicsResult> GetAllTopics(string subdomain, string consumerKey, string consumerSecret, int page = 1, int per_page = 10)
        {
            KnowledgeBaseTopicsResult allKbTopics = new KnowledgeBaseTopicsResult();

            IRestResponse response = await GetAllTopicsResponse(subdomain, consumerKey, consumerSecret, page, per_page);

            if (response.IsSuccess)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string jsonContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                    allKbTopics = JsonConvert.DeserializeObject<KnowledgeBaseTopicsResult>(jsonContent);
                }
            }
            return allKbTopics;
        }



        public enum ArticlesFilter
        {
            [DefaultValue(true)]
            all = 1,
            published = 2, 
            unpublished =3
        }

        public enum ArticlesSort
        {
            //instant_answers = 1,
            [DefaultValue(true)]
            newest = 2, 
            oldest =3
        }


    }
}
