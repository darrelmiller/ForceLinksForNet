using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using NUnit.Framework;


namespace ForceLinksForNet.UnitTests
{
    public class CommonTests
    {
        [Test]
        public void Auth_CheckHttpRequestMessage_HttpGet()
        {

            var forceLink = new ForceLink()
            {
                ApiVersion = "v29",
                Resource = "wade"
            };

            var r = forceLink.CreateRequest();

            Assert.AreEqual(r.RequestUri.ToString(), "services/data/v29/wade");

            Assert.NotNull(r.Headers.UserAgent);
            Assert.AreEqual(r.Headers.UserAgent.ToString(), "common-libraries-dotnet/v29");


        }

[Test]
        public void Auth_CheckHttpRequestMessage_HttpGet_WithNode()
        {
           // N/A because Node is not part of the request
        }


[Test]
        public void Auth_CheckHttpRequestMessage_HttpPost()
        {

            var forceLink = new ForceLink()
            {
                ApiVersion = "v29",
                Resource = "wade",
                Method = HttpMethod.Post

            };

            var r = forceLink.CreateRequest();

            Assert.AreEqual(r.RequestUri.ToString(), "services/data/v29/wade");

            Assert.NotNull(r.Headers.UserAgent);
            Assert.AreEqual(r.Headers.UserAgent.ToString(), "common-libraries-dotnet/v29");


            Assert.AreEqual(HttpMethod.Post,r.Method);
        }

[Test]
        public void Auth_CheckHttpRequestMessage_HttpPatch()
        {
            var forceLink = new ForceLink()
            {
                ApiVersion = "v29",
                Resource = "wade",
                Method = new HttpMethod("PATCH")

            };

            var r = forceLink.CreateRequest();

            Assert.AreEqual(r.RequestUri.ToString(), "services/data/v29/wade");

            Assert.NotNull(r.Headers.UserAgent);
            Assert.AreEqual(r.Headers.UserAgent.ToString(), "common-libraries-dotnet/v29");

            Assert.NotNull(r.Headers.Authorization);
            Assert.AreEqual(r.Headers.Authorization.ToString(), "Bearer accessToken");

            Assert.AreEqual(new HttpMethod("PATCH"), r.Method);
        }

        [Test]
        public void Auth_CheckHttpRequestMessage_HttpDelete()
        {
            var forceLink = new ForceLink()
            {
                ApiVersion = "v29",
                Resource = "wade",
                Method = HttpMethod.Delete

            };

            var r = forceLink.CreateRequest();

            Assert.AreEqual(r.RequestUri.ToString(), "services/data/v29/wade");

            Assert.NotNull(r.Headers.UserAgent);
            Assert.AreEqual(r.Headers.UserAgent.ToString(), "common-libraries-dotnet/v29");


            Assert.AreEqual(HttpMethod.Delete, r.Method);
        }
    }
}
