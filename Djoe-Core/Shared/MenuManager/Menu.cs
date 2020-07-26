using CitizenFX.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.MenuManager
{
    public class Menu
    {
        #region Public delegates
        public delegate void MenuCallback(Player client, Menu menu, IMenuItem menuItem, int itemIndex);
        public delegate void MenuCheckbox(Player client, Menu menu, IMenuItem menuItem, bool value);
        public delegate void MenuListCallback(Player client, Menu menu, IListItem listItem, int listIndex);
        public delegate void MenuCurrentIndex(Player client, Menu menu, int itemIndex, IMenuItem menuItem);
        public delegate void MenuFinalizer(Player client, Menu menu);
        #endregion

        #region Private fields
        private Dictionary<string, object> _data;
        private int _selectedIndex;
        private MenuItem _selectedItem;
        #endregion

        #region Public properties
        public string Id { get; set; } = "";
        public Banner BannerSprite { get; set; }
        public string Title { get; set; } = "";
        public string SubTitle { get; set; } = "";
        public int PosX { get; set; }
        public int PosY { get; set; }
        public MenuAnchor Anchor { get; set; } = MenuAnchor.MiddleRight;
        public bool NoExit { get; set; }
        public bool EnableBanner { get; set; }
        public bool CallbackOnIndexChange
        {
            get 
            {
                if (IndexChangeCallback != null)
                    return true;
                return false;
            }
        }
        public MenuItemList Items { get; set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value >= Items.Count)
                    return;

                _selectedIndex = value;
                _selectedItem = Items[(value != -1) ? value : 0];
            }
        }
        public bool BackCloseMenu { get; set; }
        #endregion

        #region Public Json ignored properties
        [JsonIgnore]
        public MenuItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (!Items.Contains(value))
                    return;

                _selectedItem = value;
                _selectedIndex = Items.IndexOf(value);
            }
        }

        [JsonIgnore]
        public MenuCallback ItemSelectCallback { get; set; }
        [JsonIgnore]
        public MenuListCallback ListItemChangeCallback { get; set; }
        [JsonIgnore]
        public MenuCurrentIndex IndexChangeCallback { get; set; }
        [JsonIgnore]
        public MenuCheckbox CallbackCheckBox { get; set; }
        [JsonIgnore]
        public MenuFinalizer Finalizer { get; set; }
        #endregion

        #region Constructor
        public Menu(string id, string title, string subTitle = "", int posX = 0, int posY = 0, MenuAnchor anchor = MenuAnchor.MiddleRight, bool noExit = false, bool enableBanner = true, bool backCloseMenu = false, Banner banner = null)
        {
            _selectedIndex = -1;
            _selectedItem = null;

            if (id == null && id.Trim().Length == 0)
                Id = null;
            else
                Id = id;

            BannerSprite = banner;

            if (title != null && title.Trim().Length == 0)
                Title = null;
            else
                Title = title;

            if (subTitle != null && subTitle.Trim().Length > 0)
                SubTitle = subTitle;

            PosX = posX;
            PosY = posY;

            Anchor = anchor;
            NoExit = noExit;
            EnableBanner = enableBanner;
            BackCloseMenu = backCloseMenu;

            _data = new Dictionary<string, object>();
            Items = new MenuItemList();
            ItemSelectCallback = null;
            Finalizer = null;
        }
        #endregion

        #region Public operators
        public MenuItem this[string id]
        {
            get
            {
                foreach (MenuItem menuItem in Items)
                {
                    if (menuItem.Id == id)
                        return menuItem;
                }

                return null;
            }
        }
        #endregion

        #region Public methods
        public void Add(MenuItem menuItem)
        {
            Items.Add(menuItem);
        }

        public void ClearItems()
        {
            Items.Clear();
        }

        public bool Contains(string id)
        {
            foreach (MenuItem menuItem in Items)
            {
                if (menuItem.Id == id)
                    return true;
            }

            return false;
        }

        public bool Contains(MenuItem menuItem)
        {
            return Items.Contains(menuItem);
        }

        public dynamic GetData(string key)
        {
            if (!_data.ContainsKey(key))
                return null;

            return _data[key];
        }

        public bool HasData(string key)
        {
            return _data.ContainsKey(key);
        }

        public void Insert(int index, MenuItem menuItem)
        {
            Items.Insert(index, menuItem);

            if (index <= _selectedIndex)
                _selectedIndex++;
        }

        public bool Remove(string id)
        {
            foreach (MenuItem menuItem in Items)
            {
                if (menuItem.Id == id)
                    return Remove(menuItem);
            }

            return false;
        }

        public bool Remove(MenuItem menuItem)
        {
            int pos = Items.IndexOf(menuItem);
            bool result = Items.Remove(menuItem);

            if (pos == -1 || !result)
                return false;

            if (Items.Count == 0)
            {
                _selectedIndex = -1;
                _selectedItem = null;
            }
            else if (menuItem == _selectedItem)
            {
                if (_selectedIndex == Items.Count)
                    _selectedIndex--;

                _selectedItem = Items[_selectedIndex];
            }
            else if (pos < _selectedIndex)
                _selectedIndex--;

            return true;
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);

            if (Items.Count == 0)
            {
                _selectedIndex = -1;
                _selectedItem = null;
            }
            else if (index == _selectedIndex)
            {
                if (_selectedIndex == Items.Count)
                    _selectedIndex--;

                _selectedItem = Items[_selectedIndex];
            }
            else if (index < _selectedIndex)
                _selectedIndex--;
        }

        public void Reset()
        {
            Items.Clear();
            _data.Clear();
        }

        public void ResetData(string key)
        {
            _data.Remove(key);
        }

        public void SetData(string key, object value)
        {
            _data[key] = value;
        }
        /*
        public void OpenMenu(Player client) => MenuManager.OpenMenu(client, this);

        public void CloseMenu(Player client) => MenuManager.CloseMenu(client);*/
        #endregion
    }
}
