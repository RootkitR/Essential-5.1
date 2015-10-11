using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Inventory.Purse
{
	internal sealed class GetCreditsInfoEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Session.GetHabbo().UpdateCredits(false);
			Session.GetHabbo().UpdateActivityPoints(false);
        //    Session.GetHabbo().UpdateVipPoints(false,false);
		}
	}
}
