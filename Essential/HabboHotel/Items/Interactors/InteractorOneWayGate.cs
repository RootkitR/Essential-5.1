using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pathfinding;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorOneWayGate : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
            if (RoomItem_0.InteractingUser != 0u)
			{
                RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser);
				if (@class != null)
				{
					@class.method_3(true);
					@class.method_6();
				}
                RoomItem_0.InteractingUser = 0u;
			}
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
            if (RoomItem_0.InteractingUser != 0u)
			{
                RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(RoomItem_0.InteractingUser);
				if (@class != null)
				{
					@class.method_3(true);
					@class.method_6();
				}
                RoomItem_0.InteractingUser = 0u;
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			RoomUser @class = RoomItem_0.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (@class != null && (RoomItem_0.GStruct1_2.x < RoomItem_0.GetRoom().RoomModel.int_4 && RoomItem_0.GStruct1_2.y < RoomItem_0.GetRoom().RoomModel.int_5))
			{
                if (ThreeDCoord.IsNot(@class.Position, RoomItem_0.GStruct1_1) && @class.bool_0)
				{
					@class.MoveTo(RoomItem_0.GStruct1_1);
				}
				else
				{
                    if (RoomItem_0.GetRoom().method_30(RoomItem_0.GStruct1_2.x, RoomItem_0.GStruct1_2.y, RoomItem_0.Double_0, true, false) && RoomItem_0.InteractingUser == 0)
					{
                        RoomItem_0.InteractingUser = @class.UId;
						@class.bool_0 = false;
						if (@class.bool_6 && (@class.int_10 != RoomItem_0.GStruct1_1.x || @class.int_11 != RoomItem_0.GStruct1_1.y))
						{
							@class.method_3(true);
						}
						@class.bool_1 = true;
						@class.MoveTo(RoomItem_0.GStruct1_0);
						RoomItem_0.ReqUpdate(3);
					}
				}
			}
		}
	}
}
