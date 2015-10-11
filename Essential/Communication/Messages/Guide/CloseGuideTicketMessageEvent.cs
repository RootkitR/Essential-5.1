using Essential.HabboHotel.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class CloseGuideTicketMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            GuideTicket gt = Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id);

            Essential.GetGame().GetGuideManager().GetGuideById(gt.GuideId).IsInUse = false;
            gt.SendToTicket(Essential.GetGame().GetGuideManager().DetachedMessage);
            Essential.GetGame().GetGuideManager().RemoveTicket(Session.GetHabbo().Id);
            
        }
    }
}
