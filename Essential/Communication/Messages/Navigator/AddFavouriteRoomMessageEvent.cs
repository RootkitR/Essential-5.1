using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class AddFavouriteRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
            RoomData @class = Essential.GetGame().GetRoomManager().method_12(num);
			if (@class == null || Session.GetHabbo().list_1.Count >= 30 || Session.GetHabbo().list_1.Contains(num) || @class.Type == "public")
			{
			}
			else
			{
                ServerMessage Message2 = new ServerMessage(Outgoing.FavsUpdate); // Updated
				Message2.AppendUInt(num);
				Message2.AppendBoolean(true);
				Session.SendMessage(Message2);
				Session.GetHabbo().list_1.Add(num);
				using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_favorites (user_id,room_id) VALUES ('",
						Session.GetHabbo().Id,
						"','",
						num,
						"')"
					}));
				}
			}
		}
	}
}
