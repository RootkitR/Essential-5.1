using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Essential.HabboHotel.Rooms.Polls
{
    internal class PollManager
    {
        public Room thatRoom = null;
        public Poll currentPoll;

        public PollManager(Room targetRoom)
        {
            this.thatRoom = targetRoom;
        }
        public Poll GetCurrentPoll()
        {
            return this.currentPoll;
        }
        public Poll CreateNewRoomPoll(DataRow Question, DataTable Answers)
        {
            List<PollAnswer> Pollanswers = new List<PollAnswer>();
            foreach (DataRow Answer in Answers.Rows)
            {
                Pollanswers.Add(new PollAnswer(Convert.ToInt32(Answer["id"].ToString()), Answer["answer_text"].ToString()));
            }
            return new Poll(Question["question"].ToString(), Pollanswers, this.thatRoom);
        }
        public void SetCurrentPoll(Poll poll)
        {
            this.currentPoll = poll;
        }
    }
}
