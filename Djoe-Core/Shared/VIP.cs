using System;

namespace Shared
{
    public enum VIPLevel
    {
        Level0,
        Level1,
        Level2
    }

    public class VIP
    {
        public string SteamID;
        public VIPLevel VIPLevel;
        public DateTime Souscription;
        public DateTime EndSouscription;
    }
}
