using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Users
{
    internal sealed class ExitGuildEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int GuildId = Event.PopWiredInt32();
            int UserId = Event.PopWiredInt32();
            string OwnerName;
            GroupsManager Guild = Groups.GetGroupById(GuildId);

            if (Guild == null || Session.GetHabbo().Id != UserId)
            {
                return;
            }

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("DELETE FROM `group_memberships` WHERE (`groupid`='" + GuildId + "') AND (`userid`='" + UserId + "') LIMIT 1");
            }

            using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
            {
                Session.GetHabbo().method_0(class2);
            }

            Guild.Leave((int)Session.GetHabbo().Id);

            ServerMessage Message = new ServerMessage(Outgoing.GroupInfo); // Updated
            Message.AppendInt32(Guild.Id);
            Message.AppendBoolean(true);
            Message.AppendInt32(Guild.Locked == "open" ? 0 : 1);
            Message.AppendStringWithBreak(Guild.Name);
            Message.AppendStringWithBreak(Guild.Description);
            Message.AppendStringWithBreak(Guild.Badge);
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                OwnerName = dbClient.ReadString("SELECT username FROM users WHERE id = '" + Guild.OwnerId + "' LIMIT 1");
            }
            if (Guild.RoomId > 0u)
            {
                Message.AppendUInt(Guild.RoomId);
                if (Essential.GetGame().GetRoomManager().GetRoom(Guild.RoomId) != null)
                {
                    Message.AppendStringWithBreak(Essential.GetGame().GetRoomManager().GetRoom(Guild.RoomId).Name);
                    goto IL_15A;
                }
                using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                {
                    try
                    {
                        DataRow dataRow_ = class2.ReadDataRow("SELECT * FROM rooms WHERE Id = " + Guild.RoomId + " LIMIT 1;");
                        string string_ = Essential.GetGame().GetRoomManager().method_17(Guild.RoomId, dataRow_).Name;
                        Message.AppendStringWithBreak(string_);
                    }
                    catch
                    {
                           if (Session.GetHabbo().CurrentRoomId > 0)
            {
                Message.AppendInt32((int)Session.GetHabbo().CurrentRoomId);
                Message.AppendStringWithBreak("");
            }
            else
            {

                        Message.AppendInt32(-1);
                        Message.AppendStringWithBreak("");
            }
                    }
                    goto IL_15A;
                }
            }
            if (Session.GetHabbo().CurrentRoomId > 0)
            {
                Message.AppendInt32((int)Session.GetHabbo().CurrentRoomId);
                Message.AppendStringWithBreak("");
            }
            else
            {
                Message.AppendInt32(-1);
                Message.AppendStringWithBreak("");
            }
       
        IL_15A:
            bool flag = false;
            foreach (DataRow dataRow in Session.GetHabbo().dataTable_0.Rows)
            {
                if ((int)dataRow["groupid"] == Guild.Id)
                {
                    flag = true;
                }
            }
            if (Session.GetHabbo().list_0.Contains(Guild.Id))
            {
                Message.AppendInt32(2);
            }
            else
            {
                if (flag)
                {
                    Message.AppendInt32(1);
                }
                else
                {
                
                        if (Guild.Members.Contains((int)Session.GetHabbo().Id))
                        {
                            Message.AppendInt32(1);
                        }
                        else
                        {
                            Message.AppendInt32(0);
                        }
                    
                }
            }
            Message.AppendInt32(Guild.Members.Count);
            if (Session.GetHabbo().FavouriteGroup == Guild.Id)
            {
                Message.AppendBoolean(true);
            }
            else
            {
                Message.AppendBoolean(false);
            }
            Message.AppendString(Guild.Created);
            Message.AppendBoolean(false);
            Message.AppendBoolean(false);
            Message.AppendString(OwnerName);
            Message.AppendBoolean(true);
            Message.AppendBoolean(false);
            Message.AppendInt32(0); // Pending
            Session.SendMessage(Message);

            ServerMessage InitGroupData = new ServerMessage(Outgoing.OwnGuilds);
            InitGroupData.AppendInt32(Session.GetHabbo().dataTable_0.Rows.Count);
            foreach (DataRow dataRow in Session.GetHabbo().dataTable_0.Rows)
            {
                GroupsManager current = Groups.GetGroupById((int)dataRow["groupId"]);
                InitGroupData.AppendInt32(current.Id);
                InitGroupData.AppendString(current.Name);
                InitGroupData.AppendString(current.Badge);
                InitGroupData.AppendString(current.ColorOne);
                InitGroupData.AppendString(current.ColorTwo);
                InitGroupData.AppendBoolean((Session.GetHabbo().FavouriteGroup == current.Id));
            }


            Session.SendMessage(InitGroupData);


            if (Session.GetHabbo().FavouriteGroup == Guild.Id)
            {
                Session.GetHabbo().FavouriteGroup = 0;
                using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                {
                    class2.ExecuteQuery("UPDATE user_stats SET groupid = 0 WHERE Id = " + Session.GetHabbo().Id + " LIMIT 1;");
                }
            }
         }
    }
}
