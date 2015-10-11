using Essential.Communication.Messages.Handshake;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.TalentTrack
{
    internal sealed class PostQuizAnswers : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string str = Event.PopFixedString();
            int num = Event.PopWiredInt32();
            int num2 = Event.PopWiredInt32();
            int num3 = Event.PopWiredInt32();
            int num4 = Event.PopWiredInt32();
            int num5 = Event.PopWiredInt32();
            int num6 = Event.PopWiredInt32();
                List<int> list = new List<int>();
                if (str.Equals("HabboWay1"))
                {
                    if (num2 != 3)
                    {
                        list.Add(5);
                    }
                    if (num3 != 3)
                    {
                        list.Add(7);
                    }
                    if (num4 != 2)
                    {
                        list.Add(0);
                    }
                    if (num5 != 1)
                    {
                        list.Add(1);
                    }
                    if (num6 != 1)
                    {
                        list.Add(6);
                    }
                }else if (str.Equals("SafetyQuiz1"))
                {
                    if (num2 != 0)
                    {
                        list.Add(5);
                    }
                    if (num3 != 1)
                    {
                        list.Add(7);
                    }
                    if (num4 != 1)
                    {
                        list.Add(0);
                    }
                    if (num5 != 1)
                    {
                        list.Add(1);
                    }
                    if (num6 != 1)
                    {
                        list.Add(6);
                    }
                }
                ServerMessage message = new ServerMessage(Outgoing.CheckQuiz);
                message.AppendString(str);//HabboWay1
                message.AppendInt32(list.Count);
                foreach (int num7 in list)
                {
                    message.AppendInt32(num7);
                }
                Session.SendMessage(message);
                if (list.Count == 0)
                {
                    if (str == "HabboWay1")
                    {
                        Essential.GetGame().GetAchievementManager().addAchievement(Session, 25, 1);
                        Session.GetHabbo().PassedHabboWayQuiz = true;
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery("UPDATE users SET quiz_passed='1' WHERE id=" + Session.GetHabbo().Id);
                        }
                        ServerMessage msg = new ServerMessage(Outgoing.TalentTrackEarned);
                        msg.AppendString("citizenship");
                        msg.AppendInt32(1);
                        Session.SendMessage(msg);
                    }else if(str == "SafetyQuiz1")
                    {
                        Essential.GetGame().GetAchievementManager().addAchievement(Session, 23, 1);
                        Session.GetHabbo().PassedSafetyQuiz = true;
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery("UPDATE users SET passed_safety_quiz='1' WHERE id=" + Session.GetHabbo().Id);
                        }
                        ServerMessage msg = new ServerMessage(Outgoing.TalentTrackEarned);
                        msg.AppendString("citizenship");
                        msg.AppendInt32(1);
                        Session.SendMessage(msg);
                        Interface iface = new InfoRetrieveMessageEvent();
                        iface.Handle(Session, null);
                    }
                }
        }
    }
}
