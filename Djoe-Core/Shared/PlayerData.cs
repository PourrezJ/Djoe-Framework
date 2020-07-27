using Newtonsoft.Json;
using Server.Models;

namespace Shared
{
    public class Identite
    {
        public string LastName;
        public string FirstName;
        public int Age;
        public string Nationality;

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }
    }

    public class PlayerData : Entity
    {
        [JsonIgnore]
        public dynamic Client;

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

        public BankAccount BankAccount;

        public Inventory PocketInventory;

        public Inventory BagInventory;

        public OutfitInventory OutfitInventory;

        public PlayerData()
        {

        }
    }
}
