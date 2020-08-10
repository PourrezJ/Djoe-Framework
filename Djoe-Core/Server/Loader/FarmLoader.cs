using Newtonsoft.Json.Linq;
using Server.Farms;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Loader
{
    public class FarmLoader
    {
        public static List<Farm> Farms = new List<Farm>();

        public static void Init()
        {
            Logger.Info("--- Start loading all farms ---");

            if (!Directory.Exists(MakePath()))
                Directory.CreateDirectory(MakePath());

            string[] files = Directory.GetFiles(MakePath(), "*.json");
            
            foreach (var file in files)
            {
                var o = JObject.Parse(File.ReadAllText(file));

                switch(o["FarmType"].ToObject<FarmType>())
                {
                    case FarmType.Miner:
                        Farms.Add(o.ToObject<Farm>());
                        break;
                }
            }

            Logger.Info($"--- Finish loading all farms in database: {Farms.Count} ---");
        }

        private static string MakePath(string relativePath = "") => "Farms";
    }
}
