using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using System.Data;
using Essential.Storage;
namespace Essential.Communication.Messages.FriendStream
{
    internal sealed class GetEventStreamComposer : Interface
	{
        public void Handle(GameClient Session, ClientMessage Message)
        {
           
            try
            {
                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                {
                    Session.SendMessage(Session.GetHabbo().GetStream().GetEntries(@class.ReadDataTable("SELECT friend_stream.id, friend_stream.type, friend_stream.userid, friend_stream.gender, friend_stream.look, friend_stream.time, friend_stream.data, friend_stream.data_extra FROM friend_stream JOIN messenger_friendships ON friend_stream.userid = messenger_friendships.user_two_id OR friend_stream.userid = messenger_friendships.user_one_id OR friend_stream.userid=1 WHERE messenger_friendships.user_one_id = '" + Session.GetHabbo().Id + "' ORDER BY friend_stream.time DESC LIMIT 100"), Session));

                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
	}
}
