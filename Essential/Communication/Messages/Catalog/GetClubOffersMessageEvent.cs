using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class GetClubOffersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
		}
	}
}
