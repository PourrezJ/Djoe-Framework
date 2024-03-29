﻿using CitizenFX.Core;
using Newtonsoft.Json;
using Shared.MenuManager;
using System.Collections.Generic;

namespace Server
{
    public class RadialMenuItemIconDesc
    {
        public string Type;
        public string Name;

        public RadialMenuItemIconDesc(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }

    public class RadialMenuItem
    {
        #region Private fields
        private Dictionary<string, object> _data;
        #endregion

        #region Events
        public delegate void OnMenuItemCallBackDelegate(Player client, RadialMenu menu, RadialMenuItem menuItem, int itemIndex, dynamic data);

        [JsonIgnore]
        public OnMenuItemCallBackDelegate OnMenuItemCallback { get; set; } = null;
        #endregion

        #region Public properties
        public string Id { get; set; }
        public string Text { get; set; }
        public RadialMenuItemIconDesc Icon { get; set; }
        public string Description { get; set; }
        public bool ExecuteCallback { get; set; }
        public string InputValue { get; set; }
        public byte? InputMaxLength { get; set; }
        public InputType? InputType { get; set; }
        #endregion

        #region Constructor
        public RadialMenuItem(string text, string description = null, string id = null, RadialMenuItemIconDesc icon = null, bool executeCallback = true)
        {
            if (text != null && text.Trim().Length == 0)
                Text = null;
            else
                Text = text;

            Icon = icon;

            if (description != null && description.Trim().Length == 0)
                Description = null;
            else
                Description = description;

            if (id != null && id.Trim().Length == 0)
                Id = null;
            else
                Id = id;

            ExecuteCallback = executeCallback;
            _data = new Dictionary<string, object>();
        }
        #endregion

        #region Public methods
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

        public void ResetData(string key)
        {
            _data.Remove(key);
        }

        public void SetData(string key, object value)
        {
            _data[key] = value;
        }

        public void SetInput(string defaultText, byte maxLength, InputType inputType)
        {
            InputValue = defaultText;
            InputMaxLength = maxLength;
            InputType = inputType;
        }
        #endregion
    }
}
