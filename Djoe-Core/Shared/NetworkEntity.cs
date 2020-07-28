using CitizenFX.Core;

namespace Shared
{
    public class NetworkEntity
    {
        public int NetworkID;
        public UCoords LastCoord;
        public float Health;
        public int Model;

    }

    public class VehicleNetwork : NetworkEntity
    {

    }

    public class PedNetwork : NetworkEntity
    {
        public int PedHandle; // Seulement utilisable par le client, il est différent pour chaque client.
        public bool IsPositionFrozen;
        public bool Networked;
        public bool TaskWanderStandard;
        public bool Invincible;
    }

    public class HorseNetwork : PedNetwork
    {

    }

    public class BlipNetwork
    {
        public string Name;
        public uint Sprite;
        public uint Type;
        public Vector3 Position;
        public float Rotation;

        public BlipNetwork(string name, uint sprite, uint type, Vector3 pos, float rot)
        {
            Name = name;
            Sprite = sprite;
            Type = type;
            Position = pos;
            Rotation = rot;
        }
    }
}
