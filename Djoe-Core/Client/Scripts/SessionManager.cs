using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented;
using ClientExtented.External;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SessionManager : BaseScript
    {
        private static bool showDebugInfo = true;

        public SessionManager()
        {
            API.RegisterCommand("netinfo", new Action<int, List<object>, string>((source, args, raw) =>
            {
                showDebugInfo = !showDebugInfo;
            }), false);

            EventHandlers["LeaveSession"] += new Action(LeaveSession);
            EventHandlers["ChangeSession"] += new Action<int>(ChangeSession);
        }

        private void ChangeSession(int id)
        {
            Debug.WriteLine("Changed session");
            Function.Call((Hash)0x4019AE4956D4393, 0, IntPtr.Zero, id);
        }

        private void LeaveSession()
        {
            Debug.WriteLine("Leave Network Session");
            API.NetworkSessionLeave();
        }

        [Tick]
        public Task OnTick()
        {
            if (!showDebugInfo)
                return Task.FromResult(0);

            bool inSession = Function.Call<bool>(Hash.NETWORK_IS_IN_SESSION);
            bool isSessionActive = Function.Call<bool>(Hash.NETWORK_IS_HOST);
            bool isStarted = Function.Call<bool>(Hash.NETWORK_IS_SESSION_STARTED);
            bool isHost = Function.Call<bool>(Hash.NETWORK_IS_SESSION_ACTIVE);

            UIHelper.DrawRect(0.895f, 0.58f, 0.22f, 0.3f, Color.FromArgb(200,0,0,0));
            UIHelper.DrawText("============== Network Infos ==============", 0.8f, 0.45f, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);
            UIHelper.DrawText("NETWORK_IS_IN_SESSION:       " + inSession.ToString(), 0.8f, 0.48f, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);
            UIHelper.DrawText("NETWORK_IS_SESSION_ACTIVE:   " + isSessionActive.ToString(), 0.8f, 0.50f, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);
            UIHelper.DrawText("NETWORK_IS_SESSION_STARTED:  " + isStarted.ToString(), 0.8f, 0.52f, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);
            UIHelper.DrawText("NETWORK_IS_HOST:             " + isHost.ToString(), 0.8f, 0.54f, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);
            UIHelper.DrawText("NETWORK_PLAYER_ID_TO_INT:    " + API.GetPlayerServerId(Game.Player.Handle), 0.8f, 0.56f, 0.3f, 0.3f, Color.FromArgb(255, 255, 255), false, false);


            return Task.FromResult(0);
        }
    }
}
