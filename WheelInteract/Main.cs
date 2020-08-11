using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented;
using ClientExtented.External;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace WheelInteract
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["RadialManager_OpenMenu"] += new Action<string>(RadialManager_OpenMenu); 
            EventHandlers["RadialManager_CloseMenu"] += new Action(RadialManager_CloseMenu);
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            RegisterNUICallback("closemenu", OnCloseMenu);
        }

        private CallbackDelegate OnCloseMenu(IDictionary<string, object> datas, CallbackDelegate callback)
        {
            return callback;
        }

        private void OnClientResourceStart(string name)
        {
            if (API.GetCurrentResourceName() != name)
                return;

            API.SetNuiFocus(false, false);

            RegisterNUICallback("wheelinteract", WheelInteract_Callback);
        }

        private void RadialManager_OpenMenu(string menuData)
        {
            JObject dataObj = JObject.Parse(menuData);

            var data = new Nui()
            {
                Action = "OpenMenu",
                Data = menuData
            };
            data.SendNuiMessage();
            API.SetNuiFocus2(true, true);

            Tick += OnTick;
            //API.SetNuiFocus(true, true);
        }

        private CallbackDelegate WheelInteract_Callback(IDictionary<string, object> datas, CallbackDelegate callback)
        {
            TriggerServerEvent("RadialManager_ExecuteCallback", datas["indexSelected"], "");
            return callback;
        }

        private void RadialManager_CloseMenu()
        {
            Tick -= OnTick;
            var data = new Nui()
            {
                Action = "Activate",
                Data = false
            };
            data.SendNuiMessage();
            API.SetNuiFocus2(false, false);
        }

        private Task OnTick()
        {
            Util.DisEnableControls();

            return Task.FromResult(0);
        }

        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            Debug.WriteLine($"Registering NUI EventHandler for {msg}");
            API.RegisterNuiCallbackType(msg);
            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
            /*
            EventHandlers[msg] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });*/
        }
    }
}
