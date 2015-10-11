using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class ModerateRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
                bool flag = (Event.PopWiredInt32() == 1);
                bool flag2 = (Event.PopWiredInt32() == 1);
                bool flag3 = (Event.PopWiredInt32() == 1);
				string text = "";
				if (flag)
				{
					text += "Apply Doorbell";
				}
				if (flag2)
				{
					text += " Change Name";
				}
				if (flag3)
				{
					text += " Kick Users";
				}
				Essential.GetGame().GetClientManager().StoreCommand(Session, "ModTool - Room Action", text);
		        Essential.GetGame().GetModerationTool().method_12(Session, uint_, flag3, flag, flag2);
			}
		}
	}
}
