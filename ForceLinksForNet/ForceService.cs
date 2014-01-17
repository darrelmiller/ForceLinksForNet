using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ForceLinksForNet
{
    public class ForceService
    {
        private readonly HttpClient _httpClient;

        public ForceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IList<T>> Query<T>(string query)
        {
            var queryLink = new QueryLink()
            {
                Query = query
            };

            var response = await _httpClient.SendAsync(queryLink.CreateRequest());

            return await queryLink.ParseResponse<T>(response);
        }

        public async Task<T> QueryById<T>(string objectName, string recordId)
        {
            var queryLink = new QueryLink();
            queryLink.SetQuery<T>(objectName,recordId);

            var response = await _httpClient.SendAsync(queryLink.CreateRequest());

            var results = await queryLink.ParseResponse<T>(response);

            return results.FirstOrDefault();
        }

        public async Task<string> Create(string objectName, object record)
        {
            var createLink = new CreateLink()
            {
                ObjectName = objectName,
                Record = record
            };
            
            var response = await _httpClient.SendAsync(createLink.CreateRequest());

            var result = await createLink.ParseResponse(response);
            return result.id;
        }

        public async Task<bool> Update(string objectName, string recordId)
        {
            var createLink = new ObjectLink()
            {
                ObjectName = objectName,
                RecordId = recordId,
                Method = new HttpMethod("PATCH")
            };

            var response = await _httpClient.SendAsync(createLink.CreateRequest());

            return await createLink.ParseResponse(response);
        }

        public async Task<bool> Delete(string objectName, string recordId)
        {
            var createLink = new ObjectLink()
            {
                ObjectName = objectName,
                RecordId = recordId,
                Method = HttpMethod.Delete
            };

            var response = await _httpClient.SendAsync(createLink.CreateRequest());

            return await createLink.ParseResponse(response);
        }

        public async Task<IList<T>> GetObjects<T>()
        {
            var objectsLink = new ObjectsLink();

            var response = await _httpClient.SendAsync(objectsLink.CreateRequest());

            return await objectsLink.ParseResponse<T>(response);
        }

        public async Task<T> Describe<T>(string objectName)
        {
            var link = new CreateLink();

            var response = await _httpClient.SendAsync(link.CreateRequest());

            return await link.ParseNode<T>(response, "objectDescribe");
        }


        public static async Task<AuthLink> AuthClientUsernamePassword(string consumerKey, string consumerSecret, string username, string password)
        {
            var authLink = new AuthLink()
            {
                ClientId = consumerKey,
                ClientSecret = consumerSecret,
                Username = username,
                Password = password
            };

            var authClient = new HttpClient();
            var response = await authClient.SendAsync(authLink.CreateRequest());
            await authLink.ParseAccessToken(response);
            return authLink;
        }

        public static async Task<AuthLink> AuthClientWebServer(string consumerKey, string consumerSecret, string code, Uri redirectUri)
        {
            var authLink = new AuthLink()
            {
                ClientId = consumerKey,
                ClientSecret = consumerSecret,
                Code = code,
                RedirectUri = redirectUri
            };

            var authClient = new HttpClient();
            authClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("common-toolkit-dotnet", authLink.ApiVersion));
            var response = await authClient.SendAsync(authLink.CreateRequest());
            await authLink.ParseAccessToken(response);
            return authLink;
        }

        public static HttpClient CreateForceClient(AuthLink authLink, HttpMessageHandler innerHandler = null)
        {
            if (innerHandler == null) innerHandler = new HttpClientHandler();

            var httpClient = new HttpClient(innerHandler) { BaseAddress = authLink.InstanceUrl };

            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("forcedotcom-toolkit-dotnet", authLink.ApiVersion));
            // Auth headers should be defined at the HttpClient/MessageHandler layer because when using multiple realms and redirects, we need
            // auth header to be switched on the fly.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authLink.AccessToken);


            return httpClient;
        }

    }
}
