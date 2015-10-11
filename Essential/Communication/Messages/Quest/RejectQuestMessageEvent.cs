using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Quest
{
	internal sealed class RejectQuestMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Essential.GetGame().GetQuestManager().ActivateQuest(0u, Session);
		}
	}
}
