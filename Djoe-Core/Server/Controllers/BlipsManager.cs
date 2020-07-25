using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Utils.Enums;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public static class BlipsManager
    {
        public static Dictionary<int, BlipNetwork> BlipsList = new Dictionary<int, BlipNetwork>();

        public static void OnPlayerConnected(Player player)
        {
            player.TriggerEvent("GetAllBlips", JsonConvert.SerializeObject(BlipsList));
        }

        public static BlipNetwork CreateBlip(string name, BlipSprite sprite, BlipType type, Vector3 pos, float rot)
        {
            BlipNetwork blip = new BlipNetwork(name, (uint)sprite, (uint)type, pos, rot);
            lock (BlipsList)
            {
                int id = BlipsList.Count + 1;
                BlipsList.Add(id, blip);

                lock (GameMode.PlayersList)
                {
                    foreach (Player player in GameMode.PlayersList)
                    {
                        player.TriggerEvent("CreateBlip", id, JsonConvert.SerializeObject(blip));
                    }
                }
            }
            return blip;
        }
    }
}
