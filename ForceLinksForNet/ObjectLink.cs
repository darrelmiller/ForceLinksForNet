using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ForceLinksForNet
{
    public class ObjectLink : ForceLink
    {
        public string ObjectName { get; set; }
        public string RecordId { get; set; }

        public ObjectLink()
        {
            RequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public override HttpRequestMessage CreateRequest()
        {

            Target = FormatUrl("sobjects/{objectname}/{recordid}");
            SetParameter("objectname", ObjectName);
            SetParameter("recordid", RecordId);

            var request = base.CreateRequest();
            
            return request;
        }

        public async Task<bool> ParseResponse(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw await ParseException(responseMessage);;
            }
        }
    }
}