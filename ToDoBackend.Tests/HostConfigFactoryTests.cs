using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceStack;

namespace ToDoBackend.Tests
{
    [TestFixture]
    public class HostConfigFactoryTests
    {
        [SetUp]
        public void TestSetup()
        {
            Environment.SetEnvironmentVariable("todobackend_host", String.Empty);
            Environment.SetEnvironmentVariable("todobackend_port", String.Empty);
            Environment.SetEnvironmentVariable("todobackend_protocol", String.Empty);
        }

        [Test]
        public void HostConfigWebHostUrlShouldBeDefaultInCaseBindingEnvironmentSettingsNotSet()
        {
            var target = new HostConfigFactory();
            Assert.That(target.GetHostConfig().WebHostUrl.Equals(HostConfigFactory.DefaultBinding));
        }

        [Test]
        public void HostConfigUrlShouldReflectEnvironmentSettingsWhenSetProperly()
        {
            Environment.SetEnvironmentVariable("todobackend_host","localhost");
            Environment.SetEnvironmentVariable("todobackend_port","1337");
            Environment.SetEnvironmentVariable("todobackend_protocol","ftp");

            var target = new HostConfigFactory();
            var hostConfig = target.GetHostConfig();
            var url = hostConfig.WebHostUrl;
            Assert.That(url.Equals("ftp://localhost:1337"));
        }
    }
}
