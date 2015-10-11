using System;
namespace Essential.HabboHotel.Users.Subscriptions
{
	internal sealed class Subscription
	{
		public string Type;

		public int StartingTime;
		public int ExpirationTime;

		public Subscription(string type, int startingTime, int expirationTime)
		{
			Type = type;

			StartingTime = startingTime;
			ExpirationTime = expirationTime;
		}

		public bool HasExpired()
		{
			return (double)this.ExpirationTime > Essential.GetUnixTimestamp();
		}

		public void AppendTime(int seconds)
		{
			if (this.ExpirationTime + seconds < 2147483647)
			{
				this.ExpirationTime += seconds;
			}
		}
	}
}
