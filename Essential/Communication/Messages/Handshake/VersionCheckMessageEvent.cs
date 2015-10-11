using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Handshake
{
	internal sealed class VersionCheckMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            //Console.WriteLine("Trying connect with " + Event.PopFixedString());
		}
	}
}
