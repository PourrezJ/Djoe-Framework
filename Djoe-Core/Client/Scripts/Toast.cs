using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ClientExtented;

namespace Client.Scripts
{
    public class Toast : BaseScript
    {
        class Toasty
        {
            public string Text { get; set; }
            public float X { get; set; }
            public float Y { get; set; }

            public int StartTime { get; set; }
            public int Duration { get; set; }

            bool m_isFadingIn = true;
            bool m_isFadingOut = false;

            int m_alpha = 0;
            float m_fadeX;
            bool m_firstDraw = true;

            private bool FadeIn()
            {
                m_alpha += (int)(1.0f + Function.Call<float>(Hash.GET_FRAME_TIME)) * 10;

                if (m_alpha < 200)
                {
                    return true;
                }

                m_alpha = 200;

                return false;
            }

            private void FadeOut()
            {
                m_alpha -= (int)(1.0f + Function.Call<float>(Hash.GET_FRAME_TIME)) * 10;

                if (m_alpha < 0)
                {
                    m_alpha = 0;
                }
            }

            public void DrawToast()
            {
                if (m_firstDraw)
                {
                    Function.Call((Hash)0xCE5D0FFE83939AF1, -1, "INFO_HIDE", "HUD_SHOP_SOUNDSET", 1);
                    m_fadeX = X + 0.25f;
                    m_firstDraw = false;
                }

                if (m_fadeX > X) m_fadeX -= Function.Call<float>(Hash.GET_FRAME_TIME) * 1f;
                if (m_fadeX < X) m_fadeX = X;

                float height = 0.035f;

                if (Text.Contains("\n"))
                    height *= Text.Split('\n').Count();

                if (m_isFadingIn)
                {
                    m_isFadingIn = FadeIn();
                }

                m_isFadingOut = (Function.Call<int>(Hash.GET_GAME_TIMER) - StartTime) > Duration - 100;

                if (m_isFadingOut)
                {
                    FadeOut();
                }

                UIHelper.DrawText(Text, m_fadeX - (0.3f / 2) + 0.01f, Y - (height / 2) + 0.002f, 0.1f, 0.4f, Color.FromArgb(255, 255, 255), false, false);
                //UIHelper.DrawRect(X, Y, 0.3f, height, Color.FromArgb(0,0,0));
                UIHelper.DrawTexture("menu_textures", "translate_bg_1a", m_fadeX - 0.01f, Y - 0.01f, 0.3f, height, 0, Color.FromArgb(0, 0, 0));
            }
        }

        static List<Toasty> ms_activeToasts = new List<Toasty>();
        static List<Toasty> ms_toastsToRemove = new List<Toasty>();

        public Toast()
        {
            Tick += OnTick;
        }

        public static void AddToast(string text, int durationMsec, float xPos = 0.88f, float yPos = 0.2f)
        {
            if (ms_activeToasts.Any(t => t.Y == yPos))
            {
                ms_toastsToRemove.AddRange(ms_activeToasts.Where(t => t.Y == yPos));
            }

            ms_activeToasts.Add(new Toasty()
            {
                Text = text,
                X = xPos,
                Y = yPos,
                Duration = durationMsec,
                StartTime = Function.Call<int>(Hash.GET_GAME_TIMER)
            });
        }

        private Task OnTick()
        {
            if (ms_activeToasts.Count > 0)
            {
                ms_activeToasts.ForEach(t =>
                {
                    t.DrawToast();

                    int currentTime = Function.Call<int>(Hash.GET_GAME_TIMER);
                    if (currentTime - t.StartTime > t.Duration)
                    {
                        ms_toastsToRemove.Add(t);
                    }
                });
            }

            if (ms_toastsToRemove.Count > 0)
            {
                ms_toastsToRemove.ForEach(t => ms_activeToasts.Remove(t));
                ms_toastsToRemove.Clear();
            }
            return Task.FromResult(0);
        }
    }
}