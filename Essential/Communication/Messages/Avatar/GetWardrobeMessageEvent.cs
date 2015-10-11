using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Avatar
{
    internal sealed class GetWardrobeMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage Message = new ServerMessage(Outgoing.WardrobeData); //Rootkit
            Message.AppendInt32(Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") ? 1 : 0);
            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                {
                    @class.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable dataTable = @class.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (dataTable == null)
                    {
                        Message.AppendInt32(0);
                    }
                    else
                    {
                        Message.AppendInt32(dataTable.Rows.Count);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            Message.AppendUInt((uint)dataRow["slot_id"]);
                            Message.AppendStringWithBreak((string)dataRow["look"]);
                            Message.AppendStringWithBreak((string)dataRow["gender"]);
                        }
                    }
                }
                Session.SendMessage(Message);
            }
            else
            {
                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                {
                    @class.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable dataTable = @class.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (dataTable == null)
                    {
                        Message.AppendInt32(0);
                    }
                    else
                    {
                        Message.AppendInt32(dataTable.Rows.Count);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            Message.AppendUInt((uint)dataRow["slot_id"]);
                            Message.AppendStringWithBreak((string)dataRow["look"]);
                            Message.AppendStringWithBreak((string)dataRow["gender"]);
                        }
                    }
                }
                Session.SendMessage(Message);
            }
        }
    }
}