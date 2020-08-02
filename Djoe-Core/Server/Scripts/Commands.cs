using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using Server.Utils;
using Server.Utils.Enums;
using Server.Utils.Extensions;

namespace Server.Scripts
{
    class Commands : BaseScript
    {
        public Commands()
        {
            API.RegisterCommand("ped", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var pl = new PlayerList();
                var player = pl[source];

                var data = player.GetPlayerDatabase();

                if (data == null)
                    return;

                var pos = data.LastCoord.ToVector3();


            }), false);
        }
    }
}
