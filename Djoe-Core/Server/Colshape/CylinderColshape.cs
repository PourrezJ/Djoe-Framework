using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Utils.Extensions;
using System;
using System.Runtime.ExceptionServices;
using System.Security;

namespace Server.Colshape
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
            if (entity == null)
            {
                return false;
            }

            lock (entity)
            {
                /*
                if (entity.Character == null) < -- can't work server side on RedM | FiveM without OneSync
                    return false;*/

                var data = entity.GetPlayerDatabase();

                if (data == null)
                    return false;

                if (data.LastCoord == null)
                    return false;

                if (data.LastCoord.Z < Position.Z || data.LastCoord.Z > Position.Z + Height)
                    return false;

                return IsPositionInside(data.LastCoord.ToVector3());
            }
        }

        public override bool IsPositionInside(Vector3 position)
        {
            return Position.DistanceToSquared2D(position) <= Radius;
        }
        #endregion
    }
}
