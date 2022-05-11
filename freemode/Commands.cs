using System;
using GTANetworkAPI;
using RAGE;

namespace freemode
{
    class Commands : Script
    {

        [Command("spawn_car", "/spawn_car [Хэш авто] [Осн. цвет] [Доп. цвет]", Alias = "scar")]
        private void cmd_car(Player player, string carname, int color1, int color2)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Helper))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            uint carHash = NAPI.Util.GetHashKey(carname);
            if (carHash <= 0)
            {
                player.SendChatMessage("~r~ [ОШИБКА] - Неверная модель т/с");
            }
            Vehicle car = NAPI.Vehicle.CreateVehicle(carHash, player.Position, player.Heading, color1, color2);
            car.NumberPlate = "A777AA";
            car.Locked = false;
            car.EngineStatus = true;
            player.SetIntoVehicle(car, (int)VehicleSeat.Driver);
        }

        [Command("freeze", "/freeze [ник игрока] [true/false]", Alias = "fr")]
        private void cmd_freezeplayer(Player player, Player target, bool freezestatus)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Moderator))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            NAPI.ClientEvent.TriggerClientEvent(target, "PlayerFreeze", freezestatus);
        }

        [Command("gun", "/gun [хэш оружия]", Alias = "weapon")]
        private void cmd_giveWeapon(Player player, WeaponHash hash)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Helper))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            NAPI.Player.GivePlayerWeapon(player, hash, 500);
        }

        [Command("heal", "/heal восстанавливает хп", Alias = "h")]
        private void cmd_heal(Player player)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Helper))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            NAPI.Player.SetPlayerHealth(player, 100);
        }

        [Command("armor", "/armor - восстанавливает броню", Alias = "ar")]
        private void cmd_armor(Player player)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Helper))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            NAPI.Player.SetPlayerArmor(player, 100);
        }

        [Command("ha", "/ha [ник игрока]", Alias = "ah")]
        private void cmd_healthAndArmor(Player player)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Helper))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            NAPI.Player.SetPlayerArmor(player, 100);
            NAPI.Player.SetPlayerHealth(player, 100);
        }

        [Command("login", "/login [пароль]", Alias = "l")]
        private void cmd_login(Player player, string password)
        {
            if (Accounts.IsPlayerLoggedIn(player))
            {
                player.SendNotification("~r~Вы уже авторизованы!");
                return;
            }
            if (!mysql.IsAccountExist(player.Name))
            {
                player.SendNotification("~r~Вы не зарегистрированы!");
                return;
            }
            if (!mysql.IsValidPassword(player.Name, password))
            {
                player.SendNotification("~r~Пароль неверный!");
                return;
            }
            Accounts account = new Accounts(player.Name, player);
            account.Login(player, false);
            NAPI.ClientEvent.TriggerClientEvent(player, "PlayerFreeze", false);
        }

        [Command("register", "/register [пароль]", Alias = "reg")]
        private void cmd_register(Player player, string password)
        {
            if (Accounts.IsPlayerLoggedIn(player))
            {
                player.SendNotification("~r~Вы уже авторизованы!");
                return;
            }
            if (mysql.IsAccountExist(player.Name))
            {
                player.SendNotification("~r~Вы уже зарегистрированы!");
                return;
            }

            Accounts account = new Accounts(player.Name, player);
            account.Register(player.Name, password);
            NAPI.ClientEvent.TriggerClientEvent(player, "PlayerFreeze", false);
        }
        [Command("help", "/help - список команд", Alias = "commands")]
        private void cmd_help(Player player)
        {
            player.SendChatMessage("~g~--------СПИСОК КОМАНД-------");
            player.SendChatMessage(">    /spawn_car - спавнит авто");
            player.SendChatMessage(">    /freeze - замораживает игрока");
            player.SendChatMessage(">    /gun - выдает оружие игроку");
            player.SendChatMessage(">    /heal - восстанавливает хп игроку");
            player.SendChatMessage(">    /armor - восстанавливает броню игроку");
            player.SendChatMessage(">    /weather - устанавливает погоду");
            player.SendChatMessage(">    /time - устанавливает время");
            player.SendChatMessage("~g~---------------------------------");
        }

        [Command("kill", "/kill [игрок]")]
        private void cmd_kill(Player player)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Admin))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }
            NAPI.Player.SetPlayerHealth(player, 0);
        }

        /*
        
        СДЕЛАТЬ ТП НА waypoint!!!!!
        [Command("tp")]
        private void cmd_teleport(Player player)
        {
            NAPI.Entity.SetEntityPosition(player, new Vector3(NAPI.Blip.GetBlipPosition(player).x, NAPI.Blip.GetBlipPosition(player).y, NAPI.Blip.GetBlipPosition(player).z));
        }
        */

        [Command("weather", "/weather [id погоды]", Alias = "wthr")]
        private void cmd_weather(Player player, byte weatherId)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Admin))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }

            player.SendChatMessage("~g~--------СПИСОК ПОГОДЫ-------");
            player.SendChatMessage("> 0 - EXTRASUNNY");
            player.SendChatMessage("> 1 - CLEAR");
            player.SendChatMessage("> 2 - CLOUDS");
            player.SendChatMessage("> 3 - SMOG");
            player.SendChatMessage("> 4 - FOGGY");
            player.SendChatMessage("> 5 - OVERCAST");
            player.SendChatMessage("> 6 - RAIN");
            player.SendChatMessage("> 7 - THUNDER");
            player.SendChatMessage("> 8 - CLEARING");
            player.SendChatMessage("> 9 - NEUTRAL");
            player.SendChatMessage("> 10 - SNOW");
            player.SendChatMessage("> 11 - BLIZZARD");
            player.SendChatMessage("> 12 - SNOWLIGHT");
            player.SendChatMessage("> 13 - HALLOWEEN");
            player.SendChatMessage("~g~--------------------------------");

            string weatherType = " ";

            try
            {
                switch (weatherId)
                {
                    case 0: weatherType = "EXTRASUNNY";
                        break;
                    case 1:
                        weatherType = "CLEAR";
                        break;
                    case 2:
                        weatherType = "CLOUDS";
                        break;
                    case 3:
                        weatherType = "SMOG";
                        break;
                    case 4:
                        weatherType = "FOGGY";
                        break;
                    case 5:
                        weatherType = "OVERCAST";
                        break;
                    case 6:
                        weatherType = "RAIN";
                        break;
                    case 7:
                        weatherType = "THUNDER";
                        break;
                    case 8:
                        weatherType = "CLEARING";
                        break;
                    case 9:
                        weatherType = "NEUTRAL";
                        break;
                    case 10:
                        weatherType = "SNOW";
                        break;
                    case 11:
                        weatherType = "BLIZZARD";
                        break;
                    case 12:
                        weatherType = "SNOWLIGHT";
                        break;
                    case 13:
                        weatherType = "HALLOWEEN";
                        break;
                    default:
                        weatherType = "XMAS";
                        break;
                }
                NAPI.World.SetWeather(weatherType);
            }
            catch{

            }
        }

        [Command("time", "/time [час] [минута]", Alias = "settime")]
        private void cmd_time(Player player, int hours, int minutes)
        {
            Accounts account = player.GetData<Accounts>(Accounts._accountKey);
            if (!account.IsPlayerHasAdminLevel((int)Accounts.AdminRanks.Admin))
            {
                player.SendNotification("~r~У вас недостаточно прав для этой команды!");
                return;
            }

            NAPI.World.SetTime(hours, minutes, 0);
            NAPI.Chat.SendChatMessageToAll($"~r~установлено время {hours}:{minutes}");
        }
    }
}
