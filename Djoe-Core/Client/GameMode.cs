using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Client.Ui;
using Shared.Utils;
using ClientExtended;
using Client.Menus;

namespace Client
{
    public class GameMode : BaseScript
    {
        internal static GameMode Instance;

        public GameMode()
        {
            Instance = this;

            Config.LoadConfig("Config.json");

            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

        }

        private void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;

            Hud.UiCoreInit();
            Commands.Init();

            // Menus
            GeneralStore.Init();

            Tick += OnTick;
        }

        private Task OnTick()
        {
            Misc.ShowControlPressed();
            return Task.FromResult(0);
        }

        internal static void RegisterEventHandler(string name, Delegate action)
        {
            Instance.EventHandlers[name] += action;
        }

        public static void RegisterTickHandler(Func<Task> tick)
        {
            Instance.Tick += tick;
        }

        public static void DeregisterTickHandler(Func<Task> tick)
        {
            Instance.Tick -= tick;
        }

        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            Debug.WriteLine($"Registering NUI EventHandler for {msg}");
            API.RegisterNuiCallbackType(msg);

            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
            EventHandlers[$"{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
        }
    }
}
