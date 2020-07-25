using Newtonsoft.Json;
using Shared;

namespace Server
{
    public class RPGInventoryItem
    {
        [JsonIgnore]
        public ItemStack Stack { get; private set; }

        public ItemID Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        [JsonProperty("class")]
        public string Classes { get; set; }
        public int Quantity { get; set; }
        public int InventorySlot { get; set; }
        public string InventoryType { get; set; }
        public bool Usable { get; set; }
        public bool Givable { get; set; }
        public bool Dropable { get; set; }
        public double Weight { get; set; }
        public bool Stackable { get; set; }
        public double Price { get; set; }
        public int OutfitPosition { get; set; }
        public bool Equipable { get; set; }

        public RPGInventoryItem(ItemStack stack, string inventoryType, int slotIndex)
        {
            Stack = stack;
            InventoryType = inventoryType;
            InventorySlot = slotIndex;
            Id = stack.Item.Id;
            Name = stack.Item.Name;
            Icon = stack.Item.Icon;
            Classes = stack.Item.Classes;
            Quantity = stack.Quantity;
            Usable = stack.Item.IsUsable;
            Givable = stack.Item.IsGiven;
            Dropable = stack.Item.IsDropable;
            Weight = stack.Item.Weight;
            Stackable = stack.Item.IsStackable;
            Price = stack.Price;
            Equipable = true;

            OutfitPosition = GetOutfitPosition(this.Classes);
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
