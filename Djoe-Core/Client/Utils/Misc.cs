using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Client.Utils
{
    public class Misc
    {
         /*
         * LoadModel | "int hash" is a hash key from model
         * Wait for Model hash Load in cache
         */
        public static async Task<bool> LoadTexture(string texture)
        {
            if (!Function.Call<bool>((Hash)0x7332461FC59EB7EC, texture))
            {
                if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, texture))
                    return true;

                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, texture);
                while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, texture))
                {
                    Debug.WriteLine($"Waiting for texture {texture} load!");
                    await BaseScript.Delay(25);
                }
                return true;
            }
            else
            {
                Debug.WriteLine($"Texture {texture} is not valid!");
                return false;
            }
        }

        //Wait a moment ???? DrawTxt have co.... ah sure no problem, DrawText is Patented?
        public static void DrawText(string text, int font, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, r, g, b, alpha);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);
            if (shadow) { Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255); }
            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, font);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
        }
    }
}
