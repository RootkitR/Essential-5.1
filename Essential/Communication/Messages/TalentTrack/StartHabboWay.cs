using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.Messages;
using Essential.HabboHotel.GameClients;
namespace Essential.Communication.Messages.TalentTrack
{
    internal sealed class StartHabboWay : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string quiz = Event.PopFixedString();
            if (!Session.GetHabbo().PassedSafetyQuiz)
                quiz = "SafetyQuiz1";
            else if (!Session.GetHabbo().PassedHabboWayQuiz)
                quiz = "HabboWay1";

                ServerMessage Quiz = new ServerMessage(Outgoing.StartQuiz);//3499
                Quiz.AppendString(quiz);
                Quiz.AppendInt32(5);
                Quiz.AppendInt32(5);
                Quiz.AppendInt32(7);
                Quiz.AppendInt32(0);
                Quiz.AppendInt32(1);
                Quiz.AppendInt32(6);
                Session.SendMessage(Quiz);
        }
    }
}
