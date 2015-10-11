using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pathfinding;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorPuzzleBox : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			Room @class = RoomItem_0.GetRoom();
			RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (class2 != null && @class != null)
			{
				ThreeDCoord gstruct1_ = new ThreeDCoord(RoomItem_0.GetX + 1, RoomItem_0.Int32_1);
				ThreeDCoord gstruct1_2 = new ThreeDCoord(RoomItem_0.GetX - 1, RoomItem_0.Int32_1);
				ThreeDCoord gstruct1_3 = new ThreeDCoord(RoomItem_0.GetX, RoomItem_0.Int32_1 + 1);
				ThreeDCoord gstruct1_4 = new ThreeDCoord(RoomItem_0.GetX, RoomItem_0.Int32_1 - 1);
                if (ThreeDCoord.IsNot(class2.Position, gstruct1_) && ThreeDCoord.IsNot(class2.Position, gstruct1_2) && ThreeDCoord.IsNot(class2.Position, gstruct1_3) && ThreeDCoord.IsNot(class2.Position, gstruct1_4))
				{
					if (class2.bool_0)
					{
						class2.MoveTo(RoomItem_0.GStruct1_0);
					}
				}
				else
				{
					int num = RoomItem_0.GetX;
					int num2 = RoomItem_0.Int32_1;
                    if (ThreeDCoord.Equals(class2.Position, gstruct1_))
					{
						num = RoomItem_0.GetX - 1;
						num2 = RoomItem_0.Int32_1;
					}
					else
					{
                        if (ThreeDCoord.Equals(class2.Position, gstruct1_2))
						{
							num = RoomItem_0.GetX + 1;
							num2 = RoomItem_0.Int32_1;
						}
						else
						{
                            if (ThreeDCoord.Equals(class2.Position, gstruct1_3))
							{
								num = RoomItem_0.GetX;
								num2 = RoomItem_0.Int32_1 - 1;
							}
							else
							{
                                if (ThreeDCoord.Equals(class2.Position, gstruct1_4))
								{
									num = RoomItem_0.GetX;
									num2 = RoomItem_0.Int32_1 + 1;
								}
							}
						}
					}
					if (@class.method_37(num, num2, true, true, true, true, false, false, false))
					{
						List<RoomItem> list_ = new List<RoomItem>();
						list_ = @class.method_93(num, num2);
						double double_ = @class.method_84(num, num2, list_);
                        ServerMessage Message = new ServerMessage(Outgoing.ObjectOnRoller); // Update
						Message.AppendInt32(RoomItem_0.GetX);
						Message.AppendInt32(RoomItem_0.Int32_1);
						Message.AppendInt32(num);
						Message.AppendInt32(num2);
						Message.AppendInt32(1);
						Message.AppendUInt(RoomItem_0.uint_0);
                        Message.AppendStringWithBreak(double_.ToString());
						Message.AppendStringWithBreak(double_.ToString());
                        Message.AppendInt32(-1);
						@class.SendMessage(Message, null);
						@class.method_81(RoomItem_0, num, num2, double_);
					}
				}
			}
		}
	}
}
