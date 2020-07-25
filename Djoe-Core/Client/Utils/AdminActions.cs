using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_cl.Utils
{
    class AdminActions : BaseScript
    {
        public static async Task TeleportAndFoundGroundAsync(Vector3 tpCoords)
        {
            float groundZ = 0.0F;
            Vector3 normal = new Vector3(1.0f,1.0f,1.0f);
            bool foundGround = false;
            

            for (int i = 1; i < 1000.0; i++)
            {
                API.SetEntityCoords(API.PlayerPedId(), tpCoords.X, tpCoords.Y, (float)i, true, true, true, false);
                foundGround = API.GetGroundZAndNormalFor_3dCoord(tpCoords.X, tpCoords.Y, (float)i, ref groundZ, ref normal);
                await Delay(1);
                if (foundGround==true)
                {  
                    API.SetEntityCoords(API.PlayerPedId(), tpCoords.X, tpCoords.Y, (float)i, true, true, true, false);
                    break;
                }
            }
        }
    }
}
