using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorFreezeIceBlock : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, true);
        }

        public override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, true);
        }

        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            //TODO
        }
    }
}
