using System;
using System.Collections.Concurrent;

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
        public static ConcurrentDictionary<string, VIP> VIPS = new ConcurrentDictionary<string, VIP>();
        
        public VIPLevel VIPLevel;
        public DateTime Souscription;
        public DateTime EndSouscription;
    }
}
