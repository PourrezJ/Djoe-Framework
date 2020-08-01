using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using ClientExtented;
using ClientExtented;
using Client.Menus.CharCreator;
using Client.Scripts;
using Client.Ui;
using Client.Utils;

namespace Client.Menus
{
    class MainMenuCreator
    {
        private static Menu mainMenu = new Menu("Éditeur", "Éditez votre personnage");
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            IdentityMenu.SetupMenu();
            SkinMenu.SetupMenu();
            FaceMenu.SetupMenu();
            ClothesMenu.SetupMenu();

            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            //SkinMenu
            MenuController.AddSubmenu(mainMenu, SkinMenu.GetMenu());

            MenuItem subMenuSkinBtn = new MenuItem("Personnaliser la peau", "Personnalisez le corps de votre personnage")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuSkinBtn);
            MenuController.BindMenuItem(mainMenu, SkinMenu.GetMenu(), subMenuSkinBtn);

            //FaceMenu
            MenuController.AddSubmenu(mainMenu, FaceMenu.GetMenu());

            MenuItem subMenuFaceBtn = new MenuItem("Personnaliser le visage", "Personnalisez le visage de votre personnage")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuFaceBtn);
            MenuController.BindMenuItem(mainMenu, FaceMenu.GetMenu(), subMenuFaceBtn);

            //ClothesMenu
            MenuController.AddSubmenu(mainMenu, ClothesMenu.GetMenu());

            MenuItem subMenuClothesBtn = new MenuItem("Vestiaire", "Habillez votre personnage")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuClothesBtn);
            MenuController.BindMenuItem(mainMenu, ClothesMenu.GetMenu(), subMenuClothesBtn);

            //Identity
            MenuController.AddSubmenu(mainMenu, IdentityMenu.GetMenu());

            MenuItem subMenuIdentityBtn = new MenuItem("Identité", "Choisissez une identité pour votre personnage")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuIdentityBtn);
            MenuController.BindMenuItem(mainMenu, IdentityMenu.GetMenu(), subMenuIdentityBtn);

            List<string> scaleValues = new List<string>();

            foreach(float sc in Utils.SkinsUtils.SCALE_LIST)
            {
                scaleValues.Add("Hauteur #" + sc.ToString());
            }

            MenuListItem ScaleBtn = new MenuListItem("Taille du personnage", scaleValues, 4, "Ajustez la hauteur (Nous vous recommandons de le choisir comme option finale ou vous ne pourrez pas bien voir les caméras)")
            {
                RightIcon = MenuItem.Icon.STAR
            };
            mainMenu.AddMenuItem(ScaleBtn);



            //Finish Button
            MenuItem FinishBtn = new MenuItem("~e~Confirmer votre personnage", "Terminez la création du personnage.")
            {
                RightIcon = MenuItem.Icon.TICK
            };
            mainMenu.AddMenuItem(FinishBtn);

            //Events
            mainMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                if (_itemIndex == 4)
                {
                    CreatePlayer.ChangeScale(SkinsUtils.SCALE_LIST[_newIndex]);
                }
            };

            mainMenu.OnMenuClose += (_menu) =>
            {
                if (CreatePlayer.isInCharCreation)
                {
                    CreatePlayer.CloseSecureMenu();
                }
            };

            mainMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_index == 5)
                {
                    if (CreatePlayer.Identity == null)
                    {
                        Toast.AddToast("Vous devez vous créer une identité avant de valider!", 10000);
                        return;
                    }

                    UIHelper.LoadingScreenText("RDRP", "Chargement...", "Envoi des informations au serveur");
                    CreatePlayer.SaveChanges();
                    mainMenu.CloseMenu();
                }
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }
    }
}
