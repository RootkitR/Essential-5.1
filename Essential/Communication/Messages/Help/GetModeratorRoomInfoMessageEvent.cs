using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetModeratorRoomInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
                RoomData class27_ = Essential.GetGame().GetRoomManager().method_11(uint_);
				Session.SendMessage(Essential.GetGame().GetModerationTool().method_14(class27_));
			}
		}
	}
}
