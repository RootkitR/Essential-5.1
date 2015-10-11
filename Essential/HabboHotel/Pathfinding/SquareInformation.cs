using System;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Pathfinding
{
    internal struct SquareInformation
    {
        private int mX;
        private int mY;
        private SquarePoint[] mPos;
        private SquarePoint mTarget;
        private SquarePoint mPoint;

        public SquareInformation(int pX, int pY, SquarePoint pTarget, ModelInfo pMap, bool pUserOverride, bool CalculateDiagonal, int[,] GroupGates, Room room, double Height)
        {
            this.mX = pX;
            this.mY = pY;
            this.mTarget = pTarget;

            this.mPoint = new SquarePoint(pX, pY, pTarget.X, pTarget.Y, pMap.GetState(pX, pY), pUserOverride, false, room.method_93(pX, pY), Height, room.GetRoomUsersBySquare(pX, pY),room);
            this.mPos = new SquarePoint[8];
            if (CalculateDiagonal)
            {
                this.mPos[1] = new SquarePoint(pX - 1, pY - 1, pTarget.X, pTarget.Y, pMap.GetState(pX - 1, pY - 1), pUserOverride, false, room.method_93(pX - 1, pY - 1), Height, room.GetRoomUsersBySquare(pX - 1, pY - 1),room);
                this.mPos[3] = new SquarePoint(pX - 1, pY + 1, pTarget.X, pTarget.Y, pMap.GetState(pX - 1, pY + 1), pUserOverride, false, room.method_93(pX - 1, pY + 1), Height, room.GetRoomUsersBySquare(pX - 1, pY + 1),room);
                this.mPos[5] = new SquarePoint(pX + 1, pY + 1, pTarget.X, pTarget.Y, pMap.GetState(pX + 1, pY + 1), pUserOverride, false, room.method_93(pX + 1, pY + 1), Height, room.GetRoomUsersBySquare(pX + 1, pY + 1),room);
                this.mPos[7] = new SquarePoint(pX + 1, pY - 1, pTarget.X, pTarget.Y, pMap.GetState(pX + 1, pY - 1), pUserOverride, false, room.method_93(pX + 1, pY - 1), Height, room.GetRoomUsersBySquare(pX + 1, pY - 1),room);
            }
            this.mPos[0] = new SquarePoint(pX, pY - 1, pTarget.X, pTarget.Y, pMap.GetState(pX, pY - 1), pUserOverride, false, room.method_93(pX, pY - 1), Height, room.GetRoomUsersBySquare(pX, pY - 1),room);
            this.mPos[2] = new SquarePoint(pX - 1, pY, pTarget.X, pTarget.Y, pMap.GetState(pX - 1, pY), pUserOverride, false, room.method_93(pX - 1, pY), Height, room.GetRoomUsersBySquare(pX - 1, pY),room);
            this.mPos[4] = new SquarePoint(pX, pY + 1, pTarget.X, pTarget.Y, pMap.GetState(pX, pY + 1), pUserOverride, false, room.method_93(pX, pY + 1), Height, room.GetRoomUsersBySquare(pX, pY + 1),room);
            this.mPos[6] = new SquarePoint(pX + 1, pY, pTarget.X, pTarget.Y, pMap.GetState(pX + 1, pY), pUserOverride, false, room.method_93(pX + 1, pY), Height, room.GetRoomUsersBySquare(pX + 1, pY),room);
        }

        internal SquarePoint Pos(int val)
        {
            return this.mPos[val];
        }

        internal SquarePoint Point
        {
            get
            {
                return this.mPoint;
            }
        }
    }
}
