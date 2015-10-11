using System;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class CanCreateRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Message = new ServerMessage(Outgoing.CanCreateRoom); // Updated
            Message.AppendInt32(0);
            Message.AppendInt32(99999);
			Session.SendMessage(Message);
		}
	}
}
