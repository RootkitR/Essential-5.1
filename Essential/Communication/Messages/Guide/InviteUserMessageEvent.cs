using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Guides;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class InviteUserMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            GuideTicket gt = Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id);
            ServerMessage Message = new ServerMessage(Outgoing.GuideSessionInvitedToGuideRoom); //Rootkit
            Message.AppendInt32(Session.GetHabbo().CurrentRoomId);
            Message.AppendString(Session.GetHabbo().CurrentRoom.Name);
            gt.SendToTicket(Message);
        }
    }
}
