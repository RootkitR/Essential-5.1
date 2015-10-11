using System;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Session
{
	internal sealed class OpenFlatConnectionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string string_ = Event.PopFixedString();
			Event.PopWiredInt32();
            Session.GetHabbo().uint_2 = num;
            if (Session.GetClientMessageHandler() == null)
                Session.CreateClientMessageHandler();
            Session.GetClientMessageHandler().method_5(num, string_);
		}
	}
}
