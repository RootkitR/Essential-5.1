using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Catalogs;
using System.Data;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class GetCatalogPageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			CatalogPage @class = Essential.GetGame().GetCatalog().GetPage(Event.PopWiredInt32());
			if (@class != null && @class.Enabled && @class.Visible && @class.MinRank <= Session.GetHabbo().Rank)
			{
                if (@class.ClubOnly && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
				{
                    Session.SendNotification("Diese Seite ist nur für Clubmitglieder zugänglich!");
				}
				else
				{
                    Session.SendMessage(@class.GetMessage);
				}
            }
		}
	}
}
