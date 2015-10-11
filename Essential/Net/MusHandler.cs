
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Text;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using System.Threading;
using Essential.Storage;
using Essential.HabboHotel.AntiAd;
namespace Essential.Net
{
	internal sealed class MusHandler
	{
		private Socket ClientSocket;

		private byte[] Buffer = new byte[1024];

		public MusHandler(Socket serverSocket)
		{
			this.ClientSocket = serverSocket;

			try
			{
				this.ClientSocket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceiveCallback), this.ClientSocket);
			}
			catch
			{
				this.Dispose();
			}
		}

		public void Dispose()
		{
            try
            {
                this.ClientSocket.Shutdown(SocketShutdown.Both);
                this.ClientSocket.Close();
                this.ClientSocket.Dispose();
            }
            catch { }
		}

		public void OnReceiveCallback(IAsyncResult ar)
		{
            try
            {
                int count = 0;

                try
                {
                    count = this.ClientSocket.EndReceive(ar);
                }
                catch
                {
                    this.Dispose();
                    return;
                }

                string data = Encoding.Default.GetString(this.Buffer, 0, count);

                if (data.Length > 0)
                {
                    this.ParsePacket(data);
                }
            }
            catch { }

			this.Dispose();
		}

		public void ParsePacket(string data)
		{
			string text = data.Split(new char[]
			{
				Convert.ToChar(1)
			})[0];

			string text2 = data.Split(new char[]
			{
				Convert.ToChar(1)
			})[1];

			GameClient client = null;
			DataRow dataRow = null;

			string text3 = text.ToLower();

			if (text3 != null)
			{
				if (MusCommands.dictionary_0 == null)
				{
					MusCommands.dictionary_0 = new Dictionary<string, int>(29)
					{

						{
							"update_items",
							0
						},

						{
							"update_catalogue",
							1
						},

						{
							"update_catalog",
							2
						},

						{
							"updateusersrooms",
							3
						},

						{
							"senduser",
							4
						},

						{
							"updatevip",
							5
						},

						{
							"giftitem",
							6
						},

						{
							"giveitem",
							7
						},

						{
							"unloadroom",
							8
						},

						{
							"roomalert",
							9
						},

						{
							"updategroup",
							10
						},

						{
							"updateusersgroups",
							11
						},

						{
							"shutdown",
							12
						},

						{
							"update_filter",
							13
						},

						{
							"refresh_filter",
							14
						},

						{
							"updatecredits",
							15
						},

						{
							"updatesettings",
							16
						},

						{
							"updatepixels",
							17
						},

						{
							"updatepoints",
							18
						},

						{
							"reloadbans",
							19
						},

						{
							"update_bots",
							20
						},

						{
							"signout",
							21
						},

						{
							"exe",
							22
						},

						{
							"alert",
							23
						},

						{
							"sa",
							24
						},

						{
							"ha",
							25
						},

						{
							"hal",
							26
						},

						{
							"updatemotto",
							27
						},
                       	{
							"update_badges",
							29
						},
                        {
							"update_navigator",
							40
						},
                        {
							"startquestion",
							31
						},
                        {
							"roomkick",
							37
						},
                         {
							"setinapp",
							38
						},
                        {
							"lockroom",
							39
						},
						{
							"updatelook",
							28
						},
                        {
                            "infobuspoll",
                            34
                        },
                        {
                            "givebadge",
                            32
                        },
                        {
                            "update_permissions",
                            33
                        },
                        {
                            "update_catalogid",
                            35
                        },
                        {
                            "addFriend",
                            36
                        },
                        {
                            "ref_websocket",
                            50
                        },
                        {
                            "eventha",
                            51
                        }
					};
				}

				int num;

				if (MusCommands.dictionary_0.TryGetValue(text3, out num))
				{
					uint num2;
					uint uint_2;
					Room class4;
					uint num3;
					string text5;

					switch (num)
					{
					case 0:
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							Essential.GetGame().GetItemManager().Initialize(class2);
                           
							goto IL_C70;
						}
					case 1:
					case 2:
						break;
					case 3:
					{
						Habbo class3 = Essential.GetGame().GetClientManager().GetClient(Convert.ToUInt32(text2)).GetHabbo();
						if (class3 != null)
						{
							using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
							{
								class3.method_1(class2);
								goto IL_C70;
							}
						}
						goto IL_C70;
					}
					case 4:
						goto IL_34E;
					case 5:
					{
						Habbo class3 = Essential.GetGame().GetClientManager().GetClient(Convert.ToUInt32(text2)).GetHabbo();
						if (class3 != null)
						{
							class3.UpdateRights();
							goto IL_C70;
						}
						goto IL_C70;
					}
					case 6:
					case 7:
					{
						num2 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
						uint uint_ = uint.Parse(text2.Split(new char[]
						{
							' '
						})[1]);
						int int_ = int.Parse(text2.Split(new char[]
						{
							' '
						})[2]);
						string string_ = text2.Substring(num2.ToString().Length + uint_.ToString().Length + int_.ToString().Length + 3);
						Essential.GetGame().GetCatalog().CreateGift(string_, num2, uint_, int_);
                     
						goto IL_C70;
					}
					case 8:
						uint_2 = uint.Parse(text2);
						class4 = Essential.GetGame().GetRoomManager().GetRoom(uint_2);
						Essential.GetGame().GetRoomManager().method_16(class4);
						goto IL_C70;
                    case 37:
                        num3 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);

                        class4 = Essential.GetGame().GetRoomManager().GetRoom(num3);
                        if (class4 != null)
                        {
                            Essential.GetGame().GetModerationTool().ClearRoomFromUsers(num3);
                            goto IL_C70;
                        }
                        goto IL_C70;
                    case 38:
                        num3 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);

                        class4 = Essential.GetGame().GetRoomManager().GetRoom(num3);
                        if (class4 != null)
                        {
                            Essential.GetGame().GetModerationTool().SetInApp(num3);
                            goto IL_C70;
                        }
                        goto IL_C70;
                    case 39:
                        num3 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);

                        class4 = Essential.GetGame().GetRoomManager().GetRoom(num3);
                        if (class4 != null)
                        {
                            Essential.GetGame().GetModerationTool().LockRoom(num3);
                            goto IL_C70;
                        }
                        goto IL_C70;

                        case 40:
                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetNavigator().Initialize(dbClient);
                                    Essential.GetGame().GetRoomManager().method_8(dbClient);
                                    Essential.GetGame().GetRoomManager().LoadMagicTiles(dbClient);
                                    Essential.GetGame().GetRoomManager().LoadBillboards(dbClient);
                                }
                        goto IL_C70;
					case 9:
						num3 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
						class4 = Essential.GetGame().GetRoomManager().GetRoom(num3);
						if (class4 != null)
						{
							string string_2 = text2.Substring(num3.ToString().Length + 1);
							for (int i = 0; i < class4.RoomUsers.Length; i++)
							{
								RoomUser class5 = class4.RoomUsers[i];
								if (class5 != null && !class5.IsBot && !class5.IsPet)
								{
									class5.GetClient().SendNotification(string_2);
								}
							}
							goto IL_C70;
						}
						goto IL_C70;
					case 10:
					{
						int int_2 = int.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							Groups.UpdateGroup(class2, int_2);
							goto IL_C70;
						}
					}
					case 11:
						goto IL_5BF;
					case 12:
						goto IL_602;
					case 13:
					case 14:
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							ChatCommandHandler.InitWords(class2);
							goto IL_C70;
						}
					case 15:
						goto IL_633;
					case 16:
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							Essential.GetGame().LoadServerSettings(class2);
							goto IL_C70;
						}
					case 17:
						goto IL_6F7;
					case 18:
						client = Essential.GetGame().GetClientManager().GetClient(uint.Parse(text2));
						if (client != null)
						{
							client.GetHabbo().UpdateVipPoints(true, false);
							goto IL_C70;
						}
						goto IL_C70;
					case 19:
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							Essential.GetGame().GetBanManager().Initialise(class2);
						}
						Essential.GetGame().GetClientManager().UpdateBans();
						goto IL_C70;
					case 20:
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
                            Essential.GetGame().GetBotManager().Initialize(class2);
							goto IL_C70;
						}
					case 21:
						goto IL_839;
					case 22:
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(text2);
							goto IL_C70;
						}
					case 23:
						goto IL_880;
					case 24:
					{
                        ServerMessage Message = new ServerMessage(Outgoing.InstantChat);
						Message.AppendUInt(0u);
                        Message.AppendString("Habbo: " + text2);
                        Message.AppendString(Essential.GetUnixTimestamp() + string.Empty);
						Essential.GetGame().GetClientManager().SendToStaffs(Message, Message);
						goto IL_C70;
					}
					case 25:
					{
                        ServerMessage Message2 = new ServerMessage(Outgoing.BroadcastMessage);
						Message2.AppendStringWithBreak(EssentialEnvironment.GetExternalText("mus_ha_title") + "\n\n" + text2);
						Message2.AppendStringWithBreak("");
						/*ServerMessage Message3 = new ServerMessage(161u);
						Message3.AppendStringWithBreak(text2);
                        */
                        Essential.GetGame().GetClientManager().SendToHotel(Message2, Message2);
						goto IL_C70;
					}
					case 26:
					{
						string text4 = text2.Split(new char[]
						{
							' '
						})[0];
						text5 = text2.Substring(text4.Length + 1);
                        ServerMessage Message4 = new ServerMessage(Outgoing.SendNotif); // Updated
						Message4.AppendStringWithBreak(string.Concat(new string[]
						{
							EssentialEnvironment.GetExternalText("mus_hal_title"),
							"\r\n",
							text5,
							"\r\n-",
							EssentialEnvironment.GetExternalText("mus_hal_tail")
						}));
						Message4.AppendStringWithBreak(text4);
						Essential.GetGame().GetClientManager().BroadcastMessage(Message4);
						goto IL_C70;
					}
                    case 29:
                    {
                        num3 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
                        uint UserIidu3 = num3;
                        client = Essential.GetGame().GetClientManager().GetClient(UserIidu3);
                        if (client != null)
                        {
                            client.GetHabbo().GetBadgeComponent().AddBadge(text2.Split(new char[]
						{
							' '
						})[1], 0, false);

                        }

                        ServerMessage Message = new ServerMessage(Outgoing.AddBadge);

                            Message.AppendInt32(1);
                            Message.AppendInt32(4);
                            Message.AppendInt32(1);

                            Message.AppendUInt(0);

                            client.SendMessage(Message);
                           
                            client.SendMessage(client.GetHabbo().GetBadgeComponent().ComposeBadgeListMessage());

                           
                        goto IL_C70;
                    }
                    case 30:
                    {
                        uint UserIidu = uint.Parse(text2);
                        client = Essential.GetGame().GetClientManager().GetClient(UserIidu);
                        if (client != null)
                        {
                            if (client.GetHabbo().InRoom)
                            {
                                if (client.GetHabbo().CurrentRoom.CCTs.Contains("park"))
                                {
                                    if (!client.GetHabbo().CurrentRoom.IsInfobusOpen)
                                                                {
                                                                    client.GetHabbo().CurrentRoom.IsInfobusOpen = true;
                                                                    
                                                                    
                                                                }
                                                                else
                                                                {
                                                                    client.GetHabbo().CurrentRoom.IsInfobusOpen = false;
                                                              
                                                                }
                                                               
                                                                for (int i = 0; i < client.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                                {
                                    RoomUser RoomiUser = client.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (RoomiUser != null)
                                    {
                                        ServerMessage ParkBusDoorMessage = new ServerMessage(Outgoing.ParkBusDoor);
                                            ParkBusDoorMessage.AppendBoolean(client.GetHabbo().CurrentRoom.IsInfobusOpen);
                                        RoomiUser.GetClient().SendMessage(ParkBusDoorMessage);
                                    }
                                }
                            }
                                else
                                {
                                   // client.SendNotification("Sinun täytyy olla puistossa avataksesi tietolinjurin oven!");
                                }
                            }
                        }
                        goto IL_C70;
                    }
                    case 355:
                    {
                        uint UserIidu2 = uint.Parse(text2.Split(';')[0]);
                        client = Essential.GetGame().GetClientManager().GetClient(UserIidu2);
                        if (client != null)
                        {
                            if (client.GetHabbo().InRoom)
                            {

                                Room Room = Essential.GetGame().GetRoomManager().GetRoom(client.GetHabbo().CurrentRoomId);
                                DataTable Data = null;
                                int QuestionId = int.Parse(text2.Split(';')[1]);
                                Room.CurrentPollId = QuestionId;
                                string Question;

                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Question = dbClient.ReadString("SELECT question FROM infobus_questions WHERE id = '" + QuestionId + "' LIMIT 1");
                                    Data = dbClient.ReadDataTable("SELECT * FROM infobus_answers WHERE question_id = '" + QuestionId + "'");
                                }

                                ServerMessage InfobusQuestion = new ServerMessage(Outgoing.InfobusPoll3); // Updated
                                InfobusQuestion.AppendStringWithBreak(Question);
                                InfobusQuestion.AppendInt32(Data.Rows.Count);
                                if (Data != null)
                                {
                                    foreach (DataRow Row in Data.Rows)
                                    {
                                        InfobusQuestion.AppendInt32((int)Row["id"]);
                                        InfobusQuestion.AppendStringWithBreak((string)Row["answer_text"]);
                                    }
                                }
                                Room.SendMessage(InfobusQuestion, null);

                                Thread Infobus = new Thread(delegate() { Room.ShowResults(Room, QuestionId, client); });
                                Infobus.Start();

                                
                            }
                        }
                        goto IL_C70;
                    }
					case 27:
					case 28:
					{
						uint_2 = uint.Parse(text2);
						client = Essential.GetGame().GetClientManager().GetClient(uint_2);
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							dataRow = class2.ReadDataRow("SELECT look,gender,motto,mutant_penalty,block_newfriends FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1");
						}
						client.GetHabbo().Figure = (string)dataRow["look"];
						client.GetHabbo().Gender = dataRow["gender"].ToString().ToLower();
						client.GetHabbo().Motto = Essential.FilterString((string)dataRow["motto"]);
						client.GetHabbo().BlockNewFriends = Essential.StringToBoolean(dataRow["block_newfriends"].ToString());
                        /*ServerMessage Message5 = new ServerMessage(Outgoing.UpdateUserInformation);
						Message5.AppendInt32(-1);
						Message5.AppendStringWithBreak(client.GetHabbo().Figure);
						Message5.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
						Message5.AppendStringWithBreak(client.GetHabbo().Motto);
						client.SendMessage(Message5);*/
                        if (client.GetHabbo().InRoom)
						{
							class4 = Essential.GetGame().GetRoomManager().GetRoom(client.GetHabbo().CurrentRoomId);
							RoomUser class6 = class4.GetRoomUserByHabbo(client.GetHabbo().Id);
                            ServerMessage Message6 = new ServerMessage(Outgoing.UpdateUserInformation);
							Message6.AppendInt32(class6.VirtualId);
							Message6.AppendStringWithBreak(client.GetHabbo().Figure);
							Message6.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
							Message6.AppendStringWithBreak(client.GetHabbo().Motto);
							Message6.AppendInt32(client.GetHabbo().AchievementScore);
							class4.SendMessage(Message6, null);
						}
						text3 = text.ToLower();
						if (text3 == null)
						{
							goto IL_C70;
						}
						if (text3 == "updatemotto")
						{
                            class4 = Essential.GetGame().GetRoomManager().GetRoom(client.GetHabbo().CurrentRoomId);
                            RoomUser class6 = class4.GetRoomUserByHabbo(client.GetHabbo().Id);
                            ServerMessage Message6 = new ServerMessage(Outgoing.UpdateUserInformation);
                            Message6.AppendInt32(class6.VirtualId);
                            Message6.AppendStringWithBreak(client.GetHabbo().Figure);
                            Message6.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                            Message6.AppendStringWithBreak(client.GetHabbo().Motto);
                            Message6.AppendInt32(client.GetHabbo().AchievementScore);
                            class4.SendMessage(Message6, null);
                            client.GetHabbo().MottoAchievementsCompleted();
							goto IL_C70;
						}
						if (text3 == "updatelook")
						{
                            ServerMessage serverMessage = new ServerMessage(Outgoing.UpdateUserInformation);
                            serverMessage.AppendInt32(-1);
                            serverMessage.AppendStringWithBreak(client.GetHabbo().Figure);
                            serverMessage.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                            serverMessage.AppendStringWithBreak(client.GetHabbo().Motto);
                            serverMessage.AppendInt32(client.GetHabbo().AchievementScore);
                            // serverMessage.AppendStringWithBreak("");
                            client.SendMessage(serverMessage);
                            class4 = Essential.GetGame().GetRoomManager().GetRoom(client.GetHabbo().CurrentRoomId);
                            RoomUser class6 = class4.GetRoomUserByHabbo(client.GetHabbo().Id);
                            ServerMessage Message6 = new ServerMessage(Outgoing.UpdateUserInformation);
                            Message6.AppendInt32(class6.VirtualId);
                            Message6.AppendStringWithBreak(client.GetHabbo().Figure);
                            Message6.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                            Message6.AppendStringWithBreak(client.GetHabbo().Motto);
                            Message6.AppendInt32(client.GetHabbo().AchievementScore);
                            class4.SendMessage(Message6, null);
                            client.GetHabbo().AvatarLookAchievementsCompleted();
							goto IL_C70;
						}
						goto IL_C70;
					}
                    case 31:
                    {

                        int QuestionID = int.Parse(text2.Split(';')[1]);
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            DataRow QuestionRow = dbClient.ReadDataRow("SELECT * FROM infobus_questions WHERE id='" + QuestionID + "' LIMIT 1");
                            DataTable AnswersTable = dbClient.ReadDataTable("SELECT * FROM infobus_answers WHERE question_id='" + QuestionID + "'");
                            Room PollRoom = Essential.GetGame().GetClientManager().GetClientByHabbo(QuestionRow["owner"].ToString()).GetHabbo().CurrentRoom;
                            if (PollRoom.Owner == QuestionRow["owner"].ToString() || Essential.GetGame().GetClientManager().GetClientByHabbo(QuestionRow["owner"].ToString()).GetHabbo().Rank < 5)
                            {
                                PollRoom.GetPollManager().SetCurrentPoll(PollRoom.GetPollManager().CreateNewRoomPoll(QuestionRow, AnswersTable));
                                PollRoom.SendMessage(PollRoom.GetPollManager().GetCurrentPoll().PollToServerMessage(new ServerMessage(Outgoing.InfobusPoll3)), null);
                                Thread Poll = new Thread(delegate() { PollRoom.GetPollManager().GetCurrentPoll().ShowResults(); });
                                Poll.Start();
                            }
                        }

                        goto IL_C70;
                    }
                    case 32:
                    {
                        uint UserId = uint.Parse(text2.Split(new char[] { ' ' })[0]);
                        string BadgeCode = text2.Split(new char[] { ' ' })[1];
                        
                        GameClient Session = Essential.GetGame().GetClientManager().GetClientByHabbo(Essential.GetGame().GetClientManager().GetNameById(UserId));
                        if (Session != null)
                        {
                            Session.GetHabbo().GetBadgeComponent().SendBadge(Session, BadgeCode, true);
                        }
                        else
                        {
                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                            {
                                DataRow dr = dbClient.ReadDataRow("SELECT username FROM users WHERE id=" + UserId + " LIMIT 1");
                                if (dr != null)
                                {
                                    dbClient.AddParamWithValue("badge", BadgeCode);
                                    DataRow dr2 = dbClient.ReadDataRow("SELECT * FROM user_badges WHERE user_id=" + UserId + " AND badge_id=@badge LIMIT 1");
                                    if (dr2 == null)
                                    {
                                        dbClient.AddParamWithValue("badge", BadgeCode);
                                        dbClient.ExecuteQuery(string.Concat(new object[]
					                    {
						                    "INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('",
						                    UserId,
						                    "',@badge,'0')"
					                    }));
                                    }
                                }
                            }
                        }  
                    }
                    goto IL_C70;
                    case 33:
                            using (DatabaseClient class5 = Essential.GetDatabase().GetClient())
                            {
                                Essential.GetGame().GetRoleManager().Initialize(class5);
                            }
                            goto IL_C70;
                        case 35:
                            {
                                using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
					            {
						            Essential.GetGame().GetCatalog().Initialize(class2);
					            }
					            Essential.GetGame().GetCatalog().InitializeCache();
                                goto IL_C70;
                            }
                        case 36:
                            {
                                uint sender = uint.Parse(text2.Split(new char[] { ' ' })[0]);
                                uint reciever = uint.Parse(text2.Split(new char[]{ ' '})[1]);
                                if (!this.FriendshipExists(reciever, sender))
                                {
                                    using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                                    {
                                        @class.AddParamWithValue("toid", reciever);
                                        @class.AddParamWithValue("userid", sender);
                                        @class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@userid,@toid)");
                                        @class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@toid,@userid)");
                                    }
                                    GameClient SenderClient = Essential.GetGame().GetClientManager().GetClient(sender);
                                    GameClient RecieverClient = Essential.GetGame().GetClientManager().GetClient(reciever);
                                    if(RecieverClient != null)
                                        RecieverClient.GetHabbo().GetMessenger().method_14(sender);
                                    if (SenderClient != null)
                                        SenderClient.GetHabbo().GetMessenger().method_14(reciever);
                                }
                                goto IL_C70;
                            }
                        case 50:
                            {
                                Essential.getWebSocketManager().Dispose();
                                Essential.InitWebsocketManager();
                                goto IL_C70;
                            }
                        case 51:
                            {
                                Room room = Essential.GetGame().GetRoomManager().GetRoom(uint.Parse(text2.Split(';')[0]));
                                string Eventname = text2.Split(';')[1];
                                if (Eventname.Length > 1)
                                {
                                    Eventname = AntiAd.Utf8ToUtf16(Eventname);
                                    string toSend = "5|" + Eventname + "|" + room.Owner + "|" + room.Id;
                                    Essential.getWebSocketManager().SendMessageToEveryConnection(toSend);
                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                    {
                                        dbClient.AddParamWithValue("param1", room.Id);
                                        dbClient.AddParamWithValue("param2", Eventname);
                                        dbClient.ExecuteQuery("INSERT INTO hp_aktivitaetenstream (`user_id`,`type`,`extra_data`,`extra_data2`,`timestamp`) VALUES ('" + room.OwnerId + "','makeevent',@param1,@param2,'" + Convert.ToInt32(Essential.GetUnixTimestamp()) + "');");
                                    }
                                }
                                goto IL_C70;
                            }
					default:
						goto IL_C70;
					}
					using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
					{
						Essential.GetGame().GetCatalog().Initialize(class2);
					}
					Essential.GetGame().GetCatalog().InitializeCache();
                    Essential.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(Outgoing.UpdateShop)); // Updated
					goto IL_C70;
					IL_34E:
					num2 = uint.Parse(text2.Split(new char[]
					{
						' '
					})[0]);
					num3 = uint.Parse(text2.Split(new char[]
					{
						' '
					})[1]);
					GameClient class7 = Essential.GetGame().GetClientManager().GetClient(num2);
					class4 = Essential.GetGame().GetRoomManager().GetRoom(num3);
					if (class7 != null)
					{

                        ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                        Message.AppendBoolean(class4.IsPublic);
                        Message.AppendUInt(class4.Id);
                        class7.SendMessage(Message);
						goto IL_C70;
					}
					goto IL_C70;
					IL_5BF:
					uint_2 = uint.Parse(text2);
					using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
					{
						Essential.GetGame().GetClientManager().GetClient(uint_2).GetHabbo().method_0(class2);
						goto IL_C70;
					}
					IL_602:
					Essential.Close();
					goto IL_C70;
					IL_633:
					client = Essential.GetGame().GetClientManager().GetClient(uint.Parse(text2));
					if (client != null)
					{
						int int_3 = 0;
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							int_3 = (int)class2.ReadDataRow("SELECT credits FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1")[0];
						}
						client.GetHabbo().SetCredits(int_3,"MUS UPDATE","");
						client.GetHabbo().UpdateCredits(false);
						goto IL_C70;
					}
					goto IL_C70;
					IL_6F7:
					client = Essential.GetGame().GetClientManager().GetClient(uint.Parse(text2));
					if (client != null)
					{
						int int_4 = 0;
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							int_4 = (int)class2.ReadDataRow("SELECT activity_points FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1")[0];
						}
						client.GetHabbo().ActivityPoints = int_4;
						client.GetHabbo().UpdateActivityPoints(false);
						goto IL_C70;
					}
					goto IL_C70;
					IL_839:
					Essential.GetGame().GetClientManager().GetClient(uint.Parse(text2)).Disconnect("MUS");
					goto IL_C70;
					IL_880:
					string text6 = text2.Split(new char[]
					{
						' '
					})[0];
					text5 = text2.Substring(text6.Length + 1);

					Essential.GetGame().GetClientManager().GetClient(uint.Parse(text6)).SendNotification(text5);
				}
			}
			IL_C70:
            try
            {
                ServerMessage Message9 = new ServerMessage(Outgoing.MusAnswer);
                Message9.AppendString("Essential 5");
                Message9.AppendString("MUS Handler");
                this.ClientSocket.Send(Message9.GetBytes());
            }
            catch {}
		}
        internal bool FriendshipExists(uint uint_1, uint uint_2)
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
	}
}
