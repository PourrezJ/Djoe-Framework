using CitizenFX.Core;

namespace Server
{
    public static class MenuExtension
    {
        public static bool OpenMenu(this Shared.MenuManager.Menu menu, Player player, string customData = "")
        {
           return MenuManager.OpenMenu(player, menu, customData);
        }
    }
}
