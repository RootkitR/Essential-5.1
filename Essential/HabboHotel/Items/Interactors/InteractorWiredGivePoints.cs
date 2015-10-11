using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredGivePoints : FurniInteractor
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
                ServerMessage message = new ServerMessage(Outgoing.WiredEffect); // Updated
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
                if (RoomItem_0.string_2 != "")
                    message.AppendInt32(int.Parse(RoomItem_0.string_2));
                else
                    message.AppendInt32(5);
                if (RoomItem_0.string_3 != "")
                    message.AppendInt32(int.Parse(RoomItem_0.string_3));
                else
                    message.AppendInt32(1);
                message.AppendInt32(0);
                message.AppendInt32(6);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(0);
                Session.SendMessage(message);
            }
		}
	}
}
