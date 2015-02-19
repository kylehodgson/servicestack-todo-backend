using System.Collections.Generic;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace ToDoBackend
{
    [Route("/items", "POST")]
    public class NewItemRequest : IReturn<Item>
    {
        public string title { get; set; }
        public int order { get; set; }
    }

    [Route("/items/{itemid}", "PATCH")]
    public class PatchItemRequest : IReturn<Item>
    {
        public string itemid { get; set; }
        public string title { get; set; }
        public bool? completed { get; set; }
        public int? order { get; set; }
    }

    [Route("/items", "GET")]
    public class ViewListRequest : IReturn<List<Item>>
    { }

    [Route("/items/{itemid}", "GET")]
    public class ViewItemRequest : IReturn<Item>
    {
        public string itemid { get; set; }
    }

    [Route("/items", "DELETE")]
    public class DeleteAllItemsRequest : IReturnVoid
    { }

    [Route("/items/{itemid}", "DELETE")]
    public class DeleteItemRequest : IReturnVoid
    {
        public string itemid { get; set; }
    }

    public class ToDoService : Service
    {
        public List<Item> Get(ViewListRequest request)
        {
            return Db.Select<Item>();
        }

        public Item Get(ViewItemRequest request)
        {
            return Db.SingleById<Item>(request.itemid);
        }

        public Item Post(NewItemRequest request)
        {
            var item = request.ToItem();
            Db.Save(item);
            return item;
        }

        public Item Patch(PatchItemRequest request)
        {
            var item = Db.SingleById<Item>(request.itemid)
                         .PopulateWithNonDefaultValues(request);
            Db.Save(item);
            return item;
        }

        public void Delete(DeleteAllItemsRequest request)
        {
            Db.DeleteAll<Item>();
        }

        public void Delete(DeleteItemRequest request)
        {
            Db.DeleteById<Item>(request.itemid);
        }
    }
}