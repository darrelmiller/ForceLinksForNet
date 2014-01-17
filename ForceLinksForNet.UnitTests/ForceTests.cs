using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace ForceLinksForNet.UnitTests
{
    public class ForceTests
    {

        [Test]
        public void Created_query_request()
        {
            var queryLink = new QueryLink()
            {
                Query = "SELECT id, name, description FROM Account"
            };

            var request = queryLink.CreateRequest();

            Assert.AreEqual("services/data/v29.0/query?q=SELECT%20id%2C%20name%2C%20description%20FROM%20Account", request.RequestUri.OriginalString);
            Assert.AreEqual(HttpMethod.Get,request.Method);
            Assert.IsNull(request.Content);
        }


  
    }

   }
