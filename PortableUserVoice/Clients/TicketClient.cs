﻿using Newtonsoft.Json;
using PortableUserVoice.Auth;
using PortableUserVoice.Data;
using PortableUserVoice.Clients;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortableUserVoice.Tokens;

namespace PortableUserVoice.Clients
{
    public class TicketClient
    {
        private static RestClient _client;

        public TicketClient()
        {
            _client = new RestClient();
        }

        #region get tickets and messages

        /// <summary>
        /// the response of the all user tickets request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="callbackUrl">ypur callback url</param>
        /// <param name="userMailAddress">the user's mail address</param>
        /// <param name="page">the page you want to show</param>
        /// <param name="perPage">the number of entries per page</param>
        /// <returns>the response for the all user ticket request</returns>
        private async Task<IRestResponse> GetAllUserTicketsResponse(string subdomain, string consumerKey, string consumerSecret, string callbackUrl, string userMailAddress, int page=1, int perPage = 10)
        {
            if (!AuthService.IsOwnerAuthenticated())
            {
                var owner = await AuthService.LoginAsOwner(subdomain, consumerKey, consumerSecret, callbackUrl);
            }

            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/tickets/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, OwnerTokens.AccessToken, OwnerTokens.AccessTokenSecret);

            RestRequest request = new RestRequest("search.json", HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.Subtract(new TimeSpan(0,0,1,0)).ToUniversalTime().ToString("R"));
            
            request.AddParameter("query", userMailAddress);
            request.AddParameter("page", page);
            request.AddParameter("per_page", perPage);

            return await _client.Execute(request);
        }


        /// <summary>
        /// gets all tickets for the specified user
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="callbackUrl">ypur callback url</param>
        /// <param name="userMailAddress">the user's mail address</param>
        /// <param name="page">the page you want to show</param>
        /// <param name="perPage">the number of entries per page</param>
        /// <returns>List of all tickets for the specified user</returns>
        public async Task<UserTicketsResult> GetAllUserTickets(string subdomain, string consumerKey, string consumerSecret, string callbackUrl, string userMailAddress, int page = 1, int perPage = 10)
        {
            IRestResponse response = await GetAllUserTicketsResponse(subdomain, consumerKey, consumerSecret, callbackUrl, userMailAddress, page, perPage);

            UserTicketsResult result = new UserTicketsResult();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                result = JsonConvert.DeserializeObject<UserTicketsResult>(responseContent);
            }

