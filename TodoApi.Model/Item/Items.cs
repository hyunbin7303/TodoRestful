using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Model.Item
{
    class Items
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public ItemType ItemType { get; set; }
    }
    public enum ItemType
    {
        Food,
        Car,
        House,
        Entertainment,
        Electronic,
        Others
    }
}
