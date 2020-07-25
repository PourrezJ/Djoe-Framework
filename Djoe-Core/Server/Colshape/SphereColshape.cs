using CitizenFX.Core;
using Server.Utils.Extensions;
using System;

namespace Server.Colshape
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
            /*
            if (entity.Character == null) < -- can't work server side on RedM | FiveM without OneSync
                return false;*/

            Console.WriteLine("bite");
            var data = entity?.GetPlayerDatabase();
            Console.WriteLine("bite 1");
            if (data == null)
                return false;
            Console.WriteLine("bite 2");


            return IsPositionInside(data.LastCoord.ToVector3());
        }

        public override bool IsPositionInside(Vector3 position)
        {
            return Position.DistanceToSquared(position) <= Radius;
        }
        #endregion
    }
}
