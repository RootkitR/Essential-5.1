using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Users
{
	internal sealed class GetHabboGroupBadgesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().uint_2 > 0u)
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);
				if (@class != null && Session.GetHabbo().FavouriteGroup > 0)
				{
                    GroupsManager class2 = Groups.GetGroupById(Session.GetHabbo().FavouriteGroup);
					if (class2 != null && !@class.list_17.Contains(class2))
					{
						@class.list_17.Add(class2);
                        ServerMessage Message = new ServerMessage(Outgoing.Guilds); // Updated
						Message.AppendInt32(@class.list_17.Count);
						foreach (GroupsManager current in @class.list_17)
						{
							Message.AppendInt32(current.Id);
							Message.AppendStringWithBreak(current.Badge);
						}
						@class.SendMessage(Message, null);
					}
					else
					{
						foreach (GroupsManager current2 in @class.list_17)
						{
                            if (current2 == class2 && current2.Badge != class2.Badge)
							{
                                ServerMessage Message = new ServerMessage(Outgoing.Guilds);
								Message.AppendInt32(@class.list_17.Count);
								foreach (GroupsManager current in @class.list_17)
								{
									Message.AppendInt32(current.Id);
                                    Message.AppendStringWithBreak(current.Badge);
								}
								@class.SendMessage(Message, null);
							}
						}
					}
				}
				if (@class != null && @class.list_17.Count > 0)
				{
                    ServerMessage Message = new ServerMessage(Outgoing.Guilds);
					Message.AppendInt32(@class.list_17.Count);
					foreach (GroupsManager current in @class.list_17)
					{
						Message.AppendInt32(current.Id);
                        Message.AppendStringWithBreak(current.Badge);
					}
					Session.SendMessage(Message);
				}
			}
		}
	}
}
