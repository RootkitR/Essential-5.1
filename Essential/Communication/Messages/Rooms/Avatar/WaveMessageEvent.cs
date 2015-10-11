using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Avatar
{
	internal sealed class WaveMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (class2 != null)
				{
                    
                    int Action = Event.PopWiredInt32();
                    //kiss == 2
					class2.Unidle();
					class2.DanceId = 0;
                    ServerMessage Message = new ServerMessage(Outgoing.Action); // Updated
					Message.AppendInt32(class2.VirtualId);
                    Message.AppendInt32(Action);
					@class.SendMessage(Message, null);

                    if (Action == 5) // idle
                    {
                        class2.bool_8 = true;

                        ServerMessage FallAsleep = new ServerMessage(Outgoing.IdleStatus);
                        FallAsleep.AppendInt32(class2.VirtualId);
                        FallAsleep.AppendBoolean(class2.bool_8);
                        @class.SendMessage(FallAsleep, null);
                    }


                    if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "WAVE")
					{
                        Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
					}
				}
			}
		}
	}
}
