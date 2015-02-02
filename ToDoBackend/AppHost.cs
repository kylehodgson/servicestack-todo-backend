using System.Collections.Generic;
using System.Linq;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
namespace ToDoBackend
{
    public class AppHost : AppHostBase
    {
        private readonly IISBindingManager _bindingManager;

        public AppHost() : base(
            "ToDoBackend", typeof (ToDoService).Assembly)
        {
            _bindingManager = new IISBindingManager();
        }

        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature(
                            allowedOrigins: "*",
                            allowedMethods: "GET, POST, PUT, DELETE, PATCH, OPTIONS"));
            var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            container.Register<IDbConnectionFactory>(dbFactory);
            container.RegisterAutoWired<ToDoService>();
            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<Item>();
            }
            var bindings = _bindingManager.GetBindings().ToList();
            if (bindings.Count > 0) SetConfig(new HostConfig { WebHostUrl = bindings[0] });
        }
    }
}