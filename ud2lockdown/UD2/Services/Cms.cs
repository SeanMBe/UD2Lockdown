using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace UD2.Services
{
    public class Cms : ICMS
    {
        public string Customer(string id)
        {
            var customers = new Dictionary<string, Tuple<string, string>> { { "2", new Tuple<string, string>("John", "7") } };
            var address = "";
            var addressId = customers[id].Item2;
            address = Lms.GetAddress(addressId);

            return String.Format("CustomerId={0},CustomerName={1},{2}", id, customers[id].Item1, address);
        }

        public static string Uri {  get { return "http://localhost:8341"; } }

        public static string GetCustomer(string customerId)
        {
            string customerResult;
            using (var cf = new ChannelFactory<ICMS>(new WebHttpBinding(), Uri))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();
                customerResult = channel.Customer(customerId);
            }
            return customerResult;
        }
    }
}