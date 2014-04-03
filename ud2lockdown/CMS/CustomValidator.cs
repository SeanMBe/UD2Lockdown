using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace CMS
{
    public class CustomValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            var allowedCert = "be0276f2425048a946f0d56926269e5a";
            if (certificate.SerialNumber.ToUpper() == allowedCert.ToUpper())
            {
                return;
            }
            
            throw new ArgumentNullException("Invalid serial number");
        }

    }
}