using System;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Avatar
{
	internal sealed class SaveWardrobeOutfitMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            uint num = Event.PopWiredUInt();
            string text = Event.PopFixedString();
            string text2 = Event.PopFixedString();
            if (AntiMutant.ValidateLook(text, text2) && Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                using (DatabaseClient client = Essential.GetDatabase().GetClient())
                {
                    client.AddParamWithValue("look", text);
                    client.AddParamWithValue("gender", text2.ToUpper());
                    if (client.ReadDataRow(string.Concat(new object[]
					{
						"SELECT null FROM user_wardrobe WHERE user_id = ",
						Session.GetHabbo().Id,
						" AND slot_id = ",
						num,
						" LIMIT 1"
					})) != null)
                    {
                        client.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_wardrobe SET look = @look, gender = @gender WHERE user_id = ",
							Session.GetHabbo().Id,
							" AND slot_id = ",
							num,
							" LIMIT 1;"
						}));
                    }
                    else
                    {
                        client.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_wardrobe (user_id,slot_id,look,gender) VALUES (",
							Session.GetHabbo().Id,
							",",
							num,
							",@look,@gender)"
						}));
                    }
                }
            }
		}
	}
}
