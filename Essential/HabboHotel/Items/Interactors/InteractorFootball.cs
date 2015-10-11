using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorFootball : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (Session != null)
			{
				RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
				Room class2 = RoomItem_0.GetRoom();
                if (RoomItem_0.GetRoom().method_99(RoomItem_0.GetX, RoomItem_0.Int32_1, @class.X, @class.Y))
				{
					RoomItem_0.GetRoom().method_10(@class, RoomItem_0);
                    int num = RoomItem_0.GetX;
					int num2 = RoomItem_0.Int32_1;
					RoomItem_0.ExtraData = "11";
					if (@class.BodyRotation == 4)
					{
						num2--;
					}
					else
					{
						if (@class.BodyRotation == 0)
						{
							num2++;
						}
						else
						{
							if (@class.BodyRotation == 6)
							{
								num++;
							}
							else
							{
								if (@class.BodyRotation == 2)
								{
									num--;
								}
								else
								{
									if (@class.BodyRotation == 3)
									{
										num--;
										num2--;
									}
									else
									{
										if (@class.BodyRotation == 1)
										{
											num--;
											num2++;
										}
										else
										{
											if (@class.BodyRotation == 7)
											{
												num++;
												num2++;
											}
											else
											{
												if (@class.BodyRotation == 5)
												{
													num++;
													num2--;
												}
											}
										}
									}
								}
							}
						}
					}
                    @class.MoveTo(RoomItem_0.GetX, RoomItem_0.Int32_1);
					class2.method_79(null, RoomItem_0, num, num2, 0, false, true, true);
				}
			}
		}
	}
}
