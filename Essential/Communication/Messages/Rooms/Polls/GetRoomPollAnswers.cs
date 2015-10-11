using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Essential.Communication.Messages.Rooms.Polls
{
    class GetRoomPollAnswers : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int PollId = Event.PopWiredInt32();
            int QuestionId = Event.PopWiredInt32();
            int AnswerCount = Event.PopWiredInt32();
            string AnswerText = "";
            if (AnswerCount > 1)
            {
                for (int i = 1; i <= AnswerCount; i++)
                {
                    if (AnswerText == "")
                    {
                        AnswerText = Event.PopFixedString();
                    }
                    else
                    {
                        AnswerText = AnswerText + "," + Event.PopFixedString();
                    }
                }
            }
            else
            {
                AnswerText = Event.PopFixedString();
            }

            if (string.IsNullOrEmpty(AnswerText))
            {
                return;
            }

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("answer", AnswerText);
                dbClient.ExecuteQuery("INSERT INTO `room_poll_results` (`poll_id`, `question_id`, `answer_text`, `user_id`) VALUES ('" + PollId + "', '" + QuestionId + "', @answer, '" + Session.GetHabbo().Id + "')");
            }
        }
    }
}
