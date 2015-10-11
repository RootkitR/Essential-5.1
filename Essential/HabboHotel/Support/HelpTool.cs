using System;
using System.Collections.Generic;
using System.Data;
using Essential.Core;
using Essential.Messages;
using Essential.Storage;
namespace Essential.HabboHotel.Support
{
	internal sealed class HelpTool
	{
		public Dictionary<uint, HelpCategory> dictionary_0;
		public Dictionary<uint, HelpTopic> dictionary_1;
		public List<HelpTopic> list_0;
		public List<HelpTopic> list_1;
		public HelpTool()
		{
			this.dictionary_0 = new Dictionary<uint, HelpCategory>();
			this.dictionary_1 = new Dictionary<uint, HelpTopic>();
			this.list_0 = new List<HelpTopic>();
			this.list_1 = new List<HelpTopic>();
		}
		public void method_0(DatabaseClient class6_0)
		{
			Logging.Write("Loading Help Categories..");
			this.dictionary_0.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id, caption FROM help_subjects");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.dictionary_0.Add((uint)dataRow["Id"], new HelpCategory((uint)dataRow["Id"], (string)dataRow["caption"]));
				}
				Logging.WriteLine("completed!", ConsoleColor.Green);
			}
		}
		public HelpCategory method_1(uint uint_0)
		{
			HelpCategory result;
			if (this.dictionary_0.ContainsKey(uint_0))
			{
				result = this.dictionary_0[uint_0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void method_2()
		{
			this.dictionary_0.Clear();
		}
		public void method_3(DatabaseClient class6_0)
		{
			Logging.Write("Loading Help Topics..");
			this.dictionary_1.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id, title, body, subject, known_issue FROM help_topics");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					HelpTopic @class = new HelpTopic((uint)dataRow["Id"], (string)dataRow["title"], (string)dataRow["body"], (uint)dataRow["subject"]);
					this.dictionary_1.Add((uint)dataRow["Id"], @class);
					int num = int.Parse(dataRow["known_issue"].ToString());
					if (num == 1)
					{
						this.list_1.Add(@class);
					}
					else
					{
						if (num == 2)
						{
							this.list_0.Add(@class);
						}
					}
				}
				Logging.WriteLine("completed!", ConsoleColor.Green);
			}
		}
		public HelpTopic method_4(uint uint_0)
		{
			HelpTopic result;
			if (this.dictionary_1.ContainsKey(uint_0))
			{
				result = this.dictionary_1[uint_0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void method_5()
		{
			this.dictionary_1.Clear();
			this.list_0.Clear();
			this.list_1.Clear();
		}
		public int method_6(uint uint_0)
		{
			int num = 0;
			using (TimedLock.Lock(this.dictionary_1))
			{
				foreach (HelpTopic current in this.dictionary_1.Values)
				{
					if (current.uint_1 == uint_0)
					{
						num++;
					}
				}
			}
			return num;
		}
		public ServerMessage method_7()
		{
            ServerMessage Message = new ServerMessage(Outgoing.Quests2);
			Message.AppendInt32(this.list_0.Count);
			using (TimedLock.Lock(this.list_0))
			{
				foreach (HelpTopic current in this.list_0)
				{
					Message.AppendUInt(current.UInt32_0);
					Message.AppendStringWithBreak(current.string_0);
				}
			}
			Message.AppendInt32(this.list_1.Count);
			using (TimedLock.Lock(this.list_1))
			{
				foreach (HelpTopic current in this.list_1)
				{
					Message.AppendUInt(current.UInt32_0);
					Message.AppendStringWithBreak(current.string_0);
				}
			}
			return Message;
		}
	}
}
