using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using ClientExtended;
using ClientExtended.External;

namespace Client
{
    public class LoadPlayer
    {
        public static Dictionary<string, string> cache_skin = new Dictionary<string, string>();
        public static Dictionary<string, uint> cache_cloths = new Dictionary<string, uint>();

        public async static Task LoadAllComps(Character skin, Clothes cloths)
        {
            await Game.Player.ChangeModel(new Model(Util.GetHashKey(skin.Sex)));

            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
            await BaseScript.Delay(1000);
            int pPedID = API.PlayerPedId();

            //PreLoad TextureFace
            if (skin.Sex.Equals("mp_male"))
            {
                CreatePlayer.texture_types["albedo"] = skin.Albedo;
                CreatePlayer.texture_types["normal"] = Util.GetHashKey("mp_head_mr1_000_nm");
                CreatePlayer.texture_types["material"] = 0x7FC5B1E1;
                CreatePlayer.texture_types["color_type"] = 1;
                CreatePlayer.texture_types["texture_opacity"] = 1.0f;
                CreatePlayer.texture_types["unk_arg"] = 0;
            }
            else
            {
                CreatePlayer.texture_types["albedo"] = skin.Albedo;
                CreatePlayer.texture_types["normal"] = Util.GetHashKey("head_fr1_mp_002_nm");
                CreatePlayer.texture_types["material"] = 0x7FC5B1E1;
                CreatePlayer.texture_types["color_type"] = 1;
                CreatePlayer.texture_types["texture_opacity"] = 1.0f;
                CreatePlayer.texture_types["unk_arg"] = 0;
            }
            //End

            //LoadSkin
            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, skin.HeadType, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, skin.BodyType, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, skin.LegsType, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x84D6, skin.HeadSize);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3303, skin.EyeBrowH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x2FF9, skin.EyeBrowW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x4AD1, skin.EyeBrowD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC04F, skin.EarsH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xB6CE, skin.EarsH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x2844, skin.EarsD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xED30, skin.EarsL);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x8B2B, skin.EyeLidH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1B6B, skin.EyeLidW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xEE44, skin.EyeD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xD266, skin.EyeAng);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xA54E, skin.EyeDis);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xDDFB, skin.EyeH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x6E7F, skin.NoseW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3471, skin.NoseS);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x03F5, skin.NoseH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x34B1, skin.NoseAng);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
             await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xF156, skin.NoseC);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x561E, skin.NoseDis);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x6A0B, skin.CheekBonesH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xABCF, skin.CheekBonesW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x358D, skin.CheekBonesW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xF065, skin.MouthW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xAA69, skin.MouthD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x7AC3, skin.MouthX);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x410D, skin.MouthY);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1A00, skin.ULiphH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x91C1, skin.ULiphH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC375, skin.ULiphD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xBB4D, skin.LLiphH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xB0B0, skin.LLiphW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x5D16, skin.LLiphD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x8D0A, skin.JawH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xEBAE, skin.JawW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1DF6, skin.JawD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3C0F, skin.ChinH);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC3B2, skin.ChinW);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xE323, skin.ChinD);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);

            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, skin.Eyes, true, true, true);
           // await BaseScript.Delay(100);

            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, skin.Hair, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            
                        
            Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, skin.Body);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(100);
            

            Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, skin.Waist);

            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await BaseScript.Delay(500);

            SetPlayerComponent(skin.Sex, 0x9925C067, cloths.Hat);      
            SetPlayerComponent(skin.Sex, 0x5E47CA6, cloths.EyeWear);
            SetPlayerComponent(skin.Sex, 0x7505EF42, cloths.Mask);
            SetPlayerComponent(skin.Sex, 0x5FC29285, cloths.NeckWear);
            SetPlayerComponent(skin.Sex, 0x7A96FACA, cloths.NeckTies);
            SetPlayerComponent(skin.Sex, 0x2026C46D, cloths.Skirt);
            SetPlayerComponent(skin.Sex, 0x877A2CF7, cloths.Suspender);
            SetPlayerComponent(skin.Sex, 0x485EE834, cloths.Vest);
            SetPlayerComponent(skin.Sex, 0xE06D30CE, cloths.Coat);
            SetPlayerComponent(skin.Sex, 0xAF14310B, cloths.Poncho);
            SetPlayerComponent(skin.Sex, 0x3C1A74CD, cloths.Cloak);
            SetPlayerComponent(skin.Sex, 0xEABE0032, cloths.Glove);
            SetPlayerComponent(skin.Sex, 0x7A6BBD0B, cloths.RingRh);
            SetPlayerComponent(skin.Sex, 0xF16A1D23, cloths.RingLh);
            SetPlayerComponent(skin.Sex, 0x7BC10759, cloths.Bracelet);
            SetPlayerComponent(skin.Sex, 0x9B2C8B89, cloths.Gunbelt);
            SetPlayerComponent(skin.Sex, 0xA6D134C6, cloths.Belt);
            SetPlayerComponent(skin.Sex, 0xFAE9107F, cloths.Buckle);
            SetPlayerComponent(skin.Sex, 0xB6B6122D, cloths.Holster);
            if (cloths.Skirt != -1) // Prevents both Pant & Skirt in female ped.
            {
                SetPlayerComponent(skin.Sex, 0x1D4C528A, cloths.Pant);
            }
            SetPlayerComponent(skin.Sex, 0xA0E3AB7F, cloths.Skirt);
            SetPlayerComponent(skin.Sex, 0x3107499B, cloths.Chap);
            SetPlayerComponent(skin.Sex, 0x777EC6EF, cloths.Boots);
            SetPlayerComponent(skin.Sex, 0x18729F39, cloths.Spurs);
            
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false); // this fix Hair not appears
            
            //Load Face Texture
            CreatePlayer.ToggleOverlayChange("eyebrows", skin.Eyebrows_visibility, skin.Eyebrows_tx_id, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("scars", skin.Scars_visibility, skin.Scars_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("spots", skin.Spots_visibility, skin.Spots_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("disc", skin.Disc_visibility, skin.Disc_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("complex", skin.Complex_visibility, skin.Complex_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("acne", skin.Acne_visibility, skin.Acne_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("ageing", skin.Ageing_visibility, skin.Ageing_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("freckles", skin.Freckles_visibility, skin.Freckles_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("moles", skin.Moles_visibility, skin.Moles_tx_id, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("grime", skin.Grime_visibility, skin.Grime_tx_id, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("lipsticks", skin.Lipsticks_visibility, skin.Lipsticks_tx_id, 0, 0, 0, 1.0f, 0, skin.Lipsticks_palette_id, skin.Lipsticks_palette_color_primary, 0, 0, 0, 1.0f);
            CreatePlayer.ToggleOverlayChange("shadows", skin.Shadows_visibility, skin.Shadows_tx_id, 0, 0, 0, 1.0f, 0, skin.Shadows_palette_id, skin.Shadows_palette_color_primary, 0, 0, 0, 1.0f);
            
            await BaseScript.Delay(5000);
            Function.Call((Hash)0x59BD177A1A48600A, pPedID, 0xF8016BCA);
            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, skin.Beard, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);

            //await IsLoaded();
        }


        public static uint ConvertValue(string s)
        {
            uint result;

            if (uint.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                int interesante = int.Parse(s);
                result = (uint) interesante;
                return result;
            }
        }

        private static void SetPlayerComponent(string model, uint category, long component)
        {
            int pPID = Game.PlayerPed.Handle;
            if (model == "mp_male")
            {
                if(component != -1)
                {
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, component, true, true, false);
                }
            }
            else
            {
                Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                Function.Call((Hash)0xD3A7B003ED343FD9, pPID, component, true, true, true);                
            }

            //Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }
    }
}
