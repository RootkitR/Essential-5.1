using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Games
{
    class GetGames : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if ((Session.GetHabbo() != null) && ((Session.GetHabbo().InRoom && (Session.GetHabbo().CurrentRoomId > 0))))
            {
            Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
            room.RemoveUserFromRoom(Session, true, false);
            }
            Session.GetHabbo().CurrentRoomId = 0;
            ServerMessage message1 = new ServerMessage(Outgoing.SendGroup);
            message1.AppendInt32(0);
            Session.SendMessage(message1);
            }
            ServerMessage response = new ServerMessage(664);
            response.AppendInt32(1); // Número de jogos (Normal = 10) 2 = Snow + Fast
            response.AppendInt32(1);
            response.AppendString("flappybird");
            response.AppendString("93d4f3");
            response.AppendString("");
            response.AppendString("");
            response.AppendString(Session.ClientVar + "/c_images/gamecenter_flappybird/");
            response.AppendString("");
            response.AppendString("");
            Session.SendMessage(response);
            ServerMessage message = new ServerMessage(1563);
            message.AppendInt32(2); // Quantidade de Games
            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
            message.AppendInt32(3);
            }
            else
            {
            message.AppendInt32(3);
            }
            message.AppendInt32(0);
            Session.SendMessage(message);
        }
    }
}
