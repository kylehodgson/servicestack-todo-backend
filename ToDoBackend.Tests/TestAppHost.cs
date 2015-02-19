using System;
using System.Collections.Generic;
using System.Reflection;
using Funq;
using ServiceStack;

namespace ToDoBackend.Tests
{
    class TestAppHost : AppSelfHostBase
    {
        public TestAppHost(String serviceName, Assembly assembly)
            : base(serviceName, assembly)
        { }

        public Action<Container> ConfigureContainer { get; set; }
        public Action<List<IPlugin>> AddPlugins { get; set; }
        public HostConfig HostConfig { get; set; }
        public string BaseUrl { get; set; }

        public override void Configure(Container container)
        {
            if (this.ConfigureContainer != null) this.ConfigureContainer(container);
            if (this.AddPlugins != null) this.AddPlugins(Plugins);
            if (this.BaseUrl != null) SetConfig(new HostConfig {WebHostUrl = BaseUrl});
        }
    }
}