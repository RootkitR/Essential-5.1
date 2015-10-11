using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
	internal sealed class DiceOffMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
					RoomItem class2 = @class.method_28(Event.PopWiredUInt());
					if (class2 != null)
					{
						bool bool_ = false;
						if (@class.method_26(Session))
						{
							bool_ = true;
						}
						class2.Interactor.OnTrigger(Session, class2, -1, bool_);
					}
				}
			}
			catch
			{
			}
		}
	}
}
