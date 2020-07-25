using CitizenFX.Core;
using MenuAPI;
using System;
using ClientExtented;

namespace Client.Menus.CharCreator
{
    class IdentityMenu
    {
        private static Menu identityMenu = new Menu("Identité", "Identité de votre personnage");
        private static bool setupDone = false;
        public static void SetupMenu()
        {
            if (setupDone)
                return;

            setupDone = true;
            MenuController.AddMenu(identityMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            identityMenu.OnItemSelect += Menu_OnItemSelect;

            identityMenu.AddMenuItem(new MenuItem("Nom:"));
            identityMenu.AddMenuItem(new MenuItem("Prénom:"));
            identityMenu.AddMenuItem(new MenuItem("Âge:"));
            identityMenu.AddMenuItem(new MenuItem("Nationalité:"));
        }

        private static async void Menu_OnItemSelect(Menu menu, MenuItem menuItem, int itemIndex)
        {
            if (CreatePlayer.Identity == null)
                CreatePlayer.Identity = new Shared.Identite();

            switch (menuItem.Text)
            {
                case "Nom:":
                    CreatePlayer.Identity.LastName = await Inputbox.GetUserInput(CreatePlayer.Identity.LastName);
                    menuItem.Label = CreatePlayer.Identity.LastName;
                    break;

                case "Prénom:":
                    CreatePlayer.Identity.FirstName = await Inputbox.GetUserInput(CreatePlayer.Identity.FirstName);
                    menuItem.Label = CreatePlayer.Identity.FirstName;
                    break;

                case "Âge:":
                    CreatePlayer.Identity.Age = Convert.ToInt32(await Inputbox.GetUserInput(CreatePlayer.Identity.Age.ToString()));
                    menuItem.Label = CreatePlayer.Identity.Age.ToString();
                    break;

                case "Nationalité:":
                    CreatePlayer.Identity.Nationality = await Inputbox.GetUserInput(CreatePlayer.Identity.Nationality);
                    menuItem.Label = CreatePlayer.Identity.Nationality;
                    break;
            }
        }

        public static Menu GetMenu()
        {
            return identityMenu;
        }
    }
}
