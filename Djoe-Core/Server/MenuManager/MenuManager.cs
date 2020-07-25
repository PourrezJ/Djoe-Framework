using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Utils;
using Shared.MenuManager;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server
{
    public class MenuManager :  BaseScript
    {
        #region Private static properties
        private static ConcurrentDictionary<Player, Menu> _clientMenus = new ConcurrentDictionary<Player, Menu>();
        #endregion

        #region Constructor
        public MenuManager()
        {
            EventHandlers["MenuManager_ExecuteCallback"] += new Action<Player, int, bool, bool, string, int>(MenuManager_ExecuteCallback);
            EventHandlers["MenuManager_IndexChanged"] += new Action<Player, int>(MenuManager_IndexChanged);
            EventHandlers["MenuManager_ListChanged"] += new Action<Player, int, int>(MenuManager_ListChanged);
            EventHandlers["MenuManager_BackKey"] += new Action<Player>(MenuManager_BackKey);
            EventHandlers["MenuManager_ClosedMenu"] += new Action<Player>(MenuManager_ClosedMenu);
        }

        #endregion

        #region API Event handlers
        public static void OnPlayerDisconnect(Player player)
        {
            _clientMenus.TryRemove(player, out _);
        }
        #endregion

        #region Private API triggers
        public static void MenuManager_BackKey([FromSource]Player player)
        {
            _clientMenus.TryGetValue(player, out Menu menu);

            if (menu == null)
                return;

            if (!menu.BackCloseMenu)
            {
                menu.ItemSelectCallback?.Invoke(player, menu, null, -1);
            }
            else
            {
                menu.Finalizer?.Invoke(player, menu);
                _clientMenus.TryRemove(player, out _);
            }
        }

        private static void MenuManager_ExecuteCallback([FromSource]Player player, int itemIndex, bool forced, bool checkbox, string input, int itemListIndex)
        {
            _clientMenus.TryGetValue(player, out Menu menu);

            if (menu != null)
            {
                Debug.WriteLine("Execute Callback");

                MenuItem menuItem = menu.Items[itemIndex];

                if (menuItem == null)
                    return;

                if (itemIndex >= menu.Items.Count)
                    return;

                try
                {
                    if (menuItem.Type == MenuItemType.CheckboxItem)
                        ((CheckboxItem)menuItem).Checked = checkbox;
                    else if (menuItem.Type == MenuItemType.ListItem)
                        ((ListItem)menuItem).SelectedItem = itemListIndex;
                    else if (menuItem.InputMaxLength > 0)
                        menuItem.InputValue = input;

                    
                    menu.ItemSelectCallback?.Invoke(player, menu, menuItem, itemIndex);

                    menuItem.OnMenuItemCallback?.Invoke(player, menu, menuItem, itemIndex);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        public static void MenuManager_IndexChanged([FromSource]Player player, int index)
        {
            _clientMenus.TryGetValue(player, out Menu menu);

            if (menu != null)
            {
                menu.IndexChangeCallback?.Invoke(player, menu, index, menu.Items[index]);
            }
        }

        public static void MenuManager_ListChanged([FromSource]Player player, int itemID, int itemValue)
        {
            _clientMenus.TryGetValue(player, out Menu menu);

            if (menu != null)
            {
                menu.ListItemChangeCallback?.Invoke(player, menu, (ListItem)menu.Items[itemID], itemValue);
            }
        }

        public static void MenuManager_ClosedMenu([FromSource]Player player)
        {
            Console.WriteLine("MenuManager_ClosedMenu called");
            lock (_clientMenus)
            {
                _clientMenus.TryGetValue(player, out Menu menu);

                if (menu != null)
                {
                    menu.Finalizer?.Invoke(player, menu);
                    _clientMenus.TryRemove(player, out _);
                }
            }
        }
        #endregion

        #region Public static methods
        public static void CloseMenu(Player client)
        {
            lock (_clientMenus)
            {
                if (_clientMenus.TryRemove(client, out Menu menu) && menu != null)
                {
                    menu.Finalizer?.Invoke(client, menu);
                }

                client.TriggerEvent("MenuManager_CloseMenu");
            }
        }

        public static void ForceCallback(Player client)
        {
            _clientMenus.TryGetValue(client, out Menu menu);

            if (menu != null && menu.ItemSelectCallback != null)
                client.TriggerEvent("MenuManager_ForceCallback");
        }

        public static Menu GetMenu(Player client)
        {
            _clientMenus.TryGetValue(client, out Menu menu);
            return menu;
        }

        public static bool HasOpenMenu(Player client)
        {
            return _clientMenus.ContainsKey(client);
        }

        public static bool OpenMenu(Player client, Menu menu)
        {
            if (menu.Items.Count == 0 || menu.Items == null)
                return false;

            lock (_clientMenus)
            {
                _clientMenus.TryRemove(client, out Menu oldMenu);

                if (oldMenu != null)
                {
                    oldMenu.Finalizer?.Invoke(client, menu);
                }

                if (_clientMenus.TryAdd(client, menu))
                {
                    string json = JsonConvert.SerializeObject(menu, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    client.TriggerEvent("MenuManager_OpenMenu", json);
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}