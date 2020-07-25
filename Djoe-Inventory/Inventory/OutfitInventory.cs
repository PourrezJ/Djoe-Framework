
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;

namespace RDResurrection_Server.Inventory
{

    public class OutfitSlots
    {
        public Item Item;
    }

    [BsonIgnoreExtraElements]
    public class OutfitInventory
    {
        public static string[] OutfitClasses = { "glasses", "cap", "necklace", "mask", "earring", "jacket", "watch", "shirt", "bracelet", "pants", "gloves", "shoes", "kevlar", "backpack", "phone", "radio", "weapon", "weapon" };

        public ItemStack[] Slots = new ItemStack[18];

        //[BsonIgnore]
        //public Entities.Objects.WorldObject prop = null;

        public OutfitInventory()
        {

        }

        public bool Delete(ItemStack itemStack, int quantite)
        {
            if (Delete(GetSlotIndexUseStack(itemStack), quantite))
                return true;
            DestroyProp();
            return false;
        }


        public bool Delete(ItemID itemID, int quantite)
        {
            if (Slots.Any(x => x?.Item.Id == itemID))
            {
                ItemStack itemStack = Slots.FirstOrDefault(x => x?.Item.Id == itemID);

                if (itemStack == null)
                    return false;

                DestroyProp();
                if (Delete(GetSlotIndexUseStack(itemStack), quantite))
                    return true;
            }
            return false;
        }

        public bool Delete(int itemSlot, int quantite)
        {
            try
            {
                DestroyProp();
                var itemStack = Slots[itemSlot];
                if (itemStack.Quantity > quantite)
                    itemStack.Quantity -= quantite;
                else
                    Slots[itemSlot] = null;
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }


        public void DestroyProp()
        {
            /*
            if (prop != null)
            {
                prop.Destroy();
                prop = null;
            }*/
        }


        public int GetSlotIndexUseStack(ItemStack stack)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] == stack)
                    return i;
            }
            return -1;
        }

        public ItemStack HasItemEquip(ItemID item) =>
            Slots.FirstOrDefault((p) => (p?.Item.Id == item));

        public ItemStack[] FindAllItemWithType(ItemID itemID)
        {
            return Array.FindAll(Slots, x => x?.Item.Id == itemID);
        }
    }
}
