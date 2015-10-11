using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.RoomBots;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Pathfinding;
namespace Essential.Communication.Messages.Rooms.Action
{
	internal sealed class CallGuideBotMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				for (int i = 0; i < @class.RoomUsers.Length; i++)
				{
					RoomUser class2 = @class.RoomUsers[i];
                    if (class2 != null && (class2.IsBot && class2.RoomBot.AiType == AIType.Guide))
					{
                        ServerMessage Message = new ServerMessage(Outgoing.GenericError);
						Message.AppendInt32(4009);
						Session.SendMessage(Message);
						return;
					}
				}
				if (Session.GetHabbo().bool_10)
				{
                    ServerMessage Message = new ServerMessage(Outgoing.GenericError);
					Message.AppendInt32(4010);
					Session.SendMessage(Message);
				}
				else
				{
                    RoomUser class3 = @class.BotToRoomUser(Essential.GetGame().GetBotManager().GetRoomBotById(2u));
					class3.method_7(@class.RoomModel.DoorX, @class.RoomModel.DoorY, @class.RoomModel.double_0);
					class3.UpdateNeeded = true;
					RoomUser class4 = @class.method_56(@class.Owner);
					if (class4 != null)
					{
						class3.MoveTo(class4.Position);
                        class3.method_9(Rotation.GetRotation(class3.X, class3.Y, class4.X, class4.Y));
					}
                    Session.GetHabbo().CallGuideBotAchievementsCompleted();
					Session.GetHabbo().bool_10 = true;
				}
			}
		}
	}
}
