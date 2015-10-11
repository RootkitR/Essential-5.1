using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class GetPopularRoomTagsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Session.SendMessage(Essential.GetGame().GetNavigator().SerializePopularRoomTags());
		}
	}
}
