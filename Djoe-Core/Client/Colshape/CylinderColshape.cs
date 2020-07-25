using CitizenFX.Core;
using ClientExtended.External;
using System.Runtime.ExceptionServices;
using System.Security;

namespace Client.Colshape
{
    public class CylinderColshape : Colshape
    {
        #region Fields
        public float Height { get; }
        #endregion

        #region Constructor
        public CylinderColshape(uint id, Vector3 position, float radius, float height) : base(id, position, radius)
        {
            Height = height;
        }
        #endregion

        #region Public methods
        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public override bool IsEntityInside(Player entity)
        {
            lock (entity)
            {
                if (entity.Character == null)
                    return false;
                if (entity.Character.Position.Z < Position.Z || entity.Character.Position.Z > Position.Z + Height)
                    return false;

                return IsPositionInside(entity.Character.Position);
            }
        }

        public override bool IsPositionInside(Vector3 position)
        {
            return Position.DistanceToSquared2D(position) <= Radius;
        }
        #endregion
    }
}
