using System;
using System.Collections.Generic;
using System.Data;
using Essential.Core;
using Essential.Storage;
using Essential.HabboHotel.SoundMachine;
namespace Essential.HabboHotel.Items
{
	internal sealed class ItemManager
	{
		private Dictionary<uint, Item> dictionary_0;
        private bool isLoading = false;
		public ItemManager()
		{
			this.dictionary_0 = new Dictionary<uint, Item>();
		}
		public void Initialize(DatabaseClient class6_0)
		{
            if (!isLoading)
            {
                isLoading = true;
                Logging.Write("Loading Items..");
                this.dictionary_0 = new Dictionary<uint, Item>();
                // this.FurnitureAliases = new List<string>();

                DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM furniture;");
                //  DataTable dataTable2 = class6_0.ReadDataTable("SELECT * FROM furniture_aliases WHERE branding_enabled = '1';");
                if (dataTable != null)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        try
                        {
                            this.dictionary_0.Add((uint)dataRow["Id"], new Item((uint)dataRow["Id"], (int)dataRow["sprite_id"], (string)dataRow["public_name"], (string)dataRow["item_name"], (string)dataRow["type"], (int)dataRow["width"], (int)dataRow["length"], (double)dataRow["stack_height"], Essential.StringToBoolean(dataRow["can_stack"].ToString()), Essential.StringToBoolean(dataRow["is_walkable"].ToString()), Essential.StringToBoolean(dataRow["can_sit"].ToString()), Essential.StringToBoolean(dataRow["allow_recycle"].ToString()), Essential.StringToBoolean(dataRow["allow_trade"].ToString()), Essential.StringToBoolean(dataRow["allow_marketplace_sell"].ToString()), Essential.StringToBoolean(dataRow["allow_gift"].ToString()), Essential.StringToBoolean(dataRow["allow_inventory_stack"].ToString()), (string)dataRow["interaction_type"], (int)dataRow["interaction_modes_count"], (string)dataRow["vending_ids"], dataRow["height_adjustable"].ToString(), Convert.ToByte((int)dataRow["EffectF"]), Convert.ToByte((int)dataRow["EffectM"]), Essential.StringToBoolean(dataRow["HeightOverride"].ToString())));
                        }
                        catch (Exception e)
                        {
                            Logging.WriteLine("Could not load item #" + (uint)dataRow["Id"] + ", please verify the data is okay.");
                            Logging.LogItemError(e.Message);
                        }
                    }
                }
                /* if (dataTable2 != null)
                 {
                     foreach (DataRow dataRow in dataTable2.Rows)
                     {
                         try
                         {
                             this.FurnitureAliases.Add((string)dataRow["item_name"]);
                         }
                         catch (Exception e)
                         {
                             Logging.WriteLine("Could not load furniture alias " + (string)dataRow["item_name"] + ".");
                             Logging.LogItemError(e.Message);
                         }
                     }
                 }
                 * */
                Logging.WriteLine("completed!", ConsoleColor.Green);
                /*Logging.smethod_0("Loading Soundtracks.."); //OMA LUOTU :3
                this.dictionary_1 = new Dictionary<int, Soundtrack>();
                DataTable dataTable2 = class6_0.ReadDataTable("SELECT * FROM soundtracks;");
                if (dataTable2 != null)
                {
                    foreach (DataRow dataRow in dataTable2.Rows)
                    {
                        try
                        {
                            this.dictionary_1.Add((int)dataRow["Id"], new Soundtrack((int)dataRow["Id"], (string)dataRow["name"], (string)dataRow["author"], (string)dataRow["track"], (int)dataRow["length"]));
                        }
                        catch (Exception)
                        {
                            Logging.WriteLine("Could not load item #" + (uint)dataRow["Id"] + ", please verify the data is okay.");
                        }
                    }
                }
                Logging.WriteLine("completed!", ConsoleColor.Green);*/
                Logging.Write("Loading Soundtracks..");
                SongManager.Initialize();
                Logging.WriteLine("completed!", ConsoleColor.Green);
                isLoading = false;
            }
		}
		public bool ItemExists(uint uint_0)
		{
			return this.dictionary_0.ContainsKey(uint_0);
		}
		public Item GetItemById(uint uint_0)
		{
			Item result;
			if (this.ItemExists(uint_0))
			{
				result = this.dictionary_0[uint_0];
			}
			else
			{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Couldn't find Item #" + uint_0);
                Console.ForegroundColor = ConsoleColor.Gray;
				result = null;
			}
			return result;
		}
	}
}
