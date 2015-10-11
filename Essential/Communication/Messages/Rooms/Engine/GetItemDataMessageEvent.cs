using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class GetItemDataMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomItem class2 = @class.method_28(Event.PopWiredUInt());
				if (class2 != null && !(class2.GetBaseItem().InteractionType.ToLower() != "postit"))
				{
                    ServerMessage Message = new ServerMessage(Outgoing.OpenPostIt); // Update
					Message.AppendStringWithBreak(class2.uint_0.ToString());
					Message.AppendStringWithBreak(class2.ExtraData);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
