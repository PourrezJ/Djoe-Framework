using CitizenFX.Core;
using Server.Utils.Extensions;
using Shared.MenuManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Menus
{
    public class PlayerMenu
    {
        #region Fields
        public Player Player;
        public Menu Menu;

        #endregion

        #region C4tor
        PlayerMenu(Player player)
        {
            Player = player;
            var data = player.GetPlayerDatabase();

            Menu = new Menu("ID_Personnal", data.identity.ToString(), "Menu Personnel");

            if (data.StaffRank >= Shared.StaffRank.Animateur)
                Menu.Add(new MenuItem("Administration", "", "Id_AdminMenu", true));

            Menu.ItemSelectCallback += OnItemSelect;

            Menu.BackCloseMenu = true;
            Menu.OpenMenu(player);
        }
        #endregion

        #region Events
        private void OnItemSelect(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            switch (menuItem.Id)
            {
                case "Id_AdminMenu":
                    AdminMenu.OpenMenu(client);
                    break;
            }
        }
        #endregion

        #region Methods

        public static void OpenMenu(Player player)
        {
            new PlayerMenu(player);
        }
        #endregion
    }
}
