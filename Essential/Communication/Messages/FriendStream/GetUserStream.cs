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
    class GetUserStream : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            try
            {
                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                {
                    Session.SendMessage(Session.GetHabbo().GetStream().GetEntries(@class.ReadDataTable("SELECT friend_stream.id, friend_stream.type, friend_stream.userid, friend_stream.gender, friend_stream.look, friend_stream.time, friend_stream.data, friend_stream.data_extra FROM friend_stream WHERE friend_stream.userid=" + Event.PopWiredInt32() +" ORDER BY friend_stream.time DESC LIMIT 100"), Session));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
