using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System.Collections.Generic;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.SoundMachine;

namespace Essential.Communication.Messages.Inventory.Furni
{
	internal sealed class RequestFurniInventoryEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
                Session.GetHabbo().GetInventoryComponent().method_13();
			}
		}
	}
}
