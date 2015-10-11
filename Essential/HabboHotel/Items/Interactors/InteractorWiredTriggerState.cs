using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredTriggerState : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (bool_0 && Session != null)
			{
				RoomItem_0.CheckExtraData3();
				ServerMessage Message = new ServerMessage(Outgoing.WiredEffect); // Update
                Message.AppendBoolean(false);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                {
                    Message.AppendInt32(1000000);
                }
                else
                {
                    Message.AppendInt32(5);
                }
                if (RoomItem_0.string_3 != "")
                {
                    Message.AppendInt32(RoomItem_0.string_3.Split(',').Length);
                    foreach (string ItemId in RoomItem_0.string_3.Split(','))
                    {
                        Message.AppendInt32(int.Parse(ItemId));
                    }
                }
                else
                {
                    Message.AppendInt32(0);
                }
				Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
				Message.AppendUInt(RoomItem_0.uint_0);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(0);
				Message.AppendInt32(8);
				Message.AppendInt32(0);
                double value = 0;
                if (RoomItem_0.string_2 != "" && double.TryParse(RoomItem_0.string_2, out value))
                    Message.AppendInt32(Convert.ToInt32(value * 2));
                else
                    Message.AppendInt32(0);
				Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
		}
	}
}
