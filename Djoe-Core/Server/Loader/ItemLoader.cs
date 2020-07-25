using Newtonsoft.Json.Linq;
using Server.Utils;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Loader
{
    public static class ItemLoader
    {
        public static List<Item> BusinessesList = new List<Item>();
        public static void Init()
        {
            Logger.Info("--- Start loading all item in database ---");

            if (!Directory.Exists(MakePath()))
                Directory.CreateDirectory(MakePath());

            string[] files = Directory.GetFiles(MakePath(), "*.json");

            foreach (var file in files)
            {
                var o = JObject.Parse(File.ReadAllText(file));
                var jsonType = (int)o["StoreType"];

                switch (jsonType)
                {

                }
            }

            Logger.Info($"--- Finish loading all item in database: {BusinessesList.Count} ---");
        }

        private static string MakePath(string relativePath = "") => "Items";
    }
}