            return result;
        }



        /// <summary>
        /// request of a ticket's messages
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="callbackUrl">ypur callback url</param>
        /// <param name="ticketId">the id of the requested ticket messages</param>
        /// <param name="page">the page you want to show</param>
        /// <param name="perPage">the number of entries per page</param>
        /// <returns>the response for the request of a ticket's messages</returns>
        private async Task<IRestResponse> GetTicketMessagesResponse(string subdomain, int ticketId, string consumerKey, string consumerSecret, string callbackUrl, int page = 1, int perPage = 10)
        {
            if (!AuthService.IsOwnerAuthenticated())
            {
                var owner = await AuthService.LoginAsOwner(subdomain, consumerKey, consumerSecret, callbackUrl);
            }
            
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/tickets/{1}/", subdomain, ticketId));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, OwnerTokens.AccessToken, OwnerTokens.AccessTokenSecret);

            RestRequest request = new RestRequest("ticket_messages.json", HttpMethod.Get);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));

            request.AddParameter("page", page);
            request.AddParameter("per_page", perPage);

            return await _client.Execute(request);
        }

        /// <summary>
        /// requests the messages of a ticket
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="callbackUrl">ypur callback url</param>
        /// <param name="ticketId">the id of the requested ticket messages</param>
        /// <param name="page">the page you want to show</param>
        /// <param name="perPage">the number of entries per page</param>
        /// <returns>the messages of a ticket</returns>
        public async Task<TicketMessagesResult> GetTicketMessages(string subdomain, int ticketId, string consumerKey, string consumerSecret, string callbackUrl, int page = 1, int perPage = 10)
        {
            IRestResponse response = await GetTicketMessagesResponse(subdomain, ticketId, consumerKey, consumerSecret, callbackUrl, page, perPage);

            TicketMessagesResult result = new TicketMessagesResult();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                result = JsonConvert.DeserializeObject<TicketMessagesResult>(responseContent);
            }
            return result;            
        }
        #endregion



        #region create ticket
        /// <summary>
        /// the response for the new ticket request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <param name="email">the user's mail adress</param>
        /// <param name="subject">the ticket subject</param>
        /// <param name="message">the ticket message</param>
        /// <returns>the server resopnse for the new ticket request</returns>
        private async Task<IRestResponse> CreateNewTicketResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string email, string subject, string message)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/", subdomain));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            RestRequest request = new RestRequest("tickets.json", HttpMethod.Post);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));


            var ticket = new PostTicket
            {
                TicketDetail = new PostTicket.TicketDetails
                {
                    UserMailAddress = email,
                    Subject = subject,
                    Message = message,
                }
            };

            request.AddJsonBody(ticket);

            return await _client.Execute(request);            
        }

        /// <summary>
        /// creates a new ticket for the currently authenticated user
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <param name="email">the user's mail adress</param>
        /// <param name="subject">the ticket subject</param>
        /// <param name="message">the ticket message</param>
        /// <returns>the ticket creation date, ticket id and ticket number</returns>
        public async Task<NewTicketResult> CreateNew(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string email, string subject, string message)
        {
            NewTicketResult newTicket = new NewTicketResult();

            if (AuthService.IsUserAuthenticated())
            {
                IRestResponse response = await CreateNewTicketResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret, email, subject, message);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                    newTicket = JsonConvert.DeserializeObject<NewTicketResult>(responseContent);
                }
            }
            else
            {
                throw new UnauthorizedAccessException("user needs to be authenticated first.");
            }
            return newTicket;
        }



        /// <summary>
        /// the response for the new ticket request
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <param name="ticketId">the ticket id this message belongs to</param>
        /// <param name="message">the ticket message</param>
        /// <returns>the server resopnse for the new ticket message request</returns>
        private async Task<IRestResponse> CreateNewTicketMessageResponse(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int ticketId, string message)
        {
            _client.BaseUrl = new Uri(string.Format("https://{0}.uservoice.com/api/v1/tickets/{1}", subdomain, ticketId));
            _client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            RestRequest request = new RestRequest("ticket_messages.json", HttpMethod.Post);
            request.AddHeader("If-Modified-Since", DateTime.Now.ToUniversalTime().ToString("R"));

            var newmsg = new NewTicketMessage
            {
                NewMessage = new NewTicketMessage.TicketMessage()
                {
                    MessageText = message
                }
            };

            request.AddJsonBody(newmsg);
            

            return await _client.Execute(request);
        }

        /// <summary>
        /// creates a new ticket for the currently authenticated user
        /// </summary>
        /// <param name="subdomain">the site's subdomain</param>
        /// <param name="consumerKey">your consumerKey</param>
        /// <param name="consumerSecret">your conumserSecret</param>
        /// <param name="accessToken">the user's access token</param>
        /// <param name="accessTokenSecret">the user's access token secret</param>
        /// <param name="ticketId">the ticket id this message belongs to</param>
        /// <param name="message">the ticket message</param>
        /// <returns>the updated ticket</returns>
        public async Task<TicketWithCreatedMessagesResult> CreateNewMessage(string subdomain, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, int ticketId, string message)
        {
            TicketWithCreatedMessagesResult newMessagesResult = new TicketWithCreatedMessagesResult();

            if (AuthService.IsUserAuthenticated())
            {
                IRestResponse response = await CreateNewTicketMessageResponse(subdomain, consumerKey, consumerSecret, accessToken, accessTokenSecret, ticketId, message);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);

                    newMessagesResult = JsonConvert.DeserializeObject<TicketWithCreatedMessagesResult>(responseContent);
                }
            }
            else
            {
                throw new UnauthorizedAccessException("user needs to be authenticated first.");
            }
            return newMessagesResult;
        }


        
        
        
        
        #endregion





    }
}
