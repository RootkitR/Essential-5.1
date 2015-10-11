using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
	internal sealed class PresentOpenMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true))
				{
					RoomItem class2 = @class.method_28(Event.PopWiredUInt());
					if (class2 != null)
					{
						DataRow dataRow = null;
						using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
						{
							dataRow = class3.ReadDataRow("SELECT base_id,amount,extra_data FROM user_presents WHERE item_id = '" + class2.uint_0 + "' LIMIT 1");
						}
						if (dataRow != null)
						{
							Item class4 = Essential.GetGame().GetItemManager().GetItemById((uint)dataRow["base_id"]);
							if (class4 != null)
							{
								@class.method_29(Session, class2.uint_0, true, true);
                                ServerMessage Message = new ServerMessage(Outgoing.UpdateInventary);
								Message.AppendUInt(class2.uint_0);
								Session.SendMessage(Message);
                                /*ServerMessage Message2 = new ServerMessage(Outgoing.Item2);
								Message2.AppendStringWithBreak(class4.Type.ToString());
								Message2.AppendInt32(class4.Sprite);
								Message2.AppendStringWithBreak(class4.Name);
                                Message2.AppendInt32(class2.uint_0);
                                Message2.AppendString(class2.GetBaseItem().Type.ToString());
                                Message2.AppendBoolean(true);
                                Message2.AppendString(class2.ExtraData);
								Session.SendMessage(Message2);*/
                                ServerMessage Message3 = new ServerMessage(Outgoing.OpenGift);
                                Message3.AppendString(class4.Type.ToString());
                                Message3.AppendInt32(class4.Sprite);
                                Message3.AppendString(class4.Name);
                                Message3.AppendInt32(class2.uint_0);
                                Message3.AppendString(class2.GetBaseItem().Type.ToString());
                                Message3.AppendBoolean(true);
                                Message3.AppendString(class2.ExtraData);
                                Session.SendMessage(Message3);
								using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
								{
									class3.ExecuteQuery("DELETE FROM user_presents WHERE item_id = '" + class2.uint_0 + "' LIMIT 1");
								}
								Essential.GetGame().GetCatalog().AddItemToInventory(Session, class4, (int)dataRow["amount"], (string)dataRow["extra_data"], true, 0u, 0, 0, "");
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
