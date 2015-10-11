using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.HabboHotel.Rooms
{
	internal sealed class RoomEvent
	{
		public string Name;
		public string Description;
		public int Category;
		public List<string> Tags;
		public string StartTime;
		public uint RoomId;

		public RoomEvent(uint mRoomId, string mName, string mDescription, int mCategory, List<string> mTags)
		{
			this.RoomId = mRoomId;
			this.Name = mName;
			this.Description = mDescription;
			this.Category = mCategory;
			this.Tags = mTags;
			this.StartTime = DateTime.Now.ToShortTimeString();
		}
		public ServerMessage Serialize(GameClient Session)
		{
            ServerMessage Message = new ServerMessage(Outgoing.RoomEvent); // Updated
			Message.AppendStringWithBreak(string.Concat(Session.GetHabbo().Id));
			Message.AppendStringWithBreak(Session.GetHabbo().Username);
			Message.AppendStringWithBreak(string.Concat(RoomId));
			Message.AppendInt32(Category);
			Message.AppendStringWithBreak(Name);
			Message.AppendStringWithBreak(Description);
			Message.AppendStringWithBreak(StartTime);
			Message.AppendInt32(Tags.Count);

			using (TimedLock.Lock(this.Tags))
			{
				foreach (string Tag in Tags)
				{
					Message.AppendStringWithBreak(Tag);
				}
			}
			return Message;
		}
        internal void SerializeTo(RoomData data, ServerMessage Message)
        {
            Message.AppendInt32(data.OwnerId);
            Message.AppendString(data.Owner);
            Message.AppendInt32(this.RoomId);
            Message.AppendInt32(this.Category);
            Message.AppendString(this.Name);
            Message.AppendString(this.Description);
            Message.AppendString(this.StartTime);
            Message.AppendInt32(this.Tags.Count);
            foreach (string str in this.Tags.ToArray())
            {
                Message.AppendString(str);
            }
        }
	}
}
