using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
	internal sealed class RoomDimmerChangeStateMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true) && @class.MoodlightData != null)
				{
					RoomItem class2 = null;
					foreach (RoomItem class3 in @class.Hashtable_1.Values)
					{
						if (class3.GetBaseItem().InteractionType.ToLower() == "dimmer")
						{
							class2 = class3;
							break;
						}
					}
					if (class2 != null)
					{
						if (@class.MoodlightData.Enabled)
						{
							@class.MoodlightData.Deactivate();
						}
						else
						{
							@class.MoodlightData.Activate();
						}
						class2.ExtraData = @class.MoodlightData.GenerateExtraData();
						class2.UpdateState();
					}
				}
			}
			catch
			{
			}
		}
	}
}
