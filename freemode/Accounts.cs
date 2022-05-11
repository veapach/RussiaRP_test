using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace freemode
{
    class Accounts
    {
        public enum AdminRanks { Player, Helper, Moderator, Admin };

        public const string _accountKey = "Player_Data";
        public int _id, _adminlevel;
        public string _name;
        public long _cash;
        public Player _player;

        public Accounts()
        {
            this._name = "";
            this._cash = 1000;
            this._adminlevel = 0;
        }

        public Accounts(string name, Player player, long cash = 1000)
        {
            this._name = name;
            this._player = player;
            this._cash = cash;
            this._adminlevel = 0;
        }

        public static bool IsPlayerLoggedIn(Player player)
        {
            if (player != null) return player.HasData(_accountKey);
            return false;
        }

        public bool IsPlayerHasAdminLevel(int adminlevel)
        {
            return this._adminlevel >= adminlevel;
        }

        public void Register(string name, string password)
        {
            mysql.NewAccountRegister(this, password);
            Login(_player, true);
        }

        public void Login(Player player, bool isFirstLogin)
        {
            mysql.LoadAccount(this);
            if (isFirstLogin)
            {
                player.SendChatMessage("Вы успешно зарегистрированы!");
            }
            else
            {
                player.SendChatMessage("Вы успешно авторизовались!");
            }

            player.SetData(Accounts._accountKey, this);
        }
    }
}
