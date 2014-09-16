using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Api.Infrastructure.Util
{
    public class JsonContent : StringContent
    {
        public JsonContent(object contentObject) : base(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json") { }
        public JsonContent(object contentObject, string mediaType) : base(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, mediaType) { }
        public JsonContent(string content) : base(content, Encoding.UTF8, "application/json") { }
        public JsonContent(string content, Encoding encoding) : base(content, encoding) { }
        public JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType) { }
    }
}
