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
    class GuildInfoMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            bool flag = Event.PopWiredBoolean();
            GroupsManager guild = Groups.GetGroupById(guildId);
           /* if (!guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                return;*/
            if (guild != null)
            {
                RoomData data = Essential.GetGame().GetRoomManager().method_11((uint)guild.RoomId);
                if (data != null)
                {
                    ServerMessage message;
                    if (!flag)
                    {
                        try
                        {
                            message = new ServerMessage(Outgoing.SendAdvGroupInit);
                            message.AppendInt32(guild.Id);
                            message.AppendBoolean(true);
                            message.AppendInt32(guild.Type);
                            message.AppendString(guild.Name);
                            message.AppendString(guild.Description);
                            message.AppendString(guild.Badge);
                            message.AppendInt32(data.Id);
                            message.AppendString(data.Name);
                            if (guild.Petitions.Contains((int)Session.GetHabbo().Id))
                                message.AppendInt32(2);
                            else if (!guild.HasMember(Session.GetHabbo().Id))
                                message.AppendInt32(0);
                            else if (guild.HasMember(Session.GetHabbo().Id))
                                message.AppendInt32(1);
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
                            message.AppendString(guild.OwnerName);
                            message.AppendBoolean(false);
                            message.AppendBoolean(false);
                            message.AppendInt32(guild.Petitions.Count);
                            Session.SendMessage(message);
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            message = new ServerMessage(Outgoing.SendAdvGroupInit);
                            message.AppendInt32(guild.Id);
                            message.AppendBoolean(false);
                            message.AppendInt32(guild.Type);
                            message.AppendString(guild.Name);
                            message.AppendString(guild.Description);
                            message.AppendString(guild.Badge);
                            message.AppendInt32(data.Id);
                            message.AppendString(data.Name);
                            if (guild.Petitions.Contains((int)Session.GetHabbo().Id))
                                message.AppendInt32(2);
                            else if (!guild.HasMember(Session.GetHabbo().Id))
                                message.AppendInt32(0);
                            else if (guild.HasMember(Session.GetHabbo().Id))
                                message.AppendInt32(1);
                            message.AppendInt32(guild.Members.Count);
                            message.AppendBoolean(false);
                            message.AppendString(guild.Created);
                            message.AppendBoolean(Session.GetHabbo().Id == guild.OwnerId);
                            if (Session.GetHabbo().InGuild(guild.Id))
                            {
                                if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                                    message.AppendBoolean(true);
                                else message.AppendBoolean(false);
                            }
                            else message.AppendBoolean(false);
                            message.AppendString(guild.OwnerName);
                            message.AppendBoolean(true);
                            message.AppendBoolean(true);
                            message.AppendInt32(guild.Members.Contains((int)Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                            Session.SendMessage(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }else
                    Console.WriteLine("Roomdata for room: " + guild.RoomId + " was null");

            }else if(guildId != 1)
                Console.WriteLine("Null group");
        }
    }
}
