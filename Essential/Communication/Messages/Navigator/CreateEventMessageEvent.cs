using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class CreateEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event == null && @class.State == 0)
			{
				int int_ = Event.PopWiredInt32();
				string text = Essential.FilterString(Event.PopFixedString());
				string string_ = Essential.FilterString(Event.PopFixedString());
				int num = Event.PopWiredInt32();
				if (text.Length >= 1)
				{
					@class.Event = new RoomEvent(@class.Id, text, string_, int_, null);
					@class.Event.Tags = new List<string>();
					for (int i = 0; i < num; i++)
					{
						@class.Event.Tags.Add(Essential.FilterString(Event.PopFixedString()));
					}
					@class.SendMessage(@class.Event.Serialize(Session), null);
				}
			}
		}
	}
}
