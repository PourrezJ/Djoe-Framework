using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Server.Utils;
using Shared;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class HorseData
    {
        public ObjectId _id { get; private set; }
        public string OwnerID;
        public int Hash;
        public string Name;
        public UCoords LastCoords;
        public Inventory Inventory;
        public bool NeedUpdate;
        public bool IsDefault;
        public int Health;
        public int ParkID;

        [BsonIgnore]
        public PedNetwork PedNetwork;

        public async Task UpdateAsync()
        {
            var result = await Database.MongoDB.Update(this, "horses", _id);

            if (result.MatchedCount == 0)
                Logger.Warn($"Update error for horse {OwnerID} {Name}");
        }
    }
}
