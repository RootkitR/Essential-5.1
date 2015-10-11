using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Navigator
{
    internal sealed class GetReceptionBadgeStatus : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string ReqBadge = Event.PopFixedString();

            ServerMessage BadgeStatus = new ServerMessage(Outgoing.PrepareCampaing);
            BadgeStatus.AppendString(ReqBadge);
            BadgeStatus.AppendBoolean(Session.GetHabbo().GetBadgeComponent().HasBadge(ReqBadge));
            Session.SendMessage(BadgeStatus);
        }
    }
}
