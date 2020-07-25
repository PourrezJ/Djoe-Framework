using Shared.ItemsClass;
using System;
using System.Diagnostics;
using System.Linq;

namespace Shared
{
    public partial class Inventory
    {
        #region Variables
        public ItemStack[] InventoryList;
        public int MaxSize { get; set; }
        public int MaxSlot { get; set; }
        public bool Locked { get; set; }
        #endregion

        #region Constructor
        public Inventory(int maxSize, int maxSlot)
        {
            InventoryList = new ItemStack[maxSlot];
            MaxSize = maxSize;
            MaxSlot = maxSlot;
        }

        public ItemStack this[int x]
        {
            get { return InventoryList[x]; }
            set
            {
                if (value.Item != null)
                {
                    InventoryList[x] = value;
                }
            }
        }
        #endregion

        #region Method
        public ItemStack[] FindAllItemWithType(ItemID itemID)
        {
            return Array.FindAll(InventoryList, x => x?.Item.Id == itemID);
        }

        public double CurrentSize()
        {
            double currentSize = 0;

            foreach (ItemStack itemStack in InventoryList)
            {
                if (itemStack != null && itemStack.Item != null)
                    currentSize += itemStack.Item.Weight * itemStack.Quantity;
            }

            return currentSize;
        }

        public bool IsFull(double itemsize = 0)
        {
            if (CurrentSize() + itemsize <= MaxSize)
                return false;

            return true;
        }

        public bool IsEmpty()
        {
            foreach (ItemStack itemStack in InventoryList)
            {
                if (itemStack != null)
                    return false;
            }

            return true;
        }

        public bool AddItem(Item item, int quantity = 1, bool message = true)
        {
            if (AddItem(item, quantity))
            {
                /* TODO Add message system and save 
                if (message)
                    client.TriggerEvent("Display_Help", "Vous venez d'ajouter " + quantity + " " + item.Name + " dans l'inventaire", 10000);

                client.GetPlayerHandler()?.UpdateFull();*/
                return true;
            }

            return false;
        }

        public bool AddItem(Item item, int quantity)
        {
            if (item.IsStackable)
                return AddItem(item, quantity, out _);
            else {
                for(int i = 0; i < quantity; i++) {
                    if (!AddItem(item, 1, out _))
                        return false;
                }
                return true;
            }
        }

        public bool AddItem(Item item, int quantity, out int slot)
        {
            slot = -1;

            if (CurrentSize() + (item.Weight * quantity) > MaxSize)
                return false;

            if (InventoryList.Any(x => x?.Item.Id == item.Id) && item.IsStackable)
            {
                ItemStack itemStack = InventoryList.First(x => x?.Item.Id == item.Id);
                itemStack.Quantity += quantity;
            }
            else
            {
                slot = GetEmptySlot();

                if (slot == -1)
                    return false;

                InventoryList[slot] = new ItemStack(item, quantity, slot);
            }

            return true;
        }

        public int GetEmptySlot()
        {
            for (int i = 0; i < InventoryList.Length; i++)
            {
                if (InventoryList[i] == null) return i;
            }
            return -1;
        }

        public void Clear() => new Inventory(MaxSize, MaxSlot);

        public void Clear(int newsize, int maxSlot) => new Inventory(newsize, maxSlot);

        public bool Delete(ItemStack itemStack, int quantity = 1)
        {
            try
            {
                if (itemStack.Quantity < quantity)
                    return false;
                else if (itemStack.Quantity > quantity)
                    itemStack.Quantity -= quantity;
                else
                    InventoryList[GetSlotIndexUseStack(itemStack)] = null;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool Delete(int slot, int quantity = 1)
        {
            try
            {
                var itemStack = InventoryList[slot];

                if (itemStack.Quantity > quantity)
                    itemStack.Quantity -= quantity;
                else
                {
                    itemStack.Quantity = 0;
                    InventoryList[slot] = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public int DeleteAll(ItemID itemID, int quantityNeeded)
        {
            var value = quantityNeeded;

            for (int i = 0; i < InventoryList.Length; i++)
            {
                if (InventoryList[i] != null)
                {
                    if (InventoryList[i].Item == null)
                        continue;

                    if (InventoryList[i].Item.Id == itemID)
                    {
                        if (InventoryList[i].Quantity > value)
                        {
                            InventoryList[i].Quantity -= value;
                            return quantityNeeded;
                        }
                        else
                        {
                            value -= InventoryList[i].Quantity; //5 - 3
                            InventoryList[i] = null;
                        }
                    }
                }
            }

            return quantityNeeded - value; // valeur supprimer
        }
        /*
        public static Item ItemByID(ItemID id)
        {
            var item = Items.ItemsList.Find(i => i.Id == id) ?? null;

            if (item == null)
                return null;

            return item.CloneItem();
        }
        */
        public Item GetItem(ItemID itemID)
        {
            ItemStack item = InventoryList.ToList().Find(p => p?.Item.Id == itemID);
            return item?.Item;
        }

        public bool HasItemID(ItemID id)
        {
            if (InventoryList.Any(x => x?.Item.Id == id))
                return true;

            return false;
        }

        public bool HasItem(Item item)
        {
            if (InventoryList.Any(x => x?.Item.Id == item?.Id))
                return true;

            return false;
        }

        public int CountItem(Item item)
        {
            int count = 0;

            foreach (ItemStack invItem in InventoryList)
            {
                if (invItem != null && invItem.Item.Id == item.Id)
                    count += invItem.Quantity;
            }

            return count;
        }

        public int CountItem(ItemID itemID)
        {
            int count = 0;

            foreach (ItemStack invItem in InventoryList)
            {
                if (invItem != null && invItem.Item.Id == itemID)
                    count += invItem.Quantity;
            }

            return count;
        }

        public int GetSlotIndexUseStack(ItemStack stack)
        {
            for (int i = 0; i < InventoryList.Length; i++)
            {
                if (InventoryList[i] == stack)
                    return i;
            }

            return -1;
        }
        #endregion
    }
}