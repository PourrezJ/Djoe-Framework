using CitizenFX.Core;
using MenuAPI;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

namespace Client.Menus
{
    public class GeneralStore
    {
        private static double[] itemsPrice;

        public static void Init()
        {
            MenuManager.OpenMenuCallback += OnOpenMenuCallback;
            MenuManager.ItemCallBack += OnItemCallBack;
            MenuManager.ListIndexChangeCallBack += OnListIndexChangeCallBack;
        }

        private static void OnListIndexChangeCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
        {
            if (menu.Id != "ID_GeneralStoreMenu")
                return;

            var item = menu.Items[itemIndex];
            if (item.Text.Contains("$"))
            {
                var a = item.Text.Split('$');

                string itemName = a[0];

                var str = itemName + $" ${(Math.Round(itemsPrice[itemIndex] * newSelectionIndex, 2))}/${itemsPrice[itemIndex]}";
                listItem.Text = str;
            }
        }

        private static void OnItemCallBack(Shared.MenuManager.Menu menu, MenuAPI.Menu uimenu, Shared.MenuManager.MenuItem menuItem, MenuAPI.MenuItem uiMenuItem, string input)
        {
            if (menu.Id != "ID_GeneralStoreMenu")
                return;

            Debug.WriteLine("OnItemCallBack");
        }

        private static void OnOpenMenuCallback(MenuAPI.Menu uimenu, Shared.MenuManager.Menu menu, string customData)
        {
            if (menu.Id != "ID_GeneralStoreMenu")
                return;

            itemsPrice = JsonConvert.DeserializeObject<double[]>(customData);
        }
    }
}
