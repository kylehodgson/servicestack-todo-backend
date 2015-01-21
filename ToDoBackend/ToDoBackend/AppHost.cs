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
            Plugins.Add(new CorsFeature(allowedMethods: "GET, POST, PUT, DELETE, PATCH, OPTIONS"));
            this.PreRequestFilters.Add((httpReq, httpRes) =>
            {
                //Handles Request and closes Responses after emitting global HTTP Headers
                if (httpReq.Verb == "OPTIONS")
                    httpRes.EndRequest(); //add a 'using ServiceStack;'
            });
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