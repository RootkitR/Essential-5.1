using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Achievements;
namespace Essential.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgePointLimitsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Message = new ServerMessage(Outgoing.AchievementPoints); // Update
			Message.AppendInt32(Session.GetHabbo().AchievementScore);
			Session.SendMessage(Message);

        /*    ServerMessage BadgePointLimitsComposer = new ServerMessage(890);
            BadgePointLimitsComposer.AppendInt32(Essential.GetGame().GetAchievementManager().GetAchievements().Count);
            foreach (Achievement Ach in Essential.GetGame().GetAchievementManager().GetAchievements().Values) 
            {
                BadgePointLimitsComposer.AppendString(Ach.BadgeCode.Replace("ACH_", ""));

                BadgePointLimitsComposer.AppendInt32(Ach.Levels);

                for (int i = 1; i < Ach.Levels + 1; i++)
                {
                    BadgePointLimitsComposer.AppendInt32(i);
                    BadgePointLimitsComposer.AppendInt32(Ach.ScoreBase);
                } 
            }
            Session.SendMessage(BadgePointLimitsComposer);
            */
		}
	}
}
