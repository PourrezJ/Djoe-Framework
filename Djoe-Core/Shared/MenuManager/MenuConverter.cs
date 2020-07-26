﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.MenuManager
{
    public class MenuConverter : JsonConverter
    {
        private readonly Type[] _types;

        public MenuConverter(params Type[] types)
        {
            _types = types;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            Menu menu = jo.ToObject<Menu>();
            
            List<dynamic> Items = jo["Items"].ToObject<List<dynamic>>();
            MenuItemList MenuItems = new MenuItemList();
           
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                JValue temp = (JValue)item["Type"];
                MenuItemType type = temp.ToObject<MenuItemType>();

                InputType inputtype = (item["InputType"] != null) ? ((JToken)item["InputType"]).ToObject<InputType>() : InputType.Text;
                
                string Id = (string)item["Id"];
                string Text = (string)item["Text"];
                string Description = (string)item["Description"];
                bool InputSetRightLabel = (bool)item["InputSetRightLabel"];
                bool InputErrorResetValue = (bool)item["InputErrorResetValue"];
                string LeftBadge = (string)item["LeftBadge"];
                MenuItemType MenuType = type;
                string RightBadge = (string)item["RightBadge"];
                string RightLabel = (string)item["RightLabel"];
                MenuItem menuitem = null;
                switch (type)
                {
                    case MenuItemType.ColoredItem:
                    case MenuItemType.MenuItem:
                        menuitem = new MenuItem(Text, Description, Id, RightLabel);
                        break;

                    case MenuItemType.CheckboxItem:
                        menuitem = new CheckboxItem(Text, Description, Id, (bool)item["Checked"]);
                        break;

                    case MenuItemType.ListItem:
                        JArray a = (JArray)item["Items"];
                        List<string> items = a.ToObject<List<string>>();
                        int SelectedItem = (int)item["SelectedItem"];
                        menuitem = new ListItem(Text, Description, Id, items, SelectedItem);
                        break;

                    default:
                        menuitem = new MenuItem(Text, Description, Id, RightLabel);
                        break;
                }

                if (item["InputMaxLength"] != null) menuitem.InputMaxLength = (byte)item["InputMaxLength"];
                if (item["InputType"] != null) menuitem.InputType = inputtype;
                if (item["InputValue"] != null) menuitem.InputValue = (string)item["InputValue"];
                MenuItems.Add(menuitem);
            };

            menu.Items = MenuItems;
            return menu;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
