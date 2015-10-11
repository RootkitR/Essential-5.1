using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Essential.Communication.Messages.Rooms.Polls
{
    class AnswerInfobusPoll : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {

            Room raum = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            int AnswerId = Event.PopWiredInt32();
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                raum.GetPollManager().GetCurrentPoll().AddVote(AnswerId);
            }
            /*using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("INSERT INTO `infobus_results` (`question_id`, `answer_id`) VALUES ('" + QuestionId + "', '" + AnswerId + "')");
            }*/
        }
    }
}
