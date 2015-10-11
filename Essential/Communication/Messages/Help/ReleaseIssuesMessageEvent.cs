using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class ReleaseIssuesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					uint uint_ = Event.PopWiredUInt();
					Essential.GetGame().GetModerationTool().method_7(Session, uint_);
				}
			}
		}
	}
}
