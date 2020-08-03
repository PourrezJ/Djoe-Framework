using System;
using System.Collections.Generic;
using UIMenu = MenuAPI.Menu;
using Menu = Shared.MenuManager.Menu;
using Newtonsoft.Json;
using MenuAPI;
using MenuItem = Shared.MenuManager.MenuItem;
using Shared.MenuManager;
using System.Diagnostics;
using CitizenFX.Core;
using Debug = CitizenFX.Core.Debug;
using ClientExtented;
using System.Threading.Tasks;

namespace Client
{
    public class MenuManager : BaseScript
    {
        #region Variables
        private static Menu menuData;
        private static UIMenu uiMenu;


        public static MenuBeforeOpenedDelegate BeforeOpenMenuCallback { get; set; }
        public static MenuOpenedDelegate OpenMenuCallback { get; set; }
        public static ItemCallBackDelegate ItemCallBack { get; set; }
        public static ListIndexChangeDelegate ListIndexChangeCallBack { get; set; }
        public static CheckBoxChangeDelegate CheckBoxChangeCallBack { get; set; }
        public static IndexChangeDelegate IndexChangeCallBack { get; set; }
        public static MenuClosedDelegate MenuClosedCallBack { get; set; }
        #endregion

        #region Delegates
        public delegate void MenuBeforeOpenedDelegate(Menu menu, string customData);
        public delegate void MenuOpenedDelegate(UIMenu uimenu, Menu menu, string customData);
        public delegate void ItemCallBackDelegate(Menu menu, UIMenu uimenu, MenuItem menuItem, MenuAPI.MenuItem uiMenuItem, string input = "");
        public delegate void ListIndexChangeDelegate(Menu menu, UIMenu uimenu, MenuAPI.MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex);
        public delegate void CheckBoxChangeDelegate(Menu menu, UIMenu uimenu, MenuAPI.MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState);
        public delegate void IndexChangeDelegate(Menu menu, UIMenu uimenu, MenuAPI.MenuItem oldItem, MenuAPI.MenuItem newItem, int oldIndex, int newIndex);
        public delegate void MenuClosedDelegate(Menu menu, UIMenu uimenu);
        #endregion

        #region Constructor
        public MenuManager()
        {
            EventHandlers["MenuManager_OpenMenu"] += new Action<string, string>(OpenMenuManager);
            EventHandlers["MenuManager_CloseMenu"] += new Action(CloseMenu);
        }
        #endregion

        #region Menu
        private static void OpenMenuManager(string data, string customData = "")
        {
            Debug.WriteLine(data);

            menuData = JsonConvert.DeserializeObject<Menu>(data, new MenuConverter(typeof(Menu)));

            BeforeOpenMenuCallback?.Invoke(menuData, customData);

            if (uiMenu != null)
            {
                uiMenu.CloseMenu();
                uiMenu = null;
            }

            uiMenu = new UIMenu(!string.IsNullOrEmpty(menuData.Title) ? menuData.Title : " ", menuData.SubTitle);

            OpenMenuCallback?.Invoke(uiMenu, menuData, customData);

            if (menuData.BannerSprite != null)
                uiMenu.HeaderTexture = new KeyValuePair<string, string>(menuData.BannerSprite.Dict, menuData.BannerSprite.Name);
            else
            {
                uiMenu.HeaderTexture = new KeyValuePair<string, string>("generic_textures", "menu_header_1a");
            }


            uiMenu.OnMenuClose += (uimenu) =>
            {
                Debug.WriteLine("OnMenuClose event called");
                MenuClosedCallBack?.Invoke(menuData, uiMenu);

                if (menuData != null)
                    return;

                if (!menuData.BackCloseMenu)
                {
                    //uiMenu.Visible = true;
                    TriggerServerEvent("MenuManager_BackKey");

                }
                else if (CanClose())
                {
                    CloseMenu();
                }
            };

            for (int i = 0; i < menuData.Items.Count; i++)
            {
                MenuAPI.MenuItem menuItem = new MenuAPI.MenuItem(menuData.Items[i]?.Text ?? "", menuData.Items[i]?.Description ?? "");

                if (menuData.Items[i].Type == MenuItemType.MenuItem || menuData.Items[i].Type == MenuItemType.ColoredItem)
                {
                    /*
                    if (menuData.Items[i].RightBadge != null)
                        menuItem.SetRightBadge(menuData.Items[i].RightBadge.Value);

                    if (menuData.Items[i].LeftBadge != null)
                        menuItem.SetLeftBadge(menuData.Items[i].RightBadge.Value);

                    if (menuData.Items[i].RightLabel != null)
                        menuItem.SetRightLabel(menuData.Items[i].RightLabel);*/
                }

                else if (menuData.Items[i].Type == MenuItemType.CheckboxItem)
                {
                    menuItem = new MenuCheckboxItem(menuData.Items[i].Text, menuData.Items[i].Description, ((CheckboxItem)menuData.Items[i]).Checked);
                }
                else if (menuData.Items[i].Type == MenuItemType.ListItem)
                {
                    menuItem = new MenuListItem(menuData.Items[i].Text, ((ListItem)menuData.Items[i]).Items, ((ListItem)menuData.Items[i]).SelectedItem, menuData.Items[i].Description);
                }

                if (menuItem != null)
                    uiMenu.AddMenuItem(menuItem);
            }

            uiMenu.OnCheckboxChange += (UIMenu menu, MenuAPI.MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState) => 
            {
                CheckBoxChangeCallBack?.Invoke(menuData, menu, menuItem, itemIndex, newCheckedState);
                ((CheckboxItem)menuData.Items[itemIndex]).Checked = newCheckedState;
                TriggerServerEvent("MenuManager_ExecuteCallback", itemIndex, false, newCheckedState, "", 0);
            };

            uiMenu.OnListIndexChange += (UIMenu menu, MenuAPI.MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex) =>
            {
                ListIndexChangeCallBack?.Invoke(menuData, menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex);

                ((ListItem)menuData.Items[itemIndex]).SelectedItem = itemIndex;

                if (((ListItem)menuData.Items[itemIndex]).ExecuteCallbackListChange)
                {
                    TriggerServerEvent("MenuManager_ListChanged", itemIndex, itemIndex);
                }
            };

            uiMenu.OnListItemSelect += (UIMenu menu, MenuListItem listItem, int selectedIndex, int itemIndex) =>
            {
                OnItemSelect(menu, listItem, itemIndex, selectedIndex);
            };

            uiMenu.OnItemSelect += (sender, uiitem, index) =>
            {
                Debug.WriteLine("MenuManager_ExecuteCallback");
                OnItemSelect(sender, uiitem, index);
            };


            uiMenu.OnIndexChange += (MenuAPI.Menu menu, MenuAPI.MenuItem oldItem, MenuAPI.MenuItem newItem, int oldIndex, int newIndex) =>
            {
                IndexChangeCallBack?.Invoke(menuData, menu, oldItem, newItem, oldIndex, newIndex);
                TriggerServerEvent("MenuManager_IndexChanged", newIndex);
            };

            //uiMenu.MouseControlsEnabled = false;
            //uiMenu.MouseEdgeEnabled = false;
            //uiMenu.ControlDisablingEnabled = false;
            MenuController.AddMenu(uiMenu);
            uiMenu.OpenMenu();
        }
        #endregion

