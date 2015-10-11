using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class RedeemVoucherMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Essential.GetGame().GetCatalog().GetVoucherHandler().HandleVoucher(Session, Event.PopFixedString());
		}
	}
}
