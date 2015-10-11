using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Avatar
{
    internal sealed class SignMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {

            Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    int SignId = Event.PopWiredInt32();
                    roomUserByHabbo.Unidle();

                    roomUserByHabbo.AddStatus("sign", Convert.ToString(SignId));
                    roomUserByHabbo.UpdateNeeded = true;

                    roomUserByHabbo.CarryItem(0);
                }
            }
        }
    }
}
