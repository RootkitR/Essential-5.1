using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Pathfinding;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorVendor : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.InteractingUser > 0u)
			{
				RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser);
				if (@class != null)
				{
					@class.bool_0 = true;
				}
			}
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.InteractingUser > 0u)
			{
				RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser);
				if (@class != null)
				{
					@class.bool_0 = true;
				}
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (RoomItem_0.ExtraData != "1" && RoomItem_0.GetBaseItem().VendingIds.Count >= 1 && RoomItem_0.InteractingUser == 0u)
			{
				if (Session != null)
				{
					RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class == null)
					{
						return;
					}
                    if (!RoomItem_0.GetRoom().method_99(@class.X, @class.Y, RoomItem_0.GetX, RoomItem_0.Int32_1))
					{
						if (!@class.bool_0)
						{
							return;
						}
						try
						{
							@class.MoveTo(RoomItem_0.GStruct1_1);
							return;
						}
						catch
						{
							return;
						}
					}
					RoomItem_0.InteractingUser = Session.GetHabbo().Id;
					@class.bool_0 = false;
					@class.method_3(true);
                    @class.method_9(Rotation.GetRotation(@class.X, @class.Y, RoomItem_0.GetX, RoomItem_0.Int32_1));
				}
				RoomItem_0.ReqUpdate(2);
				RoomItem_0.ExtraData = "1";
				RoomItem_0.UpdateState(false, true);
			}
		}
	}
}
