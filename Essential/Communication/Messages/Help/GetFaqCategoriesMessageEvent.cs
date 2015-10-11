using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Help
{
	internal sealed class GetFaqCategoriesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			//Session.SendMessage(Essential.GetGame().GetHelpTool().method_8());
		}
	}
}
