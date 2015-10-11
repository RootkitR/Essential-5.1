using System;
using System.Collections.Generic;
using System.Data;
using Essential.Storage;
namespace Essential
{
	internal sealed class GroupsManager
	{
		public int Id;
		public string Name;
		public string Description;
		public int OwnerId;
		public List<int> Members;
		public string Badge;
		public uint RoomId;
        public string Locked;
        public string Created;
        public string ColorOne;
        public string ColorTwo;
        public List<int> GuildStates = new List<int>();
        public int GuildBase = 0;
        public int GuildBaseColor = 0;
        public List<int> UserWithRanks = new List<int>();
        public bool canMove = false;
        public List<int> Petitions = new List<int>();
        public int OnlyAdminsCanMove = 1;
		public GroupsManager(int int_2, DataRow Row, DatabaseClient class6_0)
		{
			this.Id = int_2;
            this.Name = (string)Row["name"];
            this.Description = (string)Row["desc"];
            this.OwnerId = (int)Row["OwnerId"];
			this.Badge = (string)Row["badge"];
            this.RoomId = (uint)Row["roomid"];
			this.Locked = (string)Row["locked"];
            this.GuildBaseColor = (int)Row["GuildBaseColor"];
            this.GuildBase = (int)Row["GuildBase"];
            foreach (string str in Row["GuildStates"].ToString().Split(new char[] { ';' }))
            {
                try
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        GuildStates.Add(int.Parse(str));
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to add guild states for guild ID: " + int_2);
                }
            }
			this.Members = new List<int>();
            this.canMove = Essential.StringToBoolean((string)Row["members_canmove"]);
            this.OnlyAdminsCanMove = this.canMove ? 0 : 1;
            try
            {
                this.Created = Essential.TimestampToDate(int.Parse(((string)Row["created"]).Split('.')[0])).ToShortDateString();
            }catch{this.Created = Essential.TimestampToDate(Essential.GetUnixTimestamp()).ToShortDateString();}
                DataTable dataTable = class6_0.ReadDataTable("SELECT userid,hasRights FROM group_memberships WHERE groupid = " + int_2 + ";");
			foreach (DataRow dataRow in dataTable.Rows)
			{
                this.JoinGroup((int)dataRow["userid"]);
                if (Essential.StringToBoolean(dataRow["hasRights"].ToString()))
                    this.UserWithRanks.Add((int)dataRow["userid"]);
			}
            if (!this.UserWithRanks.Contains(this.OwnerId))
                this.UserWithRanks.Add(this.OwnerId);
            this.ColorOne = (string)Row["color_one"];
            this.ColorTwo = (string)Row["color_two"];
            DataTable dt = class6_0.ReadDataTable("SELECT userid FROM group_requests WHERE groupid=" + this.Id);
            foreach (DataRow dRow in dt.Rows)
            {
                if (!this.Petitions.Contains((int)dRow["userid"]))
                    this.Petitions.Add((int)dRow["userid"]);
            }
		}
		public void JoinGroup(int int_2)
		{
			if (!this.Members.Contains(int_2))
			{
				this.Members.Add(int_2);
			}
		}
        public bool MemberCanMove(uint userId)
        {
            return (this.Members.Contains((int)userId) && (this.UserWithRanks.Contains((int)userId) || this.canMove));
        }
		public void Leave(int int_2)
		{
			if (this.Members.Contains(int_2))
			{
				this.Members.Remove(int_2);
			}
		}
        public int Type
        {
            get
            {
                return (Locked.ToLower() == "open") ? 0 : (Locked.ToLower() == "closed") ? 2 : 1;
            }
        }
        public string OwnerName
        {
            get
            {
                return Essential.GetGame().GetClientManager().GetNameById((uint)this.OwnerId);
            }
        }
       /* public int getRank(int userId)
        {
            if (this.UserWithRanks.Contains(userId))
                return 2;
            if (this.Petitions.Contains(userId))
                return 3;
            if (this.list_0.Contains(userId))
                return 1;
            return 1;
        }*/
        public int getRank(int userId)
        {
            if (this.UserWithRanks.Contains(userId))
                return 1;
            if (this.Petitions.Contains(userId))
                return 3;
            if (this.Members.Contains(userId))
                return 2;
            return 2;
        }
        public int GetThing(uint userId)
        {
            if (this.Petitions.Contains((int)userId))
                return 2;
            if (this.Members.Contains((int)userId))
                return 3;
            return 0;
        }
        public bool HasMember(uint UserId)
        {
            return this.Members.Contains((int)UserId);
        }
	}
}
