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
            API.RegisterCommand("coords", new Action<int, List<object>, string>((source, args, raw) =>
            {

            }), false);
        }
    }
}
