using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Essential.Core;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pets;
using Essential.Util;
using Essential.Catalogs;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.Storage;
using Essential.HabboHotel.Users.Inventory;
using Essential.HabboHotel.SurpriseBoxes;

namespace Essential.HabboHotel.Catalogs
{
	internal sealed class Catalog
	{
		public Dictionary<int, CatalogPage> dictionary_0;
		public List<EcotronReward> list_0;
		private VoucherHandler VoucherHandler_0;
		private Marketplace class43_0;
		private ServerMessage[] Message5_0;
		private uint uint_0 = 0u;
		private readonly object object_0 = new object();
        public ServerMessage groupsDataMessage;
        public bool isLoading = false;
        public SurpriseBoxManager spbmanager;
		public Catalog()
		{
			this.VoucherHandler_0 = new VoucherHandler();
			this.class43_0 = new Marketplace();
		}
        public void setLastId(uint id)
        {
            lock (this.object_0)
            {
                this.uint_0 = id;
            }
        }
        public SurpriseBoxManager GetSurpriseBoxManager()
        {
            return this.spbmanager;
        }
		public void Initialize(DatabaseClient class6_0)
		{
            if (!isLoading)
            {
                isLoading = true;
                Logging.Write("Loading Catalogue..");
                this.dictionary_0 = new Dictionary<int, CatalogPage>();


                this.list_0 = new List<EcotronReward>();
                DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM catalog_pages WHERE order_num >= '0' ORDER BY order_num ASC");
                DataTable dataTable2 = class6_0.ReadDataTable("SELECT * FROM ecotron_rewards ORDER BY item_id");
                DataTable dataTable4 = class6_0.ReadDataTable("SELECT * FROM catalog_pages WHERE order_num = '-1' ORDER BY caption ASC");
                try
                {
                    this.uint_0 = (uint)class6_0.ReadDataRow("SELECT ID FROM items ORDER BY ID DESC LIMIT 1")[0];
                }
                catch
                {
                    this.uint_0 = 0u;
                }
                this.uint_0 += 1u;
                this.spbmanager = new SurpriseBoxManager(class6_0);
                Hashtable hashtable = new Hashtable();
                DataTable dataTable3 = class6_0.ReadDataTable("SELECT * FROM catalog_items");
                if (dataTable3 != null)
                {
                    foreach (DataRow dataRow in dataTable3.Rows)
                    {
                        try
                        {
                            if (!(dataRow["item_ids"].ToString() == "") && (int)dataRow["amount"] > 0)
                            {
                                string BadgeID = dataRow["BadgeID"].ToString();
                                if (string.IsNullOrEmpty(BadgeID) || string.IsNullOrWhiteSpace(BadgeID)) BadgeID = string.Empty;
                                hashtable.Add((uint)dataRow["Id"], new CatalogItem((uint)dataRow["Id"], (string)dataRow["catalog_name"], (string)dataRow["item_ids"], (int)dataRow["cost_credits"], (int)dataRow["cost_pixels"], (int)dataRow["cost_snow"], (int)dataRow["amount"], (int)dataRow["page_id"], Essential.StringToInt(dataRow["vip"].ToString()), (uint)dataRow["achievement"], (int)dataRow["song_id"], BadgeID, (int)dataRow["limited_sold"], (int)dataRow["limited_count"], int.Parse(dataRow["have_offer"].ToString()) == 1, (int)dataRow["poster_id"], (string)dataRow["extradata"]));
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Error loading Item #" + dataRow["Id"].ToString());
                        }
                    }
                }
                if (dataTable != null)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        bool bool_ = false;
                        bool bool_2 = false;
                        if (dataRow["visible"].ToString() == "1")
                        {
                            bool_ = true;
                        }
                        if (dataRow["enabled"].ToString() == "1")
                        {
                            bool_2 = true;
                        }
                        this.dictionary_0.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (string)dataRow["link_name"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_, bool_2, (uint)dataRow["min_rank"], Essential.StringToBoolean(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable));
                    }
                }
                if (dataTable4 != null)
                {
                    foreach (DataRow dataRow in dataTable4.Rows)
                    {
                        bool bool_ = false;
                        bool bool_2 = false;
                        if (dataRow["visible"].ToString() == "1")
                        {
                            bool_ = true;
                        }
                        if (dataRow["enabled"].ToString() == "1")
                        {
                            bool_2 = true;
                        }
                        this.dictionary_0.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (string)dataRow["link_name"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_, bool_2, (uint)dataRow["min_rank"], Essential.StringToBoolean(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable));
                    }
                }
                if (dataTable2 != null)
                {
                    foreach (DataRow dataRow in dataTable2.Rows)
                    {
                        this.list_0.Add(new EcotronReward((uint)dataRow["Id"], (uint)dataRow["display_id"], (uint)dataRow["item_id"], (uint)dataRow["reward_level"]));
                    }
                }
                this.groupsDataMessage = GetGuildPage();
                Logging.WriteLine("completed!", ConsoleColor.Green);
                isLoading = false;
            }
		}
		internal void InitializeCache()
		{
			Logging.Write("Loading Catalogue Cache..");
		
           
                int num = Essential.GetGame().GetRoleManager().dictionary_2.Count + 1;
			this.Message5_0 = new ServerMessage[num];
			for (int i = 1; i < num; i++)
			{
				this.Message5_0[i] = this.SerializeIndexForCache(i);
			}
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
                current.InitMsg();
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
            
           
		}
		public CatalogItem GetItem(uint uint_1)
		{
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
				foreach (CatalogItem current2 in current.Items)
				{
					if (current2.uint_0 == uint_1)
					{
						return current2;
					}
				}
			}
			return null;
		}
        public List<CatalogPage> GetPagesByRank(uint rank)
        {
            List<CatalogPage> cps = new List<CatalogPage>();
            foreach (CatalogPage cp in dictionary_0.Values)
            {
                if (cp.MinRank == 1)
                {
                    cps.Add(cp);
                }
            }
            return cps;
        }
		public bool ItemExists(uint uint_1)
		{
			DataRow dataRow = null;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT Id FROM catalog_items WHERE item_ids = '" + uint_1 + "' LIMIT 1");
			}
			return dataRow != null;
		}
        public int GetTreeSize(int rank, int TreeId)
		{
			int num = 0;
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
                if ((ulong)current.MinRank <= (ulong)((long)rank) && current.ParentId == TreeId)
				{
					num++;
				}
			}
			return num;
		}
		public CatalogPage GetPage(int int_0)
		{
            if (this.dictionary_0.ContainsKey(int_0))
                return this.dictionary_0[int_0];
            return null;
		}
		public bool HandlePurchase(GameClient Session, int int_0, uint uint_1, string ExtraParams, bool bool_0, string string_1, string string_2, bool bool_1, int Amount, bool ShowGiftPurchaser = false)
		{
            int finalAmount = Amount;
            if (Amount >= 6)
                finalAmount -= 1;
            if (Amount >= 12)
                finalAmount -= 2;
            if (Amount >= 18)
                finalAmount -= 2;
            if (Amount >= 24)
                finalAmount -= 2;
            if (Amount >= 30)
                finalAmount -= 2;
            if (Amount >= 36)
                finalAmount -= 2;
            if (Amount >= 42)
                finalAmount -= 2;
            if (Amount >= 48)
                finalAmount -= 2;
            if (Amount >= 54)
                finalAmount -= 2;
            if (Amount >= 60)
                finalAmount -= 2;
            if (Amount >= 66)
                finalAmount -= 2;
            if (Amount >= 72)
                finalAmount -= 2;
            if (Amount >= 78)
                finalAmount -= 2;
            if (Amount >= 84)
                finalAmount -= 2;
            if (Amount >= 90)
                finalAmount -= 2;
            if (Amount >= 96)
                finalAmount -= 2;
            if (Amount >= 99)
                finalAmount -= 2;

            int LimitedCount = 0;
            int LimitedId = 0;
            string GuildData = "";
			CatalogPage @class = this.GetPage(int_0);
			if (@class == null || !@class.Enabled || !@class.Visible || @class.MinRank > Session.GetHabbo().Rank)
			{
				return false;
			}
			else
			{
                if (@class.ClubOnly && (!Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") || !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip")))
				{
					return false;
				}
				else
				{
                    CatalogItem class2 = @class.GetItem(uint_1);
					if (class2 == null)
					{
						return false;
					}
					else
					{
                        if (Session.GetHabbo().GetCredits() < class2.CreditsCost * Amount)
                            return false;
                        if (Session.GetHabbo().ActivityPoints < class2.PixelsCost * Amount && class2.int_2 == 0)
                            return false;
                        if (Session.GetHabbo().VipPoints < class2.PixelsCost * Amount && class2.int_2 > 0)
                            return false;
                        if (!class2.HaveOffer && Amount > 1)
                            return false;
						uint num = 0u;
						if (bool_0)
						{
                            if (!class2.GetBaseItem().AllowGift)
                            {
                                return false;
                            }
							if (Session.GetHabbo().method_4() > 0)
							{
								TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
								if (timeSpan.Seconds > 4)
								{
									Session.GetHabbo().int_23 = 0;
								}
								if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 3)
								{
									Session.GetHabbo().bool_15 = true;
									return false;
								}
								if (Session.GetHabbo().bool_15 && timeSpan.Seconds < Session.GetHabbo().method_4())
								{
									return false;
								}
								Session.GetHabbo().bool_15 = false;
								Session.GetHabbo().dateTime_0 = DateTime.Now;
								Session.GetHabbo().int_23++;
							}
							using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
							{
								class3.AddParamWithValue("gift_user", string_1);
								try
								{
									num = (uint)class3.ReadDataRow("SELECT Id FROM users WHERE username = @gift_user LIMIT 1")[0];
								}
								catch (Exception)
								{
								}
							}
							if (num == 0u)
							{
                                ServerMessage Message = new ServerMessage(Outgoing.GiftError); // Updated
								Message.AppendStringWithBreak(string_1);
								Session.SendMessage(Message);
								return false;
							}
						}
                      
                        if (class2.IsLimited)
                        {

                            if (class2.LimitedSold >= class2.LimitedCount)
                            {
                                Session.SendNotification("Sorry but we ran out of this Limited Edition!");
                                return false;
                            }
                            class2.LimitedSold++;
                     
                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                            {
                                dbClient.ExecuteQuery("UPDATE catalog_items SET limited_sold = " + class2.LimitedSold + " WHERE id = " + class2.uint_0);
                            }
                            LimitedId = class2.LimitedSold;
                            LimitedCount = class2.LimitedCount;
                            
                            @class.InitMsg(); // update page!
                            Session.SendMessage(@class.GetMessage);
                        }

						bool flag = false;
						bool flag2 = false;
						int int_ = class2.int_2;
                        if (bool_0)
                        {
                            if (Session.GetHabbo().GetCredits() < class2.CreditsCost)
						    {
							    flag = true;
						    }
                        }
                        else
                        {
                            if (Session.GetHabbo().GetCredits() < (class2.CreditsCost * finalAmount))
                            {
                                flag = true;
                            }
                        }
                        if (bool_0)
                        {
                            if ((int_ == 0 && Session.GetHabbo().ActivityPoints < class2.PixelsCost) || (int_ > 0 && Session.GetHabbo().VipPoints <class2.int_2))
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            if ((int_ == 0 && Session.GetHabbo().ActivityPoints < (class2.PixelsCost * finalAmount)) || (int_ > 0 && Session.GetHabbo().VipPoints < (class2.int_2 * finalAmount)))
                            {
                                flag2 = true;
                            }
                        }
                       
						if (flag || flag2)
						{
							/*ServerMessage Message2 = new ServerMessage(68u);
							Message2.AppendBoolean(flag);
							Message2.AppendBoolean(flag2);
							Session.SendMessage(Message2);
                            */
							return false;
						}
						else
						{
							if (bool_0 && class2.GetBaseItem().Type == 'e')
							{
								Session.SendNotification("Du kannst keine Effekte als Geschenk versenden!");
								return false;
							}
							else
							{
								string text = class2.GetBaseItem().InteractionType.ToLower();
								if (text != null)
								{
                                    if (text == "bot")
                                    {
                                        int LastId = 0;
                                        DataRow AIl = null;
                                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                        {
                                            AIl = dbClient.ReadDataRow("SHOW TABLE STATUS LIKE 'user_bots'");
                                            LastId = int.Parse(AIl["Auto_increment"].ToString());
                                            LastId++;
                                            dbClient.AddParamWithValue("name", class2.Name);
                                            dbClient.AddParamWithValue("look", class2.Extradata);
                                            dbClient.ExecuteQuery("INSERT INTO `user_bots` (`id`, `name`, `look`, `walk_mode`, `owner_id`,`bot_type`) VALUES ('" + LastId + "', @name, @look, 'freeroam', '" + Session.GetHabbo().Id + "', '" + class2.Extradata.Split('|')[1] +"')");
                                        }
                                        UserBot NewBot = this.CreateBot((uint)LastId, Session.GetHabbo().Id, class2.Extradata, class2.Name, class2.Extradata.Split('|')[1],"freeroam");
                                        Session.GetHabbo().GetInventoryComponent().AddBot(NewBot);
                                        goto IL_4FC;
                                    }
                                    if (!(text == "pet"))
									{
										if (text == "roomeffect")
										{
											double num2 = 0.0;
											try
											{
                                                num2 = double.Parse(ExtraParams);
											}
											catch (Exception)
											{
											}
                                            ExtraParams = num2.ToString().Replace(',', '.');
											goto IL_4FC;
										}
                                        if (text == "gld_furni")
                                        {
                                            GroupsManager GldData = Groups.GetGroupById(int.Parse(ExtraParams));
                                            GuildData = GldData.Id.ToString();
                                            goto IL_4FC;
                                        }
										if (text == "postit")
										{
                                            ExtraParams = "FFFF33";
											goto IL_4FC;
										}
										if (text == "dimmer")
										{
                                            ExtraParams = "1,1,1,#000000,255";
											goto IL_4FC;
										}
										if (text == "trophy")
										{
                                            ExtraParams = string.Concat(new object[]
											{
												Session.GetHabbo().Username,
												Convert.ToChar(9),
												DateTime.Now.Day,
												"-",
												DateTime.Now.Month,
												"-",
												DateTime.Now.Year,
												Convert.ToChar(9),
												ChatCommandHandler.ApplyFilter(Essential.DoFilter(ExtraParams, true, true))
											});
											goto IL_4FC;
										}
                                        if (text == "poster")
                                        {
                                            ExtraParams = class2.PosterId.ToString();
                                            goto IL_4FC;
                                        }
                                        if (text == "musicdisc")
                                        {
                                            ExtraParams = class2.song_id.ToString();
                                            goto IL_4FC;
                                        }
									}
									else
									{
										try
										{
                            
                                                string[] array = ExtraParams.Split(new char[]
											{
												'\n'
											});
                                                string string_3 = array[0];
                                                string text2 = array[1];
                                                string text3 = array[2];
                                                int.Parse(text2);
                                                if (!this.ValidPetName(string_3))
                                                {
                                                    return false;
                                                }
                                                if (text2.Length > 2)
                                                {
                                                    return false;
                                                }
                                                if (text3.Length != 6)
                                                {
                                                    return false;
                                                }
                                                goto IL_4FC;
                                            }
                                            
                                          
										catch (Exception)
										{
											return false;
										}
									}
								}
                                if (class2.Name.StartsWith("disc_"))
								{
                                    ExtraParams = class2.Name.Split(new char[]
									{
										'_'
									})[1];
								}
								else
								{
                                    ExtraParams = "";
								}
								IL_4FC:
                                if (class2.CreditsCost > 0)
								{

                                    if (bool_0)
                                    {
                                        Session.GetHabbo().TakeCredits(class2.CreditsCost, "Purchase", class2.uint_0 +"");
                                    }
                                    else
                                    {
                                        Session.GetHabbo().TakeCredits((class2.CreditsCost * finalAmount),"Purchase", class2.uint_0 + "*" + Amount);
                                    }
                                    
									Session.GetHabbo().UpdateCredits(true);
								}
								if (class2.PixelsCost > 0 && int_ == 0)
								{
                                     if (bool_0)
                                     {
                                         Session.GetHabbo().ActivityPoints -= class2.PixelsCost; 
                                     }
                                     else
                                     {
                                         Session.GetHabbo().ActivityPoints -= (class2.PixelsCost * finalAmount); 
                                     }
                                   
									Session.GetHabbo().UpdateActivityPoints(true);
								}
								else
								{
                                    if (class2.PixelsCost > 0 && int_ > 0)
									{
                                        if (bool_0)
                                        {
                                            Session.GetHabbo().VipPoints -= class2.PixelsCost;
                                        }
                                        else
                                        {
                                            Session.GetHabbo().VipPoints -= (class2.PixelsCost * finalAmount);
                                        }
                                       
										Session.GetHabbo().method_16(0);
										Session.GetHabbo().UpdateVipPoints(false, true);
									}
								}
                                ServerMessage Message3 = new ServerMessage(Outgoing.PurchaseOK); // F
                                Message3.AppendInt32(class2.GetBaseItem().UInt32_0);
                                Message3.AppendString(class2.GetBaseItem().Name);
                                Message3.AppendInt32(class2.CreditsCost);
                                Message3.AppendInt32(class2.PixelsCost);
                                Message3.AppendInt32(0);
                                Message3.AppendBoolean(true);
                                Message3.AppendInt32(1);
                                Message3.AppendString(class2.GetBaseItem().Type.ToString().ToLower());
                                Message3.AppendInt32(class2.GetBaseItem().Sprite);
                                Message3.AppendString("");
                                Message3.AppendInt32(1);
                                Message3.AppendInt32(0);
                                Message3.AppendString(ExtraParams);
                                Message3.AppendInt32(1);
								Session.SendMessage(Message3);
                                /*ServerMessage Message55 = new ServerMessage(Outgoing.SendPurchaseAlert);
                                Message55.AppendInt32(1);
                                int i = 2;
                                if (class2.method_0().Type.ToString().ToLower().Equals("s"))
                                {
                                    if (class2.method_0().InteractionType.ToLower() == "pet")
                                    {
                                        i = 3;
                                    }
                                    else
                                    {
                                        i = 1;
                                    }
                                }
                                Message55.AppendInt32(i);
                                Message55.AppendInt32(1);
                                Message55.AppendInt32(class2.uint_0);
                                Session.SendMessage(Message55);*/
								if (bool_0)
								{
									uint num3 = this.GetNextId();
									Item class4 = this.GetGiftPackage();
									using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
									{
                                        if (ShowGiftPurchaser)
                                        {
                                            class3.AddParamWithValue("gift_message", "true" + (char)5 + ChatCommandHandler.ApplyFilter(Essential.DoFilter(string_2, true, true)) + (char)5 + Session.GetHabbo().Id);
                                        }
                                        else
                                        {
                                            class3.AddParamWithValue("gift_message", "false" + (char)5 + ChatCommandHandler.ApplyFilter(Essential.DoFilter(string_2, true, true)));
                                        }
										
										class3.AddParamWithValue("extra_data", ExtraParams);
										class3.ExecuteQuery(string.Concat(new object[]
										{
											"INSERT INTO items (Id,user_id,base_item,wall_pos, ltd_id, ltd_cnt, guild_data) VALUES ('",
											num3,
											"','",
											num,
											"','",
											class4.UInt32_0,
											"','','",
											LimitedId,
											"', '",
											LimitedCount,
											"', '",
											GuildData,
											"')"
										}));
                                        class3.ExecuteQuery(string.Concat(new object[]
						                {
							                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
							                num3,
							                "',@gift_message)"
						                }));
										class3.ExecuteQuery(string.Concat(new object[]
										{
											"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
											num3,
											"','",
											class2.GetBaseItem().UInt32_0,
											"','",
											class2.int_3,
											"',@extra_data)"
										}));
                                        class3.ExecuteQuery("INSERT INTO present_log (item_id,base_id,amount,giftmessage,fromid,toid) VALUES (" + num3 + "," + class2.GetBaseItem().UInt32_0 + "," + class2.int_3 +",@gift_message,"+ Session.GetHabbo().Id + "," + num + ")");
									}
									GameClient class5 = Essential.GetGame().GetClientManager().GetClient(num);
									if (class5 != null)
									{
                                        if(class5.GetHabbo().GiftAlert)
										    class5.SendNotification("Du hast ein Geschenk erhalten. Überprüf deinen Inventar!");
										class5.GetHabbo().GetInventoryComponent().method_9(true);
										class5.GetHabbo().GiftsReceived++;
                                        class5.GetHabbo().CheckGiftReceivedAchievements();
                                        ServerMessage Message555 = new ServerMessage(Outgoing.SendPurchaseAlert);
                                        Message555.AppendInt32(1);
                                        Message555.AppendInt32(1);
                                        Message555.AppendInt32(1);
                                        Message555.AppendInt32(class2.uint_0);
                                        class5.SendMessage(Message555);
									}
									Session.GetHabbo().GiftsGiven++;
                                    Session.GetHabbo().CheckGiftGivenAchievements();
									Session.SendNotification("Geschenk versendet!");
									return true;
								}
								else
								{
                                    this.AddItemToInventory(Session, class2.GetBaseItem(), (Amount * class2.int_3), ExtraParams, true, 0u, LimitedId, LimitedCount, GuildData);
									if (class2.uint_2 > 0u)
									{
										Essential.GetGame().GetAchievementManager().addAchievement(Session, class2.uint_2, 1);
									}
                                    if (!string.IsNullOrEmpty(class2.BadgeID))
                                    {
                                        Session.GetHabbo().GetBadgeComponent().SendBadge(Session, class2.BadgeID, true);
                                    }
									return true;
								}
							}
						}
					}
				}
			}
		}
		public void CreateGift(string string_0, uint uint_1, uint uint_2, int int_0)
		{
			CatalogPage @class = this.GetPage(int_0);
            CatalogItem class2 = @class.GetItem(uint_2);
			uint num = this.GetNextId();
			Item class3 = this.GetGiftPackage();
			using (DatabaseClient class4 = Essential.GetDatabase().GetClient())
			{
				class4.AddParamWithValue("gift_message", "!" + ChatCommandHandler.ApplyFilter(Essential.DoFilter(string_0, true, true)));
				class4.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('",
					num,
					"','",
					uint_1,
					"','",
					class3.UInt32_0,
					"','')"
				}));
                class4.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
							num,
							"',@gift_message)"
						}));
				class4.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
					num,
					"','",
					class2.GetBaseItem().UInt32_0,
					"','",
					class2.int_3,
					"','')"
				}));
			}
			GameClient class5 = Essential.GetGame().GetClientManager().GetClient(uint_1);
			if (class5 != null)
			{
                if (class5.GetHabbo().GiftAlert)
				    class5.SendNotification("Du hast ein Geschenk erhalten. Überprüf deinen Inventar!");
				class5.GetHabbo().GetInventoryComponent().method_9(true);
			}
		}
		public bool ValidPetName(string string_0)
		{
			return string_0.Length >= 1 && string_0.Length <= 16 && Essential.smethod_9(string_0) && !(string_0 != ChatCommandHandler.ApplyFilter(string_0));
		}
        public void AddItemToInventory(GameClient Session, Item Item, int int_0, string string_0, bool bool_0, uint uint_1, int ltd_id, int ltd_cnt, string GuildData)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                string text = Item.Type.ToString();
                if (text != null)
                {
                    if (text == "i" || text == "s" && text != "r")
                    {
                        int i = 0;
                        while (i < int_0)
                        {
                            uint num;
                            if (!bool_0 && uint_1 > 0u)
                            {
                                num = uint_1;
                            }
                            else
                            {
                                num = this.GetNextId();
                            }
                            text = Item.InteractionType.ToLower();
                            if (text == null)
                            {
                                goto IL_4CF;
                            }
                            if (!(text == "pet"))
                            {
                                if (!(text == "teleport") && !(text == "instant_teleport"))
                                {
                                    if (!(text == "dimmer"))
                                    {
                                        goto IL_4CF;
                                    }
                                    using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                                    {
                                        @class.ExecuteQuery("INSERT INTO room_items_moodlight (item_id,enabled,current_preset,preset_one,preset_two,preset_three) VALUES ('" + num + "','0','1','#000000,255,0','#000000,255,0','#000000,255,0')");
                                    }
                                    Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, string_0, bool_0, ltd_id, ltd_cnt, GuildData);
                                }
                                else
                                {
                                    uint num2 = this.GetNextId();
                                    using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                                    {
                                        @class.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('",
										num,
										"','",
										num2,
										"')"
									}));
                                        @class.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('",
										num2,
										"','",
										num,
										"')"
									}));
                                    }
                                    Session.GetHabbo().GetInventoryComponent().method_11(num2, Item.UInt32_0, "0", bool_0, ltd_id, ltd_cnt, GuildData);
                                    Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, "0", bool_0, ltd_id, ltd_cnt, GuildData);
                                }
                            }
                            else
                            {
                                string[] array = string_0.Split(new char[]
							{
								'\n'
							});
                                Pet class15_ = this.CreatePet(Session.GetHabbo().Id, array[0], Convert.ToInt32(Item.Name.Split(new char[]
							{
								't'
							})[1]), array[1], array[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(class15_);
                                Session.GetHabbo().GetInventoryComponent().method_11(num, 320u, "0", bool_0, ltd_id, ltd_cnt, GuildData);
                            }
                        IL_4EA:
                            ServerMessage Message = new ServerMessage(Outgoing.UnseenItems); // P
                            Message.AppendInt32(1);
                            if (Item.InteractionType.ToLower() == "pet")
                            {
                                Message.AppendInt32(3);
                                Session.GetHabbo().NewPetsBuyed++;
                                Session.GetHabbo().CheckPetCountAchievements();
                            }
                            else
                            {
                                if (Item.Type.ToString() == "i")
                                {
                                    Message.AppendInt32(2);
                                }
                                else
                                {
                                    Message.AppendInt32(1);
                                }
                            }
                            Message.AppendInt32(1);
                            Message.AppendUInt(num);
                            Session.SendMessage(Message);
                            i++;
                            continue;
                        IL_4CF:
                            Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, string_0, bool_0, ltd_id, ltd_cnt, GuildData);
                            goto IL_4EA;
                        }
                        Session.GetHabbo().GetInventoryComponent().method_9(false);
                        return;
                    }
                    if (text == "e")
                    {
                        for (int i = 0; i < int_0; i++)
                        {
                            Session.GetHabbo().GetEffectsInventoryComponent().method_0(Item.Sprite, 3600);
                        }
                        return;
                    }
                    if (text == "h")
                    {
                        for (int i = 0; i < int_0; i++)
                        {
                            Session.GetHabbo().GetSubscriptionManager().method_3("habbo_club", 2678400);
                            Session.GetHabbo().CheckHCAchievements();
                        }
                        ServerMessage Message2 = new ServerMessage(Outgoing.SerializeClub); // Updated
                        Message2.AppendStringWithBreak("habbo_club");
                        if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                        {
                            double num3 = (double)Session.GetHabbo().GetSubscriptionManager().GetSubscriptionByType("habbo_club").ExpirationTime;
                            double num4 = num3 - Essential.GetUnixTimestamp();
                            int num5 = (int)Math.Ceiling(num4 / 86400.0);
                            int num6 = num5 / 31;
                            if (num6 >= 1)
                            {
                                num6--;
                            }
                            Message2.AppendInt32(num5 - num6 * 31);
                            Message2.AppendInt32(2);
                            Message2.AppendInt32(0);
                            Message2.AppendInt32(1);
                            Message2.AppendBoolean(true);
                            Message2.AppendBoolean(true);
                            Message2.AppendInt32(num6);
                            Message2.AppendInt32(0);
                            Message2.AppendInt32(0);
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Message2.AppendInt32(0);
                            }
                        }
                        Session.SendMessage(Message2);
                        ServerMessage Message3 = new ServerMessage(Outgoing.Fuserights); // Updated
                        if (Session.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes)
                        {
                            Message3.AppendInt32(2);
                        }
                        else
                        {
                            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                            {
                                Message3.AppendInt32(1);
                            }
                            else
                            {
                                Message3.AppendInt32(0);
                            }
                        }
                        if (Session.GetHabbo().HasFuse("acc_anyroomowner"))
                        {
                            Message3.AppendInt32(7);
                        }
                        else
                        {
                            if (Session.GetHabbo().HasFuse("acc_anyroomrights"))
                            {
                                Message3.AppendInt32(5);
                            }
                            else
                            {
                                if (Session.GetHabbo().HasFuse("acc_supporttool"))
                                {
                                    Message3.AppendInt32(4);
                                }
                                else
                                {
                                    if (Session.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes || Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                                    {
                                        Message3.AppendInt32(2);
                                    }
                                    else
                                    {
                                        Message3.AppendInt32(0);
                                    }
                                }
                            }
                        }
                        Session.SendMessage(Message3);
                        return;
                    }
                }
                if(text != "r")
                    Session.SendNotification("Something went wrong! The item type could not be processed. Please do not try to buy this item anymore, instead inform support as soon as possible.");
            }
        }
		public Item GetGiftPackage()
		{
			switch (Essential.smethod_5(0, 6))
			{
			case 0:
			{
                return Essential.GetGame().GetItemManager().GetItemById(164u);
			}
			case 1:
			{
				return Essential.GetGame().GetItemManager().GetItemById(165u);
			}
			case 2:
			{
				return Essential.GetGame().GetItemManager().GetItemById(166u);
			}
			case 3:
			{
				return Essential.GetGame().GetItemManager().GetItemById(167u);
			}
			case 4:
			{
                return Essential.GetGame().GetItemManager().GetItemById(168u);
			}
			case 5:
			{
                return Essential.GetGame().GetItemManager().GetItemById(169u);
			}
			case 6:
			{
                return Essential.GetGame().GetItemManager().GetItemById(170u);
			}
            default:
            {
                return null;
            }
			}
		}
        public UserBot CreateBot(uint BotId, uint OwnerId, string Look, string Name, string type, string wm)
        {
            return new UserBot(BotId, OwnerId, Look, Name, false, 0,0,0,type,wm)
            {
                DBState = DatabaseUpdateState.NeedsInsert
            };
        }
		public Pet CreatePet(uint uint_1, string string_0, int int_0, string string_1, string string_2)
		{
			return new Pet(this.GetNextId(), uint_1, 0u, string_0, (uint)int_0, string_1, string_2, 0, 100, 100, 0, Essential.GetUnixTimestamp(), 0, 0, 0.0)
			{
				DBState = DatabaseUpdateState.NeedsInsert
			};
		}
        public UserBot RetrBot(DataRow dataRow_0)
        {
            if (dataRow_0 == null)
            {
                return null;
            }
            else
            {
                return new UserBot(uint.Parse(dataRow_0["id"].ToString()), uint.Parse(dataRow_0["owner_id"].ToString()), dataRow_0["look"].ToString(), dataRow_0["name"].ToString(), (uint.Parse(dataRow_0["room_id"].ToString()) > 0), int.Parse(dataRow_0["room_id"].ToString()),(int)dataRow_0["x"], (int)dataRow_0["y"], dataRow_0["bot_Type"].ToString(),dataRow_0["walk_mode"].ToString());
            }
        }
		public Pet GetPet(DataRow dataRow_0)
		{
			if (dataRow_0 == null)
			{
				return null;
			}
			else
			{
				return new Pet((uint)dataRow_0["Id"], (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
			}
		}
		internal Pet GetPet(DataRow dataRow_0, uint uint_1)
		{
			if (dataRow_0 == null)
			{
				return null;
			}
			else
			{
				return new Pet(uint_1, (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
			}
		}
		internal uint GetNextId()
		{
			lock (this.object_0)
			{
				return this.uint_0++;
			}
		}
		public EcotronReward GetEcotronReward()
		{
			uint uint_ = 1u;
			if (Essential.smethod_5(1, 2000) == 2000)
			{
				uint_ = 5u;
			}
			else
			{
				if (Essential.smethod_5(1, 200) == 200)
				{
					uint_ = 4u;
				}
				else
				{
					if (Essential.smethod_5(1, 40) == 40)
					{
						uint_ = 3u;
					}
					else
					{
						if (Essential.smethod_5(1, 4) == 4)
						{
							uint_ = 2u;
						}
					}
				}
			}
			List<EcotronReward> list = this.GetEcotronRewards(uint_);
			if (list != null && list.Count >= 1)
			{
				return list[Essential.smethod_5(0, list.Count - 1)];
			}
			else
			{
				return new EcotronReward(0u, 0u, 1479u, 0u);
			}
		}
		public List<EcotronReward> GetEcotronRewards(uint uint_1)
		{
			List<EcotronReward> list = new List<EcotronReward>();
			foreach (EcotronReward current in this.list_0)
			{
				if (current.uint_3 == uint_1)
				{
					list.Add(current);
				}
			}
			return list;
		}
        internal Dictionary<uint, List<EcotronReward>> GetEcotronRewards()
        {
            Dictionary<uint, List<EcotronReward>> list = new Dictionary<uint, List<EcotronReward>>();
            foreach (EcotronReward reward in this.list_0)
            {

                if (!list.ContainsKey(reward.uint_3))
                {
                    list.Add(reward.uint_3, new List<EcotronReward>());
                    list[reward.uint_3].Add(reward);

                }
                else
                {
                    list[reward.uint_3].Add(reward);

                }

            }
            return list;
        }
        public ServerMessage SerializeIndexForCache(int rank)
		{
            ServerMessage Message = new ServerMessage(Outgoing.OpenShop);
			Message.AppendBoolean(true);
			Message.AppendInt32(0);
			Message.AppendInt32(0);
			Message.AppendInt32(-1);
            Message.AppendString("root");
            Message.AppendString("");
			Message.AppendInt32(this.GetTreeSize(rank, -1));
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
                if (current.ParentId == -1)
				{
                    current.Serialize(rank, Message);
					foreach (CatalogPage current2 in this.dictionary_0.Values)
					{
                        if (current2.ParentId == current.PageId)
						{
                            current2.Serialize(rank, Message);
						}
					}
				}
			}
            Message.AppendBoolean(false);
			return Message;
		}
		internal ServerMessage GetIndexMessageForRank(uint uint_1)
		{
			if (uint_1 < 1u)
			{
				uint_1 = 1u;
			}
			if ((ulong)uint_1 > (ulong)((long)Essential.GetGame().GetRoleManager().dictionary_2.Count))
			{
				uint_1 = (uint)Essential.GetGame().GetRoleManager().dictionary_2.Count;
			}
			return this.Message5_0[(int)((UIntPtr)uint_1)];
		}
        public ServerMessage SerializePage(CatalogPage Page)
		{
            ServerMessage Message = new ServerMessage(Outgoing.OpenShopPage);
            Message.AppendInt32(Page.PageId);

            switch (Page.Layout)
			{
			case "frontpage":
				Message.AppendStringWithBreak("frontpage3");
				Message.AppendInt32(3);
                Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(11);
                Message.AppendStringWithBreak(Page.string_5.Split((char)2)[0]);
                Message.AppendStringWithBreak(Page.string_5.Split((char)2)[1]);
                Message.AppendStringWithBreak("");
                Message.AppendStringWithBreak(Page.string_6.Split((char)2)[0]);
                Message.AppendStringWithBreak(Page.string_6.Split((char)2)[1]);
                Message.AppendStringWithBreak(Page.TextDetails);
                Message.AppendStringWithBreak("");
                Message.AppendStringWithBreak("#FEFEFE");
                Message.AppendStringWithBreak("#FEFEFE");
                Message.AppendStringWithBreak("Weiterlesen >>");
                Message.AppendStringWithBreak("credits");

               /* Message.AppendStringWithBreak(Page.string_5.Split((char)2)[0]);
                Message.AppendStringWithBreak(Page.TextDetails);
                Message.AppendStringWithBreak("");
                Message.AppendStringWithBreak(Page.string_7);
                Message.AppendStringWithBreak(Page.TextTeaser);
				Message.AppendStringWithBreak("#FAF8CC");
				Message.AppendStringWithBreak("#FAF8CC");
				Message.AppendStringWithBreak("Lue lisää >>");
				Message.AppendStringWithBreak("magic.credits");
                    */
				break;
			case "recycler_info":
                Message.AppendStringWithBreak(Page.Layout);
				Message.AppendInt32(2);
                Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
				Message.AppendInt32(3);
                Message.AppendStringWithBreak(Page.string_5);
                Message.AppendStringWithBreak(Page.string_6);
                Message.AppendStringWithBreak(Page.string_7);
				break;
			case "recycler_prizes":
				Message.AppendStringWithBreak("recycler_prizes");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak("catalog_recycler_headline3");
				Message.AppendInt32(1);
                Message.AppendStringWithBreak(Page.string_5);
				break;
			case "spaces":
				Message.AppendStringWithBreak("spaces_new");
				Message.AppendInt32(1);
                Message.AppendStringWithBreak(Page.LayoutHeadline);
				Message.AppendInt32(1);
                Message.AppendStringWithBreak(Page.string_5);
				break;
			case "recycler":
                Message.AppendStringWithBreak(Page.Layout);
				Message.AppendInt32(2);
                Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
				Message.AppendInt32(3);
				Message.AppendStringWithBreak(Page.string_5, 10);
				Message.AppendStringWithBreak(Page.string_6);
				Message.AppendStringWithBreak(Page.string_7);
				break;
			case "trophies":
				Message.AppendStringWithBreak("trophies");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak(Page.LayoutHeadline);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(Page.string_5);
				Message.AppendStringWithBreak(Page.string_7);
				break;
			case "pets":
				Message.AppendStringWithBreak("pets");
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
				Message.AppendInt32(4);
				Message.AppendStringWithBreak(Page.string_5);
				Message.AppendStringWithBreak("Wähl einen Namen:");
				Message.AppendStringWithBreak("Wähl eine Farbe:");
				Message.AppendStringWithBreak("Wähl eine Rasse:");
				break;
			case "club_buy":
                Message.AppendString("vip_buy");
                Message.AppendInt32(2);
                Message.AppendString("ctlg_buy_vip_header");
                Message.AppendString("ctlg_gift_vip_teaser");
                Message.AppendInt32(0);
				break;
			case "soundmachine":
                Message.AppendStringWithBreak(Page.Layout);
				Message.AppendInt32(2);
                Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(Page.string_5);
				Message.AppendStringWithBreak(Page.string_7);
				break;
            case "bots":
                Message.AppendStringWithBreak(Page.Layout);
                Message.AppendInt32(3);
                Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
                Message.AppendStringWithBreak(Page.LayoutSpecial);
                Message.AppendInt32(3);
                Message.AppendStringWithBreak(Page.string_5);
                Message.AppendStringWithBreak(Page.string_7);
                Message.AppendStringWithBreak(Page.string_8);
                break;
            case "guilds":
                Message.AppendString("guild_frontpage");
                Message.AppendInt32(2);
                Message.AppendString(Page.LayoutHeadline);
                Message.AppendString(Page.LayoutTeaser);
                Message.AppendInt32(3);
                Message.AppendString(Page.string_5);
                Message.AppendString(Page.TextDetails.Replace("[13]", Convert.ToChar(13).ToString()).Replace("[10]", Convert.ToChar(13).ToString()));
                Message.AppendString(Page.TextTeaser);
                break;
            case "guild_furni":
                Message.AppendString("guild_custom_furni");
                Message.AppendInt32(3);
                Message.AppendString(Page.LayoutHeadline);
                Message.AppendString(Page.LayoutTeaser);
                Message.AppendString(Page.LayoutSpecial);
                Message.AppendInt32(3);
                Message.AppendString(Page.string_5);
                Message.AppendString(Page.string_7);
                Message.AppendString(Page.string_8);
                break;
            case "roomads":
                Message.AppendString("roomads");
                Message.AppendInt32(2);
                Message.AppendString("events_header");
                Message.AppendString("");
                Message.AppendInt32(2);
                Message.AppendInt32(0);//0
                Message.AppendInt32(1);
                Message.AppendInt32(7384);
                Message.AppendString("room_ad_plus_badge");
                Message.AppendInt32(0);//0
                Message.AppendString("");
                Message.AppendInt32(3840);
                Message.AppendInt32(0);//0
                Message.AppendInt32(1);
                Message.AppendString("b");
                Message.AppendString("RADZZ");
                break;
            case "camera1":
                Message.AppendString("camera2");
                Message.AppendInt32(3);
                Message.AppendString(Page.LayoutHeadline);
                Message.AppendString(Page.LayoutTeaser);
                Message.AppendString(Page.LayoutSpecial);
                Message.AppendInt32(3);
                Message.AppendString(Page.string_5);
                Message.AppendString(Page.string_7);
                Message.AppendString(Page.string_8);
                break;
            default:
                Message.AppendStringWithBreak(Page.Layout);
			    Message.AppendInt32(3);
			    Message.AppendStringWithBreak(Page.LayoutHeadline);
                Message.AppendStringWithBreak(Page.LayoutTeaser);
			    Message.AppendStringWithBreak(Page.LayoutSpecial);
			    Message.AppendInt32(3);
			    Message.AppendStringWithBreak(Page.string_5);
			    Message.AppendStringWithBreak(Page.string_7);
			    Message.AppendStringWithBreak(Page.string_8);
            break;
			}
            if (!Page.Layout.Equals("frontpage") && !Page.Layout.Equals("club_buy"))
            {
			    Message.AppendInt32(Page.Items.Count);
                foreach (CatalogItem current in Page.Items)
			    {
                    try
                    {
                        current.Serialize(Message);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("OOPS! Dieses Möbel wurde nicht berechnet: #" + current.Name);
                    }
                }
                Message.AppendInt32(-1);
                Message.AppendBoolean(false);
            }
            else
            {
                Message.AppendInt32(0);
                Message.AppendInt32(-1);
                Message.AppendBoolean(false);
            }
			return Message;
		}
		public VoucherHandler GetVoucherHandler()
		{
			return this.VoucherHandler_0;
		}
		public Marketplace GetMarketplace()
		{
			return this.class43_0;
		}
        public static ServerMessage GetGuildPage()
        {
            ServerMessage message = new ServerMessage(Outgoing.SendGuildElements);
            message.AppendInt32(GroupsPartsData.BaseBadges.Count);
            foreach (GroupsPartsData data in GroupsPartsData.BaseBadges)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
                message.AppendString(data.ExtraData2);
            }
            message.AppendInt32(GroupsPartsData.SymbolBadges.Count);
            foreach (GroupsPartsData data in GroupsPartsData.SymbolBadges)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
                message.AppendString(data.ExtraData2);
            }
            message.AppendInt32(GroupsPartsData.ColorBadges1.Count);
            foreach (GroupsPartsData data in GroupsPartsData.ColorBadges1)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
            }
            message.AppendInt32(GroupsPartsData.ColorBadges2.Count);
            foreach (GroupsPartsData data in GroupsPartsData.ColorBadges2)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
            }
            message.AppendInt32(GroupsPartsData.ColorBadges3.Count);
            foreach (GroupsPartsData data in GroupsPartsData.ColorBadges3)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
            }
            return message;
        }
	}
}
