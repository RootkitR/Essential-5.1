using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Pets
{
	internal sealed class GetPetInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && !@class.IsPublic)
			{
				RoomUser class2 = @class.method_48(num);
				if (class2 == null || class2.PetData == null)
				{
					DataRow dataRow = null;
					using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
					{
						class3.AddParamWithValue("petid", num);
						dataRow = class3.ReadDataRow("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE Id = @petid LIMIT 1");
					}
					if (dataRow != null)
					{
						Session.SendMessage(Essential.GetGame().GetCatalog().GetPet(dataRow).SerializeInfo());
					}
				}
				else
				{
					Session.SendMessage(class2.PetData.SerializeInfo());
				}
			}
		}
	}
}
