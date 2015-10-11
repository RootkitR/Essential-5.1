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
    class EditPrivilegesMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            //Console.WriteLine(Event.PopWiredInt32() + " | " + Event.PopWiredInt32() + " | " + Event.PopWiredInt32());
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null)
            {
                if (!guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                    return;
                Room room = Essential.GetGame().GetRoomManager().method_15(Convert.ToUInt32(guild.RoomId));
                if (room != null)
                {
                    int type = Event.PopWiredInt32();
                    guild.Locked = Groups.IntToType(type);
                    int onlyAdminMove = Event.PopWiredInt32();
                    guild.canMove = !Essential.StringToBoolean(onlyAdminMove + "");
                    guild.OnlyAdminsCanMove = onlyAdminMove;
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("UPDATE groups SET locked='" + guild.Locked + "' WHERE id=" + guild.Id);
                        dbClient.ExecuteQuery("UPDATE groups SET members_canmove='" +(guild.canMove ? 1 : 0) + "' WHERE id=" + guild.Id);
                    }
                    room.SaveSettingsPackets(guild, Session);
                }
            }
        }
    }
}
