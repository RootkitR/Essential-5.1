using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Quest
{
	internal sealed class GetQuestsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Session.SendMessage(Essential.GetGame().GetQuestManager().SerializeQuests(Session));
		}
	}
}
