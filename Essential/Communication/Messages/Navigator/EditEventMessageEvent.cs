using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class EditEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event != null)
			{
				int int_ = Event.PopWiredInt32();
				string string_ = Essential.FilterString(Event.PopFixedString());
				string string_2 = Essential.FilterString(Event.PopFixedString());
				int num = Event.PopWiredInt32();
				@class.Event.Category = int_;
				@class.Event.Name = string_;
				@class.Event.Description = string_2;
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
