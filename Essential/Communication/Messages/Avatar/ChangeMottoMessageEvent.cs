using System;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Avatar
{
	internal sealed class ChangeMottoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = Essential.FilterString(Event.PopFixedString());
			if (text.Length <= 50 && !(text != ChatCommandHandler.ApplyFilter(text)) && !(text == Session.GetHabbo().Motto))
			{
				Session.GetHabbo().Motto = text;
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("motto", text);
					@class.ExecuteQuery("UPDATE users SET motto = @motto WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
				}
                if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "CHANGEMOTTO")
				{
                    Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
				}
                if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					if (class14_ == null)
					{
						return;
					}
					RoomUser class2 = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (class2 == null)
					{
						return;
					}
                    ServerMessage Message2 = new ServerMessage(Outgoing.UpdateUserInformation); //Rootkit
					Message2.AppendInt32(class2.VirtualId);
					Message2.AppendStringWithBreak(Session.GetHabbo().Figure);
					Message2.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower()); 
					Message2.AppendStringWithBreak(Session.GetHabbo().Motto);
					Message2.AppendInt32(Session.GetHabbo().AchievementScore);
				//	Message2.AppendStringWithBreak("");
					class14_.SendMessage(Message2, null);
				}

                Session.GetHabbo().MottoAchievementsCompleted();
			}
		}
	}
}
