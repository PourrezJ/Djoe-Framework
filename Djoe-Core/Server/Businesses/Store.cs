using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Controllers;
using Server.Entities;
using Server.Loader;
using Server.Utils.Enums;
using Shared;

namespace Server.Stores
{
    public enum StoreType
    {
        Unknown = -1,
        GeneralStore = 0
    }

    public class Store
    {
        #region Fields
        public StoreType StoreType { get; private set; }

        public string BusinnessName { get; private set; } = "Undefined Name";
        public UCoords Location { get; private set; }

        public uint BlipSprite { get; private set; }

        public PedHash PedHash { get; private set; }


        [JsonIgnore]

        public BlipNetwork Blip { get; private set; }

        [JsonIgnore]
        protected PedNetwork Ped;
        #endregion

        #region Constructor
        public Store(string businnessName, UCoords location, BlipSprite blipSprite, BlipType blipType, PedHash pedhash = 0)
        {
            BusinnessName = businnessName;
            Location = location;
            BlipSprite = (uint)blipSprite;
            PedHash = pedhash;
        }
        #endregion

        #region Loader
        public virtual void Init()
        {
            if (PedHash != 0)
            {
                Ped = PedsManager.CreatePed(PedHash, Location, false, true);
            }

            Blip = BlipsManager.CreateBlip(BusinnessName, (BlipSprite)BlipSprite, BlipType.WeaponWhite, Location.ToVector3(), Location.Heading);

            lock (StoreLoader.BusinessesList)
            {
                StoreLoader.BusinessesList.Add(this);
            }
        }

        #endregion

        #region Events

        public void OnNpcFirstInteract(Player client, PedNetwork npc = null)
        {
            OpenMenu(client, npc);
        }

        public virtual void OpenMenu(Player client, PedNetwork npc) { }
        #endregion
    }
}
