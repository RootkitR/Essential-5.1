using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
using Essential.Storage;
using System.Data;
namespace Essential.Communication.Messages.Games
{
    internal sealed class PurchasePowerupPackage : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string PackageName = Event.PopFixedString();
            PowerupPackage PuPackage = Essential.GetGame().GetGamesManager().GetPowerupPackage(PackageName);

            if (PuPackage == null)
            {
                return;
            }

            int Credits = 0;
            int UpdatedCAmount = 0;
            int Bigparachutes = 0;
            int Missiles = 0;
            int Shields = 0;
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                Credits = dbClient.ReadInt32("SELECT credits FROM users WHERE id = '" + Session.Basejump_UserId + "' LIMIT 1");
                DataRow PuRow = dbClient.ReadDataRow("SELECT * FROM basejump_users_powerups WHERE user_id = '" + Session.Basejump_UserId + "' LIMIT 1;");
                Bigparachutes = int.Parse(PuRow["bigparachutes"].ToString());
                Missiles = int.Parse(PuRow["missiles"].ToString());
                Shields = int.Parse(PuRow["shields"].ToString());

                if (Missiles >= 9000 || Bigparachutes >= 9000 || Shields >= 9000)
                {
                    return;
                }
            }

            if (Credits < PuPackage.CostCredits)
            {
                // No credits for it.. Sorry. :(
                return;
            }
            UpdatedCAmount = Credits - PuPackage.CostCredits;

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                switch (PuPackage.PowerupType)
                {
                    case "missile":
                        Missiles += PuPackage.Amount;
                        dbClient.ExecuteQuery("UPDATE `basejump_users_powerups` SET `missiles`='" + Missiles + "' WHERE (`user_id`='" + Session.Basejump_UserId + "');");
                        break;
                    case "bigparachute":
                        Bigparachutes += PuPackage.Amount;
                        dbClient.ExecuteQuery("UPDATE `basejump_users_powerups` SET `bigparachutes`='" + Bigparachutes + "' WHERE (`user_id`='" + Session.Basejump_UserId + "');");
                        break;
                    case "shield":
                        Shields += PuPackage.Amount;
                        dbClient.ExecuteQuery("UPDATE `basejump_users_powerups` SET `shields`='" + Shields + "' WHERE (`user_id`='" + Session.Basejump_UserId + "');");
                        break;
                }
               
                GameClient RealClient = Essential.GetGame().GetClientManager().GetClientByHabbo(dbClient.ReadString("SELECT username FROM users WHERE id = '" + Session.Basejump_UserId + "' LIMIT 1"));
                if (RealClient != null)
                {
                    RealClient.GetHabbo().Credits -= PuPackage.CostCredits;
                    RealClient.GetHabbo().UpdateCredits(true);
                }
                else
                {
                    dbClient.ExecuteQuery("UPDATE `users` SET `credits`='" + UpdatedCAmount + "' WHERE (`id`='" + Session.Basejump_UserId + "')");
                }
            }

            ServerMessage SerializeCredits = new ServerMessage(16);
            SerializeCredits.AppendInt32(UpdatedCAmount);
            Session.SendMessage(SerializeCredits);

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
               


                ServerMessage SerializeUserPowerUps = new ServerMessage(14);
                SerializeUserPowerUps.AppendInt32(3);
                SerializeUserPowerUps.AppendInt32(0);
                SerializeUserPowerUps.AppendInt32(Bigparachutes);
                SerializeUserPowerUps.AppendInt32(1);
                SerializeUserPowerUps.AppendInt32(Missiles);
                SerializeUserPowerUps.AppendInt32(2);
                SerializeUserPowerUps.AppendInt32(Shields);
                Session.SendMessage(SerializeUserPowerUps);

            }

        }
    }
}