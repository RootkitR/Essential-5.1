using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class RoomsWithHighestScoreSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetConnection().SendData(Essential.GetGame().GetNavigator().SerializeNavigator(Session, -2));
		}
	}
}
