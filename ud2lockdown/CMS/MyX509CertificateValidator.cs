using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CMS
{
    public class MyX509CertificateValidator : X509CertificateValidator
    {
        string allowedIssuerName;

        public MyX509CertificateValidator(string allowedIssuerName)
        {
            Debugger.Launch();
            if (allowedIssuerName == null)
            {
                throw new ArgumentNullException("allowedIssuerName");
            }

            this.allowedIssuerName = allowedIssuerName;
        }

        public override void Validate(X509Certificate2 certificate)
        {
            Debugger.Launch();
            if (!certificate.GetSerialNumber().SequenceEqual(new byte[] { 1, 2, 3 }))
            {
                throw new SecurityTokenValidationException("Serial number not in Allowed List");
            }

            // Check that there is a certificate.
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            // Check that the certificate issuer matches the configured issuer.
            if (this.allowedIssuerName != certificate.IssuerName.Name)
            {
                throw new SecurityTokenValidationException
                    ("Certificate was not issued by a trusted issuer");
            }
        }
    }
}