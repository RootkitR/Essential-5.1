using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Achievements;
using Essential.Messages;
namespace Essential.Communication.Messages.Inventory.Achievements
{
	internal sealed class GetAchievementsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(AchievementManager.SerializeAchievements(Session));
		}
	}
}
