using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;
using CitizenFX.Core;
using Server.Utils.Extensions;
using Shared;
using Server.Utils;
using Server.Entities;
using Server.ItemsClass;

namespace Server
{
    public static class RPGInventoryManager
    {
        #region Private static properties
        private static ConcurrentDictionary<Player, RPGInventoryMenu> _clientMenus = new ConcurrentDictionary<Player, RPGInventoryMenu>();
        #endregion

        #region Init
        public static void Init()
        {
            GameMode.RegisterEventHandler("RPGInventory_UseItem", new Action<Player, int, string, int>(RPGInventory_UseItem));
            GameMode.RegisterEventHandler("RPGInventory_DropItem", new Action<Player, string, int, int, int>(RPGInventory_DropItem));
            GameMode.RegisterEventHandler("RPGInventory_GiveItem", new Action<Player, string, int, int, int>(RPGInventory_GiveItem));
            GameMode.RegisterEventHandler("RPGInventory_ClosedMenu_SRV", new Action<Player>(RPGInventory_ClosedMenu_SRV));
            GameMode.RegisterEventHandler("RPGInventory_PriceItemInventory_SRV", new Action<Player, string, int, int, int>(RPGInventory_PriceItemInventory_SRV));

            GameMode.RegisterEventHandler("RPGInventory_SplitItemInventory_SRV", new Action<Player, string, int, int, int, int, int, int>(RPGInventory_SplitItemInventory_SRV));
            GameMode.RegisterEventHandler("RPGInventory_SwitchItemInventory_SRV", new Action<Player, string, string, int, int, int>(RPGInventory_SwitchItemInventory_SRV));
        }

        #endregion

        #region Server Events
        public static void OnPlayerQuit(Player sender)
        {
            if (HasInventoryOpen(sender))
                CloseMenu(sender);
        }
        #endregion

        #region Public static methods
        public static void CloseMenu(Player client, RPGInventoryMenu oldmenu = null)
        {
            RPGInventoryMenu menu = null;
            if (oldmenu != null || _clientMenus.TryRemove(client, out menu))
            {
                if (menu != null)
                    menu.OnClose?.Invoke(client, menu);

                client.TriggerEvent("InventoryManager_CloseMenu");
            }
        }

        public static bool HasInventoryOpen(Inventory inventory) => _clientMenus.Values.Any(i => i.Inventory == inventory || i.Bag == inventory || i.Distant == inventory);

        public static bool HasInventoryOpen(Player client) => _clientMenus.ContainsKey(client);

        public static RPGInventoryMenu GetRPGInventory(Player client)
        {
            if (_clientMenus.TryGetValue(client, out RPGInventoryMenu menu))
                return menu;
            return null;
        }

        public static bool OpenMenu(Player client, RPGInventoryMenu menu)
        {
            _clientMenus.TryRemove(client, out _);

            if (_clientMenus.TryAdd(client, menu))
            {
                //Logger.Debug(JsonConvert.SerializeObject(menu.PocketsItems));

                client.TriggerEvent("InventoryManager_OpenMenu",
                    JsonConvert.SerializeObject(menu.PocketsItems),
                    JsonConvert.SerializeObject(menu.BagItems),
                    JsonConvert.SerializeObject(menu.DistantItems),
                    JsonConvert.SerializeObject(menu.OutfitItems),
                    (menu.DistantPlayer == null) ? false : true);
            }

            return false;
        }
        #endregion

        #region Add Physical
        public static void RPGInventory_SetPlayerProps(Player client, Item item)
        {
            PlayerData player = client.GetPlayerDatabase();
            _clientMenus.TryGetValue(client, out RPGInventoryMenu menu);
        
        }
        #endregion

