using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CMS
{
    public class CustomValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            var serialNumbers = File.ReadAllLines("certificates.txt");
            foreach (var serial in serialNumbers)
            {
                if (certificate.SerialNumber.ToUpper() == serial.ToUpper())
                {
                    return;
                }    
            }
            
            throw new ArgumentNullException("Invalid serial number");
        }

    }
}