using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Pets;
namespace Essential.Communication.Messages.Rooms.Pets
{
	internal sealed class GetPetCommandsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			RoomUser class2 = @class.method_48(num);
			if (class2 != null && class2.PetData != null)
			{
                Pet pet = class2.PetData;
				
				Session.SendMessage(pet.SerializePetCommands());
			}
		}
	}
}
