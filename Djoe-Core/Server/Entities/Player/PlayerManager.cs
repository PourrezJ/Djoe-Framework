using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Shared;
using Server.Utils;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using Server.Utils.Extensions;
using Server.Entities;
using Shared.Utils;
using System.Threading;
using Server.Models;
using Server.Controllers;

namespace Server
{
    public class PlayerManager
    {
        public static ConcurrentDictionary<string, PlayerData> Characters = new ConcurrentDictionary<string, PlayerData>();

        public static void Init()
        {
            GameMode.RegisterEventHandler("djoe:playerSpawn", new Action<Player>(PlayerSpawnFunction));
            GameMode.RegisterEventHandler("djoecharacter:SaveSkinDB", new Action<Player, string, string, string>(SaveSkinDB));
            GameMode.RegisterEventHandler("djoe:update", new Action<Player, Vector3, float, float>(UpdatePlayer));

            Thread InstanceCaller = new Thread(
            new ThreadStart(PlayerUpdateLoop));

            // Start the thread.
            InstanceCaller.Start();
        }

        private static async void PlayerUpdateLoop()
        {
            while (true)
            {
                foreach (var player in Characters)
                {
                    if (player.Value.Client != null && player.Value.NeedUpdate)
                    {
                        await player.Value.UpdateAsync();
                        player.Value.NeedUpdate = false;
                    }

                    Thread.Sleep(50);
                }

                Thread.Sleep(100);
            }
        }

        private static void UpdatePlayer([FromSource]Player source, Vector3 position, float heading, float health)
        {
            lock (Characters)
            {
                lock (source)
                {
                    var sid = source.Identifiers["steam"];
                    if (sid != null)
                    {
                        Characters[sid].LastCoord.SetUcoord(position, heading);
                        Characters[sid].Health = health;
                        Characters[sid].NeedUpdate = true;
                    }
                }
            }
        }

