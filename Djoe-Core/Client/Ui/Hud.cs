using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Ui
{
    class UiDataSend
    {
        public string type;
        public string action;
    }

    public class Hud
    {
        public static bool Show;

        public static void UiCoreInit()
        {
            GameMode.RegisterEventHandler("djoe:updateUi", new Action<string>(UpdateUI));
            GameMode.RegisterEventHandler("djoe:showUi", new Action<bool>(ShowUI));
            GameMode.RegisterEventHandler("djoe:addMoneyUi", new Action<string>(AddMoneyUi));
            GameMode.RegisterEventHandler("djoe:removeMoneyUi", new Action<string>(RemoveMoneyUi));

            //GameMode.RegisterTickHandler(OnTick);
            Function.Call((Hash)0x8BC7C1F929D07BF3, Game.GenerateHash("HUD_CTX_PROMPT_MONEY")); // Disable money
            Function.Call((Hash)0x8BC7C1F929D07BF3, Game.GenerateHash("HUD_CTX_IN_FAST_TRAVEL_MENU")); // emoves reticle, help, feed, award massages, etc.

            ShowUI(false);
        }

        private static Task OnTick()
        {


            return Task.FromResult(0);
        }

        private static void RemoveMoneyUi(string stringJson)
        {
            API.SendNuiMessage(stringJson);
        }

        private static void AddMoneyUi(string stringJson)
        {
            API.SendNuiMessage(stringJson);
        }

        public static void UpdateUI(string stringJson)
        {
            var data = JObject.Parse(stringJson);

            data.Add("temperature_val", Math.Round(Game.PlayerPed.GetCurrentTemperatureAtThisPos(), 1));
            
            API.SendNuiMessage(data.ToString());
        }

        public static void ShowUI(bool active)
        {
            Show = active;

            if (Show)
            {
                Function.Call((Hash)0x4CC5F2FC1332577F, Game.GenerateHash("HUD_CTX_ITEM_CONSUMPTION_HEALTH"));
                Function.Call((Hash)0x4CC5F2FC1332577F, Game.GenerateHash("HUD_CTX_ITEM_CONSUMPTION_HEALTH_CORE"));
            }
            else
            {
                Function.Call((Hash)0x8BC7C1F929D07BF3, Game.GenerateHash("HUD_CTX_ITEM_CONSUMPTION_HEALTH"));
                Function.Call((Hash)0x8BC7C1F929D07BF3, Game.GenerateHash("HUD_CTX_ITEM_CONSUMPTION_HEALTH_CORE"));
            }

            string jsonpost = JsonConvert.SerializeObject(new UiDataSend() { type = "ui", action = (active ? "show" : "hide") });

            API.SendNuiMessage(jsonpost);
        }
    }
}