        #region Remove physical
        public static void RPGInventory_DeletePlayerProps(Player client, Item item)
        {
            PlayerData ph = client.GetPlayerDatabase();
            _clientMenus.TryGetValue(client, out RPGInventoryMenu menu);
            /*
            if(item is ClothItem cloth) {
                switch(item.id) {
                    case ItemID.Glasses: // glasses
                        ph.Clothing.Glasses = (ph.Character.Gender == 0) ? new PropData(14, 0) : new PropData(13, 0);
                        break;

                    case ItemID.Hats: // hair
                        ph.Clothing.Hats = (ph.Character.Gender == 0) ? new PropData(121, 0) : new PropData(120, 0);
                        break;

                    case ItemID.Necklace: // necklace
                        ph.Clothing.Accessory = new ClothData();
                        break;

                    case ItemID.Mask: // mask
                        ph.Clothing.Mask = new ClothData();
                        break;

                    case ItemID.Ears: // earring
                        ph.Clothing.Ears = (ph.Character.Gender == 0) ? new PropData(33, 0) : new PropData(12, 0);
                        break;

                    case ItemID.Jacket: // jacket
                        ph.Clothing.Torso = new ClothData(15, 0, 0);
                        ph.Clothing.Tops = new ClothData(15, 0, 0);
                        break;

                    case ItemID.Watch: // watch
                        ph.Clothing.Watches = (ph.Character.Gender == 0) ? new PropData(2, 0) : new PropData(1, 0);
                        break;

                    case ItemID.TShirt: // shirt
                        ph.Clothing.Undershirt = (ph.Character.Gender == 0) ? new ClothData(57, 0, 0) : new ClothData(34, 0, 0);
                        break;

                    case ItemID.Bracelet: // bracelet
                        ph.Clothing.Bracelets = new PropData(15, 0);
                        break;

                    case ItemID.Pant: // pants
                        ph.Clothing.Legs = (ph.Character.Gender == 0) ? new ClothData(14, 0, 0) : new ClothData(15, 0, 0);
                        break;

                    case ItemID.Glove: // gloves
                        ph.Clothing.Glasses = (ph.Character.Gender == 0) ? new PropData(6, 0) : new PropData(5, 0);
                        break;

                    case ItemID.Shoes: // shoes
                        ph.Clothing.Feet = (ph.Character.Gender == 0) ? new ClothData(34, 0, 0) : new ClothData(35, 0, 0);
                        break;

                    case ItemID.Kevlar: // kevlar
                        ph.Clothing.BodyArmor = new ClothData(0, 0, 0);
                        break;
                }
            } else if((item) is Items.Weapons weapons) {
                switch(item.id) {
                    case ItemID.Weapon:
                    case ItemID.LampeTorche:
                    case ItemID.Carabine:
                    case ItemID.Matraque:
                    case ItemID.Bat:
                    case ItemID.BattleAxe:
                    case ItemID.CombatPistol:
                    case ItemID.Flashlight:
                    case ItemID.HeavyPistol:
                    case ItemID.Knife:
                    case ItemID.Machete:
                    case ItemID.Musket:
                    case ItemID.SNSPistol:
                    case ItemID.Colt6Coup:
                    case ItemID.Colt1911:
                    case ItemID.Magnum357:
                    case ItemID.Pistol50:
                    case ItemID.Pistol:
                    case ItemID.Pump:
                    case ItemID.Taser:
                        client.RemoveAllWeapons();
                        break;
                }
            } else {
                switch(item.id) {
                    case ItemID.Phone:
                        if((item) is Items.PhoneItem phoneItem && phoneItem.PhoneHandler != null) {
                            ph.PhoneSelected = null;
                        }
                        break;
                    case ItemID.Bag: // backpack
                        ph.Clothing.Bags = new ClothData();
                        ph.BagInventory = null;
                        menu.Bag = null;
                        menu.BagItems = null;
                        //await menu.CloseMenu(sender);
                        break;

                    case ItemID.Hache:
                    case ItemID.Pioche:
                    case ItemID.Marteau:
                    case ItemID.MarteauPiqueur:
                    case ItemID.Pelle:
                    case ItemID.CrateTool:
                        menu.Outfit.DestroyProp();
                        client.StopAnimation();
                        break;
                }
            }
            ph.Clothing.UpdatePlayerClothing();*/
        }
        #endregion

