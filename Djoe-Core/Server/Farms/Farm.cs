using CitizenFX.Core;
using Server.Controllers;
using Server.Utils;
using Server.Utils.Enums;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Farms
{
    public enum FarmType
    {
        Unknown = 0,
        Miner = 1
    }

    public abstract class Farm
    {
        public FarmType FarmType;

        public string FarmName;
        public Vector3 FarmBlipPos;
        public BlipSprite FarmBlipSprite;
        public UCoords[] FarmPos;



        public virtual void Init()
        {
            if (FarmBlipPos != null)
            {
                BlipsManager.CreateBlip(FarmName, FarmBlipSprite, BlipType.WeaponWhite, FarmBlipPos, 0);
            }

            Logger.Debug($"Farm {FarmName} initialized.");
        }

        public abstract void OnFarm(Player player);
    }
}
