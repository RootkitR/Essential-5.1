using System;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class RoomTextSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = Event.PopFixedString();
			if (Session != null && Session.GetHabbo() != null && text != Essential.smethod_0(Session.GetHabbo().Username))
			{
                Session.SendMessage(Essential.GetGame().GetNavigator().SerializeRoomSearch(text));
			}
		}
	}
}
