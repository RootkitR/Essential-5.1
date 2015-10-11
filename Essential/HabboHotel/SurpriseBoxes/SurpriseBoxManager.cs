using Essential.HabboHotel.Items;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.SurpriseBoxes
{
    class SurpriseBoxManager
    {
        public List<SurpriseBox> surpriseboxes = new List<SurpriseBox>();
        public SurpriseBoxManager(DatabaseClient dbClient)
        {
                foreach(DataRow itm in dbClient.ReadDataTable("SELECT * FROM surpriseboxes").Rows)
                {
                    surpriseboxes.Add(new SurpriseBox(Convert.ToUInt32((int)itm["itemid"]), (int)itm["probability"], (string)itm["image"],(string)itm["name"]));
                }
            surpriseboxes = surpriseboxes.OrderByDescending(s => s.Probability).ToList();
        }
        public SurpriseBox GetRandomSurpriseBox()
        {
            foreach(SurpriseBox surpriseBox in surpriseboxes)
            {
                if(Essential.GetRandomNumber(1,surpriseBox.Probability) == surpriseBox.Probability)
                {
                    return surpriseBox;
                }
            }
            return new SurpriseBox(9943, 1, "","");
            //Essential.GetGame().GetItemManager().GetItemById(9943); // gibt ein Standartitem zurück..
        }
    }
}
