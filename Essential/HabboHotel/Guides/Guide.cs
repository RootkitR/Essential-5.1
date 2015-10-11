using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Guides
{
    class Guide
    {
        public uint Id;
        public bool IsInUse;
            public Guide(uint id)
            {
                this.Id = id;
                this.IsInUse = false;
            }
    }
}
