using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using Server.Utils;

namespace Server.Scripts
{
    class Commands : BaseScript
    {
        public Commands()
        {
            API.RegisterCommand("radialtest", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var pl = new PlayerList();
                var player = pl[source];

                var menu = new RadialMenu("ID_Pouette");

                menu.Add(new RadialMenuItem("Truc 1", "Un exemple de description", "ID_1", RadialMenuItemIcons.ADDRESS_CARD_SOLID));

                menu.OpenRadialMenu(player);
            }), false);
        }
    }
}
