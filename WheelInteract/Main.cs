using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelInteract
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["RadialManager_OpenMenu"] += new Action<string>(RadialManager_OpenMenu); 
            EventHandlers["RadialManager_CloseMenu"] += new Action(RadialManager_CloseMenu);

            RegisterNUICallback("WheelInteract_Callback", WheelInteract_Callback);
        }

        private void RadialManager_OpenMenu(string menuData)
        {
            Debug.WriteLine("djoe_radialMenu " + menuData);

            JObject dataObj = JObject.Parse(menuData);

            var data = new Nui()
            {
                Action = "OpenMenu",
                Data = menuData
            };
            data.SendNuiMessage();
            API.SetNuiFocus(true, true);
        }

        private CallbackDelegate WheelInteract_Callback(IDictionary<string, object> datas, CallbackDelegate callback)
        {
            return callback;
        }

        private void RadialManager_CloseMenu()
        {
            
        }

        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            Debug.WriteLine($"Registering NUI EventHandler for {msg}");
            API.RegisterNuiCallbackType(msg);
            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
            EventHandlers[msg] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
        }
    }
}
