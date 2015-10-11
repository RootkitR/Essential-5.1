using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Quest
{
	internal sealed class OpenQuestTrackerMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Essential.GetGame().GetQuestManager().OpenQuestTracker(Session);
		}
	}
}
