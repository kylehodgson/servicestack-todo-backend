using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
namespace ToDoBackend
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base(
            "ToDoBackend", typeof (ToDoService).Assembly)
        { }

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

        }
    }
}