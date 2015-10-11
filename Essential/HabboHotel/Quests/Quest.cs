using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.HabboHotel.Quests
{
	internal sealed class Quest
	{
		private readonly uint Id;
		public string Type;
		public string Action;
		public int NeedForLevel;
		public int Level;
		public int PixelReward;
		public Quest(uint mId, string mType, string mAction, int mNeedForLevel, int mLevel, int mPixelReward)
		{
			this.Id = mId;
			this.Type = mType;
			this.Action = mAction;
			this.NeedForLevel = mNeedForLevel;
			this.Level = mLevel;
			this.PixelReward = mPixelReward;
		}
		public uint QuestId()
		{
			return this.Id;
		}
		public void Serialize(ServerMessage Message, GameClient Session, bool Single)
		{
			Message.AppendStringWithBreak(this.Type);
			if (Session.GetHabbo().CompletedQuests.Contains(this.Id))
			{
				Message.AppendInt32(this.Level);
			}
			else
			{
				Message.AppendInt32(this.Level - 1);
			}
			Message.AppendInt32(Essential.GetGame().GetQuestManager().GetHighestLevelForType(this.Type));
			if (Essential.GetGame().GetQuestManager().GetHighestLevelForType(this.Type) == this.Level && Session.GetHabbo().CompletedQuests.Contains(this.Id) && !Single)
			{
				Message.AppendInt32(0);
				Message.AppendInt32(0);
                Message.AppendBoolean(false);
				Message.AppendStringWithBreak("");
				Message.AppendStringWithBreak("");
				Message.AppendInt32(0);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
                Message.AppendStringWithBreak("");
                Message.AppendStringWithBreak("");
                Message.AppendBoolean(true);
			}
			else
			{
                Message.AppendInt32(-1);
                Message.AppendUInt(this.Id);
				//Message.AppendBoolean(false);
			//	Message.AppendUInt(this.Id);
				Message.AppendBoolean(Session.GetHabbo().CurrentQuestId == this.Id);
				Message.AppendStringWithBreak(this.Action.StartsWith("FIND") ? "FIND_STUFF" : this.Action);
				Message.AppendStringWithBreak("_2");
				Message.AppendInt32(this.PixelReward);
				Message.AppendStringWithBreak(this.Action.Replace("_", ""));
				Message.AppendInt32(Session.GetHabbo().CurrentQuestProgress);
				Message.AppendInt32(this.NeedForLevel);
				Message.AppendInt32(Essential.GetGame().GetQuestManager().GetIntValue(this.Type));
                Message.AppendStringWithBreak("set_kuurna");
                Message.AppendStringWithBreak("MAIN_CHAIN");
                Message.AppendBoolean(true);
			}
		}
	}
}
