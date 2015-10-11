using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Action
{
	internal sealed class BanUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Console.WriteLine("Tries to bann User from Room!");
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.GetRoomUserByHabbo(uint_);
				if (class2 != null && !class2.IsBot && !class2.GetClient().GetHabbo().HasFuse("acc_unbannable"))
				{
					@class.method_70(uint_);
					@class.RemoveUserFromRoom(class2.GetClient(), true, false);
				}
			}
		}
	}
}
