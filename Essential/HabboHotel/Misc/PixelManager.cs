using System;
using System.Threading;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Misc
{
	internal sealed class PixelManager
	{
		public bool KeepAlive;
		private Thread WorkerThread;
		public PixelManager()
		{
			this.KeepAlive = true;
			this.WorkerThread = new Thread(new ThreadStart(this.RewardThread));
			this.WorkerThread.Name = "Pixel Manager";
			this.WorkerThread.Priority = ThreadPriority.Lowest;
		}
		public void Initialize()
		{
			Logging.Write("Starting Reward Timer..");
			this.WorkerThread.Start();
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		private void RewardThread()
		{
			try
			{
				while (this.KeepAlive)
				{
					if (Essential.GetGame() != null && Essential.GetGame().GetClientManager() != null)
					{
						Essential.GetGame().GetClientManager().CheckPixelUpdates();
					}
					Thread.Sleep(15000);
				}
			}
			catch (ThreadAbortException)
			{
			}
		}
		public bool CanHaveReward(GameClient Session)
		{
			double num = (Essential.GetUnixTimestamp() - Session.GetHabbo().LastActivityPointsUpdate) / 60.0;
			return num >= (double)ServerConfiguration.CreditingInterval;
		}
		public void UpdateNeeded(GameClient Session)
		{
			try
			{
                if (Session.GetHabbo().InRoom)
				{
					RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.int_1 <= ServerConfiguration.SleepTimer)
					{
						double double_ = Essential.GetUnixTimestamp();
						Session.GetHabbo().LastActivityPointsUpdate = double_;
						if (ServerConfiguration.PointingAmount > 0 && (Session.GetHabbo().ActivityPoints < ServerConfiguration.PixelLimit || ServerConfiguration.PixelLimit == 0))
						{
							Session.GetHabbo().ActivityPoints += ServerConfiguration.PointingAmount;
							Session.GetHabbo().method_16(ServerConfiguration.PointingAmount);
						}
						if (ServerConfiguration.CreditingAmount > 0 && (Session.GetHabbo().GetCredits() < ServerConfiguration.CreditLimit || ServerConfiguration.CreditLimit == 0))
						{
							Session.GetHabbo().GiveCredits(ServerConfiguration.CreditingAmount, "Pixelmanager");
							if (Session.GetHabbo().IsVIP)
							{
								Session.GetHabbo().GiveCredits(ServerConfiguration.CreditingAmount, "VIP Bonus (Pixelmanager)");
							}
							Session.GetHabbo().UpdateCredits(true);
						}
						if (ServerConfiguration.PixelingAmount > 0 && (Session.GetHabbo().VipPoints < ServerConfiguration.PointLimit || ServerConfiguration.PointLimit == 0))
						{
							Session.GetHabbo().VipPoints += ServerConfiguration.PixelingAmount;
							Session.GetHabbo().UpdateVipPoints(false, true);
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
