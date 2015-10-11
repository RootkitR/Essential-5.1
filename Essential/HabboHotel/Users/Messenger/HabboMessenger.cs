using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.Users.UserDataManagement;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Messages;
using Essential.HabboHotel.Users.Messenger;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
using System.Linq;
namespace Essential.HabboHotel.Users.Messenger
{
	internal sealed class HabboMessenger
	{
		private uint uint_0;
		internal Hashtable hashtable_0;
		private Hashtable hashtable_1;
		internal bool bool_0;
		public HabboMessenger(uint uint_1)
		{
			this.hashtable_0 = new Hashtable();
			this.hashtable_1 = new Hashtable();
			this.uint_0 = uint_1;
		}
		internal void method_0(UserDataFactory class12_0)
		{
			this.hashtable_0 = new Hashtable();

			DataTable dataTable_ = class12_0.GetFriends();

			if (dataTable_ != null)
			{
                int status = 0;
				foreach (DataRow dataRow in dataTable_.Rows)
				{
                    if (!this.hashtable_0.Contains((uint)dataRow["Id"]))
                    {
                        if (class12_0.getRelation((uint)dataRow["Id"]) != null)
                            status = (int)class12_0.getRelation((uint)dataRow["Id"]).relationshipStatus;
                        else
                            status = 0;

                        this.hashtable_0.Add((uint)dataRow["Id"], new MessengerBuddy((uint)dataRow["Id"], dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string, status));
                    }
				}
				try
				{
					if (this.method_25().GetHabbo().HasFuse("receive_sa"))
					{
                        this.hashtable_0.Add(0, new MessengerBuddy(0u, "Staff Chat", this.method_25().GetHabbo().Figure, "Staff Chat Room", "0",1));
					}
				}
				catch
				{
				}
			}
		}
		internal void method_1(UserDataFactory class12_0)
		{
			this.hashtable_1 = new Hashtable();
			DataTable dataTable_ = class12_0.GetFriendRequests();
			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
                    if (!this.hashtable_1.ContainsKey((uint)dataRow["from_id"]))
                    {
                        this.hashtable_1.Add((uint)dataRow["from_id"], new MessengerRequest((uint)dataRow["Id"], this.uint_0, (uint)dataRow["from_id"], dataRow["username"] as string, dataRow["gender"] as string, dataRow["look"] as string));
                    }
				}
			}
		}

		internal void method_2()
		{
			this.hashtable_0.Clear();
		}
		public void method_3()
		{
			this.hashtable_1.Clear();
		}
		internal MessengerRequest method_4(uint uint_1)
		{
			return this.hashtable_1[uint_1] as MessengerRequest;
		}
		internal void method_5(bool bool_1)
		{
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				try
				{
					GameClient class2 = Essential.GetGame().GetClientManager().GetClient(@class.UInt32_0);
					if (class2 != null && class2.GetHabbo() != null && class2.GetHabbo().GetMessenger() != null)
					{
						class2.GetHabbo().GetMessenger().method_6(this.uint_0);
						if (bool_1)
						{
							class2.GetHabbo().GetMessenger().method_7();
						}
					}
				}
				catch
				{
				}
			}
			hashtable.Clear();
			hashtable = null;
		}
		internal bool method_6(uint uint_1)
		{
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			bool result;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.UInt32_0 == uint_1)
				{
					@class.bool_0 = true;
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_7()
		{
            this.SerializeUpdates(this.method_25());
		}
		internal bool method_8(uint uint_1, uint uint_2)
		{
			bool result;
			if (uint_1 == uint_2)
			{
				result = true;
			}
			else
			{
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					if (@class.ReadDataRow(string.Concat(new object[]
					{
						"SELECT Id FROM messenger_requests WHERE to_id = '",
						uint_1,
						"' AND from_id = '",
						uint_2,
						"' LIMIT 1"
					})) != null)
					{
						result = true;
						return result;
					}
					if (@class.ReadDataRow(string.Concat(new object[]
					{
						"SELECT Id FROM messenger_requests WHERE to_id = '",
						uint_2,
						"' AND from_id = '",
						uint_1,
						"' LIMIT 1"
					})) != null)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}
		internal bool method_9(uint uint_1, uint uint_2)
		{
			bool result;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				if (@class.ReadDataRow(string.Concat(new object[]
				{
					"SELECT user_one_id FROM messenger_friendships WHERE user_one_id = '",
					uint_1,
					"' AND user_two_id = '",
					uint_2,
					"' LIMIT 1"
				})) != null)
				{
					result = true;
					return result;
				}
				if (@class.ReadDataRow(string.Concat(new object[]
				{
					"SELECT user_one_id FROM messenger_friendships WHERE user_one_id = '",
					uint_2,
					"' AND user_two_id = '",
					uint_1,
					"' LIMIT 1"
				})) != null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_10()
		{
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = '" + this.uint_0 + "'");
			}
			this.method_3();
		}
		internal void method_11(uint uint_1)
		{
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("userid", this.uint_0);
				@class.AddParamWithValue("fromid", uint_1);
				@class.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = @userid AND from_id = @fromid LIMIT 1");
			}
			if (this.method_4(uint_1) != null)
			{
				this.hashtable_1.Remove(uint_1);
			}
		}
		internal void method_12(uint uint_1)
		{
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("toid", uint_1);
				@class.AddParamWithValue("userid", this.uint_0);
				@class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@userid,@toid)");
				@class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@toid,@userid)");
			}
			this.method_14(uint_1);
			GameClient class2 = Essential.GetGame().GetClientManager().GetClient(uint_1);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().method_14(this.uint_0);

                class2.GetHabbo().CheckFriendListSize();
			}
		}
		internal void method_13(uint uint_1)
		{
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("toid", uint_1);
				@class.AddParamWithValue("userid", this.uint_0);
				@class.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @toid AND user_two_id = @userid LIMIT 1");
				@class.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @userid AND user_two_id = @toid LIMIT 1");
			}
			this.method_15(uint_1);
			GameClient class2 = Essential.GetGame().GetClientManager().GetClient(uint_1);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().method_15(this.uint_0);
			}
		}
		internal void method_14(uint uint_1)
		{

            this.method_25().GetHabbo().CheckFriendListSize();
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				DataRow dataRow = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + uint_1 + "' LIMIT 1");
                int relation = this.method_25().GetHabbo().getRelation(uint_1);
                MessengerBuddy class2 = new MessengerBuddy(uint_1, dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string, relation);
				class2.bool_0 = true;
				if (!this.hashtable_0.Contains(class2.UInt32_0))
				{
					this.hashtable_0.Add(class2.UInt32_0, class2);
				}
				this.method_7();
			}
		}
		internal void method_15(uint uint_1)
		{
			this.hashtable_0.Remove(uint_1);
            ServerMessage Message = new ServerMessage(Outgoing.FriendUpdate);
			Message.AppendInt32(0);
			Message.AppendInt32(1);
			Message.AppendInt32(-1);
			Message.AppendUInt(uint_1);
			this.method_25().SendMessage(Message);
		}
		internal void method_16(string string_0)
		{
			DataRow dataRow = null;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0.ToLower());
				dataRow = @class.ReadDataRow("SELECT Id,block_newfriends FROM users WHERE username = @query LIMIT 1");
			}
			if (dataRow != null)
			{
				if (Essential.StringToBoolean(dataRow["block_newfriends"].ToString()) && !this.method_25().GetHabbo().HasFuse("ignore_friendsettings"))
				{
                    ServerMessage Message = new ServerMessage(Outgoing.MessengerError); // Updated
					Message.AppendInt32(39);
					Message.AppendInt32(3);
					this.method_25().SendMessage(Message);
				}
				else
				{
					uint num = (uint)dataRow["Id"];
					if (!this.method_8(this.uint_0, num))
					{
						using (DatabaseClient @class = Essential.GetDatabase().GetClient())
						{
							@class.AddParamWithValue("toid", num);
							@class.AddParamWithValue("userid", this.uint_0);
							@class.ExecuteQuery("INSERT INTO messenger_requests (to_id,from_id) VALUES (@toid,@userid)");
						}
						GameClient class2 = Essential.GetGame().GetClientManager().GetClient(num);
						if (class2 != null && class2.GetHabbo() != null)
						{
							uint num2 = 0u;
							using (DatabaseClient @class = Essential.GetDatabase().GetClient())
							{
								@class.AddParamWithValue("toid", num);
								@class.AddParamWithValue("userid", this.uint_0);
								num2 = @class.ReadUInt32("SELECT Id FROM messenger_requests WHERE to_id = @toid AND from_id = @userid ORDER BY Id DESC LIMIT 1");
							}

                            string gender = Essential.GetGame().GetClientManager().GetDataById(this.uint_0, "gender");
                            string look = Essential.GetGame().GetClientManager().GetDataById(this.uint_0, "look");
                            string username = Essential.GetGame().GetClientManager().GetNameById(this.uint_0);

                            if (!(string.IsNullOrEmpty(gender) && string.IsNullOrEmpty(look) && string.IsNullOrEmpty(username)))
                            {
                                MessengerRequest class3 = new MessengerRequest(num2, num, this.uint_0, username, gender, look);
                                class2.GetHabbo().GetMessenger().method_17(num2, num, this.uint_0, username, gender, look);
                                ServerMessage Message5_ = new ServerMessage(Outgoing.SendFriendRequest); // Updated
                                class3.method_0(Message5_);
                                class2.SendMessage(Message5_);
                            }
						}
					}
				}
			}
		}
		internal void method_17(uint uint_1, uint uint_2, uint uint_3, string username, string gender, string look)
		{
			if (!this.hashtable_1.ContainsKey(uint_3))
			{
				this.hashtable_1.Add(uint_3, new MessengerRequest(uint_1, uint_2, uint_3, username, gender, look));
			}
		}
		internal void method_18(uint uint_1, string string_0)
		{

            if (!String.IsNullOrEmpty(string_0) && !String.IsNullOrWhiteSpace(string_0))
            {


                if (!this.method_9(uint_1, this.uint_0))
                {
                    this.method_20(6, uint_1);
                }
                else
                {
                    GameClient @class = Essential.GetGame().GetClientManager().GetClient(uint_1);
                    if (@class == null || @class.GetHabbo().GetMessenger() == null)
                    {
                        this.method_20(5, uint_1);
                    }
                    else
                    {
                        if (this.method_25().GetHabbo().IsMuted)
                        {
                            this.method_20(4, uint_1);
                        }
                        else
                        {
                            if (@class.GetHabbo().IsMuted)
                            {
                                this.method_20(3, uint_1);
                                return;
                            }
                            if (this.method_25().GetHabbo().method_4() > 0)
                            {
                                TimeSpan timeSpan = DateTime.Now - this.method_25().GetHabbo().dateTime_0;
                                if (timeSpan.Seconds > 4)
                                {
                                    this.method_25().GetHabbo().int_23 = 0;
                                }
                                if (timeSpan.Seconds < 4 && this.method_25().GetHabbo().int_23 > 5)
                                {
                                    this.method_20(4, uint_1);
                                    return;
                                }
                                this.method_25().GetHabbo().dateTime_0 = DateTime.Now;
                                this.method_25().GetHabbo().int_23++;
                            }

                            string_0 = ChatCommandHandler.ApplyFilter(string_0);
                            if (ServerConfiguration.EnableChatlog && !this.method_25().GetHabbo().IsJuniori)
                            {
                                using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                                {
                                    class2.AddParamWithValue("message", "<PM to " + @class.GetHabbo().Username + ">: " + string_0);
                                    class2.ExecuteQuery(string.Concat(new object[]
								{
									"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
									this.method_25().GetHabbo().Id,
									"','0','",
									DateTime.Now.Hour,
									"','",
									DateTime.Now.Minute,
									"',UNIX_TIMESTAMP(),@message,'",
									this.method_25().GetHabbo().Username,
									"','",
									DateTime.Now.ToLongDateString(),
									"')"
								}));
                                }
                            }
                            @class.GetHabbo().GetMessenger().method_19(string_0, this.uint_0);
                        }
                    }
                }
            }
		}
		internal void method_19(string string_0, uint uint_1)
		{
            ServerMessage Message = new ServerMessage(Outgoing.InstantChat);
			Message.AppendUInt(uint_1);
			Message.AppendString(string_0);
            Message.AppendInt32(0);
			this.method_25().SendMessage(Message);
		}
		internal void method_20(int int_0, uint uint_1)
		{
            if (this != null)
            {
                ServerMessage Message = new ServerMessage(Outgoing.InstantChatError);
                Message.AppendInt32(int_0);
                Message.AppendUInt(uint_1);
                Message.AppendString(Essential.GetGame().GetClientManager().GetNameById(uint_1));
                this.method_25().SendMessage(Message);
            }
		}
		internal ServerMessage method_21()
		{
            this.method_25().GetHabbo().CheckFriendListSize();
            ServerMessage Message = new ServerMessage(Outgoing.InitFriends); // OLD 12 Updated
            Message.AppendInt32(1100);
            Message.AppendInt32(300);
            Message.AppendInt32(800);
            Message.AppendInt32(1100);
            Message.AppendInt32(0);
			Message.AppendInt32(this.hashtable_0.Count);
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
                @class.Serialize(Message, false);
			}
			return Message;
		}
        internal static ServerMessage SerializeUpdate(MessengerBuddy friend)
        {
            ServerMessage reply = new ServerMessage(Outgoing.FriendUpdate);
            reply.AppendInt32(0); // category
            reply.AppendInt32(1); // number of updates
            reply.AppendInt32(0); // don't know

            friend.Serialize(reply, false);
            reply.AppendBoolean(false);

            return reply;
        }
	    internal void SerializeUpdates(GameClient Session)
		{
			List<MessengerBuddy> list = new List<MessengerBuddy>();
			int num = 0;
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.bool_0)
				{
					num++;
					list.Add(@class);
					@class.bool_0 = false;
				}
			}

			hashtable.Clear();
        	foreach (MessengerBuddy @class in list)
			{
                ServerMessage Message = new ServerMessage(Outgoing.FriendUpdate); // Updated
			    Message.AppendInt32(0);
			    Message.AppendInt32(1);
			    Message.AppendInt32(0);
                @class.Serialize(Message, false);
                Session.SendMessage(Message);
            }
        }		
		internal ServerMessage method_23()
		{
            ServerMessage Message = new ServerMessage(Outgoing.InitRequests); // Updated
			Message.AppendInt32(this.hashtable_1.Count);
			Message.AppendInt32(this.hashtable_1.Count);
			Hashtable hashtable = this.hashtable_1.Clone() as Hashtable;
			foreach (MessengerRequest @class in hashtable.Values)
			{
				@class.method_0(Message);
			}
			return Message;
		}
		internal ServerMessage method_24(string string_0)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0 + "%");
				dataTable = @class.ReadDataTable("SELECT Id FROM users WHERE username LIKE @query LIMIT 50");
			}
			List<DataRow> list = new List<DataRow>();
			List<DataRow> list2 = new List<DataRow>();
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					if (this.method_9(this.uint_0, (uint)dataRow["Id"]))
					{
						list.Add(dataRow);
					}
					else
					{
						list2.Add(dataRow);
					}
				}
			}
            ServerMessage Message = new ServerMessage(Outgoing.SearchFriend);
			Message.AppendInt32(list.Count);
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				foreach (DataRow dataRow in list)
				{
					uint num = (uint)dataRow["Id"];
					DataRow dataRow2 = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + num + "' LIMIT 1");
                    int relation = this.method_25().GetHabbo().getRelation(num);
                    new MessengerBuddy(num, dataRow2["username"] as string, dataRow2["look"] as string, dataRow2["motto"] as string, dataRow2["last_online"] as string, relation).Serialize(Message, true);
				}
				Message.AppendInt32(list2.Count);
				foreach (DataRow dataRow in list2)
				{
					uint num = (uint)dataRow["Id"];
					DataRow dataRow2 = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + num + "' LIMIT 1");
                    int relation = this.method_25().GetHabbo().getRelation(num);
                    new MessengerBuddy(num, dataRow2["username"] as string, dataRow2["look"] as string, dataRow2["motto"] as string, dataRow2["last_online"] as string, relation).Serialize(Message, true);
				}
			}
			return Message;
		}
		private GameClient method_25()
		{
			return Essential.GetGame().GetClientManager().GetClient(this.uint_0);
		}
		internal Hashtable method_26()
		{
			return this.hashtable_0.Clone() as Hashtable;
		}
        internal bool UserInFriends(uint id)
        {
            return this.hashtable_0.ContainsKey(id);
        }
        internal IEnumerable<RoomData> GetActiveFriendsRooms()
        {
            foreach (MessengerBuddy iteratorVariable0 in from p in this.hashtable_0.Values.OfType<MessengerBuddy>()
                                                         where p.Boolean_1
                                                         select p)
            {
                yield return iteratorVariable0.CurrentRoom.RoomData;
            }
        }
	}
}
