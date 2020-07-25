using Newtonsoft.Json;

namespace RDResurrection_Server.Inventory
{
    public class RPGInventoryItem
    {
        [JsonIgnore]
        public ItemStack stack { get; private set; }

        public ItemID id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        [JsonProperty("class")]
        public string classes { get; set; }
        public int quantity { get; set; }
        public int inventorySlot { get; set; }
        public string inventoryType { get; set; }
        public bool usable { get; set; }
        public bool givable { get; set; }
        public bool dropable { get; set; }
        public double weight { get; set; }
        public bool stackable { get; set; }
        public double price { get; set; }
        public int outfitPosition { get; set; }
        public bool equipable { get; set; }

        public RPGInventoryItem(ItemStack stack, string inventoryType, int slotIndex)
        {
            this.stack = stack;
            this.inventoryType = inventoryType;
            this.inventorySlot = slotIndex;
            this.id = stack.Item.Id;
            this.name = stack.Item.Name;
            this.icon = stack.Item.Icon;
            this.classes = stack.Item.Classes;
            this.quantity = stack.Quantity;
            this.usable = stack.Item.IsUsable;
            this.givable = stack.Item.IsGiven;
            this.dropable = stack.Item.IsDropable;
            this.weight = stack.Item.Weight;
            this.stackable = stack.Item.IsStackable;
            this.price = stack.Price;
            this.equipable = true;

            this.outfitPosition = GetOutfitPosition(this.classes);
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
