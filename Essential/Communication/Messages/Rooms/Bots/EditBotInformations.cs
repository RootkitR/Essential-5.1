using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.RoomBots;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Users.Inventory;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Rooms.Bots
{
    class EditBotInformations : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (!Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                return;
            Room room = Session.GetHabbo().CurrentRoom;
            uint id = Event.PopWiredUInt();
            int ActionType = Event.PopWiredInt32();
            RoomUser BotUser = room.getBot(id);
            /*string Username = Event.PopFixedString();

            BotUser.RoomBot.Name = Username;
            using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("username", Username);
                dbClient.ExecuteQuery("UPDATE user_bots SET name=@username WHERE id=" + id);
            }*/

            List<RandomSpeech> list = new List<RandomSpeech>();
            List<BotResponse> list2 = new List<BotResponse>();
            int currentX = BotUser.X;
            int currentY = BotUser.Y;
            int currentRot = BotUser.BodyRotation;
            double currentH = BotUser.double_0;
            UserBot bot = null;
            switch(ActionType)
            {
                case 1:
                    string Look = Session.GetHabbo().Figure;
                    BotUser.RoomBot.Look = Look;
                    room.method_6(BotUser.VirtualId, false);
                    using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.AddParamWithValue("look", Look);
                        dbClient.ExecuteQuery("UPDATE user_bots SET look=@look WHERE id=" + id);
                        bot = Essential.GetGame().GetCatalog().RetrBot(dbClient.ReadDataRow("SELECT * FROM user_bots WHERE id=" + id));
                    }
                    room.AddBotToRoom(new RoomBot(id, Session.GetHabbo().CurrentRoomId, AIType.UserBot, "freeroam", BotUser.RoomBot.Name, BotUser.RoomBot.Motto, Look, currentX, currentY, 0, currentRot, 0, 0, 0, 0, ref list, ref list2, 0), bot);
                    break;
                case 2:
                    string Data = Event.PopFixedString();
                    DataRow BotData;
                    string[] firstdata = Data.Split(';');
                    string[] toinendata = firstdata[0].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string automaticChat = firstdata[2];
                    string speakingInterval = firstdata[4];  //seconds

                    if (String.IsNullOrEmpty(speakingInterval) || Convert.ToInt32(speakingInterval) <= 0)
                        speakingInterval = "7";
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("DELETE FROM bots_speech WHERE bot_id = '" + id + "'");
                    }
                    for (int i = 0; i <= toinendata.Length - 1; i++)
                    {
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            dbClient.AddParamWithValue("data", toinendata[i]);
                            dbClient.ExecuteQuery("INSERT INTO `bots_speech` (`bot_id`, `text`) VALUES ('" + id + "', @data)");
                            dbClient.ExecuteQuery("UPDATE user_bots SET automatic_chat='" + automaticChat + "',speaking_interval=" + Convert.ToInt32(speakingInterval) + " WHERE id = " + id);
                        }
                    }

                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        BotData = dbClient.ReadDataRow("SELECT * FROM user_bots WHERE id = '" + id + "'");
                    }
                    DataTable BotSpeech;
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        BotSpeech = dbClient.ReadDataTable("SELECT text, shout, bot_id FROM bots_speech;");
                        bot = Essential.GetGame().GetCatalog().RetrBot(dbClient.ReadDataRow("SELECT * FROM user_bots WHERE id=" + id));

                    }
                    foreach (DataRow Row2 in BotSpeech.Rows)
                    {
                        list.Add(new RandomSpeech((string)Row2["text"], Essential.StringToBoolean(Row2["shout"].ToString()), Convert.ToUInt32(Row2["bot_id"])));
                    }
                    room.method_6(BotUser.VirtualId, false);
                    room.AddBotToRoom(new RoomBot((uint)BotData["id"], (uint)BotData["room_id"], AIType.UserBot, "freeroam", (string)BotData["name"], (string)BotData["motto"], (string)BotData["look"], currentX, currentY, 0, currentRot, 0, 0, 0, 0, ref list, ref list2, (int)Session.GetHabbo().Id), bot);


                    break;
                case 3:
                    //stop dancing
                    break;
                case 4:
                    if (BotUser.DanceId > 0)
                    {
                        BotUser.DanceId = 0;
                    }
                    else
                    {
                        Random rnd = new Random();
                        BotUser.DanceId = rnd.Next(1, 4);
                    }
                    ServerMessage message = new ServerMessage(Outgoing.Dance);
                    message.AppendInt32(BotUser.VirtualId);
                    message.AppendInt32(BotUser.DanceId);
                    Session.GetHabbo().CurrentRoom.SendMessage(message,null);
                    break;
                case 5:
                    string Username = Event.PopFixedString();
                    if (!Essential.IsValidName(Username))
                        break;
                    BotUser.RoomBot.Name = Username;
                    room.method_6(BotUser.VirtualId, false);
                    using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.AddParamWithValue("username", Username);
                        dbClient.ExecuteQuery("UPDATE user_bots SET name=@username WHERE id=" + id);
                        bot = Essential.GetGame().GetCatalog().RetrBot(dbClient.ReadDataRow("SELECT * FROM user_bots WHERE id=" + id));
                    }
                    room.AddBotToRoom(new RoomBot(id, Session.GetHabbo().CurrentRoomId, AIType.UserBot, "freeroam", Username, BotUser.RoomBot.Motto, BotUser.RoomBot.Look, currentX, currentY, 0, currentRot, 0, 0, 0, 0, ref list, ref list2, 0), bot);
                    break;
                default:
                    //nothing
                    break;
            }
        }
    }
}
