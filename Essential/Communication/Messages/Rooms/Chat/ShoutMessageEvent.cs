using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Chat
{
	internal sealed class ShoutMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
                Session.GetHabbo().CheckForUnmute();
				RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (class2 != null && Session.GetHabbo().PassedSafetyQuiz)
				{
                    class2.HandleSpeech(Session, Essential.FilterString(Event.PopFixedString()), true, Event.PopWiredInt32());
				}
			}
		}
	}
}
