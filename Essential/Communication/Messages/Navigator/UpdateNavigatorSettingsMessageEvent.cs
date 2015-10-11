using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class UpdateNavigatorSettingsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
            RoomData @class = Essential.GetGame().GetRoomManager().method_12(num);
			if (num == 0u || (@class != null))
			{
				Session.GetHabbo().HomeRoomId = num;
				using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET home_room = '",
						num,
						"' WHERE Id = '",
						Session.GetHabbo().Id,
						"' LIMIT 1;"
					}));
				}
                ServerMessage Message = new ServerMessage(Outgoing.HomeRoom); // Updated
				Message.AppendUInt(num);
                Message.AppendUInt(0);
				Session.SendMessage(Message);
			}
		}
	}
}
