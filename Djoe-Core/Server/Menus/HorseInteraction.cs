using CitizenFX.Core;
using Server.Entities;
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
            var radialMenu = new RadialMenu("ID_HorseMenu");
            radialMenu.Add(new RadialMenuItem("Inventaire", "Ouvrir l'inventaire du cheval", "ID_HorseInventory", RadialMenuItemIcons.FOLDER_OPEN_SOLID));

            radialMenu.OpenRadialMenu(player);
        }

        public static void OpenMenu(Player player, PedNetwork horse)
        {
            new HorseInteractionMenu(player, horse);
        }
    }
}
