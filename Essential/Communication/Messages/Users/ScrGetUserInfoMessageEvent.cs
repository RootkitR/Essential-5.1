using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Users
{
	internal sealed class ScrGetUserInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Message = new ServerMessage(Outgoing.SerializeClub); // Updated
            Message.AppendString("club_habbo");
            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                double expireTime = Session.GetHabbo().GetSubscriptionManager().GetSubscriptionByType("habbo_club").ExpirationTime;
                double num2 = expireTime - Essential.GetUnixTimestamp();
                int num3 = (int)Math.Ceiling((double)(num2 / 86400.0));
                int i = num3 / 0x1f;
                if (i >= 1)
                {
                    i--;
                }
                Message.AppendInt32((int)(num3 - (i * 0x1f)));
                Message.AppendInt32(2);//2
                Message.AppendInt32(i);
                Message.AppendInt32(1);
                Message.AppendBoolean(true);
                Message.AppendBoolean(true);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0x1ef);
            }
            else
            {
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendBoolean(false);
                Message.AppendBoolean(true);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
            }
			Session.SendMessage(Message);
		}
	}
}
