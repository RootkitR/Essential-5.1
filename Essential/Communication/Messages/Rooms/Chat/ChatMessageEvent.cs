using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Chat
{
	internal sealed class ChatMessageEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Session.GetHabbo().CheckForUnmute();
                Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                if (room != null)
                {
                    RoomUser user = room.GetRoomUserByHabbo(Session.GetHabbo().Id);

                    if (user != null && Session.GetHabbo().PassedSafetyQuiz)
                        user.HandleSpeech(Session, Essential.FilterString(Event.PopFixedString()), false, Event.PopWiredInt32());
                }
            }
        }
	}
}
