using System;
using System.Data;
using System.Text.RegularExpressions;
using Essential.Core;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.Users.UserDataManagement;
using Essential.HabboHotel.Support;
using Essential.Messages;
using Essential.Util;
using Essential.HabboHotel.Users;
using Essential.Net;
using Essential.HabboHotel.Users.Authenticator;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
using HabboEncryption;
using System.Text;
namespace Essential.HabboHotel.GameClients
{
	internal sealed class GameClient
	{
		private uint Id;
        internal bool CDPolicySent = false;
        private SocketConnection Connection;
        private GameClientMessageHandler ClientMessageHandler;
		private Habbo Habbo;
        internal int Basejump_UserId = 0;
        internal int Basejump_Missiles = 0;
        internal int Basejump_Shields = 0;
        internal int Basejump_Bigparachutes = 0;
        internal int Basejump_LobbyId = 0;
		public bool bool_0;
		internal bool bool_1 = false;
		private bool bool_2 = false;
        internal int DesignedHandler = 1;
        internal int i = 0;
        internal int[] table = new int[0x100];
        internal int j = 0;
        internal bool CryptoInitialized = false;
        public string ClientVar = "https://swf.habbo.tl";
        public string MachineId = "";
        public bool IsMobileUser = false;
		public uint ID
		{
			get
			{
				return this.Id;
			}
		}
		public bool LoggedIn
        {
            get
            {
                return this.Habbo != null;
            }
        }
        public GameClient(uint id, ref SocketConnection connection)
        {
            this.Id = id;
            this.Connection = connection;
            this.CreateClientMessageHandler();
        }
        public SocketConnection GetConnection()
        {
            return this.Connection;
        }
		public GameClientMessageHandler GetClientMessageHandler()
		{
			return this.ClientMessageHandler;
		}
		public Habbo GetHabbo()
		{
			return this.Habbo;
		}
		public void GetSocketConnection()
		{
			if (this.Connection != null)
			{
                this.bool_0 = true;

                SocketConnection.RouteReceivedDataCallback dataRouter = new SocketConnection.RouteReceivedDataCallback(this.ParsePacket);
                this.Connection.Start(dataRouter);
			}
		}
		internal void CreateClientMessageHandler()
		{
			this.ClientMessageHandler = new GameClientMessageHandler(this);
		}
        internal void tryLogin(string AuthTicket)
		{
            try
            {
                UserDataFactory @class = new UserDataFactory(AuthTicket, this.GetConnection().String_0, true);
                if (!@class.Validated)
                {
                    @class = new UserDataFactory(AuthTicket,this.GetConnection().String_0, true);
                }
                if (!@class.Validated)
                {
                    string str = "";
                    if (ServerConfiguration.EnableSSO)
                    {
                        str = EssentialEnvironment.GetExternalText("emu_sso_wrong_secure") + "(" + this.GetConnection().String_0 + ")";
                    }
                    SendNotifWithScroll(EssentialEnvironment.GetExternalText("emu_sso_wrong") + str);
                    return;
                }
                Habbo class2 = Authenticator.CreateHabbo(AuthTicket, this, @class, @class);
                Essential.GetGame().GetClientManager().Disconnect(class2.Id,"New Session");
                this.Habbo = class2;
                this.Habbo.method_2(@class);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logging login error because you are on alpha test!");
                Logging.LogException(ex.ToString());
                if (this != null)
                {
                    this.SendNotification(ex.ToString());
                    this.Disconnect("Login Error");
                }
                return;
            }

			try
			{
				Essential.GetGame().GetBanManager().method_1(this);
			}
			catch (ModerationBanException gException)
			{
				this.NotifyBan(gException.Message);
				this.Disconnect("Banned!");
				return;
            }
            #region "AKS"
            if (this.MachineId != "" && !Essential.StringToBoolean(this.GetHabbo().GetUserDataFactory().GetUserData()["staff_inacc"].ToString()))
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    string oldMachineId = dbClient.SpecialString("SELECT machineId FROM user_machineids WHERE userid=" + this.GetHabbo().Id);

                    dbClient.AddParamWithValue("machineid", this.MachineId);
                    if ((oldMachineId == "" || oldMachineId != this.MachineId))
                    {
                        dbClient.AddParamWithValue("username", this.GetHabbo().Username);
                        dbClient.ExecuteQuery("INSERT INTO user_machineids (username,userid,timestamp,machineId) VALUES (@username," + this.GetHabbo().Id + ",'" + Convert.ToInt32(Essential.GetUnixTimestamp()) + "',@machineid)");
                    }
                   
                }
            }
            #endregion
            ServerMessage Message2 = new ServerMessage(Outgoing.Fuserights);
            if (this == null || this.GetHabbo() == null)
                return;
			if (this.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes)
			{
				Message2.AppendInt32(2);
			}
			else
			{
                if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
				{
					Message2.AppendInt32(1);
				}
				else
				{
					Message2.AppendInt32(0);
				}
			}
			if (this.GetHabbo().HasFuse("acc_anyroomowner"))
			{
				Message2.AppendInt32(7);
			}
			else
			{
				if (this.GetHabbo().HasFuse("acc_anyroomrights"))
				{
					Message2.AppendInt32(5);
				}
				else
				{
					if (this.GetHabbo().HasFuse("acc_supporttool"))
					{
						Message2.AppendInt32(4);
					}
					else
					{
                        if (this.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes || this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
						{
							Message2.AppendInt32(2);
						}
						else
						{
							Message2.AppendInt32(0);
						}
					}
				}
			}

			this.SendMessage(Message2);

            this.SendMessage(this.GetHabbo().GetEffectsInventoryComponent().method_6());
            
            ServerMessage Message3 = new ServerMessage(Outgoing.AvailabilityStatus);
			Message3.AppendBoolean(true);
			Message3.AppendBoolean(false);
			this.SendMessage(Message3);
            ServerMessage message = new ServerMessage(Outgoing.UniqueID);
            message.AppendString(this.MachineId);
            this.SendMessage(message);
            ServerMessage Message5_ = new ServerMessage(Outgoing.AuthenticationOK);
		    this.SendMessage(Message5_);
            
            if (GetHabbo().HomeRoomId <= 0)
                GetHabbo().HomeRoomId = 64259;
            ServerMessage Message5 = new ServerMessage(Outgoing.HomeRoom);
            Message5.AppendUInt(this.GetHabbo().HomeRoomId);
            Message5.AppendUInt(this.GetHabbo().HomeRoomId);
            this.SendMessage(Message5);
            Console.WriteLine("[" + this.GetConnection().UInt32_0 + "] [" + this.GetHabbo().Username + "] [" + this.GetHabbo().Id + "] [" + this.GetConnection().String_0 + "] [" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "]");
            GetHabbo().UpdateCredits(false);
            GetHabbo().UpdateActivityPoints(false);
            if (this.GetHabbo().HasFuse("acc_supporttool"))
            {
                this.SendMessage(Essential.GetGame().GetModerationTool().SerializeTool());
             Essential.GetGame().GetModerationTool().method_4(this);
            }
			if (Essential.GetGame().GetPixelManager().CanHaveReward(this))
			{
				Essential.GetGame().GetPixelManager().UpdateNeeded(this);
			}

            ServerMessage Message6 = new ServerMessage(Outgoing.FavouriteRooms);
			Message6.AppendInt32(30);
			Message6.AppendInt32(this.GetHabbo().list_1.Count);
			foreach (uint current in this.GetHabbo().list_1)
			{
				Message6.AppendUInt(current);
			}
			this.SendMessage(Message6);
            try
            {
                this.GetHabbo().CheckTotalTimeOnlineAchievements();
                this.GetHabbo().CheckHappyHourAchievements();
                this.GetHabbo().CheckTrueHabboAchievements();
                this.GetHabbo().CheckRegularVisitorAchievements();
                this.GetHabbo().CheckFootballGoalHostScoreAchievements();
                this.GetHabbo().CheckStaffPicksAchievement();
            }
            catch { }
			if (ServerConfiguration.MOTD != "")
			{
				this.SendNotification(ServerConfiguration.MOTD, 2);
			}
            for (uint num = (uint)Essential.GetGame().GetRoleManager().GetRankCount(); num > 1u; num -= 1u)
			{
                if (Essential.GetGame().GetRoleManager().GetBadgeByRank(num).Length > 0)
				{
                    if (!this.GetHabbo().GetBadgeComponent().HasBadge(Essential.GetGame().GetRoleManager().GetBadgeByRank(num)) && this.GetHabbo().Rank == num)
					{
                        this.GetHabbo().GetBadgeComponent().SendBadge(this, Essential.GetGame().GetRoleManager().GetBadgeByRank(num), true);
					}
					else
					{
                        if (this.GetHabbo().GetBadgeComponent().HasBadge(Essential.GetGame().GetRoleManager().GetBadgeByRank(num)) && this.GetHabbo().Rank < num)
						{
                            this.GetHabbo().GetBadgeComponent().RemoveBadge(Essential.GetGame().GetRoleManager().GetBadgeByRank(num));
						}
					}
				}
			}
            if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
			{
                this.GetHabbo().CheckHCAchievements();
			}
			if (this.GetHabbo().IsVIP && !this.GetHabbo().GetBadgeComponent().HasBadge("VIP"))
			{
				this.GetHabbo().GetBadgeComponent().SendBadge(this, "VIP", true);
			}
			else
			{
				if (!this.GetHabbo().IsVIP && this.GetHabbo().GetBadgeComponent().HasBadge("VIP"))
				{
					this.GetHabbo().GetBadgeComponent().RemoveBadge("VIP");
				}
			}
			if (this.GetHabbo().CurrentQuestId > 0u)
			{
				Essential.GetGame().GetQuestManager().ActivateQuest(this.GetHabbo().CurrentQuestId, this);
			}
			if (!Regex.IsMatch(this.GetHabbo().Username, "^[-a-zA-Z0-9._:,]+$"))
			{
                ServerMessage Message5_2 = new ServerMessage(Outgoing.Disconnect);
				this.SendMessage(Message5_2);
			}
			this.GetHabbo().Motto = Essential.FilterString(this.GetHabbo().Motto);
			DataTable dataTable = null;
			using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
			{
				dataTable = class3.ReadDataTable("SELECT achievement,achlevel FROM achievements_owed WHERE user = '" + this.GetHabbo().Id + "'");
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					Essential.GetGame().GetAchievementManager().addAchievement(this, (uint)dataRow["achievement"], (int)dataRow["achlevel"]);
					using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
					{
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"DELETE FROM achievements_owed WHERE achievement = '",
							(uint)dataRow["achievement"],
							"' AND user = '",
							this.GetHabbo().Id,
							"' LIMIT 1"
						}));
					}
				}
			}
            if(this.GetHabbo().list_2 != null && this.GetHabbo().list_2.Count > 0)
            {
                ServerMessage IgnoringList = new ServerMessage(Outgoing.IgnoringList);
                IgnoringList.AppendInt32(this.GetHabbo().list_2.Count);
                foreach(uint userId in this.GetHabbo().list_2)
                {
                    IgnoringList.AppendString(Essential.GetGame().GetClientManager().GetNameById(userId));
                }
                this.SendMessage(IgnoringList);
            }
            //this.GetClientMessageHandler().method_5(this.GetHabbo().HomeRoomId, "");
		}
		public void NotifyBan(string reason)
		{
            this.SendNotifWithScroll(reason);
		}
		public void SendNotification(string Message)
		{
			this.SendNotification(Message, 0);
		}
        public void Whisper(string message)
        {
            GetHabbo().Whisper(message);
        }
		public void SendNotification(string message, int int_0)
		{
            if (this != null && this.GetConnection() != null)
            {
                ServerMessage nMessage = new ServerMessage();
                switch (int_0)
                {
                    case 1:
                        nMessage.Init(Outgoing.BroadcastMessage);
                        nMessage.AppendStringWithBreak(message);
                        nMessage.AppendStringWithBreak("");
                        break;
                    case 2:
                        nMessage.Init(Outgoing.SetCommandsView);
                        nMessage.AppendUInt(1);
                        nMessage.AppendStringWithBreak(message);
                        break;
                    default:
                        nMessage.Init(Outgoing.BroadcastMessage);
                        nMessage.AppendStringWithBreak(message);
                        nMessage.AppendStringWithBreak("");
                        break;
                }
                this.GetConnection().SendMessage(nMessage);
            }
		}
        internal void SendNotifWithScroll(string message)
        {
            ServerMessage message2 = new ServerMessage(Outgoing.SetCommandsView);
            message2.AppendInt32(1);
            message2.AppendString(message);
            this.SendMessage(message2);
        }
		public void AboutMessage(string string_0, string string_1)
		{
            ServerMessage Message = new ServerMessage(Outgoing.BroadcastMessage); // Update
			Message.AppendStringWithBreak(string_0);
			Message.AppendStringWithBreak(string_1);
			this.GetConnection().SendMessage(Message);
		}
		public void Dispose()
		{
			if (this.Connection != null)
			{
				this.Connection.Close();
				this.Connection = null;
			}
			if (this.GetHabbo() != null)
			{
				this.Habbo.Dispose();
				this.Habbo = null;
			}
			if (this.GetClientMessageHandler() != null)
			{
				this.ClientMessageHandler.Destroy();
				this.ClientMessageHandler = null;
			}
		}
		public void Disconnect(string Reason)
		{
			if (!this.bool_2)
			{
                Console.WriteLine("[DISCONNECT] [" + this.GetHabbo().Username + "] [" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "] REASON: " + Reason);
				Essential.GetGame().GetClientManager().Dispose(this.Id, false);
				this.bool_2 = true;
			}
		}
		public void ParsePacket(ref byte[] bytes)
		{
            //Console.WriteLine(Essential.GetDefaultEncoding().GetString(bytes));
            string Packet = Essential.GetDefaultEncoding().GetString(bytes);
            if (Essential.GetDefaultEncoding().GetString(bytes) == "<policy-file-request/>" + (char)0) 
            {
                this.Connection.SendMessage(CrossdomainPolicy.GetXmlPolicy());
            }else if(Packet.StartsWith("GET")|| Packet.StartsWith("POST"))
            {
                Essential.GetWebManager().HandleRequest(Packet, this.GetConnection());
            }
            /*if(Packet.StartsWith("imaphone"))
            {
                //Essential.GetMobileHandler().HandleRequest(Packet, this);
            }*/
            int index = 0;
            while (index < bytes.Length)
            {
                try
                {
                    // I tried to make a mobile Version of Habbo, but didn't continue it. If you want I could continue the developing of Essential Mobile
                    if (!Packet.StartsWith("mobile"))
                    {
                        #region "Normal Part"
                        int MessageLength = HabboEncoding.DecodeInt32(new byte[] { bytes[index++], bytes[index++], bytes[index++], bytes[index++] });
                        if (MessageLength < 2 || MessageLength > 1024)
                        {
                            //Console.WriteLine("bad size packet!");
                            continue;
                        }
                        int MessageId = HabboEncoding.DecodeInt16(new byte[] { bytes[index++], bytes[index++] });
                        byte[] Content = new byte[MessageLength - 2];
                        for (int i = 0; i < Content.Length && index < bytes.Length; i++)
                        {
                            Content[i] = bytes[index++];
                        }
                        if (MessageId == 1615)
                        {
                            return;
                        }
                        Interface messageInterface;
                        ClientMessage cMessage = new ClientMessage((uint)MessageId, Content);
                        if (cMessage != null)
                        {
                            if (Essential.GetPacketManager().Handle((uint)MessageId, out messageInterface))
                            {
                                try
                                {
                                   /* Logging.WriteLine(string.Concat(new object[]
                                    {
                                        "[INCOMING] ",
                                        "[",
                                        messageInterface.GetType().Name.ToString(),
                                        "] --> [",
                                        cMessage.Id,
                                        "] ",
                                        cMessage.ToString()
                                    }));*/
                                    messageInterface.Handle(this, cMessage);
                                }
                                catch (Exception ex)
                                {
                                    Logging.LogException("Error: " + ex.ToString());
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region "Mobile Part"
                        if (Packet.Length < 2 || Packet.Length > 1024)
                            continue;
                        this.IsMobileUser = true;
                        this.GetConnection().IsMobileUser = true;
                        Interface messageInterface;
                        uint MessageId = uint.Parse(Packet.Split((char)1)[1]);
                        ClientMessage cMessage = new ClientMessage((uint)MessageId, null, true, Packet);
                        if (cMessage != null)
                        {
                            if (Essential.GetPacketManager().Handle((uint)MessageId, out messageInterface))
                            {
                                try
                                {
                                  /*  Logging.WriteLine(string.Concat(new object[]
                                    {
                                        "[INCOMING] ",
                                        "[",
                                        messageInterface.GetType().Name.ToString(),
                                        "] --> [",
                                        cMessage.Id,
                                        "] ",
                                        cMessage.ToString()
                                    }));*/
                                    messageInterface.Handle(this, cMessage);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Logging.LogException("Error: " + ex.ToString());
                                }
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(IndexOutOfRangeException)) return;
                    if (e.GetType() == typeof(NullReferenceException)) return;

                    Logging.LogException("Error: " + e.ToString());
                    ServerMessage ServerError = new ServerMessage(Outgoing.ServerError);
                    ServerError.AppendInt32(1);
                    ServerError.AppendInt32(1);
                    ServerError.AppendString(DateTime.Now.ToShortDateString().ToString());
                    this.SendMessage(ServerError);
                }
            }
		}
        public void LoadRoom(uint roomid)
        {
            Room room = Essential.GetGame().GetRoomManager().GetRoom(roomid);
            if (room != null)
            {
                ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                Message.AppendBoolean(room.IsPublic);
                Message.AppendUInt(room.Id);
                this.SendMessage(Message);

                ServerMessage Message7 = new ServerMessage(Outgoing.RoomData); // Updated
                Message7.AppendBoolean(false);
                Message7.AppendInt32(room.Id);
                Message7.AppendString(room.Name);
                Message7.AppendBoolean(true);
                Message7.AppendInt32(room.OwnerId);
                Message7.AppendString(room.Owner);
                Message7.AppendInt32(room.State);
                Message7.AppendInt32(room.UsersNow);
                Message7.AppendInt32(room.UsersMax);
                Message7.AppendString(room.Description);
                Message7.AppendInt32(0);
                Message7.AppendInt32((room.Category == 0x34) ? 2 : 0);
                Message7.AppendInt32(room.Score);
                Message7.AppendInt32(0);
                Message7.AppendInt32(room.Category);
                if (room.RoomData.GuildId == 0)
                {
                    Message7.AppendInt32(0);
                    Message7.AppendInt32(0);
                }
                else
                {
                    GroupsManager guild = Groups.GetGroupById(room.RoomData.GuildId);
                    Message7.AppendInt32(guild.Id);
                    Message7.AppendString(guild.Name);
                    Message7.AppendString(guild.Badge);
                }
                Message7.AppendString("");
                Message7.AppendInt32(room.Tags.Count);
                foreach (string str in room.Tags)
                {
                    Message7.AppendString(str);
                }
                Message7.AppendInt32(0);
                Message7.AppendInt32(0);
                Message7.AppendInt32(0);
                Message7.AppendBoolean(true);
                Message7.AppendBoolean(true);
                Message7.AppendInt32(0);
                Message7.AppendInt32(0);
                Message7.AppendBoolean(false);
                Message7.AppendBoolean(false);
                Message7.AppendBoolean(false);
                Message7.AppendInt32(0);
                Message7.AppendInt32(0);
                Message7.AppendInt32(0);
                Message7.AppendBoolean(false);
                Message7.AppendBoolean(true);
                this.SendMessage(Message7);
            }
            
        }
		public void SendMessage(ServerMessage Message5_0)
		{
			if (Message5_0 != null && this.GetConnection() != null)
			{
                //Console.WriteLine("[OUTGOING] [" + Message5_0.Id + "]"+ " " + Message5_0.ToString());
                if (!this.IsMobileUser)
                {
                    this.GetConnection().SendMessage(Message5_0);
                }
                else
                {
                    Console.WriteLine("[OUTGOING] [" + Message5_0.Id + "]" + " " + Message5_0.GetMobileString());
                    this.GetConnection().Send(Encoding.Default.GetBytes(Message5_0.GetMobileString()));
                }
			}
		}
	}
}
