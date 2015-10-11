using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class RateFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && !Session.GetHabbo().list_4.Contains(@class.Id) && !@class.CheckRights(Session, true))
			{
				switch (Event.PopWiredInt32())
				{
				case -1:
					@class.Score--;
					break;
				case 0:
					return;
				case 1:
					@class.Score++;
                    
					break;
				default:
					return;
				}
				using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE rooms SET score = '",
						@class.Score,
						"' WHERE Id = '",
						@class.Id,
						"' LIMIT 1"
					}));
				}
				Session.GetHabbo().list_4.Add(@class.Id);
                ServerMessage Message = new ServerMessage(Outgoing.ScoreMeter); // Updated
				Message.AppendInt32(@class.Score);
                Message.AppendBoolean(Session.GetHabbo().CurrentRoom.CheckRights(Session, true));
				Session.SendMessage(Message);
			}
		}
	}
}
