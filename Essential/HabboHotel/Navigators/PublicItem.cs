using Essential.HabboHotel.Rooms;
using Essential.Messages;
using System;
namespace Essential.HabboHotel.Navigators
{
    internal class PublicItem
    {
        private readonly int BannerId;
        internal string Caption;
        internal int CategoryId;
        internal string Description;
        internal string Image;
        internal PublicImageType ImageType;
        internal PublicItemType itemType;
        internal int ParentId;
        internal bool Recommended;
        internal uint RoomId;
        internal string TagsToSearch = "";
        internal int Type;

        internal PublicItem(int mId, int mType, string mCaption, string mDescription, string mImage, PublicImageType mImageType, uint mRoomId, int mCategoryId, int mParentId, bool mRecommand, int mTypeOfData, string mTags)
        {
            this.BannerId = mId;
            this.Type = mType;
            this.Caption = mCaption;
            this.Description = mDescription;
            this.Image = mImage;
            this.ImageType = mImageType;
            this.RoomId = mRoomId;
            this.ParentId = mParentId;
            this.CategoryId = mCategoryId;
            this.Recommended = mRecommand;
            if (mTypeOfData == 1)
            {
                this.itemType = PublicItemType.TAG;
            }
            else if (mTypeOfData == 2)
            {
                this.itemType = PublicItemType.FLAT;
            }
            else if (mTypeOfData == 3)
            {
                this.itemType = PublicItemType.PUBLIC_FLAT;
            }
            else if (mTypeOfData == 4)
            {
                this.itemType = PublicItemType.CATEGORY;
            }
            else
            {
                this.itemType = PublicItemType.NONE;
            }
        }

        internal void Serialize(ServerMessage Message)
        {
            try
            {
                Message.AppendInt32(this.Id);
                Message.AppendString((this.itemType != PublicItemType.PUBLIC_FLAT) ? this.Caption : string.Empty);
                Message.AppendString(this.Description);
                Message.AppendInt32(this.Type);
                Message.AppendString((this.itemType == PublicItemType.PUBLIC_FLAT) ? this.Caption : string.Empty);
                Message.AppendString(this.Image);
                Message.AppendInt32((this.ParentId > 0) ? this.ParentId : 0);
                Message.AppendInt32((this.RoomInfo != null) ? this.RoomInfo.UsersNow : 0);
                Message.AppendInt32((this.itemType == PublicItemType.NONE) ? 0 : ((this.itemType == PublicItemType.TAG) ? 1 : ((this.itemType == PublicItemType.FLAT) ? 2 : ((this.itemType == PublicItemType.PUBLIC_FLAT) ? 2 : ((this.itemType == PublicItemType.CATEGORY) ? 4 : 0)))));
                if (this.itemType == PublicItemType.TAG)
                {
                    Message.AppendString(this.TagsToSearch);
                }
                else if (this.itemType == PublicItemType.CATEGORY)
                {
                    Message.AppendBoolean(false);
                }
                else if (this.itemType == PublicItemType.FLAT)
                {
                    this.RoomInfo.Serialize(Message, false, false);
                }
                else if (this.itemType == PublicItemType.PUBLIC_FLAT)
                {
                    this.RoomInfo.Serialize(Message, false, false);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception on publicitems composing: " + exception.ToString());
            }
        }

        internal int Id
        {
            get
            {
                return this.BannerId;
            }
        }

        internal HabboHotel.Rooms.RoomData RoomData
        {
            get
            {
                if (this.RoomId == 0)
                {
                    return new HabboHotel.Rooms.RoomData();
                }
                if (Essential.GetGame() == null)
                {
                    throw new NullReferenceException();
                }
                if (Essential.GetGame().GetRoomManager() == null)
                {
                    throw new NullReferenceException();
                }
                if (Essential.GetGame().GetRoomManager().method_12(this.RoomId) == null)
                {
                    throw new NullReferenceException();
                }
                return Essential.GetGame().GetRoomManager().method_12(this.RoomId);
            }
        }

        internal HabboHotel.Rooms.RoomData RoomInfo
        {
            get
            {
                try
                {
                    if (this.RoomId > 0)
                    {
                        return Essential.GetGame().GetRoomManager().method_12(this.RoomId);
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}

