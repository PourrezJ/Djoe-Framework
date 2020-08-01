using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using System.Linq;
using ClientExtented;
using Client.Utils;

namespace Client.Menus
{
    class SkinMenu
    {
        private static Menu skinMenu = new Menu("Personnaliser la peau", "Personnalisez le corps de votre personnage");

        private static MenuListItem btnSelectorBody;
        private static MenuListItem btnSelectorFace;
        private static MenuListItem btnSelectorTorso;
        private static MenuListItem btnSelectorLegs;
        private static MenuListItem btnSelectoreyeBrows;
        private static MenuListItem btnSelectorScars;
        private static MenuListItem btnSelectorSpots;
        private static MenuListItem btnSelectorDisc;
        private static MenuListItem btnSelectorComplex;
        private static MenuListItem btnSelectorAcne;
        private static MenuListItem btnSelectorAgeing;
        private static MenuListItem btnSelectorMoles;
        private static MenuListItem btnSelectorFreckles;
        private static MenuListItem btnSelectorGrime;
        private static MenuListItem btnSelectorLipsticks;
        private static MenuListItem btnSelectorLipsticksColor;
        private static MenuListItem btnSelectorLipsticksPColor;
        private static MenuListItem btnSelectorShadows;
        private static MenuListItem btnSelectorShadowsColor;
        private static MenuListItem btnSelectorShadowsPColor;

        private static bool setupDone = false;
        public static void SetupMenu()
        {
            if (setupDone)
                return;

            setupDone = true;

            MenuController.AddMenu(skinMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            var maleOrFemale = (CreatePlayer.model_selected == "mp_male") ? SkinsUtils.Male : SkinsUtils.Female;

            List<string> bodyValues = new List<string>();
            for (int i = 1; i <= maleOrFemale.Count(); i++)
            {
                bodyValues.Add("Couleur #" + i);
            }

            btnSelectorBody = new MenuListItem("Couleur de peau", bodyValues, 0, "Choisissez la couleur de votre peau");
            skinMenu.AddMenuItem(btnSelectorBody);

            List<string> faceValues = new List<string>();
            for (int i = 1; i <= maleOrFemale[0].Heads.Count(); i++)
            {
                faceValues.Add("Visage #" + i);
            }

            btnSelectorFace = new MenuListItem("Type de visage", faceValues, 0, "Choisissez le type de visage");
            skinMenu.AddMenuItem(btnSelectorFace); 


            List<string> torsoValues = new List<string>();
            for (int i = 1; i <= maleOrFemale[0].Body.Count(); i++)
            {
                torsoValues.Add("Torse #" + i);
            }

            btnSelectorTorso = new MenuListItem("Type de torse", torsoValues, 0, "Choisissez le type de torse"); 
            skinMenu.AddMenuItem(btnSelectorTorso); 

            List<string> endoformValues = new List<string>();

            for (int i = 1; i < SkinsUtils.BODY_TYPES.Count + 1; i++) 
            {
                endoformValues.Add("Corps #" + i);
            }

            MenuListItem btnSelectorBodyForm = new MenuListItem("Forme du corps", endoformValues, 0, "Choisissez la forme du corps"); 
            skinMenu.AddMenuItem(btnSelectorBodyForm);


            List<string> waistValues = new List<string>();

            for (int i = 1; i < SkinsUtils.WAIST_TYPES.Count + 1; i++)
            {
                waistValues.Add("Taille #" + i);
            }

            MenuListItem btnSelectorWaist = new MenuListItem("Forme du ventre", waistValues, 0, "Choisissez votre forme du ventre");
            skinMenu.AddMenuItem(btnSelectorWaist);

            List<string> legsValues = new List<string>();
            for (int i = 1; i <= maleOrFemale[0].Legs.Count(); i++)
            {
                legsValues.Add("Jambes #" + i);
            }

            btnSelectorLegs = new MenuListItem("Type de jambes", legsValues, 0, "Choisissez le type de jambes"); 
            skinMenu.AddMenuItem(btnSelectorLegs);

            List<string> hairValues = new List<string>();

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.HAIR_MALE.Count + 2) : (SkinsUtils.HAIR_FEMALE.Count + 2)); i++) 
            {
                hairValues.Add("Cheveux #" + i);
            }

            MenuListItem btnSelectorHairs = new MenuListItem("Type de cheveux", hairValues, 0, "Choisissez votre coupe de cheveux"); 
            skinMenu.AddMenuItem(btnSelectorHairs);

            List<string> eyesValues = new List<string>();

