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
    class LeaveGame : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (((Session != null) || (Session.GetHabbo() != null)) && ((Session.GetHabbo().SnowWar != null) && Session.GetHabbo().SnowWar.WarUsers.Contains(Session.GetHabbo())))
            {
                SnowStorm snowWar = Session.GetHabbo().SnowWar;
                ServerMessage packet = new ServerMessage(Outgoing.Game2PlayerExitedGameArenaMessageEvent);
                packet.AppendInt32(Session.GetHabbo().Id);
                Session.GetHabbo().SnowWar.SendToStorm(packet, true, Session.GetHabbo().Id);
                snowWar.WarUsers.Remove(Session.GetHabbo());
                if ((snowWar.WarOwner == Session.GetHabbo()) && (snowWar.WarUsers.Count > 0))
                {
                    snowWar.WarOwner = snowWar.WarUsers[0];
                }
                Session.GetHabbo().SnowWar = null;
            }
        }
    }
}
