using CitizenFX.Core;
using Server.Entities;
using Server.Utils.Extensions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Menus
{
    public class HorseInteractionMenu
    {
        public HorseInteractionMenu(Player player, PedNetwork horse)
        {
            var hd = HorseManager.GetWithPedNetwork(horse);

            Console.WriteLine((hd == null) ? "hd null" : "hd not null");

            if (hd == null)
                return;

            var radialMenu = new RadialMenu("ID_HorseMenu");
            radialMenu.SetData("HorseData", hd);

            radialMenu.Add(new RadialMenuItem("Inventaire", "Ouvrir l'inventaire du cheval", "ID_HorseInventory", RadialMenuItemIcons.FOLDER_OPEN_SOLID));

            radialMenu.Callback += OnCallBack;

            radialMenu.OpenRadialMenu(player);
        }

        private void OnCallBack(Player client, RadialMenu menu, RadialMenuItem menuItem, int itemIndex, dynamic data)
        {
            HorseData horseData = menu.GetData("HorseData");
            PlayerData playerData = client.GetPlayerDatabase();

            switch (menuItem.Id)
            {
                case "ID_HorseInventory":
                    RadialManager.CloseMenu(client);
                    new RPGInventoryMenu(playerData.PocketInventory, null, null, horseData.Inventory).OpenMenu(client);
                    break;
            }
        }

        public static void OpenMenu(Player player, PedNetwork horse)
        {
            new HorseInteractionMenu(player, horse);
        }
    }
}
