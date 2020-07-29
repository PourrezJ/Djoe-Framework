using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Models;
using Server.Utils;
using Shared;
using System;

namespace Server
{
    public class PlayerData : NetworkEntity
    {
        [JsonIgnore]
        public Player Client;

        public bool NeedUpdate;

        public string SteamID;
        public Identite Identity;
       
        // Use PlayerExtension for add money if you want show update in UI
        public double Money;
        public bool IsDead;
        public StaffRank StaffRank = StaffRank.Citoyen;
        public Clothes Clothes;
        public Character SkinPlayer;
        public int Hunger = 100;
        public int Thirst = 100;
        public VIP Vip;

        public BankAccount BankAccount;

        public Inventory PocketInventory;

        public Inventory BagInventory;

        public OutfitInventory OutfitInventory;

        public PlayerData()
        {

        }

        public void UpdateUI()
        {
            if (Client == null)
                return;

            if (!PlayerManager.Characters.ContainsKey(Client.Identifiers["steam"]))
                return;

            JObject postUi = new JObject();
            postUi.Add("type", "ui");
            postUi.Add("action", "update");
            postUi.Add("moneyvalue", Math.Round(Money, 2));
            postUi.Add("thirstvalue", Thirst);
            postUi.Add("hungervalue", Hunger);

            Client.TriggerEvent("djoe:updateUi", postUi.ToString());
        }

        public void OpenInventory(Player player)
        {
            Logger.Debug("Open Inventory");

            // Inventory distant = null; // Todo besoin de connaitre quel inventaire distant ouvrir si besoin

            new RPGInventoryMenu(PocketInventory, OutfitInventory, BagInventory).OpenMenu(player);
        }
    }
}
