using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Pets
{
	internal sealed class RespectPetMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && !@class.IsPublic)
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.method_48(uint_);
				if (class2 != null && class2.PetData != null && Session.GetHabbo().PetRespectPoints > 0)
				{
					class2.PetData.OnRespect();
					Session.GetHabbo().PetRespectPoints--;
					using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
					{
						class3.AddParamWithValue("userid", Session.GetHabbo().Id);
						class3.ExecuteQuery("UPDATE user_stats SET dailypetrespectpoints = dailypetrespectpoints - 1 WHERE Id = @userid LIMIT 1");
					}
				}
			}
		}
	}
}
