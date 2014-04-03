using System.Security.Cryptography.X509Certificates;

namespace UD2
{
    public class Certificates
    {
        public static X509Certificate2 Cms
        {
            get
            {
                return new X509Certificate2(@"cmsClient.pfx", "password");
            }
        }
    }
}