using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
using System;
namespace Essential.Communication.Messages.Register
{
    internal sealed class UpdateFigureDataMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string text = Event.PopFixedString().ToUpper();
            if (!(text == "M" || text == "F"))
                return;

            string text2 = Essential.FilterString(Event.PopFixedString());
            if (AntiMutant.ValidateLook(text2, text))
            {
                if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "CHANGE_FIGURE")
                {
                    Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                }
                Session.GetHabbo().Figure = text2;
                Session.GetHabbo().Gender = text.ToLower();
                using (DatabaseClient client = Essential.GetDatabase().GetClient())
                {
                    client.AddParamWithValue("look", text2);
                    client.AddParamWithValue("gender", text);
                    client.ExecuteQuery("UPDATE users SET look = @look, gender = @gender WHERE id = " + Session.GetHabbo().Id + " LIMIT 1;");
                }
                Essential.GetGame().GetAchievementManager().addAchievement(Session, 1u, 1);
                ServerMessage serverMessage = new ServerMessage(Outgoing.UpdateUserInformation);
                serverMessage.AppendInt32(-1);
                serverMessage.AppendStringWithBreak(Session.GetHabbo().Figure);
                serverMessage.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
                serverMessage.AppendStringWithBreak(Session.GetHabbo().Motto);
                serverMessage.AppendInt32(Session.GetHabbo().AchievementScore);
               // serverMessage.AppendStringWithBreak("");
                Session.SendMessage(serverMessage);
                if (Session.GetHabbo().InRoom)
                {
                    Room currentRoom = Session.GetHabbo().CurrentRoom;
                    if (currentRoom != null)
                    {
                        RoomUser roomUserByHabbo = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                        if (roomUserByHabbo != null)
                        {
                            roomUserByHabbo.string_0 = "";
                            if (Session.GetHabbo().method_4() > 0)
                            {
                                TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                                if (timeSpan.Seconds > 4)
                                {
                                    Session.GetHabbo().int_23 = 0;
                                }
                                if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5)
                                {
                                    ServerMessage serverMessage2 = new ServerMessage(Outgoing.FigureData);
                                    serverMessage2.AppendInt32(Session.GetHabbo().method_4());
                                    Session.SendMessage(serverMessage2);
                                    return;
                                }
                                Session.GetHabbo().dateTime_0 = DateTime.Now;
                                Session.GetHabbo().int_23++;
                            }
                            ServerMessage serverMessage3 = new ServerMessage(Outgoing.UpdateUserInformation);
                            serverMessage3.AppendInt32(roomUserByHabbo.VirtualId);
                            serverMessage3.AppendStringWithBreak(Session.GetHabbo().Figure);
                            serverMessage3.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
                            serverMessage3.AppendStringWithBreak(Session.GetHabbo().Motto);
                            serverMessage3.AppendInt32(Session.GetHabbo().AchievementScore);
                         //   serverMessage3.AppendStringWithBreak("");
                            currentRoom.SendMessage(serverMessage3, null);
                        }
                    }
                }
            }
        }
    }
}
