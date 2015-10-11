using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class CanCreateEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				bool bool_ = true;
				int int_ = 0;
				if (@class.State != 0)
				{
					bool_ = false;
					int_ = 3;
				}
                ServerMessage Message = new ServerMessage(Outgoing.CanCreateEvent);
				Message.AppendBoolean(bool_);
				Message.AppendInt32(int_);
				Session.SendMessage(Message);
			}
		}
	}
}
