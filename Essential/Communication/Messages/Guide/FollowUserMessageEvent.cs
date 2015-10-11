using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class FollowUserMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            GameClient otherClient = Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).GetOtherClient(Session.GetHabbo().Id);
            Session.LoadRoom(otherClient.GetHabbo().CurrentRoomId);
        }
    }
}
