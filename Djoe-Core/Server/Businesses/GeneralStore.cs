using CitizenFX.Core;
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
        public Dictionary<int, Item> ItemList = new Dictionary<int, Item>();

        public GeneralStore(string businnessName, UCoords location, BlipSprite blipSprite, BlipType blipType, PedHash pedhash = 0) : base(businnessName, location, blipSprite, blipType, pedhash)
        {
        }

        public override void Init()
        {
            var colshape = ColshapeManager.CreateCylinderColshape(Location.ToVector3(), 10, 3);
            colshape.OnPlayerEnterColshape += OnPlayerEnterColshape;
            colshape.OnPlayerLeaveColshape += OnPlayerLeaveColshape;
            colshape.OnPlayerInteractInColshape += OnPlayerInteractInColshape;

            base.Init();
        }

        public override void OpenMenu(Player client, PedNetwork npc)
        {

        }

        private void OnPlayerInteractInColshape(IColshape colshape, Player client)
        {
            Console.WriteLine("OnPlayerInteractInColshape");

            Menu menu = new Menu("ID_GeneralStoreMenu", this.BusinnessName);

            menu.ItemSelectCallback += OnItemSelectCallBack;
            menu.ListItemChangeCallback += OnListItemChangeCallback;

            foreach (var item in ItemList)
            {
                var listItem = new ListItem(item.Value.Name + $" ${item.Value.ItemPrice}/${item.Value.ItemPrice}", item.Value.Description, item.Value.Name, 99, 0, true);
                menu.Add(listItem);
            }

            menu.OpenMenu(client);
        }

        private void OnItemSelectCallBack(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            if (!(menuItem is ListItem))
                return;

            ListItem listItem = (menuItem as ListItem);

            if (listItem.SelectedItem == 0)
                return;

            var item = ItemList[itemIndex];
            var itemPrice = item.ItemPrice;
            var totalPrice = itemPrice * listItem.SelectedItem;

            var playerData = client.GetPlayerDatabase();

            if (playerData == null)
                return;

            if (playerData.HasMoney(totalPrice))
            {
                if (playerData.PocketInventory.AddItem(item, listItem.SelectedItem, false))
                {
                    client.SendTipRightNotification($"Vous venez d'acheter  ~e~{((listItem.SelectedItem == 1) ? "un(e)" : listItem.SelectedItem.ToString())} " + item.Name 
                       + $"\n~q~pour la somme de ~e~${totalPrice}");
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
            Console.WriteLine(listIndex);
        }

        private void OnPlayerLeaveColshape(IColshape colshape, Player client)
        {
            Console.WriteLine("OnPlayerLeaveColshape");
        }

        private void OnPlayerEnterColshape(IColshape colshape, Player client)
        {
            client.SendTipNotification("Appuyez sur ~INPUT_CONTEXT~ pour intéragir");
            Console.WriteLine("OnPlayerEnterColshape");
        }
    }
}
