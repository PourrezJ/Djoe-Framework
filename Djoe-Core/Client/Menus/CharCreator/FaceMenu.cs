using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using Client.Utils;

namespace Client.Menus
{
    class FaceMenu
    {
        private static Menu faceMenu = new Menu("Personnaliser le visage", "Personnalisez le visage de votre personnages");
        private static bool setupDone = false;
        public static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuController.AddMenu(faceMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            List<string> sizeType = new List<string>();

            for (float i = 1; i < 11; i++) 
            {
                sizeType.Add("Type #" + i);
            }

            MenuListItem mListHead = new MenuListItem("Taille du visage", sizeType, 4); 
            faceMenu.AddMenuItem(mListHead); 

            MenuListItem mListEyeBrowH = new MenuListItem("Hauteur des sourcils", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeBrowH); 
            
            MenuListItem mListEyeBrowW = new MenuListItem("Largeur des sourcils", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeBrowW); 
          
            MenuListItem mListEyeBrowD = new MenuListItem("Profondeur des sourcils", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeBrowD); 

         
            MenuListItem mListEarsH = new MenuListItem("Hauteur d'oreille", sizeType, 4);
            faceMenu.AddMenuItem(mListEarsH);
           
            MenuListItem mListEarsW = new MenuListItem("Inclinaison des oreilles", sizeType, 4);
            faceMenu.AddMenuItem(mListEarsW);
           
            MenuListItem mListEarsD = new MenuListItem("Taille des oreilles", sizeType, 4);
            faceMenu.AddMenuItem(mListEarsD);

            MenuListItem mListEarsL = new MenuListItem("Taille du lobe", sizeType, 4);
            faceMenu.AddMenuItem(mListEarsL);
           
            MenuListItem mListEyeLidH = new MenuListItem("Hauteur des paupières", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeLidH); 

            MenuListItem mListEyeLidW = new MenuListItem("Largeur des paupières", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeLidW);

            MenuListItem mListEyeD = new MenuListItem("Profondeur des yeux", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeD); 

            MenuListItem mListEyeAng = new MenuListItem("Inclinaison des yeux", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeAng); 

            MenuListItem mListEyeDis = new MenuListItem("Séparation des yeux", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeDis); 
           
            MenuListItem mListEyeH = new MenuListItem("Hauteur des yeux", sizeType, 4);
            faceMenu.AddMenuItem(mListEyeH);


            MenuListItem mListNoseW = new MenuListItem("Largeur de nez", sizeType, 4);
            faceMenu.AddMenuItem(mListNoseW); 

            MenuListItem mListNoseS = new MenuListItem("Taille de nez", sizeType, 4);
            faceMenu.AddMenuItem(mListNoseS);

            MenuListItem mListNoseH = new MenuListItem("Hauteur du nez", sizeType, 4);
            faceMenu.AddMenuItem(mListNoseH);

            MenuListItem mListNoseAng = new MenuListItem("Inclinaison du nez", sizeType, 4);
            faceMenu.AddMenuItem(mListNoseAng);

            MenuListItem mListNoseC = new MenuListItem("Courbure du nez", sizeType, 4);
            faceMenu.AddMenuItem(mListNoseC);

            MenuListItem mListNoseDis = new MenuListItem("Séparation des narines", sizeType, 4);
            faceMenu.AddMenuItem(mListNoseDis);


            MenuListItem mListCheekBonesH = new MenuListItem("Hauteur des pommettes", sizeType, 4);
            faceMenu.AddMenuItem(mListCheekBonesH);

            MenuListItem mListCheekBonesW = new MenuListItem("Largeur des joues", sizeType, 4);
            faceMenu.AddMenuItem(mListCheekBonesW);

            MenuListItem mListCheekBonesD = new MenuListItem("Profondeur des pommettes", sizeType, 4);
            faceMenu.AddMenuItem(mListCheekBonesD);


            MenuListItem mListMouthW = new MenuListItem("Largeur de la bouche", sizeType, 4);
            faceMenu.AddMenuItem(mListMouthW);

            MenuListItem mListMouthD = new MenuListItem("Profondeur de la bouche", sizeType, 4);
            faceMenu.AddMenuItem(mListMouthD);

            MenuListItem mListMouthX = new MenuListItem("Écart de la bouche", sizeType, 4);
            faceMenu.AddMenuItem(mListMouthX);

            MenuListItem mListMouthY = new MenuListItem("Hauteur de la bouche", sizeType, 4);
            faceMenu.AddMenuItem(mListMouthY);


            MenuListItem mListULiphH = new MenuListItem("Hauteur de la lèvre supérieure", sizeType, 4);
            faceMenu.AddMenuItem(mListULiphH);

            MenuListItem mListULiphW = new MenuListItem("Largeur de lèvre supérieure", sizeType, 4);
            faceMenu.AddMenuItem(mListULiphW); 

            MenuListItem mListULiphD = new MenuListItem("Profondeur de la lèvre supérieure", sizeType, 4);
            faceMenu.AddMenuItem(mListULiphD);

            MenuListItem mListLLiphH = new MenuListItem("Hauteur de lèvre inférieure", sizeType, 4);
            faceMenu.AddMenuItem(mListLLiphH); 
            MenuListItem mListLLiphW = new MenuListItem("Largeur de lèvre inférieure", sizeType, 4);
            faceMenu.AddMenuItem(mListLLiphW);
            MenuListItem mListLLiphD = new MenuListItem("Profondeur des lèvres inférieure", sizeType, 4);
            faceMenu.AddMenuItem(mListLLiphD);


            MenuListItem mListJawH = new MenuListItem("Hauteur de mâchoire", sizeType, 4);
            faceMenu.AddMenuItem(mListJawH);

            MenuListItem mListJawW = new MenuListItem("Largeur de mâchoirea", sizeType, 4);
            faceMenu.AddMenuItem(mListJawW);

            MenuListItem mListJawD = new MenuListItem("Profondeur de mâchoire", sizeType, 4);
            faceMenu.AddMenuItem(mListJawD);


            MenuListItem mListChinH = new MenuListItem("Hauteur du menton", sizeType, 4);
            faceMenu.AddMenuItem(mListChinH);

            MenuListItem mListChinW = new MenuListItem("Largeur du menton", sizeType, 4);
            faceMenu.AddMenuItem(mListChinW);

            MenuListItem mListChinD = new MenuListItem("Profondeur du menton", sizeType, 4);
            faceMenu.AddMenuItem(mListChinD);



            faceMenu.OnMenuOpen += (_menu) => {
                CreatePlayer.SwapCameras(2);
            };

            faceMenu.OnMenuClose += (_menu) =>
            {

            };

            faceMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                CreatePlayer.SetPlayerFaceBlend(_itemIndex, _newIndex);
            };

        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return faceMenu;
        }
    }
}
