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
}