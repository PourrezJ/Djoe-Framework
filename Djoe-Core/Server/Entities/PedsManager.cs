﻿using CitizenFX.Core;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Server.Utils.Enums;
using Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Entities
{
    public static class PedsManager
    {
        public static List<Shared.PedNetwork> PedsList = new List<Shared.PedNetwork>();

        public delegate Task NpcPrimaryCallBackAsync(Player client, Ped npc);
        public delegate Task NpcSecondaryCallBackAsync(Player client, Ped npc);

        public delegate void NpcPrimaryCallBack(Player client, Ped npc);
        public delegate void NpcSecondaryCallBack(Player client, Ped npc);

        [JsonIgnore, BsonIgnore]
        public static NpcPrimaryCallBackAsync NpcInteractCallBackAsync { get; set; }
        [JsonIgnore, BsonIgnore]
        public static NpcSecondaryCallBackAsync NpcSecInteractCallBackAsync { get; set; }

        [JsonIgnore, BsonIgnore]
        public static NpcPrimaryCallBack NpcInteractCallBack { get; set; }
        [JsonIgnore, BsonIgnore]
        public static NpcSecondaryCallBack NpcSecInteractCallBack { get; set; }


        public static PedNetwork CreatePed(PedHash pedHash, UCoords position, bool networked, bool isFrozen)
        {
            lock (PedsList)
            {
                var ped = new PedNetwork()
                {
                    Model = (int)pedHash,
                    LastCoord = position,
                    Networked = networked,
                    IsPositionFrozen = isFrozen,
                    NetworkID = PedsList.Count + 1
                };

                PedsList.Add(ped);

                BaseScript.TriggerClientEvent("CreatePedFromSrv", JsonConvert.SerializeObject(ped));
                return ped;
            }
        }

        public static void OnPlayerConnected(Player player)
        {
            if (PedsList.Count > 0)
                player.TriggerEvent("GetAllPeds", Newtonsoft.Json.JsonConvert.SerializeObject(PedsList));
        }
    }
}