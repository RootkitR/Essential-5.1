using System;
using System.Collections.Generic;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.SoundMachine;
namespace Essential.Catalogs
{
	internal sealed class CatalogItem
	{
		public uint uint_0;
		public List<uint> list_0;
        public string Name;
        public int CreditsCost;
        public int PixelsCost;
		public int int_2; //SnowCost;
		public int int_3;
		internal int int_4;
		internal uint uint_1;
		internal int int_5;
		internal uint uint_2;
        internal int song_id;
        public string BadgeID;
        public int LimitedSold;
        public int LimitedCount;
        internal bool IsLimited;
        internal bool HaveOffer;
        internal int PosterId;
        public string Extradata;
		public bool IsPackage
		{
			get
			{
				return this.list_0.Count > 1;
			}
		}
        public CatalogItem(uint uint_3, string string_1, string string_2, int int_6, int int_7, int int_8, int int_9, int int_10, int int_11, uint uint_4, int song_id, string BadgeID, int LimitedSold, int LimitedCount, bool HaveOffer, int PosterId, string extradata)
		{
			this.uint_0 = uint_3;
            this.Name = string_1;
			this.list_0 = new List<uint>();
			this.int_4 = int_10;
			string[] array = string_2.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string s = array[i];
				this.list_0.Add(uint.Parse(s));
			}
            this.CreditsCost = int_6;
            this.PixelsCost = int_7;
			this.int_2 = int_8;
			this.int_3 = int_9;
			this.int_5 = int_11;
			this.uint_2 = uint_4;
			this.uint_1 = 7u;
            this.song_id = song_id;
            this.BadgeID = BadgeID;
            this.LimitedSold = LimitedSold;
            this.LimitedCount = LimitedCount;
            this.IsLimited = (this.LimitedCount > 0);
            this.HaveOffer = HaveOffer;
            this.PosterId = PosterId;
            this.Extradata = extradata;
		}
		public Item GetBaseItem()
		{
			if (this.IsPackage)
			{
				return null;
			}
			else
			{
				return Essential.GetGame().GetItemManager().GetItemById(this.list_0[0]);
			}
		}
		public void Serialize(ServerMessage Message5_0)
		{
			if (this.IsPackage)
			{
				throw new NotImplementedException("Multipile item ids set for catalog item #" + this.uint_0 + ", but this is usupported at this point");
			}
			Message5_0.AppendUInt(this.uint_0);
            if (this.song_id > 0)
			{
                Message5_0.AppendStringWithBreak(SongManager.GetSong(this.song_id).Name);
			}
			else
			{
                Message5_0.AppendStringWithBreak(this.Name);
			}
            Message5_0.AppendInt32(this.CreditsCost);
			Message5_0.AppendInt32(this.PixelsCost);
			Message5_0.AppendInt32(this.int_2);
            Message5_0.AppendBoolean((this.IsLimited ? false : this.GetBaseItem().AllowGift));
            if (this.BadgeID == "")
            {
                Message5_0.AppendInt32(1);
            }
            else
            {
                Message5_0.AppendInt32(2);
                Message5_0.AppendString("b");
                Message5_0.AppendString(this.BadgeID);
            }
            Message5_0.AppendStringWithBreak(this.GetBaseItem().Type.ToString());
			Message5_0.AppendInt32(this.GetBaseItem().Sprite);
			string text = "";
            text = Extradata.Split('|')[0];
            if (this.Name.Contains("wallpaper_single") || this.Name.Contains("floor_single") || this.Name.Contains("landscape_single"))
			{
                string[] array = this.Name.Split(new char[]
				{
					'_'
				});
				text = array[2];
			}
			else
			{
				if (this.song_id > 0)
				{
                    text = this.song_id.ToString();
				}
				else
				{
					if (this.PosterId > 0)
					{
                        text = this.PosterId.ToString();
					}
				}
			}
			Message5_0.AppendStringWithBreak(text);
			Message5_0.AppendInt32(this.int_3);
            if (!this.IsLimited)
                Message5_0.AppendInt32(0);
			//Message5_0.AppendInt32(this.int_5);
            Message5_0.AppendBoolean(this.IsLimited);
            if (this.IsLimited)
            {
                Message5_0.AppendInt32(this.LimitedCount);
                Message5_0.AppendInt32(this.LimitedCount - this.LimitedSold);
                Message5_0.AppendInt32(0);
            }
            Message5_0.AppendBoolean(this.IsLimited ? false : this.HaveOffer);
		}
	}
}
