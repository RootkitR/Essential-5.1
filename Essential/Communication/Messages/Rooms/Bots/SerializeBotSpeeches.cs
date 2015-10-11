using Essential.HabboHotel.GameClients;
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
    class SerializeBotSpeeches : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int botId = Event.PopWiredInt32();
            int skillType = Event.PopWiredInt32();
            switch (skillType)
            {
                case 2:
                    {
                        string truncatedText = "";
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            DataTable dt;
                            DataRow speakingRow;
                            dt = dbClient.ReadDataTable("SELECT * FROM bots_speech WHERE bot_id = " + botId);
                            if (dt != null)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    truncatedText += (dr["text"] + "\n");
                                }
                                speakingRow = dbClient.ReadDataRow("SELECT * FROM user_bots WHERE id = " + botId);
                                truncatedText += ";#;";
                                truncatedText += Convert.ToString(speakingRow["automatic_chat"]);
                                truncatedText += ";#;";
                                truncatedText += Convert.ToString(speakingRow["speaking_interval"]);
                            }
                        }
                        ServerMessage message = new ServerMessage(Outgoing.SerializeSpeechList);
                        message.AppendInt32(botId);
                        message.AppendInt32(skillType);
                        message.AppendString(truncatedText);
                        Session.SendMessage(message);
                        break;
                    }
                case 5:
                    {

                        ServerMessage message = new ServerMessage(Outgoing.SerializeSpeechList);
                        message.AppendInt32(botId);
                        message.AppendInt32(skillType);
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            message.AppendString(dbClient.ReadString("SELECT name FROM user_bots WHERE id = " + botId));
                        }
                        Session.SendMessage(message);
                        break;
                    }
                default:
                    return;

            }
        }
    }
}
