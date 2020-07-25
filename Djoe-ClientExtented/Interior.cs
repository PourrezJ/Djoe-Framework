using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ClientExtented
{
    public class Interior
    {
        public Vector3 Position;
        public string Name;
        private bool actived;

        public bool Active
        {
            set => activeInt(value);
            get => actived;
        }

        public Interior(Vector3 position, string name)
        {
            Position = position;
            Name = name;
        }

        private void activeInt(bool active)
        {
            actived = active;
            var intdata = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, Position.X, Position.Y, Position.Z);

            if (active)
            {
                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, intdata, 0);
            }
            else
            {
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, intdata, true);
            }
        }
    }
}
