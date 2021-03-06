using System;
using System.Collections.Generic;
using System.Data;
using Essential.HabboHotel.Users.UserDataManagement;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.HabboHotel.Users.Badges
{
	internal sealed class BadgeComponent
	{
		private List<Badge> Badges;

		public uint UserId;

		public int BadgeCount
		{
			get
			{
				return this.Badges.Count;
			}
		}

		public int VisibleBadges
		{
			get
			{
				int num = 0;

				foreach (Badge current in this.Badges)
				{
					if (current.Slot > 0)
						num++;
				}

				return num;
			}
		}

        public List<Badge> GetBadges()
        {
            return Badges;
        }

		public BadgeComponent(uint userId, UserDataFactory userdata)
		{
			this.Badges = new List<Badge>();

			this.UserId = userId;

			DataTable dataTable_ = userdata.GetBadges();

			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
					this.Badges.Add(new Badge((string)dataRow["badge_id"], (int)dataRow["badge_slot"]));
				}
			}
		}
        public bool HasBadgeList(List<string> badges_)
        {
            foreach(string badge in badges_)
            {
                if (GetBadgeByCode(badge) == null)
                    return false;
            }
            return true;
        }
		public Badge GetBadgeByCode(string code)
		{
			foreach (Badge badge in Badges)
			{
				if (code.ToLower() == badge.Code.ToLower())
                    return badge;
			}

            return null;
		}

		public bool HasBadge(string code)
		{
			return this.GetBadgeByCode(code) != null;
		}

		public void SendBadge(GameClient session, string code, bool addToDatabase)
		{
            if (session != null && session.GetHabbo() != null)
            {
                AddBadge(code, 0, addToDatabase);
                session.SendMessage(session.GetHabbo().GetBadgeComponent().ComposeBadgeListMessage());
            }
		}

		public void AddBadge(string code, int slotId, bool addToDatabase)
		{
            if (this.HasBadge(code))
                return;

			if (addToDatabase)
			{
				using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
				{
					dbClient.AddParamWithValue("badge", code);

					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('",
						this.UserId,
						"',@badge,'",
						slotId,
						"')"
					}));
				}
			}

			this.Badges.Add(new Badge(code, slotId));
		}

		public void ResetSlot(string code, int slot)
		{
			Badge badge = this.GetBadgeByCode(code);

			if (badge != null)
				badge.Slot = slot;
		}

		public void ResetBadgeSlots()
		{
			foreach (Badge current in this.Badges)
			{
				current.Slot = 0;
			}
		}

		public void RemoveBadge(string code)
		{
			if (this.HasBadge(code))
			{
				using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
				{
					dbClient.AddParamWithValue("badge", code);
					dbClient.ExecuteQuery("DELETE FROM user_badges WHERE badge_id = @badge AND user_id = '" + this.UserId + "' LIMIT 1");
				}

				this.Badges.Remove(this.GetBadgeByCode(code));
			}
		}

		public ServerMessage ComposeBadgeListMessage()
		{
			List<Badge> list = new List<Badge>();

            ServerMessage Message = new ServerMessage(Outgoing.BadgesInventory); // Updated

			Message.AppendInt32(this.BadgeCount);

			foreach (Badge current in this.Badges)
			{
				Message.AppendUInt(Essential.GetGame().GetAchievementManager().GetBadgeId(current.Code));
				Message.AppendStringWithBreak(current.Code);

				if (current.Slot > 0)
					list.Add(current);
			}

			Message.AppendInt32(list.Count);

			foreach (Badge current in list)
			{
				Message.AppendInt32(current.Slot);
				Message.AppendStringWithBreak(current.Code);
			}

			return Message;
		}
	}
}
