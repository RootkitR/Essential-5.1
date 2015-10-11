using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class GetInterstitialMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetClientMessageHandler().method_4();
		}
	}
}
