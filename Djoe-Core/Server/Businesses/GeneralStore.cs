using CitizenFX.Core;
using Server.Utils.Enums;
using Shared;
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


            base.Init();
        }

        public override void OpenMenu(Player client, PedNetwork npc)
        {
            base.OpenMenu(client, npc);
        }
    }
}
