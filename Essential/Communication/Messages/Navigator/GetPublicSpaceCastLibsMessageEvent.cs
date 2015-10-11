using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class GetPublicSpaceCastLibsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			//uint num = Event.PopWiredUInt();

            RoomData @class = Essential.GetGame().GetRoomManager().method_12(13);
			if (@class != null)
			{
				if (@class.Type == "private")
				{
                    ServerMessage Message = new ServerMessage(Outgoing.RoomForward); // Updated
					Message.AppendBoolean(@class.IsPublicRoom);
					Message.AppendUInt(13);
					Session.SendMessage(Message);
				}
				else
				{
                    ServerMessage Message2 = new ServerMessage(Outgoing.RoomVisits); // Updated
                    Message2.AppendUInt(@class.Id);
                    Message2.AppendStringWithBreak(@class.CCTs);
                    Message2.AppendUInt(@class.Id);
                    Session.SendMessage(Message2);
				}
			}
		}
	}
}
