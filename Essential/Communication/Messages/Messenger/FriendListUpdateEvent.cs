using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Messenger
{
	internal sealed class FriendsListUpdateEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().GetMessenger() != null)
			{
				Session.GetHabbo().GetMessenger().SerializeUpdates(Session);
			}
		}
	}
}
