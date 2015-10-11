using System;
using System.Linq;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class SetClothingChangeDataMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.method_26(Session))
            {
                uint num = Event.PopWiredUInt();
                string a = Event.PopFixedString().ToUpper();
                string text = Essential.FilterString(Event.PopFixedString());
                text = text.Replace("hd-99999-99999", "");
                text += ".";
                RoomItem class2 = @class.Hashtable_0[num] as RoomItem;
                if (class2 != null)
                {
                    if (class2.ExtraData.Contains(','))
                    {
                        class2.string_2 = class2.ExtraData.Split(new char[]
					{
						','
					})[0];
                        class2.string_3 = class2.ExtraData.Split(new char[]
					{
						','
					})[1];
                    }
                    if (a == "M")
                    {
                        class2.string_2 = text;
                    }
                    else
                    {
                        class2.string_3 = text;
                    }
                    class2.ExtraData = class2.string_2 + "," + class2.string_3;
                    class2.UpdateState(true, true);
                }
            }
		}
	}
}
