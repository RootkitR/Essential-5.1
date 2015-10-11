using System;
using Essential.HabboHotel.GameClients;
using Essential.Catalogs;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class GetIsOfferGiftableEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            /*//We don't need Marketplace!
			uint uint_ = Event.PopWiredUInt();
			CatalogItem @class = Essential.GetGame().GetCatalog().method_2(uint_);
			if (@class != null)
			{
                ServerMessage Message = new ServerMessage(Outgoing.OfferGiftable);
				Message.AppendUInt(@class.uint_0);
				Message.AppendBoolean(@class.method_0().AllowGift);
				Session.SendMessage(Message);
			}*/
		}
	}
}
