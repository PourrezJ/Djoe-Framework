using Server.Entities;
using Shared;

namespace Server.Utils.Extensions
{
    public static class PedExtensions
    {
        public static void Delete(this PedNetwork ped)
        {
            PedsManager.Delete(ped);
        }
    }
}
