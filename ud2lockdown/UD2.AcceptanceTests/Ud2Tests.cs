using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using TestStack.White.Configuration;
using TestStack.White.Utility;

namespace UD2.AcceptanceTests
{
    [TestFixture]
    public class Ud2Tests
    {
        private Ud2Screen _app;

        [SetUp]
        public void SetUp()
        {
            KillUD();

            CoreAppXmlConfiguration.Instance.BusyTimeout = 5000;
            CoreAppXmlConfiguration.Instance.UIAutomationZeroWindowBugTimeout = 5000;

            _app = new Ud2Screen();
        }

        [TearDown]
        public void TearDown()
        {
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

            Assert.That(_app.CustomerResult.Text, Is.StringStarting("CustomerId=2"));
            Assert.That(_app.CustomerResult.Text, Is.StringEnding("AddressId=7"));
        }

        private static void KillUD()
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName == "UD2");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
    }
}
