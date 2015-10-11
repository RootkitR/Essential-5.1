using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;

namespace Essential.Communication.Messages.Navigator
{
    internal sealed class EventLog : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string where = Event.PopFixedString();
            string what = Event.PopFixedString();
            string even1t = Event.PopFixedString();
            if(what == "GAMES" && where == "Toolbar" &&  even1t == "client.toolbar.clicked")
            {
                if (Session.GetHabbo().CurrentRoomId != 73231)
                {
                    RoomData room = Essential.GetGame().GetRoomManager().method_12(73231);
                    if (room != null)
                    {
                        ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                        Message.AppendBoolean(false);
                        Message.AppendUInt(room.Id);
                        Session.SendMessage(Message);
                    }
                }
            }
        }
    }
}
