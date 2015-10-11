using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Recycler
{
	internal sealed class GetRecyclerStatusMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Message = new ServerMessage(Outgoing.Recycle); // Updated
            Message.AppendInt32(1);
            Message.AppendInt32(0);
            Session.SendMessage(Message);
		}
	}
}
