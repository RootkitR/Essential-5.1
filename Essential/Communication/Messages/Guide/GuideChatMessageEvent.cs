using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class GuideChatMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string message = Essential.FilterString(Event.PopFixedString());
            if (string.IsNullOrEmpty(message.Trim()))
                return;
            if (Essential.GetAntiAd().ContainsIllegalWord(message))
            {
                ServerMessage Message2 = new ServerMessage(Outgoing.InstantChat);
                Message2.AppendUInt(0u);
                Message2.AppendString("[AWS] " + Session.GetHabbo().Username +": " + message);
                Message2.AppendString(Essential.GetUnixTimestamp() + string.Empty);
                Essential.GetGame().GetClientManager().SendToStaffs(Session, Message2);
                Session.SendNotification(Essential.GetGame().GetRoleManager().GetConfiguration().getData("antiad.alert"));
                return;
            }
            // I don't "bobba" filter the word.. I just check if the message contains a Illegal word. And if,
            // I "close" the Event!
            Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).StoreMessage(message, Session.GetHabbo().Id);
            ServerMessage Message = new ServerMessage(Outgoing.GuideSessionMessage); //Rootkit
            Message.AppendString(message);
            Message.AppendInt32(Session.GetHabbo().Id);
            Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id).SendToTicket(Message);
        }
    }
}
