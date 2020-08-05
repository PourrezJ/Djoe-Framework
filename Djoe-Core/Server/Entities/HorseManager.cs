using CitizenFX.Core;
using Server.Utils;
using Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class HorseManager
    {
        public static Dictionary<string, List<HorseData>> HorseDatas = new Dictionary<string, List<HorseData>>(); // All horse in database

        public static void Init()
        {
            GameMode.RegisterEventHandler("djoe:updatehorse", new Action<int, Vector3, float, int>(UpdateHorse));
            


            Thread InstanceCaller = new Thread(new ThreadStart(HorsesUpdateLoop));

            // Start the thread.
            InstanceCaller.Start();
        }

        private static void UpdateHorse(int networkID, Vector3 pos, float heading, int health)
        {
            PedNetwork hNetwork = PedsManager.GetWithNetworkID(networkID);

            if (hNetwork == null)
                return;

            HorseData hData = GetWithPedNetwork(hNetwork);

            if (hData == null)
                return;

            lock (hData)
            {
                hData.LastCoords.SetUcoord(pos, heading);
                hData.Health = health;
                hData.NeedUpdate = true;
            }
        }

        public static HorseData GetWithPedNetwork(PedNetwork data)
        {
            lock (HorseDatas)
            {
                foreach(var hData in HorseDatas.Values)
                {
                    var horse = hData.FirstOrDefault(p => p.PedNetwork == data);
                    if (horse != null)
                    {
                        return horse;
                    }
                }   
            }
            return null;
        }
        private static async void HorsesUpdateLoop()
        {
            while (true)
            {
                var horseDatas = HorseDatas.Values.ToList();
                foreach (var owner in horseDatas)
                {
                    foreach(var horse in owner)
                    {
                        if (horse.NeedUpdate)
                        {
                            await horse.UpdateAsync();
                            horse.NeedUpdate = false;
                        }
                    }

                    Thread.Sleep(50);
                }
                horseDatas.Clear();
                Thread.Sleep(100);
            }
        }

        public static void SetDefaultHorse(Player player, HorseData horseData)
        {
            if (HorseDatas.ContainsKey(player.Identifiers["steam"]))
            {
                foreach (var horse in HorseDatas[player.Identifiers["steam"]])
                {
                    if (horseData == horse)
                        horse.IsDefault = true;
                    else
                        horseData.IsDefault = false;
                }
            }
        }
    }
}
