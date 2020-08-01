using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.External;
using System;
using System.Drawing;

namespace ClientExtented
{
    /*Colors: 
    ~e~Red 
    ~o~Yellow 
    ~d~Orange 
    ~m~Grey 
    ~q~White 
    ~t~Light Grey 
    ~v~Black 
    ~u~Pink
    ~pa~Blue 
    ~t1~Purple 
    ~t2~Orange 
    ~t3~Light Blue 
    ~t4~Yellow 
    ~t5~Light Pink 
    ~t6~Green 
    ~t7~Dark Blue 
    ~t8~Light RedIsh
    */

    public static class UIHelper
    {
        public enum MarkerHash
        {
            Box = 1857541051,
            Cylinder = -1795314153
        }

        public static void DrawTexture(string textureStreamed, string textureName, float x, float y, float width, float height, float rotation, Color color)
        {
            if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureStreamed))
            {
                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureStreamed, false);
            }
            else
            {
                Function.Call(Hash.DRAW_SPRITE, textureStreamed, textureName, x, y, width, height, rotation, color.R, color.G, color.B, color.A);
            }
        }

        public static void DrawRect(float fromX, float fromY, float width, float height, Color color)
        {
            Function.Call(Hash.DRAW_RECT, fromX, fromY, width, height, color.R, color.G, color.B, color.A);
        }

        public static void DrawText(string text, float x, float y, float fontscale, float fontsize, Color color, bool textcentred, bool shadow, Font font = Font.Unk0)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, color.R, color.G, color.B, color.A);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);

            if (shadow) 
                Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255);

            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, 1);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
            Function.Call((Hash)0xADA9255D, font);
        }

        public static void DrawText3D(string text, Vector3 pos, Color color, Font font = 0)
        {
            float screenX = 0;
            float screenY = 0;

            API.GetScreenCoordFromWorldCoord(pos.X, pos.Y, pos.Z, ref screenX, ref screenY);
            var camCoords = Misc.GetCamDirection();
            var distance = pos.DistanceToSquared(camCoords);

            float scale = (4.00001f / distance) * 0.5f;
            if (scale > 0.2)
                scale = 0.2f;
            if (scale < 0.1)
                scale = 0;


            var fov = (1 / API.GetGameplayCamFov()) * 100;
            scale = scale * fov;

            UIHelper.DrawText(text, screenX, screenY, scale, scale, Color.FromArgb(255, 255, 255), false, false, font);
        }

        public static void DrawMarker(MarkerHash hash, Vector3 position, Vector3 direction, Vector3 rotation, Vector3 scale, Color color, bool bolUpAndDown, bool faceCamera, int unk, bool rotate, string textureDict, string textureName, bool drawOnEnt)
        {
            Function.Call((Hash)0x2A32FAA57B937173,
                hash,
                position.X, position.Y, position.Z,
                direction.X, direction.Y, direction.Z,
                rotation.X, rotation.Y, rotation.Z,
                scale.X, scale.Y, scale.Z,
                color.R, color.G, color.B, color.A,
                bolUpAndDown, faceCamera, 0, false, "", "", drawOnEnt
                );
        }

        public static void LoadingScreenText(string texthaut, string textmilieu, string textbas)
        {
            Function.Call((Hash)0x1E5B70E53DB661E5, 1122662550, 347053089, 0, texthaut, textmilieu, textbas);
        }
    }
}
