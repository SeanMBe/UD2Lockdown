using System.Net;

namespace UD2.HTTP
{
    public class HttpResponse
    {
        private readonly WebHeaderCollection headers;
        public HttpResponse(
            string requestUri,
            HttpStatusCode statusCode,
            WebHeaderCollection headers,
            string body)
        {
            this.headers = headers;
            this.RequestUri = requestUri;
            this.StatusCode = statusCode;
            this.Body = body;
        }

        public string GetResponseHeader(string responseHeader)
        {
            return this.headers[responseHeader];
        }

        public string GetResponseHeader(HttpResponseHeader responseHeader)
        {
            return this.headers[responseHeader];
        }

        public string RequestUri { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string Body { get; private set; }
    }
}