using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace CMS
{
    public class CustomValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate.SerialNumber.ToUpper() == "612C0D847933D1B441FD77AC08E3F654")
            {
                return;
            }
            
            throw new ArgumentNullException("Invalid serial number");
        }

    }
}