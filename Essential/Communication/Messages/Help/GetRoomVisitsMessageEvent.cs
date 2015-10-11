using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetRoomVisitsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				Session.SendMessage(Essential.GetGame().GetModerationTool().method_19(uint_));
			}
		}
	}
}
