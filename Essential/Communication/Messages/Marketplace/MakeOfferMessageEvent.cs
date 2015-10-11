using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Marketplace
{
	internal sealed class MakeOfferMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
                if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.Boolean_3)
					{
						return;
					}
				}
				int int_ = Event.PopWiredInt32();
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();

				UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItemById(uint_);
				if (class2 != null && class2.GetBaseItem().AllowTrade)
				{
					Essential.GetGame().GetCatalog().GetMarketplace().CreateOffer(Session, class2.uint_0, int_);
				}
			}
		}
	}
}