        #region Use
        private static void RPGInventory_UseItem([FromSource]Player client, int itemID, string targetInventory, int itemSlot)
        {
            Debug.WriteLine("RPGInventory_UseItem");
            RPGInventoryMenu menu = null;

            lock (_clientMenus)
            {
                if (_clientMenus.TryGetValue(client, out menu))
                {
                    ItemStack itemStack = null;

                    switch (targetInventory)
                    {
                        case InventoryTypes.Pocket:
                            itemStack = menu.Inventory.InventoryList[itemSlot];
                            break;

                        case InventoryTypes.Bag:
                            itemStack = menu.Bag.InventoryList[itemSlot];
                            break;

                        case InventoryTypes.Distant:
                            itemStack = menu.Distant.InventoryList[itemSlot];
                            break;
                    }

                    if (itemStack != null && itemStack.Item != null)
                    {
                        var item = itemStack.Item;

                        if (item is WeaponItem && item.Id == ItemID.Weapon)
                        {
                            (item as WeaponItem).Use(client, targetInventory, itemSlot);
                        }
                        else if (item is FoodItem)
                        {
                            (item as FoodItem).Use(client, targetInventory, itemSlot);
                        }
                        else if (item is ClothesItem)
                        {
                            (item as ClothesItem).Use(client, targetInventory, itemSlot);
                        }
                    }

                    Refresh(client, menu);
                }
            }
        }
        #endregion

        #region Drop
        private static void RPGInventory_DropItem([FromSource]Player client, string inventoryType, int itemID, int slot, int quantity)
        {
            Debug.WriteLine("RPGInventory_DropItem");
            try
            {
                if (quantity == 0)
                    return;

                var ph = client.GetPlayerDatabase();
                if (ph == null)
                    return;

                RPGInventoryMenu menu = null;
                RPGInventoryItem invItem = null;

                if (_clientMenus.TryGetValue(client, out menu))
                {
                    switch (inventoryType)
                    {
                        case InventoryTypes.Pocket:
                            invItem = menu.PocketsItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);

                            if (invItem == null)
                                return;

                            if (invItem.Stack.Item.Drop(client, quantity, slot, menu.Inventory))
                            {
                                if (invItem.Stack.Quantity == 0)
                                    menu.PocketsItems.RPGInventoryItems.Remove(invItem);
                                else
                                    invItem.Quantity = invItem.Stack.Quantity;
                            }

                            break;
                        case InventoryTypes.Bag:
                            invItem = menu.BagItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);

                            if (invItem == null)
                                return;

                            if (invItem.Stack.Item.Drop(client, quantity, slot, menu.Bag))
                            {
                                if (invItem.Stack.Quantity == 0)
                                    menu.PocketsItems.RPGInventoryItems.Remove(invItem);
                                else
                                    invItem.Quantity = invItem.Stack.Quantity;
                            }

                            break;
                        case InventoryTypes.Distant:
                            invItem = menu.DistantItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);

                            if (invItem == null)
                                return;

                            if (invItem.Stack.Item.Drop(client, quantity, slot, menu.Distant))
                            {
                                if (invItem.Stack.Quantity == 0)
                                    menu.PocketsItems.RPGInventoryItems.Remove(invItem);
                                else
                                    invItem.Quantity = invItem.Stack.Quantity;
                            }

                            break;
                        case InventoryTypes.Outfit:
                            invItem = menu.OutfitItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);

                            if (invItem == null)
                                return;

                            if (invItem.Stack.Item.Drop(client, quantity, slot, menu.Outfit))
                            {
                                if (invItem.Stack.Quantity == 0)
                                    menu.PocketsItems.RPGInventoryItems.Remove(invItem);
                                else
                                    invItem.Quantity = invItem.Stack.Quantity;
                            }
                            RPGInventory_DeletePlayerProps(client, invItem.Stack.Item);

