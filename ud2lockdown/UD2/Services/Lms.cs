using System;
using System.Net;
using System.Xml.Linq;
using UD2.HTTP;

namespace UD2.Services
{
    public class Lms : ILMS
    {
        public string Address(string id)
        {
            return "AddressId=7,StreetNumber=1234";
        }

        public static string Uri { get { return "http://localhost:8345"; } }

        public static string GetAddress(string addressId)
        {
            var client = new HttpClient();

            var uri = string.Format("{0}/Address?id={1}", Uri, addressId);
            var response = client.Get(uri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Status code is " + response.StatusCode + " and response is" + response.Body);
            }

            var xml = XDocument.Parse(response.Body);

            return ((XElement)xml.FirstNode).Value;
        }
    }
}