using System;
using System.Collections.Generic;
using Essential.HabboHotel.Items;
using System.Linq;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Pathfinding
{
    internal struct SquarePoint
    {
        private int mX;
        private int mY;
        private double mDistance;
        private byte mSquareData;
        private bool mOverride;
        private bool mInUse;
        private bool mLastStep;
        private bool mIsGroupGate;
        private List<RoomItem> itemsonSquare;
        private List<WalkUnderElement> walkunderElements;
        private double mHeight;
        private Room currentRoom;
        public SquarePoint(int pX, int pY, int pTargetX, int pTargetY, byte SquareData, bool pOverride, bool pIsGroupGate, List<RoomItem> itemsOnSq, double Height, List<RoomUser> usersonSq, Room room)
        {
            this.mX = pX;
            this.mY = pY;
            this.mIsGroupGate = pIsGroupGate;
            this.mSquareData = SquareData;
            this.mInUse = true;
            this.mOverride = pOverride;
            this.mDistance = 0.0;
            this.mLastStep = (pX == pTargetX && pY == pTargetY);
            this.mDistance = DreamPathfinder.GetDistance(pX, pY, pTargetX, pTargetY);
            this.mHeight = Height;
                List<WalkUnderElement> walkunderele = new List<WalkUnderElement>();
                this.itemsonSquare = itemsOnSq.OrderBy(o => o.Double_0).ToList();
                foreach (RoomItem ri in itemsOnSq)
                {
                    walkunderele.Add(new WalkUnderElement(ri.Double_0, ri.Double_1, ri.GetBaseItem().Walkable, ri.uint_0, ri));
                }
                this.currentRoom = room;
                if (walkunderele.Where(o => o.Height == 0).ToList().Count == 0)
                    walkunderele.Add(new WalkUnderElement(0, 0, true, 0, null));
                this.walkunderElements = walkunderele.OrderBy(o => o.Height).ToList();
        }

        internal int X
        {
            get
            {
                return this.mX;
            }
            set
            {
                this.mX = value;
            }
        }
        internal int Y
        {
            get
            {
                return this.mY;
            }
            set
            {
                this.mY = value;
            }
        }
        internal double GetDistance
        {
            get
            {
                return this.mDistance;
            }
        }
        internal bool CanWalk
        {
            get
            {
                bool result;
                if (!this.mLastStep)
                {
                    result = (this.mOverride || this.mSquareData == 1 || this.mSquareData == 4 || this.mIsGroupGate || this.WalkUnder);
                }
                else
                {
                    result = (this.mOverride || this.mSquareData == 3 || this.mSquareData == 1 || this.WalkUnder);
                }
                return result;
            }
        }
        internal bool InUse
        {
            get
            {
                return this.mInUse;
            }
        }
        /*internal bool WalkUnder
        {
            get
            {
                IEnumerator<RoomItem> ienumerable = itemsonSquare.GetEnumerator();
                RoomItem ri = null;
                RoomItem ri2 = null;
                while (ienumerable.MoveNext())
                {
                    ri = ienumerable.Current;
                    if ((ri.GetBaseItem().Walkable && ri.Double_0 - mHeight <= 1.5 && ri.Double_0 - mHeight >= -1.5) || (ri.Double_0 >= 1.5 && mHeight == 0))
                    {
                        if (ienumerable.MoveNext())
                        {
                            ri2 = ienumerable.Current;
                            if (ri2 == null)
                            {
                                return true;
                            }
                            if (ri2.GetBaseItem().Walkable || ri.Double_0 - mHeight >= 3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else { return ri.Double_0 - mHeight >= 3 && ri.GetBaseItem().Walkable; }
                    }
                    else {}
                }
                return false;
            }
        }*/
        internal bool WalkUnder
        {
            get
            {
                if (currentRoom.CanWalkUnder)
                {
                    if (currentRoom.method_43(X, Y, mHeight) == null)
                    {
                        WalkUnderElement closestItem = ClosestTo(walkunderElements, mHeight);
                        bool IsNextItem = false;
                        foreach (WalkUnderElement ri in this.walkunderElements)
                        {
                            if (ri != null && closestItem.Id == ri.Id)
                            {
                                IsNextItem = true;
                                continue;
                            }
                            if (IsNextItem && (ri.Height - closestItem.Height >= 2) && (closestItem.FullHeight - mHeight <= 2) && closestItem.Walkable)
                                return true;

                            if (IsNextItem)
                                IsNextItem = false;
                            if (ri.Height == closestItem.FullHeight)
                            {
                                IsNextItem = true;
                                closestItem = ri;
                            }

                        }
                        return IsNextItem && closestItem.Walkable && closestItem.Item != null;
                    }
                }
                return false;
            }
        }
        internal double SmallestZ(double Z)
        {
            WalkUnderElement closestItem = ClosestTo(walkunderElements, mHeight);
            bool IsNextItem = false;
            foreach (WalkUnderElement ri in this.walkunderElements)
            {
                if (ri != null && closestItem.Id == ri.Id)
                {
                    IsNextItem = true;
                    continue;
                }
                if (IsNextItem && (ri.Height - closestItem.Height >= 2) && (closestItem.FullHeight - mHeight <= 2) && closestItem.Walkable)
                    return closestItem.FullHeight;

                if (IsNextItem)
                    IsNextItem = false;
                if (ri.Height == closestItem.FullHeight)
                {
                    IsNextItem = true;
                    closestItem = ri;
                }

            }
            return IsNextItem && closestItem.Walkable && closestItem.Item != null ? closestItem.FullHeight : 0.0;
            /*List<double> doubles = new List<double>();
            if (itemsonSquare.Where(o => o.Double_0 == 0.0).ToList().Count == 0)
                doubles.Add(0.0);
            foreach (RoomItem ri in itemsonSquare)
            {
                if (ri.GetBaseItem().Walkable)
                    doubles.Add(ri.Double_1);
            }
            return this.ClosestTo(doubles, Z);*/
        }
        public double ClosestTo(List<double> collection, double target)
        {
            var closest = double.MaxValue;
            var minDifference = double.MaxValue;
            foreach (var element in collection)
            {
                var difference = Math.Abs((double)element - target);
                if (minDifference > difference)
                {
                    minDifference = (double)difference;
                    closest = element;
                }
            }
            return closest;
        }
        public WalkUnderElement ClosestTo(List<WalkUnderElement> collection, double target)
        {
            /*WalkUnderElement closest = null;
            var minDifference = double.MaxValue;
            foreach (var element in collection)
            {
                var difference = Math.Abs((double)element.Height - target);
                if (minDifference > difference)
                {
                    minDifference = (double)difference;
                    closest = element;
                }
            }

            return closest;*/
            return collection.OrderBy(item => Math.Abs(target - item.Height)).First();
        }
    }
}
