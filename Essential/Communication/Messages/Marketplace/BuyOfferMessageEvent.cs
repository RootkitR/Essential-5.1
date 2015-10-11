using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.Storage;
namespace Essential.Communication.Messages.Marketplace
{
    internal sealed class BuyOfferMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            uint num = Event.PopWiredUInt();
            DataRow dataRow = null;
            using (DatabaseClient @class = Essential.GetDatabase().GetClient())
            {

                dataRow = @class.ReadDataRow("SELECT * FROM catalog_items WHERE id = '" + num + "' LIMIT 1");
                Console.Write(num.ToString());
                if (dataRow != null)
                {
                    if (Essential.GetGame().GetCatalog().GetPage(int.Parse(dataRow["page_id"].ToString())).MinRank == 1)
                    {
                        if (Session.GetHabbo().GetCredits() >= (int)dataRow["cost_credits"] && Session.GetHabbo().ActivityPoints >= (int)dataRow["cost_pixels"] && Session.GetHabbo().VipPoints >= (int)dataRow["cost_snow"])
                        {
                            string base_item = dataRow["item_ids"].ToString();
                            Session.GetHabbo().TakeCredits((int)dataRow["cost_credits"], "Bought Item #" + num);
                            Session.GetHabbo().ActivityPoints = Session.GetHabbo().ActivityPoints - (int)dataRow["cost_pixels"];
                            Session.GetHabbo().VipPoints = Session.GetHabbo().VipPoints - (int)dataRow["cost_snow"];
                            Session.GetHabbo().UpdateCredits(true);
                            Session.GetHabbo().UpdateActivityPoints(true);
                            Session.GetHabbo().UpdateVipPoints(false, true);
                            uint num1 = Essential.GetGame().GetCatalog().GetNextId();
                            Session.GetHabbo().GetInventoryComponent().method_11(num1, uint.Parse(base_item), "", true, 0, 0, "");
                            Session.GetHabbo().GetInventoryComponent().method_9(true);
                        }
                        else
                        {
                            string s = "";
                            s = s + (Session.GetHabbo().GetCredits() >= (int)dataRow["cost_credits"] ? "Taler" : "");
                            s = s + (Session.GetHabbo().ActivityPoints >= (int)dataRow["cost_pixels"] ? ", Pixel" : "");
                            s = s + (Session.GetHabbo().VipPoints >= (int)dataRow["cost_snow"] ? " & Vip Punkte." : "");

                            Session.SendNotification("Du hast zu wenig " + s);
                        }
                    }
                }
            }
        }
    }
}
