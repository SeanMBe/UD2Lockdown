using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace UD2.Services
{
    public class Cms : ICMS
    {
        public string Customer(string id)
        {
            Debugger.Launch();
            var customers = new Dictionary<string, Tuple<string,string>> { { "2", new Tuple<string, string>("John", "7") } };
            var address = "";
            using (var cf = new ChannelFactory<ILMS>(new WebHttpBinding(), "http://localhost:8342"))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();
                var addressId = customers[id].Item2;
 
                address = channel.Address(addressId);
            }

            return string.Format("CustomerId={0},CustomerName={1},{2}", id, customers[id].Item1, address);
        }
    }
}