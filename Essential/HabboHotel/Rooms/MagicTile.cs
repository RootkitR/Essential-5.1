using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Rooms
{
    class MagicTile
    {
        internal uint RoomID;
        internal int X;
        internal int Y;
        internal string Action;
        internal int NextRoomId;
        internal int NextX;
        internal int NextY;
        internal int NextZ;
        internal int NextRot;
        public MagicTile(uint mRoomID, int mX, int mY, string mAction, int mNextRoomId, int mNextX, int mNextY, int mNextZ, int mNextRot)
        {
            this.RoomID = mRoomID;
            this.X = mX;
            this.Y = mY;
            this.Action = mAction;
            this.NextRoomId = mNextRoomId;
            this.NextX = mNextY;
            this.NextY = mNextY;
            this.NextZ = mNextZ;
            this.NextRot = mNextRot;
        }
    }
}
