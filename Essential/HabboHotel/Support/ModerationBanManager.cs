using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Storage;
namespace Essential.HabboHotel.Support
{
	internal sealed class ModerationBanManager
	{
		public List<ModerationBan> Bans;

		public ModerationBanManager()
		{
			this.Bans = new List<ModerationBan>();
		}

		public void Initialise(DatabaseClient dbClient)
		{
            Logging.Write("Loading bans..");

			this.Bans.Clear();
			
            DataTable dataTable = dbClient.ReadDataTable("SELECT bantype,value,reason,expire FROM bans WHERE expire > '" + Essential.GetUnixTimestamp() + "'");
			
            if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					ModerationBanType Type = ModerationBanType.IP;
					if ((string)dataRow["bantype"] == "user")
					{
						Type = ModerationBanType.USERNAME;
					}

					this.Bans.Add(new ModerationBan(Type, (string)dataRow["value"], (string)dataRow["reason"], (double)dataRow["expire"]));
				}

				Logging.WriteLine("completed!", ConsoleColor.Green);
			}
		}

		public void method_1(GameClient Session)
		{
			foreach (ModerationBan current in this.Bans)
			{
				if (!current.Expired)
				{
                    if (Session != null && Session.GetHabbo() != null && current.Type == ModerationBanType.IP && Session.GetConnection().String_0 == current.Variable)
					{
						throw new ModerationBanException(current.ReasonMessage);
					}
					if (Session != null && Session.GetHabbo() != null && (current.Type == ModerationBanType.USERNAME && Session.GetHabbo().Username.ToLower() == current.Variable.ToLower()))
					{
						throw new ModerationBanException(current.ReasonMessage);
					}
				}
			}
		}

		public void BanUser(GameClient Session, string string_0, double length, string reason, bool banIp)
		{
			if (!Session.GetHabbo().IsJuniori)
			{
				ModerationBanType enum4_ = ModerationBanType.USERNAME;
				string text = Session.GetHabbo().Username;
				string object_ = "user";

				double timestamp = Essential.GetUnixTimestamp() + length;

				if (banIp)
				{
					enum4_ = ModerationBanType.IP;

					if (!ServerConfiguration.IPLastBan)
                        text = Session.GetConnection().String_0;
					else
					{
						using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
						{
							text = dbClient.ReadString("SELECT ip_last FROM users WHERE Id = " + Session.GetHabbo().Id + " LIMIT 1;");
						}
					}
					object_ = "ip";
				}

				this.Bans.Add(new ModerationBan(enum4_, text, reason, timestamp));

				using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
				{
					dbClient.AddParamWithValue("rawvar", object_);
					dbClient.AddParamWithValue("var", text);
					dbClient.AddParamWithValue("reason", reason);
					dbClient.AddParamWithValue("mod", string_0);

					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO bans (bantype,value,reason,expire,added_by,added_date,appeal_state) VALUES (@rawvar,@var,@reason,'",
						timestamp,
						"',@mod,'",
						DateTime.Now.ToLongDateString(),
						"', '1')"
					}));
                    dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO banlog (bantype,value,reason,expire,added_by,added_date,appeal_state) VALUES (@rawvar,@var,@reason,'",
						timestamp,
						"',@mod,'",
						DateTime.Now.ToLongDateString(),
						"', '1')"
					}));
                    
				}

				if (banIp)
				{
					DataTable dataTable = null;

					using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
					{
						dbClient.AddParamWithValue("var", text);
						dataTable = dbClient.ReadDataTable("SELECT Id FROM users WHERE ip_last = @var");
					}

                    if (dataTable != null)
                    {
                        IEnumerator enumerator = dataTable.Rows.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                DataRow dataRow = (DataRow)enumerator.Current;
                                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                                {
                                    @class.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + (uint)dataRow["Id"] + "' LIMIT 1");
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
				}

				using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
				{
					dbClient.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + Session.GetHabbo().Id + "' LIMIT 1");
				}
				
				Session.NotifyBan(reason);
				Session.Disconnect("Banned!");
			}
		}
	}
}
