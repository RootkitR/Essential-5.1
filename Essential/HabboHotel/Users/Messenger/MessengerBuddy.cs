using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System.Globalization;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Users.Messenger
{
	internal sealed class MessengerBuddy
	{
        private uint UserId;
		internal bool bool_0;
		private string Username;
		private string Look;
		private string Motto;
		private string LastOnline;
        private int RelationshipStatus;
		public uint UInt32_0
		{
			get
			{
                return this.UserId;
			}
		}
		internal string String_0
		{
			get
			{
                return this.Username;
			}
		}
		internal string String_1
		{
			get
			{
                GameClient @class = Essential.GetGame().GetClientManager().GetClient(this.UserId);
				string result;
				if (@class != null)
				{
					result = @class.GetHabbo().RealName;
				}
				else
				{
					using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
					{
                        result = class2.ReadString("SELECT real_name FROM users WHERE Id = '" + this.UserId + "' LIMIT 1");
					}
				}
				return result;
			}
		}
		internal string String_2
		{
			get
			{
				return this.Look;
			}
		}
		internal string String_3
		{
			get
			{
				return this.Motto;
			}
		}
		internal string String_4
		{
			get
			{
				return this.LastOnline;
			}
		}
		internal bool Boolean_0
		{
			get
			{
                GameClient @class = Essential.GetGame().GetClientManager().GetClient(this.UserId);
				return @class != null && @class.GetHabbo() != null && @class.GetHabbo().GetMessenger() != null && !@class.GetHabbo().GetMessenger().bool_0 && !@class.GetHabbo().HideOnline;
			}
		}
		internal bool Boolean_1
		{
			get
			{
                GameClient @class = Essential.GetGame().GetClientManager().GetClient(this.UserId);
                return @class != null && (@class.GetHabbo().InRoom && !@class.GetHabbo().HideInRom);
			}
		}
        internal Room CurrentRoom
        {
            get
            {
                return Essential.GetGame().GetClientManager().GetClient(this.UserId).GetHabbo().CurrentRoom;
            }
        }
        public MessengerBuddy(uint mUserId, string mUsername, string mLook, string mMotto, string mLastOnline, int mRelation)
		{
            this.UserId = mUserId;
            this.Username = mUsername;
            this.Look = mLook;
			this.Motto = mMotto;
            double timestamp;
            if (double.TryParse(mLastOnline, NumberStyles.Any, CustomCultureInfo.GetCustomCultureInfo(), out timestamp))
            {
                this.LastOnline = Essential.TimestampToDate(timestamp).ToString();
            }
            else
            {
                this.LastOnline = Essential.TimestampToDate(Essential.GetUnixTimestamp()).ToString();
            }
			this.bool_0 = false;
            this.RelationshipStatus = mRelation;
		}
        public void Serialize(ServerMessage reply, bool Search)
		{
			if (Search)
			{
                reply.AppendUInt(this.UserId);
                reply.AppendStringWithBreak(this.Username);
                reply.AppendString(this.Motto);
				
				bool boolean_ = this.Boolean_0;
				reply.AppendBoolean(boolean_);
                if (boolean_)
                {
                    reply.AppendBoolean(this.Boolean_1);
                }
                else
                {
                    reply.AppendBoolean(false);
                }
                reply.AppendStringWithBreak(string.Empty);
                reply.AppendUInt(0);
				reply.AppendStringWithBreak(this.Look);
                reply.AppendStringWithBreak(this.LastOnline);
			}
			else
			{
                reply.AppendUInt(this.UserId);
                reply.AppendStringWithBreak(this.Username);
                reply.AppendUInt(1);
                if (this.UserId == 0u)
				{
					reply.AppendBoolean(true);
					reply.AppendBoolean(false);
				}
				else
				{
					bool boolean_ = this.Boolean_0;
					reply.AppendBoolean(boolean_);
					if (boolean_)
					{
						reply.AppendBoolean(this.Boolean_1);
					}
					else
					{
						reply.AppendBoolean(false);
					}
				}
				reply.AppendStringWithBreak(this.Look);
                reply.AppendUInt(0);
				reply.AppendStringWithBreak(this.Motto);
                reply.AppendStringWithBreak("");
                reply.AppendStringWithBreak(this.LastOnline);
                reply.AppendBoolean(false);
                reply.AppendInt32(this.RelationshipStatus); //relationship
			}
		}
	}
}
