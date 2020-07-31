using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ClientExtended;
using ClientExtended.External;
using ClientExtented;
using Shared;
using Client.Utils;

namespace Client
{
    public class CreatePlayer : BaseScript
    {
        #region Fields
        bool isSelectSexActive = false;
        public static bool isInCharCreation = false;
        public static string model_selected;
        string model_f = "mp_female";
        string model_m = "mp_male";
        Ped PedFemale;
        Ped PedMale;
        public static int Camera;
        public static int Camera_Male;
        public static int Camera_Female;
        public static int Camera_Editor;
        public static int Camera_FaceEditor;
        public static int Camera_Waist;
        public static int Camera_Legs;
        public static int Camera_Body;
        float DressHeading = 93.2f;
        public static Identite Identity;

        //Para guardar en DB
        public static Dictionary<string, object> skinPlayer = new Dictionary<string, object>() {
            { "sex", "none" },

            { "HeadType", 0 }, 
            { "BodyType", 0 },
            { "LegsType", 0 },

            { "HeadSize", 0.0f },

            { "EyeBrowH", 0.0f },
            { "EyeBrowW", 0.0f },
            { "EyeBrowD", 0.0f },

            { "EarsH", 0.0f },
            { "EarsW", 0.0f },
            { "EarsD", 0.0f },
            { "EarsL", 0.0f },

            { "EyeLidH", 0.0f },
            { "EyeLidW", 0.0f },

            { "EyeD", 0.0f },
            { "EyeAng", 0.0f },
            { "EyeDis", 0.0f },
            { "EyeH", 0.0f },

            { "NoseW", 0.0f },
            { "NoseS", 0.0f },
            { "NoseH", 0.0f },
            { "NoseAng", 0.0f },
            { "NoseC", 0.0f },
            { "NoseDis", 0.0f },

            { "CheekBonesH", 0.0f },
            { "CheekBonesW", 0.0f },
            { "CheekBonesD", 0.0f },

            { "MouthW", 0.0f },
            { "MouthD", 0.0f },
            { "MouthX", 0.0f },
            { "MouthY", 0.0f },

            { "ULiphH", 0.0f },
            { "ULiphW", 0.0f },
            { "ULiphD", 0.0f },

            { "LLiphH", 0.0f },
            { "LLiphW", 0.0f },
            { "LLiphD", 0.0f },

            { "JawH", 0.0f },
            { "JawW", 0.0f },
            { "JawD", 0.0f },

            { "ChinH", 0.0f },
            { "ChinW", 0.0f },
            { "ChinD", 0.0f },

            { "Beard", 0 },

            { "Hair", 0 },

            { "Body", 0 },

            { "Waist", 0 },

            { "Eyes", 0 },

            { "Scale", 1.0f },

            //New DB
            { "eyebrows_visibility", 0 },
            { "eyebrows_tx_id", 0 },

            { "scars_visibility", 0 },
            { "scars_tx_id", 0 },

            { "spots_visibility", 0 },
            { "spots_tx_id", 0 },

            { "disc_visibility", 0 },
            { "disc_tx_id", 0 },

            { "complex_visibility", 0 },
            { "complex_tx_id", 0 },

            { "acne_visibility", 0 },
            { "acne_tx_id", 0 },

            { "ageing_visibility", 0 },
            { "ageing_tx_id", 0 },

            { "freckles_visibility", 0 },
            { "freckles_tx_id", 0 },

            { "moles_visibility", 0 },
            { "moles_tx_id", 0 },

            { "grime_visibility", 0 },
            { "grime_tx_id", 0 },

            { "lipsticks_visibility", 0 },
            { "lipsticks_tx_id", 0 },
            { "lipsticks_palette_id", 0 },
            { "lipsticks_palette_color_primary", 0 },

            { "shadows_visibility", 0 },
            { "shadows_tx_id", 0 },
            { "shadows_palette_id", 0 },
            { "shadows_palette_color_primary", 0 },
        };

