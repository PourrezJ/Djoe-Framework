using CitizenFX.Core;
using ClientExtended.External;

namespace Client.Colshape
{
    public class SphereColshape : Colshape
    {
        #region Constructor
        public SphereColshape(uint id, Vector3 position, float radius) : base(id, position, radius)
        {
        }
        #endregion

        #region Public methods
        public override bool IsEntityInside(Player entity)
        {
            if (entity.Character == null)
                return false;

            return IsPositionInside(entity.Character.Position);
        }

        public override bool IsPositionInside(Vector3 position)
        {
            return Position.DistanceToSquared(position) <= Radius;
        }
        #endregion
    }
}
