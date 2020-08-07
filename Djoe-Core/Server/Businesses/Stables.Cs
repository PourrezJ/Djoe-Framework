using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Entities;
using Server.Stores;
using Server.Utils.Enums;
using Server.Utils.Extensions;
using Shared;
using Shared.MenuManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Stores
{
    public class StableStore : Store
    {
        public List<UCoords> SpawnPos = new List<UCoords>();
        public List<StableHorse> HorseList = new List<StableHorse>();
        public List<CompomentHorse> CompomentList = new List<CompomentHorse>();
        public StableHorse StableHorse;
        public int ParkID; // -1 si il est sorti 
        public double ParkingPrice = 0.5;

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

            var steamID = client.Identifiers["steam"];
            if (HorseManager.HorseDatas.ContainsKey(steamID))
            {
                var horses = HorseManager.HorseDatas[steamID].ToList().FindAll(p => p.LastCoords.ToVector3().DistanceToSquared(Location.ToVector3()) < 5f);

                foreach(var horse in horses)
                {
                    var menuItem = new MenuItem($"Ranger {horse.Name}", $"Ranger votre cheval pour le prix de ~e~${ParkingPrice}", "Id_StoreHorse");
                    menuItem.SetData("Horse", horse);
                    menu.Add(menuItem);
                }
            }

            menu.ItemSelectCallback += OnItemSelect;

            menu.OpenMenu(client);
        }

        private void OpenHorseDealer(Player client)
        {
            Menu menu = new Menu("ID_HorseDealer", "Écurie", backCloseMenu:true);

            var spawnSelected = GetFreeBox();

            menu.SetData("Spawn", spawnSelected);

            if (spawnSelected == null)
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

            menu.OpenMenu(client, JsonConvert.SerializeObject(new { Horses = HorseList[0], Spawn = spawnSelected }));
        }

        private void OpenCompomentDealer(Player player)
        {
            Menu menu = new Menu("ID_CompomentDealer", "Écurie", backCloseMenu: true);

            foreach(var comp in CompomentList)
            {
                var listItem = new ListItem(comp.Name, "", "ID_Comp", comp.Variations.Keys.ToList<string>(), 0, true);
                menu.Add(listItem);
            }

            menu.OpenMenu(player);
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

                    if (HorseManager.HorseDatas.ContainsKey(client.Identifiers["steam"]))
                    {
                        menu.ClearItems();
                        var myHorses = HorseManager.HorseDatas[client.Identifiers["steam"]];

                        foreach (HorseData myhorse in myHorses)
                        {
                            menu.Add(new MenuItem(myhorse.Name, "", "Id_MyHorse"));
                        }

                        menu.OpenMenu(client);
                    }
                    else
                    {
                        client.SendTipRightNotification("~e~Vous ne possedez pas de chevaux.");
                        //MenuManager.CloseMenu(client);
                    }
                    break;

                case "Id_Horse":

                    MenuManager.CloseMenu(client);

                    var listItem = menuItem.CastToListItem();
                    var horse = StableHorse.Variation.ElementAt(listItem.SelectedItem).Key;
                    BuyHorse(client, horse, StableHorse.Price, StableHorse.InvWeight, menuItem.InputValue, menu.GetData("Spawn"));
                    break;

                case "Id_MyHorse":

                    menu.ClearItems();

                    menu.SetData("MyHorse", HorseManager.HorseDatas[client.Identifiers["steam"]][itemIndex]);

                    menu.Add(new MenuItem("En faire votre cheval par défault", "", "ID_DefaultHorse"));

                    menu.OpenMenu(client);

                    break;

                case "ID_DefaultHorse":

                    HorseData mh = menu.GetData("MyHorse");

                    if (mh == null)
                        return;

                    HorseManager.SetDefaultHorse(client, mh);
                    client.SendTipRightNotification($"{mh.Name} est maintenant votre cheval par défault");
                    break;

                case "Id_StoreHorse":

                    HorseData horseData = menuItem.GetData("Horse");

                    if (horseData == null)
                        return;

                    horseData.ParkID = this.ParkID;

                    Task.Run(()=> horseData.UpdateAsync());
                    break;
            }
        }

        private async void BuyHorse(Player client, string horseHash, double price, double invheight, string name, UCoords spawn)
        {
            var pData = client.GetPlayerDatabase();

            if (pData == null)
                return;

            if (pData.HasMoney(price))
            {
                var hash = Utils.Misc.GetHashKey(horseHash);
                var horse = PedsManager.CreatePed(client, (PedHash)hash, spawn, false, PedType.Horse);

                var clientID = client.Identifiers["steam"];

                var hData = new HorseData()
                {
                    Name = name,
                    Hash = hash,
                    LastCoords = spawn,
                    OwnerID = clientID,
                    Inventory = new Inventory(invheight, 24),
                    PedNetwork = horse
                };

                await Database.MongoDB.InsertAsync<HorseData>("horses", hData, "BuyHorse");

                if (HorseManager.HorseDatas.ContainsKey(clientID))
                {
                    HorseManager.HorseDatas[clientID].Add(hData);
                }
                else
                {
                    HorseManager.HorseDatas.Add(clientID, new List<HorseData>() { hData });
                }

                client.SendTipRightNotification($"Vous venez d'acheter ~e~{name} ~q~pour le prix de ~e~${price}");
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
