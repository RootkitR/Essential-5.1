using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Support;
using Essential.HabboHotel.Achievements;
using Essential.Net;
using Essential.Util;
using Essential.Messages;
using Essential.Storage;
namespace Essential.HabboHotel.GameClients
{
	internal sealed class GameClientManager
	{
		private Task task_0;
		private GameClient[] Clients;
		private Hashtable hashtable_0;
		private Hashtable hashtable_1;
		private Timer DisposeTimer;
        private List<SocketConnection> DisposeQueue;
		public int ClientCount
		{
			get
			{
                if (this.Clients == null)
                    return 0;

                int num = 0;

                for (int i = 0; i < this.Clients.Length; i++)
                {
                    if (this.Clients[i] != null && this.Clients[i].GetHabbo() != null && !string.IsNullOrEmpty(this.Clients[i].GetHabbo().Username))
                    {
                        num++;
                    }
                }

                return num;
			}
		}
        public GameClient[] GetClients()
        {
            return Clients;
        }
		public GameClientManager(int clientCapacity)
		{
			this.hashtable_0 = new Hashtable();
			this.hashtable_1 = new Hashtable();

			this.Clients = new GameClient[clientCapacity];

            this.DisposeQueue = new List<SocketConnection>();
			this.DisposeTimer = new Timer(new TimerCallback(this.DisposeTimerCallback), null, 500, 500);
		}
		public void AddClient(uint uint_0, string string_0, GameClient class16_1)
		{
			this.hashtable_0[uint_0] = class16_1;
			this.hashtable_1[string_0.ToLower()] = class16_1;
		}
		public void RemoveClient(uint uint_0, string string_0)
		{
			this.hashtable_0[uint_0] = null;
			this.hashtable_1[string_0.ToLower()] = null;
		}
		public GameClient GetClient(uint id)
		{
			GameClient result;

			if (this.Clients == null || this.hashtable_0 == null)
			{
				result = null;
			}

			else
			{
				if (this.hashtable_0.ContainsKey(id))
				{
					result = (GameClient)this.hashtable_0[id];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		public GameClient GetClientByHabbo(string string_0)
		{
			GameClient result;
			if (this.Clients == null || this.hashtable_1 == null || string.IsNullOrEmpty(string_0))
			{
				result = null;
			}
			else
			{
				if (this.hashtable_1.ContainsKey(string_0.ToLower()))
				{
					result = (GameClient)this.hashtable_1[string_0.ToLower()];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		private void DisposeTimerCallback(object sender)
		{
			try
			{
                List<SocketConnection> list = this.DisposeQueue;
                this.DisposeQueue = new List<SocketConnection>();

				if (list != null)
				{
                    foreach (SocketConnection current in list)
                    //foreach (ConnectionInformation current in list)
					{
						if (current != null)
						{
                            current.method_1();
                            //current.disconnect();
						}
					}
				}
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "Disconnector task");
			}
		}
        internal void DisposeConnection(SocketConnection connection)
        {
            if (!this.DisposeQueue.Contains(connection))
            {
                this.DisposeQueue.Add(connection);
            }
        }
		public GameClient GetClientById(uint id)
		{
			GameClient result;

			try
			{
				result = this.Clients[id];
			}
			catch
			{
				result = null;
			}
			return result;
		}
        internal void AddClient(uint uint_0, ref SocketConnection Message1_0)
        {
            this.Clients[(int)((UIntPtr)uint_0)] = new GameClient(uint_0, ref Message1_0);
            this.Clients[(int)((UIntPtr)uint_0)].GetSocketConnection();
        }
		public void Dispose(uint uint_0, bool ClientDisconnect = true)
		{
			GameClient @class = this.GetClientById(uint_0);
			if (@class != null)
			{
                if(ClientDisconnect)
                    Console.WriteLine("[DISCONNECT] [" + @class.GetHabbo().Username + "] [" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "]REASON: Client Disconnect");
                //Essential.smethod_14().method_6(uint_0);
				@class.Dispose();
				this.Clients[(int)((UIntPtr)uint_0)] = null;
			}
		}
		public void StartPingTask()
		{
			if (this.task_0 == null)
			{
				this.task_0 = new Task(new Action(this.PingTask));
				this.task_0.Start();
			}
		}
		public void StopPingTask()
		{
			if (this.task_0 != null)
			{
				this.task_0 = null;
			}
		}
		private void PingTask()
		{
			int num = int.Parse(Essential.GetConfig().data["client.ping.interval"]);

			if (num <= 100)
			{
				throw new ArgumentException("Invalid configuration value for ping interval! Must be above 100 miliseconds.");
			}

			while (true)
			{
				try
				{
                    ServerMessage Message = new ServerMessage(Outgoing.Ping); // Updated
					List<GameClient> list = new List<GameClient>();
					List<GameClient> list2 = new List<GameClient>();
					for (int i = 0; i < this.Clients.Length; i++)
					{
						GameClient @class = this.Clients[i];
						if (@class != null)
						{
							if (@class.bool_0)
							{
								@class.bool_0 = false;
								list2.Add(@class);
							}
							else
							{
								list.Add(@class);
							}
						}
					}
                    foreach (GameClient @class in list)
                    {
                        /*try
                        {
                            @class.Disconnect("PING");
                        }
                        catch
                        {
                        }*/
                    }
					byte[] byte_ = Message.GetBytes();
					foreach (GameClient @class in list2)
					{
						try
						{
                            @class.GetConnection().SendData(byte_);
						}
						catch
						{
                            //@class.Disconnect("PING ERROR");
						}
					}
				}
				catch (Exception ex)
				{
                    Logging.LogThreadException(ex.ToString(), "Connection checker task");
				}
				Thread.Sleep(num);
			}
		}
		internal void BroadcastMessage(ServerMessage message)
		{
			byte[] bytes = message.GetBytes();

			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient client = this.Clients[i];
				if (client != null)
				{
                    try
                    {
                        client.GetConnection().SendData(bytes);
                    }
                    catch { }
				}
			}
		}
		internal void SendToHotel(ServerMessage Message5_0, ServerMessage Message5_1)
		{
			byte[] byte_ = Message5_0.GetBytes();
			byte[] byte_2 = Message5_1.GetBytes();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
                        if (@class.GetHabbo().InRoom)
						{
							@class.GetConnection().SendData(byte_);
						}
						else
						{
							@class.GetConnection().SendData(byte_2);
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void SendToStaffs(ServerMessage Message5_0, ServerMessage Message5_1)
		{
			byte[] byte_ = Message5_0.GetBytes();
			byte[] byte_2 = Message5_1.GetBytes();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						if (@class.GetHabbo().HasFuse("receive_sa"))
						{
                            if (@class.GetHabbo().InRoom)
							{
								@class.GetConnection().SendData(byte_);
							}
							else
							{
								@class.GetConnection().SendData(byte_2);
							}
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void SendToStaffs(GameClient class16_1, ServerMessage Message5_0, bool IsAntiAd = true)
		{
			byte[] byte_ = Message5_0.GetBytes();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && (@class != class16_1 || IsAntiAd))
				{
					try
					{
						if (@class.GetHabbo().HasFuse("receive_sa"))
						{
							@class.GetConnection().SendData(byte_);
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void GiveCredits(int int_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
                    try
                    {
                        long NoBug = 0;
                        NoBug += @class.GetHabbo().GetCredits();
                        NoBug += int_0;
                        if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                        {
                            @class.GetHabbo().GiveCredits(int_0, "Clientwide Credits");
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("Du hast " + int_0 + " Taler erhalten!");
                        }
                        else
                        {
                            if (int_0 > 0)
                            {
                                @class.GetHabbo().SetCredits(2147483647, "Reached Maximum");
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("Du hast die maximale Anzahl Taler erreicht!");
                            }
                            else if (int_0 < 0)
                            {
                                @class.GetHabbo().SetCredits(-2147483648, "Reached Minimum");
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("Du hast die minimale Anzahl Taler erreicht!");
                            }
                        }
                    }
                    catch
                    {
                    }
				}
			}
		}
		internal void GivePixels(int int_0, bool bool_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
                    try
                    {
                        long NoBug = 0;
                        NoBug += @class.GetHabbo().ActivityPoints;
                        NoBug += int_0;
                        if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                        {
                            @class.GetHabbo().ActivityPoints += int_0;
                            @class.GetHabbo().UpdateActivityPoints(bool_0);
                            @class.SendNotification("Du hast " + int_0 + " Pixel erhalten!");
                        }
                        else
                        {
                            if (int_0 > 0)
                            {
                                @class.GetHabbo().ActivityPoints = 2147483647;
                                @class.GetHabbo().UpdateActivityPoints(bool_0);
                                @class.SendNotification("Du hast gerade die maximale Anzahl Pixel erreicht!");
                            }
                            else if (int_0 < 0)
                            {
                                @class.GetHabbo().ActivityPoints = -2147483648;
                                @class.GetHabbo().UpdateActivityPoints(bool_0);
                                @class.SendNotification("Du hast die minimale Anzahl Pixel erreicht!");
                            }
                        }
                    }
                    catch
                    {
                    }
				}
			}
		}
		internal void GivePoints(int int_0, bool bool_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
                    try
                    {
                        long NoBug = 0;
                        NoBug += @class.GetHabbo().VipPoints;
                        NoBug += int_0;
                        if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                        {
                            @class.GetHabbo().VipPoints += int_0;
                            @class.GetHabbo().UpdateVipPoints(false, bool_0);
                            @class.SendNotification("Du hast " + int_0 + " VIP Punkte erhalten.");
                        }
                        else
                        {
                            if (int_0 > 0)
                            {
                                @class.GetHabbo().VipPoints = 2147483647;
                                @class.GetHabbo().UpdateVipPoints(false, bool_0);
                                @class.SendNotification("Du hast die minimale Anzahl VIP Punkte erreicht!");
                            }
                            else if (int_0 < 0)
                            {
                                @class.GetHabbo().VipPoints = -2147483648;
                                @class.GetHabbo().UpdateVipPoints(false, bool_0);
                                @class.SendNotification("Du hast die minimale Anzahl VIP Punkte erreicht!");
                            }
                        }
                    }
                    catch
                    {
                    }
				}
			}
		}
		internal void GiveBadge(string string_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
					try
					{
						@class.GetHabbo().GetBadgeComponent().SendBadge(@class, string_0, true);
						@class.SendNotification("Du hast ein Badge erhalten!");
					}
					catch
					{
					}
				}
			}
		}
		public void SendToClientsWithFuse(ServerMessage Message5_0, string string_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						if (string_0.Length <= 0 || (@class.GetHabbo() != null && @class.GetHabbo().HasFuse(string_0)))
						{
							@class.SendMessage(Message5_0);
						}
					}
					catch
					{
					}
				}
			}
		}
		public void UpdateEffects()
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && (@class.GetHabbo() != null && @class.GetHabbo().GetEffectsInventoryComponent() != null))
				{
					@class.GetHabbo().GetEffectsInventoryComponent().method_7();
				}
			}
		}
		internal void CloseAll()
		{
			StringBuilder stringBuilder = new StringBuilder();

			bool flag = false;

			using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
				for (int i = 0; i < Clients.Length; i++)
				{
					GameClient client = Clients[i];

					if (client != null && client.GetHabbo() != null)
					{
                        try
                        {
                            client.GetHabbo().GetInventoryComponent().SavePets(dbClient, true);
                            stringBuilder.Append(client.GetHabbo().UpdateQuery);
                            flag = true;
                        }
                        catch { }
					}
				}

				if (flag)
				{
					try
					{
						dbClient.ExecuteQuery(stringBuilder.ToString());
					}
					catch (Exception ex)
					{
						Logging.HandleException(ex.ToString());
					}
				}
			}

			Console.WriteLine("Done saving users inventory!");
			Console.WriteLine("Closing server connections...");

			try
			{
				for (int i = 0; i < this.Clients.Length; i++)
				{
					GameClient class2 = this.Clients[i];
					if (class2 != null && class2.GetConnection() != null)
					{
						try
						{
							class2.GetConnection().Close();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logging.HandleException(ex.ToString());
			}

			Array.Clear(this.Clients, 0, this.Clients.Length);

			Console.WriteLine("Connections closed!");
		}
		public void Disconnect(uint uint_0, string Reason)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().Id == uint_0)
				{
					@class.Disconnect(Reason);
				}
			}
		}
		public string GetNameById(uint uint_0)
		{
			GameClient @class = this.GetClient(uint_0);
			string result;
			if (@class != null)
			{
				result = @class.GetHabbo().Username;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
				{
					dataRow = class2.ReadDataRow("SELECT username FROM users WHERE Id = '" + uint_0 + "' LIMIT 1");
				}
				if (dataRow == null)
				{
					result = "Unknown User";
				}
				else
				{
					result = (string)dataRow[0];
				}
			}
			return result;
		}
        public string GetDataById(uint uint_0, string data)
        {
            string result = "";

            if (data != "gender" || data != "look")
            {
                return result;
            }

            GameClient @class = this.GetClient(uint_0);
            if (@class != null)
            {
                if (data == "gender")
                {
                    result = @class.GetHabbo().Gender;
                }
                else if (data == "look")
                {
                    result = @class.GetHabbo().Figure;
                }
            }
            else
            {
                DataRow dataRow = null;
                using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                {
                    dataRow = class2.ReadDataRow("SELECT " + data + " FROM users WHERE Id = '" + uint_0 + "' LIMIT 1");
                }
                if (dataRow == null)
                {
                    result = "Unknown data";
                }
                else
                {
                    result = (string)dataRow[0];
                }
            }
            return result;
        }
		public uint GetIdByName(string string_0)
		{
			GameClient @class = this.GetClientByHabbo(string_0);
			uint result;
			if (@class != null && @class.GetHabbo() != null)
			{
				result = @class.GetHabbo().Id;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
				{
                    class2.AddParamWithValue("username", string_0);
					dataRow = class2.ReadDataRow("SELECT Id FROM users WHERE username = @username LIMIT 1");
				}
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
		public void UpdateBans()
		{
			Dictionary<GameClient, ModerationBanException> dictionary = new Dictionary<GameClient, ModerationBanException>();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						Essential.GetGame().GetBanManager().method_1(@class);
					}
					catch (ModerationBanException value)
					{
						dictionary.Add(@class, value);
					}
				}
			}
			foreach (KeyValuePair<GameClient, ModerationBanException> current in dictionary)
			{
				current.Key.NotifyBan(current.Value.Message);
				current.Key.Disconnect("Banned!");
			}
		}
        public void CheckPixelUpdates()
        {
            try
            {
                if (this.Clients != null)
                {
                    for (int i = 0; i < this.Clients.Length; i++)
                    {
                        GameClient @class = this.Clients[i];
                        if (@class != null && (@class.GetHabbo() != null && Essential.GetGame().GetPixelManager().CanHaveReward(@class)))
                        {
                            Essential.GetGame().GetPixelManager().UpdateNeeded(@class);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), "GCMExt.CheckPixelUpdates task");
            }
        }
        internal List<GameClient> GetStaffs()
        {
            List<GameClient> staffs = new List<GameClient>();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient gc = this.Clients[i];
                if (gc != null && gc.GetHabbo() != null && gc.GetHabbo().Rank >= 4)
                {
                    staffs.Add(gc);
                }
            }
            return staffs;
        }
        internal void HotelSummon(uint roomid, bool bool0)
        {
            try
            {
                for (int i = 0; i < this.Clients.Length; i++)
                {
                    GameClient @class = this.Clients[i];
                    if (@class != null)
                    {
                        if (@class != null && roomid != @class.GetHabbo().CurrentRoomId)
                        {
                            @class.LoadRoom(roomid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
		internal void StoreCommand(GameClient class16_1, string string_0, string string_1)
		{
            if (ServerConfiguration.EnableCommandLog)
			{
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("extra_data", string_1);
                    @class.AddParamWithValue("username", class16_1.GetHabbo().Username);
                    @class.AddParamWithValue("cmd", string_0);
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO cmdlogs (user_id,user_name,command,extra_data,timestamp) VALUES ('",
						class16_1.GetHabbo().Id,
						"',@username,@cmd, @extra_data, UNIX_TIMESTAMP())"
					}));
				}
			}
        }
        public List<String> OnlineStaffs(int minrank)
        {
            List<String> Staffs = new List<String>();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient gc = this.Clients[i];
                if (gc != null && gc.GetHabbo().Rank >= minrank)
                {
                    Staffs.Add(gc.GetHabbo().Username);
                }
            }
            return Staffs;
        }

    }
}
