using Newtonsoft.Json.Linq;
using Server.Utils;
using System.Collections.Concurrent;
using System.IO;

namespace Server.Loader
{
    public static class ItemLoader
    {
        public static ConcurrentDictionary<int, Item> ItemList = new ConcurrentDictionary<int, Item>();
        public static void Init()
        {
            Logger.Info("--- Start loading all item in database ---");

            if (!Directory.Exists(MakePath()))
                Directory.CreateDirectory(MakePath());

            string[] files = Directory.GetFiles(MakePath(), "*.json");

            foreach (var file in files)
            {
                var o = JObject.Parse(File.ReadAllText(file));
                JObject[] items = o["Items"].ToObject<JObject[]>();

                foreach(var item in items)
                {
                    int id = (int)item["Id"];
                    if (ItemList.ContainsKey(id))
                    {
                        Logger.Warn($"Item {(string)item["Name"]} use a existant id {id}");
                        continue;
                    }

                    var cI = Item.DynamicToItem(item);

                    if (cI != null)
                        ItemList.TryAdd((int)cI.Id, cI);
                }
            }

            Logger.Info($"--- Finish loading all item in database: {ItemList.Count} ---");
        }

        private static string MakePath(string relativePath = "") => "Items";
    }
}
