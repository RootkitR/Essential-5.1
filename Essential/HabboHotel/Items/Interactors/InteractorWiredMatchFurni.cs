using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
namespace Essential.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredMatchFurni : FurniInteractor
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
                ServerMessage Message = new ServerMessage(Outgoing.WiredEffect); // Updated
                Message.AppendBoolean(false);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                    Message.AppendInt32(1000000);
                else
                    Message.AppendInt32(5);
                if (Item.string_4 != "")
                {
                    Message.AppendInt32(Item.string_4.Split(',').Length);

                    foreach (string ItemId in Item.string_4.Split(','))
                    {
                        Message.AppendInt32(int.Parse(ItemId));
                    }
                }
                else
                {
                    Message.AppendInt32(0);
                }
				Message.AppendInt32(Item.GetBaseItem().Sprite);
				Message.AppendUInt(Item.uint_0);
				Message.AppendStringWithBreak("");
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
