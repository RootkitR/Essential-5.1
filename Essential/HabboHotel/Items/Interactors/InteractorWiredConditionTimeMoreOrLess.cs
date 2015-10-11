using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorWiredConditionTimeMoreOrLess : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (bool_0)
            {
                ServerMessage Message = new ServerMessage(Outgoing.WiredCondition); // Updated
                Message.AppendBoolean(false);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                    Message.AppendInt32(1000000);
                else
                    Message.AppendInt32(5);
                Message.AppendInt32(0);
                Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
                Message.AppendUInt(RoomItem_0.uint_0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Session.SendMessage(Message);
            }
        }
    }
}
