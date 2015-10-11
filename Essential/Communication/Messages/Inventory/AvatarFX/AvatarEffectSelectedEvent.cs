using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Inventory.AvatarFX
{
	internal sealed class AvatarEffectSelectedEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().GetEffectsInventoryComponent().method_2(Event.PopWiredInt32(), false);
		}
	}
}
