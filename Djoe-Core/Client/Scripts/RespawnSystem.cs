using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using ClientExtented.External;
using System;
using System.Threading.Tasks;
using Control = ClientExtented.External.Control;

namespace Client.Scripts
{
    //Respawn System similar to GTA V Death Screen https://imgur.com/a/YnEz9Yd | https://gyazo.com/24cfd684129ee9771f67b3470d351021
    class RespawnSystem : BaseScript
    {
        //Hum this wheel runs a lot, they are made of aluminium
        public RespawnSystem()
        {
            EventHandlers["djoe:resurrectPlayer"] += new Action(ResurectPlayer);
        }

        static bool setDead;
        static int TimeToRespawn = 1;

        [Tick]
        public async Task OnPlayerDead()
        {
            if (Game.PlayerPed.IsDead)
            {
                if (!setDead)
                {
                    TriggerServerEvent("djoe:ImDead", true);
                    setDead = true;
                }
                API.NetworkSetInSpectatorMode(true, API.PlayerPedId());
                API.AnimpostfxPlay("DeathFailMP01");
                API.DisplayHud(true);
                API.DisplayRadar(true);
                TimeToRespawn = 30;

                while (TimeToRespawn >= 0 && setDead)
                {
                    await Delay(1000);
                    TimeToRespawn -= 1;
                    //Exports["spawnmanager"].setAutoSpawn(false); //Is this a copy ? wtf I need to create a new spawnmanager? 
                }

                uint KeyInt = 0xDFF812F9;
                bool pressKey = false; //sorry the word pressed has copyright
                while (!pressKey && setDead)
                {
                    await Delay(0);
                    if (!Game.PlayerPed.IsAttachedToAnyPed)
                    {
                        API.NetworkSetInSpectatorMode(true, API.PlayerPedId());
                        Utils.Misc.DrawText("Appuyez ~e~E~q~ pour réapparaître", 1, 0.50f, 0.50f, 1.0f, 1.0f, 255, 255, 255, 255, true, true);
                        if (Game.IsControlJustPressed(0, (Control)KeyInt))
                        {
                            API.DoScreenFadeOut(1000);
                            await Delay(1000);
                            //TriggerServerEvent("djoe:getPlayerSkin");

                            TriggerServerEvent("djoe:PlayerForceRespawn");
                            RespawnPlayer();
                            pressKey = true;
                            await Delay(1000);
                        }
                    }
                }
            }
        }

        [Tick]
        public static Task InfoOnDead()
        {
            if (Game.PlayerPed.IsAttachedToAnyPed && setDead)
            {
                int carrier = Function.Call<int>((Hash)0x09B83E68DE004CD4, Game.PlayerPed.Handle);
                API.NetworkSetInSpectatorMode(true, carrier);
                Utils.Misc.DrawText("Vous êtes porté par une personne", 4, 0.50f, 0.30f, 1.0f, 1.0f, 255, 255, 255, 255, true, true);
            }
            else if (TimeToRespawn >= 0 && setDead)
            {
                Utils.Misc.DrawText("Vous êtes morts!", 1, 0.50F, 0.50F, 1.2F, 1.2F, 171, 3, 0, 255, true, true);
                Utils.Misc.DrawText(string.Format("Vous pouvez réapparaître dans {0} secondes", TimeToRespawn.ToString()), 0, 0.50f, 0.60f, 0.5f, 0.5f, 255, 255, 255, 255, true, true);
            }
            return Task.FromResult(0);
        }

        public static void RespawnPlayer()
        {
            Game.PlayerPed.ResurrectPed();
            API.AnimpostfxStop("DeathFailMP01");
            Game.PlayerPed.SetEntityCoordsAndHeading(new Vector3(-353.08f, 752.11f, 116.0f), 321.76f);

            ClientExtented.Util.Delay(100, () =>
            {
                API.DoScreenFadeIn(1000);
                TriggerServerEvent("djoe:ImDead", false);
                setDead = false;
                API.NetworkSetInSpectatorMode(false, API.PlayerPedId());
                API.DisplayHud(true);
                API.DisplayRadar(true);
                SpawnPlayer.SetPVP();
            });
        }

        public void ResurectPlayer()
        {
            Game.PlayerPed.ResurrectPed();
            API.AnimpostfxStop("DeathFailMP01");
            API.DoScreenFadeIn(1000);
            TriggerServerEvent("djoe:ImDead", false);
            setDead = false;

            ClientExtented.Util.Delay(100, () =>
            {
                API.NetworkSetInSpectatorMode(false, API.PlayerPedId());
                API.DisplayHud(true);
                API.DisplayRadar(true);
                SpawnPlayer.SetPVP();
            });
        }
    }
}
