using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class SetObjectDataMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
                int num = Event.PopWiredInt32();
                uint Data = Event.PopWiredUInt();
                string BrandData = "state" + Convert.ToChar(9) + "0";
                for (int i = 1; i <= Data; i++)
                {
                    BrandData = BrandData + Convert.ToChar(9) + Event.PopFixedString();
                }
                using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                {
                    class2.AddParamWithValue("extradata", BrandData);
                    class2.ExecuteQuery("UPDATE items_extra_data SET extra_data = @extradata WHERE item_id = '" + num + "' LIMIT 1");
                }
                /*ServerMessage Message = new ServerMessage();
                Message.Init(Outgoing.ObjectDataUpdate); // Update
                Message.AppendStringWithBreak(num.ToString());
                Message.AppendInt32(0);
                Message.AppendInt32(1);
                Message.AppendStringWithBreak(BrandData);
                @class.SendMessage(Message, null);*/
                @class.method_28((uint)num).ExtraData = BrandData;
                @class.method_79(Session, @class.method_28((uint)num), @class.method_28((uint)num).GetX, @class.method_28((uint)num).Int32_1, @class.method_28((uint)num).int_3, false, false, true);
                @class.method_28((uint)num).UpdateState(true, false, true);
				}
			
		}
	}
}
