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
    class EditGroupColorsMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (!guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                return;
            if (guild != null)
            {
                Room room = Essential.GetGame().GetRoomManager().method_15(Convert.ToUInt32(guild.RoomId));
                if (room != null)
                {
                    guild.ColorOne = Groups.GetHtmlColor(Event.PopWiredInt32());
                    guild.ColorTwo = Groups.GetHtmlColor(Event.PopWiredInt32());
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("UPDATE groups SET color_one='" + guild.ColorOne + "', color_two='" + guild.ColorTwo + "' WHERE id=" + guild.Id);
                        dbClient.ExecuteQuery(string.Concat(new object[] { "UPDATE items SET guild_data = '", guildId, ";", guild.ColorOne, ";", guild.ColorTwo, "' WHERE guild_data LIKE '", guildId, ";%'" }));
                    }
                    room.SaveSettingsPackets(guild, Session);
                    //Session.SendNotif(guild.CustomColor1.ToString() + " " + guild.HtmlColor1.ToString() + guild.CustomColor2.ToString() + " " + guild.HtmlColor2.ToString());
                }
            }
        }
    }
}
