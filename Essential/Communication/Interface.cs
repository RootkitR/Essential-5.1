using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential
{
	internal interface Interface
	{
		void Handle(GameClient Session, ClientMessage Event);
	}
}
