using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorTent : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
            RoomItem_0.ExtraData = "0";

            for (int i = 0; i < RoomItem_0.GetRoom().RoomUsers.Length; i++)
            {
                RoomUser user = RoomItem_0.GetRoom().RoomUsers[i];
                if (user != null && !user.IsPet && !user.IsBot)
                {
                    if (user.TentID == RoomItem_0.uint_0)
                    {

                        user.TentID = 0;
                    }
                }
            }
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
            RoomItem_0.ExtraData = "0";


            for (int i = 0; i < RoomItem_0.GetRoom().RoomUsers.Length; i++)
            {
                RoomUser user = RoomItem_0.GetRoom().RoomUsers[i];
                if (user != null && !user.IsPet && !user.IsBot)
                {
                    if (user.TentID == RoomItem_0.uint_0)
                    {
                        user.TentID = 0;
                    }
                }
            }
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int int_0, bool bool_0)
        {

        }
    }
}
