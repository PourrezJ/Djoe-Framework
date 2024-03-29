﻿using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Utils.Enums;
using Shared;
using System;
using System.Collections.Concurrent;
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

            if (!_datas.ContainsKey(player))
            {
                result = default(T);
                return false;
            }

            var data = _datas[player];

            bool res = data.TryGetValue(key, out value);
            result = res ? (T)value : default(T);
            return res;
        }

        public static void SetData(this Player player, string key, object value)
        {
            if (_datas.ContainsKey(player))
            {
                _datas[player].AddOrUpdate(key, value, (k, oldValue) => value);
            }
            else
            {
                var data = new ConcurrentDictionary<string, object>();
                data.AddOrUpdate(key, value, (k, oldValue) => value);
                _datas.TryAdd(player, data);
                //_datas[player].AddOrUpdate(key, value, (k, oldValue) => value);
            }
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
            lock (PlayerManager.Characters)
            {
                if (PlayerManager.Characters.ContainsKey(sid))
                {
                    return PlayerManager.Characters[sid];
                }

            }

            return null;
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
                    /*
                case InventoryTypes.Bag:
                    return playerData.BagInventory.Delete(slot, quantity);

                case InventoryTypes.Outfit:
                    return playerData.OutfitInventory.Delete(slot, quantity);*/
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

                if (playerData.Client != null)
                {
                    var client = playerData.Client as Player;

                    JObject postUi = new JObject();
                    postUi.Add("type", "removeMoney");
                    postUi.Add("moneyvalue", playerData.Money);

                    client?.TriggerEvent("djoe:removeMoneyUi", postUi.ToString());
                }

                return true;
            }
            return false;
        }

        public static void AddMoney(this PlayerData playerData, double money)
        {
            playerData.Money += money;

            if (playerData.Client != null)
            {
                var client = playerData.Client as Player;

                JObject postUi = new JObject();
                postUi.Add("type", "addMoney");
                postUi.Add("moneyvalue", playerData.Money);

                Console.WriteLine("add money " + money);

                client?.TriggerEvent("djoe:addMoneyUi", postUi.ToString());
            }
        }
        #endregion

        #region Menu
        #endregion

        #region API
        public static void SetPosition(this Player client, Vector3 position, int fadedelay = 0)
        {
            client.TriggerEvent("SetPlayerPosition", position.X, position.Y, position.Z, fadedelay);
        }

        public static void PlayAnimation(this Player client, string animDict, string animName, float blendInSpeed = 8f, float blendOutSpeed = -8f, int duration = -1, AnimationFlags flags = 0, float playbackRate = 0f)
        {
            var animsync = new AnimationsSync()
            {
                AnimName = animName,
                AnimDict = animDict,
                BlendInSpeed = blendInSpeed,
                BlendOutSpeed = blendOutSpeed,
                Duraction = duration,
                Flag = (int)flags,
                PlaybackRate = playbackRate
            };

            client.TriggerEvent("API_PlayAnimation", JsonConvert.SerializeObject(animsync));
        }

        public static void PlayScenario(this Player client, string name)
        {
            client.TriggerEvent("API_PlayScenario", name);
        }

        public static void ClearSecondaryTask(this Player client)
        {
            client.TriggerEvent("API_ClearSecondary");
        }
        #endregion
    }
}
