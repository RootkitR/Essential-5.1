using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Games.SnowWar;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Games.Snowstorm
{
    class TalkGame : Interface
    {

        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (((Session != null) || (Session.GetHabbo() != null)) && (Session.GetHabbo().SnowWar != null))
            {
                SnowStorm snowWar = Session.GetHabbo().SnowWar;
                string s = Event.PopFixedString();
                ServerMessage talk = new ServerMessage(Outgoing.Game2GameChatFromPlayerMessageEvent);
                talk.AppendInt32(Session.GetHabbo().Id);
                talk.AppendString(s);
                snowWar.SendToStorm(talk, false, 0);
            }
        }
    }
}
