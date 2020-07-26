using CitizenFX.Core;
using System;

namespace Client.Ui
{
    public class Notifications : BaseScript
    {
        public static Notifications Instance;

        public Notifications()
        {
            Instance = this;
            //Notifications Based on Key_Value & Disquse & Vespura natives | Thanks for share.

            EventHandlers["djoe:Tip"] += new Action<string, int>(NotifyTip);
            EventHandlers["djoe:TipRight"] += new Action<string, int>(NotifyDisplayRightTip);
            EventHandlers["djoe:TipBottom"] += new Action<string, int>(NotifyDisplayObjetive);
            EventHandlers["djoe:NotifyTop"] += new Action<string, string, int>(NotifyDisplayTopCenterNotification);
            EventHandlers["djoe:NotifyLeft"] += new Action<string, string, string, string, int>(NotifyDisplayLeftNotification);
        }

        public static async void NotifyDisplayLeftNotification(string title, string text, string dic, string icon, int miliseconds)
        {
            await Utils.Misc.LoadTexture(dic);

            Instance.Exports["djoe_core"].DisplayLeftNotification(title, text, dic, icon, miliseconds);
        }

        public static void NotifyDisplayTopCenterNotification(string text, string location, int miliseconds)
        {
            Instance.Exports["djoe_core"].DisplayTopCenterNotification(text, location, miliseconds);
        }

        public static void NotifyDisplayObjetive(string text, int miliseconds)
        {
            Instance.Exports["djoe_core"].DisplayObjetive(text, miliseconds);
        }

        public static void NotifyDisplayRightTip(string text, int miliseconds)
        {
            Instance.Exports["djoe_core"].DisplayRightTip(text, miliseconds);
        }

        public static void NotifyTip(string text, int miliseconds)
        {
            Instance.Exports["djoe_core"].DisplayTip(text, miliseconds);
        }
    }
}
