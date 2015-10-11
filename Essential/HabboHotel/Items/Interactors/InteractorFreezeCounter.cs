using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Drawing;

using Essential.Storage;
using Essential.Messages;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pathfinding;
using Essential.HabboHotel.Rooms;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorFreezeCounter : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
        }

        public override void OnRemove(GameClient Session, RoomItem Item)
        {

        }

        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
        }
    }
}