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

        public static PedNetwork CreatePed(Player client, PedHash pedHash, UCoords position, bool isFrozen, PedType pedType = PedType.Pedestrial)
        {
            lock (PedsList)
            {
                var ped = new PedNetwork()
                {
                    Model = (int)pedHash,
                    LastCoord = position,
                    Networked = true,
                    IsPositionFrozen = isFrozen,
                    NetworkID = PedsList.Count + 1,
                    PedType = pedType
                };

                PedsList.Add(ped);

                client.TriggerEvent("CreatePedFromSrv", JsonConvert.SerializeObject(ped));
                return ped;
            }
        }

        public static void OnPlayerConnected(Player player)
        {
            lock (PedsList)
            {
                if (PedsList.Count > 0)
                    player.TriggerEvent("GetAllPeds", Newtonsoft.Json.JsonConvert.SerializeObject(PedsList));
            }
        }

        public static PedNetwork GetPedWithPos(Vector3 pos, float distance = 1.5f)
        {
            lock (PedsList)
            {
                return PedsList.Find(p => p.LastCoord.ToVector3().DistanceToSquared(pos) < distance && p.PedType == PedType.Pedestrial);
            }
        }

        public static PedNetwork GetHorseWithPos(Vector3 pos, float distance = 1.5f)
        {
            lock (PedsList)
            {
                return PedsList.Find(p => p.LastCoord.ToVector3().DistanceToSquared(pos) < distance && p.PedType == PedType.Horse);
            }
        }
    }
}
