using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class LatestEventsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = int.Parse(Event.PopFixedString());
            Session.SendMessage(Essential.GetGame().GetNavigator().SerializeLatestEvents(Session, int_));
		}
	}
}
