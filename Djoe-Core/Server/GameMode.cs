using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Server.Scripts;
using Server.Utils;
using Server.Entities;
using Shared.Utils;
using Server.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Server.Loader;

namespace Server
{
    public class GameMode : BaseScript
    {
        #region Fields
        public static GameMode Instance;
        public static bool ServerStarted { get; private set; }
        public static PlayerList PlayersList { get; private set; }

        public static WorldData WorldData { get; private set; }
        #endregion

        #region C4TOR
        public GameMode()
        {
            Instance = this;
            var ci = new CultureInfo("fr-FR");
            CultureInfo.DefaultThreadCurrentCulture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            PlayersList = Players;

            API.SetMapName("RDRP");
            API.SetGameType("roleplay");

            Config.LoadConfig("Config.json");

            if (!Database.MongoDB.Init())
            {
                Logger.Warn("Connection à la database impossible.");

                return;
            }

            var collection = Database.MongoDB.CollectionExist<GameMode>("worlddata");
            if (collection)
            {
                var database = Database.MongoDB.GetMongoDatabase();

                if (database == null)
                    return;

                Logger.Info("Chargement de la base de donnée");
                var collectionData = Database.MongoDB.GetCollectionSafe<WorldData>("worlddata");
                var data = collectionData.Find<WorldData>(new BsonDocument());
                if (data == null)
                    return;
                WorldData = data.FirstOrDefault();
            }
            else
            {
                Logger.Info("Création de la base de donnée");
                // Fresh Server
                WorldData = new WorldData();
                Database.MongoDB.Insert<WorldData>("worlddata", WorldData);
            }

            while (WorldData == null)
                Delay(100);

            RadialManager.Init();
            PlayerManager.Init();
            HorseManager.Init();
            Survival.Init();
            PlayerKeyHandler.Init();
            Events.Init();
            ItemLoader.Init();
            StoreLoader.Init();
           // FarmLoader.Init();

            ServerStarted = true;
        }
        #endregion

        #region Methods
        internal static void RegisterEventHandler(string name, Delegate action)
        {
            Instance.EventHandlers[name] += action;
        }

        public static void RegisterTickHandler(Func<Task> tick)
        {
            Instance.Tick += tick;
        }

        public static void DeregisterTickHandler(Func<Task> tick)
        {
            Instance.Tick -= tick;
        }
        #endregion
    }
}
