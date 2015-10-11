using Essential.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
     class InteractorBadgeDisplay : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if(Session.GetHabbo().CurrentRoom.CheckRights(Session,true))
            {
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("19|" + RoomItem_0.uint_0 + "|" + RoomItem_0.ExtraData);
            }
        }
    }
}
