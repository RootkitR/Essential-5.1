using System;
using System.Collections;
using System.Collections.Generic;
using Essential.Catalogs;
using Essential.Messages;
namespace Essential.HabboHotel.Catalogs
{
	internal sealed class CatalogPage
	{
        public int Id;
        public int ParentId;
		public string Caption;
        public bool Visible;
        public bool Enabled;
        public uint MinRank;
        public bool ClubOnly;
        public int IconColor;
        public int IconImage;
		public string Layout;
        public string LayoutHeadline;
        public string LayoutTeaser;
        public string LayoutSpecial;
		public string string_5;
		public string string_6;
		public string string_7;
		public string string_8;
        public string TextDetails;
        public string TextTeaser;
        public string LinkName;
        public List<CatalogItem> Items;
        private ServerMessage mMessage;
        public int PageId
		{
			get
			{
                return this.Id;
			}
		}
        internal ServerMessage GetMessage
		{
			get
			{
				return this.mMessage;
			}
		}
        public CatalogPage(int Id, string LinkName, int ParentId, string Caption, bool Visible, bool Enabled, uint MinRank, bool ClubOnly, int IconColor, int IconImage, string Layout, string string_13, string string_14, string string_15, string string_16, string string_17, string string_18, string string_19, string TextDetails, string TextTeaser, ref Hashtable CataItems)
		{
            this.Items = new List<CatalogItem>();
            this.Id = Id;
            this.LinkName = LinkName;
            this.ParentId = ParentId;
            this.Caption = Caption;
            this.Visible = Visible;
            this.Enabled = Enabled;
            this.MinRank = MinRank;
            this.ClubOnly = ClubOnly;
            this.IconColor = IconColor;
            this.IconImage = IconImage;
            this.Layout = Layout;
            this.LayoutHeadline = string_13;
            this.LayoutTeaser = string_14;
            this.LayoutSpecial = string_15;
			this.string_5 = string_16;
			this.string_6 = string_17;
			this.string_7 = string_18;
			this.string_8 = string_19;
            this.TextDetails = TextDetails;
            this.TextTeaser = TextTeaser;
            foreach (CatalogItem Item in CataItems.Values)
			{
                if (Item.int_4 == Id)
				{
                    this.Items.Add(Item);
				}
			}
		}
        internal void InitMsg()
		{
            this.mMessage = Essential.GetGame().GetCatalog().SerializePage(this);
		}
        public CatalogItem GetItem(uint pId)
		{
			using (TimedLock.Lock(this.Items))
			{
                foreach (CatalogItem current in this.Items)
				{
                    if (current.uint_0 == pId)
					{
						return current;
					}
				}
			}
			return null;
		}
        public void Serialize(int Rank, ServerMessage Message)
		{
            Message.AppendBoolean(this.Visible);
            Message.AppendInt32(this.IconColor);
            Message.AppendInt32(this.IconImage);
            Message.AppendInt32(this.Id);
            Message.AppendString(this.LinkName);
            Message.AppendString(this.Caption);
            Message.AppendInt32(Essential.GetGame().GetCatalog().GetTreeSize(Rank, this.Id));
		}
        public List<CatalogItem> GetItems()
        {
            return Items;
        }
	}
}
