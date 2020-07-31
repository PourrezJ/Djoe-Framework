using Server.Utils;
using Shared;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class HorseData
    {
        public string OwnerID;
        public int Hash;
        public string Name;
        public UCoords LastCoords;
        public Inventory Inventory;
        public bool NeedUpdate;

        public async Task UpdateAsync()
        {
            var result = await Database.MongoDB.Update(this, "players", OwnerID);

            if (result.MatchedCount == 0)
                Logger.Warn($"Update error for horse {OwnerID} {Name}");
        }
    }
}
