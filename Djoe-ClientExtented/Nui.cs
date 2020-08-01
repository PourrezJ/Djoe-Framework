using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace ClientExtented
{
    public class Nui
    {
        public string Action;
        public dynamic Data;

        public void SendNuiMessage()
        {
            API.SendNuiMessage(JsonConvert.SerializeObject(this));
        }
    }
}