        #region Methods
        private static void OnItemSelect(UIMenu uimenu, MenuAPI.MenuItem uiitem, int index, int selectedIndex = 0)
        {
            MenuItem menuItem = menuData.Items[index];

            if (menuItem == null)
                return;

            try
            {
                if (menuItem.InputMaxLength > 0)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        var input = await Inputbox.GetUserInput(menuItem.InputValue, menuItem.InputMaxLength.GetValueOrDefault(22));
                        bool valid = false;
                        if (menuItem.InputType == InputType.Number && long.TryParse(input, out long intValue))
                            valid = true;
                        else if (menuItem.InputType == InputType.UNumber && ulong.TryParse(input, out ulong uintValue))
                            valid = true;
                        else if (menuItem.InputType == InputType.Float && double.TryParse(input, out double doubleValue))
                            valid = true;
                        else if (menuItem.InputType == InputType.UFloat && double.TryParse(input, out double udoubleValue) && udoubleValue >= 0)
                            valid = true;
                        else
                            valid = true;

                        if (!valid && menuItem.InputErrorResetValue.HasValue && menuItem.InputErrorResetValue.Value)
                        {
                            menuItem.InputValue = "";
                        }
                        else if (valid)
                        {
                            Debug.WriteLine("Input Valide");
                            menuItem.InputValue = input;

                            if (menuItem.InputSetRightLabel)
                            {
                                /*
                                menuItem.RightLabel = input;
                                uiMenu.MenuItems[index].SetRightLabel(input);*/
                            }
                        }

                        uiMenu.Visible = true;

                        if (!valid)
                            return;


                        ItemCallBack?.Invoke(menuData, uiMenu, menuItem, uiitem, input);

                        TriggerServerEvent("MenuManager_ExecuteCallback", index, false, false, input, selectedIndex);
                    });
                }
                else
                {
                    TriggerServerEvent("MenuManager_ExecuteCallback", index, false, false, "", selectedIndex);

                    ItemCallBack?.Invoke(menuData, uiMenu, menuItem, uiitem);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private static bool CanClose()
        {
            if (menuData == null)
                return true;
            else
                //return !menuData.NoExit;
                return false;
        }

        public static void CloseMenu()
        {
            TriggerServerEvent("MenuManager_ClosedMenu");
            MenuController.CloseAllMenus();
            uiMenu = null;
            menuData = null;
        }

        /*
        public static int GetIndexOfMenuItem(MenuAPI.MenuItem item)
        {
            for (int i = 0; i < menuData.Items.Count; i++)
            {
                Debug.WriteLine($"{menuData.Items[i].Text} {item.Text}");
                if (menuData.Items[i].Text == item.Text)
                    return i;
            }

            return -1;
        }*/

        /*
        public PointF ConvertAnchorPos(float x, float y, MenuAnchor anchor, float xOffset, float yOffset)
        {
            var res = UIMenu.GetScreenResolutionMaintainRatio();
            //var res = getScreenResolution();
            switch (anchor)
            {
                case MenuAnchor.TopLeft:
                    return new PointF(x, y);
                case MenuAnchor.TopCenter:
                    return new PointF(res.Width / 2 + x, 0 + y);
                case MenuAnchor.TopRight:
                    return new PointF(res.Width - x - xOffset, y);
                case MenuAnchor.MiddleLeft:
                    return new PointF(x, res.Height / 2 + y - yOffset / 2);
                case MenuAnchor.MiddleCenter:
                    return new PointF(res.Width / 2 + x, res.Height / 2 + y - yOffset / 2);
                case MenuAnchor.MiddleRight:
                    return new PointF(res.Width - x - xOffset - 40, res.Height / 2 + y - yOffset / 2);
                case MenuAnchor.BottomLeft:
                    return new PointF(x, res.Height - y - yOffset);
                case MenuAnchor.BottomCenter:
                    return new PointF(res.Width / 2 + x, res.Height - y - yOffset);
                case MenuAnchor.BottomRight:
                    return new PointF(res.Width - x, res.Height - y - yOffset);
                default:
                    return PointF.Empty;
            }
        }*/

        public static bool HasOpen() => uiMenu != null;
        #endregion
    }
}