using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ForceLinksForNet
{
    public class ObjectsLink : ForceLink
    {
        public ObjectsLink()
        {
            Target = FormatUrl("sobjects");
        }
        public async Task<IList<T>> ParseResponse<T>(HttpResponseMessage responseMessage)
        {
            return await ParseNode<IList<T>>(responseMessage, "sobjects");
        }
    }
}