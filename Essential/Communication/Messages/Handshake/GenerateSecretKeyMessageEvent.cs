using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Handshake
{
	internal sealed class GenerateSecretKeyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage SecretKeyComposer = new ServerMessage(Outgoing.SecretKeyComposer);
            SecretKeyComposer.AppendString(EssentialEnvironment.secretKey);
            Session.SendMessage(SecretKeyComposer);
		}
	}
}
