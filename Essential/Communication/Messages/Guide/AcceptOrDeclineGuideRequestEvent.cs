using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class AcceptOrDeclineGuideRequestEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            bool accept = Event.PopWiredBoolean();
            if (!accept)
            {

                Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).SendToTicket(Essential.GetGame().GetGuideManager().DetachedMessage);
                Essential.GetGame().GetGuideManager().RemoveTicket(Session.GetHabbo().Id);
                Essential.GetGame().GetGuideManager().GetGuideById(Session.GetHabbo().Id).IsInUse = false;
                return;
            }
            ServerMessage Message = new ServerMessage(Outgoing.GuideSessionStarted); //Rootkit
            Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).Serialize(Message);
            Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).Answered = true;
            Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).SendToTicket(Message);
        }
    }
}
