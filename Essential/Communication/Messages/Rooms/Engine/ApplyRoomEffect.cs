using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class ApplyRoomEffect : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItemById(Event.PopWiredUInt());
				if (class2 != null)
				{
					string text = "floor";
					if (class2.GetBaseItem().Name.ToLower().Contains("wallpaper"))
					{
						text = "wallpaper";
					}
					else
					{
						if (class2.GetBaseItem().Name.ToLower().Contains("landscape"))
						{
							text = "landscape";
						}
					}
					string text2 = text;
					if (text2 != null)
					{
						if (!(text2 == "floor"))
						{
							if (!(text2 == "wallpaper"))
							{
								if (text2 == "landscape")
								{
									@class.Landscape = class2.string_0;
								}
							}
							else
							{
								@class.Wallpaper = class2.string_0;
                                if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "PLACEWALLPAPER")
								{
                                    Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
								}
							}
						}
						else
						{
							@class.Floor = class2.string_0;
                            if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "PLACEFLOOR")
							{
                                Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
							}
						}
					}
					using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
					{
						class3.AddParamWithValue("extradata", class2.string_0);
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE rooms SET ",
							text,
							" = @extradata WHERE Id = '",
							@class.Id,
							"' LIMIT 1"
						}));
					}
                    Session.GetHabbo().GetInventoryComponent().method_12(class2.uint_0, 0u, false);
                    ServerMessage Message = new ServerMessage(Outgoing.RoomDecoration);// Updated
					Message.AppendStringWithBreak(text);
					Message.AppendStringWithBreak(class2.string_0);
					@class.SendMessage(Message, null);
					Essential.GetGame().GetRoomManager().method_18(@class.Id);
				}
			}
		}
	}
}
