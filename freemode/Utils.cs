using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace freemode
{
    class Utils
    {

        public static List<Player> players = new List<Player> { };

        public static void SendRadiusMessage(string message, int radius, Player player)
        {
            foreach (Player p in NAPI.Pools.GetAllPlayers())
            {
                if (Accounts.IsPlayerLoggedIn(p) && IsInRangeOfPoint(p.Position, player.Position, radius))
                {
                    NAPI.Chat.SendChatMessageToPlayer(p, message);
                }
            }
        }

        public static bool IsInRangeOfPoint(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }

        public static int GetPlayerID(string name)
        {
            int counter = 0;
            foreach (Player p in NAPI.Pools.GetAllPlayers())
            {
                if (p.Handle.ToString() == name || p.Name.ToLower().Contains(name.ToLower()))
                {
                    return counter;
                }
                counter++;
            }
            return 0;
        }

        public static Player GetPlayerObject(int id)
        {
            foreach(Player player in players)
            {
                if (player.Value == id) return player;
            }
            return null;
        }

    }
}
