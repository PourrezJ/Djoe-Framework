using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace ClientExtented
{
    public static class Inputbox
    {
        public static async Task<string> GetUserInput(string defaultText, int maxLen = 22)
        {
            string output = null;

            Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, 1, "", "", defaultText, "", "", "", maxLen + 1);

            while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
            {
                await BaseScript.Delay(0);
            }

            if (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) != 1)
            {
                output = "";
            }

            output = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);

            return output;
        }
    }
}
