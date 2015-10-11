using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Essential.Communication.Messages.FriendStream
{
    class UpdateStream : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            
            string message = Event.PopFixedString();
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                DataRow dr = dbClient.ReadDataRow("SELECT * FROM friend_stream WHERE userid='" + Session.GetHabbo().Id.ToString() + "' ORDER BY time DESC LIMIT 1");
                double currentTime = Essential.GetUnixTimestamp();
                if (dr == null)
                {

                    string Id = Session.GetHabbo().Id.ToString();
                    string Gender = Session.GetHabbo().Gender.ToString();
                    string look = Session.GetHabbo().Figure.ToString();
                    dbClient.AddParamWithValue("@endstring", message);
                    dbClient.ExecuteQuery("INSERT INTO friend_stream (`type`,`userid`,`gender`,`look`,`time`,`data`,`data_extra`) VALUES ('1','" + Id + "','" + Gender + "','" + look + "','" + currentTime + "', @endstring, '0');");
                }
                else
                {
                    double lastTime = (double)dr["time"];
                    double seconds = currentTime - lastTime;
                    if (seconds > 300)
                    {
                        string Id = Session.GetHabbo().Id.ToString();
                        string Gender = Session.GetHabbo().Gender.ToString();
                        string look = Session.GetHabbo().Figure.ToString();
                        dbClient.AddParamWithValue("@endstring", message);
                        dbClient.ExecuteQuery("INSERT INTO friend_stream (`type`,`userid`,`gender`,`look`,`time`,`data`,`data_extra`) VALUES ('1','" + Id + "','" + Gender + "','" + look + "','" + currentTime + "', @endstring, '0');");
                    }
                    else
                    {
                        Session.SendNotification("Du kannst nur alle 15 Minuten einen Friendstream Eintrag erstellen! Versuchs später nochmal.");
                    }
                }
            }
        }
    }
}
