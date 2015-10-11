using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class PopularRoomsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetConnection() != null)
            {
                byte[] end = Essential.GetGame().GetNavigator().SerializeNavigator(Session, int.Parse(Event.PopFixedString()));
                Session.GetConnection().SendData(end);
            }
		}
	}
}
