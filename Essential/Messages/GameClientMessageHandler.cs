using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Games.SnowWar;
using Essential.HabboHotel.Users;
using System.Threading;
using System.Threading.Tasks;
namespace Essential.Messages
{
	internal sealed class GameClientMessageHandler
	{
		private delegate void Delegate();
		private const int HIGHEST_MESSAGE_ID = 4004;
		private GameClient Session;
		private ClientMessage Request;
		private ServerMessage Response;
		private GameClientMessageHandler.Delegate[] RequestHandlers;
		public GameClientMessageHandler(GameClient Session)
		{
			this.Session = Session;
            this.RequestHandlers = new GameClientMessageHandler.Delegate[HIGHEST_MESSAGE_ID];
			this.Response = new ServerMessage(0);
		}
		public ServerMessage GetResponse()
		{
			return this.Response;
		}
		public void Destroy()
		{
			this.Session = null;
			this.RequestHandlers = null;
			this.Request = null;
			this.Response = null;
		}
		public void HandleRequest(ClientMessage Request)
		{
			uint arg_06_0 = Request.Id;
            if (Request.Id > HIGHEST_MESSAGE_ID)
			{
				Logging.WriteLine("Warning - out of protocol request: " + Request.Header);
			}
			else
			{
				if (this.RequestHandlers[(int)((UIntPtr)Request.Id)] != null && Request != null)
				{
					this.Request = Request;
					this.RequestHandlers[(int)((UIntPtr)Request.Id)]();
					this.Request = null;
				}
			}
		}
		public void method_3()
		{
			if (this.Response != null && this.Response.Id > 0u && this.Session.GetConnection() != null)
			{
				this.Session.GetConnection().SendMessage(this.Response);
			}
           //this.GoToRoom();
		}
		public void method_4()
		{
            /*if (this != null)
            {
                RoomAdvertisement @class = Essential.GetGame().GetAdvertisementManager().method_1();
                this.Response.Init(901); // Updated
                if (@class == null)
                {
                    this.Response.AppendStringWithBreak("");
                    this.Response.AppendStringWithBreak("");
                }
                else
                {
                    this.Response.AppendStringWithBreak(@class.string_0);
                    this.Response.AppendStringWithBreak(@class.string_1);
                    this.Response.AppendStringWithBreak("");
                    @class.method_0();
                }
                this.method_3();
            }*/
		}
        public void GoToRoom()
        {
        
                RoomData @class = Essential.GetGame().GetRoomManager().method_12(Session.GetHabbo().uint_2);
				if (@class != null)
				{
				
						Room class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);
						if (class2 != null)
						{
							Session.GetClientMessageHandler().method_7();
							if (class2.Type == "public")
                            {
                                if (class2.RoomModel.string_2.Contains("|"))
                                {
                                    ServerMessage Message = new ServerMessage(Outgoing.RoomModel); // Updated
                                    Message.AppendInt32(class2.RoomModel.string_2.Split('|').Length);
                                    foreach (string PublicIt in class2.RoomModel.string_2.Split('|'))
                                    {
                                        Message.AppendInt32(0);
                                        Message.AppendString(PublicIt.Split(' ')[0]);
                                        Message.AppendString(PublicIt.Split(' ')[1]);
                                        Message.AppendInt32(int.Parse(PublicIt.Split(' ')[2]));
                                        Message.AppendInt32(int.Parse(PublicIt.Split(' ')[3]));
                                        Message.AppendInt32(int.Parse(PublicIt.Split(' ')[4]));
                                        Message.AppendInt32(int.Parse(PublicIt.Split(' ')[5]));
                                    }
                                    Session.SendMessage(Message);
                                }
                                

                             /*   ServerMessage Message6 = new ServerMessage(2877); // P
                                Message6.AppendBoolean(false);
                                Message6.AppendString(class2.ModelName);
                                Message6.AppendUInt(0);

                                Message6.AppendBoolean(false);
                                Session.SendMessage(Message6);
                                */

                            }
							if (class2.Type == "private")
							{
								Hashtable hashtable_ = class2.Hashtable_0;
								Hashtable hashtable_2 = class2.Hashtable_1;
                                ServerMessage Message2 = new ServerMessage(Outgoing.SerializeFloorItems); // P
                                Message2.AppendInt32(1);
                                if (class2.HideOwner)
                                {
                                    Message2.AppendInt32(0);
                                    Message2.AppendString("Öffentlicher Raum");
                                }
                                else
                                {
                                    Message2.AppendInt32(class2.OwnerId);
                                    Message2.AppendString(class2.Owner);
                                }
								Message2.AppendInt32(hashtable_.Count);
								foreach (RoomItem class3 in hashtable_.Values)
								{
                                    class3.Serialize(Message2);
								}
                       
								Session.SendMessage(Message2);
                                ServerMessage Message3 = new ServerMessage(Outgoing.SerializeWallItems); // P
                                Message3.AppendInt32(1);
                                if (class2.HideOwner)
                                {
                                    Message3.AppendInt32(0);
                                    Message3.AppendString("Öffentlicher Raum");
                                }
                                else
                                {
                                    Message3.AppendInt32(class2.OwnerId);
                                    Message3.AppendString(class2.Owner);
                                }
                               
								Message3.AppendInt32(hashtable_2.Count);
								foreach (RoomItem class3 in hashtable_2.Values)
								{
                                    class3.Serialize(Message3);
								}
								Session.SendMessage(Message3);
                           
							}
							class2.method_46(Session, Session.GetHabbo().bool_8);
							List<RoomUser> list = new List<RoomUser>();
							for (int i = 0; i < class2.RoomUsers.Length; i++)
							{
								RoomUser class4 = class2.RoomUsers[i];
								if (class4 != null && (!class4.bool_11 && class4.bool_12))
								{
									list.Add(class4);
								}
							}
                            ServerMessage Message4 = new ServerMessage(Outgoing.SetRoomUser); // P
							Message4.AppendInt32(list.Count);
							foreach (RoomUser class4 in list)
							{
								class4.method_14(Message4);
							}
							Session.SendMessage(Message4);

                            if (class2.Type == "private")
                            {
                                ServerMessage Message5 = new ServerMessage(Outgoing.ConfigureWallandFloor); // P
                                Message5.AppendBoolean(class2.Hidewall);
                                Message5.AppendInt32(class2.Wallthick);
                                Message5.AppendInt32(class2.Floorthick);
                                Session.SendMessage(Message5);
                            }
                         
							if (class2.Type == "public")
							{
                          /*  ServerMessage Message6 = new ServerMessage(2877); // P
								Message6.AppendBoolean(false);
                                Message6.AppendUInt(class2.Id);
							   Message6.AppendStringWithBreak(class2.ModelName);
								Message6.AppendBoolean(false);
								Session.SendMessage(Message6);
                            */
                                
							}
							else
							{
								if (class2.Type == "private")
								{

                                    ServerMessage Message6 = new ServerMessage(Outgoing.RoomEntryInfo); // P
									Message6.AppendBoolean(true);
									Message6.AppendUInt(class2.Id);
                                    if (class2.CheckRights(Session, true))
									{
										Message6.AppendBoolean(true);
									}
									else
									{
										Message6.AppendBoolean(false);
									}
									Session.SendMessage(Message6);
                                    ServerMessage Message7 = new ServerMessage(Outgoing.GetGuestRoomResult); // P
									Message7.AppendBoolean(true);
                                    @class.Serialize(Message7, false, false);
                                    Message7.AppendString(string.Empty);
                                    Message7.AppendBoolean(false);
									Session.SendMessage(Message7);

                                   /* if (class2.UsersWithRights.Count > 0 && class2.CheckRights(Session, true))
                                    {
                                        ServerMessage LoadUsersWithRights = new ServerMessage(658);
                                        LoadUsersWithRights.AppendUInt(class2.Id);
                                        LoadUsersWithRights.AppendInt32(class2.UsersWithRights.Count);
                                        foreach (uint current2 in class2.UsersWithRights)
                                        {
                                            LoadUsersWithRights.AppendUInt(current2);
                                            LoadUsersWithRights.AppendString(Essential.GetGame().GetClientManager().GetNameById(current2));
                                        }
                                        Session.SendMessage(LoadUsersWithRights);

                                        foreach (uint current3 in class2.UsersWithRights)
                                        {
                                            ServerMessage serverMessage = new ServerMessage(212); // Updated
                                            serverMessage.AppendUInt(class2.Id);
                                            serverMessage.AppendUInt(current3);
                                            serverMessage.AppendStringWithBreak(Essential.GetGame().GetClientManager().GetNameById(current3));
                                            Session.SendMessage(serverMessage);
                                        }
                                    }
                                    */
								}
							}
							ServerMessage Message8 = class2.method_67(true);
							if (Message8 != null)
							{
								Session.SendMessage(Message8);
							}
							for (int i = 0; i < class2.RoomUsers.Length; i++)
							{
								RoomUser class4 = class2.RoomUsers[i];
								if (class4 != null && !class4.bool_11)
								{
									if (class4.IsDancing)
									{
                                        ServerMessage Message9 = new ServerMessage(Outgoing.Dance); // 480 Dance Updated
										Message9.AppendInt32(class4.VirtualId);
										Message9.AppendInt32(class4.DanceId);
										Session.SendMessage(Message9);
									}
									if (class4.bool_8)
									{
                                        ServerMessage Message10 = new ServerMessage(Outgoing.IdleStatus); // 486 IdleStatus Updated
										Message10.AppendInt32(class4.VirtualId);
										Message10.AppendBoolean(true);
										Session.SendMessage(Message10);
									}
									if (class4.CarryItemID > 0 && class4.int_6 > 0)
									{
                                        ServerMessage Message11 = new ServerMessage(Outgoing.RespectPet); // CarryItem Updated
										Message11.AppendInt32(class4.VirtualId);
										Message11.AppendInt32(class4.CarryItemID);
										Session.SendMessage(Message11);
									}
									if (!class4.IsBot)
									{
										try
										{
											if (class4.GetClient().GetHabbo() != null && class4.GetClient().GetHabbo().GetEffectsInventoryComponent() != null && class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 >= 1)
											{
                                                ServerMessage Message12 = new ServerMessage(Outgoing.ApplyEffects); // 485 ApplyEffects Updated
												Message12.AppendInt32(class4.VirtualId);
												Message12.AppendInt32(class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0);
                                                Message12.AppendInt32(0);
                                                Session.SendMessage(Message12);
											}
											goto IL_5C5;
										}
										catch
										{
											goto IL_5C5;
										}
									}
									if (!class4.IsPet && class4.RoomBot.EffectId != 0)
									{
                                        ServerMessage Message12 = new ServerMessage(Outgoing.ApplyEffects);
										Message12.AppendInt32(class4.VirtualId);
										Message12.AppendInt32(class4.RoomBot.EffectId);
                                        Message12.AppendInt32(0);
										Session.SendMessage(Message12);
									}
								}
								IL_5C5:;
							}
							if (class2 != null && Session != null && Session.GetHabbo().CurrentRoom != null)
							{
                               // Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
								class2.method_8(Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id));
							}
							if (class2.Achievement > 0u)
							{
								Essential.GetGame().GetAchievementManager().addAchievement(Session, class2.Achievement, 1);
							}
							if (Session.GetHabbo().IsMuted && Session.GetHabbo().int_4 > 0)
							{
								/*ServerMessage Message13 = new ServerMessage(27u);
								Message13.AppendInt32(Session.GetHabbo().int_4);
								Session.SendMessage(Message13);
                                */
							}
                        }
					 }
			 }	
        
		public void method_5(uint uint_0, string string_0)
		{
			this.method_7();
			if (Essential.GetGame().GetRoomManager().method_12(uint_0) != null)
			{
                if (this.Session != null && this.Session.GetHabbo() != null && this.Session.GetHabbo().InRoom)
				{
					Room @class = Essential.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
					if (@class != null)
					{
                        @class.RemoveUserFromRoom(this.Session, false, false);
					}
				}
				Room class2 = Essential.GetGame().GetRoomManager().method_15(uint_0);


     
				if (class2 != null && Session != null && Session.GetHabbo() != null)
				{
					this.Session.GetHabbo().uint_2 = uint_0;

					if (class2.method_68(this.Session.GetHabbo().Id))
					{
						if (!class2.method_71(this.Session.GetHabbo().Id))
						{
                            ServerMessage Message = new ServerMessage(Outgoing.RoomErrorToEnter); // p
							Message.AppendInt32(4);
							this.Session.SendMessage(Message);
                            ServerMessage Message2 = new ServerMessage(Outgoing.OutOfRoom); // P
							this.Session.SendMessage(Message2);
							return;
						}
						class2.method_69(this.Session.GetHabbo().Id);
					}
                    if (class2.UsersNow >= class2.UsersMax && !Essential.GetGame().GetRoleManager().HasFuse(this.Session.GetHabbo().Rank, "acc_enter_fullrooms") && !this.Session.GetHabbo().IsVIP)
					{
                        ServerMessage Message = new ServerMessage(Outgoing.RoomErrorToEnter); // P
						Message.AppendInt32(1);
						this.Session.SendMessage(Message);
                        ServerMessage Message2 = new ServerMessage(Outgoing.OutOfRoom); // P
						this.Session.SendMessage(Message2);
					}
					else
					{
						if (class2.Type == "public")
						{
							if (class2.State > 0 && !this.Session.GetHabbo().HasFuse("acc_restrictedrooms"))
							{
								this.Session.SendNotification("Dieser Raum ist nur für Staffs zugänglich.");
                                ServerMessage Message2 = new ServerMessage(Outgoing.OutOfRoom); // P
								this.Session.SendMessage(Message2);
								return;
							}
						/*	ServerMessage Message3 = new ServerMessage(166u);
							Message3.AppendStringWithBreak("/client/public/" + class2.ModelName + "/0");
							this.Session.SendMessage(Message3);
                            */
						}
						else
						{
							if (class2.Type == "private")
							{
                                ServerMessage Logging = new ServerMessage(Outgoing.PrepareRoomForUsers); // P
								this.Session.SendMessage(Logging);
								if (this.Session.GetHabbo().bool_7)
								{
									RoomItem class3 = class2.method_28(this.Session.GetHabbo().uint_5);
									if (class3 == null)
									{
										this.Session.GetHabbo().bool_7 = false;
										this.Session.GetHabbo().uint_5 = 0u;
                                       // ServerMessage Message5 = new ServerMessage(Outgoing.Unkown);
										//this.Session.SendMessage(Message5);
										return;
									}
								}
                                if (!this.Session.GetHabbo().HasFuse("acc_enter_anyroom") && !class2.CheckRights(this.Session, true) && !this.Session.GetHabbo().bool_7)
								{
									if (class2.State == 1)
									{
										if (class2.UserCount == 0)
										{
                                            ServerMessage Message5 = new ServerMessage(Outgoing.DoorBellNoPerson); // P
                                            Message5.AppendString("");
											this.Session.SendMessage(Message5);
											return;
										}
                                        ServerMessage Message6 = new ServerMessage(Outgoing.Doorbell); // P
                                        Message6.AppendStringWithBreak("");
										this.Session.SendMessage(Message6);
										this.Session.GetHabbo().bool_6 = true;
                                        ServerMessage Message7 = new ServerMessage(Outgoing.Doorbell); // P
										Message7.AppendStringWithBreak(this.Session.GetHabbo().Username);
										class2.method_61(Message7);
										return;
									}
									else
									{
										if (class2.State == 2 && string_0.ToLower() != class2.Password.ToLower())
										{
                                            ServerMessage Message8 = new ServerMessage(Outgoing.GenericError); // P
											Message8.AppendInt32(-100002);
											this.Session.SendMessage(Message8);
                                            ServerMessage Message2 = new ServerMessage(Outgoing.OutOfRoom); // P
											this.Session.SendMessage(Message2);
											return;
										}
									}
								}
								/*ServerMessage Message3 = new ServerMessage(166u);
								Message3.AppendStringWithBreak("/client/private/" + class2.Id + "/Id");
								this.Session.SendMessage(Message3);
                                */
							}
						}
						this.Session.GetHabbo().bool_5 = true;
						this.method_6();
					}
				}
			}
		}
        //method_5 -> DONE!
		public void method_6()
		{
			Room room = Essential.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().uint_2);
            if (room != null && this.Session.GetHabbo().bool_5)
			{
                if (room.Type == "private")
				{
                    ServerMessage Message = new ServerMessage(Outgoing.InitialRoomInformation); // P
                    Message.AppendStringWithBreak(room.ModelName);
                    Message.AppendUInt(room.Id);
				    this.Session.SendMessage(Message);
                }
                if (room.Type == "private")
				{
                    if (room.Wallpaper != "0.0")
					{
                        ServerMessage Message3 = new ServerMessage(Outgoing.RoomDecoration); // P
						Message3.AppendStringWithBreak("wallpaper");
                        Message3.AppendStringWithBreak(room.Wallpaper);
						this.Session.SendMessage(Message3);
					}
                    if (room.Floor != "0.0")
					{
                        ServerMessage Logging = new ServerMessage(Outgoing.RoomDecoration); // P
						Logging.AppendStringWithBreak("floor");
                        Logging.AppendStringWithBreak(room.Floor);
						this.Session.SendMessage(Logging);
					}
                    ServerMessage Message5 = new ServerMessage(Outgoing.RoomDecoration); // P
					Message5.AppendStringWithBreak("landscape");
                    Message5.AppendStringWithBreak(room.Landscape);
					this.Session.SendMessage(Message5);
                    if (room.CheckRights(this.Session, true))
					{
                        ServerMessage Message6 = new ServerMessage(Outgoing.RoomRightsLevel);  // Updated
                        Message6.AppendInt32(4);
						this.Session.SendMessage(Message6);
                        ServerMessage Message7 = new ServerMessage(Outgoing.HasOwnerRights); // Updated
						this.Session.SendMessage(Message7);
					}
					else
					{
                        if (room.method_26(this.Session))
						{
                            ServerMessage Message6 = new ServerMessage(Outgoing.RoomRightsLevel);
                            Message6.AppendInt32(1);
							this.Session.SendMessage(Message6);
						}
					}
                    ServerMessage Message8 = new ServerMessage(Outgoing.ScoreMeter); // P
                    Message8.AppendInt32(room.Score);
                    if (this.Session.GetHabbo().list_4.Contains(room.Id) || room.CheckRights(this.Session, true))
					{
                        Message8.AppendBoolean(false);
					}
					else
					{
                        Message8.AppendBoolean(true);
					}
                    try
                    {
                        List<GroupsManager> list = new List<GroupsManager>();
                        GroupsManager item = null;
                        foreach (RoomUser user in room.RoomUsers)
                        {
                            if (user != null && (!user.IsPet && !user.IsBot) && (user.GetClient().GetHabbo().FavouriteGroup > 0))
                            {
                                GroupsManager guild = Groups.GetGroupById(user.GetClient().GetHabbo().FavouriteGroup);
                                if ((guild != null) && !list.Contains(guild))
                                {
                                    list.Add(guild);
                                }
                            }
                        }
                        if (this.Session.GetHabbo().FavouriteGroup > 0)
                        {
                            item = Groups.GetGroupById(this.Session.GetHabbo().FavouriteGroup);
                            if ((item != null) && !list.Contains(item))
                            {
                                list.Add(item);
                            }
                        }
                        this.Session.SendMessage(Message8);
                        ServerMessage guilds = new ServerMessage(Outgoing.SendGroup);
                        guilds.AppendInt32(list.Count);
                        foreach (GroupsManager guild2 in list)
                        {
                            guilds.AppendInt32(guild2.Id);
                            guilds.AppendString(guild2.Badge);
                        }
                        this.Session.SendMessage(guilds);
                        if (item != null)
                        {
                            ServerMessage message2 = new ServerMessage(Outgoing.SendGroup);
                            message2.AppendInt32(1);
                            message2.AppendInt32(item.Id);
                            message2.AppendString(item.Badge);
                            room.SendMessage(message2, null);
                        }
                    }
                    catch { }
				}
				this.method_4();
			}
		}
		public void method_7()
		{
			this.Session.GetHabbo().uint_2 = 0u;
			this.Session.GetHabbo().bool_5 = false;
			this.Session.GetHabbo().bool_6 = false;
		}
		public bool method_8(string string_0)
		{
			if (!Regex.IsMatch(string_0, "^[-a-zA-Z0-9._:,]+$"))
			{
				return false;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					dataRow = @class.ReadDataRow("SELECT * FROM users WHERE username = '" + string_0 + "'");
				}
				return (dataRow == null);
			}
		}
        public void LoadMembersPetitions(int a, int gId, int b, string search, GameClient Session)
        {
            try
            {
                int guildId = gId;
                int num2 = b;
                int num3 = a;
                string str = search;
                GroupsManager guild = Groups.GetGroupById(guildId);
                if (guild != null)
                {
                    ServerMessage message;
                    int memberCounter = 0;
                    int results;
                    if (num3 == 0)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            results = 0;
                            foreach (int num4 in guild.Members)
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                    results++;
                            }
                        }
                        else
                            results = guild.Members.Count;
                        message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                        message.AppendInt32(guild.Id);
                        message.AppendString(guild.Name);
                        message.AppendInt32(guild.RoomId);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(results);

                        if (num2 == 0)
                            message.AppendInt32(results);
                        else
                            message.AppendInt32(results - (num2 * 14));

                        foreach (int num4 in guild.Members)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                {
                                    if (memberCounter >= ((num2) * 14))
                                    {
                                        if (guild.OwnerId == num4)
                                        {
                                            message.AppendInt32(0);
                                        }
                                        else
                                        {
                                            message.AppendInt32(guild.getRank(num4));
                                        }
                                        message.AppendInt32(num4);
                                        message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                        message.AppendString("");
                                    }
                                    memberCounter++;
                                }
                            }

                            else
                            {
                                if (memberCounter >= (num2 * 14))
                                {
                                    if (guild.OwnerId == (uint)num4)
                                    {
                                        message.AppendInt32(0);
                                    }
                                    else
                                    {
                                        message.AppendInt32(guild.getRank(num4));
                                    }
                                    message.AppendInt32(num4);
                                    message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                        message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                    message.AppendString("");
                                }
                                memberCounter++;
                            }
                        }

                        if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                                message.AppendBoolean(true);
                            else message.AppendBoolean(false);
                        }
                        else message.AppendBoolean(false);
                        message.AppendInt32(14);
                        message.AppendInt32(num2);
                        message.AppendInt32(0);
                        message.AppendString(str);
                        Session.SendMessage(message);
                    }
                    else if (num3 == 1)
                    {
                        int admins = 0;

                        foreach (int num4 in guild.Members)
                        {
                            if (guild.UserWithRanks.Contains(num4)) admins++;
                        }

                        if (!string.IsNullOrEmpty(str))
                        {
                            results = 0;
                            foreach (int num4 in guild.Members)
                            {
                                if ((Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower())) && ((guild.UserWithRanks.Contains(num4))))
                                    results++;
                            }
                        }
                        else
                            results = admins;

                        message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                        message.AppendInt32(guild.Id);
                        message.AppendString(guild.Name);
                        message.AppendInt32(guild.RoomId);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(results);

                        if (num2 == 0)
                            message.AppendInt32(results);

                        else message.AppendInt32(results - (num2 * 14));
                        memberCounter = 0;
                        foreach (int num4 in guild.Members)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                {
                                    if (guild.UserWithRanks.Contains(num4))
                                    {
                                        if (memberCounter >= ((num2) * 14))
                                        {
                                            if (guild.OwnerId == num4)
                                            {
                                                message.AppendInt32(0);
                                            }
                                            else
                                            {
                                                message.AppendInt32(guild.getRank(num4));
                                            }
                                            message.AppendInt32(num4);
                                            message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                            message.AppendString("");
                                        }
                                    }
                                    memberCounter++;
                                }
                            }

                            else
                            {
                                if (memberCounter >= (num2 * 14))
                                {
                                    if (guild.UserWithRanks.Contains(num4))
                                    {
                                        if (guild.OwnerId == num4)
                                        {
                                            message.AppendInt32(0);
                                        }
                                        else
                                        {
                                            message.AppendInt32(guild.getRank(num4));
                                        }
                                        message.AppendInt32(num4);
                                        message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                        message.AppendString("");
                                    }
                                }
                                memberCounter++;
                            }
                        }
                        if (Session.GetHabbo().InGuild(guild.Id))
                        {
                            if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                                message.AppendBoolean(true);
                            else message.AppendBoolean(false);
                        }
                        else message.AppendBoolean(false);
                        message.AppendInt32(14);
                        message.AppendInt32(num2);
                        message.AppendInt32(1);
                        message.AppendString(str);
                        Session.SendMessage(message);
                    }

                    else if (num3 == 2)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            results = 0;
                            foreach (int num4 in guild.Petitions)
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                    results++;
                            }
                        }

                        else
                            results = guild.Petitions.Count;

                        message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                        message.AppendInt32(guild.Id);
                        message.AppendString(guild.Name);
                        message.AppendInt32(guild.RoomId);
                        message.AppendString(guild.Badge);
                        message.AppendInt32(results);
                        if (num2 == 0)
                            message.AppendInt32(results);

                        else message.AppendInt32(results - (num2 * 14));
                        memberCounter = 0;
                        foreach (int num4 in guild.Petitions)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (Essential.GetGame().GetClientManager().GetNameById((uint)num4).ToLower().Contains(str.ToLower()))
                                {
                                    if (memberCounter >= ((num2) * 14))
                                    {
                                        message.AppendInt32(3);
                                        message.AppendInt32(num4);
                                        message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                        message.AppendString("");
                                    }
                                    memberCounter++;
                                }
                            }

                            else
                            {
                                if (memberCounter >= (num2 * 14))
                                {
                                    message.AppendInt32(3);
                                    message.AppendInt32(num4);
                                    message.AppendString(Essential.GetGame().GetClientManager().GetNameById((uint)num4));
                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                        message.AppendString(dbClient.ReadString("SELECT look FROM users WHERE id=" + num4));
                                    message.AppendString("");
                                }
                                memberCounter++;
                            }
                        }
                        if (guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                        message.AppendInt32(14);
                        message.AppendInt32(num2);
                        message.AppendInt32(2);
                        message.AppendString(str);
                        Session.SendMessage(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
        internal void LoadArena(SnowStorm War)
        {
            this.GetResponse().Init(Outgoing.Game2StageStartingMessageEvent);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendString("snowwar_arena_0");
            this.GetResponse().AppendInt32(5);
            this.CheckArenaStatic(this.GetResponse(), War);
            foreach (Habbo habbo in War.WarUsers)
            {
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(habbo.SnowUserId);
                this.GetResponse().AppendInt32(habbo.SnowX);
                this.GetResponse().AppendInt32(habbo.SnowY);
                this.GetResponse().AppendInt32((int)(habbo.SnowX / 0xc80));
                this.GetResponse().AppendInt32((int)(habbo.SnowY / 0xc80));
                this.GetResponse().AppendInt32(2);
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32((int)(habbo.SnowX / 0xc80));
                this.GetResponse().AppendInt32((int)(habbo.SnowY / 0xc80));
                this.GetResponse().AppendInt32(habbo.SnowX);
                this.GetResponse().AppendInt32(habbo.SnowY);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(habbo.SnowTeam);
                this.GetResponse().AppendInt32(habbo.Id);
                this.GetResponse().AppendString(habbo.Username);
                this.GetResponse().AppendString(habbo.Motto);
                this.GetResponse().AppendString(habbo.Figure);
                this.GetResponse().AppendString(habbo.Gender.ToLower());
            }
            War.SendToStorm(this.GetResponse(), false, 0);
            Thread.Sleep(5000);
            foreach (Habbo habbo in War.WarUsers)
            {
                this.GetResponse().Init(Outgoing.Game2PlayerExitedGameArenaMessageEvent);
                this.GetResponse().AppendInt32(habbo.Id);
                this.GetResponse().AppendInt32(20);
                habbo.GetClient().SendMessage(this.GetResponse());
            }
            this.GetResponse().Init(Outgoing.Game2StageRunningMessageEvent);
            this.GetResponse().AppendInt32(120);
            War.SendToStorm(this.GetResponse(), false, 0);
            War.SnowStormStart();
        }
        internal void CheckArenaStatic(ServerMessage Message, SnowStorm War)
        {
            if (War.WarLevel == 9)
            {
                Message.AppendInt32((int)(20 + War.WarUsers.Count));
                Message.AppendInt32(3);
                Message.AppendInt32(0);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xaf00);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0);
                Message.AppendInt32(3);
                Message.AppendInt32(1);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x1770a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(1);
                Message.AppendInt32(3);
                Message.AppendInt32(2);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(3);
                Message.AppendInt32(3);
                Message.AppendInt32(3);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0x11f80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(5);
                Message.AppendInt32(3);
                Message.AppendInt32(4);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x15180);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(6);
                Message.AppendInt32(3);
                Message.AppendInt32(5);
                Message.AppendInt32(0xc800);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(7);
                Message.AppendInt32(3);
                Message.AppendInt32(6);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x16a80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(10);
                Message.AppendInt32(3);
                Message.AppendInt32(7);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0xe100);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(14);
                Message.AppendInt32(3);
                Message.AppendInt32(8);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x12c0a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(15);
                Message.AppendInt32(3);
                Message.AppendInt32(9);
                Message.AppendInt32(0x15e0a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x10);
                Message.AppendInt32(3);
                Message.AppendInt32(10);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xd480);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x11);
                Message.AppendInt32(3);
                Message.AppendInt32(11);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0xbb80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x12);
                Message.AppendInt32(3);
                Message.AppendInt32(12);
                Message.AppendInt32(0x12c0a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(20);
                Message.AppendInt32(3);
                Message.AppendInt32(13);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x11f80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x15);
                Message.AppendInt32(3);
                Message.AppendInt32(14);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0xbb80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x18);
                Message.AppendInt32(3);
                Message.AppendInt32(15);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x1450a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x19);
                Message.AppendInt32(3);
                Message.AppendInt32(0x10);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0x15180);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x1b);
                Message.AppendInt32(3);
                Message.AppendInt32(0x11);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0xed80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x1d);
                Message.AppendInt32(3);
                Message.AppendInt32(0x12);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xfa00);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(30);
                Message.AppendInt32(3);
                Message.AppendInt32(0x13);
                Message.AppendInt32(0xfa00);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x23);
            }
        }
        internal ServerMessage AddToNewGame(SnowStorm War)
        {
            //War.WarUsers.Add(this.Session.GetHabbo());
            this.Session.GetHabbo().SnowWar = War;
            ServerMessage message = new ServerMessage(Outgoing.AddToNewGame);

            message.AppendInt32(this.Session.GetHabbo().Id);
            message.AppendString(this.Session.GetHabbo().Username);
            message.AppendString(this.Session.GetHabbo().Figure);
            message.AppendString(this.Session.GetHabbo().Gender.ToLower());
            message.AppendInt32(-1);
            message.AppendInt32(this.Session.GetHabbo().SnowLevel);
            message.AppendInt32(this.Session.GetHabbo().SnowPoints);
            message.AppendInt32(1);
            message.AppendBoolean(false);
            return message;
        }
        internal SnowStorm CheckAGame()
        {
            foreach (SnowStorm storm in Essential.GetGame().GetStormWars().Wars.Values)
            {
                if ((storm.WarUsers.Count < 10) && (storm.WarStarted < 1))
                {
                    return storm;
                }
            }
            return null;
        }

	}
}
