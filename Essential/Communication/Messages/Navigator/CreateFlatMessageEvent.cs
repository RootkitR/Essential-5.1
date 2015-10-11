using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Util;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class CreateFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().OwnedRooms.Count <= ServerConfiguration.RoomUserLimit)
			{
				string Name = Essential.FilterString(Event.PopFixedString());
				string Model = Event.PopFixedString();
			//	Event.PopFixedString();
                RoomData NewRoom = Essential.GetGame().GetRoomManager().CreateRoom(Session, Name, Model);
                if (NewRoom != null)
				{
                    ServerMessage Message = new ServerMessage(Outgoing.OnCreateRoomInfo); // Update
                    Message.AppendUInt(NewRoom.Id);
                    Message.AppendStringWithBreak(NewRoom.Name);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
