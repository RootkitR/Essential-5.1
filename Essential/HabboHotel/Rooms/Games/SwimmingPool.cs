using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Essential.HabboHotel.Pathfinding;

namespace Essential.HabboHotel.Rooms.Games
{
    class SwimmingPool
    {
        // todo cleanup ^ Safeup :D
        internal static Boolean RoomIsSwimmingPool(Room Room)
        {
            return (Room != null && Room.CCTs.Contains("hh_room_pool"));
        }

        internal static Boolean UserIsOnSwimTile(RoomUser User)
        {
            if (User == null)
            {
                return false;
            }

            if (User.double_1 >= GetStandardMapHeight())
            {
                return false;
            }

            var Heights = GetStandardSwimMapHeight();

            foreach (double Height in Heights)
            {
                if (User.double_1 == Height)
                {
                    return true;
                }
            }

            return false;
        }

      

        internal static double GetStandardMapHeight()
        {
            return 7;
        }

        internal static double[] GetStandardSwimMapHeight()
        {
            return new double[] { 3, 2, 1 };
        }

       
        public static void ActivateStatus(RoomUser User)
        {
            if (User == null)
            {
                return;
            }

            if (UserIsOnSwimTile(User) && !User.Statusses.ContainsKey("swim"))
            {
                User.AddStatus("swim", "");
            }
        }
    }
}
