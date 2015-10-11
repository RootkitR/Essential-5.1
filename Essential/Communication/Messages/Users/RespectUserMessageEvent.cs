using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Users
{
	internal sealed class RespectUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if(!@class.CanRespect)
            {
                Session.GetHabbo().Whisper("Loben ist im Raum deaktiviert!");
                return;
            }
			if (@class != null && Session.GetHabbo().RespectPoints > 0)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Event.PopWiredUInt());
				if (class2 != null && class2.GetClient().GetHabbo().Id != Session.GetHabbo().Id && !class2.IsBot)
				{
					Session.GetHabbo().RespectPoints--;
					Session.GetHabbo().RespectGiven++;
					class2.GetClient().GetHabbo().Respect++;
					using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
					{
						class3.ExecuteQuery("UPDATE user_stats SET Respect = respect + 1 WHERE Id = '" + class2.GetClient().GetHabbo().Id + "' LIMIT 1");
						class3.ExecuteQuery("UPDATE user_stats SET RespectGiven = RespectGiven + 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
						class3.ExecuteQuery("UPDATE user_stats SET dailyrespectpoints = dailyrespectpoints - 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
					}
                    ServerMessage ThumbUp = new ServerMessage(Outgoing.Action); // Updated
                    ThumbUp.AppendInt32(@class.GetRoomUserByHabbo(Session.GetHabbo().Id).VirtualId);
                    ThumbUp.AppendInt32(7);
                    @class.SendMessage(ThumbUp, null);

                    /*ServerMessage ThumbOnHead = new ServerMessage(Outgoing.Action); // Updated
                    ThumbOnHead.AppendInt32(class2.VirtualId);
                    ThumbOnHead.AppendInt32(2);
                    @class.SendMessage(ThumbOnHead, null);*/
                    ServerMessage Message = new ServerMessage(Outgoing.GiveRespect); // Updated
					Message.AppendUInt(class2.GetClient().GetHabbo().Id);
					Message.AppendInt32(class2.GetClient().GetHabbo().Respect);
					@class.SendMessage(Message, null);
                    Session.GetHabbo().CheckRespectGivedAchievements();
                    class2.GetClient().GetHabbo().CheckRespectReceivedAchievements();
                    if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "GIVE_RESPECT")
					{
                        Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
					}
				}
			}
		}
	}
}
