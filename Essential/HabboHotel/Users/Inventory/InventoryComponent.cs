using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Essential.Core;
using Essential.HabboHotel.Users.UserDataManagement;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pets;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.SoundMachine;
using Essential.Source.HabboHotel.SoundMachine;
using System.Linq;

namespace Essential.HabboHotel.Users.Inventory
{
	internal sealed class InventoryComponent
	{
        private Hashtable Discs;

		public List<UserItem> Items;

		private Hashtable Pets;
        private Hashtable Bots;

		private Hashtable hashtable_1;

		public List<uint> list_1;

		private GameClient Session;

		public uint UserId;
        public UserItem GetLastItem()
        {
            return Items.OrderByDescending(ui => ui.uint_0).First();
        }
		public int ItemCount
		{
			get
			{
				return this.Items.Count;
			}
		}

		public int Int32_1
		{
			get
			{
				return this.Pets.Count;
			}
		}

		public InventoryComponent(uint userId, GameClient client, UserDataFactory userdata)
		{
			this.Session = client;
			this.UserId = userId;
       
			this.Items = new List<UserItem>();

			this.Pets = new Hashtable();
            this.Bots = new Hashtable();
			this.hashtable_1 = new Hashtable();

            this.Discs = new Hashtable();

			this.list_1 = new List<uint>();
			
			foreach (DataRow row in userdata.GetItems().Rows)
			{
                string str;

                uint id = Convert.ToUInt32(row["Id"]);
                uint baseItem = Convert.ToUInt32(row["base_item"]);
                int ltdi = Convert.ToInt32(row["ltd_id"]);
                int ltdc = Convert.ToInt32(row["ltd_cnt"]);
                string glddata = (string)row["guild_data"];
                if (!DBNull.Value.Equals(row["extra_data"]))
                    str = (string)row["extra_data"];
                else
                    str = string.Empty;

                UserItem item = new UserItem(id, baseItem, str, ltdi, ltdc, glddata);

                Items.Add(item);

               

                if (item.GetBaseItem().InteractionType == "musicdisc")
                    this.Discs.Add(item.uint_0, item);
			}

			foreach (DataRow row in userdata.GetPets().Rows)
			{
				Pet pet = Essential.GetGame().GetCatalog().GetPet(row);
				this.Pets.Add(pet.PetId, pet);
			}

           foreach (DataRow row in userdata.GetBots().Rows)
            {
                UserBot bot = Essential.GetGame().GetCatalog().RetrBot(row);
                this.Bots.Add(bot.BotId, bot);
            }
            
		}

		public void ClearInventory()
		{
			using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
				dbClient.ExecuteQuery("DELETE FROM items WHERE room_id = 0 AND user_id = '" + this.UserId + "';");
			}

            this.Discs.Clear();
			this.hashtable_1.Clear();
			this.list_1.Clear();
			this.Items.Clear();

            ServerMessage Message5_ = new ServerMessage(Outgoing.UpdateInventary); // Updated
			this.GetClient().SendMessage(Message5_);
		}

		public void ConvertCoinsToCredits()
		{
			int num = 0;

			List<UserItem> list = new List<UserItem>();
			foreach (UserItem item in Items)
			{
				if (item != null && (item.GetBaseItem().Name.StartsWith("CF_") || item.GetBaseItem().Name.StartsWith("CFC_")))
				{
					string[] array = item.GetBaseItem().Name.Split(new char[]
					{
						'_'
					});

                    int num2 = 0;

                    if (array[1] == "diamond")
                    {
                        num2 = int.Parse(array[2]);
                    }
                    else
                    {
                        num2 = int.Parse(array[1]);
                    }
					

					if (!this.list_1.Contains(item.uint_0))
					{
						if (num2 > 0)
						{
							num += num2;
						}
						list.Add(item);
					}
				}
               
			}

			foreach (UserItem current in list)
			{
				this.method_12(current.uint_0, 0u, false);
			}

			Session.GetHabbo().GiveCredits(num, "Redeem all Credit Furnis");
			Session.GetHabbo().UpdateCredits(true);

			Session.SendNotification("All exchanges redeemed to " + num + " credits!");
		}

		public void RemovePetsFromInventory()
		{
			using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
				dbClient.ExecuteQuery("DELETE FROM user_pets WHERE user_id = " + this.UserId + " AND room_id = 0;");
			}

			foreach (Pet pet in Pets.Values)
			{
                ServerMessage Message = new ServerMessage(Outgoing.PetRemovedFromInventory);
				Message.AppendUInt(pet.PetId);
				this.GetClient().SendMessage(Message);
			}

