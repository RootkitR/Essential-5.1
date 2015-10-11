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
    class ReportUserMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string string_ = Essential.FilterString(Event.PopFixedString());

            GuideTicket gt = Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id);
            Essential.GetGame().GetGuideManager().GetGuideById(gt.GuideId).IsInUse = false;
            gt.SendToTicket(Essential.GetGame().GetGuideManager().DetachedMessage);
            Essential.GetGame().GetGuideManager().RemoveTicket(Session.GetHabbo().Id);
            Essential.GetGame().GetModerationTool().method_3(Session, 0, gt.GetOtherClient(Session.GetHabbo().Id).GetHabbo().Id, string_);
        }
    }
}
