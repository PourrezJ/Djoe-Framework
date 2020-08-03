using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Scripts
{
    class DiscordRichPresence : BaseScript
    {
        public DiscordRichPresence()
        {
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            API.SetDiscordAppId("739950201464160319");
            API.SetDiscordRichPresenceAsset("logo");
            API.SetDiscordRichPresenceAssetText("Joue sur RDRP");
            await Delay(6000);

        }
    }
}
