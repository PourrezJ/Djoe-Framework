﻿using CitizenFX.Core;
using Server.ItemsClass;
using Server.Utils;
using Server.Utils.Enums;
using Server.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Menus
{
    public class WeaponWheelMenu
    {
        public RadialMenu RadialMenu;

        public WeaponWheelMenu(Player player)
        {
            return;
            // useless bad idea

            var pData = player.GetPlayerDatabase();

            if (pData == null)
                return;

            RadialMenu radialMenu = new RadialMenu("WeaponWheel");

            radialMenu.Add(new RadialMenuItem("Mains nue", "", "ID_RemoveWeapon", RadialMenuItemIcons.PAN_TOOL));

            foreach (var stack in pData.PocketInventory.InventoryList)
            {
                if (stack == null)
                    continue;

                if (stack.Item == null)
                    continue;

                try
                {
                    var item = stack.Item;

                    if (item is WeaponItem)
                    {             
                        var menuItem = new RadialMenuItem(item.Name, item.Description, "ID_WeaponSelect", RadialMenuItemIcons.PAN_TOOL);
                        menuItem.SetData("Weapon", item);
                        radialMenu.Add(menuItem);
                    }
                }
                catch(Exception ex)
                {
                    Logger.Exception(ex, "WeaponWheelMenu");
                }
            }

            radialMenu.Callback += OnItemCallback;

            radialMenu.OpenRadialMenu(player);
        }

        private void OnItemCallback(Player client, RadialMenu menu, RadialMenuItem menuItem, int itemIndex, dynamic data)
        {
            if (menuItem.Id == "ID_RemoveWeapon")
            {
                NetworkAPI.RemoveAllWeapons(client);
            }
            else if (menuItem.Id == "ID_WeaponSelect")
            {
                WeaponItem weapon = menuItem.GetData("Weapon");

                if (weapon == null)
                    return;

                bool lefthanded;

                WeaponHash weaponHash = (WeaponHash)Misc.GetHashKey(weapon.WeaponModel);

                switch (weaponHash)
                {
                    case WeaponHash.Lasso:
                        lefthanded = true;
                        break;
                    default:
                        lefthanded = false;
                        break;
                }


                NetworkAPI.GiveWeapon(client, (uint)Misc.GetHashKey(weapon.WeaponModel), weapon.CurrentAmmo, true, 0, lefthanded, 0);
            }
        }

        public static WeaponWheelMenu OpenMenu(Player player)
        {
            return new WeaponWheelMenu(player);
        }
    }
}
