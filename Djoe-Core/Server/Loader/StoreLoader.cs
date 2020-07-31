using CitizenFX.Core.Native;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Server.Businesses;
using Server.Stores;
using Server.Utils;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            string[] files = Directory.GetFiles(MakePath(), "*.json", SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                var o = JObject.Parse(File.ReadAllText(file));
                var jsonType = (int)o["StoreType"];

                try
                {
                    switch (jsonType)
                    {
                        case (int)StoreType.GeneralStore:
                            var gs = o.ToObject<GeneralStore>();

                            var itemList = o["Items"];

                            foreach (var dynItem in itemList)
                            {
                                var itemID = (int)dynItem["Id"];

                                if (ItemLoader.ItemList.ContainsKey(itemID))
                                {
                                    var item = ItemLoader.ItemList[itemID];

                                    if (dynItem["Name"] != null)
                                        item.Name = (string)dynItem["Name"];

                                    if (dynItem["Price"] != null)
                                        item.ItemPrice = (double)dynItem["Price"];

                                    if (dynItem["Description"] != null)
                                        item.Description = (string)dynItem["Description"];

                                    gs.ItemList.Add(item);
                                }
                            }

                            gs.Init();
                            break;

                        case (int)StoreType.Stables:
                            var sb = o.ToObject<StableStore>();
                            var spawnPos = o["SpawnPos"];

                            sb.SpawnPos = spawnPos.ToObject<List<UCoords>>();

                            sb.HorseList = o["Horses"].ToObject<List<StableHorse>>();
                            sb.CompomentList = o["Complements"].ToObject<List<CompomentHorse>>();

                            sb.Init();
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Logger.Debug(file);
                    Logger.Exception(ex);
                }
            }

            Logger.Info($"--- Finish loading all store in database: {BusinessesList.Count} ---");
        }

        private static string MakePath(string relativePath = "") => "Stores";

    }
}