            for (float i = 1; i < ((CreatePlayer.model_selected == "mp_male") ? (SkinsUtils.EYES_MALE.Count + 1) : (SkinsUtils.EYES_FEMALE.Count + 1)); i++)
            {
                eyesValues.Add("Yeux #" + i);
            }

            MenuListItem btnSelectorEyes = new MenuListItem("Types d'yeux", eyesValues, 0, "Changez la couleur de vos yeux"); 
            skinMenu.AddMenuItem(btnSelectorEyes);

            if (maleOrFemale == SkinsUtils.Male)
            {
                List<string> beardValues = new List<string>();

                for (float i = 1; i < SkinsUtils.BEARD_MALE.Count + 2; i++)
                {
                    beardValues.Add("Barbe #" + i);
                }

                MenuListItem btnSelectorBeards = new MenuListItem("Type de barbe", beardValues, 0, "Choisissez la barbe"); 
                skinMenu.AddMenuItem(btnSelectorBeards);
            }

            List<string> eyeBrowsValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["eyebrows"].Count() + 2; i++)
            {
                eyeBrowsValues.Add("Sourcils #" + i);
            }

            btnSelectoreyeBrows = new MenuListItem("Types de sourcils", eyeBrowsValues, 0, "Forme des sourcils");
            skinMenu.AddMenuItem(btnSelectoreyeBrows);

            List<string> scarsValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["scars"].Count() + 2; i++) 
            {
                scarsValues.Add("Cicatrice #" + i);
            }

            btnSelectorScars = new MenuListItem("Types de cicatrices", scarsValues, 0, "Ajouter des cicatrices"); 
            skinMenu.AddMenuItem(btnSelectorScars);

            List<string> spotsValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["spots"].Count() + 2; i++) 
            {
                spotsValues.Add("Tache #" + i);
            }

            btnSelectorSpots = new MenuListItem("Types de taches", spotsValues, 0, "Taches solaires"); 
            skinMenu.AddMenuItem(btnSelectorSpots);

            List<string> discValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["disc"].Count() + 2; i++) 
            {
                discValues.Add("Petechiae #" + i);
            }

            btnSelectorDisc = new MenuListItem("Types de Petechiae", discValues, 0, "Taches rouges sur le visage (généralement en raison de l'âge)"); 
            skinMenu.AddMenuItem(btnSelectorDisc);

            List<string> complexValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["complex"].Count() + 2; i++) 
            {
                complexValues.Add("Complexion #" + i);
            }

            btnSelectorComplex = new MenuListItem("Types de Complexion", complexValues, 0, "Taches de coup de soleil"); 
            skinMenu.AddMenuItem(btnSelectorComplex);

            List<string> acneValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["acne"].Count() + 2; i++)
            {
                acneValues.Add("Acné #" + i);
            }

            btnSelectorAcne = new MenuListItem("Types de Acné", acneValues, 0, "Ajoutez de l'acné au visage"); 
            skinMenu.AddMenuItem(btnSelectorAcne);

            List<string> ageingValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["ageing"].Count() + 2; i++)
            {
                ageingValues.Add("Vieillissement #" + i);
            }

            btnSelectorAgeing = new MenuListItem("Types de vieillissement", ageingValues, 0, "Vieillissement du visage"); 
            skinMenu.AddMenuItem(btnSelectorAgeing);

            List<string> molesValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["moles"].Count() + 2; i++)
            {
                molesValues.Add("Taupes #" + i);
            }

            btnSelectorMoles = new MenuListItem("Types de taupes", molesValues, 0, "Ajouter des pois");
            skinMenu.AddMenuItem(btnSelectorMoles);

            List<string> frecklesValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["freckles"].Count() + 2; i++) 
            {
                frecklesValues.Add("Taches de rousseur #" + i);
            }

            btnSelectorFreckles = new MenuListItem("Taches de rousseur", frecklesValues, 0, "Ajouter des taches de rousseur"); 
            skinMenu.AddMenuItem(btnSelectorFreckles);

            List<string> grimeValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["grime"].Count() + 2; i++)
            {
                grimeValues.Add("Saleté #" + i);
            }

            btnSelectorGrime = new MenuListItem("Types de Saleté", grimeValues, 0, "Ajouter de la saleté ou de la crasse");
            skinMenu.AddMenuItem(btnSelectorGrime);

            List<string> lipsticksValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["lipsticks"].Count() + 2; i++)
            {
                lipsticksValues.Add("Rouge à lèvres #" + i);
            }

            btnSelectorLipsticks = new MenuListItem("Types de rouges à lèvres", lipsticksValues, 0, "Ajouter du maquillage");
            skinMenu.AddMenuItem(btnSelectorLipsticks);

            List<string> lipsticksColorValues = new List<string>();

            for (float i = 1; i < SkinsUtils.COLOR_PALETTES.Count() + 1; i++) 
            {
                lipsticksColorValues.Add("Couleur #" + i);
            }

            btnSelectorLipsticksColor = new MenuListItem("Couleur de rouge à lèvres de base", lipsticksColorValues, 0, "Ajouter du maquillage");
            skinMenu.AddMenuItem(btnSelectorLipsticksColor);

            List<string> lipsticksPColorValues = new List<string>();

            for (float i = 1; i < 255; i++)
            {
                lipsticksPColorValues.Add("Couleur #" + i);
            }

            btnSelectorLipsticksPColor = new MenuListItem("Mélange de couleurs de rouge à lèvres", lipsticksPColorValues, 0, "Ajouter du maquillage");
            skinMenu.AddMenuItem(btnSelectorLipsticksPColor);

            List<string> shadowsValues = new List<string>();

            for (float i = 1; i < SkinsUtils.overlays_info["shadows"].Count() + 2; i++)
            {
                shadowsValues.Add("Le fard à paupières #" + i);
            }

            btnSelectorShadows = new MenuListItem("Types d'ombrage", shadowsValues, 0, "Ajouter du maquillage");
            skinMenu.AddMenuItem(btnSelectorShadows);

            List<string> shadowsColorValues = new List<string>();

            for (float i = 1; i < SkinsUtils.COLOR_PALETTES.Count() + 1; i++)
            {
                shadowsColorValues.Add("Couleur #" + i);
            }

            btnSelectorShadowsColor = new MenuListItem("Couleur de base de l'ombre", shadowsColorValues, 0, "Ajouter du maquillage");
            skinMenu.AddMenuItem(btnSelectorShadowsColor);

            List<string> shadowsPColorValues = new List<string>();

            for (float i = 1; i < 255; i++) 
            {
                shadowsPColorValues.Add("Couleur #" + i);
            }

            btnSelectorShadowsPColor = new MenuListItem("Mélange de couleurs d'ombrage", shadowsPColorValues, 0, "Ajouter du maquillage");
            skinMenu.AddMenuItem(btnSelectorShadowsPColor);

            skinMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                if (CreatePlayer.model_selected != "mp_male" && _itemIndex >= 8)
                    _itemIndex++;

                switch (_itemIndex)
                {
                    case 0:
                        UpdateLists(_newIndex, maleOrFemale);

                        break;
                    case 1:
                        CreatePlayer.SetPlayerModelComponent(maleOrFemale[btnSelectorBody.ListIndex].Heads[_newIndex], "HeadType");
                        ReloadTextures();

                        break;
                    case 2:
                        CreatePlayer.SetPlayerModelComponent(maleOrFemale[btnSelectorBody.ListIndex].Body[_newIndex], "BodyType");

                        break;
                    case 3:
                        CreatePlayer.SetPlayerBodyComponent((uint)SkinsUtils.BODY_TYPES.ElementAt(_newIndex), "Body");
                        break;
                    case 4:
                        CreatePlayer.SetPlayerBodyComponent((uint)SkinsUtils.WAIST_TYPES.ElementAt(_newIndex), "Waist");
                        break;
                    case 5:
                        CreatePlayer.SetPlayerModelComponent(maleOrFemale[btnSelectorBody.ListIndex].Legs[_newIndex], "LegsType");

                        break;
                    case 6:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.SetPlayerModelListComps("Hair", 0, 0x864B03AE);
                        }
                        else if (CreatePlayer.model_selected == "mp_male")
                        {
                            CreatePlayer.SetPlayerModelListComps("Hair", SkinsUtils.HAIR_MALE.ElementAt(_newIndex - 1), 0x864B03AE);
                        }
                        else
                        {
                            CreatePlayer.SetPlayerModelListComps("Hair", SkinsUtils.HAIR_FEMALE.ElementAt(_newIndex - 1), 0x864B03AE);
                        }
                        break;
                    case 7:
                        if (CreatePlayer.model_selected == "mp_male")
                        {
                            CreatePlayer.SetPlayerModelListComps("Eyes", SkinsUtils.EYES_MALE.ElementAt(_newIndex), 0x864B03AE);
                        }
                        else
                        {
                            CreatePlayer.SetPlayerModelListComps("Eyes", SkinsUtils.EYES_FEMALE.ElementAt(_newIndex), 0x864B03AE);
                        }
                        break;
                    case 8:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.SetPlayerModelListComps("Beard", 0, 0xF8016BCA);
                        }
                        else if (CreatePlayer.model_selected == "mp_male")
                        {
                            CreatePlayer.SetPlayerModelListComps("Beard", SkinsUtils.BEARD_MALE.ElementAt(_newIndex - 1), 0xF8016BCA);
                        }
                        break;
                    case 9:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("eyebrows", 0, _newIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("eyebrows", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 10:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("scars", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("scars", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 11:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("spots", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("spots", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 12:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("disc", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("disc", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 13:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("complex", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("complex", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 14:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("acne", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("acne", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 15:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("ageing", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("ageing", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 16:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("moles", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("moles", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 17:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("freckles", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("freckles", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 18:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("grime", 0, _newIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("grime", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 19:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("lipsticks", 0, _newIndex, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("lipsticks", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 20:
                        if (btnSelectorLipsticks.ListIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("lipsticks", 0, btnSelectorLipsticks.ListIndex, 0, 0, 0, 1.0f, 0, _newIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("lipsticks", 1, btnSelectorLipsticks.ListIndex - 1, 0, 0, 0, 1.0f, 0, _newIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 21:
                        if (btnSelectorLipsticks.ListIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("lipsticks", 0, btnSelectorLipsticks.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("lipsticks", 1, btnSelectorLipsticks.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 22:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("shadows", 0, _newIndex, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorShadowsPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("shadows", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorShadowsPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 23:
                        if (btnSelectorShadows.ListIndex == 0)
                        {
                            CreatePlayer.ToggleOverlayChange("shadows", 0, btnSelectorShadows.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreatePlayer.ToggleOverlayChange("shadows", 1, btnSelectorShadows.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                }
            };
        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return skinMenu;
        }

        private static void ReloadTextures() {

            if (btnSelectoreyeBrows.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("eyebrows", 0, btnSelectoreyeBrows.ListIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("eyebrows", 1, btnSelectoreyeBrows.ListIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorScars.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("scars", 0, btnSelectorScars.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("scars", 1, btnSelectorScars.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorSpots.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("spots", 0, btnSelectorSpots.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("spots", 1, btnSelectorSpots.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorDisc.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("disc", 0, btnSelectorDisc.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("disc", 1, btnSelectorDisc.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorComplex.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("complex", 0, btnSelectorComplex.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("complex", 1, btnSelectorComplex.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorAcne.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("acne", 0, btnSelectorAcne.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("acne", 1, btnSelectorAcne.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorAgeing.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("ageing", 0, btnSelectorAgeing.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("ageing", 1, btnSelectorAgeing.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorMoles.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("moles", 0, btnSelectorMoles.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("moles", 1, btnSelectorMoles.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorFreckles.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("freckles", 0, btnSelectorFreckles.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("freckles", 1, btnSelectorFreckles.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorGrime.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("grime", 0, btnSelectorGrime.ListIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("grime", 1, btnSelectorGrime.ListIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorLipsticks.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("lipsticks", 0, btnSelectorLipsticks.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("lipsticks", 1, btnSelectorLipsticks.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
            if (btnSelectorShadows.ListIndex == 0)
            {
                CreatePlayer.ToggleOverlayChange("shadows", 0, btnSelectorShadows.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
            else
            {
                CreatePlayer.ToggleOverlayChange("shadows", 1, btnSelectorShadows.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
        }

        private static void UpdateLists(int index, UkData[] sex)
        {
            btnSelectorFace.ListItems.Clear();
            btnSelectorTorso.ListItems.Clear();
            btnSelectorLegs.ListItems.Clear();

            //Faces
            for (int i = 1; i <= sex[index].Heads.Count(); i++)
            {
                btnSelectorFace.ListItems.Add("Visage #" + i);
            }
            btnSelectorFace.ListIndex = 0;

            //Torso
            for (int i = 1; i <= sex[index].Body.Count(); i++)
            {
                btnSelectorTorso.ListItems.Add("Torse #" + i);
            }
            btnSelectorTorso.ListIndex = 0;

            //Legs
            for (int i = 1; i <= sex[index].Legs.Count(); i++)
            {
                btnSelectorLegs.ListItems.Add("Jambes #" + i);
            }
            btnSelectorLegs.ListIndex = 0;

            CreatePlayer.SetPlayerModelComponent(sex[index].Heads[0], "HeadType");
            CreatePlayer.SetPlayerModelComponent(sex[index].Body[0], "BodyType");
            CreatePlayer.SetPlayerModelComponent(sex[index].Legs[0], "LegsType");

            CreatePlayer.texture_types["albedo"] = Util.GetHashKey(sex[index].HeadTexture);
            CreatePlayer.skinPlayer["albedo"] = Util.GetHashKey(sex[index].HeadTexture);
            ReloadTextures();
        } 
    }
}
