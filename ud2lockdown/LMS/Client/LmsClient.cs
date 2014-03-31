using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace LMS.Client
{
    public class LmsClient
    {
        public static string GetAddress(string addressId)
        {
            var wsHttpBinding = new WSHttpBinding();
            wsHttpBinding.Security.Mode = SecurityMode.Transport;
            wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, "CN=LMS", false)[0];
            store.Close();

            var identity = EndpointIdentity.CreateX509CertificateIdentity(cert);
            
            var endpointAddress = new EndpointAddress(new Uri(Lms.Uri)); //, identity);
            var client = new LMSClient(wsHttpBinding , endpointAddress);

            client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(@"c:\users\sbennett1\desktop\cms.pfx", "password");
           
            var response = client.Address(addressId);
            
            return response;
        }
    }
}