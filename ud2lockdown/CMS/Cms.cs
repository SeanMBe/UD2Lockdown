using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;

namespace CMS
{
    public class Cms : ICMS
    {
        public string Customer(string id)
        {
            var customers = new Dictionary<string, Tuple<string, string, string>> { { "2", new Tuple<string, string, string>("John", "7", "1234") } };
            
            return String.Format("CustomerId={0},CustomerName={1},AddressId={2},StreetNumber={3}", id, customers[id].Item1, customers[id].Item2, customers[id].Item3);
        }

        public static string Uri { get { return "https://cms/service.svc/cms"; } }
    }
}