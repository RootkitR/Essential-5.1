using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Rooms.Wired
{
    class SnapShotItem
    {
        public uint Id;
        public int X;
        public int Y;
        public double Z;
        public int Rotation;
        public string ExtraData;
        public SnapShotItem(uint id, int x, int y, double z, int rot, string extraData)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Rotation = rot;
            this.ExtraData = extraData;
        }
    }
}
