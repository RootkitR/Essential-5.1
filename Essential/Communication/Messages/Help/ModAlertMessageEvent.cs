using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class ModAlertMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				Essential.GetGame().GetModerationTool().method_16(Session, uint_, string_, true);
			}
		}
	}
}
