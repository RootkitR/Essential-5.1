using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class DeleteFavouriteRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Session.GetHabbo().list_1.Remove(num);
            ServerMessage Message = new ServerMessage(Outgoing.FavsUpdate);
			Message.AppendUInt(num);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.ExecuteQuery(string.Concat(new object[]
				{
					"DELETE FROM user_favorites WHERE user_id = '",
					Session.GetHabbo().Id,
					"' AND room_id = '",
					num,
					"' LIMIT 1"
				}));
			}
		}
	}
}
