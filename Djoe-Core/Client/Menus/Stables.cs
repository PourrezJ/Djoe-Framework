using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended;
using ClientExtended.External;
using MenuAPI;
using Newtonsoft.Json;
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

        public static void Init()
        {
            MenuManager.BeforeOpenMenuCallback += OnOpenMenuCallback;
            MenuManager.ItemCallBack += OnItemCallBack;
            MenuManager.ListIndexChangeCallBack += OnListIndexChangeCallBack;



            GameMode.RegisterEventHandler("Stable_ListUpdate", new Action<string>(OnHorseListUpdate));

            Debug.WriteLine("Stable menu init");
        }

        private static void OnHorseListUpdate(string obj)
        {
            Horses = JsonConvert.DeserializeObject<StableHorse>(obj);
        }

        private static void OnOpenMenuCallback(Shared.MenuManager.Menu menu, string customData)
        {
            if (menu.Id != "ID_HorseDealer")
                return;

            Horses = JsonConvert.DeserializeObject<StableHorse>(customData);

            PreviewHorseBuy(Horses.Variation.ElementAt(0).Key);
        }

        private static void OnListIndexChangeCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
        {
            PreviewHorseBuy(Horses.Variation.ElementAt(newSelectionIndex).Key);
        }

        private static void OnItemCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, Shared.MenuManager.MenuItem menuItem, MenuAPI.MenuItem uiMenuItem, string input)
        {
            
        }

        private static void PreviewHorseBuy(string horseName)
        {
            Debug.WriteLine(horseName);


        }
    }
}
