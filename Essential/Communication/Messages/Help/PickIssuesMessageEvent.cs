using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class PickIssuesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				Essential.GetGame().GetModerationTool().method_6(Session, uint_);
			}
		}
	}
}
