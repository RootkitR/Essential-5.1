using Essential.HabboHotel.Rooms;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Rooms.Engine
{
    class LoadUserRoomMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            int num = Event.PopWiredInt32();
            int num2 = Event.PopWiredInt32();
            int num3 = Event.PopWiredInt32();
            if ((num2 == 1) && (num3 == 0))
            {
                Room room = Essential.GetGame().GetRoomManager().GetRoom((uint)num);
                if ((room != null) && (room.GetRoomUserByHabbo(Session.GetHabbo().Id) == null))
                {
                }
            }
            else if ((num2 != 0) || (num3 != 0))
            {
                RoomData data = Essential.GetGame().GetRoomManager().method_12((uint)num);
                if (data != null)
                {
                    ServerMessage message = new ServerMessage(Outgoing.RoomData);
                    message.AppendBoolean(false);
                    data.Serialize(message, false, false);
                    message.AppendBoolean(true);
                    message.AppendBoolean(false);
                    message.AppendBoolean(true);
                    message.AppendBoolean(true);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendBoolean(true);
                    Session.SendMessage(message);
                    //Console.WriteLine("aa");
                }
            }
        }
    }
}
