using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Marketplace
{
	internal sealed class RedeemOfferCreditsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				dataTable = @class.ReadDataTable("SELECT asking_price FROM catalog_marketplace_offers WHERE user_id = '" + Session.GetHabbo().Id + "' AND state = '2'");
			}
			if (dataTable != null)
			{
				int num = 0;
				foreach (DataRow dataRow in dataTable.Rows)
				{
					num += (int)dataRow["asking_price"];
				}
				if (num >= 1)
				{
					Session.GetHabbo().GiveCredits(num, "Reedem Marketplace Offers");
					Session.GetHabbo().UpdateCredits(true);
				}
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.ExecuteQuery("DELETE FROM catalog_marketplace_offers WHERE user_id = '" + Session.GetHabbo().Id + "' AND state = '2'");
				}
			}
		}
	}
}
