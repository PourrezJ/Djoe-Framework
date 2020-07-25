using CitizenFX.Core;
using System;

namespace Shared
{
    public class UCoords
    {
        public UCoords(float x, float y, float z, float heading)
        {
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float Heading { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public float DistanceTo(Vector3 position)
        {
            return (position - new Vector3(X, Y, Z)).Length();
        }

        public float DistanceTo2D(Vector3 position)
        {
            return (new Vector3(position.X, position.Y, 0) - new Vector3(X, Y, 0.0f)).Length();
        }

        public void SetUcoord(Vector3 pos, float heading)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
            Heading = heading;
        }
    }
}
