using System;

namespace Client.Utils.Extensions
{
    public static class Vector3Extensions
    {
        public static CitizenFX.Core.Vector3 Forward(this CitizenFX.Core.Vector3 point, float rot, float dist)
        {
            var angle = rot;
            double xOff = -(Math.Sin((angle * Math.PI) / 180) * dist);
            double yOff = Math.Cos((angle * Math.PI) / 180) * dist;

            return point + new CitizenFX.Core.Vector3((float)xOff, (float)yOff, 0);
        }

        public static CitizenFX.Core.Vector3 Backward(this CitizenFX.Core.Vector3 point, float rot, float dist)
        {
            var angle = rot;
            double xOff = (Math.Cos((angle * Math.PI) / 180) * dist);
            double yOff = -(Math.Sin((angle * Math.PI) / 180) * dist);

            return point + new CitizenFX.Core.Vector3((float)xOff, (float)yOff, 0);
        }

    }
}
