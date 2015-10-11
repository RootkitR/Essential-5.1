using Essential.Communication.Messages.Catalog;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class EditGuildMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {

            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (!guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                return;
            ServerMessage message;
            if (!Session.GetHabbo().ColorsSended)
            {
                message = new ServerMessage(Outgoing.SendGuildParts);
                message.AppendInt32(10);
                message.AppendInt32((int)(Session.GetHabbo().OwnedRooms.Count - this.GetMyRoomsGuilds(Session)));
                foreach (RoomData data in Session.GetHabbo().OwnedRooms)
                {
                    if (data.GuildId == 0)
                    {
                        message.AppendInt32(data.Id);
                        message.AppendString(data.Name);
                        message.AppendBoolean(false);
                    }
                }
                message.AppendInt32(5);
                message.AppendInt32(10);
                message.AppendInt32(3);
                message.AppendInt32(4);
                message.AppendInt32(0x19);
                message.AppendInt32(0x11);
                message.AppendInt32(5);
                message.AppendInt32(0x19);
                message.AppendInt32(0x11);
                message.AppendInt32(3);
                message.AppendInt32(0x1d);
                message.AppendInt32(11);
                message.AppendInt32(4);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(0);
                Session.SendMessage(message);
                Session.SendMessage(Essential.GetGame().GetCatalog().groupsDataMessage);
                Session.GetHabbo().ColorsSended = true;
            }
            message = new ServerMessage(Outgoing.SendGestionGroup);
            message.AppendInt32((int)(Session.GetHabbo().OwnedRooms.Count - this.GetMyRoomsGuilds(Session)));
            foreach (RoomData data in Session.GetHabbo().OwnedRooms)
            {
                if (data.GuildId == 0)
                {
                    message.AppendInt32(data.Id);
                    message.AppendString(data.Name);
                    message.AppendBoolean(false);
                }
            }
            message.AppendBoolean(true);
            message.AppendInt32(guild.Id);
            message.AppendString(guild.Name);
            message.AppendString(guild.Description);
            message.AppendInt32(guild.RoomId);
            message.AppendInt32(Groups.GetColorByHTMLColor(guild.ColorOne));
            message.AppendInt32(Groups.GetColorByHTMLColor(guild.ColorTwo));//int.Parse(guild.ColorTwo));
            message.AppendInt32(guild.Type);
            message.AppendInt32(guild.OnlyAdminsCanMove);
            message.AppendBoolean(false);
            message.AppendString("");
            message.AppendInt32(5);
            message.AppendInt32(guild.GuildBase);
            message.AppendInt32(guild.GuildBaseColor);
            message.AppendInt32(4);
            message.AppendInt32(guild.GuildStates[0]);
            message.AppendInt32(guild.GuildStates[1]);
            message.AppendInt32(guild.GuildStates[2]);
            message.AppendInt32(guild.GuildStates[3]);
            message.AppendInt32(guild.GuildStates[4]);
            message.AppendInt32(guild.GuildStates[5]);
            message.AppendInt32(guild.GuildStates[6]);
            message.AppendInt32(guild.GuildStates[7]);
            message.AppendInt32(guild.GuildStates[8]);
            message.AppendInt32(guild.GuildStates[9]);
            message.AppendInt32(guild.GuildStates[10]);
            message.AppendInt32(guild.GuildStates[11]);
            message.AppendString(guild.Badge);
            message.AppendInt32(guild.Members.Count);
            message.AppendInt32(guild.Petitions.Count);
            Session.SendMessage(message);
        }
        public int GetMyRoomsGuilds(GameClient Session)
        {
            int num = 0;
            foreach (RoomData data in Session.GetHabbo().OwnedRooms)
            {
                if (data.GuildId != 0)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
