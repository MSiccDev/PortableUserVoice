# PortableUserVoice

PortableUserVoice makes it easy to integrate UserVoice features into your .NET application. It is a portable class library (profile 259), which make it availabe across a variety of project types, including Xamarin.iOS and Xamarin.Android.

There are four clients to integrate into your app:
- KnowledgeBaseClient
- SuggestionClient
- TicketClient
- UserClient

Every client provides the supported API requests and returns deserialized objects that you immediatelly can work with. All requests are asyncronous. Usage (example KnowledgeBaseClient):

var kbClient = new KnowledgeBaseClient();
var articlesResult = await kbClient.GetAllArticles(AppTokens.Subdomain, AppTokens.ConsumerKey, AppTokens.ConsumerSecret);

This returns the first 10 results from your knowledgebase.

Use the Tokens classes to store all tokens
- AppTokens = all app related tokens, like ConsumerKey, ConsumerSecret, subdomain
- OwnerTokens = the owner's access token and access token secret
- UserTokens = the user's access token and access token secret

Please make sure that all tokens are saved securely within your application.

The AuthService class helps your to easily authenticate users within your application. 

//get the authentication url to pass into the authentication request
var authUrl = await AuthService.GetAuthenticationUrl(AppTokens.Subdomain, AppTokens.ConsumerKey, AppTokens.ConsumerSecret, AppTokens.CallbackUrl);

//opens Windows Store app Authentication Broker
var authResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(authUrl), new Uri(AppTokens.CallbackUrl));

//after user has authenticated the app, get the AccessToken and AccessTokenSecret:
await AuthService.GetAccessToken(AppTokens.Subdomain, AppTokens.ConsumerKey, AppTokens.ConsumerSecret, authUrl, authResult.ResponseData);

This requests fills in the UserTokens.AccessToken and UserTokens.AccessTokenSecret. You just need to save them.

AuthService provides also the IsUserAuthenticated() method, which lets you check if you have saved the tokens already whenever you need an authenticated API call (like on SuggestionsClient or the UserClient methods).

The usage of the LoginAsOwner() method is quite similiar, except that you don't need the authentication step as you'll get the OwnerTokens directly from the UserVoice API. OwnerTokens are used for the TicketClient. The TicketClient is aware of this and calls the IsOwnerAuthenticated() method, and logs you in if needed (you should save the OwnerTokens as well to avoid unneccesary API calls).

The Utilities namespace has a experimental HTMLHelper to encode and decode html strings. 

All methods are fully commented, Visual Studio's IntelliSense will tell you all the parameters the methods can take or are required.












