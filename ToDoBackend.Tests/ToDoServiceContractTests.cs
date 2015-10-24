using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ToDoBackend.Tests
{
    [TestFixture]
    public class ToDoServiceContractTests
    {
        private ServiceStackHost _appHost;
        private string TestUrl;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var hostConfig = new HostConfigFactory().GetHostConfig();
            this.TestUrl = hostConfig.WebHostUrl;
            if (!this.TestUrl.EndsWith("/")) this.TestUrl += "/";

            _appHost = new TestAppHost("ToDoService Test AppHost", typeof(ToDoService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    container.Register<IDbConnectionFactory>(
                        new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));
                    container.RegisterAutoWired<ToDoService>();
                },
                AddPlugins = plugins =>
                {
                    plugins.Add(new CorsFeature(allowedOrigins: "*",
                                                allowedMethods:
                                                    "GET, POST, PUT, DELETE, PATCH, OPTIONS"));
                },
                BaseUrl = TestUrl
            }
            .Init()
            .Start(this.TestUrl);
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

        [Test]
        public void ShouldCreateItemsWithWorkingUrls()
        {
            var client = new JsonServiceClient(TestUrl);
            var testTitle = "test item";
            var postedItem = client.Post(new NewItemRequest {title = testTitle});

            var fetchedItem = postedItem.url
                .GetJsonFromUrl()
                .FromJson<Item>();

            Assert.That(postedItem.title.Equals(testTitle));
            Assert.That(fetchedItem.title.Equals(testTitle));
            Assert.AreEqual(postedItem.title,fetchedItem.title);
        }

        [Test]
        public void ShouldAffirmPreFlightOptions()
        {
            var client = new JsonServiceClient(TestUrl)
            {
                RequestFilter = request =>
                {
                    request.Headers.Add("Origin", "http://www.example.com");
                    request.Headers.Add("Access-Control-Request-Method", "POST");
                    request.Headers.Add("Access-Control-Request-Headers",
                                        "X-Requested-With");
                }
            };
            var optionsResult = client.CustomMethod("OPTIONS", new PreFlight());
            Assert.That(optionsResult.StatusCode.Equals(HttpStatusCode.OK));
        }
    }

    class PreFlight {}

    class TestAppHost : AppSelfHostBase
    {
        public TestAppHost(String serviceName, Assembly assembly)
            : base(serviceName, assembly)
        { }

        public Action<Container> ConfigureContainer { get; set; }
        public Action<List<IPlugin>> AddPlugins { get; set; }
        public string BaseUrl { get; set; }

        public override void Configure(Container container)
        {
            if (this.ConfigureContainer != null) this.ConfigureContainer(container);
            if (this.AddPlugins != null) this.AddPlugins(Plugins);
            if (BaseUrl!=null) SetConfig(new HostConfig{WebHostUrl = BaseUrl}); 

        }

  
    }
}
