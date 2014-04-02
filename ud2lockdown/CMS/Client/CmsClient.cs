using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace CMS.Client
{
    public class CmsClient
    {
        public static string GetCustomer(string customerId)
        {
                        
            
            var wsHttpBinding = new WSHttpBinding();
            
            wsHttpBinding.Security.Mode = SecurityMode.Transport;
            wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            
            var client = new CMSClient(wsHttpBinding, new EndpointAddress(Cms.Uri));
            
            //client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\cmsclient.pfx", "");
            
            client.ClientCredentials.ClientCertificate.SetCertificate(
                StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindByThumbprint,
                "80446469be8caf44b60f4714cc01bc9c38f271f7");
            
            var response = client.Customer(customerId);

            return response;
        }
    }
}