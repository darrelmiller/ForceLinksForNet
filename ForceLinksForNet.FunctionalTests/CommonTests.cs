using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Net;

namespace ForceLinksForNet.FunctionalTests
{
    public class CommonTests
    {
        private static string _tokenRequestEndpointUrl = ConfigurationSettings.AppSettings["TokenRequestEndpointUrl"];
        private static string _securityToken = ConfigurationSettings.AppSettings["SecurityToken"];
        private static string _consumerKey = ConfigurationSettings.AppSettings["ConsumerKey"];
        private static string _consumerSecret = ConfigurationSettings.AppSettings["ConsumerSecret"];
        private static string _username = ConfigurationSettings.AppSettings["Username"];
        private static string _password = ConfigurationSettings.AppSettings["Password"] + _securityToken;

        [Test]
        public void Auth_ValidCreds_HasApiVersion()
        {
            var auth = new AuthLink();
            Assert.IsNotNullOrEmpty(auth.ApiVersion);
        }

        [Test]
        public async void Auth_ValidCreds_HasAccessToken()
        {
            var authLink = new AuthLink()
            {
                ClientId = _consumerKey,
                ClientSecret = _consumerSecret,
                Username = _username,
                Password = _password
            };
            
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(authLink.CreateRequest());
            await authLink.ParseAccessToken(response);

            Assert.IsNotNullOrEmpty(authLink.AccessToken);
        }

        [Test]
        public async void Auth_ValidCreds_HasInstanceUrl()
        {
         
            var authLink = new AuthLink()
            {
                ClientId = _consumerKey,
                ClientSecret = _consumerSecret,
                Username = _username,
                Password = _password
            };

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(authLink.CreateRequest());
            await authLink.ParseAccessToken(response);

            Assert.IsNotNull(authLink.InstanceUrl);
        }
    }
}
