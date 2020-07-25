using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace Inventory.Utils
{
    public class Nui
    {
        public string UIName;
        public string Action;
        public dynamic Data;

        public void SendNuiMessage()
        {
            API.SendNuiMessage(JsonConvert.SerializeObject(this));
        }
    }
}
