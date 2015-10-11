using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class RemoveFavoriteGuildEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Session.GetHabbo().FavouriteGroup = 0;
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery(string.Concat(new object[] { "UPDATE user_stats SET groupid = '0' WHERE Id = '", Session.GetHabbo().Id, "'" }));
            }
            ServerMessage message = new ServerMessage(Outgoing.UpdatePetitionsGuild);
            message.AppendInt32(0);
            message.AppendInt32(-1);
            message.AppendInt32(-1);
            message.AppendString("");
            if (Session.GetHabbo().CurrentRoomId > 0)
            {
                Session.GetHabbo().CurrentRoom.SendMessage(message,null);
            }
            else
            {
                Session.SendMessage(message);
            }
            ServerMessage message2 = new ServerMessage(Outgoing.RemoveGuildFavorite);
            message2.AppendInt32((int)Session.GetHabbo().Id);
            Session.SendMessage(message2);
        }
    }
}
