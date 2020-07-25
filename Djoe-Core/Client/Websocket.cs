using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace Client
{
    class Websocket
    {
        public static void Init()
        {
            WebSocket websocket = new WebSocket("ws://localhost:1988/");
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }

        private static void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.WriteLine("websocket_MessageReceived");
        }

        private static void websocket_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine("websocket_Closed");
        }

        private static void websocket_Error(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine("websocket_Error");
        }

        private static void websocket_Opened(object sender, EventArgs e)
        {
            Debug.WriteLine("websocket_Opened");
        }
    }
}
