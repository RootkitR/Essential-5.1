using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Guides;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guide
{
    class CallGuideMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            try
            {
                /*if (Essential.GetGame().GetGuideManager().UserMadeTicket(Session.GetHabbo().Id))
                {
                    Session.SendMessage(Essential.GetGame().GetGuideManager().ErrorMessage);
                    return;
                }*/
                Event.PopWiredInt32();
                string msg = Event.PopFixedString();
                GameClient randomGuide = Essential.GetGame().GetClientManager().GetClientByHabbo(Essential.GetGame().GetClientManager().GetNameById(Essential.GetGame().GetGuideManager().GetRandomGuide().Id));
                ServerMessage Message = new ServerMessage(Outgoing.GuideSessionAttached); //Rootkit
                Message.AppendBoolean(true);
                Message.AppendInt32(Session.GetHabbo().Id);
                Message.AppendString(msg);
                Message.AppendInt32(Essential.GetGame().GetGuideManager().Timer);
                randomGuide.SendMessage(Message);
                Message = new ServerMessage(Outgoing.GuideSessionAttached); //Rootkit
                Message.AppendBoolean(false);
                Message.AppendInt32(Session.GetHabbo().Id);
                Message.AppendString(msg);
                Message.AppendInt32(Essential.GetGame().GetGuideManager().Timer);
                Session.SendMessage(Message);
                Essential.GetGame().GetGuideManager().CreateTicket(Session.GetHabbo(), randomGuide.GetHabbo());
                Action<object> a = delegate(object obj)
                {
                    int i = Essential.GetGame().GetGuideManager().Timer;
                    while (i != 0)
                    {
                        Thread.Sleep(1000);
                        i--;
                    }
                    GuideTicket gt = Essential.GetGame().GetGuideManager().GetTicket(Session.GetHabbo().Id);
                    if (gt == null || !gt.Answered)
                    {
                        try
                        {
                            gt.SendToTicket(Essential.GetGame().GetGuideManager().DetachedMessage);
                            Essential.GetGame().GetGuideManager().RemoveTicket(Session.GetHabbo().Id);
                            Essential.GetGame().GetGuideManager().GetGuideById(gt.GuideId).IsInUse = false;
                        }
                        catch { }
                    }
                };
                new Task(a,"break").Start();
            }catch
            {
                Session.SendMessage(Essential.GetGame().GetGuideManager().ErrorMessage);
            }
        }
    }
}
