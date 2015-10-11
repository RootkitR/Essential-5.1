using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Wired
{
	internal sealed class UpdateConditionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				uint uint_ = Event.PopWiredUInt();
				RoomItem class2 = @class.method_28(uint_);
				string text = class2.GetBaseItem().InteractionType.ToLower();
				if (text != null && (text == "wf_cnd_trggrer_on_frn" || text == "wf_cnd_furnis_hv_avtrs" || text == "wf_cnd_has_furni_on" || text == "wf_cnd_not_furni_on" || text == "wf_cnd_not_hv_avtrs"))
                {
                    Event.PopWiredInt32();
                    int num34 = Event.PopWiredInt32();
                    int num2 = Event.PopWiredInt32();
                    int num3 = Event.PopWiredInt32();
                    Event.PopFixedString();
					int num = Event.PopWiredInt32();
					class2.string_3 = "";
					for (int i = 0; i < num; i++)
					{
						class2.string_3 = class2.string_3 + "," + Convert.ToString(Event.PopWiredUInt());
					}
					if (class2.string_3.Length > 0)
					{
						class2.string_3 = class2.string_3.Substring(1);
					}
				}
                if (text != null && (text == "wf_cnd_user_count_in" || text == "wf_cnd_not_user_count"))
                {
                    Event.PopWiredInt32();
                    class2.string_2 = Event.PopWiredInt32() + ";" + Event.PopWiredInt32();
                }
                if (text != null && (text == "wf_cnd_match_snapshot" || text == "wf_cnd_not_match_snap"))
                {
                    Event.PopWiredInt32();
                    class2.string_3 = "";
                    if (Event.PopWiredInt32() == 1)
                    {
                        RoomItem expr_4A8 = class2;
                        expr_4A8.string_3 += "I";
                    }
                    else
                    {
                        RoomItem expr_4C0 = class2;
                        expr_4C0.string_3 += "H";
                    }
                    if (Event.PopWiredInt32() == 1)
                    {
                        RoomItem expr_4E1 = class2;
                        expr_4E1.string_3 += "I";
                    }
                    else
                    {
                        RoomItem expr_4F9 = class2;
                        expr_4F9.string_3 += "H";
                    }
                    if (Event.PopWiredInt32() == 1)
                    {
                        RoomItem expr_51A = class2;
                        expr_51A.string_3 += "I";
                    }
                    else
                    {
                        RoomItem expr_532 = class2;
                        expr_532.string_3 += "H";
                    }
                    Event.PopFixedString();
                    int num2 = Event.PopWiredInt32();
                    class2.string_2 = "";
                    class2.string_4 = "";
                    class2.string_5 = "";
                    if (num2 > 0)
                    {
                        class2.string_5 = OldEncoding.encodeVL64(num2);
                        for (int i = 0; i < num2; i++)
                        {
                            int num3 = Event.PopWiredInt32();
                            class2.string_5 += OldEncoding.encodeVL64(num3);
                            class2.string_4 = class2.string_4 + "," + Convert.ToString(num3);
                            RoomItem class3 = @class.method_28(Convert.ToUInt32(num3));
                            RoomItem expr_5E6 = class2;
                            object string_2 = expr_5E6.string_2;
                            expr_5E6.string_2 = string.Concat(new object[]
							{
								string_2,
								";",
                                class3.uint_0,
                                ",",
								class3.GetX,
								",",
								class3.Int32_1,
								",",
								class3.Double_0,
								",",
								class3.int_3,
								",",
								class3.ExtraData == string.Empty ? "0" : class3.ExtraData
							});
                        }
                        class2.string_4 = class2.string_4.Substring(1);
                        class2.string_2 = class2.string_2.Substring(1);
                    }
                }
                Session.SendMessage(new ServerMessage(Outgoing.SaveWired)); // NEW
                class2.UpdateState(true, false);
			}
			catch
			{
			}
		}
	}
}
