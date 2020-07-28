using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Models;
using Shared;

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
    }
}
