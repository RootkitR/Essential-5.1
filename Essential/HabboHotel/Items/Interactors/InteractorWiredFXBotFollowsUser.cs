﻿using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorWiredFXBotFollowsUser : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (bool_0)
            {
                string botname = "";
                int timeinseconds = 30;
                try
                {
                    botname = RoomItem_0.string_2.Split(';')[0].Replace("|","");
                    timeinseconds = int.Parse(RoomItem_0.string_2.Split(';')[1]);

                }
                catch { }
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("20|" + RoomItem_0.uint_0 + "|" + botname + "|" + timeinseconds);
            }
        }
    }
}
