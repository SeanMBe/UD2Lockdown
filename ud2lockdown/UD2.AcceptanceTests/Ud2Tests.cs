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
        private WebServiceHost _oms;
        private WebServiceHost _lms;

        [SetUp]
        public void SetUp()
        {
            KillUD();
            _oms = this.StartCmsWebService();
            _lms = this.StartLmsWebService();
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
            _oms.Close();
            _lms.Close();
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
            var actual = Lms.GetAddress("7");
            Assert.That(actual, Is.EqualTo("AddressId=7,StreetNumber=1234"));
        }
        
        [Test]
        public void When_I_call_cms()
        {
            var actual = Cms.GetCustomer("2");
            Assert.That(actual, Is.EqualTo("CustomerId=2,CustomerName=John,AddressId=7,StreetNumber=1234"));
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
            var host = new WebServiceHost(typeof(Cms), new Uri(Cms.Uri));
            var ep = host.AddServiceEndpoint(typeof(ICMS), new WebHttpBinding(), "");
            host.Open();
            return host;
        }
        
        private WebServiceHost StartLmsWebService()
        {
            var host = new WebServiceHost(typeof(Lms), new Uri(Lms.Uri));
            var ep = host.AddServiceEndpoint(typeof(ILMS), new WebHttpBinding(), "");
            host.Open();
            return host;
        }
    }
}
