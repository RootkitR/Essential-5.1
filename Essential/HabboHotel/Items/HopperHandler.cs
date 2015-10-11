using System;
using System.Data;
using Essential.Storage;
namespace Essential.HabboHotel.Items
{
    internal sealed class HopperHandler
    {
        public static uint GetOtherId(uint uint_0)
        {
            uint result;
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("itemid", uint_0);
                int ItemBaseItem = dbClient.ReadInt32("SELECT base_item FROM items WHERE id = @itemid");
                dbClient.AddParamWithValue("itembaseitem", ItemBaseItem);
                //DataRow dataRow = dbClient.ReadDataRow("SELECT id FROM items WHERE base_item = 4716 ORDER BY rand() LIMIT 1;");
                DataRow dataRow = dbClient.ReadDataRow("SELECT id FROM items WHERE base_item = @itembaseitem AND NOT id = @itemid AND room_id > 0 ORDER BY rand() LIMIT 1;");
                if (dataRow == null)
                {
                    result = 0u;
                }
                else
                {
                    result = (uint)dataRow[0];
                }
            }
            return result;
        }
        public static uint GetRoomId(uint uint_0)
        {
            uint result;
            using (DatabaseClient @class = Essential.GetDatabase().GetClient())
            {
                DataRow dataRow = @class.ReadDataRow("SELECT room_id FROM items WHERE Id = '" + uint_0 + "' LIMIT 1;");
                //DataRow dataRow = @class.ReadDataRow("SELECT room_id FROM items WHERE base_item = 4716 ORDER BY rand() LIMIT 1");
                if (dataRow == null)
                {
                    result = 0u;
                }
                else
                {
                    result = (uint)dataRow[0];
                }
            }
            return result;
        }
        public static bool IsInRoom(uint uint_0)
        {
            uint num = HopperHandler.GetOtherId(uint_0);
            bool result;
            if (num == 0u)
            {
                result = false;
            }
            else
            {
                uint num2 = HopperHandler.GetRoomId(num);
                result = (num2 != 0u);
            }
            return result;
        }
    }
}
