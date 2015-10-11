using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class DeletePendingCallsForHelpMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Essential.GetGame().GetModerationTool().method_9(Session.GetHabbo().Id))
			{
				Essential.GetGame().GetModerationTool().method_10(Session.GetHabbo().Id);
				/*ServerMessage Message5_ = new ServerMessage(320u);
				Session.SendMessage(Message5_);
                */
			}
		}
	}
}