        //Para guardar en DB
        public static Dictionary<string, object> clothesPlayer = new Dictionary<string, object>() {
            { "Hat", -1 },
            { "EyeWear", -1 },
            { "Mask", -1},
            { "NeckWear", -1 },
            { "NeckTies", -1 },
            { "Shirt", -1 },
            { "Suspender", -1 },
            { "Vest", -1 },
            { "Coat", -1 },
            { "Poncho", -1 },
            { "Cloak", -1 },
            { "Glove", -1 },
            { "RingRh", -1 },
            { "RingLh", -1 },
            { "Bracelet", -1 },
            { "Gunbelt", -1 },
            { "Belt", -1 },
            { "Buckle", -1 },
            { "Holster", -1 },
            { "Pant", -1 },
            { "Skirt", -1 },
            { "Chap", -1 },
            { "Boots", -1 },
            { "Spurs", -1 },
        };

        public static int indexCamera = 1;
        public static int textureId = -1;
        public static float overlay_opacity = 1.0f;
        public static bool is_overlay_change_active = false;

        public static Dictionary<string, dynamic> texture_types = new Dictionary<string, dynamic>();

        #endregion

        #region C4TOR
        public CreatePlayer()
        {
            EventHandlers["djoe:createPlayer"] += new Action(StartCreation);
            EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop); 
        }
        #endregion

        private async void StartCreation()
        {
            API.NetworkStartSoloTutorialSession();

            var charCreatorPos = new Vector3(-563.1345f, -3775.811f, 237.60f);
            API.RequestCollisionAtCoord(charCreatorPos.X, charCreatorPos.Y, charCreatorPos.Z);

            await Delay(500);

            Game.PlayerPed.PositionNoOffset = charCreatorPos;

            await Delay(100);

            Game.PlayerPed.IsPositionFrozen = true;

            var intdataMale = new Interior(new Vector3(-561.8157f, -3780.966f, 239.0805f), "mp_char_male_mirror");
            var intdataFemale = new Interior(new Vector3(-561.8157f, -3780.966f, 239.0805f), "mp_char_female_mirror");

            intdataFemale.Active = true;
            intdataMale.Active = true;
            
            Function.Call((Hash)0x1E5B70E53DB661E5, 1122662550, 347053089, 0, "RDRP", "Chargement...", "Lancement de la création du personnage.");

            await Delay(500);

            Function.Call(Hash._REQUEST_IMAP, 183712523);
            Function.Call(Hash._REQUEST_IMAP, -1699673416);
            Function.Call(Hash._REQUEST_IMAP, 1679934574);

            await Delay(500);

            CreateCams();

            await Delay(100);

            await CreationSelectPeds();

            await Delay(500);

            API.ShutdownLoadingScreen();
            API.DoScreenFadeIn(500);

            API.DisplayHud(false);
            API.DisplayRadar(false);

            Function.Call((Hash)0xBE83CAE8ED77A94F, Game.GenerateHash("sunny"));
            Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "Online_Character_Editor");
            World.CurrentTime = new TimeSpan(12, 0, 0);
            Game.IsCinematicModeEnabled = true;

            Function.Call(Hash._CREATE_ANIM_SCENE, "lightrig@online_character_editor", 0, 0, false, true);

            API.SetCamActive(Camera, true);
            API.RenderScriptCams(true, true, 1000, true, true, 0);

            isSelectSexActive = true;

