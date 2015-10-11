using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Inventory.AvatarFX
{
	internal sealed class AvatarEffectActivatedEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().GetEffectsInventoryComponent().method_3(Event.PopWiredInt32());
		}
	}
}
