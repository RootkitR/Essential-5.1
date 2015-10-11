using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.RoomBots
{
	internal sealed class GenericBot : BotAI
	{
		private int int_2;
		private int int_3;
		public GenericBot(int int_4)
		{
			this.int_2 = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(10, 250);
			this.int_3 = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(10, 30);
		}
		public override void OnSelfEnterRoom()
		{
		}
		public override void OnSelfLeaveRoom(bool bool_0)
		{
		}
		public override void OnUserEnterRoom(RoomUser RoomUser_0)
		{
		}
		public override void OnUserLeaveRoom(GameClient Session)
		{
		}
		public override void OnUserSay(RoomUser RoomUser_0, string string_0)
		{
            if (base.GetRoom().method_100(base.GetRoomUser().X, base.GetRoomUser().Y, RoomUser_0.X, RoomUser_0.Y) <= 8)
			{
                BotResponse @class = base.GetRoomBot().GetResponse(string_0);
				if (@class != null)
				{
                    string text = base.GetRoom().method_20(RoomUser_0, @class.Response);
					string text2 = @class.ResponseType.ToLower();
					if (text2 != null)
					{
						if (!(text2 == "say"))
						{
							if (!(text2 == "shout"))
							{
								if (text2 == "whisper")
								{
                                    ServerMessage Message = new ServerMessage(Outgoing.Whisp); // Updated
									Message.AppendInt32(base.GetRoomUser().VirtualId);
									Message.AppendStringWithBreak(text);
                                    Message.AppendInt32(0);
                                    Message.AppendInt32(0);
                                    Message.AppendInt32(-1);
									RoomUser_0.GetClient().SendMessage(Message);
								}
							}
							else
							{
								base.GetRoomUser().HandleSpeech(null, text, true);
							}
						}
						else
						{
							base.GetRoomUser().HandleSpeech(null, text, false);
						}
					}
					if (@class.ServeId >= 1)
					{
						RoomUser_0.CarryItem(@class.ServeId);
					}
				}
			}
		}
		public override void OnUserShout(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnTimerTick()
		{
			if (this.int_2 <= 0)
			{
                if (base.GetRoomBot().list_0.Count > 0)
				{
                    RandomSpeech @class = base.GetRoomBot().GetRandomSpeech();
					base.GetRoomUser().HandleSpeech(null, @class.Message, @class.Shout);
				}
				this.int_2 = Essential.smethod_5(10, 300);
			}
			else
			{
				this.int_2--;
			}
			if (this.int_3 <= 0)
			{
                string text = base.GetRoomBot().WalkMode.ToLower();
				if (text != null && !(text == "stand"))
				{
					if (!(text == "freeroam"))
					{
						if (text == "specified_range")
						{
                            int int_ = Essential.smethod_5(base.GetRoomBot().min_x, base.GetRoomBot().max_x);
                            int int_2 = Essential.smethod_5(base.GetRoomBot().min_y, base.GetRoomBot().max_y);
							base.GetRoomUser().MoveTo(int_, int_2);
						}
					}
					else
					{
                        int int_ = Essential.smethod_5(0, base.GetRoom().RoomModel.int_4);
                        int int_2 = Essential.smethod_5(0, base.GetRoom().RoomModel.int_5);
						base.GetRoomUser().MoveTo(int_, int_2);
					}
				}
				this.int_3 = Essential.smethod_5(1, 30);
			}
			else
			{
				this.int_3--;
			}
		}
	}
}
