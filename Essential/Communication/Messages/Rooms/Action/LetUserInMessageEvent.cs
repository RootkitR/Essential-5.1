using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Action
{
	internal sealed class LetUserInMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
				string string_ = Event.PopFixedString();
                bool canletin = Event.PopWiredBoolean();
				GameClient class2 = Essential.GetGame().GetClientManager().GetClientByHabbo(string_);
				if (class2 != null && class2.GetHabbo() != null && class2.GetHabbo().bool_6 && class2.GetHabbo().uint_2 == Session.GetHabbo().CurrentRoomId)
				{
					if (canletin)
					{
						class2.GetHabbo().bool_5 = true;
                        ServerMessage Message4 = new ServerMessage(Outgoing.FlatAccessible); // Updated
                        Message4.AppendString("");
                        class2.SendMessage(Message4);
					}
					else
					{
                        ServerMessage Message5 = new ServerMessage(Outgoing.DoorBellNoPerson); // Updated
                        Message5.AppendString("");
                        class2.SendMessage(Message5);
					}
				}
			}
		}
	}
}
