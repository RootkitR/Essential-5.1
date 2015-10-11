using System;
using Essential.Messages;
using Essential.HabboHotel;
using Essential.Storage;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pets;

namespace Essential.HabboHotel.Users.Inventory
{
    internal sealed class UserBot
    {
        public uint BotId;
        public uint OwnerId;
        public int RoomId;
        public string Look;
        public string Name;
        public bool PlacedInRoom;
        internal DatabaseUpdateState DBState;
        public int X;
        public int Y;
        public string BotType;
        public string walkmode;
        public int minX;
        public int maxX;
        public int minY;
        public int maxY;
        public UserBot(uint BotId, uint OwnerId, string Look, string Name, bool PlacedInRoom, int RoomId, int x, int y, string botType, string wm)
        {
            this.BotId = BotId;
            this.OwnerId = OwnerId;
            this.Look = Look;
            this.Name = Name;
            this.DBState = DatabaseUpdateState.Updated;
            this.PlacedInRoom = PlacedInRoom;
            this.RoomId = RoomId;
            this.X = x;
            this.Y = y;
            this.BotType = botType;
            this.walkmode = wm;
        }
    }
}
