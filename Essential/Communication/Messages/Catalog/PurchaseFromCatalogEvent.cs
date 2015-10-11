using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class PurchaseFromCatalogEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            try
            {
                int int_ = Event.PopWiredInt32();
                uint uint_ = Event.PopWiredUInt();
                string string_ = Event.PopFixedString();
                int Amount = Event.PopWiredInt32();
                //Console.WriteLine(int_ + "    " + uint_ + "     " + string_ + "     " + Amount);
                if (Session == null || Session.GetHabbo() == null)
                    return;
                if (Amount < 0 || Amount > 100) //BIG EXPLOIT..
                    return;
                Essential.GetGame().GetCatalog().HandlePurchase(Session, int_, uint_, string_, false, "", "", true, Amount);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
		}
	}
}
