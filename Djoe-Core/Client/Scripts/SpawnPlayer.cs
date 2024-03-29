﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.External;
using System;
using System.Threading.Tasks;
using ClientExtented;
using Shared;
using Newtonsoft.Json;
using Client.Ui;
using Newtonsoft.Json.Linq;
using Client.Menus;
using Client.Models;
using Client.Controllers;
using System.Collections.Generic;

namespace Client.Scripts
{
    public class SpawnPlayer : BaseScript
    {
        public static PlayerData PlayerData { get; private set; }
        public bool PlayerSpawned { get; private set; }

        public SpawnPlayer()
        {
            EventHandlers["djoe:initPlayer"] += new Action<string, string, uint, string>(InitPlayer);
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            Tick += HardCapTick;

            UIHelper.LoadingScreenText("RDRP", "Chargement...", "Initialisation.");
        }

        private async void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName)
                return;

            UIHelper.LoadingScreenText("RDRP", "Chargement...", "Préparation du joueur.");

            await new Model("MP_Male").Request(100);
            await new Model("MP_Female").Request(100);

            await Game.Player.ChangeModel("MP_Male");

            UIHelper.LoadingScreenText("RDRP", "Chargement...", "En attente de l'hébergeur.");    
        }

        private async void InitPlayer(string dataStr, string currentTime, uint weatherType, string weaponStr)
        {
            UIHelper.LoadingScreenText("RDRP", "Chargement...", "Information reçu par le serveur.");

            PlayerData = JsonConvert.DeserializeObject<PlayerData>(dataStr);

            var weapons = JsonConvert.DeserializeObject<List<WeaponItem>>(weaponStr);


            TriggerEvent("playerSpawned");

            var coords = PlayerData.LastCoord;

            if (coords == null)
                coords = new UCoords(0, 0, 80, 0);

            var pos = await Misc.ForceGroundZ(new Vector3(coords.X, coords.Y, coords.Z));

            Function.Call(Hash.SET_MINIMAP_HIDE_FOW, true);

            Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, pos.X, pos.Y, pos.Z, coords.Heading, true, true, false);

            UIHelper.LoadingScreenText("RDRP", "Chargement...", $"Chargement du personnage {PlayerData.Identity.FirstName} {PlayerData.Identity.LastName}.");
            await LoadPlayer.LoadAllComps(PlayerData.SkinPlayer, PlayerData.Clothes);

            Game.PlayerPed.SetEntityCoordsAndHeading(pos, coords.Heading);

            Game.PlayerPed.IsPositionFrozen = false;

            var dateTime = DateTime.Parse(currentTime);

            Function.Call(Hash._NETWORK_CLOCK_TIME_OVERRIDE, dateTime.Hour, dateTime.Minute, dateTime.Second);

            World.CurrentWeather = (WeatherType)weatherType;

            // https://discordapp.com/channels/192358910387159041/643437867044962304/730164095331991562
            Game.PlayerPed.SetConfigFlag(263, true); // HEADSHOT_IMMUNITY
            Game.PlayerPed.SetConfigFlag(547, true); // CANNOT_LOCK_ON_PLAYERS

            SetPVP();

            if (PlayerData.IsDead)
            {
                TriggerServerEvent("djoe:PlayerForceRespawn");
                TriggerEvent("djoe:PlayerForceRespawn");
                RespawnSystem.RespawnPlayer();
            }

            Admin.Init(PlayerData.StaffRank);

            JObject postUi = new JObject();
            postUi.Add("type", "ui");
            postUi.Add("action", "update");
            postUi.Add("moneyvalue", Math.Round(PlayerData.Money, 2));
            postUi.Add("thirstvalue", PlayerData.Thirst);
            postUi.Add("hungervalue", PlayerData.Hunger);

            Hud.UpdateUI(postUi.ToString());

            API.ShutdownLoadingScreen();
            API.DoScreenFadeIn(500);

            while (API.GetIsLoadingScreenActive())
            {
                await Delay(0);
            }

            API.RemoveAllPedWeapons(Game.PlayerPed.Handle, true, true);

            foreach (var weapon in weapons)
            {
                Game.PlayerPed.GiveWeapon((uint)Game.GenerateHash(weapon.HashName), weapon.CurrentAmmo, false);
            }

            Hud.ShowUI(true);
            API.DisplayRadar(true);
            API.DisplayHud(true);

            PlayerSpawned = true;

            Tick += PlayerUpdate;
        }

        public static void SetPVP()
        {
            Function.Call(Hash.NETWORK_SET_FRIENDLY_FIRE_OPTION, true);
            Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups("Player", (Relationship)6, true);
        }

        private async Task PlayerUpdate()
        {
            var pPos = Game.PlayerPed.Position;
            var health = Game.PlayerPed.Health;
            var heading = Game.PlayerPed.Heading;

            if (PlayerData.LastCoord.DistanceTo2D(pPos) > 1.5f || PlayerData.Health != health)
            {
                TriggerServerEvent("djoe:playerupdate", pPos, heading, health);

                PlayerData.LastCoord.SetUcoord(pPos, heading);
                PlayerData.Health = health;

                if (Game.PlayerPed.IsOnMount)
                {
                    lock (PedsManager.PedList)
                    {
                        if (PedsManager.PedList.ContainsKey(Game.PlayerPed.GetMount))
                        {
                            var horse = Game.PlayerPed.GetMount;
                            var hData = PedsManager.PedList[horse];

                            var hPos = horse.Position;
                            var hHeading = horse.Heading;
                            var hHealth = horse.Health;

                            if (hData.LastCoord.DistanceTo2D(hPos) > 1.5f || hData.Health != hHealth)
                            {
                                hData.Health = hHealth;
                                hData.LastCoord.SetUcoord(hPos, hHeading);
                                TriggerServerEvent("djoe:updatehorse", hData.NetworkID, hPos, hHeading, hHealth);
                            }
                        }
                    }
                }
                await Delay(150);
            }
        }

        [Tick]
        public Task DisableHud()
        {
            Game.DisableControlThisFrame(0, ClientExtented.External.Control.HudSpecial);
            Game.DisableControlThisFrame(0, ClientExtented.External.Control.RevealHud);
            Game.DisableControlThisFrame(0, ClientExtented.External.Control.TwirlPistol);
            Game.DisableControlThisFrame(0, ClientExtented.External.Control.ToggleHolster);

            return Task.FromResult(0);
        }

        private Task HardCapTick()
        {
            //Debug.WriteLine($"Started {API.NetworkIsSessionStarted()} Active {API.NetworkIsSessionActive()}");
            //Debug.WriteLine($"Host {API.NetworkIsHost()} Active {API.NetworkIsPlayerConnected(Game.Player.ServerId)}");

            if (API.NetworkIsSessionActive())
            {
                Tick -= HardCapTick;

                Debug.WriteLine("====== Network Started =======");
                UIHelper.LoadingScreenText("RDRP", "Chargement...", "Demande d'information au serveur.");

                TriggerServerEvent("djoe:playerSpawn");
            }
            return Task.FromResult(0);
        }
    }
}
