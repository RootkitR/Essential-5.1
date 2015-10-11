using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Messenger
{
	internal sealed class RemoveBuddyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					Session.GetHabbo().GetMessenger().method_13(Event.PopWiredUInt());
				}
			}
		}
	}
}
