using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.SoundMachine
{
	internal sealed class GetSoundMachinePlayListMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Message = new ServerMessage(Outgoing.SoundMachinePlaylist); // Updated
			Message.AppendUInt(Session.GetHabbo().CurrentRoomId);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
			Message.AppendStringWithBreak("Meh");
			Message.AppendStringWithBreak("Idk");
			Message.AppendInt32(1);
			Session.SendMessage(Message);
		}
	}
}
