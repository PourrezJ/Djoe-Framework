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

            API.RegisterCommand("displayComp", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Debug.WriteLine("Utilisation : hash comp");
                    return;
                }

                Function.Call((Hash)0x4CC5F2FC1332577F, (int)args[0]);
            }), false);

            API.RegisterCommand("displayComp2", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Debug.WriteLine("Utilisation : hash comp");
                    return;
                }

                Function.Call((Hash)0x4CC5F2FC1332577F, Game.GenerateHash((string)args[0]));
            }), false);

            API.RegisterCommand("hideComp", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Debug.WriteLine("Utilisation : hash comp");
                    return;
                }


                Function.Call((Hash)0x8BC7C1F929D07BF3, (int)args[0]);
            }), false);

            API.RegisterCommand("playScenario", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Debug.WriteLine("Utilisation : scenario name");
                    return;
                }

                Game.PlayerPed.Tasks.StartScenarioInPlace(args[0].ToString());
                
            }), false);

            API.RegisterCommand("stopTask", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Game.PlayerPed.Tasks.ClearAllImmediately();

            }), false);
        }
    }
}
