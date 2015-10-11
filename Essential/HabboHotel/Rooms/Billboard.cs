using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Rooms
{
    class Billboard
    {
        internal uint RoomID;
        internal string Image;
        internal string URL;

        public Billboard(uint mRoomID, string mImage, string mURL)
        {
            this.RoomID = mRoomID;
            this.Image = mImage;
            this.URL = mURL;
        }
    }
}
