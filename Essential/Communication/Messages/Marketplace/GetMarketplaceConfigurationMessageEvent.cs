using System;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Messages;
namespace Essential.Communication.Messages.Marketplace
{
	internal sealed class GetMarketplaceConfigurationMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - Essential.ServerStarted;
            ServerMessage Message = new ServerMessage(Outgoing.ShopData1); // Updated
			Message.AppendBoolean(true);
            Message.AppendInt32(ServerConfiguration.MarketplaceTax);
			Message.AppendInt32(1);
			Message.AppendInt32(5);
			Message.AppendInt32(1);
			Message.AppendInt32(ServerConfiguration.MarketplacePriceLimit);
			Message.AppendInt32(48);
			Message.AppendInt32(timeSpan.Days);
			Session.SendMessage(Message);
		}
	}
}
