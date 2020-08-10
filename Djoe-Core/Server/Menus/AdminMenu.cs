using CitizenFX.Core;
using CitizenFX.Core.Native;
using Server.Entities;
using Server.Utils;
using Server.Utils.Enums;
using Server.Utils.Extensions;
using Shared;
using Shared.MenuManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Menus
{
    public class AdminMenu
    {
        #region Fields
        public Player Player;
        public Menu Menu;
        public Player PlayerSelected;

        #endregion

        #region C4tor
        AdminMenu(Player player)
        {
            Player = player;
            PlayerSelected = player;

            Menu = new Menu("ID_Admin", "Admin Menu");

            if (player.GetPlayerDatabase().StaffRank == Shared.StaffRank.Animateur)
            {
                Menu.Add(new MenuItem("Skin", ""));
            }
            else
            {
                List<string> players = new List<string>();

                var onlinePlayer = PlayerManager.GetPlayersOnline();
                for (int a = 0; a < onlinePlayer.Count; a++)
                    players.Add(onlinePlayer[a].Identity.ToString());

                Menu.Add(new ListItem("Choix Joueur", "Sélection du joueur destinataire", "Id_ChoisePlayer", players, 0, true));

                var kickItem = new MenuItem("Kick", "", "Id_Kick");
                kickItem.SetInput("Raison", 99, InputType.Text);
                Menu.Add(kickItem);

                var banItem = new MenuItem("Ban", "", "Id_Ban");
                banItem.SetInput("Raison", 99, InputType.Text);
                Menu.Add(banItem);

                var horseItem = new MenuItem("Spawn Chevaux", "", "Id_SpawnHorse");
                horseItem.SetInput("", 99, InputType.Text);
                Menu.Add(horseItem);

                var vehicleItem = new MenuItem("Spawn Vehicule", "", "Id_SpawnVehicle");
                vehicleItem.SetInput("", 99, InputType.Text);
                Menu.Add(vehicleItem);

                var weaponItem = new MenuItem("Give Weapon", "", "Id_Weapon");
                weaponItem.SetInput("", 99, InputType.Text);
                Menu.Add(weaponItem);

                Menu.Add(new CheckboxItem("Invisible", "", "Id_Invisible", PlayerSelected.GetData("Invisible", out bool result) ? result : false));
                Menu.Add(new CheckboxItem("GodMode", "", "Id_GodMode", PlayerSelected.GetData("Invincible", out bool result1) ? result1 : false));
                Menu.Add(new CheckboxItem("NoClip", "", "Id_NoClip", PlayerSelected.GetData("NoClip", out bool result2) ? result2 : false));
                Menu.Add(new MenuItem("TP to Waypoint", "", "Id_Waypoint"));
                Menu.Add(new MenuItem("TP to Player", "", "Id_TpToPlayer"));
                Menu.Add(new MenuItem("TP Player to Me", "", "Id_TpPlayerToMe"));
                /*
                var tpToItem = new MenuItem("Tp To", "", "Id_TpTo");
                tpToItem.SetInput("", 99, InputType.Text);
                Menu.Add(tpToItem);*/
            }

            Menu.ItemSelectCallback += OnItemSelect;

            Menu.OpenMenu(player);
        }
        #endregion

        #region Events
        private void OnItemSelect(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            PlayerData playerData = PlayerSelected.GetPlayerDatabase();
            bool check = false;

            if (menuItem is CheckboxItem)
            {
                check = (menuItem as CheckboxItem).Checked;
            }

            switch(menuItem.Id)
            {
                case "Id_ChoisePlayer":
                    Debug.WriteLine("Id_ChoisePlayer");
                    break;

                case "Id_Kick":
                    client.Drop(menuItem.InputValue);
                    break;

                case "Id_Ban":
                    API.TempBanPlayer(client.Handle, menuItem.InputValue);
                    GameMode.WorldData.BanList.Add(
                        new Models.BanData()
                        {
                            BanTime = DateTime.Now,
                            EndBanTime = DateTime.MaxValue,
                            SteamID = client.Identifiers["steam"],
                            Reason = menuItem.InputValue
                        });
                    break;

                case "Id_SpawnHorse":
                    PedsManager.CreatePed(PlayerSelected, (PedHash)Utils.Misc.GetHashKey(menuItem.InputValue), playerData.LastCoord, false, PedType.Horse);
                    break;

                case "Id_SpawnVehicle":
                    NetworkAPI.CreateVehicle(PlayerSelected, menuItem.InputValue, playerData.LastCoord.ToVector3(), playerData.LastCoord.Heading, true);
                    break;

                case "Id_Invisible":
                    NetworkAPI.SetAlpha(PlayerSelected, check ? 0 : 255);
                    PlayerSelected.SetData("Invisible", check);
                    break;

                case "Id_GodMode":
                    NetworkAPI.SetInvincible(PlayerSelected, check);
                    PlayerSelected.SetData("Invincible", check);
                    break;

                case "Id_Weapon":
                    NetworkAPI.GiveWeapon(PlayerSelected, menuItem.InputValue, 200, true, 0, false, 0);
                    break;

                case "Id_NoClip":
                    NetworkAPI.SetNoClip(PlayerSelected, check);
                    PlayerSelected.SetData("NoClip", check);
                    break;

                case "Id_Waypoint":
                    NetworkAPI.SetToWaypoint(PlayerSelected);
                    break;

                case "Id_TpToPlayer":
                    NetworkAPI.SetPlayerPos(client, playerData.LastCoord);
                    break;

                case "Id_TpPlayerToMe":
                    NetworkAPI.SetPlayerPos(PlayerSelected, client.GetPlayerDatabase().LastCoord);
                    break;

                case "Id_TpTo":

                    try
                    {
                        var str = menuItem.InputValue;

                        str.Replace('f', new char());
                        var posStr = str.Split(',');

                        float x = float.Parse(posStr[0]);
                        float y = float.Parse(posStr[1]);
                        float z = float.Parse(posStr[2]);

                        NetworkAPI.SetPlayerPos(PlayerSelected, new UCoords(x, y, z, 0));
                    }
                    catch(Exception ex)
                    {
                        Logger.Exception(ex);
                    }

                    break;
            }
        }
        #endregion

        #region Methods

        public static Menu OpenMenu(Player player)
        {
            return new AdminMenu(player).Menu;
        }
        #endregion
    }
}
