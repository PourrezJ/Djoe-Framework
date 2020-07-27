using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended.External;
using System;
using System.Threading.Tasks;
using ClientExtended;
using Shared;
using Newtonsoft.Json;
using Client.Ui;
using Newtonsoft.Json.Linq;
using Client.Menus;

namespace Client.Scripts
{
    public class SpawnPlayer : BaseScript
    {
        public static PlayerData PlayerData { get; private set; }
        public bool PlayerSpawned { get; private set; }

        public SpawnPlayer()
        {
            EventHandlers["djoe:initPlayer"] += new Action<string, string, uint>(InitPlayer);
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            UIHelper.LoadingScreenText("Les hors la loi", "Chargement...", "Initialisation.");
        }

        private async void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName)
                return;

            UIHelper.LoadingScreenText("Les hors la loi", "Chargement...", "Préparation du joueur.");

            await new Model("MP_Male").Request(100);
            await new Model("MP_Female").Request(100);

            await Game.Player.ChangeModel("MP_Male");

            UIHelper.LoadingScreenText("Les hors la loi", "Chargement...", "Demande d'information au serveur.");

            TriggerServerEvent("djoe:playerSpawn");
        }
        /*
        private async void InitTpPlayer(object spawnInfo)
        {
            if (firstSpawn)
            {
                Debug.WriteLine("VORP INIT_PLAYER");
                await Delay(4000);
                TriggerServerEvent("djoe:playerSpawn");
                firstSpawn = false;
            }
        }
        */
        private async void InitPlayer(string dataStr, string currentTime, uint weatherType)
        {
            UIHelper.LoadingScreenText("Les hors la loi", "Chargement...", "Information reçu par le serveur.");

            Debug.WriteLine(dataStr);

            PlayerData = JsonConvert.DeserializeObject<PlayerData>(dataStr);

            TriggerEvent("playerSpawned");

            var coords = PlayerData.LastCoord;

            if (coords == null)
                coords = new UCoords(0, 0, 80, 0);

            var pos = await Misc.ForceGroundZ(new Vector3(coords.X, coords.Y, coords.Z));

            Function.Call(Hash.SET_MINIMAP_HIDE_FOW, true);

            Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, pos.X, pos.Y, pos.Z, coords.Heading, true, true, false);

            UIHelper.LoadingScreenText("Les hors la loi", "Chargement...", $"Chargement du personnage {PlayerData.Identity.FirstName} {PlayerData.Identity.LastName}.");
            await LoadPlayer.LoadAllComps(PlayerData.SkinPlayer, PlayerData.Clothes);

            Game.PlayerPed.PositionNoOffset = new Vector3(pos.X, pos.Y, pos.Z);
            Game.PlayerPed.Heading = coords.Heading;
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
            postUi.Add("moneyvalue", PlayerData.Money);
            postUi.Add("thirstvalue", PlayerData.Thirst);
            postUi.Add("hungervalue", PlayerData.Hunger);

            Hud.UpdateUI(postUi.ToString());

            API.ShutdownLoadingScreen();
            API.DoScreenFadeIn(500);

            while (API.GetIsLoadingScreenActive())
            {
                await Delay(0);
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
            Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups("", (Relationship)6, true);
        }

        private static DateTime _lastupdate = DateTime.Now;
        private Task PlayerUpdate()
        {
            if ((DateTime.Now - _lastupdate).TotalMilliseconds < 150)
                return Task.FromResult(0);

            _lastupdate = DateTime.Now;

            if (PlayerSpawned)
            {
                var pPos = Game.PlayerPed.Position;
                var health = Game.PlayerPed.Health;
                var heading = Game.PlayerPed.Heading;

                if (PlayerData.LastCoord.DistanceTo2D(pPos) > 1.5f || PlayerData.Health != health)
                {
                    TriggerServerEvent("djoe:update", pPos, heading, health);

                    PlayerData.LastCoord.SetUcoord(pPos, heading);
                    PlayerData.Health = health;
                }
            }

            return Task.FromResult(0);
        }

        [Tick]
        public Task DisableHud()
        {
            Game.DisableControlThisFrame(0, ClientExtended.External.Control.HudSpecial);
            Game.DisableControlThisFrame(0, ClientExtended.External.Control.RevealHud);

            return Task.FromResult(0);
        }

        [Tick]
        private Task OnTick()
        {








            /* What is this shit?
            int pped = API.PlayerPedId();
            uint playerHash = (uint)Util.GetHashKey("PLAYER");

            if (API.IsControlPressed(0, (uint)0xCEFD9220))
            {
                Function.Call((Hash)0xBF25EB89375A37AD, 1, playerHash, playerHash);
                active = true;
                await Delay(4000);
            }
            if (!API.IsPedOnMount(pped) && !API.IsPedInAnyVehicle(pped, false) && active == true)
            {
                Function.Call((Hash)0xBF25EB89375A37AD, 5, playerHash, playerHash);
                active = false;

            }
            else if (active == true && (API.IsPedOnMount(pped) || API.IsPedInAnyVehicle(pped, false)))
            {
                if (API.IsPedInAnyVehicle(pped, false))
                {

                }
                else if (API.GetPedInVehicleSeat(API.GetMount(pped), -1) == pped)
                {
                    Function.Call((Hash)0xBF25EB89375A37AD, 5, playerHash, playerHash);
                    active = false;
                }
            }*/

            return Task.FromResult(0);
        }
    }
}
