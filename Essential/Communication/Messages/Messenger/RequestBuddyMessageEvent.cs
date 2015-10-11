using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Messenger
{
	internal sealed class RequestBuddyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{

                try
                {
                    if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "REQUEST_FRIEND")
                    {
                        Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                    }
                }
                catch { }
				Session.GetHabbo().GetMessenger().method_16(Event.PopFixedString());
			}
		}
	}
}
