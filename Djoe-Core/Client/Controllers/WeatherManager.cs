using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Client
{
    public static class WeatherManager
    {
        private static int _tick;

        public static void Init()
        {
            //API.N_0x59174f1afe095b5a(RDResurrection_Shared.Utils.GetHashKey(WeatherType.Sunny.ToString()), 1, 0, 1, 1, 0);
        }

        public static Task OnTick()
        {
            /*
            _tick++;
            if (_tick < 20)
                return Task.FromResult(0);

            _tick = 0;

            API.N_0x59174f1afe095b5a(RDResurrection_Shared.Utils.GetHashKey(Weather.SUNNY.ToString()), 1, 0, 1, 1, 0);
            */
            return Task.FromResult(0);
        }
    }
}
