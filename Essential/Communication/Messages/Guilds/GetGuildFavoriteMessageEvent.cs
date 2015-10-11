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
    class GetGuildFavoriteMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null)
            {
                Session.GetHabbo().FavouriteGroup = guildId;
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[] { "UPDATE user_stats SET groupid = '", guildId, "' WHERE Id = '", Session.GetHabbo().Id, "'" }));
                }
                ServerMessage message = new ServerMessage(Outgoing.UpdatePetitionsGuild);
                message.AppendInt32(0);
                message.AppendInt32(guild.Id);
                message.AppendInt32(2);
                message.AppendString(guild.Name);
                if (Session.GetHabbo().CurrentRoomId > 0)
                {
                    Session.GetHabbo().CurrentRoom.SendMessage(message, null);
                }
                else
                {
                    Session.SendMessage(message);
                }
                ServerMessage message2 = new ServerMessage(Outgoing.SendGroup);
                message2.AppendInt32(1);
                message2.AppendInt32(guild.Id);
                message2.AppendString(guild.Badge);
                if (Session.GetHabbo().CurrentRoomId > 0)
                {
                    Session.GetHabbo().CurrentRoom.SendMessage(message2, null);
                }
                else
                {
                    Session.SendMessage(message2);
                }
                ServerMessage message3 = new ServerMessage(Outgoing.RemoveGuildFavorite);
                message3.AppendInt32((int)Session.GetHabbo().Id);
                Session.SendMessage(message3);
            }
        }
    }
}
