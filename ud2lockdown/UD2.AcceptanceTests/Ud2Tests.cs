using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.ServiceModel;
using CMS.Client;
using LMS.Client;
using NUnit.Framework;
using TestStack.White.Configuration;
using TestStack.White.Utility;

namespace UD2.AcceptanceTests
{
    [TestFixture]
    public class Ud2Tests
    {
        private Action _closeCmsService;
        private Action _closeLmsService;

        [SetUp]
        public void SetUp()
        {
            KillUD();
            KillIISEXPRESS();
            this._closeCmsService = this.StartCmsWebService();
            this._closeLmsService = this.StartLmsWebService();
            CoreAppXmlConfiguration.Instance.BusyTimeout = 5000;
            CoreAppXmlConfiguration.Instance.UIAutomationZeroWindowBugTimeout = 5000;
        }

        private static Ud2Screen OpenUd2()
        {
            return new Ud2Screen();
        }

        [TearDown]
        public void TearDown()
        {
            _closeCmsService();
            _closeLmsService();
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
        public void When_I_get_customer_with_id_2()
        {
            var app = OpenUd2();
            app.CustomerId.SetValue("2");
            app.CustomerSearch.Click();

            Retry.For(() => app.CustomerResult.Text, result => result == "", TimeSpan.FromSeconds(3));

            Assert.That(app.CustomerResult.Text, Is.StringStarting("CustomerId=2,CustomerName=John"));
            Assert.That(app.CustomerResult.Text, Is.StringEnding("AddressId=7,StreetNumber=1234"));
        }

        [Test]
        public void When_I_call_lms()
        {
            var actual = LmsClient.GetAddress("7");
            Assert.That(actual, Is.EqualTo("AddressId=7,StreetNumber=1234"));
        }
        
        [Test]
        public void When_I_call_cms()
        {
            var actual = CmsClient.GetCustomer("2");
            Assert.That(actual, Is.EqualTo("CustomerId=2,CustomerName=John,AddressId=7,StreetNumber=1234"));
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
            var servicePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\CMS\bin\Debug"));  
            var port = "8341";
            var closeIIS = StartIIS("CMS", servicePath, port);
            return closeIIS;
        }
        
        private Action StartLmsWebService()
        {
            var servicePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\LMS\bin\Debug"));  
            var port = "7979";
            var closeIIS = StartIIS("LMS", servicePath, port);
            return closeIIS;
        }

        private static Action StartIIS(string sitename, string servicePath, string port)
        {
            var args = string.Format(@"/config:C:\Users\sbennett1\Documents\iisexpress\config\applicationhost.config /site:{0}", sitename);
            var iisPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\tools\IIS Express\iisexpress.exe"));  
            var iis = Process.Start(iisPath, args);

            return iis.Kill;
        }
    }
}
