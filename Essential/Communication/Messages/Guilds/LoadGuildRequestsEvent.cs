using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class LoadGuildRequestsEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId;
            int num2;
            int num3;
            string str;
            guildId = Event.PopWiredInt32();
            num2 = Event.PopWiredInt32();
            str = Event.PopFixedString();
            num3 = Event.PopWiredInt32();
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
                                    using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
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
        }
    }
}
