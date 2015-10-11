using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Essential.Core;
namespace Essential.HabboHotel.Navigators
{
	internal sealed class NavigatorCache
	{
		private Task task_0;
		private bool bool_0;
		private Hashtable hashtable_0;
		public NavigatorCache()
		{
			this.bool_0 = false;
			this.hashtable_0 = new Hashtable();
            this.task_0 = new Task(new Action(this.CacheTask));
			this.task_0.Start();
		}
		private void CacheTask()
		{
			while (!this.bool_0)
			{
				try
				{
					Hashtable hashtable = new Hashtable();
                    hashtable.Add(-2, Essential.GetGame().GetNavigator().GetNavigatorMessage(null, -2).GetBytes());
					Hashtable hashtable2 = this.hashtable_0;
					this.hashtable_0 = hashtable;
					hashtable2.Clear();
				}
				catch (Exception ex)
				{
                    Logging.LogThreadException(ex.ToString(), "Navigator cache task");
				}
				Thread.Sleep(100000);
			}
		}
		internal byte[] GetCache(int int_0)
		{
			byte[] result;
			try
			{
				result = (this.hashtable_0[int_0] as byte[]);
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
