using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace LMS.Client
{
    public class LmsClient
    {
        public static string GetAddress(string addressId)
        {
            var client = new LMSClient(new WSHttpBinding(SecurityMode.Transport) , new EndpointAddress(Lms.Uri));

            client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(@"c:\users\sbennett1\desktop\cms.pfx", "password");

            var response = client.Address(addressId);
            
            return response;
        }
    }
}