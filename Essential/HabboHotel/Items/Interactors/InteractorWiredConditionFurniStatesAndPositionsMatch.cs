using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorWiredConditionFurniStatesAndPositionsMatch : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem Item)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights && Session != null)
            {
                Item.CheckExtraData4();
                ServerMessage Message = new ServerMessage(Outgoing.WiredCondition); // Updated
                Message.AppendBoolean(false);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                    Message.AppendInt32(1000000);
                else
                    Message.AppendInt32(5);
                if (Item.string_2 != "")
                {
                    Message.AppendInt32(Item.string_2.Split(';').Length);
                    string ItemId = "";
                    foreach (string condstring in Item.string_2.Split(';'))
                    {
                        ItemId = condstring.Split(',')[0];
                        Message.AppendInt32(int.Parse(ItemId));
                    }
                }
                else
                {
                    Message.AppendInt32(0);
                }
                Message.AppendInt32(Item.GetBaseItem().Sprite);
                Message.AppendUInt(Item.uint_0);
                Message.AppendString("");
                Message.AppendInt32(3);
                if(Item.string_3.Length > 0)
                {
                    Message.AppendInt32(Item.string_3[0] == 'I' ? 1 : 0);
                    Message.AppendInt32(Item.string_3[1] == 'I' ? 1 : 0);
                    Message.AppendInt32(Item.string_3[2] == 'I' ? 1 : 0);
                }else{
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                }
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Session.SendMessage(Message);
            }
        }
    }
}
