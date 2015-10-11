using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Inventory.Trading
{
	internal sealed class AddItemToTradeEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CanTrade)
			{
				Trade class2 = @class.method_76(Session.GetHabbo().Id);
				UserItem class3 = Session.GetHabbo().GetInventoryComponent().GetItemById(Event.PopWiredUInt());
				if (class2 != null && class3 != null)
				{
					class2.method_2(Session.GetHabbo().Id, class3);
				}
			}
		}
	}
}
