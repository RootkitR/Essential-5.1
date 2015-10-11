using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetRoomChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_chatlogs"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				if (Essential.GetGame().GetRoomManager().GetRoom(uint_) != null)
				{
					Session.SendMessage(Essential.GetGame().GetModerationTool().method_22(uint_));
				}
			}
		}
	}
}
