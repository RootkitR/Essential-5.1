using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Users
{
	internal sealed class GetHabboGroupDetailsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
            bool InWindow = false;
            InWindow = Event.PopWiredBoolean();
            string OwnerName = "Rootkit";
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				GroupsManager @class = Groups.GetGroupById(num);
				if (@class != null)
				{
                    ServerMessage Message = new ServerMessage(Outgoing.GroupInfo); // Updated
					Message.AppendInt32(@class.Id);
                    Message.AppendBoolean(true);
                    Message.AppendInt32(@class.Locked == "open" ? 0 : 1);
					Message.AppendStringWithBreak(@class.Name);
                    Message.AppendStringWithBreak(@class.Description);
                    Message.AppendStringWithBreak(@class.Badge);
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
						{
                            OwnerName = dbClient.ReadString("SELECT username FROM users WHERE id = '" + @class.OwnerId + "' LIMIT 1");
                        }
                    if (@class.RoomId > 0u)
					{
                        Message.AppendUInt(@class.RoomId);
                        if (Essential.GetGame().GetRoomManager().GetRoom(@class.RoomId) != null)
						{
                            Message.AppendStringWithBreak(Essential.GetGame().GetRoomManager().GetRoom(@class.RoomId).Name);
							goto IL_15A;
						}
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							try
							{
                                DataRow dataRow_ = class2.ReadDataRow("SELECT * FROM rooms WHERE Id = " + @class.RoomId + " LIMIT 1;");
                                string string_ = Essential.GetGame().GetRoomManager().method_17(@class.RoomId, dataRow_).Name;
								Message.AppendStringWithBreak(string_);
							}
							catch
							{
								Message.AppendInt32(-1);
								Message.AppendStringWithBreak("");
							}
							goto IL_15A;
						}
					}
					Message.AppendInt32(-1);
					Message.AppendStringWithBreak("");
					IL_15A:
					bool flag = false;
					foreach (DataRow dataRow in Session.GetHabbo().dataTable_0.Rows)
					{
						if ((int)dataRow["groupid"] == @class.Id)
						{
							flag = true;
						}
					}
					if (Session.GetHabbo().list_0.Contains(@class.Id))
					{
						Message.AppendInt32(2);
					}
					else
					{
						if (flag)
						{
							Message.AppendInt32(1);
						}
						else
						{
                      
								if (@class.Members.Contains((int)Session.GetHabbo().Id))
								{
									Message.AppendInt32(1);
								}
								else
								{
									Message.AppendInt32(0);
								}
							
						}
					}
					Message.AppendInt32(@class.Members.Count);

						Message.AppendBoolean(true);
				
                    Message.AppendString(@class.Created);
                    Message.AppendBoolean((@class.OwnerId == Session.GetHabbo().Id));
                    Message.AppendBoolean(false);
                    Message.AppendString(OwnerName);
                    Message.AppendBoolean(InWindow);
                    Message.AppendBoolean(true);
                    Message.AppendInt32(0); // Pending
					Session.SendMessage(Message);
				}
			}
		}
	}
}