                            //ph.Clothing.UpdatePlayerClothing();
                            break;
                    }

                    // Temporary solution to save inventory after object drop. Doesn't update inventory when dropping from distant inventory.
                    ph.Update();
                    Refresh(client, menu);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        #endregion

        #region Give
        private static void RPGInventory_GiveItem([FromSource]Player client, string inventoryType, int itemID, int slot, int quantity)
        {
            Debug.WriteLine("RPGInventory_GiveItem");
            try
            {
                if (quantity == 0)
                    return;

                var ph = client.GetPlayerDatabase();
                if (ph == null)
                    return;

                RPGInventoryMenu menu = null;
                RPGInventoryItem invItem = null;
                _clientMenus.TryGetValue(client, out menu);

                if (menu != null)
                {
                    if (menu.DistantPlayer == null)
                        return;

                    PlayerData phDistant = menu.DistantPlayer.GetPlayerDatabase();

                    if (phDistant == null)
                        return;

                    switch (inventoryType)
                    {
                        case InventoryTypes.Pocket:
                            invItem = menu.PocketsItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);
                            if (invItem == null)
                                return;

                            if (phDistant.AddItem(invItem.Stack.Item, quantity))
                            {
                                menu.DistantPlayer.SendTipNotification($"On vous a donné {quantity} {invItem.Stack.Item.Name}");
                                ph.DeleteItem(slot, inventoryType, quantity);
                            }

                            break;
                        case InventoryTypes.Bag:
                            invItem = menu.BagItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);
                            if (invItem == null)
                                return;
                            if (phDistant.AddItem(invItem.Stack.Item, quantity))
                            {
                                menu.DistantPlayer.SendTipNotification($"On vous a donné {quantity} {invItem.Stack.Item.Name}");
                                ph.DeleteItem(slot, inventoryType, quantity);
                            }

                            break;
                        case InventoryTypes.Distant:
                            invItem = menu.DistantItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);
                            if (invItem == null)
                                return;
                            if (phDistant.AddItem(invItem.Stack.Item, quantity))
                            {
                                menu.DistantPlayer.SendTipNotification($"On vous a donné {quantity} {invItem.Stack.Item.Name}");
                                ph.DeleteItem(slot, inventoryType, quantity);
                            }
                            break;
                        case InventoryTypes.Outfit:
                            invItem = menu.OutfitItems.RPGInventoryItems.Find(s => s.InventorySlot == slot);
                            if (invItem == null)
                                return;
                            if (phDistant.AddItem(invItem.Stack.Item, quantity))
                            {
                                menu.DistantPlayer.SendTipNotification($"On vous a donné {quantity} {invItem.Stack.Item.Name}");
                                ph.DeleteItem(slot, inventoryType, quantity);
                            }
                            RPGInventory_SetPlayerProps(client, invItem.Stack.Item);

                           // ph.Clothing.UpdatePlayerClothing();

                            break;
                    }

