using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Marketplace
{
	internal sealed class GetOffersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = Event.PopWiredInt32();
			int int_2 = Event.PopWiredInt32();
			string string_ = Event.PopFixedString();
			int int_3 = Event.PopWiredInt32();
			Session.SendMessage(Essential.GetGame().GetCatalog().GetMarketplace().GetMarketPlaceOffers(int_, int_2, string_, int_3,Session));
		}
	}
}
