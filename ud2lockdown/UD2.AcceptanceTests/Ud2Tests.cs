using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using NUnit.Framework;
using TestStack.White.Configuration;
using TestStack.White.Utility;
using UD2.Services;

namespace UD2.AcceptanceTests
{
    [TestFixture]
    public class Ud2Tests
    {
        private Ud2Screen _app;

        private WebServiceHost _oms;

        private WebServiceHost _lms;

        [SetUp]
        public void SetUp()
        {
            KillUD();
            _oms = this.StartCmsWebService();
            _lms = this.StartLmsWebService();
            _oms.Open();
            _lms.Open();
            CoreAppXmlConfiguration.Instance.BusyTimeout = 5000;
            CoreAppXmlConfiguration.Instance.UIAutomationZeroWindowBugTimeout = 5000;

            _app = new Ud2Screen();
        }

        [TearDown]
        public void TearDown()
        {
            _oms.Close();
            _lms.Close();
            KillUD();
        }

        [Test]
        public void When_I_open_app()
        {
            Assert.That(_app.CustomerSearch.Enabled, Is.True);
            Assert.That(_app.CustomerId.Enabled, Is.True);
            Assert.That(_app.CustomerResult.Enabled, Is.True);
            Assert.That(_app.CustomerId.Text, Is.Empty);
            Assert.That(_app.CustomerResult.Text, Is.Empty);
        }
        
        [Test]
        public void When_I_get_customer_with_id_2()
        {
            _app.CustomerId.SetValue("2");
            _app.CustomerSearch.Click();

            Retry.For(() => _app.CustomerResult.Text, result => result == "", TimeSpan.FromSeconds(3));

            Assert.That(_app.CustomerResult.Text, Is.StringStarting("CustomerId=2,CustomerName=John"));
            Assert.That(_app.CustomerResult.Text, Is.StringEnding("AddressId=7,StreetNumber=1234"));
        }

        private static void KillUD()
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName == "UD2");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        private WebServiceHost StartCmsWebService()
        {
            var host = new WebServiceHost(typeof(Cms), new Uri("http://localhost:8341"));
            var ep = host.AddServiceEndpoint(typeof(ICMS), new WebHttpBinding(), "");
            return host;
        }
        
        private WebServiceHost StartLmsWebService()
        {
            var host = new WebServiceHost(typeof(Lms), new Uri("http://localhost:8342"));
            var ep = host.AddServiceEndpoint(typeof(ILMS), new WebHttpBinding(), "");
            return host;
        }
    }
}
