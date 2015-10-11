using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class CancelEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event != null)
			{
				@class.Event = null;
                ServerMessage Message = new ServerMessage(Outgoing.RoomEvent); // Updated
				Message.AppendStringWithBreak("-1");
				@class.SendMessage(Message, null);
			}
		}
	}
}
