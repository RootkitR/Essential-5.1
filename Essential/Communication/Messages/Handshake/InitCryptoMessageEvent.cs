using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Handshake
{
	internal sealed class InitCryptoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{

            ServerMessage SendBannerMessageComposer = new ServerMessage(Outgoing.SendBannerMessageComposer);
            SendBannerMessageComposer.AppendString("12f449917de4f94a8c48dbadd92b6276");
            SendBannerMessageComposer.AppendBoolean(false);
            Session.SendMessage(SendBannerMessageComposer);

		}
	}
}
