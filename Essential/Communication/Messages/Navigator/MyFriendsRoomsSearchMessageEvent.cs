using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class MyFriendsRoomsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Session.SendMessage(Essential.GetGame().GetNavigator().GetNavigatorMessage(Session, -4));
		}
	}
}
