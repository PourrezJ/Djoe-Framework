using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using CitizenFX.Core;
using Server.Utils;
using Shared;
using Server.Scripts;
using System.Linq;
using Shared.Utils;
using Server.Entities;

namespace Server.Database
{
    public static class MongoDB
    {
        #region Private static variables
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        #endregion

        #region Private methods

        #endregion

        #region Public static methods
        public static bool Init()
        {
            Logger.Info("MongoDB Starting ...");

            try
            {
                string host = Config.Get<string>("Host");
                string databaseName = Config.Get<string>("Database");
                string user = Config.Get<string>("User");
                string password = Config.Get<string>("Password");
                int port = Config.Get<int>("Port");

                if (!string.IsNullOrEmpty(host))
                    _client = new MongoClient($"mongodb://{user}:{password}@{host}:{port}");
                else
                    _client = new MongoClient();

                _database = _client.GetDatabase("redm");

                var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
                ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

                /*
                BsonSerializer.RegisterSerializer(typeof(Vector3), new VectorSerializer());

                BsonClassMap.RegisterClassMap<Location>(cm =>
                {
                    cm.AutoMap();
                    cm.MapProperty(c => c.Pos).SetSerializer(new VectorSerializer());
                    cm.MapProperty(c => c.Rot).SetSerializer(new VectorSerializer());
                });
                */
                BsonClassMap.RegisterClassMap<PlayerData>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(p => p.SteamID);
                    cm.SetIgnoreExtraElements(true);
                    cm.UnmapField("Client");
                    cm.UnmapField("NeedUpdate");
                });

                BsonClassMap.RegisterClassMap<HorseData>(cm =>
                {
                    cm.AutoMap();
                    //cm.MapIdMember(p => p.OwnerID);
                    cm.SetIgnoreExtraElements(true);
                    cm.UnmapField("NeedUpdate");
                });

                var players = GetCollectionSafe<PlayerData>("players").AsQueryable();

                Logger.Info("Mise en cache des " + players.Count() + " joueurs");

                foreach (var player in players)
                {
                    // Todo: ajouter un check nombre de temps sans avoir jouer?
                    PlayerManager.Characters.TryAdd(player.SteamID, player);
                }

                var horses = GetCollectionSafe<HorseData>("horses").AsQueryable();

                Logger.Info("Mise en cache des " + horses.Count() + " chevaux");

                foreach (var horse in horses)
                {
                    // Todo: ajouter un check nombre de temps sans avoir bouger?
                    if (HorseManager.HorseDatas.ContainsKey(horse.OwnerID))
                        HorseManager.HorseDatas[horse.OwnerID].Add(horse);
                    else
                        HorseManager.HorseDatas.Add(horse.OwnerID, new System.Collections.Generic.List<HorseData>() { horse });
                }

                Logger.Info("MongoDB Started!");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        public static void Insert<T>(string collectionName, T objet, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                GetCollectionSafe<T>(collectionName).InsertOne(objet);
            }
            catch (MongoWriteException be)
            {
                Logger.Error(be);
            }
        }


        public async static Task InsertAsync<T>(string collectionName, T objet, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                await GetCollectionSafe<T>(collectionName).InsertOneAsync(objet);
            }
            catch (MongoWriteException be)
            {
                Logger.Error(be);
            }
        }

        public async static Task<ReplaceOneResult> Update<T>(T objet, string collectionName, object ID, int requests = 1, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                return await GetCollectionSafe<T>(collectionName).ReplaceOneAsync(Builders<T>.Filter.Eq("_id", ID), objet);
            }
            catch (BsonException be)
            {
                Logger.Error(be);
            }

            return null;
        }

        public async static Task<DeleteResult> Delete<T>(string collectionName, object ID, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                if (Config.Get<bool>("DBProfiling"))
                    Logger.Info($"~m~{caller}() in {file.Substring(file.LastIndexOf('\\') + 1)} line {line} - Object ID: {ID}");

                return await _database.GetCollection<T>(collectionName).DeleteOneAsync(Builders<T>.Filter.Eq("_id", ID));
            }
            catch (BsonException be)
            {
                Logger.Error(be);
            }

            return null;
        }

        public static IMongoCollection<T> GetCollectionSafe<T>(string collectionName)
        {
            if (_database == null)
                return null;

            if (_database.GetCollection<T>(collectionName) == null)
                _database.CreateCollection(collectionName);

            return _database.GetCollection<T>(collectionName);
        }

        public static bool CollectionExist<T>(string collectionName)
        {

            if (_database == null)
                return false;

            if (_database.GetCollection<T>(collectionName) == null)
                return false;

            if (_database.GetCollection<T>(collectionName).CountDocuments(new BsonDocument()) == 0)
                return false;

            return true;
        }

        public static IMongoDatabase GetMongoDatabase() => _database;

        /*
        public async static Task<UpdateResult> UpdateBankAccount(BankAccount bankAccount, int requests, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                if (Config.Get<bool>("DBProfiling"))
                    Logger.Trace($"~m~{caller}() in {file.Substring(file.LastIndexOf('\\') + 1)} line {line} - Requests: {requests} - Bank account: {bankAccount.AccountNumber} - Type: {bankAccount.AccountType.ToString()}");

                if (bankAccount.AccountType == AccountType.Business)
                {
                    var collection = GetCollectionSafe<Business>("businesses");
                    var filter = Builders<Business>.Filter.Eq("_id", ((Business)bankAccount.Owner)._id);
                    var update = Builders<Business>.Update.Set("BankAccount", bankAccount);
                    return await collection.UpdateOneAsync(filter, update);
                }
            }
            catch (BsonException be)
            {
                Logger.Error(be);
            }

            return null;
        }*/
        #endregion
    }
}
