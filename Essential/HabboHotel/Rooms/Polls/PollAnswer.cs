using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Rooms.Polls
{
    public class PollAnswer
    {
        public int ID;
        public string AnswerText;
        public PollAnswer(int ID, string answerText)
        {
            this.ID = ID;
            this.AnswerText = answerText;
        }
    }
}
