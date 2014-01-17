using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ForceLinksForNet
{
    public class QueryLink : ForceLink
    {
        public string Query { get; set; }

        public void SetQuery<T>(string objectName, string recordId)
        {
            var fields = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
            Query = string.Format("SELECT {0} FROM {1} WHERE Id = '{2}'", fields, objectName, recordId);
        }


        public override HttpRequestMessage CreateRequest()
        {
            Target = FormatUrl("query{?q}");
            SetParameter("q",Query);

            return base.CreateRequest();
        }

        public async Task<IList<T>> ParseResponse<T>(HttpResponseMessage responseMessage )
        {
            return await ParseNode<IList<T>>(responseMessage, "records");
        }
    }
}
