using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Users;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class AcceptGuildRequestEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null && guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
            {
                int userId = Event.PopWiredInt32();
                guild.JoinGroup(userId);
                guild.Petitions.Remove(userId);
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("DELETE FROM group_requests WHERE userid=" + userId + " AND groupid=" + guildId);
                    dbClient.ExecuteQuery("UPDATE user_stats SET groupid=" + guildId + " WHERE id=" + userId);
                    dbClient.ExecuteQuery("INSERT INTO group_memberships (`groupid`,`userid`) VALUES (" + guildId +"," +userId+")");
                }
                GameClient gc = Essential.GetGame().GetClientManager().GetClientById((uint)userId);
                Habbo habbo = null;
                if (gc != null)
                    habbo = gc.GetHabbo();
                if (habbo != null)
                {
                    Room room = Essential.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                    if (habbo.FavouriteGroup == 0)
                    {
                        habbo.FavouriteGroup = guild.Id;
                        habbo.RefreshGuilds();
                        if (habbo.CurrentRoomId > 0)
                        {
                            if (room == null)
                            {
                                return;
                            }
                            ServerMessage message = new ServerMessage(Outgoing.SendGroup); //Rootkit
                            message.AppendInt32(1);
                            message.AppendInt32(guild.Id);
                            message.AppendString(guild.Badge);
                            gc.SendMessage(message);
                            ServerMessage message2 = new ServerMessage(Outgoing.SetRoomUser); //Rootkit
                            message2.AppendInt32(1);
                            RoomUser ru = gc.GetHabbo().CurrentRoom.GetRoomUserByHabbo(habbo.Id);
                            if (ru != null)
                                ru.method_14(message2);
                            gc.GetHabbo().CurrentRoom.SendMessage(message2,null);
                        }
                    }
                    ServerMessage message3 = new ServerMessage(Outgoing.AddNewMember); //Rootkit
                    message3.AppendInt32(guildId);
                    message3.AppendInt32(guild.getRank(userId));
                    message3.AppendInt32(habbo.Id);
                    message3.AppendString(habbo.Username);
                    message3.AppendString(habbo.Figure);
                    message3.AppendString(string.Concat(new object[] { DateTime.Now.Month, " ", DateTime.Now.Day, ", ", DateTime.Now.Year }));
                    Session.SendMessage(message3);

                    ServerMessage message4 = new ServerMessage(Outgoing.UpdatePetitionsGuild); //Rootkit
                    message4.AppendInt32(1);
                    message4.AppendInt32(guild.Id);
                    message4.AppendInt32(3);
                    message4.AppendString(guild.Name);
                    Session.SendMessage(message4);
                    gc.SendMessage(message4);
                    ServerMessage message5 = new ServerMessage(Outgoing.SetRoomUser); //Rootkit
                    message5.AppendInt32(1);
                    RoomUser ru2 = gc.GetHabbo().CurrentRoom.GetRoomUserByHabbo(habbo.Id);
                    if (ru2 != null)
                        ru2.method_14(message5);
                        gc.GetHabbo().CurrentRoom.SendMessage(message5,null);
                    }
                    ServerMessage message6 = new ServerMessage(Outgoing.SendHtmlColors);
                    message6.AppendInt32(Session.GetHabbo().dataTable_0.Rows.Count);
                    foreach (DataRow num4 in Session.GetHabbo().dataTable_0.Rows)
                    {
                        GroupsManager guild2 = Groups.GetGroupById((int)num4["groupid"]);
                        message6.AppendInt32(guild2.Id);
                        message6.AppendString(guild2.Name);
                        message6.AppendString(guild2.Badge);
                        message6.AppendString(guild2.ColorOne);
                        message6.AppendString(guild2.ColorTwo);
                        message6.AppendBoolean(guild2.Id == Session.GetHabbo().FavouriteGroup);
                    }
                    Session.SendMessage(message6);
                    Session.GetClientMessageHandler().LoadMembersPetitions(2, guildId, 0, "", Session);
                    RoomData data = Essential.GetGame().GetRoomManager().method_11((uint)guild.RoomId);
                    if (gc != null)
                    {
                        ServerMessage message7 = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message7.AppendInt32(guild.Id);
                        message7.AppendBoolean(true);
                        message7.AppendInt32(guild.Type);
                        message7.AppendString(guild.Name);
                        message7.AppendString(guild.Description);
                        message7.AppendString(guild.Badge);
                        message7.AppendInt32(data.Id);
                        message7.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)habbo.Id))
                        {
                            message7.AppendInt32(2);
                        }
                        else if (!habbo.InGuild(guild.Id))
                        {
                            message7.AppendInt32(0);
                        }
                        else if (habbo.InGuild(guild.Id))
                        {
                            message7.AppendInt32(1);
                        }
                        message7.AppendInt32(guild.Members.Count);
                        message7.AppendBoolean(false);
                        message7.AppendString(guild.Created);

                        message7.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//habbo.Id == guild.OwnerId);

                        if (habbo.InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)habbo.Id))
                                message7.AppendBoolean(true);
                            else message7.AppendBoolean(false);
                        }
                        else message7.AppendBoolean(false);
                        message7.AppendString(guild.OwnerName);
                        message7.AppendBoolean(true);
                        message7.AppendBoolean(true);
                        message7.AppendInt32(guild.Members.Contains((int)habbo.Id) ? guild.Petitions.Count : 0);
                        gc.SendMessage(message7);
                    }
                }
            }
        }
    }
