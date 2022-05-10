using System;
using GTANetworkAPI;

namespace freemode
{
    class Events : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            mysql.InitConnection();
        }

        [ServerEvent(Event.PlayerConnected)]
        private void OnPlayerConnected(Player player)
        {
            player.SendChatMessage("~g~Добро пожаловать на сервер ~w~Rus~b~sia ~r~RP!");

            if (mysql.IsAccountExist(player.Name))
            {
                player.SendChatMessage("~w~Ваш аккаунт уже ~g~зарегистрирован ~w~на сервере. Используйте /login для авторизации.");
            }
            else
            {
                player.SendChatMessage("~w~Ваш аккаунт ~r~не зарегистрирован ~w~на сервере. Используйте /register для регистрации.");
            }

        }

        [ServerEvent(Event.PlayerSpawn)]
        private void OnPlayerSpawn(Player player)
        {
            player.Health = 50;
            player.Armor = 0;
            //  \/\/\/\/\/ Пока не работает, надо разобраться почему \/\/\/\/\/
            //
            //NAPI.TextLabel.CreateTextLabel("Добро пожаловать на ~w~Rus~b~sia ~r~RP!", new Vector3(253.9534, 225.2, 102.22),
            //    4F, 0.3F, 0, new Color(10, 240, 30));
            //
            //  /\/\/\/\/\                                           /\/\/\/\/\
        }

    }
}
