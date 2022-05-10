using System;
using GTANetworkAPI;

namespace freemode
{
    class Commands : Script
    {

        [Command("spawn_car", "/spawn_car [Хэш авто] [Осн. цвет] [Доп. цвет]", Alias = "scar")]
        private void cmd_car(Player player, string carname, int color1, int color2)
        {
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
            NAPI.ClientEvent.TriggerClientEvent(target, "PlayerFreeze", freezestatus);
        }

        [Command("gun", "/gun [хэш оружия]", Alias = "weapon")]
        private void cmd_giveWeapon(Player player, WeaponHash hash)
        {
            NAPI.Player.GivePlayerWeapon(player, hash, 500);
        }

        [Command("heal", "/heal [ник игрока]", Alias = "h")]
        private void cmd_heal(Player player)
        {
            NAPI.Player.SetPlayerHealth(player, 100);
        }
        
        [Command("armor", "/armor [ник игрока]", Alias = "ar")]
        private void cmd_armor(Player player)
        {
            NAPI.Player.SetPlayerArmor(player, 100);
        }

    }
}
