using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorSuperWired : FurniInteractor
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
                ServerMessage Message = new ServerMessage(Outgoing.WiredEffect); // Updated
                Message.AppendBoolean(false);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
				Message.AppendUInt(RoomItem_0.uint_0);
				Message.AppendStringWithBreak(RoomItem_0.string_2);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(7);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
		}
	}
}
