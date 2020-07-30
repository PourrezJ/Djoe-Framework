using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System.Collections.Generic;
using System.Linq;
using Client.Utils;

namespace Client.Menus
{
    class ClothesMenu
    {
        private static Menu clothesMenu = new Menu("Vestiaire", "Habillez votre personnage");
        private static bool setupDone = false;
        public static void SetupMenu()
        {
            if (setupDone) 
                return;

            setupDone = true;
            MenuController.AddMenu(clothesMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            #region Chapeaux
            List<string> hatType = new List<string>();
            hatType.Add("Sans Chapeau");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.HATS_MALE.Count + 1) : (SkinsUtils.HATS_FEMALE.Count + 1)); i++)
            {
                hatType.Add("Chapeau #" + i);
            }

            MenuListItem mListHats = new MenuListItem("Chapeaux", hatType, 0, "Choisissez un chapeau");
            clothesMenu.AddMenuItem(mListHats);
            #endregion

            #region Lunettes
            List<string> eyeWearType = new List<string>();
            eyeWearType.Add("Sans Lunettes");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.EYEWEAR_MALE.Count + 1) : (SkinsUtils.EYEWEAR_FEMALE.Count + 1)); i++)
            {
                eyeWearType.Add("Chapeau #" + i);
            }

            MenuListItem mListEyeWear = new MenuListItem("Lunettes #", eyeWearType, 0, "Choisissez une paire de lunettes"); 
            clothesMenu.AddMenuItem(mListEyeWear);
            #endregion

            #region Cravates
            List<string> neckWearType = new List<string>();
            neckWearType.Add("Sans Cravates");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.NECKWEAR_MALE.Count + 1) : (SkinsUtils.NECKWEAR_FEMALE.Count + 1)); i++)
            {
                neckWearType.Add("Cravates #" + i);
            }

            MenuListItem mListNeckWear = new MenuListItem("Cravates", neckWearType, 0, "Choisissez une cravates"); 
            clothesMenu.AddMenuItem(mListNeckWear);
            #endregion

            #region Colliers
            List<string> neckTiesType = new List<string>();
            neckTiesType.Add("Sans Colliers");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.NECKTIES_MALE.Count + 1) : (SkinsUtils.NECKTIES_FEMALE.Count + 1)); i++)
            {
                neckWearType.Add("Colliers #" + i);
            }

            MenuListItem mListNeckTies = new MenuListItem("Colliers", neckTiesType, 0, "Choisissez un collier"); 
            clothesMenu.AddMenuItem(mListNeckTies);
            #endregion

            #region Chemises
            List<string> shirtsType = new List<string>();
            shirtsType.Add("Sans Chemise");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.SHIRTS_MALE.Count + 1) : (SkinsUtils.SHIRTS_FEMALE.Count + 1)); i++)
            {
                shirtsType.Add("Chemise #" + i);
            }

            MenuListItem mListShirts = new MenuListItem("Chemises", shirtsType, 0, "Choisissez une chemise"); 
            clothesMenu.AddMenuItem(mListShirts);
            #endregion

            #region Bretelles
            List<string> suspendersType = new List<string>();
            suspendersType.Add("Sans Bretelles");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.SUSPENDERS_MALE.Count + 1) : (SkinsUtils.SUSPENDERS_FEMALE.Count + 1)); i++)
            {
                suspendersType.Add("Bretelles #" + i);
            }

            MenuListItem mListSuspenders = new MenuListItem("Bretelles", suspendersType, 0, "Choisissez des bretelles");
            clothesMenu.AddMenuItem(mListSuspenders);
            #endregion

            #region Gilets
            List<string> vestType = new List<string>();
            vestType.Add("Sans Gilet");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.VEST_MALE.Count + 1) : (SkinsUtils.VEST_FEMALE.Count + 1)); i++)
            {
                vestType.Add("Gilet #" + i);
            }

            MenuListItem mListVest = new MenuListItem("Gilets", vestType, 0, "Choisissez un gilet");
            clothesMenu.AddMenuItem(mListVest);
            #endregion

            #region Manteaux
            List<string> coatsType = new List<string>();
            coatsType.Add("Sans Manteau");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.COATS_MALE.Count + 1) : (SkinsUtils.COATS_FEMALE.Count + 1)); i++)
            {
                coatsType.Add("Manteau #" + i);
            }

            MenuListItem mListCoats = new MenuListItem("Manteaux", coatsType, 0, "Choisissez un manteau");
            clothesMenu.AddMenuItem(mListCoats);
            #endregion

            #region Ponchos
            List<string> ponchosType = new List<string>();
            ponchosType.Add("Sans Poncho");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.PONCHOS_MALE.Count + 1) : (SkinsUtils.PONCHOS_FEMALE.Count + 1)); i++)
            {
                ponchosType.Add("Poncho #" + i);
            }

            MenuListItem mListPonchos = new MenuListItem("Ponchos", ponchosType, 0, "Choisissez un poncho");
            clothesMenu.AddMenuItem(mListPonchos);
            #endregion

            #region Capes
            List<string> cloakType = new List<string>();
            cloakType.Add("Sans Cape");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.CLOAK_MALE.Count + 1) : (SkinsUtils.CLOAK_FEMALE.Count + 1)); i++)
            {
                cloakType.Add("Cape #" + i);
            }

            MenuListItem mListCloak = new MenuListItem("Cape", cloakType, 0, "Choisissez une cape");
            clothesMenu.AddMenuItem(mListCloak);
            #endregion

            #region Gants
            List<string> glovesType = new List<string>();
            glovesType.Add("Sans Gants");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.GLOVES_MALE.Count + 1) : (SkinsUtils.GLOVES_FEMALE.Count + 1)); i++)
            {
                glovesType.Add("Gants #" + i);
            }

            MenuListItem mListGloves = new MenuListItem("Gants", glovesType, 0, "Choisissez des gants");
            clothesMenu.AddMenuItem(mListGloves);
            #endregion

            #region Bagues Droite
            List<string> ringsRhType = new List<string>();
            ringsRhType.Add("Sans Bague");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.RINGS_RH_MALE.Count + 1) : (SkinsUtils.RINGS_RH_FEMALE.Count + 1)); i++)
            {
                ringsRhType.Add("Anneau #" + i);
            }
            
            MenuListItem mListRingsRhType = new MenuListItem("Anneaux à droite", ringsRhType, 0, "Choisissez bagues main droite");
            clothesMenu.AddMenuItem(mListRingsRhType);
            #endregion

            #region Bagues Gauche
            List<string> ringsLhType = new List<string>();
            ringsLhType.Add("Sans Bague");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.RINGS_LH_MALE.Count + 1) : (SkinsUtils.RINGS_LH_FEMALE.Count + 1)); i++)
            {
                ringsLhType.Add("Anneau #" + i);
            }

            MenuListItem mListRingsLh = new MenuListItem("Anneaux gauche", ringsLhType, 0, "Choisissez bagues main gauche"); 
            clothesMenu.AddMenuItem(mListRingsLh);
            #endregion

            #region Bracelets
            List<string> braceletsType = new List<string>();
            braceletsType.Add("Sans Bracelet");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.BRACELETS_MALE.Count + 1) : (SkinsUtils.BRACELETS_FEMALE.Count + 1)); i++)
            {
                braceletsType.Add("Bracelet #" + i);
            }

            MenuListItem mListbracelets = new MenuListItem("Bracelets", braceletsType, 0, "Choisissez un bracelet"); 
            clothesMenu.AddMenuItem(mListbracelets);
            #endregion

            #region Ceinture de pistolet
            List<string> gunbeltType = new List<string>();
            gunbeltType.Add("Sans Ceinture");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.GUNBELT_MALE.Count + 1) : (SkinsUtils.GUNBELT_FEMALE.Count + 1)); i++)
            {
                gunbeltType.Add("Ceinture #" + i);
            }

            MenuListItem mListGunbelt = new MenuListItem("Ceinture de pistolet", gunbeltType, 0, "Choisissez une ceinture de pistolet"); 
            clothesMenu.AddMenuItem(mListGunbelt);
            #endregion

            #region Ceintures
            List<string> beltType = new List<string>();
            beltType.Add("Sans Ceinture");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.BELT_MALE.Count + 1) : (SkinsUtils.BELT_FEMALE.Count + 1)); i++)
            {
                beltType.Add("Ceinture #" + i);
            }

            MenuListItem mListBelt = new MenuListItem("Ceintures", beltType, 0, "Choisissez la ceinture");
            clothesMenu.AddMenuItem(mListBelt);
            #endregion

            #region Boucles
            List<string> buckleType = new List<string>();
            buckleType.Add("Sans Boucle");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.BUCKLE_MALE.Count + 1) : (SkinsUtils.BUCKLE_FEMALE.Count + 1)); i++)
            {
                buckleType.Add("Boucle #" + i);
            }

            MenuListItem mListBuckle = new MenuListItem("Boucles", buckleType, 0, "Choisissez une boucle");
            clothesMenu.AddMenuItem(mListBuckle);
            #endregion

            #region Étui
            List<string> holstersSType = new List<string>();
            holstersSType.Add("Sans Étui");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.HOLSTERS_S_MALE.Count + 1) : (SkinsUtils.HOLSTERS_S_FEMALE.Count + 1)); i++)
            {
                holstersSType.Add("Étui #" + i);
            }

            MenuListItem mListSHolsters = new MenuListItem("Étuis secondaires", holstersSType, 0, "Choisissez l'étui secondaire");
            clothesMenu.AddMenuItem(mListSHolsters);
            #endregion

            #region Pantalons
            List<string> pantsType = new List<string>();
            pantsType.Add("Sans Pantalons");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.PANTS_MALE.Count + 1) : (SkinsUtils.PANTS_FEMALE.Count + 1)); i++)
            {
                pantsType.Add("Pantalon #" + i);
            }

            MenuListItem mListPants = new MenuListItem("Pantalons", pantsType, 0, "Choisissez un pantalon"); 
            clothesMenu.AddMenuItem(mListPants);
            #endregion

            #region Jupes
            if (CreatePlayer.model_selected != "mp_male")
            {
                List<string> skirtsType = new List<string>();
                skirtsType.Add("Sans Jupe");

                for (float i = 1; i < SkinsUtils.SKIRTS_FEMALE.Count + 1; i++)
                {
                    skirtsType.Add("Jupe #" + i);
                }

                MenuListItem mListSkirts = new MenuListItem("Jupes", skirtsType, 0, "Choisissez une jupe"); 
                clothesMenu.AddMenuItem(mListSkirts);
            }
            #endregion

            #region Chaps
            List<string> chapsType = new List<string>();
            chapsType.Add("Sans Chaps");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.CHAPS_MALE.Count + 1) : (SkinsUtils.CHAPS_FEMALE.Count + 1)); i++)
            {
                chapsType.Add("Chaps #" + i);
            }

            MenuListItem mListChaps = new MenuListItem("Chaps", chapsType, 0, "Choisissez un chaps");
            clothesMenu.AddMenuItem(mListChaps);
            #endregion

            #region Bottes
            List<string> bootsType = new List<string>();
            bootsType.Add("Sans Bottes");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.BOOTS_MALE.Count + 1) : (SkinsUtils.BOOTS_FEMALE.Count + 1)); i++)
            {
                bootsType.Add("Bottes #" + i);
            }

            MenuListItem mListBoots = new MenuListItem("Bottes", bootsType, 0, "Choisissez une paire de bottes");
            clothesMenu.AddMenuItem(mListBoots);
            #endregion

            #region Eperons
            List<string> spursType = new List<string>();
            spursType.Add("Sans Éperons");

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.SPURS_MALE.Count + 1) : (SkinsUtils.SPURS_FEMALE.Count + 1)); i++)
            {
                spursType.Add("Éperons #" + i);
            }

            MenuListItem mListSpurs = new MenuListItem("Éperons", spursType, 0, "Choisissez un éperons");
            clothesMenu.AddMenuItem(mListSpurs);
            #endregion

            clothesMenu.OnMenuOpen += ClothesMenu_OnMenuOpen;

            clothesMenu.OnIndexChange += ClothesMenu_OnIndexChange;

            clothesMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                Debug.WriteLine(_itemIndex.ToString());

                if (_itemIndex >= 19 && CreatePlayer.model_selected == "mp_male")
                    _itemIndex++;

                switch (_itemIndex)
                {
                    case 0:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x9925C067, "Hat", SkinsUtils.HATS_MALE, SkinsUtils.HATS_FEMALE);
                        break;
                    case 1:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x5E47CA6, "EyeWear", SkinsUtils.EYEWEAR_MALE, SkinsUtils.EYEWEAR_FEMALE);
                        break;
                    case 2:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x5FC29285, "NeckWear", SkinsUtils.NECKWEAR_MALE, SkinsUtils.NECKWEAR_FEMALE);
                        break;
                    case 3:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x7A96FACA, "NeckTies", SkinsUtils.NECKTIES_MALE, SkinsUtils.NECKTIES_FEMALE);
                        break;
                    case 4:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x2026C46D, "Shirt", SkinsUtils.SHIRTS_MALE, SkinsUtils.SHIRTS_FEMALE);
                        break;
                    case 5:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x877A2CF7, "Suspender", SkinsUtils.SUSPENDERS_MALE, SkinsUtils.SUSPENDERS_FEMALE);
                        break;
                    case 6:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x485EE834, "Vest", SkinsUtils.VEST_MALE, SkinsUtils.VEST_FEMALE);
                        break;
                    case 7:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xE06D30CE, "Coat", SkinsUtils.COATS_MALE, SkinsUtils.COATS_FEMALE);
                        break;
                    case 8:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xAF14310B, "Poncho", SkinsUtils.PONCHOS_MALE, SkinsUtils.PONCHOS_FEMALE);
                        break;
                    case 9:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x3C1A74CD, "Cloak", SkinsUtils.CLOAK_MALE, SkinsUtils.CLOAK_FEMALE);
                        break;
                    case 10:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xEABE0032, "Glove", SkinsUtils.GLOVES_MALE, SkinsUtils.GLOVES_FEMALE);
                        break;
                    case 11:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x7A6BBD0B, "RingRh", SkinsUtils.RINGS_RH_MALE, SkinsUtils.RINGS_RH_FEMALE);
                        break;
                    case 12:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xF16A1D23, "RingLh", SkinsUtils.RINGS_LH_MALE, SkinsUtils.RINGS_LH_FEMALE);
                        break;
                    case 13:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x7BC10759, "Bracelet", SkinsUtils.BRACELETS_MALE, SkinsUtils.BRACELETS_FEMALE);
                        break;
                    case 14:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x9B2C8B89, "Gunbelt", SkinsUtils.GUNBELT_MALE, SkinsUtils.GUNBELT_FEMALE);
                        break;
                    case 15:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xA6D134C6, "Belt", SkinsUtils.BELT_MALE, SkinsUtils.BELT_FEMALE);
                        break;
                    case 16:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xFAE9107F, "Buckle", SkinsUtils.BUCKLE_MALE, SkinsUtils.BUCKLE_FEMALE);
                        break;
                    case 17:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xB6B6122D, "Holster", SkinsUtils.HOLSTERS_S_MALE, SkinsUtils.HOLSTERS_S_FEMALE);
                        break;
                    case 18:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x1D4C528A, "Pant", SkinsUtils.PANTS_MALE, SkinsUtils.PANTS_FEMALE);
                        break;
                    case 19:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0xA0E3AB7F, "Skirt", SkinsUtils.SKIRTS_FEMALE, SkinsUtils.SKIRTS_FEMALE);
                        break;
                    case 20:
                        Debug.WriteLine("Chap");
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x3107499B, "Chap", SkinsUtils.CHAPS_MALE, SkinsUtils.CHAPS_FEMALE);
                        break;
                    case 21:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x777EC6EF, "Boots", SkinsUtils.BOOTS_MALE, SkinsUtils.BOOTS_FEMALE);
                        break;
                    case 22:
                        CreatePlayer.SetPlayerComponent(_newIndex, 0x18729F39, "Spurs", SkinsUtils.SPURS_MALE, SkinsUtils.SPURS_FEMALE);
                        break;
                }
            };

        }

        private static void ClothesMenu_OnMenuOpen(Menu menu)
        {
            if (CreatePlayer.model_selected != "mp_male")
            {
                var camPos = API.GetCamCoord(CreatePlayer.Camera_FaceEditor);
                API.SetCamCoord(CreatePlayer.Camera_FaceEditor, camPos.X, camPos.Y, camPos.Z -= 0.13f);
            }
            CreatePlayer.SwapCameras(0);
        }

        private static void ClothesMenu_OnIndexChange(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex)
        {

            if (newIndex >= 19 && CreatePlayer.model_selected != "mp_male")
                newIndex++;

            switch (newIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    CreatePlayer.SwapCameras(2); // Face
                    break;

                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 18:
                case 20:
                case 21:
                    CreatePlayer.SwapCameras(1); // Body
                    break;

                case 16:
                case 17:
                    CreatePlayer.SwapCameras(3); // Boucle
                    break;
                case 22:
                    CreatePlayer.SwapCameras(4); // Legs
                    break;

                default:
                    CreatePlayer.SwapCameras(0); // Editor
                    break;

            }
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return clothesMenu;
        }
    }
}
