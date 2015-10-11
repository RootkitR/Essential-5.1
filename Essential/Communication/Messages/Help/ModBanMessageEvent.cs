using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class ModBanMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				int int_ = Event.PopWiredInt32() * 3600;
				Essential.GetGame().GetModerationTool().method_17(Session, uint_, int_, string_);
			}
		}
	}
}
