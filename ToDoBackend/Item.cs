using System;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace ToDoBackend
{
    public class Item
    {
        [PrimaryKey]
        public string itemid { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
        public string url { get; set; }
        public int order { get; set; }
    }

    public static class NewItemRequestExtensions
    {
        public static Item ToItem(this NewItemRequest request)
        {
            var guid = Guid.NewGuid().ToString();
            var item = new Item
            {
                itemid = guid,
                completed = false,
                url = "/items/{0}".Fmt(guid)
            }.PopulateWith(request);
            return item;
        }
    }
}