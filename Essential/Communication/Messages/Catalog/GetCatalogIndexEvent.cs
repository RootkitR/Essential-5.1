using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class GetCatalogIndexEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetHabbo() != null)
            {
                Session.SendMessage(Essential.GetGame().GetCatalog().GetIndexMessageForRank(Session.GetHabbo().Rank));
            }
		}
	}
}
