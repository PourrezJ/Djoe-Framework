using Server.Utils;
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
        public static Dictionary<string, List<HorseData>> HorseDatas = new Dictionary<string, List<HorseData>>();

        public static void Init()
        {/*
            Thread InstanceCaller = new Thread(new ThreadStart(HorsesUpdateLoop));

            // Start the thread.
            InstanceCaller.Start();*/
        }
        /*
        private static async void HorsesUpdateLoop()
        {
            while (true)
            {
                foreach (var horse in HorseDatas)
                {
                    if (horse.Value.)
                    {
                        await horse.UpdateAsync();
                        horse.NeedUpdate = false;
                    }

                    Thread.Sleep(50);
                }
                horseDatas.Clear();
                Thread.Sleep(100);
            }
        }*/
    }
}
