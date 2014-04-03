using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security.Tokens;
using CMS.Client;
using NUnit.Framework;
using TestStack.White.Configuration;
using TestStack.White.Utility;

namespace UD2.AcceptanceTests
{
    [TestFixture]
    public class Ud2Tests
    {
        private Action _closeCmsService;

        [SetUp]
        public void SetUp()
        {
            KillUD();
            KillIISEXPRESS();
            this._closeCmsService = this.StartCmsWebService();
            CoreAppXmlConfiguration.Instance.BusyTimeout = 5000;
            CoreAppXmlConfiguration.Instance.UIAutomationZeroWindowBugTimeout = 5000;
        }


        private void Execute(string process, string args)
        {
            var iis = Process.Start(process, args);
            
            iis.WaitForExit();
            Debug.WriteLineIf(iis.ExitCode != 0, string.Format("Exit Code for {0} {1} is = {2}", process, args, iis.ExitCode));
        }

        private static Ud2Screen OpenUd2()
        {
            return new Ud2Screen();
        }

        [TearDown]
        public void TearDown()
        {
            _closeCmsService();
            KillUD();
        }

        [Test]
        public void When_I_open_app()
        {
            var app = OpenUd2();

            Assert.That(app.CustomerSearch.Enabled, Is.True);
            Assert.That(app.CustomerId.Enabled, Is.True);
            Assert.That(app.CustomerResult.Enabled, Is.True);
            Assert.That(app.CustomerId.Text, Is.Empty);
            Assert.That(app.CustomerResult.Text, Is.Empty);
        }
        
        [Test]
        public void When_secured_ud_gets_customer()
        {
            var app = OpenUd2();
            app.CustomerId.SetValue("2");
            app.CustomerSearch.Click();

            Retry.For(() => app.CustomerResult.Text, result => result == "", TimeSpan.FromSeconds(3));

            Assert.That(app.CustomerResult.Text, Is.StringStarting("CustomerId=2,CustomerName=John"));
            Assert.That(app.CustomerResult.Text, Is.StringEnding("AddressId=7,StreetNumber=1234"));
        }

        [Test]
        public void When_I_call_cms_with_certificate()
        {
            var actual = CmsClient.GetCustomer("2", Certificates.Cms);
            Assert.That(actual, Is.EqualTo("CustomerId=2,CustomerName=John,AddressId=7,StreetNumber=1234"));
        }
        
        [Test]
        public void When_I_call_cms_without_certificate()
        {
            Exception actual = null;
            try
            {
                CmsClient.GetCustomer("2");
            }
            catch (Exception ex)
            {
                actual = ex;
            }         

            Assert.That(actual, Is.TypeOf<InvalidOperationException>());
            Assert.That(actual.Message, Is.StringContaining("The client certificate is not provided."));
        }

        private static void KillUD()
        {
            KillProcess("UD2");
        }

        private void KillIISEXPRESS()
        {
            KillProcess("iisexpress");
        }
        
        private static void KillProcess(string ud2)
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName == ud2);
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }
        }

        private Action StartCmsWebService()
        {
            var apphost = Path.Combine(Directory.GetCurrentDirectory(), "applicationhost.config");
            this.SetupVirtualDirectoryPath("CMS", apphost);
            var closeIIS = StartIIS("CMS", apphost);
            return closeIIS;
        }

        private void SetupVirtualDirectoryPath(string app, string apphost)
        {
            var websiteRel = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\CMS\bin\debug");
            var website = Path.GetFullPath(websiteRel);
            var args = string.Format(@"set vdir ""{0}/"" -physicalPath:""{1}"" /apphostconfig:{2}", app, website, apphost);
            var appcmd = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\tools\IIS Express\appcmd.exe"));  
            var process = Process.Start(appcmd, args);
            process.WaitForExit();
        }

        private static Action StartIIS(string sitename, string apphost)
        {
            var args = string.Format(@"/config:{0} /site:{1} /trace:i", apphost, sitename);
            var iis = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\tools\IIS Express\iisexpress.exe"));  
            var process = Process.Start(iis, args);

            return process.Kill;
        }
    }
}
