using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.Catalogs;
using Essential.Storage;
namespace Essential.Communication.Messages.Recycler
{
	internal sealed class RecycleItemsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session.GetHabbo().InRoom)
			{
				int num = Event.PopWiredInt32();
				if (num >= 3)
				{
					for (int i = 0; i < num; i++)
					{
						UserItem @class = Session.GetHabbo().GetInventoryComponent().GetItemById(Event.PopWiredUInt());
						if (@class == null || !@class.GetBaseItem().AllowRecycle)
						{
							return;
						}
                        Session.GetHabbo().GetInventoryComponent().method_12(@class.uint_0, 0u, false);
					}
					uint num2 = Essential.GetGame().GetCatalog().GetNextId();
					EcotronReward class2 = Essential.GetGame().GetCatalog().GetEcotronReward();
					using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
					{
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('",
							num2,
							"','",
							Session.GetHabbo().Id,
							"','1478','')"
						}));
                        class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
							num2,
							"','",
							DateTime.Now.ToLongDateString(),
							"')"
						}));
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
							num2,
							"','",
							class2.uint_2,
							"','1','')"
						}));
					}
					Session.GetHabbo().GetInventoryComponent().method_9(true);
                    ServerMessage Response = new ServerMessage(Outgoing.SendPurchaseAlert);
                    Response.AppendInt32(1);
                    Response.AppendInt32(1);
                    Response.AppendInt32(1);
                    Response.AppendInt32(class2.GetBaseItem().Sprite);
                    Session.SendMessage(Response);
                    ServerMessage Response2 = new ServerMessage(Outgoing.RecycleState);
                    Response2.AppendInt32(1);
                    Response2.AppendInt32(class2.GetBaseItem().Sprite);
                    Session.SendMessage(Response2);
				}
			}
		}
	}
}
