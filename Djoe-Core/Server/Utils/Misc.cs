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

        public static void Delay(int ms, Action action)
        {
            Task.Delay(ms).ContinueWith((t) => action());
        }

        public static void StopTimer(System.Timers.Timer timer) => timer.Stop();

        public static int GetHashKey(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text.ToLowerInvariant());

            uint num = 0u;

            for (int i = 0; i < bytes.Length; i++)
            {
                num += (uint)bytes[i];
                num += num << 10;
                num ^= num >> 6;
            }
            num += num << 3;
            num ^= num >> 11;

            return (int)(num + (num << 15));
        }

    }
}
