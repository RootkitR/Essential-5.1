using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Essential.Core;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.HabboHotel.Rooms
{
	internal sealed class Trade
	{
		private TradeUser[] class65_0;
		private int int_0;
		private uint uint_0;
		private uint uint_1;
		private uint uint_2;
		public bool Boolean_0
		{
			get
			{
				bool result;
				for (int i = 0; i < this.class65_0.Length; i++)
				{
					if (this.class65_0[i] != null && !this.class65_0[i].Boolean_0)
					{
						result = false;
						return result;
					}
				}
				result = true;
				return result;
			}
		}
		public Trade(uint uint_3, uint uint_4, uint uint_5)
		{
			this.uint_1 = uint_3;
			this.uint_2 = uint_4;
			this.class65_0 = new TradeUser[2];
			this.class65_0[0] = new TradeUser(uint_3, uint_5);
			this.class65_0[1] = new TradeUser(uint_4, uint_5);
			this.int_0 = 1;
			this.uint_0 = uint_5;
			TradeUser[] array = this.class65_0;
			for (int i = 0; i < array.Length; i++)
			{
				TradeUser @class = array[i];
				if (!@class.method_0().Statusses.ContainsKey("trd"))
				{
					@class.method_0().AddStatus("trd", "");
					@class.method_0().UpdateNeeded = true;
				}
			}
            ServerMessage Message = new ServerMessage(Outgoing.TradeStart); // Updated
			Message.AppendUInt(uint_3);
			Message.AppendInt32(1);
			Message.AppendUInt(uint_4);
            Message.AppendInt32(1);
			this.method_13(Message);
		}
		public bool method_0(uint uint_3)
		{
			bool result;
			for (int i = 0; i < this.class65_0.Length; i++)
			{
				if (this.class65_0[i] != null && this.class65_0[i].UserId == uint_3)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public TradeUser method_1(uint uint_3)
		{
			TradeUser result;
			for (int i = 0; i < this.class65_0.Length; i++)
			{
				if (this.class65_0[i] != null && this.class65_0[i].UserId == uint_3)
				{
					result = this.class65_0[i];
					return result;
				}
			}
			result = null;
			return result;
		}
		public void method_2(uint uint_3, UserItem class39_0)
		{
			TradeUser @class = this.method_1(uint_3);
			if (@class != null && class39_0 != null && class39_0.GetBaseItem().AllowTrade && !@class.Boolean_0 && this.int_0 == 1)
			{
				this.method_8();
				@class.OfferedItems.Add(class39_0);
				this.method_9();
			}
		}
		public void method_3(uint uint_3, UserItem class39_0)
		{
			TradeUser @class = this.method_1(uint_3);
			if (@class != null && class39_0 != null && !@class.Boolean_0 && this.int_0 == 1)
			{
				this.method_8();
				@class.OfferedItems.Remove(class39_0);
				this.method_9();
			}
		}
		public void method_4(uint uint_3)
		{
			TradeUser @class = this.method_1(uint_3);
			if (@class != null && this.int_0 == 1)
			{
				@class.Boolean_0 = true;
                ServerMessage Message = new ServerMessage(Outgoing.TradeAcceptUpdate); // Updated
				Message.AppendUInt(uint_3);
				Message.AppendInt32(1);
				this.method_13(Message);
				if (this.Boolean_0)
				{
                    this.method_13(new ServerMessage(Outgoing.TradeComplete)); // Update
					this.int_0++;
					this.method_8();
				}
			}
		}
		public void method_5(uint uint_3)
		{
			TradeUser @class = this.method_1(uint_3);
			if (@class != null && this.int_0 == 1 && !this.Boolean_0)
			{
				@class.Boolean_0 = false;
                ServerMessage Message = new ServerMessage(Outgoing.TradeAcceptUpdate); // Updated
				Message.AppendUInt(uint_3);
                Message.AppendInt32(0);
				this.method_13(Message);
			}
		}
		public void method_6(uint uint_3)
		{
			TradeUser @class = this.method_1(uint_3);
			if (@class != null && this.int_0 == 2)
			{
				@class.Boolean_0 = true;
                ServerMessage Message = new ServerMessage(Outgoing.TradeAcceptUpdate); // Updated
				Message.AppendUInt(uint_3);
                Message.AppendInt32(1);
				this.method_13(Message);
				if (this.Boolean_0)
				{
					this.int_0 = 999;
					Task task = new Task(new Action(this.method_7));
					task.Start();
				}
			}
		}
		private void method_7()
		{
			try
			{
				this.method_10();
				this.method_11();
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "Trade task");
			}
		}
		public void method_8()
		{
			using (TimedLock.Lock(this.class65_0))
			{
				TradeUser[] array = this.class65_0;
				for (int i = 0; i < array.Length; i++)
				{
					TradeUser @class = array[i];
					@class.Boolean_0 = false;
				}
			}
		}
		public void method_9()
		{
            ServerMessage Message = new ServerMessage(Outgoing.TradeUpdate); // Updated
			using (TimedLock.Lock(this.class65_0))
			{
				for (int i = 0; i < this.class65_0.Length; i++)
				{
					TradeUser @class = this.class65_0[i];
					if (@class != null)
					{
						Message.AppendUInt(@class.UserId);
						Message.AppendInt32(@class.OfferedItems.Count);
						using (TimedLock.Lock(@class.OfferedItems))
						{
							foreach (UserItem current in @class.OfferedItems)
							{
								Message.AppendUInt(current.uint_0);
								Message.AppendStringWithBreak(current.GetBaseItem().Type.ToString().ToLower());
								Message.AppendUInt(current.uint_0);
								Message.AppendInt32(current.GetBaseItem().Sprite);
                                Message.AppendInt32(0); 
								Message.AppendBoolean(true);
                                Message.AppendInt32((current.GetBaseItem().InteractionType == "poster" ? 6 : 0));
                                Message.AppendStringWithBreak((current.GetBaseItem().InteractionType == "poster" ? current.string_0 : string.Empty));
                                Message.AppendInt32(0); // xmas 09 furni had a special furni tag here, with wired day (wat?)
                                Message.AppendInt32(0); // xmas 09 furni had a special furni tag here, wired month (wat?)
                                Message.AppendInt32(0); // xmas 09 furni had a special furni tag here, wired year (wat?)
								if (current.GetBaseItem().Type == 's')
								{
									Message.AppendInt32(0);
								}
							}
						}
					}
				}
			}
			this.method_13(Message);
		}
		public void method_10()
		{
			List<UserItem> list_ = this.method_1(this.uint_1).OfferedItems;
			List<UserItem> list_2 = this.method_1(this.uint_2).OfferedItems;
			foreach (UserItem current in list_)
			{
				if (this.method_1(this.uint_1).method_1().GetHabbo().GetInventoryComponent().GetItemById(current.uint_0) == null)
				{
					this.method_1(this.uint_1).method_1().SendNotification("Tausch abgebrochen.");
					this.method_1(this.uint_2).method_1().SendNotification("Tausch abgebrochen.");
					return;
				}
			}
			foreach (UserItem current in list_2)
			{
				if (this.method_1(this.uint_2).method_1().GetHabbo().GetInventoryComponent().GetItemById(current.uint_0) == null)
				{
                    this.method_1(this.uint_1).method_1().SendNotification("Tausch abgebrochen.");
                    this.method_1(this.uint_2).method_1().SendNotification("Tausch abgebrochen.");
					return;
				}
			}
			this.method_1(this.uint_2).method_1().GetHabbo().GetInventoryComponent().SavePets();
			this.method_1(this.uint_1).method_1().GetHabbo().GetInventoryComponent().SavePets();
            string trade1 = "";
            string trade2 = "";
			foreach (UserItem current in list_)
			{
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE items SET room_id = '0', user_id = '",
						this.method_1(this.uint_2).method_1().GetHabbo().Id,
						"' WHERE Id = '",
						current.uint_0,
						"' LIMIT 1"
					}));
				}
                if (trade1 == "")
                {
                    trade1 = current.uint_0.ToString();
                }
                else
                {
                    trade1 = trade1 + "," + current.uint_0;
                }
                this.method_1(this.uint_1).method_1().GetHabbo().GetInventoryComponent().method_12(current.uint_0, this.method_1(this.uint_2).method_1().GetHabbo().Id, true);
				this.method_1(this.uint_2).method_1().GetHabbo().GetInventoryComponent().method_11(current.uint_0, current.uint_1, current.string_0, false, current.LtdId, current.LtdCnt, current.GuildData);
			}
			foreach (UserItem current in list_2)
			{
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE items SET room_id = '0', user_id = '",
						this.method_1(this.uint_1).method_1().GetHabbo().Id,
						"' WHERE Id = '",
						current.uint_0,
						"' LIMIT 1"
					}));
				}
                if (trade2 == "")
                {
                    trade2 = current.uint_0.ToString();
                }
                else
                {
                    trade2 = trade2 + "," + current.uint_0;
                }
                this.method_1(this.uint_2).method_1().GetHabbo().GetInventoryComponent().method_12(current.uint_0, this.method_1(this.uint_1).method_1().GetHabbo().Id, true);
				this.method_1(this.uint_1).method_1().GetHabbo().GetInventoryComponent().method_11(current.uint_0, current.uint_1, current.string_0, false, current.LtdId, current.LtdCnt, current.GuildData);
			}
			this.method_1(this.uint_1).method_1().GetHabbo().GetInventoryComponent().method_9(true);
			this.method_1(this.uint_2).method_1().GetHabbo().GetInventoryComponent().method_9(true);
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "INSERT INTO trades (owner_before,owner_after,items,timestamp) VALUES (",
                        this.method_1(this.uint_2).method_1().GetHabbo().Id,
                        ",",
                        this.method_1(this.uint_1).method_1().GetHabbo().Id,
                        ",'",
                        trade2,
                        "','",
                        Essential.GetUnixTimestamp(),
                        "')"
                    }));
                dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "INSERT INTO trades (owner_before,owner_after,items,timestamp) VALUES (",
                        this.method_1(this.uint_1).method_1().GetHabbo().Id,
                        ",",
                        this.method_1(this.uint_2).method_1().GetHabbo().Id,
                        ",'",
                        trade1,
                        "','",
                        Essential.GetUnixTimestamp(),
                        "')"
                    }));
            }
		}
		public void method_11()
		{
			for (int i = 0; i < this.class65_0.Length; i++)
			{
				TradeUser @class = this.class65_0[i];
				if (@class != null && @class.method_0() != null)
				{
					@class.method_0().RemoveStatus("trd");
					@class.method_0().UpdateNeeded = true;
				}
			}
            this.method_13(new ServerMessage(Outgoing.TradeCloseClean)); // Updated
			this.method_14().list_2.Remove(this);
		}
		public void method_12(uint uint_3)
		{
			for (int i = 0; i < this.class65_0.Length; i++)
			{
				TradeUser @class = this.class65_0[i];
				if (@class != null && @class.method_0() != null)
				{
					@class.method_0().RemoveStatus("trd");
					@class.method_0().UpdateNeeded = true;
				}
			}
            ServerMessage Message = new ServerMessage(Outgoing.TradeClose); // Updated
			Message.AppendUInt(uint_3);
            Message.AppendInt32(2);
			this.method_13(Message);
		}
		public void method_13(ServerMessage Message5_0)
		{
			if (this.class65_0 != null)
			{
				for (int i = 0; i < this.class65_0.Length; i++)
				{
					TradeUser @class = this.class65_0[i];
					if (@class != null && @class != null && @class.method_1() != null)
					{
						@class.method_1().SendMessage(Message5_0);
					}
				}
			}
		}
		private Room method_14()
		{
			return Essential.GetGame().GetRoomManager().GetRoom(this.uint_0);
		}
	}
}
