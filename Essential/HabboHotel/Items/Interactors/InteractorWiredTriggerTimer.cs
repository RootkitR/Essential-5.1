using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredTriggerTimer : FurniInteractor
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
                ServerMessage Message = new ServerMessage(Outgoing.WiredFurniTrigger);
                Message.AppendBoolean(false);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                    Message.AppendInt32(1000000);
                else
                    Message.AppendInt32(5);
				Message.AppendInt32(0);
				Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
				Message.AppendUInt(RoomItem_0.uint_0);
				Message.AppendStringWithBreak(RoomItem_0.string_1);
                Message.AppendInt32(1);
                Message.AppendInt32(1);
                Message.AppendInt32(1);
                Message.AppendInt32(3);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
				Session.SendMessage(Message);
				RoomItem_0.ReqUpdate(1);
			}
		}
	}
}
