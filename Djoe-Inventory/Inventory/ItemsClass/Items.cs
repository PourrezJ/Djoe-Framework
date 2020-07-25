using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDResurrection_Server.Inventory.ItemsClass
{
    public static class Items
    {
        private static List<Item> items;
        public static List<Item> ItemsList
        {
            get
            {
                if (items == null)
                {
                    items = new List<Item>();
                    items.AddRange(LoadItemList());
                }
                return items;
            }
        }

        private static List<Item> LoadItemList()
        {
            return new List<Item>()
            {
                new Item(ItemID.Fromage, "Fromage", "", 0.1, true, false)
            };
        }
    }
}
