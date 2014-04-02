using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace CMS.Client
{
    public class CmsClient
    {
        public static string GetCustomer(string customerId, X509Certificate2 certificate)
        {
            var wsHttpBinding = new WSHttpBinding();
            
            wsHttpBinding.Security.Mode = SecurityMode.Transport;
            wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            
            var client = new CMSClient(wsHttpBinding, new EndpointAddress(Cms.Uri));

            
            client.ClientCredentials.ClientCertificate.Certificate = certificate;
            
            var response = client.Customer(customerId);

            return response;
        }

        public static string GetCustomer(string customerId)
        {
            var wsHttpBinding = new WSHttpBinding();

            wsHttpBinding.Security.Mode = SecurityMode.Transport;
            wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            var client = new CMSClient(wsHttpBinding, new EndpointAddress(Cms.Uri));
            
            var response = client.Customer(customerId);

            return response;            
        }
    }
}