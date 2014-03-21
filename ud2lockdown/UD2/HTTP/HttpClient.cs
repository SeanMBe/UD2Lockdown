using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace UD2.HTTP
{
    public class HttpClient
    {
        public HttpClient()
        {
            this.Timeout = TimeSpan.FromSeconds(60);
        }
        public string ContentType { get; set; }
        public ICredentials Credentials { get; set; }
        public TimeSpan Timeout { get; set; }
        public HttpResponse Get(string uri)
        {
            return this.Get(uri, null);
        }

        public HttpResponse Get(string uri, NameValueCollection headers)
        {
            return GetResponse(this.CreateRequest(uri, headers));
        }
        public HttpResponse Put(string uri, XElement payload)
        {
            return this.Put(uri, payload, null);
        }

        public HttpResponse Put(string uri, XElement payload, NameValueCollection headers)
        {
            return this.PostByMethod("PUT", uri, payload, headers);
        }

        public HttpResponse Delete(string uri)
        {
            return this.Delete(uri, null, null);
        }

        public HttpResponse Delete(string uri, NameValueCollection headers)
        {
            return this.Delete(uri, null, headers);
        }

        public HttpResponse Delete(string uri, XElement payload)
        {
            return this.Delete(uri, payload, null);
        }
        public HttpResponse Delete(string uri, XElement payload, NameValueCollection headers)
        {
            return this.PostByMethod("DELETE", uri, payload, headers);
        }

        public HttpResponse Post(string uri)
        {
            return this.Post(uri, null, null);
        }

        public HttpResponse Post(string uri, XElement payload)
        {
            return this.Post(uri, payload, null);
        }

        public HttpResponse Post(string uri, XElement payload, NameValueCollection headers)
        {
            return this.PostByMethod("POST", uri, payload, headers);
        }

        private HttpResponse PostByMethod(string method, string uri, XElement payload, NameValueCollection headers)
        {
            var request = this.CreateRequest(uri, headers);
            request.Method = "POST";
            request.ContentType = this.ContentType ?? "text/xml";
            if (!string.Equals("POST", method, StringComparison.InvariantCultureIgnoreCase))
            {
                request.Headers.Add("X-HTTP-Method-Override", method);
            }
            WritePayload(request, payload);
            return GetResponse(request);
        }
        private static HttpResponse GetResponse(WebRequest request)
        {
            try
            {
                return CreateResponseFrom(request.RequestUri.ToString(), request.GetResponse());
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.Timeout) throw new TimeoutException(e.ToString());
                if (e.Response != null) return CreateResponseFrom(request.RequestUri.ToString(), e.Response);
                throw;
            }
        }

        private WebRequest CreateRequest(string uri, NameValueCollection headers)
        {
            var request = WebRequest.Create(uri);
            request.Timeout = (int)this.Timeout.TotalMilliseconds;
            if (null != this.Credentials)
            {
                request.Credentials = this.Credentials;
            }
            else
            {
                request.UseDefaultCredentials = true;
            }
            if (null == headers) headers = new NameValueCollection();
            if (null != headers)
            {
                foreach (var headerName in headers.AllKeys)
                {
                    request.Headers.Add(headerName, headers[headerName]);
                }
            }
            return request;
        }

        private static void WritePayload(WebRequest request, XElement payloadElement)
        {
            var bytes = Encoding.UTF8.GetBytes(null == payloadElement ? "" : payloadElement.ToString());
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private static HttpResponse CreateResponseFrom(string requestUri, WebResponse response)
        {
            try
            {
                var httpWebResponse = (HttpWebResponse)response;
                return new HttpResponse(requestUri, httpWebResponse.StatusCode, httpWebResponse.Headers, ReadBodyFrom(httpWebResponse));
            }
            finally
            {
                response.Close();
            }
        }

        private static string ReadBodyFrom(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

    }
}
