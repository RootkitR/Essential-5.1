using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.HabboHotel.GameClients;
namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorConditionUserHasHandItem : FurniInteractor
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
                int x = 0;
                int.TryParse(RoomItem_0.string_2, out x);
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("9|" + RoomItem_0.uint_0 + "|" + x);
            }
        }
    }
}
