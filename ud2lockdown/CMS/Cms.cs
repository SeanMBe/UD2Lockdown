using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using LMS;
using LMS.Client;

namespace CMS
{
    public class Cms : ICMS
    {
        public string Customer(string id)
        {
            //var customers = new Dictionary<string, Tuple<string, string>> { { "2", new Tuple<string, string>("John", "7") } };
            //var addressId = customers[id].Item2;

            //var address = LmsClient.GetAddress(addressId);

            return "test";
            //String.Format("CustomerId={0},CustomerName={1},{2}", id, customers[id].Item1, address);
        }

        public static string Uri { get { return "https://cms/service.svc/cms"; } }
    }
}