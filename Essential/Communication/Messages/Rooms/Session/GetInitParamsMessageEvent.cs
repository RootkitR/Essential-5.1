using System;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Session
{
    internal sealed class GetInitParamsMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            RoomData @class = Essential.GetGame().GetRoomManager().method_12(Session.GetHabbo().uint_2);

            ServerMessage Message = new ServerMessage(Outgoing.InitialRoomInformation); // P
            Message.AppendStringWithBreak(@class.ModelName);
            Message.AppendUInt(@class.Id);
            Session.SendMessage(Message);

        }
    }
}
