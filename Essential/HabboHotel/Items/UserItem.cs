using System;
using Essential.Core;
using Essential.Messages;
namespace Essential.HabboHotel.Items
{
	internal sealed class UserItem
	{
		internal uint uint_0;
		internal uint uint_1;
		internal string string_0;
        internal int LtdId;
        internal int LtdCnt;
		private Item Item;
        internal string GuildData;
		internal UserItem(uint Id, uint BaseItem, string ExtraData, int LtdId, int LtdCnt, string GuildData)
		{
			this.uint_0 = Id;
			this.uint_1 = BaseItem;
			this.string_0 = ExtraData;
			this.Item = this.GetBaseItem();
            this.LtdId = LtdId;
            this.LtdCnt = LtdCnt;
            this.GuildData = GuildData;
		}
		internal void Serialize(ServerMessage Message, bool bool_0)
		{
			if (this.Item == null)
			{
                throw new Exception ("Unknown base: " + this.uint_1);
			}
			Message.AppendUInt(this.uint_0);
			Message.AppendStringWithBreak(this.Item.Type.ToString().ToUpper());
			Message.AppendUInt(this.uint_0);
			Message.AppendInt32(this.Item.Sprite);
            if (this.Item.Name.Contains("a2 "))
                Message.AppendInt32(3);
            else if (this.Item.Name.Contains("wallpaper"))
                Message.AppendInt32(2);
            else if (this.Item.Name.Contains("landscape"))
                Message.AppendInt32(4);
            else if (this.Item.Name == "poster")
                Message.AppendInt32(6);
            else if (this.GetBaseItem().Name == "song_disk")
                Message.AppendInt32(8);
            else if (this.GetBaseItem().InteractionType == "gld_furni")
                Message.AppendInt32(17);
            else
                Message.AppendInt32(1);
			
            if (this.LtdId > 0)
            {
                Message.AppendString("");
                Message.AppendBoolean(true);
                Message.AppendBoolean(false);
            }
            else if (this.GetBaseItem().InteractionType == "gld_furni")
            {
                try
                {
                    GroupsManager Guild = Groups.GetGroupById(int.Parse(this.GuildData));
                    if (Guild == null)
                    {
                        Message.AppendInt32(2);
                        Message.AppendInt32(5);
                        Message.AppendString("0");
                        Message.AppendString("1");
                        Message.AppendString("");
                        Message.AppendString("ffffff");
                        Message.AppendString("ffffff");
                    }
                    else
                    {
                        Message.AppendInt32(2);
                        Message.AppendInt32(5);
                        Message.AppendString("0");
                        Message.AppendString(Guild.Id.ToString());
                        Message.AppendString(Guild.Badge);
                        Message.AppendString(Guild.ColorOne);
                        Message.AppendString(Guild.ColorTwo);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Message.AppendInt32(0);
            }
			if (this.GetBaseItem().Name == "song_disk")
			{
				Message.AppendStringWithBreak("");
			}
			else if (this.GetBaseItem().Name.StartsWith("poster_"))
			{
					Message.AppendStringWithBreak(this.GetBaseItem().Name.Split(new char[]
					{
						'_'
					})[1]);
		    }
		    else if(this.GetBaseItem().InteractionType != "gld_furni")
			{
				Message.AppendStringWithBreak(this.string_0);   
            }
            if (this.LtdId > 0)
            {
                Message.AppendInt32(this.LtdId);
                Message.AppendInt32(this.LtdCnt);
            }
			Message.AppendBoolean(this.Item.AllowRecycle);
			Message.AppendBoolean(this.Item.AllowTrade);
			Message.AppendBoolean(this.LtdId > 0 ? false : this.Item.AllowInventoryStack);
			Message.AppendBoolean(false);
            Message.AppendInt32(-1);
            Message.AppendBoolean(true);
            Message.AppendInt32(-1);
			if (this.Item.Type.ToString().ToUpper() == "S")
			{
				Message.AppendStringWithBreak("");
				Message.AppendInt32(-1);
			}
		}
		internal Item GetBaseItem()
		{
			return Essential.GetGame().GetItemManager().GetItemById(this.uint_1);
		}
	}
}
