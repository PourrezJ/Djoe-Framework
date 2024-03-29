﻿using CitizenFX.Core;
using MongoDB.Bson.IO;
using Server.Colshape;
using Server.Utils.Enums;
using Server.Utils.Extensions;
using Shared;
using Shared.MenuManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Stores
{
    public class GeneralStore : Store
    {
        public List<Item> ItemList = new List<Item>();

        public GeneralStore(string businnessName, UCoords location, BlipSprite blipSprite, BlipType blipType, PedHash pedhash = 0) : base(businnessName, location, blipSprite, blipType, pedhash)
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void OpenMenu(Player client, PedNetwork npc)
        {
            Menu menu = new Menu("ID_GeneralStoreMenu", this.BusinnessName);

            menu.ItemSelectCallback += OnItemSelectCallBack;
            menu.ListItemChangeCallback += OnListItemChangeCallback;

            double[] prices = new double[ItemList.Count];

            for (int a = 0; a < ItemList.Count; a++)
            {
                var item = ItemList[a];
                prices[a] = item.ItemPrice;
                var listItem = new ListItem(item.Name + $" ${item.ItemPrice}/${item.ItemPrice}", item.Description, item.Name, 99, 0, true);
                menu.Add(listItem);
            }

            menu.OpenMenu(client, Newtonsoft.Json.JsonConvert.SerializeObject(prices));
        }


        private void OnItemSelectCallBack(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            if (!(menuItem is ListItem))
                return;

            ListItem listItem = (menuItem as ListItem);

            if (listItem.SelectedItem == 0)
                return;

            var item = ItemList.ElementAt(itemIndex);
            var itemPrice = item.ItemPrice;
            var totalPrice = itemPrice * listItem.SelectedItem;

            var playerData = client.GetPlayerDatabase();

            if (playerData == null)
                return;

            if (playerData.HasMoney(totalPrice))
            {
                if (playerData.PocketInventory.AddItem(item, listItem.SelectedItem, false))
                {
                    client.SendTipRightNotification($"Vous venez d'acheter ~e~{((listItem.SelectedItem == 1) ? "un(e)" : listItem.SelectedItem.ToString())} {item.Name} ~q~pour ~e~${totalPrice}");
                }
                else
                {
                    client.SendTipRightNotification($"Vous n'avez pas la place dans votre sacoche pour {((listItem.SelectedItem == 1) ? "un(e)" : listItem.SelectedItem.ToString())} ~e~" + item.Name);
                }
            }
            else
            {
                client.SendTipRightNotification($"Vous n'avez pas assez d'argent pour  ~e~{((listItem.SelectedItem == 1) ? "un(e)" : listItem.SelectedItem.ToString())} " + item.Name);
            }
        }

        private void OnListItemChangeCallback(Player client, Menu menu, IListItem listItem, int listIndex)
        {
           
        }
    }
}
