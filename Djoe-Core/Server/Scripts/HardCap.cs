using CitizenFX.Core;
using CitizenFX.Core.Native;
using Server.Entities;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Scripts
{
    public class HardCap : BaseScript
    {
        public static Dictionary<int, DateTime> ActivePlayers = new Dictionary<int, DateTime>();
        public static int MaxClients;
        public static int PlayerCount;

        public HardCap()
        {
            MaxClients = API.GetConvarInt("sv_maxclients", 32);

            EventHandlers["playerDropped"] += new Action<Player, string>(PlayerDropped);
            EventHandlers["playerConnecting"] += new Action<Player, string, CallbackDelegate>(PlayerConnecting);
            EventHandlers["HardCap.PlayerActivated"] += new Action<Player>(PlayerActivated);
        }

        private static void PlayerConnecting([FromSource] Player source, string playerName, CallbackDelegate DenyWithReason)
        {
            try
            {
                Debug.WriteLine($"Connecting: '{source.Name}' (steam: {source.Identifiers.Where(i => i.Contains("steam")).FirstOrDefault().ToString()} ip: {source.Identifiers.Where(i => i.Contains("ip")).FirstOrDefault().ToString()}) | Player count {ActivePlayers.Count}/{MaxClients}");

                if (source.Identifiers["steam"] == null)
                {
                    DenyWithReason?.Invoke($"Steam identifiant introuvable. Est t-il démarrer?");
                    API.CancelEvent();
                    return;
                }

                if (ActivePlayers.Count >= MaxClients)
                {
                    DenyWithReason?.Invoke($"The server is full with {PlayerCount}/{MaxClients} players on.");
                    API.CancelEvent();
                    return;
                }
                BaseScript.TriggerClientEvent("playerConnecting", source.Handle, playerName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HardCap PlayerConnecting Error: {ex.Message}");
            }
        }

        internal void PlayerDropped([FromSource] Player source, string reason)
        {
            try
            {
                int sessionId = int.Parse(source.Handle);
                if (ActivePlayers.ContainsKey(sessionId)) ActivePlayers.Remove(sessionId);
                BaseScript.TriggerClientEvent("playerDropped", source.Handle, reason);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HardCap PlayerDropped Error: {ex.Message}");
            }
        }

        public static void PlayerActivated(Player source)
        {
            Console.WriteLine("==== PlayerActivated ====");
            int sessionId = int.Parse(source.Handle);
            if (!ActivePlayers.ContainsKey(sessionId))
            {
                if (ActivePlayers.Count == 0)
                {
                    // Il va host la session
                    Logger.Info($"Le joueur {source.Name} {source.Identifiers["steam"]} host la session.");
                    HorseManager.SpawnAllHorses(source);
                }
                ActivePlayers.Add(sessionId, DateTime.UtcNow);
            }
        }
    }
}
