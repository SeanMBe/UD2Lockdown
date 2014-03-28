using System.ServiceModel;

namespace CMS.Client
{
    public class CmsClient
    {
        public static string GetCustomer(string customerId)
        {
            var client = new CMSClient(new WSHttpBinding(SecurityMode.None), new EndpointAddress(Cms.Uri));

            var response = client.Customer(customerId);

            return response;
        }
    }
}