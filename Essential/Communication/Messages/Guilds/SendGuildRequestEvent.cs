using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
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
    class SendGuildRequestEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null)
            {
                RoomData data = Essential.GetGame().GetRoomManager().method_12((uint)guild.RoomId);
                if (data != null)
                {
                    DatabaseClient adapter;
                    ServerMessage message4;
                    if (guild.Type == 0)
                    {
                        Console.WriteLine("Wants to join a Open guild.");
                        if (!Session.GetHabbo().InGuild(guild.Id))
                        {
                            guild.JoinGroup((int)Session.GetHabbo().Id);
                            if (Session.GetHabbo().FavouriteGroup == 0)
                            {
                                Session.GetHabbo().FavouriteGroup = guild.Id;
                                if (Session.GetHabbo().CurrentRoomId > 0)
                                {
                                    Room room = Essential.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                                    if (data == null)
                                    {
                                        return;
                                    }
                                    if (room != null)
                                    {
                                        List<RoomUser> list = new List<RoomUser>(room.RoomUsers.ToList());
                                        ServerMessage message = new ServerMessage(Outgoing.SendGroup);
                                        message.AppendInt32(1);
                                        message.AppendInt32(guild.Id);
                                        message.AppendString(guild.Badge);
                                        Session.SendMessage(message);
                                        ServerMessage message2 = new ServerMessage(Outgoing.SetRoomUser);
                                        message2.AppendInt32(1);
                                        foreach (RoomUser user in list)
                                        {
                                            if (user != null && user.UId == Session.GetHabbo().Id)
                                            {
                                                user.method_14(message2);
                                            }
                                        }
                                        room.SendMessage(message2, null);
                                    }
                                }
                            }
                            using (adapter = Essential.GetDatabase().GetClient())
                            {
                                adapter.ExecuteQuery(string.Concat(new object[]
							    {
								    "INSERT INTO group_memberships (groupid, userid) VALUES (",
								    guild.Id,
								    ", ",
								    Session.GetHabbo().Id,
								    ");"
							    }));
                            }
                            Session.GetHabbo().RefreshGuilds();
                            //guild = ButterflyEnvironment.GetGame().GetGuilds().ReloadGuild(guildId);
                            ServerMessage message3 = new ServerMessage(Outgoing.SendHtmlColors);
                            message3.AppendInt32(Session.GetHabbo().dataTable_0.Rows.Count);
                            foreach (DataRow dataRow in Session.GetHabbo().dataTable_0.Rows)
                            {
                                GroupsManager guild2 = Groups.GetGroupById((int)dataRow["groupId"]);
                                message3.AppendInt32(guild2.Id);
                                message3.AppendString(guild2.Name);
                                message3.AppendString(guild2.Badge);
                                message3.AppendString(guild2.ColorOne);
                                message3.AppendString(guild2.ColorTwo);
                                message3.AppendBoolean(guild2.Id == Session.GetHabbo().FavouriteGroup);
                            }
                            Session.SendMessage(message3);
                            message4 = new ServerMessage(Outgoing.SendAdvGroupInit);
                            message4.AppendInt32(guild.Id);
                            message4.AppendBoolean(true);
                            message4.AppendInt32(guild.Type);
                            message4.AppendString(guild.Name);
                            message4.AppendString(guild.Description);
                            message4.AppendString(guild.Badge);
                            message4.AppendInt32(data.Id);
                            message4.AppendString(data.Name);
                            if (guild.Petitions.Contains((int)Session.GetHabbo().Id))
                            {
                                message4.AppendInt32(2);
                            }
                            else if (!Session.GetHabbo().InGuild(guild.Id))
                            {
                                message4.AppendInt32(0);
                            }
                            else if (Session.GetHabbo().InGuild(guild.Id))
                            {
                                message4.AppendInt32(1);
                            }
                            message4.AppendInt32(guild.Members.Count);
                            message4.AppendBoolean(false);
                            message4.AppendString(guild.Created);
                            message4.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//Session.GetHabbo().Id == guild.OwnerId);
                            message4.AppendBoolean(Session.GetHabbo().FavouriteGroup == guild.Id);
                            message4.AppendString(guild.OwnerName);
                            message4.AppendBoolean(true);
                            message4.AppendBoolean(true);
                            message4.AppendInt32(guild.Petitions.Count);
                            Session.SendMessage(message4);
                        }
                    }
                    else if ((guild.Type == 1) && !guild.Petitions.Contains((int)Session.GetHabbo().Id))
                    {
                        guild.Petitions.Add((int)Session.GetHabbo().Id);
                        string str = "";
                        foreach (int num3 in guild.Petitions)
                        {
                            str = str + num3 + ";";
                        }
                        if (str.Length > (Session.GetHabbo().Id.ToString().Length + 1))
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        using (adapter = Essential.GetDatabase().GetClient())
                        {
                            adapter.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO group_requests (groupid, userid) VALUES (",
								guild.Id,
								", ",
								Session.GetHabbo().Id,
								");"
							}));
                        }
                        message4 = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message4.AppendInt32(guild.Id);
                        message4.AppendBoolean(true);
                        message4.AppendInt32(guild.Type);
                        message4.AppendString(guild.Name);
                        message4.AppendString(guild.Description);
                        message4.AppendString(guild.Badge);
                        message4.AppendInt32((int)data.Id);
                        message4.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)Session.GetHabbo().Id))
                        {
                            message4.AppendInt32(2);
                        }
                        else if (!Session.GetHabbo().InGuild(guild.Id))
                        {
                            message4.AppendInt32(0);
                        }
                        else if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            message4.AppendInt32(1);
                        }
                        message4.AppendInt32(guild.Members.Count);
                        message4.AppendBoolean(false);
                        message4.AppendString(guild.Created);
                        message4.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//Session.GetHabbo().Id == guild.OwnerId);
                        message4.AppendBoolean(Session.GetHabbo().FavouriteGroup == guild.Id);
                        message4.AppendString(guild.OwnerName);
                        message4.AppendBoolean(true);
                        message4.AppendBoolean(true);
                        message4.AppendInt32(guild.Members.Contains((int)Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                        Session.SendMessage(message4);
                    }
                }
            }
        }
    }
}
