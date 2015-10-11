using Essential.HabboHotel.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Pathfinding
{
    class WalkUnderElement
    {
        public double Height;
        public double FullHeight;
        public bool Walkable;
        public uint Id;
        public RoomItem Item;
        public WalkUnderElement(double Height, double FullHeight, bool Walkable, uint Id, RoomItem itm)
        {
            this.Id = Id;
            this.Height = Height;
            this.FullHeight = FullHeight;
            this.Walkable = Walkable;
            this.Item = itm;
        }
    }
}
