using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.Catalogs;
using Essential.Catalogs;
namespace Essential.HabboHotel.Catalogs
{
	internal sealed class Marketplace
	{
		public List<uint> list_0;
		public List<MarketplaceOffers> list_1;
		public Dictionary<int, int> dictionary_0;
		public Dictionary<int, int> dictionary_1;
		public Marketplace()
		{
			this.list_0 = new List<uint>();
			this.list_1 = new List<MarketplaceOffers>();
			this.dictionary_0 = new Dictionary<int, int>();
			this.dictionary_1 = new Dictionary<int, int>();
		}
		public bool CanSell(UserItem class39_0)
		{
			return class39_0.GetBaseItem().AllowTrade && class39_0.GetBaseItem().AllowMarketplaceSell;
		}
		public void CreateOffer(GameClient Session, uint uint_0, int int_0)
		{
			/*UserItem @class = Session.GetHabbo().GetInventoryComponent().GetItemById(uint_0);
			if (@class == null || int_0 > ServerConfiguration.MarketplacePriceLimit || !this.method_0(@class))
			{
                ServerMessage Message = new ServerMessage(Outgoing.CanMakeOffer); // Updated
                Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
			else
			{
				int num = this.method_2((float)int_0);
				int num2 = int_0 + num;
				int num3 = 1;
				if (@class.method_1().Type == 'i')
				{
					num3++;
				}
				using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
				{
					class2.AddParamWithValue("public_name", @class.method_1().PublicName);
					class2.AddParamWithValue("extra_data", @class.string_0);
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO catalog_marketplace_offers (furni_id, item_id,user_id,asking_price,total_price,public_name,sprite_id,item_type,timestamp,extra_data) VALUES ('",
						uint_0,
						"','",
						@class.uint_1,
						"','",
						Session.GetHabbo().Id,
						"','",
						int_0,
						"','",
						num2,
						"',@public_name,'",
						@class.method_1().Sprite,
						"','",
						num3,
						"','",
						Essential.GetUnixTimestamp(),
						"',@extra_data)"
					}));
				}
                Session.GetHabbo().GetInventoryComponent().method_12(uint_0, 0u, true);
                ServerMessage Message2 = new ServerMessage(Outgoing.CanMakeOffer); // Updated
                Message2.AppendInt32(1);
				Session.SendMessage(Message2);
			}*/
		}
		public int GetCosts(float float_0)
		{
			double num = (double)(float_0 / 100f);
			return (int)Math.Ceiling((double)((float)(num * (double)ServerConfiguration.MarketplaceTax)));
		}
		internal double FormatTimestamp()
		{
			return Essential.GetUnixTimestamp() - 172800.0;
		}
		internal string GetSeconds()
		{
			return this.FormatTimestamp().ToString().Split(new char[]
			{
				','
			})[0];
		}
        public ServerMessage GetMarketPlaceOffers(int minprice, int maxprice, string SearchString, int order, GameClient Session)
        {
            List<CatalogItem> items = new List<CatalogItem>();
            Dictionary<CatalogItem, uint> Result = new Dictionary<CatalogItem, uint>();
            List<CatalogPage> catalogPages = Essential.GetGame().GetCatalog().GetPagesByRank(Session.GetHabbo().Rank);
            foreach (CatalogPage cp in catalogPages)
            {
                foreach (CatalogItem ci in cp.GetItems())
                {
                    if (ci.GetBaseItem().Type.ToString().ToLower() == "s")
                        items.Add(ci);
                }

            }
            Console.WriteLine(items.Count);
            uint virtualId = 0;
            foreach (CatalogItem ci in items)
            {

                if (SearchString.Length > 0)
                {
                    if (ci.Name.ToLower().Contains(SearchString.ToLower()))
                    {
                        if (!Result.ContainsKey(ci))
                        {
                            virtualId = virtualId + 1;
                            Result.Add(ci, virtualId);
                        }
                    }
                }
            }
            if (Result.Count == 0 && SearchString.Length == 0)
            {
                foreach (CatalogItem ci in items)
                {
                    virtualId = virtualId + 1;
                    Result.Add(ci, virtualId);
                }
            }
            Console.WriteLine(Result.Count + "");
            ServerMessage Message = new ServerMessage(Outgoing.Marketplace);
            Message.AppendInt32(Result.Count);
            foreach (CatalogItem ci in Result.Keys)
            {
                Message.AppendUInt(ci.uint_0);
                Message.AppendInt32(1);
                Message.AppendInt32(1);
                Message.AppendInt32(ci.GetBaseItem().Sprite);
                Message.AppendInt32(0);
                Message.AppendStringWithBreak("");
                Message.AppendInt32(ci.CreditsCost);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(1);
            }
            Message.AppendInt32(0);
            Message.AppendInt32(0);
            return Message;
		}
		public ServerMessage SerializeMarketplace(int int_0, int int_1)
		{
			Dictionary<int, MarketplaceOffers> dictionary = new Dictionary<int, MarketplaceOffers>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
            ServerMessage Message = new ServerMessage(Outgoing.Marketplace);
			foreach (MarketplaceOffers current in this.list_1)
			{
				if (dictionary.ContainsKey(current.int_0))
				{
					if (dictionary[current.int_0].int_1 > current.int_1)
					{
						dictionary.Remove(current.int_0);
						dictionary.Add(current.int_0, current);
					}
					int num = dictionary2[current.int_0];
					dictionary2.Remove(current.int_0);
					dictionary2.Add(current.int_0, num + 1);
				}
				else
				{
					dictionary.Add(current.int_0, current);
					dictionary2.Add(current.int_0, 1);
				}
			}
			if (dictionary.Count > 0)
			{
				Message.AppendInt32(dictionary.Count);
				using (Dictionary<int, MarketplaceOffers>.Enumerator enumerator2 = dictionary.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, MarketplaceOffers> current2 = enumerator2.Current;

                        /*
                         *                         message.AppendInt32(int.Parse(row["offer_id"].ToString()));
                        message.AppendInt32(1);
                        message.AppendInt32(1);
                        message.AppendInt32((int) row["sprite_id"]);
                        message.AppendInt32(0);
                        message.AppendString("");
                        message.AppendInt32((int) row["total_price"]);
                        message.AppendInt32(0);
                        message.AppendInt32(avarage[0]);
                        message.AppendInt32(avarage[1]);
                         * */
						Message.AppendUInt(current2.Value.uint_0);
						Message.AppendInt32(1);
						//Message.AppendInt32(1);
                        Message.AppendInt32(current2.Value.int_2);
                        if (current2.Value.int_2 == 2)
                        {
                            Message.AppendInt32(current2.Value.int_0);
                            using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                            {

                            
					int FurniId = @class.ReadInt32("SELECT furni_id FROM catalog_marketplace_offers WHERE offer_id = '" + current2.Value.uint_0 + "' LIMIT 1");

                    Message.AppendStringWithBreak((@class.ReadInt32("SELECT COUNT(extra_data) FROM items_extra_data WHERE item_id = '" + FurniId + "'") > 0 ? @class.ReadString("SELECT extra_data FROM items_extra_data WHERE item_id = '" + FurniId + "'") : ""));
                    }
                            }
                        else
                        {
                            Message.AppendInt32(current2.Value.int_0);
                            Message.AppendInt32(0);
                            Message.AppendStringWithBreak("");

                        }
						Message.AppendInt32(current2.Value.int_1);
						Message.AppendInt32(0);
						Message.AppendInt32(this.GetAveragePrice(current2.Value.int_0));
						Message.AppendInt32(dictionary2[current2.Value.int_0]);
					}
                    Message.AppendInt32(0);
					return Message;

				}
			}
			Message.AppendInt32(0);
            Message.AppendInt32(0);
			return Message;
		}
		public int GetAveragePrice(int int_0)
		{
			int num = 0;
			int num2 = 0;
			if (this.dictionary_0.ContainsKey(int_0) && this.dictionary_1.ContainsKey(int_0))
			{
				if (this.dictionary_1[int_0] > 0)
				{
					return this.dictionary_0[int_0] / this.dictionary_1[int_0];
				}
				else
				{
					return 0;
				}
			}
			else
			{
				try
				{
					using (DatabaseClient @class = Essential.GetDatabase().GetClient())
					{
						num = @class.ReadInt32("SELECT avgprice FROM catalog_marketplace_data WHERE sprite = '" + int_0 + "' AND daysago = 0 LIMIT 1;");
						num2 = @class.ReadInt32("SELECT sold FROM catalog_marketplace_data WHERE sprite = '" + int_0 + "' AND daysago = 0 LIMIT 1;");
					}
				}
				catch
				{
				}
				this.dictionary_0.Add(int_0, num);
				this.dictionary_1.Add(int_0, num2);
				if (num2 > 0)
				{
					return (int)Math.Ceiling((double)((float)(num / num2)));
				}
				else
				{
					return 0;
				}
			}
		}
		public ServerMessage SerializeMyOffers(uint uint_0)
		{
			int int_ = 0;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT timestamp, state, offer_id, item_type, sprite_id, total_price, furni_id FROM catalog_marketplace_offers WHERE user_id = '" + uint_0 + "'");
				string text = @class.ReadDataRow("SELECT SUM(asking_price) FROM catalog_marketplace_offers WHERE state = '2' AND user_id = '" + uint_0 + "'")[0].ToString();
				if (text.Length > 0)
				{
					int_ = int.Parse(text);
				}
                ServerMessage Message = new ServerMessage(Outgoing.Offerstats); // Update
				Message.AppendInt32(int_);
				if (dataTable != null)
				{
					Message.AppendInt32(dataTable.Rows.Count);
					IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							int num = (int)Math.Floor(((double)dataRow["timestamp"] + 172800.0 - Essential.GetUnixTimestamp()) / 60.0);
							int num2 = int.Parse(dataRow["state"].ToString());
							if (num <= 0 && num2 != 2)
							{
								num2 = 3;
								num = 0;
							}
							Message.AppendUInt((uint)dataRow["offer_id"]);
							Message.AppendInt32(num2);
							Message.AppendInt32(int.Parse(dataRow["item_type"].ToString()));
                            if (int.Parse(dataRow["item_type"].ToString()) == 2)
                            {
                                Message.AppendInt32((int)dataRow["sprite_id"]);
                                Message.AppendStringWithBreak((@class.ReadInt32("SELECT COUNT(extra_data) FROM items_extra_data WHERE item_id = '" + dataRow["furni_id"] + "'") > 0 ? @class.ReadString("SELECT extra_data FROM items_extra_data WHERE item_id = '" + dataRow["furni_id"] + "'") : ""));
                            }
                            else
                            {
                                Message.AppendInt32((int)dataRow["sprite_id"]);
                                Message.AppendInt32(0);
                                Message.AppendStringWithBreak("");
                              
                            }
                            Message.AppendInt32((int)dataRow["total_price"]);
                            Message.AppendInt32(num);
                            Message.AppendInt32((int)dataRow["sprite_id"]);

							
						}
						goto IL_1DE;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);
				IL_1DE:
				return Message;
			}
		}
	}
}
