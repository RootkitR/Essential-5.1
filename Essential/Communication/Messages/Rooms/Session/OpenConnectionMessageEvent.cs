using System;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Session
{
	internal sealed class OpenConnectionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
		
			uint num = Event.PopWiredUInt();

			if (Essential.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Public Room [ID: " + num + "]");
			}
            RoomData @class = Essential.GetGame().GetRoomManager().method_12(num);

            Session.GetHabbo().uint_2 = num;
           Session.GetClientMessageHandler().method_5(num, "");
         /*   RoomData @class = Essential.GetGame().GetRoomManager().method_12(num);
			if (@class != null && !(@class.Type != "public"))
			{
				Session.GetClientMessageHandler().method_5(num, "");
			}
            else
            {
               
            }
            */
		}
	}
}
