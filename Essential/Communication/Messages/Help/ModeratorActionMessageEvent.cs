using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class ModeratorActionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				Event.PopWiredInt32();
				int num = Event.PopWiredInt32();
				string text = Event.PopFixedString();
				string text2 = "";
				if (num.Equals(3))
				{
					text2 += "Room Cautioned.";
				}
				text2 = text2 + " Message: " + text;
				Essential.GetGame().GetClientManager().StoreCommand(Session, "ModTool - Room Alert", text2);
				Essential.GetGame().GetModerationTool().method_13(Session.GetHabbo().CurrentRoomId, !num.Equals(3), text);
			}
		}
	}
}
