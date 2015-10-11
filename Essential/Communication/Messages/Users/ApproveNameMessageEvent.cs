using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Users
{
	internal sealed class ApproveNameMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            string PetName = Event.PopFixedString();
            ServerMessage Message = new ServerMessage(Outgoing.CheckPetName); // Updated
            Message.AppendInt32(Essential.GetGame().GetCatalog().ValidPetName(PetName) ? 0 : 2);
            Message.AppendString(PetName);
			Session.SendMessage(Message);
		}
	}
}
