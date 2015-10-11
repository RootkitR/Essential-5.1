using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Messenger
{
	internal sealed class DeclineBuddyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				bool AllRequestDecline = Event.PopWiredBoolean();
				int num2 = Event.PopWiredInt32();
                if (AllRequestDecline == false && num2 == 1)
				{
					uint uint_ = Event.PopWiredUInt();
					Session.GetHabbo().GetMessenger().method_11(uint_);
				}
				else
				{
                    if (AllRequestDecline == true)
					{
						Session.GetHabbo().GetMessenger().method_10();
					}
				}
			}
		}
	}
}
