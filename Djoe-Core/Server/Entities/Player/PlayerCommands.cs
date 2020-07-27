using CitizenFX.Core;
using CitizenFX.Core.Native;
using Server.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public static class PlayerCommands
    {
        public static void Init()
        {
            API.RegisterCommand("addmoney", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var pl = new PlayerList();
                var player = pl[source];

                var data = player.GetPlayerDatabase();

                if (data == null)
                    return;

                if (data.StaffRank >= Shared.StaffRank.Moderateur && double.TryParse(args[0].ToString(), out double moneyval))
                {
                    data.AddMoney(moneyval);
                }
            }), false);
        }
    }
}
