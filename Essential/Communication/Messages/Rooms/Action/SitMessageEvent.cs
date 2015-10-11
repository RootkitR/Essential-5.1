using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Action
{
    internal sealed class SitMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            RoomUser RoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomUser.Statusses.ContainsKey("lay"))
            {
                return;
            }
            if (RoomUser.Statusses.ContainsKey("sit"))
            {
                RoomUser.Statusses.Remove("sit");
                RoomUser.UpdateNeeded = true;
                return;
            }
            if (!RoomUser.Statusses.ContainsKey("sit"))
            {
                if (!(RoomUser.X == Session.GetHabbo().CurrentRoom.RoomModel.DoorX && RoomUser.Y == Session.GetHabbo().CurrentRoom.RoomModel.DoorY))
                {
                    RoomUser.Statusses.Add("sit", ((RoomUser.double_0 + 1.0) / 2.0 - RoomUser.double_0 * 0.5).ToString().Replace(",", "."));
                    RoomUser.UpdateNeeded = true;
                }
            }
        }
    }
}
