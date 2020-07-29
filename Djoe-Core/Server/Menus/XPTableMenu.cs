using CitizenFX.Core;
using Server.Utils.Extensions;
using Shared.MenuManager;
using System;

namespace Server.Menus
{
    public class XPTableMenu
    {
        public Menu Menu { get; private set; }

        public XPTableMenu(Player player)
        {
            var playerData = player.GetPlayerDatabase();

            if (playerData == null)
                return;

            var xpTable = playerData.XPTable;

            Menu = new Menu("ID_XpMenu", "Table XP");

            Menu.Add(new MenuItem($"{xpTable.Agriculture.Name} : {Math.Round(xpTable.Agriculture.CurrentXP)}"));
            Menu.Add(new MenuItem($"{xpTable.Artisanat.Name} : {Math.Round(xpTable.Artisanat.CurrentXP)}"));
            Menu.Add(new MenuItem($"{xpTable.Lumberman.Name} : {Math.Round(xpTable.Lumberman.CurrentXP)}"));
            Menu.Add(new MenuItem($"{xpTable.Mineur.Name} : {Math.Round(xpTable.Mineur.CurrentXP)}"));

            Menu.OpenMenu(player);
        }

        public static Menu OpenMenu(Player player)
        {
            return new XPTableMenu(player).Menu;
        }
    }
}
