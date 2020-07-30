using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Stores;
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
            Menu menu = new Menu("ID_HorseDealer", "Écurie");

            var spawn = GetFreeBox();

            if (spawn == null)
            {
                client.SendTipRightNotification("Aucun box n'est libre actuellement");
                return;
            }

            foreach (var horse in HorseList)
            {
                List<string> horseName = horse.Variation.Values.ToList<string>();
                menu.Add(new ListItem(horse.Name, $"Prix: ${horse.Price}\nCapacité de stockage: {horse.InvWeight}", "Id_Horse", horseName, 0, true));
            }

            menu.IndexChangeCallback += OnIndexChange;

            menu.OpenMenu(client, JsonConvert.SerializeObject(HorseList[0]));
        }

        private void OnIndexChange(Player client, Menu menu, int itemIndex, IMenuItem menuItem)
        {
            if (itemIndex <= HorseList.Count)
                client.TriggerEvent("Stable_ListUpdate", JsonConvert.SerializeObject(HorseList[itemIndex]));
        }

        private void OnItemSelect(Player client, Menu menu, IMenuItem menuItem, int itemIndex)
        {
            switch (menuItem.Id)
            {
                case "Id_BuyHorse":
                    OpenHorseDealer(client);
                    break;

                case "Id_Management":

                    break;
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
