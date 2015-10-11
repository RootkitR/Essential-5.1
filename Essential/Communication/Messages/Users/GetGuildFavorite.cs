using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Users
{
	internal sealed class GetGuildFavorite : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				Session.GetHabbo().FavouriteGroup = num;
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_stats SET groupid = ",
						num,
						" WHERE Id = ",
						Session.GetHabbo().Id,
						" LIMIT 1;"
					}));
				}
				DataTable dataTable_ = Session.GetHabbo().dataTable_0;
                string OwnerName = "";
				if (dataTable_ != null)
				{
                    GroupsManager @class = Groups.GetGroupById(Session.GetHabbo().FavouriteGroup);
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
						Message.AppendInt32(1);
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
		
						Message.AppendBoolean(false);
					
                    Message.AppendString(@class.Created);
                    Message.AppendBoolean((@class.OwnerId == Session.GetHabbo().Id));
                    Message.AppendBoolean((Session.GetHabbo().FavouriteGroup == @class.Id));
                    Message.AppendString(OwnerName);
                    Message.AppendBoolean(false);
                    Message.AppendBoolean(true);
                    Message.AppendInt32(0); // Pending
					Session.SendMessage(Message);

                    ServerMessage UpdateFaver = new ServerMessage(Outgoing.UpdateFavGuild);
                    UpdateFaver.AppendUInt(Session.GetHabbo().Id);
                    Session.SendMessage(UpdateFaver);

                    if (Session.GetHabbo().InRoom)
					{
						Room class14_ = Session.GetHabbo().CurrentRoom;
						RoomUser class3 = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
                        ServerMessage Message2 = new ServerMessage(Outgoing.SetRoomUser); // P
						Message2.AppendInt32(1);
						class3.method_14(Message2);
						class14_.SendMessage(Message2, null);
						GroupsManager class4 = Groups.GetGroupById(Session.GetHabbo().FavouriteGroup);
						if (!class14_.list_17.Contains(class4))
						{
							class14_.list_17.Add(class4);
                            ServerMessage Message3 = new ServerMessage(Outgoing.Guilds); // Updated
							Message3.AppendInt32(class14_.list_17.Count);
							foreach (GroupsManager class2 in class14_.list_17)
							{
								Message3.AppendInt32(class2.Id);
                                Message3.AppendStringWithBreak(class2.Badge);
							}
							class14_.SendMessage(Message3, null);
						}
						else
						{
							foreach (GroupsManager current in class14_.list_17)
							{
                                if (current == class4 && current.Badge != class4.Badge)
								{
                                    ServerMessage Message3 = new ServerMessage(Outgoing.Guilds); // Updated
									Message3.AppendInt32(class14_.list_17.Count);
									foreach (GroupsManager class2 in class14_.list_17)
									{
										Message3.AppendInt32(class2.Id);
                                        Message3.AppendStringWithBreak(class2.Badge);
									}
									class14_.SendMessage(Message3, null);
								}
							}
						}
					}
				}
			}
		}
	}
}
