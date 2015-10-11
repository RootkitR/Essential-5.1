using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class GuideUserStartsTypingMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            ServerMessage Message = new ServerMessage(Outgoing.ToggleGuideTicketTyping); //Rootkit
            Message.AppendBoolean(Event.PopWiredBoolean());
            Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).SendToOther(Message,Session.GetHabbo().Id);
        }
    }
}
