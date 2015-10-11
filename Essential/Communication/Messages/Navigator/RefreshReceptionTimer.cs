using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Navigator
{
    internal sealed class RefreshReceptionTimer : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string ReqTimer = Event.PopFixedString();
            int RemLeng = (int.Parse(ReqTimer) - (int)Essential.GetUnixTimestamp());
            ServerMessage ReceptionTimer = new ServerMessage(Outgoing.ReceptionTimer);
            ReceptionTimer.AppendString(ReqTimer);
            ReceptionTimer.AppendInt32((RemLeng > 0 ? RemLeng : 0));
            Session.SendMessage(ReceptionTimer);
        }
    }
}
