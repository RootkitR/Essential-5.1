using Essential.HabboHotel.Users;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Games.Snowstorm
{
    class RequestFullGameStatus : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            ServerMessage Message5_0 = new ServerMessage(Outgoing.Game2FullGameStatusMessageEvent);//Game2FullGameStatusMessageEvent
            Message5_0.AppendInt32(1);
            Message5_0.AppendInt32(1);
            Message5_0.AppendInt32(1);
            //SOMETHING
            Message5_0.AppendInt32(1);
            Message5_0.AppendInt32(1);
            Message5_0.AppendInt32(1);
            Message5_0.AppendInt32(1);
            Message5_0.AppendInt32(3);
            Habbo habbo = Session.GetHabbo();
            Message5_0.AppendInt32(habbo.SnowUserId);
            Message5_0.AppendInt32(habbo.SnowX);
            Message5_0.AppendInt32(habbo.SnowY);
            Message5_0.AppendInt32((int)(habbo.SnowX / 0xc80));
            Message5_0.AppendInt32((int)(habbo.SnowY / 0xc80));
            Message5_0.AppendInt32(habbo.SnowRot);
            Message5_0.AppendInt32(0);
            Message5_0.AppendInt32(0);
            //SOMETHING2
            Message5_0.AppendInt32(0);
            Message5_0.AppendInt32(0);
            Message5_0.AppendInt32(0);
            Session.GetHabbo().SnowWar.SendToStorm(Message5_0);
        }
    }
}
