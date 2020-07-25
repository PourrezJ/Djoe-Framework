using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils
{
    public static class Misc
    {
        public static System.Timers.Timer SetInterval(Action action, int ms)
        {
            var t = new System.Timers.Timer(ms);
            t.Elapsed += (s, e) => action();
            t.Start();
            return t;
        }

        public static void StopTimer(System.Timers.Timer timer) => timer.Stop();
    }
}
