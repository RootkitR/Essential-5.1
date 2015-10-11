using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
	internal sealed class RoomDimmerSavePresetMessageEvent : Interface
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
						int num = Event.PopWiredInt32();
						int num2 = Event.PopWiredInt32();
						string string_ = Event.PopFixedString();
						int int_ = Event.PopWiredInt32();
						bool bool_ = false;
						if (num2 >= 2)
						{
							bool_ = true;
						}
						@class.MoodlightData.Enabled = true;
						@class.MoodlightData.CurrentPreset = num;
						@class.MoodlightData.UpdatePreset(num, string_, int_, bool_);
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
