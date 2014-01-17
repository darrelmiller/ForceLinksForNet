using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salesforce.Common;
using Salesforce.Common.Models;
using Tavis;

namespace ForceLinksForNet
{
    public class ForceLink : Link
    {
        private string _resource;

        public string ApiVersion { get; set; }
        

        public string Resource
        {
            get { return _resource; }
            set
            {
                _resource = value;
                SetParameter("resourcename", _resource);
            }
        }

        public ForceLink()
        {
            ApiVersion = "v29.0";
            Target = new Uri("services/data/{apiversion}/{resourcename}", UriKind.Relative);
        }

        protected Uri FormatUrl(string urlSuffix)
        {
                return new Uri("services/data/{apiversion}/" + urlSuffix, UriKind.Relative);
        }

        public override HttpRequestMessage CreateRequest()
        {
            
            SetParameter("apiversion", ApiVersion);
           
            var request =  base.CreateRequest();

            return request;
        }


        public async Task<T> ParseNode<T>(HttpResponseMessage responseMessage, string nodeName)
        {
            var response = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                var jObject = JObject.Parse(response);
                var jToken = jObject.GetValue(nodeName);

                var r = JsonConvert.DeserializeObject<T>(jToken.ToString());
                return r;
            }

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);
            throw new ForceException(errorResponse.errorCode, errorResponse.message);
        }

        protected static async Task<ForceException> ParseException(HttpResponseMessage responseMessage)
        {
            var response = await responseMessage.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);
            var exception = new ForceException(errorResponse.error, errorResponse.error_description);
            return exception;
        }
    }

}
