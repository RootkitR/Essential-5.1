using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Inventory.Furni
{
	internal sealed class GetPetInventoryEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetInventoryComponent().ComposePetInventoryListMessage());
			}
		}
	}
}