			this.Pets.Clear();
		}
        public void ClearBots()
        {
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("DELETE FROM user_bots WHERE owner_id = " + this.UserId + " AND room_id = 0;");
            }
            this.Bots.Clear();
            this.GetClient().SendMessage(this.ComposeBotInventoryListMessage());
        }
		public void method_3(bool bool_0)
		{
			if (bool_0)
				this.ReloadItems();

			this.GetClient().SendMessage(this.ComposePetInventoryListMessage());
		}
        public UserBot GetBotById(uint botId)
        {
            return Bots[botId] as UserBot;
        }
		public Pet GetPetById(uint petId)
		{
			return Pets[petId] as Pet;
		}
        public void RemoveBotById(uint botId)
        {
            this.Bots.Remove(botId);
            this.GetClient().SendMessage(ComposeBotInventoryListMessage());
        }
		public void RemovePetById(uint petId)
		{
            /*ServerMessage Message = new ServerMessage(Outgoing.PetRemovedFromInventory); // Updated
			Message.AppendUInt(petId);
			this.GetClient().SendMessage(Message);*/
			this.Pets.Remove(petId);
            this.GetClient().SendMessage(this.ComposePetInventoryListMessage());
		}
        public void AddBot(UserBot bot)
        {
            try
            {
                if (bot != null)
                {
                    bot.PlacedInRoom = false;

                    if (!Bots.ContainsKey(bot.BotId))
                        Bots.Add(bot.BotId, bot);
 
                    this.GetClient().SendMessage(ComposeBotInventoryListMessage());
                }
            }
            catch { }
        }
        public void AddBot(uint BotId)
        {
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                DataRow drow = dbClient.ReadDataRow("SELECT * FROM user_bots WHERE Id=" + BotId);
                this.Bots.Add(BotId,Essential.GetGame().GetCatalog().RetrBot(drow));
                this.GetClient().SendMessage(ComposeBotInventoryListMessage());
            }

        }
		public void AddPet(Pet pet)
		{
            try
            {
                if (pet != null)
                {
                    pet.PlacedInRoom = false;

                    if (!Pets.ContainsKey(pet.PetId))
                        Pets.Add(pet.PetId, pet);

                    this.GetClient().SendMessage(ComposePetInventoryListMessage());
                }
            }
            catch { }
		}

		public void ReloadItems()
		{
			using (TimedLock.Lock(this.Items))
			{
				Items.Clear();

				hashtable_1.Clear();
				list_1.Clear();

				DataTable dataTable;

				using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
				{
                    dataTable = dbClient.ReadDataTable("SELECT items.Id,items.base_item,items.ltd_id,items.ltd_cnt,items.guild_data,items_extra_data.extra_data FROM items LEFT JOIN items_extra_data ON items_extra_data.item_id = items.Id WHERE room_id = 0 AND user_id = " + this.UserId);
				}

				if (dataTable != null)
				{
					foreach (DataRow row in dataTable.Rows)
					{
                        string extraData = (row["extra_data"] == DBNull.Value) ? string.Empty : (string)row["extra_data"];

                        if (extraData != null && !DBNull.Value.Equals(extraData) && !string.IsNullOrEmpty(extraData))
                            Items.Add(new UserItem((uint)row["Id"], (uint)row["base_item"], extraData, (int)row["ltd_id"], (int)row["ltd_cnt"], (string)row["guild_data"]));
                        else
                            Items.Add(new UserItem((uint)row["Id"], (uint)row["base_item"], "", (int)row["ltd_id"], (int)row["ltd_cnt"], (string)row["guild_data"]));
					}
				}

				using (TimedLock.Lock(Pets))
				{
                    Pets.Clear();

					using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
					{
						dbClient.AddParamWithValue("userid", this.UserId);
						dataTable = dbClient.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE user_id = @userid AND room_id <= 0");
					}

					if (dataTable != null)
					{
						foreach (DataRow row in dataTable.Rows)
						{
							Pet pet = Essential.GetGame().GetCatalog().GetPet(row);

							this.Pets.Add(pet.PetId, pet);
						}
					}
				}
			}
		}

		public void method_9(bool bool_0)
		{
			if (bool_0)
			{
				this.ReloadItems();
				this.SavePets();
			}

            if (this.GetClient() != null)
                this.GetClient().SendMessage(new ServerMessage(Outgoing.UpdateInventary));
		}

		public UserItem GetItemById(uint itemId)
		{
			List<UserItem>.Enumerator enumerator = this.Items.GetEnumerator();

			while (enumerator.MoveNext())
			{
				UserItem current = enumerator.Current;

				if (current.uint_0 == itemId)
                    return current;
			}

            return null;
		}

		public void method_11(uint uint_1, uint uint_2, string string_0, bool bool_0, int ltd_id, int ltd_count, string GuildData)
		{
            UserItem item = new UserItem(uint_1, uint_2, string_0, ltd_id, ltd_count, GuildData);
			this.Items.Add(item);
			if (this.list_1.Contains(uint_1))
			{
				this.list_1.Remove(uint_1);
			}
			if (!this.hashtable_1.ContainsKey(uint_1))
			{
				if (bool_0)
				{
					using (DatabaseClient @class = Essential.GetDatabase().GetClient())
					{
						@class.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items (Id,user_id,base_item,wall_pos, ltd_id, ltd_cnt, guild_data) VALUES ('",
							uint_1,
							"','",
							this.UserId,
							"','",
							uint_2,
							"', '', '",
							ltd_id,
							"','",
							ltd_count,
							"','",
							GuildData,
							"')"
						}));

                        if (!string.IsNullOrEmpty(string_0))
                        {
                            @class.AddParamWithValue("extra_data", string_0);
                            @class.ExecuteQuery(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + uint_1 + "'; ",
                                                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + uint_1 + "' , @extra_data); "
                                            }));
                        }
                        else
                        {
                            @class.ExecuteQuery(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + uint_1 + "'; "
                                            }));
                        }
						return;
					}
				}

                if (item.GetBaseItem().InteractionType == "musicdisc")
                {
                    if (this.Discs.ContainsKey(item.uint_0))
                    {
                        this.Discs.Add(item.uint_0, item);
                    }
                }

				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE items SET room_id = 0, user_id = '",
						this.UserId,
						"' WHERE Id = '",
						uint_1,
						"'"
					}));
				}
			}
		}
        public void method_12(uint uint_1, uint uint_2, bool bool_0)
        {
            if (this != null && this.GetClient() != null)
            {
                ServerMessage Message = new ServerMessage(Outgoing.RemoveObjectFromInventory); // Update
                Message.AppendUInt(uint_1);
                this.GetClient().SendMessage(Message);
                if (this.hashtable_1.ContainsKey(uint_1))
                {
                    this.hashtable_1.Remove(uint_1);
                }
                if (!this.list_1.Contains(uint_1))
                {
                    this.Items.Remove(this.GetItemById(uint_1));
                    this.list_1.Add(uint_1);
                    this.Discs.Remove(uint_1);
                    if (bool_0)
                    {
                        using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                        {
                            @class.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE items SET user_id = '",
							uint_2,
							"' WHERE Id = '",
							uint_1,
							"' LIMIT 1"
						}));
                            return;
                        }
                    }
                    if (uint_2 == 0u && !bool_0)
                    {
                        using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                        {
                            @class.ExecuteQuery("DELETE FROM items WHERE Id = '" + uint_1 + "' LIMIT 1");
                        }
                    }
                }
            }
        }
     
		public void method_13(int Page = 1)
		{
            if(this.Items.Count >= 2300)
            {
                Session.SendNotification("HINWEIS: Da sich in deinem Inventar mehr als 2300 Möbel befinden, werden nur 2300 Möbel angezeigt.");
            }
            ServerMessage Message = new ServerMessage(Outgoing.Inventory);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
            Message.AppendInt32(this.Items.Count >= 2300 ? 2300: this.Items.Count);
            List<UserItem>.Enumerator enumerator = this.Items.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext())
            {
                if(count <= 2300)
                    enumerator.Current.Serialize(Message, true);
                count++;
            }
            Session.SendMessage(Message);
		}

		
        public ServerMessage ComposeBotInventoryListMessage()
        {
            ServerMessage Message = new ServerMessage(Outgoing.BotInventory); // Updated
            Message.AppendInt32(Bots.Count);
            foreach(UserBot userbot in Bots.Values)
            {
                Message.AppendUInt(userbot.BotId);
                Message.AppendString(userbot.Name);
                Message.AppendString("");
                Message.AppendString("M");
                Message.AppendString(userbot.Look);
            }
            return Message;
        }

		public ServerMessage ComposePetInventoryListMessage()
		{
            ServerMessage Message = new ServerMessage(Outgoing.PetInventory); // Updated
            Message.AppendInt32(1);
            Message.AppendInt32(1);
			Message.AppendInt32(Pets.Count);

			foreach (Pet pet in Pets.Values)
			{
				pet.SerializeInventory(Message);
			}

			return Message;
		}

		private GameClient GetClient()
		{
			return Essential.GetGame().GetClientManager().GetClient(this.UserId);
		}

		public void method_17(List<RoomItem> list_2)
		{
			foreach (RoomItem current in list_2)
			{
				this.method_11(current.uint_0, current.uint_2, current.ExtraData, false, current.LimitedId, current.LimitetCnt, current.GuildData);
			}
		}

		internal void SavePets()
		{
			using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
				this.SavePets(dbClient, false);
			}
		}

		internal void SavePets(DatabaseClient dbClient, bool consoleOutput)
		{
			try
			{
				if (this.list_1.Count > 0 || this.hashtable_1.Count > 0 || this.Pets.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();

					foreach (Pet pet in Pets.Values)
					{
						if (pet.DBState == DatabaseUpdateState.NeedsInsert)
						{
							dbClient.AddParamWithValue("petname" + pet.PetId, pet.Name);
							dbClient.AddParamWithValue("petcolor" + pet.PetId, pet.Color);
							dbClient.AddParamWithValue("petrace" + pet.PetId, pet.Race);

							stringBuilder.Append(string.Concat(new object[]
							{
								"INSERT INTO `user_pets` VALUES ('",
								pet.PetId,
								"', '",
								pet.OwnerId,
								"', '",
								pet.RoomId,
								"', @petname",
								pet.PetId,
								", @petrace",
								pet.PetId,
								", @petcolor",
								pet.PetId,
								", '",
								pet.Type,
								"', '",
								pet.Expirience,
								"', '",
								pet.Energy,
								"', '",
								pet.Nutrition,
								"', '",
								pet.Respect,
								"', '",
								pet.CreationStamp,
								"', '",
								pet.X,
								"', '",
								pet.Y,
								"', '",
								pet.Z,
								"');"
							}));
						}
						else
						{
							if (pet.DBState == DatabaseUpdateState.NeedsUpdate)
							{
								stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE user_pets SET room_id = '",
									pet.RoomId,
									"', expirience = '",
									pet.Expirience,
									"', energy = '",
									pet.Energy,
									"', nutrition = '",
									pet.Nutrition,
									"', respect = '",
									pet.Respect,
									"', x = '",
									pet.X,
									"', y = '",
									pet.Y,
									"', z = '",
									pet.Z,
									"' WHERE Id = '",
									pet.PetId,
									"' LIMIT 1; "
								}));
							}
						}

						pet.DBState = DatabaseUpdateState.Updated;
					}

					if (stringBuilder.Length > 0)
					{
						dbClient.ExecuteQuery(stringBuilder.ToString());
					}
				}

				if (consoleOutput)
				{
					Console.WriteLine("Inventory for user: " + this.GetClient().GetHabbo().Username + " saved.");
				}
			}
			catch (Exception ex)
			{
                Logging.LogCacheError("FATAL ERROR DURING DB UPDATE: " + ex.ToString());
			}
		}

        internal Hashtable songDisks
        {
            get
            {
                return this.Discs;
            }
        }

        public void RedeemPixel(GameClient client)
        {
            int num = 0;

            List<UserItem> list = new List<UserItem>();

            foreach (UserItem current in this.Items)
            {
                if (current != null && (current.GetBaseItem().Name.StartsWith("PixEx_")))
                {
                    string[] array = current.GetBaseItem().Name.Split(new char[]
					{
						'_'
					});

                    int num2 = int.Parse(array[1]);

                    if (!this.list_1.Contains(current.uint_0))
                    {
                        if (num2 > 0)
                            num += num2;

                        list.Add(current);
                    }
                }
            }

            foreach (UserItem current in list)
            {
                this.method_12(current.uint_0, 0u, false);
            }

            client.GetHabbo().ActivityPoints += num;
            client.GetHabbo().UpdateActivityPoints(true);

            client.SendNotification("All deine Pixel im Inventar wurden zu " + num + " Pixel umgetauscht!");
        }

        public void RedeemShell(GameClient client)
        {
            int num = 0;

            List<UserItem> list = new List<UserItem>();

            foreach (UserItem current in this.Items)
            {
                if (current != null && (current.GetBaseItem().Name.StartsWith("PntEx_")))
                {
                    string[] array = current.GetBaseItem().Name.Split(new char[]
					{
						'_'
					});

                    int num2 = int.Parse(array[1]);

                    if (!this.list_1.Contains(current.uint_0))
                    {
                        if (num2 > 0)
                            num += num2;

                        list.Add(current);
                    }
                }
            }

            foreach (UserItem current in list)
            {
                this.method_12(current.uint_0, 0u, false);
            }

            client.GetHabbo().VipPoints += num;
            client.GetHabbo().UpdateVipPoints(false, true);

            client.SendNotification("All deine VIP Punkte im Inventar wurden zu " + num + " VIP Punkte umgetauscht!");
        }
	}
}
