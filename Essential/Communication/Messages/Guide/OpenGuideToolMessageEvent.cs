using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class OpenGuideToolMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            if (!Session.GetHabbo().IsGuide)
                return;
            bool onDuty = Event.PopWiredBoolean();
            Essential.GetGame().GetGuideManager().ToggleState(onDuty, Session.GetHabbo().Id);
            ServerMessage GuideTool = new ServerMessage(Outgoing.SerializeGuideTool); //Rootkit
            GuideTool.AppendBoolean(Essential.GetGame().GetGuideManager().OnDuty(Session.GetHabbo().Id)); //Im dienst?
            GuideTool.AppendInt32(Essential.GetGame().GetGuideManager().GuidesOnDutyCount); //Helper im Dienst
            GuideTool.AppendInt32(0); //Guardians on Duty (WILL NEVER EXIST!!)
            Session.SendMessage(GuideTool);
        }
    }
}
