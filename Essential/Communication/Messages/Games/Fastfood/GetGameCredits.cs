using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
using Essential.Storage;

namespace Essential.Communication.Messages.Games
{
    internal sealed class GetGameCredits : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int Credits = 0;

         using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
                Credits = dbClient.ReadInt32("SELECT credits FROM users WHERE id = '" + Session.Basejump_UserId + "' LIMIT 1");
            }

         ServerMessage SerializeCredits = new ServerMessage(Outgoing.GameCredits);
         SerializeCredits.AppendInt32(Credits);
         Session.SendMessage(SerializeCredits);

        }
    }
}
