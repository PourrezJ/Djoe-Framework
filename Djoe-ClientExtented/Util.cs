using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ClientExtended
{
    public static class Util
    {
        public static void Delay(int ms, Action action)
        {
            BaseScript.Delay(ms).ContinueWith((t) => action());
        }

        public static System.Timers.Timer SetInterval(Action action, int ms)
        {
            var t = new System.Timers.Timer(ms);
            t.Elapsed += (s, e) => action();
            t.Start();
            return t;
        }

        public static void StopTimer(System.Timers.Timer timer) => timer.Stop();

        public static async Task PerformRequest(int hash)
        {
            if (Function.Call<bool>(Hash.IS_MODEL_IN_CDIMAGE, hash) && Function.Call<bool>(Hash.IS_MODEL_VALID, hash))
            {
                Debug.WriteLine($"Requesting model {hash}");

                Function.Call(Hash.REQUEST_MODEL, hash);

                while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
                {
                    await BaseScript.Delay(50);
                }
            }
            else
            {
                Debug.WriteLine("Requested model not valid");
            }
        }

        public static int GetHashKey(string text)
        {
            //return Function.Call<int>(Hash.GET_HASH_KEY, text);

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

        public static long CreateVarString(string text)
            => Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);

        public static long CreateVarStringColor(string text, int color)
            => Function.Call<long>(Hash._CREATE_VAR_STRING, 42, "COLOR_STRING", Function.Call<long>(Hash._CREATE_COLOR_STRING, color), text);


        public static bool LoadScript(string script)
        {
            if (!Function.Call<bool>(Hash.DOES_SCRIPT_EXIST, script))
                return false;

            Function.Call(Hash.REQUEST_SCRIPT, script);
            if (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, script))
                return false;

            var scriptID = API.StartNewScript(script, 1024);
            Function.Call(Hash.SET_SCRIPT_WITH_NAME_HASH_AS_NO_LONGER_NEEDED, script);
            return true;
        }
    }
}
