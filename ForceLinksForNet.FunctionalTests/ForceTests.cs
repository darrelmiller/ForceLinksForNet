using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Salesforce.Common;

namespace ForceLinksForNet.FunctionalTests
{
    [TestFixture]
    public class ForceTests
    {
        private static string _tokenRequestEndpointUrl = ConfigurationSettings.AppSettings["TokenRequestEndpointUrl"];
        private static string _securityToken = ConfigurationSettings.AppSettings["SecurityToken"];
        private static string _consumerKey = ConfigurationSettings.AppSettings["ConsumerKey"];
        private static string _consumerSecret = ConfigurationSettings.AppSettings["ConsumerSecret"];
        private static string _username = ConfigurationSettings.AppSettings["Username"];
        private static string _password = ConfigurationSettings.AppSettings["Password"] + _securityToken;

        public async Task<HttpClient> CreateClient()
        {
            var authLink = await ForceService.AuthClientUsernamePassword(_consumerSecret, _consumerSecret, _username, _password);
            return ForceService.CreateForceClient(authLink);
        }

        [Test]
        public async void Query_Accounts_IsNotEmpty()
        {
            var client = await CreateClient();
            var queryLink = new QueryLink()
            {
                Query = "SELECT id, name, description FROM Account"
            };

            var response = await client.SendAsync(queryLink.CreateRequest());

            var accounts = queryLink.ParseResponse<Account>(response);

            Assert.IsNotNull(accounts);
        }

    }

    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
