using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended.External;
using Newtonsoft.Json;
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

            GameMode.RegisterTickHandler(OnTick);
            ShowUI(false);
        }

        private static Task OnTick()
        {
            

            return Task.FromResult(0);
        }

        public static void UpdateUI(string stringJson)
        {
            API.SendNuiMessage(stringJson);
        }

        public static void ShowUI(bool active)
        {
            Show = active;

            string jsonpost = JsonConvert.SerializeObject(new UiDataSend() { type = "ui", action = (active ? "show" : "hide") });

            API.SendNuiMessage(jsonpost);
        }
    }
}
