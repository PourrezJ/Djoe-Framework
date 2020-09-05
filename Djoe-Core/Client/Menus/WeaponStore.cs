using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Utils;
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
        private static List<uint> weaponsHashList;
        private static List<int> weaponsModelList;
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

            weaponsHashList = data["WeaponHash"].ToObject<List<uint>>();
            weaponsModelList = data["WeaponModel"].ToObject<List<int>>();
            spawnCoords = data["SpawnObject"].ToObject<UCoords>();

            Camera = World.CreateCamera(spawnCoords.ToVector3().Forward(0, 1f), new Vector3(0, 0, 0));

            foreach (var weap in weaponsModelList)
            {
                await Misc.LoadModel(weap);
            }

            Game.PlayerPed.Alpha = 0;

            Prop = World.CreateWeaponProp((WeaponHash)weaponsHashList[0], 100, spawnCoords.ToVector3(), 0.8f, true, true);
            //Prop.Rotation = new Vector3(0, 0, 180);
            
            Camera.PointAt(Prop.Position);
            Camera.IsActive = true;
            API.RenderScriptCams(true, true, 200, true, true, 0);
        }

        private static void OnIndexChangeCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, MenuAPI.MenuItem oldItem, MenuAPI.MenuItem newItem, int oldIndex, int newIndex)
        {
            if (menu.Id != "ID_WeaponShopMenu")
                return;

            lock(newItem)
            {
                if (Prop != null)
                {
                    lock (Prop)
                    {
                        Prop.Delete();
                        Prop = World.CreateWeaponProp((WeaponHash)weaponsHashList[newIndex], 100, spawnCoords.ToVector3(), 0.8f, true, true);

                    }
                }
            }

            //Prop.Rotation = new Vector3(0, 0, 180);
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
