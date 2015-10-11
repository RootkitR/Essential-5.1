using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorConditionHasPurse : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if(bool_0)
            {
                int Id = 33;
                if (RoomItem_0.GetBaseItem().InteractionType == "wf_cnd_hasnot_purse")
                    Id = 34;
                string currency = "credits";
                uint number = 1337;
                if(RoomItem_0.string_2.Length > 0 && RoomItem_0.string_2.Contains(";"))
                {    currency = RoomItem_0.string_2.Split(';')[0]; number = uint.Parse(RoomItem_0.string_2.Split(';')[1]);}
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send(Id+"|" + RoomItem_0.uint_0 + "|" + currency + "|" + number);
            }
        }
    }
}
