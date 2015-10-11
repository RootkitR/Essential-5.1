using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
using System.Data;
namespace Essential.Communication.Messages.Users
{
    internal sealed class GetGuildMembers : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int GuildId = Event.PopWiredInt32();

            GroupsManager Guild = Groups.GetGroupById(GuildId);

            if (Guild == null)
            {
                return;
            }

            ServerMessage Message = new ServerMessage(Outgoing.Guild); // Updated
            Message.AppendInt32(Guild.Id);
            Message.AppendString(Guild.Name);
            Message.AppendUInt(Guild.RoomId);
            Message.AppendString(Guild.Badge);
            Message.AppendInt32(Guild.Members.Count);
            Message.AppendInt32((Guild.Members.Count > 13) ? 14 : Guild.Members.Count);
            foreach (int UserId in Guild.Members)
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("uid", UserId);
                DataRow UserData = dbClient.ReadDataRow("SELECT username,look FROM users WHERE Id = @uid LIMIT 1");
                if (Guild.OwnerId != UserId)
                    Message.AppendInt32(0);
                else
                    Message.AppendInt32(2);
                Message.AppendInt32(UserId);
                Message.AppendString((string)UserData["username"]);
                Message.AppendString((string)UserData["look"]);
                Message.AppendString("");

                }
            }
            Message.AppendBoolean(false);
            Message.AppendInt32(14);
            Message.AppendInt32(0);
            Message.AppendInt32(0);
            Message.AppendString("");
            Session.SendMessage(Message);
        }
          
    }
}
