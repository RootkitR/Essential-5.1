using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
    internal sealed class GetHelpToolMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage Frontpage = new ServerMessage(Outgoing.OpenHelpTool);
            Frontpage.AppendInt32(0);
            Session.SendMessage(Frontpage);
        }
    }
}
