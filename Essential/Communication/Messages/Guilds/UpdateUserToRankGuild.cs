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
    class UpdateUserToRankGuild : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            if (guild != null && guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
            {
                int num2 = Event.PopWiredInt32();
                GameClient gc = Essential.GetGame().GetClientManager().GetClientByHabbo(Essential.GetGame().GetClientManager().GetNameById((uint)num2));
                Habbo habbo = gc.GetHabbo();
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    if (guild.UserWithRanks.Contains(num2))
                    {
                        dbClient.ExecuteQuery("UPDATE group_memberships SET hasRights='0' WHERE userid=" + num2 + " AND groupid=" + guild.Id);
                        guild.UserWithRanks.Remove(num2);
                    }
                    else
                    {
                        dbClient.ExecuteQuery("UPDATE group_memberships SET hasRights='1' WHERE userid=" + num2 + " AND groupid=" + guild.Id);
                        guild.UserWithRanks.Add(num2);
                    }
                }
                
                if (gc != null  && habbo != null)
                {
                    RoomData data = Essential.GetGame().GetRoomManager().method_11((uint)guild.RoomId);
                    try
                    {
                        ServerMessage message = new ServerMessage(Outgoing.SendHtmlColors);
                        message.AppendInt32(habbo.dataTable_0.Rows.Count);
                        foreach (DataRow num3 in habbo.dataTable_0.Rows)
                        {
                            GroupsManager guild2 = Groups.GetGroupById((int)num3["groupid"]);
                            message.AppendInt32(guild2.Id);
                            message.AppendString(guild2.Name);
                            message.AppendString(guild2.Badge);
                            message.AppendString(guild2.ColorOne);
                            message.AppendString(guild2.ColorTwo);
                            message.AppendBoolean(guild2.Id == habbo.FavouriteGroup);
                        }

                        gc.SendMessage(message);
                        ServerMessage message1;

                        message1 = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message1.AppendInt32(guild.Id);
                        message1.AppendBoolean(true);
                        message1.AppendInt32(guild.Type);
                        message1.AppendString(guild.Name);
                        message1.AppendString(guild.Description);
                        message1.AppendString(guild.Badge);
                        message1.AppendInt32(data.Id);
                        message1.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)habbo.Id))
                        {
                            message1.AppendInt32(2);
                        }
                        else if (!habbo.InGuild(guild.Id))
                        {
                            message1.AppendInt32(0);
                        }
                        else if (habbo.InGuild(guild.Id))
                        {
                            message1.AppendInt32(1);
                        }
                        message1.AppendInt32(guild.Members.Count);
                        message1.AppendBoolean(false);
                        message1.AppendString(guild.Created);
                        message1.AppendBoolean(guild.UserWithRanks.Contains((int)Session.GetHabbo().Id));//habbo.Id == guild.OwnerId);

                        if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)habbo.Id))
                                message1.AppendBoolean(true);
                            else message1.AppendBoolean(false);
                        }
                        else message1.AppendBoolean(false);
                        message1.AppendString(guild.OwnerName);
                        message1.AppendBoolean(true);
                        message1.AppendBoolean(true);
                        message1.AppendInt32(guild.Members.Contains((int)habbo.Id) ? guild.Petitions.Count : 0);
                        gc.SendMessage(message);
                    }
                    catch
                    {

                    }
                    Session.GetClientMessageHandler().LoadMembersPetitions(0, guildId, 0, "", Session);
                }
            }
        }
    }
}
