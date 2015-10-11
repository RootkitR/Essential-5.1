using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Mobile
{
    class MobileHandler
    {
        private Dictionary<uint, MobileInterface> MobileHandlers;
        public MobileHandler()
        {
            this.MobileHandlers = new Dictionary<uint, MobileInterface>();
        }
        public void Load()
        {
            #region "Authentication"
            
            #endregion
        }
        public void AddHandler(uint id, MobileInterface interface_)
        {
            this.MobileHandlers.Add(id, interface_);
        }
    }
}
