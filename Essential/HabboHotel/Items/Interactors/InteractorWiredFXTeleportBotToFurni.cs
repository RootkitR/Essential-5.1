using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorWiredFXTeleportBotToFurni : FurniInteractor
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
                ServerMessage Message = new ServerMessage(Outgoing.WiredEffect);
                Message.AppendBoolean(true);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                    Message.AppendInt32(1000000);
                else
                    Message.AppendInt32(5);
                if (RoomItem_0.string_2 != "")
                {
                    Message.AppendInt32(RoomItem_0.string_2.Split(';').Length);
                    string ItemId = "";
                    foreach (string condstring in RoomItem_0.string_2.Split(';'))
                    {
                        ItemId = condstring.Split(',')[0];
                        Message.AppendInt32(int.Parse(ItemId));
                    }
                }
                else
                {
                    Message.AppendInt32(0);
                }
                Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
                Message.AppendUInt(RoomItem_0.uint_0);
                Message.AppendString(RoomItem_0.string_2);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(11);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Session.SendMessage(Message);
            }
        }
    }
}
