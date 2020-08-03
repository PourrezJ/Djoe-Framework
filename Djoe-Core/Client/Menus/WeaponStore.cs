using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.Extensions;
using ClientExtented.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Menus
{
    public class WeaponStore
    {
        private static List<string> weaponsList = new List<string>();
        private static UCoords spawnCoords;
        private static Prop Prop;
        private static Camera Camera;

        public static void Init()
        {
            MenuManager.OpenMenuCallback += OnOpenMenuCallback;
            MenuManager.IndexChangeCallBack += OnIndexChangeCallBack;
            MenuManager.MenuClosedCallBack += OnMenuClosedCallBack;
        }

        private static async void OnOpenMenuCallback(MenuAPI.Menu uimenu, Shared.MenuManager.Menu menu, string customData)
        {
            if (menu.Id != "ID_WeaponShopMenu")
                return;

            Game.PlayerPed.IsPositionFrozen = true;

            var data = JObject.Parse(customData);

            weaponsList = data["WeaponModel"].ToObject<List<string>>();
            spawnCoords = data["SpawnObject"].ToObject<UCoords>();

            Game.PlayerPed.Alpha = 0;

            Prop = await World.CreateProp(new Model(weaponsList[0]), spawnCoords.ToVector3(), new Vector3(), true, false, false);
            Camera = World.CreateCamera(Prop.Position.Forward(0, 1f), new Vector3(0,0,0));

            Camera.PointAt(Prop.Position);
            Camera.IsActive = true;
            API.RenderScriptCams(true, true, 200, true, true, 0);
        }

        private static async void OnIndexChangeCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, MenuAPI.MenuItem oldItem, MenuAPI.MenuItem newItem, int oldIndex, int newIndex)
        {
            if (menu.Id != "ID_WeaponShopMenu")
                return;

            Prop?.Delete();
            Prop = await World.CreateProp(new Model(weaponsList[newIndex]), spawnCoords.ToVector3(), new Vector3(0, 180, spawnCoords.Heading), true, true, false);
        }

        private static void OnMenuClosedCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu)
        {
            if (menu.Id != "ID_WeaponShopMenu")
                return;

            if (Camera != null)
            {
                Camera.IsActive = false;
                Camera.Delete();
                Camera = null;
            }

            API.RenderScriptCams(true, true, 200, true, true, 0);
            Game.PlayerPed.Alpha = 255;
            Prop?.Delete();

            Game.PlayerPed.IsPositionFrozen = false;
        }
    }
}
