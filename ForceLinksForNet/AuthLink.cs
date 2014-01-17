using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Salesforce.Common;
using Salesforce.Common.Models;
using Tavis;

namespace ForceLinksForNet
{
   
    public class AuthLink : ForceLink
    {

        public AuthLink()
        {
           
            Target = new Uri("https://login.salesforce.com/services/oauth2/token");
        }


        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Uri RedirectUri { get; set; }
        public string Code { get; set; }

        public Uri InstanceUrl { get; set; }
        public string AccessToken { get; set; }

        public override HttpRequestMessage CreateRequest()
        {
            var request = base.CreateRequest();

            request.Method = HttpMethod.Post;
            request.Content = CreateBodyContent();

            return request;
        }

        private HttpContent CreateBodyContent()
        {
            var authParams = new List<KeyValuePair<string, string>>(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret)
            });


            if (!String.IsNullOrEmpty(Username))
            {
                authParams.Add(new KeyValuePair<string, string>("username", Username));
                authParams.Add(new KeyValuePair<string, string>("password", Password));
            }
            else
            {
                authParams.Add(new KeyValuePair<string, string>("redirect_uri", RedirectUri.OriginalString));
                authParams.Add(new KeyValuePair<string, string>("code", Code));
            }

            return new FormUrlEncodedContent(authParams);
        }


        public async Task ParseAccessToken(HttpResponseMessage responseMessage)
        {
            var response = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                var authToken = JsonConvert.DeserializeObject<AuthToken>(response);

                AccessToken = authToken.access_token;
                InstanceUrl = new Uri(authToken.instance_url);
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);
                throw new ForceException(errorResponse.error, errorResponse.error_description);
            }
        }
    }

   
}
