using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Quest
{
	internal sealed class AcceptQuestMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
            Essential.GetGame().GetQuestManager().ActivateQuest(uint_, Session);
		}
	}
}
