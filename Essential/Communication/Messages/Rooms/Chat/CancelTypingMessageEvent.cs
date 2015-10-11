using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Chat
{
    internal sealed class CancelTypingMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null)
                {
                    RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (class2 != null)
                    {
                        ServerMessage Message = new ServerMessage(Outgoing.TypingStatus); // Updated
                        Message.AppendInt32(class2.VirtualId);
                        Message.AppendInt32(0);
                        @class.SendMessage(Message, null);
                    }
                }
            }
        }
    }
}
