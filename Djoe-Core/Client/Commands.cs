using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended.External;
using System;
using System.Collections.Generic;

namespace Client
{
    internal static class Commands
    {
        internal static void Init()
        {
            API.RegisterCommand("coords", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Debug.WriteLine("Utilisation : coords [nom]");
                    return;
                }

                string coordName = Convert.ToString(args[0]);

                Vector3 playerPosGet = Game.Player.Character.Position;
                float playerRotGet = Game.Player.Character.Heading;

                string pPosX = (playerPosGet.X.ToString().Replace(',', '.'));
                string pPosY = (playerPosGet.Y.ToString().Replace(',', '.'));
                string pPosZ = (playerPosGet.Z.ToString().Replace(',', '.'));

                BaseScript.TriggerServerEvent("RegisterCoords", coordName, pPosX, pPosY, pPosZ, playerRotGet.ToString());
                Debug.WriteLine("Your position is: ~y~" + playerPosGet + "~w~Your rotation is: ~y~" + playerRotGet);
            }), false);
        }
    }
}
