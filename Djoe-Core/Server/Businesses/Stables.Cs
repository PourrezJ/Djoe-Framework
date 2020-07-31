using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Entities;
using Server.Stores;
using Server.Utils;
using Server.Utils.Enums;
using Server.Utils.Extensions;
using Shared;
using Shared.MenuManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Businesses
{

    public class StableStore : Store
    {

        public List<UCoords> SpawnPos = new List<UCoords>();
        public List<StableHorse> HorseList = new List<StableHorse>();
        public StableHorse StableHorse;
        public UCoords SpawnSelected;

        public StableStore(string businnessName, UCoords location, BlipSprite blipSprite, BlipType blipType, PedHash pedhash = 0) : base(businnessName, location, blipSprite, blipType, pedhash)
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void OpenMenu(Player client, PedNetwork npc)
        {
            Menu menu = new Menu("ID_Stable", "Écurie");

            menu.Add(new MenuItem("Acheter un cheval", "", "Id_BuyHorse"));
            menu.Add(new MenuItem("Gestion des chevaux", "", "Id_Management"));

            menu.ItemSelectCallback += OnItemSelect;

            menu.OpenMenu(client);
        }

        private void OpenHorseDealer(Player client)
        {
            Menu menu = new Menu("ID_HorseDealer", "Écurie", backCloseMenu:true);

            SpawnSelected = GetFreeBox();

            if (SpawnSelected == null)
            {
                client.SendTipRightNotification("Aucun box n'est libre actuellement");
                return;
            }

            foreach (var horse in HorseList)
            {
                List<string> horseName = horse.Variation.Values.ToList<string>();
                var horses = new ListItem(horse.Name, $"Prix: ${horse.Price}\nCapacité de stockage: {horse.InvWeight}", "Id_Horse", horseName, 0, true);
                horses.SetInput("Nom du cheval", 99, InputType.Text);
                menu.Add(horses);
            }

            menu.IndexChangeCallback += OnIndexChange;
            menu.ItemSelectCallback += OnItemSelect;

            menu.OpenMenu(client, JsonConvert.SerializeObject(new { Horses = HorseList[0], Spawn = SpawnSelected }));
        }

        private void OnIndexChange(Player client, Menu menu, int itemIndex, IMenuItem menuItem)
        {
            if (itemIndex <= HorseList.Count)
            {
                client.TriggerEvent("Stable_ListUpdate", JsonConvert.SerializeObject(HorseList[itemIndex]));
                StableHorse = HorseList[itemIndex]; 
            }    
        }

        private void OnItemSelect(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            Console.WriteLine("OnItemSelect");
            switch (menuItem.Id)
            {
                case "Id_BuyHorse":
                    OpenHorseDealer(client);
                    break;

                case "Id_Management":

                    break;

                case "Id_Horse":

                    MenuManager.CloseMenu(client);

                    var listItem = menuItem.CastToListItem();
                    var horse = StableHorse.Variation.ElementAt(listItem.SelectedItem).Key;
                    BuyHorse(client, horse, StableHorse.Price, StableHorse.InvWeight, menuItem.InputValue);
                    break;
            }
        }

        private async void BuyHorse(Player client, string horseHash, double price, double invheight, string name)
        {
            var pData = client.GetPlayerDatabase();

            if (pData == null)
                return;

            if (pData.HasMoney(price))
            {
                var hash = Utils.Misc.GetHashKey(horseHash);
                PedsManager.CreatePed(client, (PedHash)hash, SpawnSelected, false, PedType.Horse);

                var hData = new HorseData()
                {
                    Name = name,
                    Hash = hash,
                    LastCoords = SpawnSelected,
                    OwnerID = client.Identifiers["steam"],
                    Inventory = new Inventory(invheight, 40)
                };

                await Database.MongoDB.InsertAsync<HorseData>("horses", hData, "BuyHorse");
            }
        }

        private UCoords GetFreeBox()
        {
            var players = PlayerManager.GetPlayersOnline();

            foreach(var spawn in SpawnPos)
            {
                if (!players.Exists(p => p.LastCoord.ToVector3().DistanceToSquared(spawn.ToVector3()) < 2f))
                {
                    return spawn;
                }
            }

            return null;
        }
    }
}
