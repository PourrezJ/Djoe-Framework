﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.External;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

namespace Client.Controllers
{
    internal class PedsManager : BaseScript
    {
        public static Dictionary<Ped, PedNetwork> PedList = new Dictionary<Ped, PedNetwork>();

        public PedsManager()
        {
            EventHandlers["GetAllPeds"] += new Action<string>(CreateAllPeds);
            EventHandlers["CreatePedFromSrv"] += new Action<string>(CreatePedFromSrv);
            EventHandlers["onResourceStop"] += new Action<string>(OnClientResourceStop);

        }

        private void OnClientResourceStop(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName)
                return;

            Debug.WriteLine("Supression des peds.");
            lock (PedList)
            {
                foreach(var ped in PedList)
                {
                    ped.Key?.Delete();
                }
            }
        }

        private static async void CreatePedFromSrv(string data)
        {
            PedNetwork peddata = JsonConvert.DeserializeObject<PedNetwork>(data);

            if (PedList.ContainsValue(peddata))
                return;

            Ped ped = await World.CreatePed(new Model(peddata.Model), peddata.LastCoord.ToVector3(), peddata.LastCoord.Heading, peddata.Networked);

            if (ped == null)
                return;

            while (!ped.Exists())
                await BaseScript.Delay(10);

            if (peddata.TaskWanderStandard)
                ped.Tasks.WanderAround();

            ped.IsPositionFrozen = peddata.IsPositionFrozen;
            ped.IsInvincible = peddata.Invincible | peddata.IsPositionFrozen;
            ped.BlockPermanentEvents = true;
            ped.CanRagdoll = false;

            API.SetEntityCanBeDamaged(ped.Handle, false);

            PedList.Add(ped, peddata);

            Debug.WriteLine("ped created: " + peddata.NetworkID);
        }

        private static async void CreateAllPeds(string obj)
        {       
            var pedsData = JsonConvert.DeserializeObject<List<PedNetwork>>(obj);

            foreach(var pednet in pedsData)
            {
                if (PedList.ContainsValue(pednet))
                    continue;

               var ped = await World.CreatePed(pednet.Model, pednet.LastCoord.ToVector3(), pednet.LastCoord.Heading, pednet.Networked);

                if (ped == null)
                    return;

                while (!ped.Exists())
                    await Delay(10);

                if (pednet.TaskWanderStandard)
                    ped.Tasks.WanderAround();

                ped.IsPositionFrozen = pednet.IsPositionFrozen;

                PedList.Add(ped, pednet);

                Debug.WriteLine("ped created: " + pednet.NetworkID);
            }
        }
    }
}