                    Refresh(client, menu);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region Switch
        private static void RPGInventory_SwitchItemInventory_SRV([FromSource]Player client, string targetRPGInv, string oldRPGInv, int itemID, int slotID, int oldslotID)
        {
            Logger.Debug("RPGInventory_SwitchItemInventory_SRV");

            RPGInventoryMenu menu = null;
            _clientMenus.TryGetValue(client, out menu);

            if (menu != null)
            {
                Inventory oldInventory = null;

                switch (oldRPGInv) // OLD Inventory
                {
                    case InventoryTypes.Pocket:
                        oldInventory = menu.Inventory;
                        break;
                    case InventoryTypes.Bag:
                        oldInventory = menu.Bag;
                        break;
                    case InventoryTypes.Distant:
                        oldInventory = menu.Distant;
                        break;
                }

                ItemStack stack = null;
                Item item = null;

                PlayerData player = client.GetPlayerDatabase();
                if (player == null)
                    return;

                if (oldInventory != null)
                {
                    if (oldInventory.InventoryList[oldslotID] != null)
                    {
                        stack = oldInventory.InventoryList[oldslotID];
                        item = stack?.Item;
                    }
                    oldInventory.OnItemRemoved(client, stack);
                }
                else
                {
                    if (oldRPGInv == InventoryTypes.Outfit)
                    {
                        stack = menu.Outfit.Slots[oldslotID];
                        item = stack?.Item;
                    }
                }

                if (item != null)
                {
                    if (oldRPGInv == targetRPGInv) // Changement de slots
                    {
                        if (oldInventory == null) // Changement de slots dans le outfit?! 
                            return;

                        //stack.SlotIndex = slotID;
                        if (oldInventory.InventoryList[slotID] != null)
                        {
                            if (oldInventory.InventoryList[slotID].Item.Id == item.Id)
                            {
                                oldInventory.InventoryList[slotID].Quantity += stack.Quantity;
                            }
                        }
                        else
                        {
                            oldInventory.InventoryList[slotID] = stack;
                        }

                        oldInventory.InventoryList[oldslotID] = null;

                        switch (oldRPGInv) // OLD Inventory
                        {
                            case InventoryTypes.Pocket:
                                menu.Inventory = oldInventory;
                                break;
                            case InventoryTypes.Bag:
                                menu.Bag = oldInventory;
                                break;
                            case InventoryTypes.Distant:
                                menu.Distant = oldInventory;
                                break;
                        }
                    }
                    else
                    {
                        /*
                        if (stack.Item.Id == ItemID.Bag && targetRPGInv != InventoryTypes.Outfit)
                        {
                            var backpack = item as BagItem;

                            if (!backpack.InventoryBag.IsEmpty())
                            {
                                menu.CloseMenu(client);
                                client.SendNotificationError("Votre sac n'est pas vide!");
                                return;
                            }
                        }*/

                        switch (targetRPGInv) // NEW Inventory
                        {
                            case InventoryTypes.Pocket:
                                if (!menu.Inventory.IsFull(stack.Quantity * stack.Item.Weight)) // vérification si y'a de la place
                                {
                                    if (menu.Inventory.InventoryList[slotID] != null)
                                    {
                                        if (menu.Inventory.InventoryList[slotID].Item.Id == stack.Item.Id)
                                        {
                                            menu.Inventory.InventoryList[slotID].Quantity += stack.Quantity;
                                        }
                                    }
                                    else
                                    {
                                        menu.Inventory.InventoryList[slotID] = stack;
                                    }
                                    menu.Inventory?.OnItemAdded(client, menu.Inventory.InventoryList[slotID]);
                                }
                                else
                                {
                                    //client.SendNotificationError("Vous n'avez pas assez de place pour faire ça");
                                    return;
                                }

                                break;

                            case InventoryTypes.Bag:
                                if (!menu.Bag.IsFull(stack.Quantity * stack.Item.Weight)) // vérification si y'a de la place
                                {
                                    if (stack.Item.Id == ItemID.Bag)
                                    {
                                        menu.CloseMenu(client);
                                        //client.SendNotificationError("Euh ... non!");
                                        return;
                                    }

                                    if (menu.Bag.InventoryList[slotID] != null)
                                    {
                                        if (menu.Bag.InventoryList[slotID].Item.Id == stack.Item.Id)
                                        {
                                            menu.Bag.InventoryList[slotID].Quantity += stack.Quantity;
                                        }
                                    }
                                    else
                                    {
                                        menu.Bag.InventoryList[slotID] = stack;
                                    }
                                }
                                else
                                {
                                    //client.SendNotificationError("Vous n'avez pas assez de place pour faire ça");
                                    return;
                                }

                                break;

                            case InventoryTypes.Distant:
                                if (!menu.Distant.IsFull(stack.Quantity * stack.Item.Weight)) // vérification si y'a de la place
                                {
                                    if (menu.Distant.InventoryList[slotID] != null)
                                    {
                                        if (menu.Distant.InventoryList[slotID].Item.Id == stack.Item.Id)
                                        {
                                            menu.Distant.InventoryList[slotID].Quantity += stack.Quantity;
                                        }
                                    }
                                    else
                                    {
                                        menu.Distant.InventoryList[slotID] = stack;
                                    }
                                    menu.Distant?.OnItemAdded(client, menu.Distant.InventoryList[slotID]);
                                }
                                else
                                {
                                    //client.SendNotificationError("Vous n'avez pas assez de place pour faire ça");
                                    return;
                                }

                                break;

                            case InventoryTypes.Outfit:
                                menu.Outfit.Slots[slotID] = stack;

                                break;
                        }

                        if (oldInventory != null)
                            oldInventory.InventoryList[oldslotID] = null;

                        #region Clothing 
                        // Remove
                        if (oldRPGInv == InventoryTypes.Outfit)
                        {
                            RPGInventory_DeletePlayerProps(client, item);

                            menu.Outfit.Slots[oldslotID] = null;
                        }
                        // Equip
                        else if (targetRPGInv == InventoryTypes.Outfit)
                        {
                            RPGInventory_SetPlayerProps(client, item);
                        }
                        #endregion
                    }
                }

                menu.OnMove?.Invoke(client, menu);
                Refresh(client, menu);
                player.Update();
            }
        }
        #endregion

