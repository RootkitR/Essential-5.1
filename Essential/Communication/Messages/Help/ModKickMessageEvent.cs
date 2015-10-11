using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class ModKickMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint num = Event.PopWiredUInt();
				string text = Event.PopFixedString();
				string string_ = string.Concat(new object[]
				{
					"User: ",
					num,
					", Message: ",
					text
				});
				Essential.GetGame().GetClientManager().StoreCommand(Session, "ModTool - Kick User", string_);
				Essential.GetGame().GetModerationTool().method_15(Session, num, text, false);
			}
		}
	}
}
