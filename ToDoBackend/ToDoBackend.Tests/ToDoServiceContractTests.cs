using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace ToDoBackend.Tests
{
    [TestFixture]
    public class ToDoServiceContractTests
    {
        private ServiceStackHost _appHost;
        private const string BaseUrl = "http://localhost:8888/";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _appHost = new TestAppHost("ToDoService Test AppHost", typeof(ToDoService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    container.Register<IDbConnectionFactory>(
                        new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));
                    container.RegisterAutoWired<ToDoService>();
                }
            }
            .Init()
            .Start(BaseUrl);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _appHost.Dispose();
        }

        [SetUp]
        public void TestSetUp()
        {
            var dbFactory = _appHost.TryResolve<IDbConnectionFactory>();
            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<Item>();
            }
        }

        [Test,Ignore]
        public void ShouldCreateItemsWithWorkingUrls()
        {
            var client = new JsonServiceClient(BaseUrl);
            var testTitle = "test item";
            var postedItem = client.Post(new NewItemRequest {title = testTitle});

            var fetchedItem = (BaseUrl + postedItem.url)
                .GetJsonFromUrl()
                .FromJson<Item>();

            Assert.That(postedItem.title.Equals(testTitle));
            Assert.That(fetchedItem.title.Equals(testTitle));
            Assert.AreEqual(postedItem.title,fetchedItem.title);
        }
    }

    class TestAppHost : AppSelfHostBase
    {
        public TestAppHost(String serviceName, Assembly assembly)
            : base(serviceName, assembly)
        { }

        public Action<Container> ConfigureContainer { get; set; }

        public override void Configure(Container container)
        {
            if (this.ConfigureContainer == null)
                return;
            this.ConfigureContainer(container);
        }
    }
}
