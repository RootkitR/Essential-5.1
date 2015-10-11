using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetModeratorUserInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				if (Essential.GetGame().GetClientManager().GetNameById(uint_) != "Unknown User")
				{
					Session.SendMessage(Essential.GetGame().GetModerationTool().method_18(uint_));
				}
				else
				{
					Session.SendNotification("Could not load user info, invalid user.");
				}
			}
		}
	}
}
