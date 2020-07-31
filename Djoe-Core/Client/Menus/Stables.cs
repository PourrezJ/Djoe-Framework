using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Utils.Extensions;
using ClientExtended;
using ClientExtended.External;
using MenuAPI;
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
    public static class Stables
    {
        private static StableHorse Horses = new StableHorse();
        private static UCoords Spawn;
        private static Ped Ped;
        private static Camera Camera;

        private static MenuAPI.Menu Menu;

        public static void Init()
        {
            MenuManager.BeforeOpenMenuCallback += OnBeforeOpenMenu;
            MenuManager.OpenMenuCallback += OnOpenMenuCallback;
            MenuManager.ItemCallBack += OnItemCallBack;
            MenuManager.ListIndexChangeCallBack += OnListIndexChangeCallBack;

            GameMode.RegisterEventHandler("Stable_ListUpdate", new Action<string>(OnHorseListUpdate));
        }

        private static void OnHorseListUpdate(string obj)
        {
            Horses = JsonConvert.DeserializeObject<StableHorse>(obj);
        }

        private static void OnOpenMenuCallback(MenuAPI.Menu uimenu, Shared.MenuManager.Menu menu, string customData)
        {
            Debug.WriteLine(menu.Id);

            if (menu.Id != "ID_HorseDealer")
                return;

            Game.PlayerPed.IsPositionFrozen = true;

            Menu = uimenu;

            Menu.OnMenuClose += OnMenuClosed;
        }

        private static void OnMenuClosed(Menu menu)
        {
            if (Camera != null)
            {
                Camera.IsActive = false;
                Camera.Delete();
                Camera = null;
                API.RenderScriptCams(true, true, 200, true, true, 0);

                Ped?.Delete();
                Game.PlayerPed.IsPositionFrozen = false;

                Console.WriteLine("Stable menu closed");
            }
        }

        private static void OnBeforeOpenMenu(Shared.MenuManager.Menu menu, string customData)
        {
            Debug.WriteLine(menu.Id);
            if (menu.Id != "ID_HorseDealer")
                return;

            var data = JObject.Parse(customData);

            Horses = data["Horses"].ToObject<StableHorse>();
            Spawn = data["Spawn"].ToObject<UCoords>();

            PreviewHorseBuy(Horses.Variation.ElementAt(0).Key);
        }

        private static void OnListIndexChangeCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
        {
            if (menu.Id != "ID_HorseDealer")
                return;

            PreviewHorseBuy(Horses.Variation.ElementAt(newSelectionIndex).Key);
        }

        private static void OnItemCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, Shared.MenuManager.MenuItem menuItem, MenuAPI.MenuItem uiMenuItem, string input)
        {
            if (menu.Id != "ID_HorseDealer")
                return;
        }

        private static async void PreviewHorseBuy(string horseName)
        {
            Debug.WriteLine(horseName);

            if (Ped != null)
                Ped.Delete();

            var model = new Model(horseName);

            Ped = await World.CreatePed(model, Spawn.ToVector3(), Spawn.Heading, false, false);

            if (Ped == null)
                return;

            Ped.IsPositionFrozen = true;
            Ped.BlockPermanentEvents = true;
            Ped.SetNoCollision(Game.PlayerPed, true);
            Function.Call(Hash.TASK_ANIMAL_FLEE, Ped.Handle, false, false);
            

            if (Camera == null)
            {
                Camera = World.CreateCamera(Spawn.ToVector3().Forward(Ped.Heading + 25f, 3f) + new Vector3(0,0,2.5f), new Vector3(180,180, Ped.Heading));
                Camera.PointAt(Spawn.ToVector3() + new Vector3(0,0,1));
                Camera.IsActive = true;
                API.RenderScriptCams(true, true, 200, true, true, 0);
                Debug.WriteLine("Camera created");
            }
        }
    }
}
