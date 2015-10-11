using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class GetFurnitureAliasesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().uint_2 > 0)
			{
                ServerMessage Message = new ServerMessage(Outgoing.FurnitureAliases);
				Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
		}
	}
}
