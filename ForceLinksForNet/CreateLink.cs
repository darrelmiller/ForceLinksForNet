using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Salesforce.Common;
using Salesforce.Common.Models;

namespace ForceLinksForNet
{
    public class CreateLink : ForceLink
    {
        public string ObjectName { get; set; }
        public object Record { get; set; }

        public CreateLink()
        {
            RequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public override HttpRequestMessage CreateRequest()
        {

            Target = FormatUrl("sobjects/{objectname}");
            SetParameter("objectname",ObjectName);

            var request = base.CreateRequest();

            request.Method = HttpMethod.Post;

            var json = JsonConvert.SerializeObject(Record, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return request;
        }

        public async Task<SuccessResponse> ParseResponse(HttpResponseMessage responseMessage)
        {
            return await ParseNode<SuccessResponse>(responseMessage, "records");
        }
    }
}