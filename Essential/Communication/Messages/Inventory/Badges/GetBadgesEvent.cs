using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgesEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Session.SendMessage(Session.GetHabbo().GetBadgeComponent().ComposeBadgeListMessage());
        }
	}
}
