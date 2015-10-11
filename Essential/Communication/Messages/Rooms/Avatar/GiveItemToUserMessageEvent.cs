using Essential.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Rooms.Avatar
{
    class GiveItemToUserMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            RoomUser Current = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
            RoomUser Target = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Event.PopWiredUInt());
            if(Current != null && Target != null)
            {
                Current.Unidle();
                Target.Unidle();
                Target.CarryItem(Current.CarryItemID);
                Current.CarryItem(0);

            }
        }
    }
}
