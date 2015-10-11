using System;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Handshake
{
	internal sealed class SSOTicketMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetHabbo() == null)
                Session.tryLogin(Event.PopFixedString());
		}
	}
}
