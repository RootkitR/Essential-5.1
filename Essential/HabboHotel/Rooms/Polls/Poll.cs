using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
using System.Threading;
namespace Essential.HabboHotel.Rooms.Polls
{
    internal class Poll
    {
        string Question;
        List<PollAnswer> Answers = new List<PollAnswer>();
        List<int> Votes = new List<int>();
        Room PollRoom;
        public Poll(string Question, List<PollAnswer> Answers, Room PollRoom)
        {
            this.Question = Question;
            this.Answers = Answers;
            this.Votes = new List<int>();
            this.PollRoom = PollRoom;
        }
        public void AddVote(int VoteID)
        {
            Votes.Add(VoteID);
        }
        public ServerMessage PollToServerMessage(ServerMessage Message)
        {
            Message.AppendStringWithBreak(Question);
            Message.AppendInt32(Answers.Count);
            foreach (PollAnswer Answer in Answers)
            {
                Message.AppendInt32(Answer.ID);
                Message.AppendStringWithBreak(Answer.AnswerText);
            }
            return Message;
        }
        int AnswerCount;
        public void ShowResults()
        {
            Thread.Sleep(30000);
            ServerMessage InfobusQuestion = new ServerMessage(Outgoing.InfobusPoll2); 
            InfobusQuestion.AppendStringWithBreak(Question);
            InfobusQuestion.AppendInt32(Answers.Count);
            foreach (PollAnswer Answer in Answers)
            {
                InfobusQuestion.AppendInt32(Answer.ID);
                InfobusQuestion.AppendStringWithBreak(Answer.AnswerText);

                foreach (int AnswerID in Votes)
                {
                    if (AnswerID == Answer.ID)
                    {
                        AnswerCount++;
                    }
                }
                InfobusQuestion.AppendInt32(AnswerCount);
                AnswerCount = 0;
            }
            InfobusQuestion.AppendInt32(Votes.Count);
            this.PollRoom.SendMessage(InfobusQuestion, null);
        }
    }
}
