using System;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Items;
using System.Collections.Generic;
namespace Essential.HabboHotel.Pathfinding
{
    internal sealed class DreamPathfinder
    {
        private static SquarePoint GetClosetSqare(SquareInformation pInfo, HeightInfo Height, bool bool_0, bool UserOverride, bool[,] iHeightOverride, int[,] GroupGates)
        {
            double num = pInfo.Point.GetDistance;
            SquarePoint result = pInfo.Point;
            double state = Height.GetState(pInfo.Point.X, pInfo.Point.Y);
            for (int i = 0; i < 8; i++)
            {
                SquarePoint squarePoint = pInfo.Pos(i);
                if (squarePoint.InUse && squarePoint.CanWalk && GroupGates[squarePoint.X, squarePoint.Y] == 0 && (squarePoint.WalkUnder ||(Height.GetState(squarePoint.X, squarePoint.Y) - state) <= 2.0|| UserOverride || iHeightOverride[squarePoint.X, squarePoint.Y]))
                {
                    double getDistance = squarePoint.GetDistance;
                    if (num > getDistance)
                    {
                        num = getDistance;
                        result = squarePoint;
                    }
                }
            }
            return result;
        }

        internal static double GetDistance(int x1, int y1, int x2, int y2)
        {
            int xDiff = x2 - x1;
            int yDiff = y2 - y1;

            return Math.Sqrt((double)(xDiff * xDiff) + (double)(yDiff * yDiff));
        }

        internal static SquarePoint GetNextStep(int pUserX, int pUserY, int pUserTargetX, int pUserTargetY, byte[,] pGameMap, double[,] pHeight, double[,] double_1, double[,] double_2, int MaxX, int MaxY, bool pUserOverride, bool pDiagonal, bool[,] iHeightOverride, int[,] GroupGates, Room room, double Height)
        {
            ModelInfo pMap = new ModelInfo(MaxX, MaxY, pGameMap);
            List<RoomItem> ItemsOnSquare = room.method_93(pUserTargetX, pUserTargetY);
            SquarePoint squarePoint = new SquarePoint(pUserTargetX, pUserTargetY, pUserTargetX, pUserTargetY, pMap.GetState(pUserTargetX, pUserTargetY), pUserOverride, GroupGates[pUserTargetX, pUserTargetY] > 0, ItemsOnSquare, Height, room.GetRoomUsersBySquare(pUserTargetX, pUserTargetY),room);
            SquarePoint result;
            if (pUserX == pUserTargetX && pUserY == pUserTargetY)
            {
                result = squarePoint;
            }
            else
            {
                try
                {
                    SquareInformation pInfo = new SquareInformation(pUserX, pUserY, squarePoint, pMap, pUserOverride, pDiagonal, GroupGates,room,Height);
                    result = DreamPathfinder.GetClosetSqare(pInfo, new HeightInfo(MaxX, MaxY, pHeight, double_1, double_2), pDiagonal, pUserOverride, iHeightOverride, GroupGates);
                }
                catch
                {
                    return squarePoint;
                }
               
            }
            return result;
        }
    }
}
