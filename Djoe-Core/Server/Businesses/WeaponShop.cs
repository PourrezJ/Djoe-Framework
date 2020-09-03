using CitizenFX.Core;
using MongoDB.Bson.IO;
using Server.ItemsClass;
using Server.Utils.Enums;
using Server.Utils.Extensions;
using Shared;
using Shared.MenuManager;
using System;
using System.Collections.Generic;

namespace Server.Stores
{
    public class WeaponShop : Store
    {
        public List<WeaponItem> Weapons = new List<WeaponItem>();
        public UCoords SpawnObjectStore;
        public WeaponShop(string businnessName, UCoords location, BlipSprite blipSprite, BlipType blipType, PedHash pedhash = 0) : base(businnessName, location, blipSprite, blipType, pedhash)
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void OpenMenu(Player client, PedNetwork npc)
        {
            Menu menu = new Menu("ID_WeaponShopMenu", this.BusinnessName);

            menu.ItemSelectCallback += OnItemSelectCallBack;

            List<int> weaponsModels = new List<int>();
            List<uint> weaponsHash = new List<uint>();

            foreach(var weapon in Weapons)
            {
                var item = new MenuItem($"{weapon.Name} ${weapon.ItemPrice}", "", "ID_WeaponSelect");
                item.Description = $"{weapon.Description}";
                menu.Add(item);
                weaponsModels.Add(Utils.Misc.GetHashKey(weapon.WeaponModel));
                weaponsHash.Add((uint)Utils.Misc.GetHashKey(weapon.HashName));
            }

            menu.OpenMenu(client , Newtonsoft.Json.JsonConvert.SerializeObject(new { WeaponModel = weaponsModels, WeaponHash = weaponsHash, SpawnObject = SpawnObjectStore }));
        }

        private void OnItemSelectCallBack(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            var pData = client.GetPlayerDatabase();

            if (pData == null)
                return;

            if (itemIndex > Weapons.Count)
                return;

            WeaponItem weapon = Weapons[itemIndex];

            if (!pData.PocketInventory.IsFull(weapon.Weight) && pData.HasMoney(weapon.ItemPrice))
            {
                client.SendTipRightNotification($"Vous venez d'acheter ~e~un(e) {weapon.Name} ~q~pour le prix de ~e~${weapon.ItemPrice}~q~.");
                pData.PocketInventory.AddItem(weapon);
                weapon.Equip(client);
            }
        }
    }
}
