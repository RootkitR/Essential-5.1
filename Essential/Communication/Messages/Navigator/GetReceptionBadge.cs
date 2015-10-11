using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
using System.Data;
namespace Essential.Communication.Messages.Navigator
{
    internal sealed class GetReceptionBadge : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string ReqBadge = Event.PopFixedString();
            DataRow Checking = null;

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                Checking = dbClient.ReadDataRow("SELECT * FROM reception_slot_configurations WHERE button_type = 'BADGE' AND extra_param = '" + ReqBadge + "'");
            }

            if (Checking == null)
            {
                ServerMessage ServerError = new ServerMessage(Outgoing.ServerError);
                ServerError.AppendInt32(1);
                ServerError.AppendInt32(1);
                ServerError.AppendString(DateTime.Now.ToShortDateString().ToString());
                Session.SendMessage(ServerError);
                return;
            }

            if (Session.GetHabbo().GetBadgeComponent().HasBadge(ReqBadge))
            {
                ServerMessage BadgeError = new ServerMessage(Outgoing.BadgeError);
                BadgeError.AppendInt32(1);
                Session.SendMessage(BadgeError);
            }
            else
            {
                ServerMessage BadgeError = new ServerMessage(Outgoing.BadgeError);
                BadgeError.AppendInt32(6);
                Session.SendMessage(BadgeError);

                Session.GetHabbo().GetBadgeComponent().AddBadge(ReqBadge, 0, true);


            }
        }
    }
}
