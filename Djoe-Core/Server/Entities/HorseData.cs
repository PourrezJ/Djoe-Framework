using Shared;

namespace Server.Entities
{
    public class HorseData
    {
        public string OwnerID;
        public int Hash;
        public string Name;
        public UCoords LastCoords;
        public Inventory Inventory;
    }
}