        private static void SaveSkinDB([FromSource]Player source, string skin, string components, string identityStr)
        {
            Character skinPlayer = JsonConvert.DeserializeObject<Character>(skin);
            Clothes componentsPlayer = JsonConvert.DeserializeObject<Clothes>(components);
            Identite identityPlayer = JsonConvert.DeserializeObject<Identite>(identityStr);

            var sid = source.Identifiers["steam"];

            Task.Run(async () =>
            {
                // Creation du personnage
                try
                {
                    var posTemp = Config.Get<dynamic>("FirstSpawnCoord");
                    var pos = new UCoords((float)posTemp["X"], (float)posTemp["Y"], (float)posTemp["Z"], (float)posTemp["Heading"]);

                    var pData = new PlayerData()
                    {
                        SteamID = sid,
                        identity = identityPlayer,
                        LastCoord = pos,
                        Money = Config.Get<double>("StartingMoney"),
                        Clothes = componentsPlayer,
                        SkinPlayer = skinPlayer,
                        PocketInventory = new Inventory(30, 16),
                        OutfitInventory = new OutfitInventory(),
                        BankAccount = new Models.BankAccount(Models.AccountType.Personal, BankAccount.GenerateNewAccountNumber(), Config.Get<double>("StartingBankMoney"))
                    };

                    Characters.TryAdd(sid, pData);

                    await Database.MongoDB.InsertAsync<PlayerData>("players", pData, "SaveSkinDB");

                    await BaseScript.Delay(0); // Back to the main thread

                    PlayerSpawnFunction(source);

                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
            });


            Logger.Info($"Ajout du joueur {sid} {source.Name} dans la base de donnée");
        }

        private static void PlayerSpawnFunction([FromSource] Player source)
        {
            string sid;

            //Logger.Debug(JsonConvert.SerializeObject(source.Identifiers));

            if (source.Identifiers["steam"] != null)
                sid = source.Identifiers["steam"];
            else
            {
                Logger.Info("Kick du joueur " + source.Name + " pour problème avec steam");
                source.Drop("Erreur avec Steam, est t'il bien démarrer?");
                return;
            }

            Logger.Info("Demande d'information du joueur: " + sid);

            if (!Characters.ContainsKey(sid))
            {
                Logger.Info("Envoi du joueur" + sid + " dans le character creator.");
                Characters[sid] = new PlayerData();
                source.TriggerEvent("djoe:createPlayer");
            }
            else
            {
                Logger.Info("Envoi des informations du joueur " + sid);
                var playerData = Characters[sid];
                playerData.Client = source;
                // Reset temporaire de l'inventaire
                playerData.PocketInventory = new Inventory(30, 16);
                playerData.OutfitInventory = new OutfitInventory();

                playerData.PocketInventory.AddItem(new Item(ItemID.Fromage, "Fromage", "", 0.1, true, true, true));

                BlipsManager.OnPlayerConnected(source);
                PedsManager.OnPlayerConnected(source);

                source.TriggerEvent("djoe:initPlayer", JsonConvert.SerializeObject(playerData), GameMode.WorldData.WorldTime.ToString(), (uint)GameMode.WorldData.WeatherActual);
            }
        }

        public static void OpenInventory(Player player)
        {
            Logger.Debug("Open Inventory");

           // Inventory distant = null; // Todo besoin de connaitre quel inventaire distant ouvrir si besoin

            var ph = player.GetPlayerDatabase();

            if (ph == null)
                return;

            new RPGInventoryMenu(ph.PocketInventory, ph.OutfitInventory, ph.BagInventory).OpenMenu(player);
        }

        public static bool AddItem(Player client, Item item, int quantity = 1)
        {
            PlayerData playerData = client.GetPlayerDatabase();

            if (playerData == null)
                return false;

            if (playerData.PocketInventory.AddItem(item, quantity))
            {
                //if (RPGInventoryManager.HasInventoryOpen(this.Client))
                //{
                //    var rpg = RPGInventoryManager.GetRPGInventory(this.Client);
                //    if (rpg != null)
                //        RPGInventoryManager.Refresh(this.Client, rpg);
                //}

                //item.OnPlayerGetItem(client);
                return true;
            }
            else if (playerData.BagInventory != null && playerData.BagInventory.AddItem(item, quantity))
            {
                //if (RPGInventoryManager.HasInventoryOpen(client))
                //{
                //    var rpg = RPGInventoryManager.GetRPGInventory(this.Client);
                //    if (rpg != null)
                //        RPGInventoryManager.Refresh(this.Client, rpg);
                //}

                //item.OnPlayerGetItem(this.Client);
                return true;
            }
            else
                return false;
        }

        private static void UpdateInventory([FromSource]Player client, string pocketStr, string bagStr)
        {
            Logger.Info("Update Inventory");
            PlayerData playerData = client.GetPlayerDatabase();

            if (playerData == null)
                return;

            Inventory pocketinventory = Newtonsoft.Json.JsonConvert.DeserializeObject<Inventory>(pocketStr);
            playerData.PocketInventory = pocketinventory;

            Inventory baginventory = null;

            if (string.IsNullOrEmpty(bagStr))
            {
                baginventory = Newtonsoft.Json.JsonConvert.DeserializeObject<Inventory>(bagStr);
                playerData.BagInventory = baginventory;
            }

            playerData.Update();
        }

        public static void UpdateUI(PlayerData pData)
        {
            if (pData.Client == null)
                return;

            var player = pData.Client as Player;

            if (!Characters.ContainsKey(player.Identifiers["steam"]))
                return;

            JObject postUi = new JObject();
            postUi.Add("type", "ui");
            postUi.Add("action", "update");
            postUi.Add("moneyquanty", pData.Money);
            postUi.Add("serverId", player.Handle);
            postUi.Add("water", pData.Thirst);
            postUi.Add("food", pData.Hunger);

            player.TriggerEvent("djoe:updateUi", postUi.ToString());
        }

        public static List<PlayerData> GetPlayerOnline()
        {
            return Characters.Values.ToList().FindAll(p => p.Client != null);
        }

        public static PlayerData GetPlayerBySteamID(string steamID)
        {
            try
            {
                var players = GameMode.PlayersList;
                for (int a = 0; a < players.Count(); a++)
                {
                    if (players[a] == null)
                        continue;

                    var psteamID = players[a].GetSteamID();
                    if (steamID.ToLower() == steamID.ToLower())
                        return players[a].GetPlayerDatabase();
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("GetPlayerBySCN: " + steamID + " " + ex);
            }

            return null;
        }
    }
}
