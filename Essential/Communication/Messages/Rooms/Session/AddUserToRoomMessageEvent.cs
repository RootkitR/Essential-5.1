using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Rooms.Session
{
    class AddUserToRoomMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Session.GetClientMessageHandler().GoToRoom();
        }
    }
}
