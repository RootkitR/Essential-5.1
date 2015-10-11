using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class PurchaseFromCatalogAsGiftEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            try
            {
                int pageId = Event.PopWiredInt32();
                uint itemId = Event.PopWiredUInt();
                string extraData = Event.PopFixedString();
                string giftUser = Essential.FilterString(Event.PopFixedString());
                string giftMessage = Essential.FilterString(Event.PopFixedString());
                int giftSpriteId = Event.PopWiredInt32();
                int giftLazo = Event.PopWiredInt32();
                int giftColor = Event.PopWiredInt32();
                bool undef = Event.PopWiredBoolean();
                Essential.GetGame().GetCatalog().HandlePurchase(Session, pageId, itemId, extraData, true, giftUser, giftMessage, true, 0, undef);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            }
	}
}
