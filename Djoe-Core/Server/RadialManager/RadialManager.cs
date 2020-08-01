using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Utils;
using System;
using System.Collections.Concurrent;

namespace Server
{
    public static class RadialManager
    {
        #region Private static properties
        private static ConcurrentDictionary<Player, RadialMenu> _clientMenus = new ConcurrentDictionary<Player, RadialMenu>();
        #endregion

        #region Constructor
        public static void Init()
        {
            GameMode.RegisterEventHandler("XMenuManager_ExecuteCallback", new Action<Player, int, string>(XMenuManager_ExecuteCallback));
            GameMode.RegisterEventHandler("XMenuManager_ClosedMenu", new Action<Player>(XMenuManager_ClosedMenu));
        }
        #endregion

        #region Sync Callback
        private static void XMenuManager_ExecuteCallback([FromSource] Player client, int menuIndex, string data)
        {/*
            if (!client.Exists)
                return;
                */
            if (_clientMenus.TryGetValue(client, out RadialMenu menu))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    RadialMenu temp = JsonConvert.DeserializeObject<RadialMenu>(data);

                    if (!string.IsNullOrEmpty(temp.Items[menuIndex]?.InputValue))
                        menu.Items[menuIndex].InputValue = temp.Items[menuIndex].InputValue;
                }

                if (menu.Items[menuIndex] != null)
                {
                    menu.Items[menuIndex].OnMenuItemCallback?.Invoke(client, menu, menu.Items[menuIndex], menuIndex, "");
                    menu.Callback?.Invoke(client, menu, menu.Items[menuIndex], menuIndex, "");
                }
            }
        }

        public static void XMenuManager_ClosedMenu([FromSource] Player client)
        {
            /*
            if (!client.Exists)
                return;
                */
            _clientMenus.TryGetValue(client, out RadialMenu menu);
            if (menu != null)
            {
                menu.Finalizer?.Invoke(client, menu);
                client.TriggerEvent("XMenuManager_CloseMenu");
            }
            else if (menu != null)
                _clientMenus.TryRemove(client, out menu);
        }
        #endregion

        #region Public static methods
        public static bool OpenMenu(Player client, RadialMenu menu)
        {
            if (menu.Items.Count == 0 || menu.Items == null) return false;
            if (menu.Items.Count > 8)
                Logger.Warn($"Warning {menu.Id} have more 8 items");
            _clientMenus.TryRemove(client, out RadialMenu oldMenu);

            if (oldMenu != null)
            {
                menu.Finalizer?.Invoke(client, menu);
                client.TriggerEvent("RadialManager_CloseMenu");
            }

            if (_clientMenus.TryAdd(client, menu))
            {
                Console.WriteLine("RadialManager_OpenMenu");
                client.TriggerEvent("RadialManager_OpenMenu", JsonConvert.SerializeObject(menu));
                return true;
            }
            return false;
        }

        public static void CloseMenu(Player client)
        {
            if (_clientMenus.TryRemove(client, out RadialMenu menu))
            {
                menu.Finalizer?.Invoke(client, menu);
                client.TriggerEvent("RadialManager_CloseMenu");
            }
        }

        internal static bool HasOpenMenu(Player client) => _clientMenus.ContainsKey(client);
        #endregion
    }
}
