using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Action
{
	internal sealed class RemoveAllRightsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				foreach (uint current in @class.UsersWithRights)
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(current);
                    ServerMessage Message = new ServerMessage(Outgoing.RemovePowers); // Updated
					Message.AppendUInt(@class.Id);
					Message.AppendUInt(current);
					Session.SendMessage(Message);
				}
				using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
				{
					class3.ExecuteQuery("DELETE FROM room_rights WHERE room_id = '" + @class.Id + "'");
				}
				@class.UsersWithRights.Clear();
			}
		}
	}
}
