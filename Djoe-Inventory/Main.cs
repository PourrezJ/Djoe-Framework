using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Inventory.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Inventory
{
    public class Main : BaseScript
    {
        #region Fields
        private bool show;
        private DateTime lastCheck = DateTime.Now;
        #endregion

        #region C4Tor
        public Main()
        {
            Debug.WriteLine("Resource Inventory loading...");

            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }
        #endregion

        #region Events
        private void OnClientResourceStop(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName)
                return;

            ShowInventory(false);
        }

        private void OnClientResourceStart(string name)
        {
            if (API.GetCurrentResourceName() != name)
                return;

            API.SetNuiFocus(false, false);

            EventHandlers["InventoryManager_OpenMenu"] += new Action<string, string, string, string, bool>(OpenInventory);


            RegisterNUICallback("exit", CloseInventory);

            RegisterNUICallback("inventoryUseItem", InventoryUseItem);
            RegisterNUICallback("inventoryDropItem", InventoryDropItem);
            RegisterNUICallback("inventorySwitchItem", InventorySwitchItem);
            RegisterNUICallback("inventoryChangeItemPrice", InventoryChangeItemPrice);
            RegisterNUICallback("inventorySplitItem", InventorySplitItem);


            // Todo
            // inventoryGiveItem

            Debug.WriteLine("Resource Inventory loaded.");

        }
        private void OpenInventory(string pocketsItems, string bagItems, string distantItems, string outfitItems, bool give)
        {
            Debug.WriteLine("Ouverture de l'inventaire");

            if (CheckCallBack())
                return;

            if (!show)
                ShowInventory(true);

            Nui data = new Nui()
            {
                UIName = "Inventory",
                Action = "ItemsLoaded",
                Data = new
                {
                    Pocket = JsonConvert.DeserializeObject(pocketsItems),
                    Bag = JsonConvert.DeserializeObject(bagItems),
                    Distant = JsonConvert.DeserializeObject(distantItems),
                    Outfit = JsonConvert.DeserializeObject(outfitItems),
                    Give = give
                }
            };
            data.SendNuiMessage();
        }

        #endregion

        #region NUI Callback
        private CallbackDelegate InventoryUseItem(IDictionary<string, object> data, CallbackDelegate callback)
        {
            if (CheckCallBack())
                return callback;


            TriggerServerEvent("RPGInventory_UseItem", data["id"], data["inventoryType"], data["inventorySlot"]);
            return callback;
        }

        private CallbackDelegate InventoryDropItem(IDictionary<string, object> data, CallbackDelegate callback)
        {
            if (CheckCallBack())
                return callback;

            TriggerServerEvent("RPGInventory_DropItem", data["inventoryType"], data["id"], data["inventorySlot"], data["value"]);
            return callback;
        }

        private CallbackDelegate InventorySwitchItem(IDictionary<string, object> data, CallbackDelegate callback)
        {
            if (CheckCallBack())
                return callback;

            TriggerServerEvent("RPGInventory_SwitchItemInventory_SRV", data["slottype"], data["oldtype"], data["olditemid"], data["slotindex"], data["invSlot"]);
            return callback;
        }

        private CallbackDelegate InventoryChangeItemPrice(IDictionary<string, object> data, CallbackDelegate callback)
        {
            if (CheckCallBack())
                return callback;

            TriggerServerEvent("RPGInventory_PriceItemInventory_SRV", data["inventoryType"], data["id"], data["inventorySlot"], data["value"]);
            return callback;
        }

        private CallbackDelegate InventorySplitItem(IDictionary<string, object> data, CallbackDelegate callback)
        {
            if (CheckCallBack())
                return callback;

            TriggerServerEvent("RPGInventory_SplitItemInventory_SRV", data["inventoryType"], data["id"], data["newSlot"], data["oldSlot"], data["oldCount"], data["quantity"], data["splitCount"]);
            return callback;
        }

        private CallbackDelegate CloseInventory(IDictionary<string, object> data, CallbackDelegate callback)
        {
            ShowInventory(false);
            this.show = false;
            return callback;
        }
        #endregion

        #region Methods
        private void ShowInventory(bool show)
        {
            Debug.WriteLine("ShowInventory: " + show.ToString());
            this.show = show;

            var data = new Nui()
            {
                UIName = "Inventory",
                Action = "Activate",
                Data = show
            };
            data.SendNuiMessage();
            API.SetNuiFocus(show, show);
        }


        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            Debug.WriteLine($"Registering NUI EventHandler for {msg}");
            API.RegisterNuiCallbackType(msg);
            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
            EventHandlers[msg] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
        }

        public bool CheckCallBack()
        {
            var time = DateTime.Now - lastCheck;

            if (time.TotalMilliseconds < 100)
                return true;

            lastCheck = DateTime.Now;
            return false;
        }
        #endregion
    }
}