            Tick += OnTick;
        }

        public static void ToggleOverlayChange(string name, int visibility, int tx_id, int tx_normal, int tx_material, int tx_color_type, float tx_opacity, int tx_unk, int palette_id, int palette_color_primary, int palette_color_secondary, int palette_color_tertiary, int var, float opacity)
        {
            for (int i = 0; i < SkinsUtils.overlay_all_layers.Count(); i++)
            {
                if (SkinsUtils.overlay_all_layers[i]["name"].ToString().Equals(name))
                {
                    skinPlayer[$"{name}_visibility"] = visibility;
                    skinPlayer[$"{name}_tx_id"] = tx_id;

                    if (name.Contains("shadows") || name.Contains("lipsticks"))
                    {
                        skinPlayer[$"{name}_palette_id"] = palette_id;
                        skinPlayer[$"{name}_palette_color_primary"] = palette_color_primary;
                    }

                    SkinsUtils.overlay_all_layers[i]["visibility"] = visibility;
                    if (visibility != 0)
                    {
                        SkinsUtils.overlay_all_layers[i]["tx_normal"] = tx_normal;
                        SkinsUtils.overlay_all_layers[i]["tx_material"] = tx_material;
                        SkinsUtils.overlay_all_layers[i]["tx_color_type"] = tx_color_type;
                        SkinsUtils.overlay_all_layers[i]["tx_opacity"] = tx_opacity;
                        SkinsUtils.overlay_all_layers[i]["tx_unk"] = tx_unk;
                        if (tx_color_type == 0)
                        {
                            SkinsUtils.overlay_all_layers[i]["palette"] = SkinsUtils.COLOR_PALETTES[(int)palette_id];
                            SkinsUtils.overlay_all_layers[i]["palette_color_primary"] = palette_color_primary;
                            SkinsUtils.overlay_all_layers[i]["palette_color_secondary"] = palette_color_secondary;
                            SkinsUtils.overlay_all_layers[i]["palette_color_tertiary"] = palette_color_tertiary;
                        }
                        if (name.Equals("shadows") || name.Equals("eyeliners") || name.Equals("lipsticks"))
                        {
                            SkinsUtils.overlay_all_layers[i]["var"] = var;
                            SkinsUtils.overlay_all_layers[i]["tx_id"] = (int)SkinsUtils.overlays_info[name][0]["id"];
                        }
                        else
                        {
                            SkinsUtils.overlay_all_layers[i]["var"] = 0;
                            SkinsUtils.overlay_all_layers[i]["tx_id"] = (int)SkinsUtils.overlays_info[name][(int)tx_id]["id"];
                        }
                        SkinsUtils.overlay_all_layers[i]["opacity"] = opacity;
                    }
                }
            }
            ChangeOverlays();
        }

        public static async void ChangeOverlays()
        {
            int ped = API.PlayerPedId();
            if (textureId != -1)
            {
                Function.Call((Hash)0xB63B9178D0F58D82, textureId);
                Function.Call((Hash)0x6BEFAA907B076859, textureId);
            }

            textureId = Function.Call<int>((Hash)0xC5E7204F322E49EB, texture_types["albedo"], texture_types["normal"], texture_types["material"]);
            

            foreach (Dictionary<string, dynamic> layer in SkinsUtils.overlay_all_layers)
            {
                if (layer["visibility"] != 0)
                {
                    int overlay_id = Function.Call<int>((Hash)0x86BB5FF45F193A02, textureId, layer["tx_id"], layer["tx_normal"], layer["tx_material"], layer["tx_color_type"], layer["tx_opacity"], layer["tx_unk"]);
                    if (layer["tx_color_type"] == 0)
                    {
                        Function.Call((Hash)0x1ED8588524AC9BE1, textureId, overlay_id, layer["palette"]);
                        Function.Call((Hash)0x2DF59FFE6FFD6044, textureId, overlay_id, layer["palette_color_primary"], layer["palette_color_secondary"], layer["palette_color_tertiary"]);
                    }
                    Function.Call((Hash)0x3329AAE2882FC8E4, textureId, overlay_id, layer["var"]);
                    Function.Call((Hash)0x6C76BC24F8BB709A, textureId, overlay_id, layer["opacity"]);
                }
            }

            while(!Function.Call<bool>((Hash)0x31DC8D3F216D8509, textureId))
            {
                await Delay(0);
            }
            
            Function.Call<bool>((Hash)0x0B46E25761519058, ped, Util.GetHashKey("heads"), textureId);
            Function.Call<bool>((Hash)0x92DAABA2C1C10B0E, textureId);
            Function.Call<bool>((Hash)0xCC8CA3E88256E58F, ped, 0, 1, 1, 1, false);
        }

        public static void ChangeScale(float scale)
        {
            skinPlayer["Scale"] = scale;
            Function.Call((Hash)0x25ACFC650B65C538, Game.PlayerPed.Handle, scale);
        }

        public static uint FromHex(string value)
        {
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            return (uint)Int32.Parse(value, NumberStyles.HexNumber);
        }

        public static void SetPlayerComponent(int _newIndex, uint category, string idlist, List<uint> male_components, List<uint> female_components)
        {
            int pPID = API.PlayerPedId();
            if (model_selected == "mp_male")
            {
                if (_newIndex == 0)
                {
                    //Coats is a really shit
                    if (category == 0xE06D30CE)
                    {
                        Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x662AC34, 0);
                    }
                    //end
                    Function.Call((Hash)0xD710A5007C2AC539, pPID, category, 0);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                    clothesPlayer[idlist] = -1;
                }
                else
                {
                    //Coats is a really shit
                    if (category == 0xE06D30CE) {
                        Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x662AC34, 0);
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                    }
                    //end
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, male_components[_newIndex - 1], true, true, false);
                    clothesPlayer[idlist] = male_components[_newIndex - 1];
                }
            }
            else
            {
                if (_newIndex == 0)
                {
                    Function.Call((Hash)0xD710A5007C2AC539, pPID, category, 0);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                    clothesPlayer[idlist] = -1;
                }
                else
                {
                    //Miscellanea.LoadModel(male_components[_newIndex - 1]);
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, female_components[_newIndex - 1], true, true, true);
                    clothesPlayer[idlist] = female_components[_newIndex - 1];
                }
            }
            //Fix Hair
            Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x864B03AE, 0);
            Function.Call((Hash)0xD3A7B003ED343FD9, pPID, ConvertValue(skinPlayer["Hair"].ToString()), true, true, true);
            Function.Call((Hash)0xD3A7B003ED343FD9, pPID, ConvertValue(skinPlayer["Beard"].ToString()), true, true, true);
            //end
            Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }

        public static void SetPlayerModelComponent(uint hex, string skinP)
        {
            Function.Call((Hash)0xD3A7B003ED343FD9, Game.PlayerPed.Handle, hex, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, Game.PlayerPed.Handle, 0, 1, 1, 1, false);
            skinPlayer[skinP] = hex;
        }

        public static void SetPlayerModelListComps(string skinP, uint comp, uint category)
        {
            if (comp == 0)
            {
                Function.Call((Hash)0xD710A5007C2AC539, Game.PlayerPed.Handle, category, 0);
                skinPlayer[skinP] = -1;
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, Game.PlayerPed.Handle, comp, true, true, true);
                skinPlayer[skinP] = comp;
            }


            Function.Call((Hash)0xCC8CA3E88256E58F, Game.PlayerPed.Handle, 0, 1, 1, 1, false);
        }
        public static void SetPlayerBodyComponent(uint comp, string skinP)
        {
            Function.Call((Hash)0x1902C4CFCC5BE57C, Game.PlayerPed.Handle, comp);
            Function.Call((Hash)0xCC8CA3E88256E58F, Game.PlayerPed.Handle, 0, 1, 1, 1, false);
            skinPlayer[skinP] = comp;
        }

        public static void SetPlayerFaceBlend(int item, int index)
        {
            int pPID = API.PlayerPedId();
            float _sizeValue = (float)index;
            _sizeValue = _sizeValue / 10.0f;

            switch (item)
            {
                case 0:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x84D6, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["HeadSize"] = _sizeValue;
                    break;
                case 1:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x3303, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeBrowH"] = _sizeValue;
                    break;
                case 2:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x2FF9, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeBrowW"] = _sizeValue;
                    break;
                case 3:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x4AD1, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeBrowD"] = _sizeValue;
                    break;
                case 4:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xC04F, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsH"] = _sizeValue;
                    break;
                case 5:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xB6CE, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsW"] = _sizeValue;
                    break;
                case 6:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x2844, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsD"] = _sizeValue;
                    break;
                case 7:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xED30, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsL"] = _sizeValue;
                    break;
                case 8:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x8B2B, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeLidH"] = _sizeValue;
                    break;
                case 9:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x1B6B, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeLidW"] = _sizeValue;
                    break;
                case 10:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xEE44, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeD"] = _sizeValue;
                    break;
                case 11:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xD266, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeAng"] = _sizeValue;
                    break;
                case 12:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xA54E, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeDis"] = _sizeValue;
                    break;
                case 13:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xDDFB, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeH"] = _sizeValue;
                    break;
                case 14:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x6E7F, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseW"] = _sizeValue;
                    break;
                case 15:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x3471, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseS"] = _sizeValue;
                    break;
                case 16:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x03F5, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseH"] = _sizeValue;
                    break;
                case 17:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x34B1, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseAng"] = _sizeValue;
                    break;
                case 18:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xF156, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseC"] = _sizeValue;
                    break;
                case 19:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x561E, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseDis"] = _sizeValue;
                    break;
                case 20:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x6A0B, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["CheekBonesH"] = _sizeValue;
                    break;
                case 21:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xABCF, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["CheekBonesW"] = _sizeValue;
                    break;
                case 22:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x358D, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["CheekBonesD"] = _sizeValue;
                    break;
                case 23:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xF065, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthW"] = _sizeValue;
                    break;
                case 24:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xAA69, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthD"] = _sizeValue;
                    break;
                case 25:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x7AC3, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthX"] = _sizeValue;
                    break;
                case 26:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x410D, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthY"] = _sizeValue;
                    break;
                case 27:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x1A00, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ULiphH"] = _sizeValue;
                    break;
                case 28:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x91C1, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ULiphW"] = _sizeValue;
                    break;
                case 29:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xC375, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ULiphD"] = _sizeValue;
                    break;
                case 30:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xBB4D, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["LLiphH"] = _sizeValue;
                    break;
                case 31:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xB0B0, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["LLiphW"] = _sizeValue;
                    break;
                case 32:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x5D16, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["LLiphD"] = _sizeValue;
                    break;
                case 33:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x8D0A, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["JawH"] = _sizeValue;
                    break;
                case 34:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xEBAE, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["JawW"] = _sizeValue;
                    break;
                case 35:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x1DF6, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["JawD"] = _sizeValue;
                    break;
                case 36:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x3C0F, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ChinH"] = _sizeValue;
                    break;
                case 37:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xC3B2, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ChinW"] = _sizeValue;
                    break;
                case 38:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xE323, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ChinD"] = _sizeValue;
                    break;
            }
        }

        public static void SaveChanges()
        {
            TriggerServerEvent("djoe:SaveSkinDB", JsonConvert.SerializeObject(skinPlayer), JsonConvert.SerializeObject(clothesPlayer), JsonConvert.SerializeObject(Identity));
            isInCharCreation = false;
            DeleteAll();
        }

        private void CreateCams()
        {
            Camera = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.83f, -3776.33f, 239.58f, -13.56231f, 0.00f, -91.93626f, 45.00f, false, 0);
            Camera_Male = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.6671f, -3775.44f, 239.4266f, -9.622695f, 0.0f, -86.08074f, 45.00f, false, 0);
            Camera_Female = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.8455f, -3776.596f, 239.4435f, -13.41718f, 0.0f, -88.04576f, 45.00f, false, 0);
            Camera_Editor = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.1333f, -3780.923f, 239.4437f, -11.32719f, 0.0f, -90.96693f, 45.00f, false, 0);
            Camera_FaceEditor = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -558.9781f, -3780.955f, 239.186f, 6.591177f, 0.0f, -92.76723f, 40.00f, false, 0);
            Camera_Waist = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.1779f, -3780.964f, 238.4654f, -0.6631846f, 0.0f, -91.76698f, 40.00f, false, 0);
            Camera_Legs = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.2103f, -3781.039f, 238.4678f, -42.50001f, 0.0f, -89.2997f, 40.00f, false, 0);
            Camera_Body = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.6195f, -3780.708f, 239.1954f, -15.75687f, 0.0f, -89.49976f, 40.00f, false, 0);
            //Camera_Back = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -563.0956f, -3780.669f, 238.465f, 0.906957f, 0.0f, -89.36639f, 40.00f, false, 0);
        }

        private static void DeleteAll()
        {
            API.SetCamActive(Camera, false);
            API.DestroyCam(Camera, true);
            API.SetCamActive(Camera_Male, false);
            API.DestroyCam(Camera_Male, true);
            API.SetCamActive(Camera_Female, false);
            API.DestroyCam(Camera_Female, true);
            API.SetCamActive(Camera_Editor, false);
            API.DestroyCam(Camera_Editor, true);
            API.SetCamActive(Camera_FaceEditor, false);
            API.DestroyCam(Camera_FaceEditor, true);
            API.SetCamActive(Camera_Waist, false);
            API.DestroyCam(Camera_Waist, true);
            API.SetCamActive(Camera_Legs, false);
            API.DestroyCam(Camera_Legs, true);
            API.SetCamActive(Camera_Body, false);
            API.DestroyCam(Camera_Body, true);
            API.RenderScriptCams(false, true, 5000, true, true, 0);
        }

        private async Task CreationSelectPeds()
        {
            var pedFemaleModel = new Model(model_f);
            var pedMaleModel = new Model(model_m);

            await pedFemaleModel.Request(1000);
            await pedMaleModel.Request(1000);

            PedFemale = await World.CreatePed(pedFemaleModel, new Vector3(-558.43f, -3776.65f, 237.7f), 93.2f, false, true);
            PedMale = await World.CreatePed(pedMaleModel, new Vector3(-558.52f, -3775.6f, 237.7f), 93.2f, false, true);

            Function.Call((Hash)0x283978A15512B2FE, PedFemale.Handle, true);
            Function.Call((Hash)0x283978A15512B2FE, PedMale.Handle, true);

            API.SetPedOutfitPreset(PedFemale.Handle, 2, 0);
            API.SetPedOutfitPreset(PedMale.Handle, 4, 0);

            PedFemale.IsPositionFrozen = true;
            PedMale.IsPositionFrozen = true;
        }

        private async void CreationSexPed(string model, int camedit)
        {
            model_selected = model;
            skinPlayer["sex"] = model;
            

            if (model_selected == model_m)
            {
                skinPlayer["albedo"] = Util.GetHashKey("mp_head_mr1_sc08_c0_000_ab");
                texture_types["albedo"] = Util.GetHashKey("mp_head_mr1_sc08_c0_000_ab");
                texture_types["normal"] = Util.GetHashKey("mp_head_mr1_000_nm");
                texture_types["material"] = 0x7FC5B1E1;
                texture_types["color_type"] = 1;
                texture_types["texture_opacity"] = 1.0f;
                texture_types["unk_arg"] = 0;
            }
            else
            {
                skinPlayer["albedo"] = Util.GetHashKey("mp_head_fr1_sc08_c0_000_ab");
                texture_types["albedo"] = Util.GetHashKey("mp_head_fr1_sc08_c0_000_ab");
                texture_types["normal"] = Util.GetHashKey("head_fr1_mp_002_nm");
                texture_types["material"] = 0x7FC5B1E1;
                texture_types["color_type"] = 1;
                texture_types["texture_opacity"] = 1.0f;
                texture_types["unk_arg"] = 0;
            }

            Menus.MainMenuCreator.GetMenu();

            await Game.Player.ChangeModel(model);
            Function.Call((Hash)0x283978A15512B2FE, Game.PlayerPed.Handle, true);

            Game.PlayerPed.SetEntityCoordsAndHeading(new Vector3(-558.3258f, -3781.111f, 237.60f), 93.2f);
            Game.PlayerPed.IsPositionFrozen = true;

            
            API.RenderScriptCams(false, true, 3000, true, true, 0);
            await Delay(2500);
            API.SetCamActive(Camera_Editor, true);
            API.RenderScriptCams(true, true, 1000, true, true, 0);

            PedFemale.Delete();
            PedMale.Delete();

            isInCharCreation = true;
            Menus.MainMenuCreator.GetMenu().OpenMenu();

            //MenuCreateCharacter(model);
        }

        public static void SwapCameras(int index)
        {
            switch (index)
            {
                case 0:
                    API.SetCamActive(Camera_Editor, true);
                    API.SetCamActive(Camera_Body, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 1:
                    API.SetCamActive(Camera_Body, true);
                    API.SetCamActive(Camera_Editor, false);
                    API.SetCamActive(Camera_FaceEditor, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 2:
                    API.SetCamActive(Camera_FaceEditor, true);
                    API.SetCamActive(Camera_Body, false);
                    API.SetCamActive(Camera_Waist, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 3:
                    API.SetCamActive(Camera_Waist, true);
                    API.SetCamActive(Camera_FaceEditor, false);
                    API.SetCamActive(Camera_Legs, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 4:
                    API.SetCamActive(Camera_Legs, true);
                    API.SetCamActive(Camera_Waist, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 5:
                    API.SetCamActive(Camera_Legs, false);
                    API.SetCamActive(Camera_Editor, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
            }
        }

        #region Ticks
        private Task OnTick()
        {     
            if (Game.IsControlJustPressed(0, ClientExtended.External.Control.FrontendRight) && isSelectSexActive) {

                if (API.IsCamActive(Camera))
                {
                    API.SetCamActiveWithInterp(Camera_Female, Camera, 500, 0, 0);
                    API.SetCamActive(Camera, false);
                }
                else if (API.IsCamActive(Camera_Male))
                {
                    API.SetCamActiveWithInterp(Camera_Female, Camera_Male, 500, 0, 0);
                    API.SetCamActive(Camera_Male, false);
                }
                else
                {
                    API.SetCamActiveWithInterp(Camera, Camera_Female, 500, 0, 0);
                    API.SetCamActive(Camera_Female, false);
                }
            }

            if (Game.IsControlJustPressed(2, ClientExtended.External.Control.FrontendLeft) && isSelectSexActive)
            {

                if (API.IsCamActive(Camera))
                {
                    API.SetCamActiveWithInterp(Camera_Male, Camera, 500, 0, 0);
                    API.SetCamActive(Camera, false);
                }
                else if (API.IsCamActive(Camera_Female))
                {
                    API.SetCamActiveWithInterp(Camera_Male, Camera_Female, 500, 0, 0);
                    API.SetCamActive(Camera_Female, false);
                }
                else
                {
                    API.SetCamActiveWithInterp(Camera, Camera_Male, 500, 0, 0);
                    API.SetCamActive(Camera_Male, false);
                }
            }

            if (Game.IsControlJustPressed(0, ClientExtended.External.Control.FrontendAccept) && isSelectSexActive)
            {         
                if (API.IsCamActive(Camera_Male))
                {
                    CreationSexPed(model_m, Camera_Male);
                    isSelectSexActive = false;
                }
                else if (API.IsCamActive(Camera_Female))
                {
                    CreationSexPed(model_f, Camera_Female);
                    isSelectSexActive = false;
                }
            }

            if (isSelectSexActive)
            {

                if (API.IsCamActive(Camera_Male))
                {
                    UIHelper.DrawText("~e~Homme", 0.5f, 0.9f, 0.7f, 0.7f, Color.FromArgb(255, 255, 255, 255), true, false, Font.Unk1);
                }
                else if (API.IsCamActive(Camera_Female))
                {
                    UIHelper.DrawText("~e~Femme", 0.5f, 0.9f, 0.7f, 0.7f, Color.FromArgb(255, 255, 255, 255), true, false, Font.Unk1);
                }
                else
                    UIHelper.DrawText("Appuyez [<-] ou [->] pour sélectionner votre sexe", 0.5f, 0.9f, 0.7f, 0.7f, Color.FromArgb(255, 255, 255, 255), true, false, Font.Unk1);

            }

            if (isInCharCreation) //Fix Run Ped
            {
                UIHelper.DrawText("Tourner <-[A] [D]-> | Caméra [W] o [S]", 0.5f, 0.9f, 0.7f, 0.7f, Color.FromArgb(255, 255, 255, 255), true, false, Font.Unk1);
                Game.PlayerPed.IsPositionFrozen = true;
                Game.PlayerPed.Tasks.ClearAll();
                API.DrawLightWithRange(-560.1646f, -3782.066f, 238.5975f, 255, 255, 255, 7.0f, 150.0f);

                if (Game.IsControlJustPressed(0, ClientExtended.External.Control.MoveUpOnly))
                {
                    indexCamera += 1;
                    if (indexCamera > 4)
                    {
                        indexCamera = 0;
                    }

                    SwapCameras(indexCamera);
                }
                if (Game.IsControlJustPressed(0, ClientExtended.External.Control.MoveDownOnly))
                {
                    indexCamera -= 1;
                    if (indexCamera < 0)
                    {
                        indexCamera = 4;
                    }

                    SwapCameras(indexCamera);
                }

                if (Game.IsControlPressed(0, ClientExtended.External.Control.MoveLeftOnly))
                {
                    DressHeading += 1.0f;
                    Game.PlayerPed.Heading = DressHeading;

                    Debug.WriteLine($"test {Game.PlayerPed.Heading}");

                }

                if (Game.IsControlPressed(0, ClientExtended.External.Control.MoveRightOnly))
                {
                    DressHeading -= 1.0f;
                    Game.PlayerPed.Heading = DressHeading;

                    Debug.WriteLine($"test {Game.PlayerPed.Heading}");
                }
            }

            return Task.FromResult(0);
        }
        #endregion

        #region Methods

        public static uint ConvertValue(string s)
        {
            uint result;

            if (uint.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                int eresante = int.Parse(s);
                result = (uint)eresante;
                return result;
            }
        }

        public static void CloseSecureMenu()
        {
            if (!MenuController.IsAnyMenuOpen())
            {
                Menus.MainMenuCreator.GetMenu().OpenMenu();
            }
        }

        public void DrawTxt(string text, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, r, g, b, alpha);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);
            if (shadow) { Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255); }
            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, 1);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
        }


        #endregion

        #region ServerEvent
        private void OnResourceStop(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) 
                return;

            isInCharCreation = false;
            DeleteAll();
            PedFemale?.Delete();
            PedMale.Delete();
        }
        #endregion
    }
}
