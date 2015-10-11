using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Sound
{
	internal sealed class GetSoundSettingsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
         /*   ServerMessage Message = new ServerMessage(Outgoing.SoundSettings); // Updated
			Message.AppendInt32(Session.GetHabbo().Volume);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);*/
		}
	}
}
