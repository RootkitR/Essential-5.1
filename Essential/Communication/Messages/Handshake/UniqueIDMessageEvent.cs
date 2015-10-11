using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Handshake
{
	internal sealed class UniqueIDMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Event.PopFixedString();
            Session.MachineId = Event.PopFixedString();
		}
	}
}
