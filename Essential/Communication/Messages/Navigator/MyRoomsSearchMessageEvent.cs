using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class MyRoomsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null)
            {
                Session.SendMessage(Essential.GetGame().GetNavigator().GetNavigatorMessage(Session, -3));
            }
		}
	}
}
