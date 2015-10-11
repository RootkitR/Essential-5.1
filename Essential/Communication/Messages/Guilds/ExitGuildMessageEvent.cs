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
    class ExitGuildMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            int UserId = Event.PopWiredInt32();
            if (UserId != Session.GetHabbo().Id && !guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                return;
            if (guild != null)
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("DELETE FROM `group_memberships` WHERE (`groupid`='" + guildId + "') AND (`userid`='" + UserId + "') LIMIT 1");
                    dbClient.ExecuteQuery("UPDATE user_stats SET groupid=0 WHERE (groupid=" + guildId + ") AND (id=" + UserId + ") LIMIT 1");
                }
                guild.Leave(UserId);
                if (guild.UserWithRanks.Contains((int)UserId))
                    guild.UserWithRanks.Remove((int)UserId);
                if ((uint)UserId != Session.GetHabbo().Id)
                    LoadMembersPetitions(0, guildId, 0, "", Session);
                GameClient habbo = Essential.GetGame().GetClientManager().GetClientById((uint)UserId);
                if (habbo != null)
                {
                    if (habbo.GetHabbo().FavouriteGroup == guild.Id)
                    {
                        habbo.GetHabbo().FavouriteGroup = 0;
                        if (habbo.GetHabbo().CurrentRoom != null)
                        {
                            List<RoomUser> list = new List<RoomUser>(habbo.GetHabbo().CurrentRoom.RoomUsers);
                            ServerMessage message = new ServerMessage(Outgoing.SendGroup);
                            message.AppendInt32(0);
                            Session.SendMessage(message);
                            ServerMessage message2 = new ServerMessage(Outgoing.SetRoomUser);
                            message2.AppendInt32(1);
                            foreach (RoomUser user in list)
                            {
                                if (user.UId == habbo.GetHabbo().Id)
                                {
                                    user.method_14(message2);
                                    user.RemoveStatus("flatctrl 1");
                                    user.AddStatus("flatctrl 0", "");
                                    user.UpdateNeeded = true;
                                }
                            }
                            habbo.GetHabbo().CurrentRoom.SendMessage(message2, null);
                        }

                        habbo.GetHabbo().RefreshGuilds();
                    }
                    if ((int)Session.GetHabbo().Id == UserId)
                    {
                        RoomData data = Essential.GetGame().GetRoomManager().method_11((uint)guild.RoomId);
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
                        if (guild.Petitions.Contains((int)Session.GetHabbo().Id))
                            message.AppendInt32(2);
                        else if (!Session.GetHabbo().InGuild(guild.Id))
                            message.AppendInt32(0);
                        else if (Session.GetHabbo().InGuild(guild.Id))
                            message.AppendInt32(1);
                        message.AppendInt32(guild.Members.Count);
                        message.AppendBoolean(false);
                        message.AppendString(guild.Created);
                        message.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//(int)Session.GetHabbo().Id == guild.OwnerId);

                        if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            if (guild.getRank((int)Session.GetHabbo().Id) < 2)
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
                }
            }
        }
        public void LoadMembersPetitions(int a, int gId, int b, string search, GameClient Session)
        {
            try
            {
                int guildId = gId;
                int num2 = a;
                int num3 = b;
                string str = search;
                GroupsManager guild = Groups.GetGroupById(guildId);
                if (guild != null)
                {
                    ServerMessage message;
                    int memberCounter = 0;
                    int results;
                    if (num3 == 0)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            results = 0;
                            foreach (int num4 in guild.Members)
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                    results++;
                            }
                        }
                        else
                            results = guild.Members.Count();
                        message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                        message.AppendInt32(guild.Id);
                        message.AppendString(guild.Name);
                        message.AppendInt32(guild.RoomId);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(results);

                        if (num2 == 0)
                            message.AppendInt32(results);
                        else
                            message.AppendInt32(results - (num2 * 14));

                        foreach (int num4 in guild.Members)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                {
                                    if (memberCounter >= ((num2) * 14))
                                    {
                                        if (guild.OwnerId == num4)
                                        {
                                            message.AppendInt32(0);
                                        }
                                        else
                                        {
                                            message.AppendInt32(guild.getRank(num4));
                                        }
                                        message.AppendInt32(num4);
                                        message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                        message.AppendString("");
                                    }
                                    memberCounter++;
                                }
                            }

                            else
                            {
                                if (memberCounter >= (num2 * 14))
                                {
                                    if (guild.OwnerId == (uint)num4)
                                    {
                                        message.AppendInt32(0);
                                    }
                                    else
                                    {
                                        message.AppendInt32(guild.getRank(num4));
                                    }
                                    message.AppendInt32(num4);
                                    message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                        message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                    message.AppendString("");
                                }
                                memberCounter++;
                            }
                        }

                        if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                                message.AppendBoolean(true);
                            else message.AppendBoolean(false);
                        }
                        else message.AppendBoolean(false);
                        message.AppendInt32(14);
                        message.AppendInt32(num2);
                        message.AppendInt32(0);
                        message.AppendString(str);
                        Session.SendMessage(message);
                    }
                    else if (num3 == 1)
                    {
                        int admins = 0;

                        foreach (int num4 in guild.Members)
                        {
                            if (guild.UserWithRanks.Contains(num4)) admins++;
                        }

                        if (!string.IsNullOrEmpty(str))
                        {
                            results = 0;
                            foreach (int num4 in guild.Members)
                            {
                                if ((Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower())) && ((guild.UserWithRanks.Contains(num4))))
                                    results++;
                            }
                        }
                        else
                            results = admins;

                        message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                        message.AppendInt32(guild.Id);
                        message.AppendString(guild.Name);
                        message.AppendInt32(guild.RoomId);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(results);

                        if (num2 == 0)
                            message.AppendInt32(results);

                        else message.AppendInt32(results - (num2 * 14));
                        memberCounter = 0;
                        foreach (int num4 in guild.Members)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                {
                                    if (guild.UserWithRanks.Contains(num4))
                                    {
                                        if (memberCounter >= ((num2) * 14))
                                        {
                                            if (guild.OwnerId == num4)
                                            {
                                                message.AppendInt32(0);
                                            }
                                            else
                                            {
                                                message.AppendInt32(guild.getRank(num4));
                                            }
                                            message.AppendInt32(num4);
                                            message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                            message.AppendString("");
                                        }
                                    }
                                    memberCounter++;
                                }
                            }

                            else
                            {
                                if (memberCounter >= (num2 * 14))
                                {
                                    if (guild.UserWithRanks.Contains(num4))
                                    {
                                        if (guild.OwnerId == num4)
                                        {
                                            message.AppendInt32(0);
                                        }
                                        else
                                        {
                                            message.AppendInt32(guild.getRank(num4));
                                        }
                                        message.AppendInt32(num4);
                                        message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                        message.AppendString("");
                                    }
                                }
                                memberCounter++;
                            }
                        }
                        if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                                message.AppendBoolean(true);
                            else message.AppendBoolean(false);
                        }
                        else message.AppendBoolean(false);
                        message.AppendInt32(14);
                        message.AppendInt32(num2);
                        message.AppendInt32(1);
                        message.AppendString(str);
                        Session.SendMessage(message);
                    }

                    else if (num3 == 2)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            results = 0;
                            foreach (int num4 in guild.Petitions)
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                    results++;
                            }
                        }

                        else
                            results = guild.Petitions.Count();

                        message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                        message.AppendInt32(guild.Id);
                        message.AppendString(guild.Name);
                        message.AppendInt32(guild.RoomId);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(results);
                        if (num2 == 0)
                            message.AppendInt32(results);

                        else message.AppendInt32(results - (num2 * 14));
                        memberCounter = 0;
                        foreach (int num4 in guild.Petitions)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                {
                                    if (memberCounter >= ((num2) * 14))
                                    {
                                        message.AppendInt32(3);
                                        message.AppendInt32(num4);
                                        message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                        message.AppendString("");
                                    }
                                    memberCounter++;
                                }
                            }

                            else
                            {
                                if (memberCounter >= (num2 * 14))
                                {
                                    message.AppendInt32(3);
                                    message.AppendInt32(num4);
                                    message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                        message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                    message.AppendString("");
                                }
                                memberCounter++;
                            }
                        }
                        if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                        message.AppendInt32(14);
                        message.AppendInt32(num2);
                        message.AppendInt32(2);
                        message.AppendString(str);
                        Session.SendMessage(message);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
