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
    class EditGroupNameMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            string str = Event.PopFixedString();
            string str2 = Event.PopFixedString();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (!guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                return;
            if (guild != null)
            {
                Room room = Essential.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                if (room != null)
                {
                    guild.Name = str;
                    guild.Description = str2;
                    using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.AddParamWithValue("gd1", guild.Name);
                        dbClient.AddParamWithValue("gd2", guild.Description);
                        dbClient.ExecuteQuery("UPDATE groups SET name=@gd1, groups.desc=@gd2 WHERE id=" + guildId);
                    }
                    room.SaveSettingsPackets(guild, Session);
                }
            }
        }
    }
}
