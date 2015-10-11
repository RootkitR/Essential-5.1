using Essential.HabboHotel.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.SurpriseBoxes
{
    class SurpriseBox
    {
        public uint ItemId;
        public int Probability;
        public string Image;
        public string Name;
        public SurpriseBox(uint ItemId, int probability, string image, string name)
        {
            this.ItemId = ItemId;
            this.Probability = probability;
            this.Image = image;
            this.Name = name;
        }
        public Item GetItem()
        {
            return Essential.GetGame().GetItemManager().GetItemById(ItemId);
        }
    }
}
