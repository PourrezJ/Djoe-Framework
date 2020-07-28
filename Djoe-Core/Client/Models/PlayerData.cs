using Shared;

namespace Client.Models
{
    public class PlayerData
    {
        public int NetworkID;
        public UCoords LastCoord;
        public float Health;
        public int Model;

        public string SteamID;
        public Identite Identity;

        public double Money;
        public bool IsDead;
        public StaffRank StaffRank = StaffRank.Citoyen;
        public Clothes Clothes;
        public Character SkinPlayer;
        public int Hunger = 100;
        public int Thirst = 100;
    }
}