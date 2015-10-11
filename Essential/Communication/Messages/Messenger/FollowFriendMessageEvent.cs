using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Messenger
{
    internal sealed class FollowFriendMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			GameClient @class = Essential.GetGame().GetClientManager().GetClient(uint_);
            if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().InRoom)
			{
				Room room = Essential.GetGame().GetRoomManager().GetRoom(@class.GetHabbo().CurrentRoomId);
				if (room != null && Session != null && Session.GetHabbo() != null && room != Session.GetHabbo().CurrentRoom)
				{
                    ServerMessage FollowBuddy = new ServerMessage(Outgoing.RoomForward);
                    FollowBuddy.AppendBoolean(room.IsPublic);
                    FollowBuddy.AppendInt32(@class.GetHabbo().CurrentRoomId);
                    Session.SendMessage(FollowBuddy);
				}
			}
		}
	}
}
