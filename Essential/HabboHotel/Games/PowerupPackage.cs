using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Games
{
    class PowerupPackage
    {
        internal int Id;
        internal string PackageName;
        internal string PowerupType;
        internal int CostCredits;
        internal int Amount;

        public PowerupPackage(int Id, string PackageName, string PowerupType, int CostCredits, int Amount)
        {
            this.Id = Id;
            this.PackageName = PackageName;
            this.PowerupType = PowerupType;
            this.CostCredits = CostCredits;
            this.Amount = Amount;
        }
    }
}
