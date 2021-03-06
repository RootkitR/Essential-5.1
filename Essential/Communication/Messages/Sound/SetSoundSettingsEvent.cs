using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Sound
{
	internal sealed class SetSoundSettingsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num < 0)
			{
				num = 0;
			}
			else
			{
				if (num > 100)
				{
					num = 100;
				}
			}
			Session.GetHabbo().Volume = num;
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("user_id", Session.GetHabbo().Id);
				@class.AddParamWithValue("volume", num);
				@class.ExecuteQuery("UPDATE users SET volume = @volume WHERE Id = @user_id LIMIT 1;");
			}
		}
	}
}
