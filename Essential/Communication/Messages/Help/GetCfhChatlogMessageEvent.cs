using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Support;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetCfhChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				SupportTicket @class = Essential.GetGame().GetModerationTool().method_5(Event.PopWiredUInt());
				if (@class != null)
				{
                    RoomData class2 = Essential.GetGame().GetRoomManager().method_11(@class.RoomId);
					if (class2 != null)
					{
                        Session.SendMessage(Essential.GetGame().GetModerationTool().method_21(@class, class2, @class.Timestamp));
					}
				}
			}
		}
	}
}
