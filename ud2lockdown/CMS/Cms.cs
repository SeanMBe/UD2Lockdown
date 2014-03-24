using System;
using System.Collections.Generic;
using System.ServiceModel;
using LMS;

namespace CMS
{
    public class Cms : ICMS
    {
        public string Customer(string id)
        {
            var customers = new Dictionary<string, Tuple<string, string>> { { "2", new Tuple<string, string>("John", "7") } };
            var address = "";
            var addressId = customers[id].Item2;

            var client = new LMSClient(new WSHttpBinding(), new EndpointAddress(Lms.Uri));

            address = client.Address(addressId);


            return String.Format("CustomerId={0},CustomerName={1},{2}", id, customers[id].Item1, address);
        }

        public static string Uri { get { return "http://localhost:8341/service.svc/cms"; } }
    }
}