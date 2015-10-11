using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
using System.Data;
using Essential.Storage;

namespace Essential.Communication.Messages.Games
{
    internal sealed class GetUserPowerups : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
               DataRow PuRow = dbClient.ReadDataRow("SELECT * FROM basejump_users_powerups WHERE user_id = '" + Session.Basejump_UserId + "' LIMIT 1;");

                ServerMessage SerializeUserPowerUps = new ServerMessage(Outgoing.PowerUps);
                SerializeUserPowerUps.AppendInt32(3);
                SerializeUserPowerUps.AppendInt32(0);
                SerializeUserPowerUps.AppendInt32(int.Parse(PuRow["bigparachutes"].ToString()));
                SerializeUserPowerUps.AppendInt32(1);
                SerializeUserPowerUps.AppendInt32(int.Parse(PuRow["missiles"].ToString()));
                SerializeUserPowerUps.AppendInt32(2);
                SerializeUserPowerUps.AppendInt32(int.Parse(PuRow["shields"].ToString()));
                Session.SendMessage(SerializeUserPowerUps);

                Session.Basejump_Bigparachutes = int.Parse(PuRow["bigparachutes"].ToString());
                Session.Basejump_Missiles = int.Parse(PuRow["missiles"].ToString());
                Session.Basejump_Shields = int.Parse(PuRow["shields"].ToString());
               
            }

        }
    }
}
