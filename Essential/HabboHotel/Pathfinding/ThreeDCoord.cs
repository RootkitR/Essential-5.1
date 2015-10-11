using System;
namespace Essential.HabboHotel.Pathfinding
{
    public struct ThreeDCoord
    {
        internal int x;
        internal int y;
        internal ThreeDCoord(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }
        public static bool Equals(ThreeDCoord a, ThreeDCoord b)
        {
            return object.ReferenceEquals(a, b) || ((object)a != null && (object)b != null && a.x == b.x && a.y == b.y);
        }
        public static bool IsNot(ThreeDCoord a, ThreeDCoord b)
        {
            return !ThreeDCoord.Equals(a, b);
        }
        public override int GetHashCode()
        {
            return this.x ^ this.y;
        }
        public override bool Equals(object obj)
        {
            return base.GetHashCode().Equals(obj.GetHashCode());
        }
    }
}
