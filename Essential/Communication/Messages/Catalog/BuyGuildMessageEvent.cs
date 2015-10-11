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

namespace Essential.Communication.Messages.Catalog
{
    class BuyGuildMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            try
            {
                if ((Session != null) && (Session.GetHabbo().GetCredits() >= 10))
                {
                    List<int> gStates = new List<int>();
                    string name = Event.PopFixedString();
                    string description = Event.PopFixedString();
                    int roomid = Event.PopWiredInt32();
                    int color = Event.PopWiredInt32();
                    int num3 = Event.PopWiredInt32();
                    Event.PopWiredInt32();
                    int guildBase = Event.PopWiredInt32();
                    int guildBaseColor = Event.PopWiredInt32();
                    int num6 = Event.PopWiredInt32();
                    if (Essential.GetGame().GetRoomManager().method_15((uint)roomid).RoomData.GuildId != 0)
                        return;
                    for (int i = 0; i < (num6 * 3); i++)
                    {
                        int item = Event.PopWiredInt32();
                        gStates.Add(item);
                    }

                    string image = Groups.GenerateGuildImage(guildBase, guildBaseColor, gStates);
                    string htmlColor = Groups.GetHtmlColor(color);
                    string str5 = Groups.GetHtmlColor(num3);
                    string datecreated = DateTime.Now.ToShortDateString();

                    int id = (int)Session.GetHabbo().Id;

                    string username = Session.GetHabbo().Username;

                    Dictionary<int, string> members = new Dictionary<int, string>();
                    members.Add(id, DateTime.Now.Day + " - " + DateTime.Now.Month + " -  " + DateTime.Now.Year);



                    Room room = Essential.GetGame().GetRoomManager().GetRoom(Convert.ToUInt32(roomid));

                    if (room != null && room.CheckRights(Session,true))
                    {
                        GroupsManager guild = Groups.AddGuild(0, name, id, username, description, roomid, image, color, num3, guildBase, guildBaseColor, gStates, htmlColor, str5, Essential.GetUnixTimestamp().ToString(), members, new List<int>(), 0, 0);

                        using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                        {
                            Session.GetHabbo().dataTable_0 = adapter.ReadDataTable("SELECT * FROM group_memberships WHERE userid = " + Session.GetHabbo().Id);
                            Session.GetHabbo().FavouriteGroup = guild.Id;

                            adapter.ExecuteQuery(string.Concat(new object[] { "UPDATE user_stats SET groupid = '", guild.Id, "' WHERE Id = '", guild.OwnerId, "'" }));
                        }
                        ServerMessage message = new ServerMessage(Outgoing.SerializePurchaseInformation); //Rootkit
                        message.AppendInt32(0x1815);
                        message.AppendString("CREATE_GUILD");
                        message.AppendInt32(10);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendBoolean(true);
                        message.AppendInt32(0);
                        message.AppendInt32(2);
                        message.AppendBoolean(false);

                        Session.SendMessage(message);
                        Session.GetHabbo().TakeCredits(10, "Bought Guild");
                        Session.GetHabbo().UpdateCredits(true);

                        ServerMessage message2 = new ServerMessage(Outgoing.SendHtmlColors);
                        message2.AppendInt32(Session.GetHabbo().dataTable_0.Rows.Count);
                        foreach (DataRow drow in Session.GetHabbo().dataTable_0.Rows)
                        {
                            GroupsManager guild2 = Groups.GetGroupById((int)drow["groupid"]);
                            message2.AppendInt32(guild2.Id);
                            message2.AppendString(guild2.Name);
                            message2.AppendString(guild2.Badge);
                            message2.AppendString(guild2.ColorOne);
                            message2.AppendString(guild2.ColorTwo);
                            message2.AppendBoolean(guild2.Id == Session.GetHabbo().FavouriteGroup);
                        }

                        Session.SendMessage(message2);

                        if ((Session != null) && (room != null) && Session.GetHabbo().CurrentRoomId == roomid)
                        {
                            ServerMessage message3 = new ServerMessage(Outgoing.SetRoomUser); //Rootkit
                            message3.AppendInt32(1);
                            room.GetRoomUserByHabbo(Session.GetHabbo().Id).method_14(message3);
                            room.SendMessage(message3, null);
                        }

                        ServerMessage message4 = new ServerMessage(Outgoing.UpdateRoom); //Rootkit
                        message4.AppendInt32(guild.RoomId);
                        Session.SendMessage(message4);

                        ServerMessage message5 = new ServerMessage(Outgoing.ConfigureWallandFloor); //Rootkit
                        message5.AppendBoolean(room.Hidewall);
                        message5.AppendInt32(room.Wallthick);
                        message5.AppendInt32(room.Floorthick);
                        Session.SendMessage(message5);

                        ServerMessage message6 = new ServerMessage(Outgoing.SendRoomAndGroup); //Rootkit
                        message6.AppendInt32(guild.RoomId);
                        message6.AppendInt32(guild.Id);
                        Session.SendMessage(message6);

                        ServerMessage message7 = new ServerMessage(Outgoing.RoomData); //Rootkit
                        message7.AppendBoolean(true);
                        message7.AppendInt32(guild.RoomId);
                        message7.AppendString(room.Name);
                        message7.AppendBoolean(true);
                        message7.AppendInt32(room.OwnerId);
                        message7.AppendString(room.Owner);
                        message7.AppendInt32(room.State);
                        message7.AppendInt32(room.UsersNow);
                        message7.AppendInt32(room.UsersMax);
                        message7.AppendString(room.Description);
                        message7.AppendInt32(0);
                        message7.AppendInt32((room.Category == 0x34) ? 2 : 0);
                        message7.AppendInt32(room.Score);
                        message7.AppendInt32(0);
                        message7.AppendInt32(room.Category);

                        if (room.RoomData.GuildId == 0)
                        {
                            message7.AppendInt32(0);
                            message7.AppendInt32(0);
                        }
                        else
                        {
                            message7.AppendInt32(guild.Id);
                            message7.AppendString(guild.Name);
                            message7.AppendString(guild.Badge);
                        }

                        message7.AppendString("");
                        message7.AppendInt32(room.Tags.Count);

                        foreach (string str8 in room.Tags.ToArray())
                        {
                            message7.AppendString(str8);
                        }

                        message7.AppendInt32(0);
                        message7.AppendInt32(0);
                        message7.AppendInt32(0);
                        message7.AppendBoolean(true);
                        message7.AppendBoolean(true);
                        message7.AppendInt32(0);
                        message7.AppendInt32(0);
                        message7.AppendBoolean(false);
                        message7.AppendBoolean(false);
                        message7.AppendBoolean(false);
                        message7.AppendInt32(0);
                        message7.AppendInt32(0);
                        message7.AppendInt32(0);
                        message7.AppendBoolean(false);
                        message7.AppendBoolean(true);
                        room.SendMessage(message7, null);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
