using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Utils.Extensions
{
    public static class PlayerExtensions
    {
        #region Datas
        public static ConcurrentDictionary<Player, ConcurrentDictionary<string, object>> _datas = new ConcurrentDictionary<Player, ConcurrentDictionary<string, object>>();

        public static bool GetData<T>(this Player player, string key, out T result)
        {
            object value;
            bool res = _datas[player].TryGetValue(key, out value);
            result = (T)value;
            return res;
        }

        public static void SetData(this Player player, string key, object value)
        {
            _datas[player].AddOrUpdate(key, value, (k, oldValue) => value);
        }
        #endregion

        #region Notification
        public static void SendTipNotification(this Player client, string text, int duration = 7000)
        {
            if (text == "")
                return;

            client.TriggerEvent("djoe:Tip", text, duration);
        }

        public static void SendTipRightNotification(this Player client, string text, int duration = 7000)
        {
            if (text == "")
                return;

            client.TriggerEvent("djoe:TipRight", text, duration);
        }

        public static void SendTipBottomNotification(this Player client, string text, int duration = 7000)
        {
            if (text == "")
                return;

            client.TriggerEvent("djoe:TipBottom", text, duration);
        }
        #endregion

        #region Misc
        public static string GetSteamID(this Player player)
        {
            return player.Identifiers["steam"];
        }

        public static PlayerData GetPlayerDatabase(this Player player)
        {
            if (player == null)
                return null;

            var sid = player.Identifiers["steam"];
            if (PlayerManager.Characters.ContainsKey(sid))
            {
                return PlayerManager.Characters[sid];
            }

            return null;
        }

        public static void SetPosition(this Player client, Vector3 position, int fadedelay = 0)
        {
            client.TriggerEvent("SetPlayerPosition", position.X, position.Y, position.Z, fadedelay);
        }
        #endregion

        #region Save
        public static void Update(this PlayerData playerData)
        {
            Task.Run(async () =>
            {
                var result = await Database.MongoDB.Update(playerData, "players", playerData.SteamID);

                if (result.MatchedCount == 0)
                    Logger.Warn($"Update error for player {playerData.SteamID}");
            });
        }

        public static async Task UpdateAsync(this PlayerData playerData)
        {
            var result = await Database.MongoDB.Update(playerData, "players", playerData.SteamID);

            if (result.MatchedCount == 0)
                Logger.Warn($"Update error for player {playerData.SteamID}");
        }
        #endregion

        #region Inventory
        public static bool AddItem(this PlayerData playerData, Item item, int quantity = 1)
        {
            Player client = playerData.Client;

            if (playerData.PocketInventory.AddItem(item, quantity))
            {
                if (RPGInventoryManager.HasInventoryOpen(client))
                {
                    var rpg = RPGInventoryManager.GetRPGInventory(client);
                    if (rpg != null)
                        RPGInventoryManager.Refresh(client, rpg);
                }

                return true;
            }
            else if (playerData.BagInventory != null && playerData.BagInventory.AddItem(item, quantity))
            {
                if (RPGInventoryManager.HasInventoryOpen(client))
                {
                    var rpg = RPGInventoryManager.GetRPGInventory(client);
                    if (rpg != null)
                        RPGInventoryManager.Refresh(client, rpg);
                }

                return true;
            }
            else
                return false;
        }

        public static bool HasItemID(this PlayerData playerData, ItemID id)
        {
            if (playerData.PocketInventory.HasItemID(id))
                return true;
            else if (playerData.BagInventory != null && playerData.BagInventory.HasItemID(id))
                return true;
            else
                return false;
        }

        public static bool DeleteItem(this PlayerData playerData, int slot, string inventoryType, int quantity)
        {
            switch (inventoryType)
            {
                case InventoryTypes.Pocket:
                    return playerData.PocketInventory.Delete(slot, quantity);

                case InventoryTypes.Bag:
                    return playerData.BagInventory.Delete(slot, quantity);

                case InventoryTypes.Outfit:
                    return playerData.OutfitInventory.Delete(slot, quantity);
            }

            return false;
        }
        #endregion

        #region Money
        public static bool HasMoney(this PlayerData playerData, double money)
        {
            if (playerData.Money >= money)
            {
                playerData.Money -= money;
                return true;
            }
            return false;
        }
        #endregion
    }
}
