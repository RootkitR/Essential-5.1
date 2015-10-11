using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Session
{
	internal sealed class GoToFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetClientMessageHandler().method_6();
		}
	}
}
