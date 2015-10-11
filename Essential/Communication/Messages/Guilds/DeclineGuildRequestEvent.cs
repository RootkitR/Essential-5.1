using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Users;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class DeclineGuildRequestEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null && guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
            {
                int num2 = Event.PopWiredInt32();
                RoomData data = Essential.GetGame().GetRoomManager().method_11((uint)guild.RoomId);
                if(guild.Petitions.Contains(num2))
                    guild.Petitions.Remove(num2);
                using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("DELETE FROM group_requests WHERE userid=" + num2 + " AND groupid=" + guildId);
                    dbClient.ExecuteQuery("UPDATE user_stats WHERE id=" + num2 + " AND groupid=" + guildId);
                }
                GameClient gc = Essential.GetGame().GetClientManager().GetClientById((uint)num2);
                Habbo habbo = null;
                if (gc != null) habbo = gc.GetHabbo();
                if (gc != null && habbo != null)
                {
                    Room room = Essential.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                    if (room != null)
                    {
                        ServerMessage message;
                        message = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message.AppendInt32(guild.Id);
                        message.AppendBoolean(true);
                        message.AppendInt32(guild.Type);
                        message.AppendString(guild.Name);
                        message.AppendString(guild.Description);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(data.Id);
                        message.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)habbo.Id))
                        {
                            message.AppendInt32(2);
                        }
                        else if (!habbo.InGuild(guild.Id))
                        {
                            message.AppendInt32(0);
                        }
                        else if (habbo.InGuild(guild.Id))
                        {
                            message.AppendInt32(1);
                        }
                        message.AppendInt32(guild.Members.Count);
                        message.AppendBoolean(false);
                        message.AppendString(guild.Created);
                        message.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//habbo.Id == guild.OwnerId);

                        if (habbo.InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)habbo.Id))
                                message.AppendBoolean(true);
                            else message.AppendBoolean(false);
                        }
                        else message.AppendBoolean(false);
                        message.AppendString(guild.OwnerName);
                        message.AppendBoolean(true);
                        message.AppendBoolean(true);
                        message.AppendInt32(guild.Members.Contains((int)habbo.Id) ? guild.Petitions.Count : 0);
                        gc.SendMessage(message);
                    }
                }
            }
            Session.GetClientMessageHandler().LoadMembersPetitions(2, guild.Id, 0, "", Session);
        }
    }
}
