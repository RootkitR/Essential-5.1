using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Users.Stream
{
    public class StreamEntry
    {
        public int ID;
        public int VirtualID;
        public string UserID;
        public string Username;
        public string Gender;
        public string Look;
        public int Time;
        public int Likes;
        public bool Liked;
        public string Text;
        public bool isStaffEntry;
        public StreamEntry(int ID, int VirtualID, string UserID, string Username, string Gender, string look, int Time, int Likes, bool Liked, string Text)
        {
            this.ID = ID;
            this.VirtualID = VirtualID;
            this.UserID = UserID;
            this.Username = Username;
            this.Gender = Gender;
            this.Look = look;
            this.Time = Time;
            this.Likes = Likes;
            this.Liked = Liked;
            this.Text = Text;
            this.isStaffEntry = UserID == "1" ? true : false;
        }
    }
}
