using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using Server.Stores;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Loader
{
    public static class StoreLoader
    {
        public static List<Store> BusinessesList = new List<Store>();
        public static void Init()
        {
            Logger.Info("--- Start loading all store in database ---");

            if (!Directory.Exists(MakePath())) 
                Directory.CreateDirectory(MakePath());

            string[] files = Directory.GetFiles(MakePath(), "*.json");
            
            foreach (var file in files)
            {
                var o = JObject.Parse(File.ReadAllText(file));
                var jsonType = (int)o["StoreType"];

                switch (jsonType)
                {
                    case (int)StoreType.GeneralStore:
                        var gs = o.ToObject<GeneralStore>();
                        gs.Init();
                    break;
                }
            }

            Logger.Info($"--- Finish loading all store in database: {BusinessesList.Count} ---");
        }

        private static string MakePath(string relativePath = "") => "Stores";

    }
}
