using Newtonsoft.Json;
using Shared;
using Shared.ItemsClass;
using System;

namespace Server
{
    public class RPGInventoryItem
    {
        [JsonIgnore]
        public ItemStack Stack { get; private set; }

        [JsonProperty("id")]
        public ItemID Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("class")]
        public string Classes { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("inventorySlot")]
        public int InventorySlot { get; set; }

        [JsonProperty("inventoryType")]
        public string InventoryType { get; set; }

        [JsonProperty("usable")]
        public bool Usable { get; set; }

        [JsonProperty("givable")]
        public bool Givable { get; set; }

        [JsonProperty("dropable")]
        public bool Dropable { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("stackable")]
        public bool Stackable { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("outfitPosition")]
        public int OutfitPosition { get; set; }

        [JsonProperty("equitable")]
        public bool Equipable { get; set; }

        public RPGInventoryItem(ItemStack stack, string inventoryType, int slotIndex)
        {
            Stack = stack;
            InventoryType = inventoryType;
            InventorySlot = slotIndex;
            
            Id = stack.Item.Id;
            Name = stack.Item.Name;
            Icon = stack.Item.Icon;
            Quantity = stack.Quantity;
            Usable = stack.Item.IsUsable;
            Givable = stack.Item.IsGiven;
            Dropable = stack.Item.IsDropable;
            Weight = stack.Item.Weight;
            Stackable = stack.Item.IsStackable;
            Price = stack.Price;
            Equipable = true;

            if (stack.Item is ClothesItem)
            {
                var item = (stack.Item as ClothesItem);
                Classes = item.Classes;
                OutfitPosition = GetOutfitPosition(this.Classes);
            }
        }

        private int GetOutfitPosition(string classes)
        {
            for (int i = 0; i < OutfitInventory.OutfitClasses.Length; i++)
            {
                if (OutfitInventory.OutfitClasses[i] == classes)
                    return (i + 1);
            }
            return -1;
        }
    }
}
