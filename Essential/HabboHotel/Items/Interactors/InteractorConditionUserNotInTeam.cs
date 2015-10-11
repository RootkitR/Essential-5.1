using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.HabboHotel.GameClients;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorConditionUserNotInTeam : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
            {
                string team = Session.GetHabbo().CurrentRoom.IsValidTeam(RoomItem_0.string_2) ? RoomItem_0.string_2 : "none";
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("15|" + RoomItem_0.uint_0 + "|" + team);
            }
        }
    }
}
