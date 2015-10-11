using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Pathfinding;

namespace Essential.Communication.Messages.Rooms.Engine
{
    internal sealed class EnterInfobusMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (!Session.GetHabbo().CurrentRoom.CCTs.Contains("park"))
            {
                return;
            }

            if (Session.GetHabbo().CurrentRoom.IsInfobusOpen == false)
            {
                ServerMessage InfobusClosed = new ServerMessage(Outgoing.InfobusMessage);
                InfobusClosed.AppendStringWithBreak("Ich bin zurzeit geschlossen! Komm später wieder.");
                Session.SendMessage(InfobusClosed);
            }
            else
            {
                Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id).MoveTo(28, 5);

                if (Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id).X == 28 && Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id).Y == 5)
                {
                    ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                    Message.AppendBoolean(true);
                    Message.AppendInt32(56);
                    Session.SendMessage(Message);
                }
                    
                
            }
        }
    }
}
