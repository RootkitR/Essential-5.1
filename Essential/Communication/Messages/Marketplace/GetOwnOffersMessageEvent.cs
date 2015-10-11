using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Marketplace
{
	internal sealed class GetOwnOffersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(Essential.GetGame().GetCatalog().GetMarketplace().SerializeMyOffers(Session.GetHabbo().Id));
		}
	}
}
