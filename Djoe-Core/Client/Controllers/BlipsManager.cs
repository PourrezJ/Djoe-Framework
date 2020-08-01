using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.External;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

namespace Client.Controllers
{
    public class BlipsManager : BaseScript
    {
        internal static Dictionary<int, Blip> BlipsList = new Dictionary<int, Blip>();

        public BlipsManager()
        {
            EventHandlers["GetAllBlips"] += new Action<string>(GetAllBlips);
            EventHandlers["CreateBlip"] += new Action<int, string>(CreateBlip);
            EventHandlers["onResourceStop"] += new Action<string>(OnClientResourceStop);
        }

        private void OnClientResourceStop(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) 
                return;

            Debug.WriteLine("Supression des blips.");
            lock (BlipsList)
            {
                foreach (var blip in BlipsList)
                {
                    blip.Value?.Delete();
                }
            }
        }
        private static void CreateBlip(int id, dynamic data)
        {
            BlipNetwork blipdata = JsonConvert.DeserializeObject<BlipNetwork>(data);

            BlipsList.Add(id, World.CreateBlip(blipdata.Name, blipdata.Position, (BlipSprite)blipdata.Sprite, (BlipType)blipdata.Type));
        }

        private static void GetAllBlips(string data)
        {
            var blips = JsonConvert.DeserializeObject<Dictionary<int, BlipNetwork>>(data);

            lock(BlipsList)
            {
                foreach (var blipDict in blips)
                {
                    var blipdata = blipDict.Value;
                    BlipsList.Add(blipDict.Key, World.CreateBlip(blipdata.Name, blipdata.Position, (BlipSprite)blipdata.Sprite, (BlipType)blipdata.Type));
                }
            }
        }
    }
}
