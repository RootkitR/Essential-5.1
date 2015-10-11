using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class EditGroupHomeRoomMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null)
            {
                if (!guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                    return;

                Room room = Essential.GetGame().GetRoomManager().method_15((uint)guild.RoomId);
                if (room != null)
                {
                    Event.PopWiredInt32();
                    guild.GuildBase = Event.PopWiredInt32();
                    guild.GuildBaseColor = Event.PopWiredInt32();
                    Event.PopWiredInt32();
                    guild.GuildStates.Clear();
                    string str = "";
                    for (int i = 0; i < 12; i++)
                    {
                        int item = Event.PopWiredInt32();
                        guild.GuildStates.Add(item);
                        str = str + item + ";";
                    }
                    str = str.Substring(0, str.Length - 1);
                    guild.Badge = Groups.GenerateGuildImage(guild.GuildBase, guild.GuildBaseColor, guild.GuildStates);
                    using (DatabaseClient dbClient =  Essential.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery(string.Concat(new object[] { "UPDATE groups SET Badge = '", guild.Badge, "', GuildBase = '", guild.GuildBase, "', GuildBaseColor = '", guild.GuildBaseColor, "', GuildStates = '", str, "' WHERE Id = '", guild.Id, "'" }));
                    }
                    room.SaveSettingsPackets(guild, Session);
                    ServerMessage message;
                    message = new ServerMessage(Outgoing.SendAdvGroupInit);
                    message.AppendInt32(guild.Id);
                    message.AppendBoolean(true);
                    message.AppendInt32(guild.Type);
                    message.AppendString(guild.Name);
                    message.AppendString(guild.Description);
                    message.AppendString(guild.Badge);
                    message.AppendInt32(room.Id);
                    message.AppendString(room.Name);
                    if (guild.Petitions.Contains((int)Session.GetHabbo().Id))
                    {
                        message.AppendInt32(2);
                    }
                    else if (!Session.GetHabbo().InGuild(guild.Id))
                    {
                        message.AppendInt32(0);
                    }
                    else if (Session.GetHabbo().InGuild(guild.Id))
                    {
                        message.AppendInt32(1);
                    }
                    message.AppendInt32(guild.Members.Count);
                    message.AppendBoolean(false);
                    message.AppendString(guild.Created);

                    message.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//Session.GetHabbo().Id == guild.OwnerId);

                    if (Session.GetHabbo().InGuild(guild.Id))
                    {
                        if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                    }
                    else message.AppendBoolean(false);
                    message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)guild.OwnerId));
                    message.AppendBoolean(false);
                    message.AppendBoolean(false);
                    message.AppendInt32(guild.Members.Contains((int)Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                    Session.SendMessage(message);

                }
            }
        }
    }
}