        #region Split
        private static void RPGInventory_SplitItemInventory_SRV([FromSource]Player client, string inventoryType, int itemID, int newSlot, int oldSlot, int oldCount, int newCount, int splitCount)
        {
            RPGInventoryMenu menu;
            Inventory inv = null;

            if (_clientMenus.TryGetValue(client, out menu))
            {
                switch (inventoryType)
                {
                    case InventoryTypes.Pocket:
                        inv = menu.Inventory;
                        break;
                    case InventoryTypes.Bag:
                        inv = menu.Bag;
                        break;
                    case InventoryTypes.Distant:
                        inv = menu.Distant;
                        break;
                }

                try
                {
                    if (inv != null && inv.InventoryList[oldSlot] != null && oldCount == inv.InventoryList[oldSlot].Quantity && oldCount - splitCount == newCount)
                    {
                        inv.InventoryList[oldSlot].Quantity -= splitCount;

                        var cloneItem = (ItemStack)inv.InventoryList[oldSlot].Clone();
                        cloneItem.Quantity = splitCount;

                        inv.InventoryList[newSlot] = cloneItem;
                       // client.GetPlayerHandler().UpdateFull();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            new RPGInventoryMenu(menu.Inventory, menu.Outfit, menu.Bag, menu.Distant).OpenMenu(client);
        }
        #endregion

        #region Price
        private static void RPGInventory_PriceItemInventory_SRV([FromSource]Player client, string inventoryType, int itemID, int slot, int price)
        {
            RPGInventoryMenu menu;
            Inventory inv = null;

            if (_clientMenus.TryGetValue(client, out menu))
            {
                switch (inventoryType)
                {
                    case InventoryTypes.Pocket:
                        inv = menu.Inventory;
                        break;
                    case InventoryTypes.Bag:
                        inv = menu.Bag;
                        break;
                    case InventoryTypes.Distant:
                        inv = menu.Distant;
                        break;
                }

                if (inv != null)
                {
                    if (inv.InventoryList[slot] != null)
                    {
                        inv.InventoryList[slot].Price = price;
                        menu.PriceChange?.Invoke(client, menu, inv.InventoryList[slot], price);
                    }
                }
            }
        }
        #endregion

        #region Refresh
        public static void Refresh(Player sender, RPGInventoryMenu menu)
        {
            menu.PocketsItems.RPGInventoryItems = new List<RPGInventoryItem>();

            for (int i = 0; i < menu.Inventory.InventoryList.Length; i++)
            {
                if (menu.Inventory.InventoryList[i] != null && menu.Inventory.InventoryList[i].Item != null)
                {
                    menu.PocketsItems.RPGInventoryItems.Add(new RPGInventoryItem(menu.Inventory.InventoryList[i], InventoryTypes.Pocket, i));
                }
            }

            if (menu.Bag != null)
            {
                menu.BagItems = new RPGInventory
                {
                    CurrentSize = menu.Bag.CurrentSize(),
                    MaxSize = menu.Bag.MaxSize,
                    Slots = menu.Bag.MaxSlot,
                    RPGInventoryItems = new List<RPGInventoryItem>()
                };

                for (int i = 0; i < menu.Bag.InventoryList.Length; i++)
                {
                    if (menu.Bag.InventoryList[i] != null && menu.Bag.InventoryList[i].Item != null)
                        menu.BagItems.RPGInventoryItems.Add(new RPGInventoryItem(menu.Bag.InventoryList[i], InventoryTypes.Bag, i));
                }
            }
            else
            {
                menu.BagItems = null;
            }

            if (menu.Distant != null)
            {
                bool market = false;
                if (menu.DistantItems != null)
                    market = menu.DistantItems.IsMarket;

                menu.DistantItems = new RPGInventory
                {
                    CurrentSize = menu.Distant.CurrentSize(),
                    MaxSize = menu.Distant.MaxSize,
                    Slots = menu.Distant.MaxSlot,
                    RPGInventoryItems = new List<RPGInventoryItem>(),
                    IsMarket = market
                };

                for (int i = 0; i < menu.Distant.InventoryList.Length; i++)
                {
                    if (menu.Distant.InventoryList[i] != null && menu.Distant.InventoryList[i].Item != null)
                        menu.DistantItems.RPGInventoryItems.Add(new RPGInventoryItem(menu.Distant.InventoryList[i], InventoryTypes.Distant, i));
                }
            }

            if (menu.Outfit != null)
            {
                menu.OutfitItems = new RPGInventoryOutfit
                {
                    NamedSlots = new RPGOutfitSlots[18],
                    RPGInventoryItems = new List<RPGInventoryItem>()
                };
                for (int i = 0; i < menu.Outfit.Slots.Length; i++)
                {
                    if (menu.Outfit.Slots[i] != null)
                    {
                        menu.OutfitItems.NamedSlots[i] = new RPGOutfitSlots(i + 1, (menu.Outfit.Slots[i].Item != null) ? menu.Outfit.Slots[i].Item.Name : "", OutfitInventory.OutfitClasses[i], true);
                        menu.OutfitItems.RPGInventoryItems.Add(new RPGInventoryItem(menu.Outfit.Slots[i], InventoryTypes.Outfit, i));
                    }
                    else
                    {
                        menu.OutfitItems.NamedSlots[i] = new RPGOutfitSlots(i + 1, "", OutfitInventory.OutfitClasses[i], true);
                    }
                }
            }
            Logger.Info("refresh");
            sender.TriggerEvent("InventoryManager_OpenMenu",
                    JsonConvert.SerializeObject(menu.PocketsItems),
                    JsonConvert.SerializeObject(menu.BagItems),
                    JsonConvert.SerializeObject(menu.DistantItems),
                    JsonConvert.SerializeObject(menu.OutfitItems),
                    (menu.DistantPlayer == null) ? false : true);

        }
        #endregion

        #region Close
        private static void RPGInventory_ClosedMenu_SRV([FromSource]Player client)
        {
            var player = client;

            if (_clientMenus.TryRemove(player, out RPGInventoryMenu menu))
                menu.OnClose?.Invoke(player, menu);
        }
        #endregion
    }
}