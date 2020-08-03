using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Menus
{
    public class WeaponStore
    {
        public static void Init()
        {
            MenuManager.OpenMenuCallback += OnOpenMenuCallback;
        }

        private static void OnOpenMenuCallback(MenuAPI.Menu uimenu, Shared.MenuManager.Menu menu, string customData)
        {
            if (menu.Id != "ID_WeaponShopMenu")
                return;


        }
    }
}
