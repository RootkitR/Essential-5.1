using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class CallForHelpMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (!Essential.GetGame().GetModerationTool().method_9(Session.GetHabbo().Id))
			{
				string string_ = Essential.FilterString(Event.PopFixedString());
				Event.PopWiredInt32();
				int int_ = Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				Essential.GetGame().GetModerationTool().method_3(Session, int_, uint_, string_);
			}
            ServerMessage Message = new ServerMessage(Outgoing.HelpRequestAlert); // 321 old UPDATED
            Message.AppendInt32(0);
			Session.SendMessage(Message);
		}
	}
}
