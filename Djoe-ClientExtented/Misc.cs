using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended.External;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ClientExtended
{
    public static class Misc
    {
        public static Vector3 GetCamDirection()
        {
            int pedID = API.PlayerPedId();

            float pedHeading = Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_HEADING) + Function.Call<float>(Hash.GET_ENTITY_HEADING, pedID);
            float camPitch = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 0).X;

            double x = -Math.Sin(pedHeading * Math.PI / 180.0);
            double y = Math.Cos(pedHeading * Math.PI / 180.0);
            double z = Math.Sin(camPitch * Math.PI / 180.0);

            return new Vector3((float)x, (float)y, (float)z);
        }

        // Need used on tick
        public static void ShowControlPressed()
        {
            float controlY = 0.01f;

            foreach (Control fi in (Control[])Enum.GetValues(typeof(Control)))
            {

                if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 0, fi))
                {
                    UIHelper.DrawText(fi.ToString(), 0.0f, controlY, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);
                    controlY += 0.02f;
                }
            }
        }

        public static async Task<Vector3> ForceGroundZ(Vector3 v)
        {
            float zcoord = 0.0f;
            var outArgb = new OutputArgument();


            float[] firstCheck = new float[] { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

            float[] secondCheck = new float[] { 1000, 900, 800, 700, 600, 500,
            400, 300, 200, 100, 0, -100, -200, -300, -400, -500 };

            float[] thirdCheck = new float[] { -500, -400, -300, -200, -100, 0,
            100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};

            if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
                zcoord = outArgb.GetResult<float>();

            if (zcoord == 0)
            {
                for (int i = 0; i < firstCheck.Length; i++)
                {
                    Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, firstCheck[i]);
                    await BaseScript.Delay(10);
                    //await GameMode.Delay(10);
                }

                if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
                    zcoord = outArgb.GetResult<float>();
            }

            if (zcoord == 0)
            {
                for (int i = 0; i < secondCheck.Length; i++)
                {
                    Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, secondCheck[i]);
                    await BaseScript.Delay(10);
                    //await GameMode.Delay(10);
                }

                if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
                    zcoord = outArgb.GetResult<float>();
            }

            if (zcoord == 0)
            {
                for (int i = 0; i < thirdCheck.Length; i++)
                {
                    Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, thirdCheck[i]);
                    await BaseScript.Delay(10);
                    //await GameMode.Delay(0);
                }

                if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
                    zcoord = outArgb.GetResult<float>();
            }

            return new Vector3(v.X, v.Y, zcoord);
        }
    }
}
