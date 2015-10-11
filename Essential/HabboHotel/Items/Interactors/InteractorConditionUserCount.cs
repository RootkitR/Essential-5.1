using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorConditionUserCount : FurniInteractor
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
                ServerMessage message = new ServerMessage(Outgoing.WiredCondition);
                message.AppendBoolean(false);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                    message.AppendInt32(1000000);
                else
                    message.AppendInt32(5);
                message.AppendInt32(0);
                message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
                message.AppendInt32(RoomItem_0.uint_0);
                message.AppendString("");
                message.AppendInt32(2);
                if (!string.IsNullOrEmpty(RoomItem_0.string_3))
                {
                    message.AppendInt32(int.Parse(RoomItem_0.string_3.Split(';')[0]));
                    message.AppendInt32(int.Parse(RoomItem_0.string_3.Split(';')[1]));
                }
                else
                {
                    message.AppendInt32(1);
                    message.AppendInt32(50);
                }
                message.AppendBoolean(false);
                message.AppendInt32(0);
                message.AppendInt32(1290);
                Session.SendMessage(message);
            }
        }
    }
}
