using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetUserChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_chatlogs"))
			{
				Session.SendMessage(Essential.GetGame().GetModerationTool().method_20(Event.PopWiredUInt()));
			}
		}
	}
}
