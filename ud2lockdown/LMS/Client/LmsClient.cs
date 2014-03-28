using System.ServiceModel;

namespace LMS.Client
{
    public class LmsClient
    {
        public static string GetAddress(string addressId)
        {
            var client = new LMSClient(new WSHttpBinding(SecurityMode.Transport) , new EndpointAddress(Lms.Uri));

            var response = client.Address(addressId);
            
            return response;
        }
    }
}